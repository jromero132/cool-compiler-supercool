using System;
using System.Collections.Generic;
using System.Linq;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;

namespace SuperCOOL.Core
{
    public class CoolMethod
    {
        public CoolMethod(CoolType contextType, string name, List<CoolType> formals, CoolType returnType, ISymbolTable symbolTable)
        {
            Type = contextType;
            Name = name;
            ParamsSignature = formals;
            ReturnType = returnType;
            SymbolTable = symbolTable;
        }

        public CoolType Type { get; }
        public ISymbolTable SymbolTable { get; set; }

        List<CoolType> ParamsSignature;
        public string Name { get; }
        public CoolType ReturnType { get; }

        public IEnumerable<SymbolInfo> Locals{ get; private set; }
        public IEnumerable<SymbolInfo> Parameters{ get; private set; }
        public void AssignParametersAndLocals()
        {
            Locals = SymbolTable.GetInSubScopes().Where(x => x.Kind == ObjectKind.Local).Select((x, i) => { x.Offset = 4 * i; return x; });
            Parameters = SymbolTable.AllDefinedObjects().Where(x => x.Kind == ObjectKind.Parameter).Select((x, i) => { x.Offset = 4 * (i + 1); return x; });
        }

        public virtual CoolType GetParam(int i)
        {
            return ParamsSignature[i];
        }

        public virtual bool EnsureParametersCount(int length)
        {
            return ParamsSignature.Count == length;
        }

        public virtual bool EnsureParameter(int index, CoolType type)
        {
            return type.IsIt(ParamsSignature[index]);
        }

        public int CountParams {get;set;}
    }

    public class NullMethod : CoolMethod
    {
        public NullMethod(string name):base(null,name,new List<CoolType>(),new NullType(),null)
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