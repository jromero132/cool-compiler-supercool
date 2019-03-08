using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCOOL.Core
{
    public class CompilationUnit
    {
        HashSet<CoolType> Types { get; set; }

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

        public bool NotCyclicalInheritance()
        {
            return true;//TODO: Verify if CoolType Tree is a DAG
        }

        public bool HasEntryPoint()
        {
            return true;//TODO: Verify if there is an entry Point;
        }
    }
}
