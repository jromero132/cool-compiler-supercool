using System;
using System.Collections.Generic;

namespace SuperCOOL.Core
{
    public class CompilationUnit
    {
        private Dictionary<CoolType, List<CoolType>> lca_table;
        private Dictionary<CoolType, int> distance;
        public HashSet<CoolType> Types { get; set; }
        Dictionary<(string type,string method),CoolMethod> Method { get; set; }

        public CoolType ObjectType => GetTypeIfDef( "object" );

        public HashSet<CoolType> Types;

        HashSet<CoolMethod> Method { get; set; }

        public bool IsTypeDef( string Name ) => this.Types.Contains( new CoolType( Name ) );
        public bool IsTypeDef(string Name)
        {
            return Types.Contains(new CoolType(Name));
        }

        public bool InheritsFrom( CoolType A, CoolType B ) => A.IsIt( B );

        public CoolType GetTypeIfDef( string Name )
        {
            this.Types.TryGetValue( new CoolType( Name ), out var ret );
            return ret;
        }


        public bool NotCyclicalInheritance()
        {
            HashSet<CoolType> hs = new HashSet<CoolType>();
            Queue<CoolType> q = new Queue<CoolType>();

            for( hs.Add( this.ObjectType ), q.Enqueue( this.ObjectType ) ; q.Count > 0 ; q.Dequeue() )
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
            if( this.lca_table == null )
                LCATable();

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

        private void LCATable()
        {
            this.lca_table = new Dictionary<CoolType, List<CoolType>>()
            {
                [ this.ObjectType ] = new List<CoolType>() { null }
            };
            this.distance = new Dictionary<CoolType, int>()
            {
                [ this.ObjectType ] = 1
            };

            Queue<CoolType> q = new Queue<CoolType>();
            for( q.Enqueue( this.ObjectType ) ; q.Count > 0 ; q.Dequeue() )
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

        internal CoolMethod GetMethodIfDef(string coolType, string method)
        {
            CoolMethod ret;
            Method.TryGetValue((coolType,method), out ret);
            return ret;
        }

        internal bool IsMethodDef(string coolType, string method)
        {
            return Method.ContainsKey((coolType, method));
        }
    }
}
