using System;
using System.Collections.Generic;

namespace SuperCOOL.Core
{
    public class CompilationUnit
    {
        Dictionary<string,CoolType> Types { get; set; }
        Dictionary<(string type,string method),CoolMethod> Method { get; set; }
        public CoolType Int => Types["Int"];
        public CoolType String => Types["String"];
        public CoolType Bool => Types["Bool"];
        public CoolType Object => Types["Object"];
        public CoolType IO => Types["IO"];

        public CompilationUnit()
        {
            Types = new Dictionary<string, CoolType>();
            Method = new Dictionary<(string type, string method), CoolMethod>();
            AddType("SelfType");
            AddType("Object");
            AddType("Int");
            AddInheritance("Int", "Object");
            AddType("String");
            AddInheritance("String", "Object");
            AddType("Bool");
            AddInheritance("Bool", "Object");
            AddType("IO");
            AddInheritance("IO", "Object");
            AddMethod("Object", "abort", new List<CoolType>(), Object);
            AddMethod("Object", "type_name", new List<CoolType>(), String);
            AddMethod("Object", "copy", new List<CoolType>(), new SelfType(Object));
            AddMethod("String", "length", new List<CoolType>(), Int);
            AddMethod("String", "concat", new List<CoolType>() {String}, String);
            AddMethod("String", "substr", new List<CoolType>() {Int,Int}, String);
            AddMethod("IO", "out_string", new List<CoolType>() {String},new SelfType(IO));
            AddMethod("IO", "out_int", new List<CoolType>() {Int}, new SelfType(IO));
            AddMethod("IO", "in_string", new List<CoolType>(), String);
            AddMethod("IO", "in_int", new List<CoolType>(), Int);
        }

        public bool IsTypeDef(string Name)
        {
            return Types.ContainsKey(Name);
        }

        public bool InheritsFrom(CoolType A, CoolType B) {
            return A.IsIt(B);
        }

        public CoolType GetTypeIfDef( string Name )
        {
            Types.TryGetValue(Name, out CoolType ret);
            return ret;
        }

        public bool NotCyclicalInheritance()
        {
            HashSet<CoolType> hs = new HashSet<CoolType>();
            Queue<CoolType> q = new Queue<CoolType>();

            for( hs.Add( Object ), q.Enqueue( Object ) ; q.Count > 0 ; q.Dequeue() )
            {
                var cur = q.Peek();
                foreach( var child in cur.Childs )
                {
                    if( hs.Contains( child ) )
                        return false;
                    hs.Add( child );
                    q.Enqueue( child );
                }
            }
            return true;
        }

        public bool HasEntryPoint()
        {
            return true;//TODO: Verify if there is an entry Point;
        }

        public CoolType GetTypeLCA( CoolType type1, CoolType type2 )
        {
            if (this.lca_table == null )
                LCATable();

            if (type1 is SelfType A && type2 is SelfType B && A.ContextType == B.ContextType)
                return A;

            if (type1 is SelfType C)
                return GetTypeLCA(C.ContextType,type2);

            if (type2 is SelfType D)
                return GetTypeLCA(type1,D.ContextType);

            int l1 = this.distance[ type1 ], l2 = this.distance[ type2 ];
            if( l1 > l2 )
            {
                var temp1 = type1;
                type1 = type2;
                type2 = temp1;

                var temp2 = l1;
                l1 = l2;
                l2 = temp2;
            }

            for( int i = (int)Math.Log( l2, 2 ) ; i >= 0 ; --i )
                if( l2 - ( 1 << i ) >= l1 )
                {
                    type2 = this.lca_table[ type2 ][ i ];
                    l2 = this.distance[ type2 ];
                }

            if( type1 == type2 )
                return type1;

            for( int i = (int)Math.Log( l1, 2 ) ; i >= 0 ; --i )
            {
                var t1 = this.lca_table[ type1 ];
                var t2 = this.lca_table[ type2 ];

                if( t1[ i ] != null && t1[ i ] != t2[ i ] )
                {
                    type1 = this.lca_table[ type1 ][ i ];
                    type2 = this.lca_table[ type2 ][ i ];
                }
            }
            return this.lca_table[ type1 ][ 0 ];
        }

        public void AddInheritance(string t1, string t2)
        {
            var type1 = GetTypeIfDef(t1);
            var type2 = GetTypeIfDef(t2);
            type1.Parent = type2;
            type2.Childs.Add(type1);
        }

        Dictionary<CoolType, List<CoolType>> lca_table;
        Dictionary<CoolType, int> distance;
        private void LCATable()
        {
            this.lca_table = new Dictionary<CoolType, List<CoolType>>()
            {
                [ Object ] = new List<CoolType>() { null }
            };
            this.distance = new Dictionary<CoolType, int>()
            {
                [ Object] = 1
            };

            Queue<CoolType> q = new Queue<CoolType>();
            for( q.Enqueue(Object) ; q.Count > 0 ; q.Dequeue() )
            {
                var now = q.Peek();
                foreach( var child in now.Childs )
                {
                    if( !this.lca_table.ContainsKey( child ) )
                    {
                        this.lca_table[ child ] = new List<CoolType> { now };
                        this.distance[ child ] = this.distance[ now ] + 1;
                        q.Enqueue( child );
                    }
                }
            }

            for( int j = 1 ; ( 1 << j ) <= this.lca_table.Count ; ++j )
            {
                foreach( var pair in this.lca_table )
                {
                    var parent = pair.Value[ j - 1 ];
                    var parents_table = parent is null ? null : this.lca_table[ parent ];
                    var next_parent = parents_table?[ j - 1 ];
                    pair.Value.Add( next_parent );
                }
            }
        }

        public CoolMethod GetMethodIfDef(string coolType, string method)
        {
            CoolMethod ret;
            Method.TryGetValue((coolType,method), out ret);
            return ret;
        }

        public bool IsMethodDef(string coolType, string method)
        {
            return Method.ContainsKey((coolType, method));
        }

        public void AddType(string coolTypeName)
        {
            Types.Add(coolTypeName,new CoolType(coolTypeName));
        }

        public void AddMethod(string type, string method,List<CoolType> formals,CoolType returnType)
        {
            Method.Add((type, method), new CoolMethod(method,formals,returnType));
        }
    }
}
