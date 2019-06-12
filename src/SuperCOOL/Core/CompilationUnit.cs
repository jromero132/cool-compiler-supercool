using SuperCOOL.ANTLR;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using SuperCOOL.Constants;
using SuperCOOL.Core.Constants;

namespace SuperCOOL.Core
{
    public class CompilationUnit
    {
        public ITypeEnvironment TypeEnvironment { get;private set; }
        public IMethodEnvironment MethodEnvironment { get; private set; }

        public CompilationUnit()
        {
            TypeEnvironment = new TypeEnvironment();
            MethodEnvironment = new MethodEnvironment();
            TypeEnvironment.AddType(Types.Object);
            TypeEnvironment.Object.SetAttributes();
            TypeEnvironment.AddType(Types.Int);
            TypeEnvironment.Int.SetAttributes();
            TypeEnvironment.AddInheritance(Types.Int, Types.Object);
            TypeEnvironment.AddType(Types.String);
            TypeEnvironment.String.SymbolTable.DefObject(Attributes.StringLength, Types.Int, ObjectKind.Atribute);
            TypeEnvironment.String.SetAttributes();
            TypeEnvironment.AddInheritance(Types.String,Types.Object);
            TypeEnvironment.AddType(Types.Bool);
            TypeEnvironment.Bool.SetAttributes();
            TypeEnvironment.AddInheritance(Types.Bool, Types.Object);
            TypeEnvironment.AddType(Types.IO);
            TypeEnvironment.IO.SetAttributes();
            TypeEnvironment.AddInheritance(Types.IO, Types.Object);
            MethodEnvironment.AddMethod(TypeEnvironment.Object, Functions.Abort, new List<CoolType>(), TypeEnvironment.Object,new SymbolTable(TypeEnvironment.Object.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.Object, Functions.Abort).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.Object, Functions.Type_Name, new List<CoolType>(), TypeEnvironment.String, new SymbolTable(TypeEnvironment.Object.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.Object, Functions.Type_Name).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.Object, Functions.Copy, new List<CoolType>(), new SelfType(TypeEnvironment.Object), new SymbolTable(TypeEnvironment.Object.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.Object, Functions.Copy).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.String, Functions.Length, new List<CoolType>(), TypeEnvironment.Int,new SymbolTable(TypeEnvironment.String.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.String, Functions.Length).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.String, Functions.Concat, new List<CoolType>() { TypeEnvironment.String }, TypeEnvironment.String, new SymbolTable(TypeEnvironment.String.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.String, Functions.Concat).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.String, Functions.Substr, new List<CoolType>() { TypeEnvironment.Int, TypeEnvironment.Int }, TypeEnvironment.String, new SymbolTable(TypeEnvironment.String.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.String, Functions.Substr).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.IO, Functions.InInt, new List<CoolType>(), TypeEnvironment.Int, new SymbolTable(TypeEnvironment.IO.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.IO, Functions.InInt).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.IO, Functions.OutInt, new List<CoolType>() { TypeEnvironment.Int }, new SelfType(TypeEnvironment.IO), new SymbolTable(TypeEnvironment.IO.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.IO, Functions.OutInt).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.IO, Functions.InString, new List<CoolType>(), TypeEnvironment.String, new SymbolTable(TypeEnvironment.IO.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.IO, Functions.InString).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.IO, Functions.OutString, new List<CoolType>() { TypeEnvironment.String }, new SelfType(TypeEnvironment.IO), new SymbolTable(TypeEnvironment.IO.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.IO, Functions.OutString).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.Object, Functions.Init, new List<CoolType>(), TypeEnvironment.Object, new SymbolTable(TypeEnvironment.Object.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.Object, Functions.Init).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.String, Functions.Init, new List<CoolType>(), TypeEnvironment.String, new SymbolTable(TypeEnvironment.String.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.String, Functions.Init).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.IO, Functions.Init, new List<CoolType>(), TypeEnvironment.IO, new SymbolTable(TypeEnvironment.IO.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.IO, Functions.Init).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.Int, Functions.Init, new List<CoolType>(), TypeEnvironment.Int, new SymbolTable(TypeEnvironment.Int.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.Int, Functions.Init).AssignParametersAndLocals();
            MethodEnvironment.AddMethod(TypeEnvironment.Bool, Functions.Init, new List<CoolType>(), TypeEnvironment.Bool, new SymbolTable(TypeEnvironment.Bool.SymbolTable));
            MethodEnvironment.GetMethod(TypeEnvironment.Bool, Functions.Init).AssignParametersAndLocals();
        }

        public bool HasEntryPoint()
        {
            if (!TypeEnvironment.GetTypeDefinition("Main", null, out var Main)) return false;
            if (!MethodEnvironment.GetMethodIfDef(Main, "main", out var main)) return false;
            return main.EnsureParametersCount(0);
        }

        public bool NotCyclicalInheritance()
        {
            HashSet<CoolType> hs = new HashSet<CoolType>();
            Queue<CoolType> q = new Queue<CoolType>();

            for (hs.Add(TypeEnvironment.Object), q.Enqueue(TypeEnvironment.Object); q.Count > 0; q.Dequeue())
            {
                var cur = q.Peek();
                foreach (var child in cur.Childs)
                {
                    if (hs.Contains(child))
                        return false;
                    hs.Add(child);
                    q.Enqueue(child);
                }
            }
            return true;
        }

    }
}
