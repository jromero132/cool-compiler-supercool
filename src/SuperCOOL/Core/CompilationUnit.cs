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

        public SemanticCheckResult BuildTypeEnvironment(SuperCoolASTGeneratorVisitor visitor)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            Dictionary<string, CoolType> Types=new Dictionary<string, CoolType>();
            Dictionary<CoolType, List<CoolMethod>> MethodEnvironment=new Dictionary<CoolType, List<CoolMethod>>();
            TypeEnvironment = new TypeEnvironment(Types, MethodEnvironment);

            AddType(Types,"Object");
            AddType(Types, "Int");
            AddInheritance(Types,"Int", "Object");
            AddType(Types,"String");
            AddInheritance(Types,"String", "Object");
            AddType(Types,"Bool");
            AddInheritance(Types,"Bool", "Object");
            AddType(Types, "IO");
            AddInheritance(Types, "IO", "Object");
            AddMethod(MethodEnvironment,TypeEnvironment.Object, "abort", new List<CoolType>(), TypeEnvironment.Object);
            AddMethod(MethodEnvironment,TypeEnvironment.Object, "type_name", new List<CoolType>(), TypeEnvironment.String);
            AddMethod(MethodEnvironment,TypeEnvironment.Object, "copy", new List<CoolType>(), new SelfType(TypeEnvironment.Object));
            AddMethod(MethodEnvironment,TypeEnvironment.String, "length", new List<CoolType>(), Types["Int"]);
            AddMethod(MethodEnvironment,TypeEnvironment.String, "concat", new List<CoolType>() { TypeEnvironment.String }, TypeEnvironment.String);
            AddMethod(MethodEnvironment,TypeEnvironment.String, "substr", new List<CoolType>() { TypeEnvironment.Int, TypeEnvironment.Int }, TypeEnvironment.String);
            AddMethod(MethodEnvironment,TypeEnvironment.IO, "out_string", new List<CoolType>() { TypeEnvironment.String }, new SelfType(TypeEnvironment.IO));
            AddMethod(MethodEnvironment,TypeEnvironment.IO, "out_int", new List<CoolType>() { TypeEnvironment.Int }, new SelfType(TypeEnvironment.IO));
            AddMethod(MethodEnvironment,TypeEnvironment.IO, "in_string", new List<CoolType>(), TypeEnvironment.String);
            AddMethod(MethodEnvironment,TypeEnvironment.IO, "in_int", new List<CoolType>(), TypeEnvironment.Int);

            //Creating All Types
            foreach (var type in visitor.Types)
            {
                var exist = Types.ContainsKey(type.type);
                result.Ensure(!exist, new Error($"Multiple Definitions for class {type.type}",ErrorKind.TypeError));
                if (!exist)
                    Types.Add(type.type, new CoolType(type.type));
            }

            //Inheritance
            foreach (var type in visitor.Types)
            {
                var parent = type.parent ?? "Object";
                var exist = Types.ContainsKey(parent);
                result.Ensure(exist,new Error( $"Missing declaration for type {type.parent}.",ErrorKind.TypeError));
                if (exist)
                    AddInheritance(Types,type.type, parent);
            }

            //Not Repeated Method Definitions
            foreach (var method in visitor.Functions)
            {
                TypeEnvironment.GetTypeDefinition(method.type,out var type);
                var def = TypeEnvironment.GetMethod(type, method.method, out var m);
                result.Ensure(!def,new Error($"Not allowed multyple methods with the same name on type {type}.",ErrorKind.MethodError));
                if (!def)
                {
                    TypeEnvironment.GetTypeDefinition(method.returnType,out var ret);
                    AddMethod(MethodEnvironment,type, method.method, method.asrgTypes.Select(x => Types[x]).ToList(),ret);
                }
            }

            result.Ensure(NotCyclicalInheritance(),new Error("Detected Cyclical Inheritance",ErrorKind.SemanticError));
            result.Ensure(HasEntryPoint(),new Error("No Entry Point Detected",ErrorKind.SemanticError));

            return result;
        }

        private void AddType(Dictionary<string, CoolType> Types,string coolTypeName)
        {
            Types.Add(coolTypeName, new CoolType(coolTypeName));
        }
        private void AddMethod(Dictionary<CoolType, List<CoolMethod>> MethodEnvironment,CoolType type, string method, List<CoolType> formals, CoolType returnType)
        {
            if(!MethodEnvironment.ContainsKey(type))
                MethodEnvironment[type] = new List<CoolMethod>();
            MethodEnvironment[type].Add(new CoolMethod(method, formals, returnType));
        }
        private void AddInheritance(Dictionary<string, CoolType> Types,string t1, string t2)
        {
            var type1 = Types[t1];
            var type2 = Types[t2];
            type1.Parent = type2;
            type2.Childs.Add(type1);
        }
        private bool NotCyclicalInheritance()
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
        private bool HasEntryPoint()
        {
            if (!TypeEnvironment.GetTypeDefinition("Main", out var Main)) return false;
            if (!TypeEnvironment.GetMethod(Main, "main",out var main)) return false;
            return main.Params.Count == 0 && main.ReturnType == TypeEnvironment.IO && Main.Parent == TypeEnvironment.Object;
        }
    }
}
