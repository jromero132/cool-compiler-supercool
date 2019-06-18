using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.Constants;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck.AST;

namespace SuperCOOL.SemanticCheck
{
    public class SuperCoolTypeDefsVisitor : SuperCoolASTVisitor<SemanticCheckResult>
    {
        private CompilationUnit CompilationUnit { get; set; }
        public SuperCoolTypeDefsVisitor(CompilationUnit compilationUnit)
        {
            this.CompilationUnit = compilationUnit;
        }

        public override SemanticCheckResult VisitProgram(ASTProgramNode Program)
        {
            //Creating All Types
            foreach (var type in Program.Clases)
            {
                Program.SemanticCheckResult.Ensure(!Types.IsSelfType(type.TypeName),
                    new Lazy<Error>(()=>new Error($"Not Allowed {type.TypeName}", ErrorKind.SemanticError,type.Type.Line,type.Type.Column)));
                
                var exist = CompilationUnit.TypeEnvironment.GetTypeDefinition(type.TypeName,Program.SymbolTable,out var _);
                Program.SemanticCheckResult.Ensure(!exist, 
                    new Lazy<Error>(()=>new Error($"Multiple Definitions for class {type.TypeName}", ErrorKind.TypeError,type.Type.Line,type.Type.Column)));
                if (!exist && !Types.IsSelfType(type.TypeName))
                    CompilationUnit.TypeEnvironment.AddType(type.SymbolTable);
            }

            //Inheritance
            foreach (var type in Program.Clases)
                Program.SemanticCheckResult.Ensure(type.Accept(this));

            Program.SemanticCheckResult.Ensure(CompilationUnit.NotCyclicalInheritance(), 
                new Lazy<Error>(()=>new Error("Detected Cyclical Inheritance", ErrorKind.SemanticError)));

            return Program.SemanticCheckResult;
        }

        public override SemanticCheckResult VisitClass(ASTClassNode Class)
        {
            var exist = CompilationUnit.TypeEnvironment.GetTypeDefinition(Class.ParentTypeName,Class.SymbolTable,out var _);
            Class.SemanticCheckResult.Ensure(exist, new Lazy<Error>(()=>new Error($"Missing declaration for type {Class.ParentTypeName}.", ErrorKind.TypeError,Class.ParentType.Line,Class.ParentType.Column)));
            Class.SemanticCheckResult.Ensure(!Types.IsStringBoolOrInt(Class.TypeName), new Lazy<Error>(()=>new Error($"Not allowed to inerit from String Int or Bool.", ErrorKind.SemanticError,Class.ParentType.Line,Class.ParentType.Column)));

            if (exist && !Types.IsStringBoolOrInt(Class.TypeName))
                CompilationUnit.TypeEnvironment.AddInheritance(Class.TypeName, Class.ParentTypeName);
            Class.SemanticCheckResult.Ensure(!Types.IsSelfType(Class.ParentTypeName),
                    new Lazy<Error>(() => new Error($"Not Allowed {Class.ParentTypeName}", ErrorKind.SemanticError, Class.ParentType.Line, Class.ParentType.Column)));
            return Class.SemanticCheckResult;
        }

    }
}
