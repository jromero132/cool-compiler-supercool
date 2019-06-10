using SuperCOOL.ANTLR;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using SuperCOOL.Constants;

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
            TypeEnvironment.AddType(Types.Int);
            TypeEnvironment.AddInheritance(Types.Int, Types.Object);
            TypeEnvironment.AddType(Types.String);
            TypeEnvironment.AddInheritance(Types.String,Types.Object);
            TypeEnvironment.AddType(Types.Bool);
            TypeEnvironment.AddInheritance(Types.Bool, Types.Object);
            TypeEnvironment.AddType(Types.IO);
            TypeEnvironment.AddInheritance(Types.IO, Types.Object);
            MethodEnvironment.AddMethod(TypeEnvironment.Object, Functions.Abort, new List<CoolType>(), TypeEnvironment.Object);
            MethodEnvironment.AddMethod(TypeEnvironment.Object, Functions.Type_Name, new List<CoolType>(), TypeEnvironment.String);
            MethodEnvironment.AddMethod(TypeEnvironment.Object, Functions.Copy, new List<CoolType>(), new SelfType(TypeEnvironment.Object));
            MethodEnvironment.AddMethod(TypeEnvironment.String, Functions.Length, new List<CoolType>(), TypeEnvironment.Int);
            MethodEnvironment.AddMethod(TypeEnvironment.String, Functions.Concat, new List<CoolType>() { TypeEnvironment.String }, TypeEnvironment.String);
            MethodEnvironment.AddMethod(TypeEnvironment.String, Functions.Substr, new List<CoolType>() { TypeEnvironment.Int, TypeEnvironment.Int }, TypeEnvironment.String);
            MethodEnvironment.AddMethod(TypeEnvironment.IO, Functions.InInt, new List<CoolType>(), TypeEnvironment.Int);
            MethodEnvironment.AddMethod(TypeEnvironment.IO, Functions.OutInt, new List<CoolType>() { TypeEnvironment.Int }, new SelfType(TypeEnvironment.IO));
            MethodEnvironment.AddMethod(TypeEnvironment.IO, Functions.InString, new List<CoolType>(), TypeEnvironment.String);
            MethodEnvironment.AddMethod(TypeEnvironment.IO, Functions.OutString, new List<CoolType>() { TypeEnvironment.String }, new SelfType(TypeEnvironment.IO));
            MethodEnvironment.AddMethod(TypeEnvironment.Object, Functions.Init, new List<CoolType>(), new SelfType(TypeEnvironment.Object));
            MethodEnvironment.AddMethod(TypeEnvironment.String, Functions.Init, new List<CoolType>(), new SelfType(TypeEnvironment.String));
            MethodEnvironment.AddMethod(TypeEnvironment.IO, Functions.Init, new List<CoolType>(), new SelfType(TypeEnvironment.IO));
        }

        public bool HasEntryPoint()
        {
            if (!TypeEnvironment.GetTypeDefinition("Main", null, out var Main)) return false;
            if (!MethodEnvironment.GetMethod(Main, "main", out var main)) return false;
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
