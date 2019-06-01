using Antlr4.Runtime.Misc;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck.AST;
using System;
using System.Linq;

namespace SuperCOOL.SemanticCheck
{
    public class SuperCoolClassDefSemanticCheckVisitor : ISuperCoolASTVisitor<SemanticCheckResult>
    {
        public CompilationUnit CompilationUnit { get; }

        public SemanticCheckResult Visit(ASTNode Node)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitAdd(ASTAddNode Add)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitAssignment(ASTAssingmentNode Assigment)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitAtribute(ASTAtributeNode Atribute)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitBlock(ASTBlockNode Block)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitBoolConstant(ASTBoolConstantNode BoolConstant)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitBoolNot(ASTBoolNotNode BoolNot)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitCase(ASTCaseNode Case)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitClass(ASTClassNode Class)
        {
            var result = new SemanticCheckResult();
            if (CompilationUnit.IsTypeDef(Class.ParentTypeName))
            {
                var t = CompilationUnit.GetTypeIfDef(Class.TypeName);
                t.Parent = CompilationUnit.GetTypeIfDef(Class.ParentTypeName);
                result.Type = t;
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
            }

            foreach (var item in Class.Methods)
                CompilationUnit.Method.Add( new CoolMethod(item.Name, item.Formals.Select(x=>CompilationUnit.GetTypeIfDef(x.Type)).ToList(), CompilationUnit.GetTypeIfDef(item.ReturnType)));

            return result;
        }

        public SemanticCheckResult VisitDivision(ASTDivideNode Division)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitEqual(ASTEqualNode Equal)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitExpression(ASTExpressionNode Expression)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitFormal(ASTFormalNode Formal)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitId(ASTIdNode Id)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitIf(ASTIfNode If)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitIntConstant(ASTIntConstantNode Int)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitIsvoid(ASTIsVoidNode IsVoid)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitLessEqual(ASTLessEqualNode LessEqual)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitLessThan(ASTLessThanNode LessThan)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitLetIn(ASTLetInNode LetIn)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitMethod(ASTMethodNode Method)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitMinus(ASTMinusNode Minus)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitMultiply(ASTMultiplyNode Multiply)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitNegative(ASTNegativeNode Negative)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitNew(ASTNewNode context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitProgram(ASTProgramNode Program)
        {
            var result = new SemanticCheckResult();
            result.Correct = true;

            foreach (var item in Program.Clases)
            {
                if (!CompilationUnit.IsTypeDef(item.TypeName))
                {
                    CompilationUnit.Types.Add(new CoolType(item.TypeName));
                    result.Correct &= true;
                }
                else
                {
                    result.Correct &= false;
                }
            }

            foreach (var item in Program.Clases)
            {
                var r=item.Accept(this);
                result.Correct &= r.Correct;
            }

            result.Correct &= CompilationUnit.HasEntryPoint();
            result.Correct &= CompilationUnit.NotCyclicalInheritance();
            return result;
        }

        public SemanticCheckResult VisitStringConstant(ASTStringConstantNode StringConstant)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitWhile(ASTWhileNode While)
        {
            throw new NotImplementedException();
        }
    }
}
