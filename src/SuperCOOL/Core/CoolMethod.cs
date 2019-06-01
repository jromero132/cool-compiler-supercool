using System.Collections.Generic;
using SuperCOOL.SemanticCheck.AST;

namespace SuperCOOL.Core
{
    public class CoolMethod
    {
        public CoolMethod(string name, List<CoolType> formals, CoolType returnType)
        {
            Name = name;
            Params = formals;
            ReturnType = returnType;
        }

        public string Name { get; }
        public List<CoolType> Params { get; }
        public CoolType ReturnType { get; }
    }
}