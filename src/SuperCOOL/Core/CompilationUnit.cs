using SuperCOOL.ANTLR;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;
using System;
using System.Collections.Generic;
using System.Linq;

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
            TypeEnvironment.AddType("Object");
            TypeEnvironment.AddType("Int");
            TypeEnvironment.AddInheritance("Int", "Object");
            TypeEnvironment.AddType("String");
            TypeEnvironment.AddInheritance("String", "Object");
            TypeEnvironment.AddType("Bool");
            TypeEnvironment.AddInheritance("Bool", "Object");
            TypeEnvironment.AddType("IO");
            TypeEnvironment.AddInheritance("IO", "Object");
            MethodEnvironment.AddMethod(TypeEnvironment.Object, "abort", new List<CoolType>(), TypeEnvironment.Object);
            MethodEnvironment.AddMethod(TypeEnvironment.Object, "type_name", new List<CoolType>(), TypeEnvironment.String);
            MethodEnvironment.AddMethod(TypeEnvironment.Object, "copy", new List<CoolType>(), new SelfType(TypeEnvironment.Object));
            MethodEnvironment.AddMethod(TypeEnvironment.String, "length", new List<CoolType>(), TypeEnvironment.Int);
            MethodEnvironment.AddMethod(TypeEnvironment.String, "concat", new List<CoolType>() { TypeEnvironment.String }, TypeEnvironment.String);
            MethodEnvironment.AddMethod(TypeEnvironment.String, "substr", new List<CoolType>() { TypeEnvironment.Int, TypeEnvironment.Int }, TypeEnvironment.String);
            MethodEnvironment.AddMethod(TypeEnvironment.IO, "out_string", new List<CoolType>() { TypeEnvironment.String }, new SelfType(TypeEnvironment.IO));
            MethodEnvironment.AddMethod(TypeEnvironment.IO, "out_int", new List<CoolType>() { TypeEnvironment.Int }, new SelfType(TypeEnvironment.IO));
            MethodEnvironment.AddMethod(TypeEnvironment.IO, "in_string", new List<CoolType>(), TypeEnvironment.String);
            MethodEnvironment.AddMethod(TypeEnvironment.IO, "in_int", new List<CoolType>(), TypeEnvironment.Int);
        }

        public bool HasEntryPoint()
        {
            if (!TypeEnvironment.GetTypeDefinition("Main", null, out var Main)) return false;
            if (!MethodEnvironment.GetMethod(Main, "main", out var main)) return false;
            return main.EnsureParametersCount(0) && main.ReturnType == TypeEnvironment.Object && Main.Parent == TypeEnvironment.IO;
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
