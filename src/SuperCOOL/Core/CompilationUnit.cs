using System;
using System.Collections.Generic;

namespace SuperCOOL.Core
{
    public class CompilationUnit
    {
        HashSet<CoolType> Types { get; set; }

        HashSet<CoolMethod> Method { get; set; }

        public bool IsTypeDef(string Name)
        {
            return Types.Contains(new CoolType(Name));
        }

        public bool InheritsFrom(CoolType A, CoolType B)
        {
            return A.IsIt(B);
        }

        public CoolType GetTypeIfDef(string Name)
        {
            CoolType ret;
            Types.TryGetValue(new CoolType(Name), out ret);
            return ret;
        }

        public bool IsOK()
        {
            return true;//TODO: Verify Types NotCyclicalInheritance and Entry Point not inheritance from bool int string ...all this semantics goes here
        }
        private bool NotCyclicalInheritance()
        {
            return true;//TODO: Verify if CoolType Tree is a DAG
        }

        private bool HasEntryPoint()
        {
            return true;//TODO: Verify if there is an entry Point;
        }

        internal CoolType GetTypeLCA(CoolType type1, CoolType type2)
        {
            throw new NotImplementedException();//TODO: MAKE LCA
        }
    }
}
