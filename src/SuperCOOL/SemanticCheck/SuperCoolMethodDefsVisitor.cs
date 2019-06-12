using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck.AST;

namespace SuperCOOL.SemanticCheck
{
    public class SuperCoolMethodDefsVisitor:SuperCoolASTVisitor<SemanticCheckResult>
    {
        CompilationUnit CompilationUnit { get; set; }
        public SuperCoolMethodDefsVisitor(CompilationUnit compilationUnit)
        {
            this.CompilationUnit = compilationUnit;
        }
        public override SemanticCheckResult VisitProgram(ASTProgramNode Program)
        {
            foreach (var item in Program.Clases)
                Program.SemanticCheckResult.Ensure(item.Accept(this));
            Program.SemanticCheckResult.Ensure(CompilationUnit.HasEntryPoint(), new Lazy<Error>(()=>new Error("No Entry Point Detected", ErrorKind.SemanticError)));
            return Program.SemanticCheckResult;
        }

        public override SemanticCheckResult VisitClass(ASTClassNode Class)
        {
            foreach (var item in Class.Methods)
                Class.SemanticCheckResult.Ensure(item.Accept(this));
            return Class.SemanticCheckResult;
        }

        public override SemanticCheckResult VisitMethod(ASTMethodNode Method)
        {
            var result = Method.SemanticCheckResult;
            var type=CompilationUnit.TypeEnvironment.GetContextType(Method.SymbolTable);
            var def = CompilationUnit.MethodEnvironment.GetMethodOnIt(type,Method.Name, out var _);
            result.Ensure(!def,
                new Lazy<Error>(()=>new Error($"Multiple declaration of method {Method.Name}  on type {CompilationUnit.TypeEnvironment.GetContextType(Method.SymbolTable)}.", ErrorKind.MethodError)));
            if (!def)
            {
                var defAncestor=CompilationUnit.MethodEnvironment.GetMethodIfDef(type, Method.Name, out var m2);
                if (defAncestor)
                {
                    var samesignature = Method.Name == m2.Name && Method.ReturnType == m2.ReturnType.Name && m2.EnsureParametersCount(Method.Formals.Count);
                    if(samesignature)
                    for (int i = 0; i < Method.Formals.Count; i++)
                        samesignature&=Method.Formals[i].type.Text == m2.GetParam(i).Name;
                    result.Ensure(samesignature, 
                        new Lazy<Error>(()=>new Error($@"Method {Method.Name} on type {type.Name} has diferent signature that a method 
                                    with the same name defined on an ancestor of {type.Name}.To override methods must have the same signature.",
                                    ErrorKind.MethodError,Method.Method.Line,Method.Method.Column)));
                }

                var defformals = true;
                List<CoolType> formalTypes = new List<CoolType>();
                foreach (var item in Method.Formals)
                {
                    var defformal = CompilationUnit.TypeEnvironment.GetTypeDefinition(item.type.Text, Method.SymbolTable, out var ftype);
                    Method.SemanticCheckResult.Ensure(defformal,
                        new Lazy<Error>(()=>new Error($"Mising declaration for type {item.type}.", ErrorKind.TypeError, item.type.Line, item.type.Column)));
                    defformals &= defformal;
                    formalTypes.Add(ftype);
                }
                var defreturn=CompilationUnit.TypeEnvironment.GetTypeDefinition(Method.ReturnType,Method.SymbolTable,out var ret);
                result.Ensure(defreturn, new Lazy<Error>(()=>new Error($"Missing declaration for type {Method.ReturnType}.", ErrorKind.TypeError, Method.Return.Line, Method.Return.Column)));
                if (defreturn && defformals)
                {
                    CompilationUnit.MethodEnvironment.AddMethod(type, Method.Name,formalTypes, ret,Method.SymbolTable);
                    var definedMethod=CompilationUnit.MethodEnvironment.GetMethod(type, Method.Name);
                    definedMethod.AssignParametersAndLocals();
                }
            }

            return result;
        }

      
    }
}
