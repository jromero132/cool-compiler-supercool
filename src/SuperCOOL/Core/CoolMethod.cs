using System;
using System.Collections.Generic;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;

namespace SuperCOOL.Core
{
    public class CoolMethod //TODO need to have the type where is defined
    {
        public CoolMethod(string name, List<CoolType> formals, CoolType returnType)
        {
            Name = name;
            Params = formals;
            ReturnType = returnType;
        }

        List<CoolType> Params;
        public string Name { get; }
        public CoolType ReturnType { get; }

        public virtual CoolType GetParam(int i)
        {
            return Params[i];
        }

        public virtual bool EnsureParametersCount(int length)
        {
            return Params.Count == length;
        }

        public virtual bool EnsureParameter(int index, CoolType type)
        {
            return type.IsIt(Params[index]);
        }

        public int CountParams {get;set;}
    }

    public class NullMethod : CoolMethod
    {
        public NullMethod(string name):base(name,new List<CoolType>(),new NullType())
        {
        }
        public override bool EnsureParameter(int index, CoolType type)
        {
            return true;
        }

        public override bool EnsureParametersCount(int length)
        {
            return true;
        }

        public override CoolType GetParam(int i)
        {
            return new NullType();
        }
    }

}