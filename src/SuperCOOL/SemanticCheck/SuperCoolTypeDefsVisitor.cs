using System;
using System.Collections.Generic;
using System.Text;
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
                var exist = CompilationUnit.TypeEnvironment.GetTypeDefinition(type.TypeName,Program.SymbolTable,out var _);
                Program.SemanticCheckResult.Ensure(!exist, new Error($"Multiple Definitions for class {type.TypeName}", ErrorKind.TypeError,type.Type.Line,type.Type.Column));
                if (!exist)
                    CompilationUnit.TypeEnvironment.AddType(type.TypeName);
            }

            //Inheritance
            foreach (var type in Program.Clases)
                Program.SemanticCheckResult.Ensure(type.Accept(this));

            Program.SemanticCheckResult.Ensure(CompilationUnit.NotCyclicalInheritance(), new Error("Detected Cyclical Inheritance", ErrorKind.SemanticError));

            return Program.SemanticCheckResult;
        }

        public override SemanticCheckResult VisitClass(ASTClassNode Class)
        {
            var exist = CompilationUnit.TypeEnvironment.GetTypeDefinition(Class.ParentTypeName,Class.SymbolTable,out var _);
            Class.SemanticCheckResult.Ensure(exist, new Error($"Missing declaration for type {Class.ParentTypeName}.", ErrorKind.TypeError,Class.ParentType?.Line??0,Class.ParentType?.Column??0));
            if (exist)
                CompilationUnit.TypeEnvironment.AddInheritance(Class.TypeName, Class.ParentTypeName);
            return Class.SemanticCheckResult;
        }

    }
}
