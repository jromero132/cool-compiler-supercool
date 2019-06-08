using SuperCOOL.SemanticCheck;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.Core
{
    public class TypeEnvironment:ITypeEnvironment
    {
        Dictionary<string, CoolType> Types { get; set; }
        public CoolType Int => Types["Int"];
        public CoolType String => Types["String"];
        public CoolType Bool => Types["Bool"];
        public CoolType Object => Types["Object"];
        public CoolType IO => Types["IO"];

        public TypeEnvironment()
        {
            this.Types = new Dictionary<string, CoolType>();
        }

        public bool GetTypeDefinition(string typeName, ISymbolTable symbolTable,out CoolType coolType)
        {
            if (typeName.IsSelfType())
            {
                coolType = SelfType(symbolTable);
                return true;
            }
            return Types.TryGetValue(typeName, out coolType);
        }

        public bool GetTypeForObject(ISymbolTable symbolTable, string nameObject,out CoolType coolType)
        {
            var result=symbolTable.IsDefObject(nameObject, out var type);
            if (!result)
            {
                coolType = new NullType();
                return false;
            }
            GetTypeDefinition(type, symbolTable, out coolType);
            coolType= coolType ?? new NullType();
            return true;
        }

        public CoolType GetContextType(ISymbolTable symbolTable)
        {
            GetTypeForObject(symbolTable,"_self",out var context);
            return context;
        }

        public CoolType SelfType(ISymbolTable symbolTable)
        {
            return new SelfType(GetContextType(symbolTable));
        }

        public bool InheritsFrom(CoolType A, CoolType B)
        {
            return A.IsIt(B);
        }

        public void AddType(ISymbolTable symbolTable)
        {
            symbolTable.IsDefObject("_self", out var coolTypeName);
            Types.Add(coolTypeName, new CoolType(coolTypeName,symbolTable));
        }

        public void AddType(string coolTypeName)
        {
            Types.Add(coolTypeName, new CoolType(coolTypeName));
        }

        public void AddInheritance(string t1, string t2)
        {
            var type1 = Types[t1];
            var type2 = Types[t2];
            type1.Parent = type2;
            type2.Childs.Add(type1);
            type1.SymbolTable.InheritsFrom(type2.SymbolTable);
        }

        public CoolType GetTypeLCA(CoolType type1, CoolType type2)
        {
            if (this.lca_table == null)
                LCATable();

            if (type1 is NullType) return type2;
            if (type2 is NullType) return type1;


            if (type1 is SelfType A && type2 is SelfType B && A.ContextType == B.ContextType)
                return A;

            if (type1 is SelfType C)
                return GetTypeLCA(C.ContextType, type2);

            if (type2 is SelfType D)
                return GetTypeLCA(type1, D.ContextType);

            int l1 = this.distance[type1], l2 = this.distance[type2];
            if (l1 > l2)
            {
                var temp1 = type1;
                type1 = type2;
                type2 = temp1;

                var temp2 = l1;
                l1 = l2;
                l2 = temp2;
            }

            for (int i = (int)Math.Log(l2, 2); i >= 0; --i)
                if (l2 - (1 << i) >= l1)
                {
                    type2 = this.lca_table[type2][i];
                    l2 = this.distance[type2];
                }

            if (type1 == type2)
                return type1;

            for (int i = (int)Math.Log(l1, 2); i >= 0; --i)
            {
                var t1 = this.lca_table[type1];
                var t2 = this.lca_table[type2];

                if (t1[i] != null && t1[i] != t2[i])
                {
                    type1 = this.lca_table[type1][i];
                    type2 = this.lca_table[type2][i];
                }
            }
            return this.lca_table[type1][0];
        }

        Dictionary<CoolType, List<CoolType>> lca_table;
        Dictionary<CoolType, int> distance;
        private void LCATable()
        {
            this.lca_table = new Dictionary<CoolType, List<CoolType>>()
            {
                [Object] = new List<CoolType>() { null }
            };
            this.distance = new Dictionary<CoolType, int>()
            {
                [Object] = 1
            };

            Queue<CoolType> q = new Queue<CoolType>();
            for (q.Enqueue(Object); q.Count > 0; q.Dequeue())
            {
                var now = q.Peek();
                foreach (var child in now.Childs)
                {
                    if (!this.lca_table.ContainsKey(child))
                    {
                        this.lca_table[child] = new List<CoolType> { now };
                        this.distance[child] = this.distance[now] + 1;
                        q.Enqueue(child);
                    }
                }
            }

            for (int j = 1; (1 << j) <= this.lca_table.Count; ++j)
            {
                foreach (var pair in this.lca_table)
                {
                    var parent = pair.Value[j - 1];
                    var parents_table = parent is null ? null : this.lca_table[parent];
                    var next_parent = parents_table?[j - 1];
                    pair.Value.Add(next_parent);
                }
            }
        }

    }

    public interface ITypeEnvironment
    {
        void AddType(ISymbolTable symbolTable);
        void AddType(string coolTypeName);
        void AddInheritance(string st1, string st2);
        bool GetTypeForObject(ISymbolTable symbolTable, string nameObjectout,out CoolType coolType);
        CoolType GetContextType(ISymbolTable symbolTable);
        bool GetTypeDefinition(string typeName,ISymbolTable symbolTable,out CoolType coolType);
        bool InheritsFrom(CoolType A, CoolType B);
        CoolType GetTypeLCA(CoolType type1, CoolType type2);
        CoolType Int { get; }
        CoolType String { get; }
        CoolType Bool { get; }
        CoolType Object { get; }
        CoolType IO { get; }
        CoolType SelfType(ISymbolTable symbolTable);
    }
}
