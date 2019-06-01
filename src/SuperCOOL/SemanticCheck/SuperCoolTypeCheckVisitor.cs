using Antlr4.Runtime.Misc;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck.AST;
using System;

namespace SuperCOOL.SemanticCheck
{
    public class SuperCoolTypeCheckVisitor : ISuperCoolASTVisitor<SemanticCheckResult>
    {
        public CompilationUnit CompilationUnit { get; }

        public SemanticCheckResult VisitAdd(ASTAddNode Add)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            result.Type = CompilationUnit.GetTypeIfDef("Int");

            var left = Add.Left.Accept(this);
            var right = Add.Right.Accept(this);

            result.Correct = left.Correct && right.Correct && left.Type == result.Type && right.Type == result.Type && left.Type==CompilationUnit.Int;
            return result;
        }

        public SemanticCheckResult VisitAssignment(ASTAssingmentNode Assingment)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            var expresult = Assingment.Expresion.Accept(this);
            var idResult = Assingment.Id.Accept(this);

            result.Correct = expresult.Correct && idResult.Correct && expresult.Type.IsIt(idResult.Type);
            result.Type = expresult.Type;
            return result;
        }

        public SemanticCheckResult VisitBlock(ASTBlockNode Block)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            result.Correct = true;

            foreach (var item in Block.Expresions)
            {
                var sem = item.Accept(this);
                result.Type =sem.Type;
                result.Correct &= sem.Correct;
            }
            return result;
        }

        public SemanticCheckResult VisitBoolNot(ASTBoolNotNode BoolNode)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            var exp = BoolNode.Accept(this);
            var boolType= CompilationUnit.Bool;
            result.Correct = exp.Type == boolType;
            result.Type = boolType;
            return result;
        }

        public SemanticCheckResult VisitCase(ASTCaseNode Case)
        {
            var result = new SemanticCheckResult();

            var expresionCaseResult = Case.ExpressionCase.Accept(this);
            result.Correct = expresionCaseResult.Correct;

            var branchresults = new SemanticCheckResult[Case.Cases.Length];
            for (int i = 0; i < Case.Cases.Length; i++)
            {
                branchresults[i] = Case.Cases[i].Branch.Accept(this);
                result.Correct &= branchresults[i].Correct && branchresults[i].Type.IsIt(CompilationUnit.GetTypeIfDef(Case.Cases[i].Type));
            }

            result.Type = branchresults[0].Type;
            for (int i = 1; i < Case.Cases.Length; i++)
                result.Type = CompilationUnit.GetTypeLCA(result.Type,branchresults[i].Type);

            return result;
        }

        public SemanticCheckResult VisitClass(ASTClassNode Class)
        {
            SemanticCheckResult semanticCheckResult = new SemanticCheckResult();
            semanticCheckResult.Correct = true;
            semanticCheckResult.Type = CompilationUnit.GetTypeIfDef(Class.TypeName);

            string inherit= Class.ParentTypeName;
            semanticCheckResult.Correct &= CompilationUnit.IsTypeDef(inherit);

            foreach (var item in Class.Methods)
            {
                semanticCheckResult.Correct &= item.Accept(this).Correct;
            }
            foreach (var item in Class.Atributes)
            {
                semanticCheckResult.Correct &= item.Accept(this).Correct;
            }
            return semanticCheckResult;
        }

        public SemanticCheckResult VisitDivision(ASTDivideNode Divide)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            result.Type = CompilationUnit.Int;

            var left = Divide.Left.Accept(this);
            var right = Divide.Right.Accept(this);

            result.Correct = left.Correct && right.Correct && left.Type == result.Type && right.Type == result.Type;
            return result;
        }

        public SemanticCheckResult VisitEqual(ASTEqualNode Equal)
        {
            var left = Equal.Left.Accept(this);
            var right = Equal.Right.Accept(this);
            if (left.Type==CompilationUnit.Bool || left.Type==CompilationUnit.Int || left.Type==CompilationUnit.String || right.Type == CompilationUnit.Bool || right.Type == CompilationUnit.Int || right.Type == CompilationUnit.String)
            {
                return new SemanticCheckResult() {Correct=left.Type==right.Type,Type=CompilationUnit.Bool };
            }
            return new SemanticCheckResult() { Correct = true, Type = CompilationUnit.Bool };
        }

        public SemanticCheckResult VisitBoolConstant(ASTBoolConstantNode BoolConstant)
        {
            return new SemanticCheckResult
            {
                Correct = true,
                Type = CompilationUnit.Bool
            };
        }

        public SemanticCheckResult VisitIf(ASTIfNode If)
        {
            var result = new SemanticCheckResult();

            var conditionResult = If.Condition.Accept(this);
            var thenResult = If.Then.Accept(this);
            var elseResult = If.Else.Accept(this);

            result.Correct = conditionResult.Type == CompilationUnit.Bool && thenResult.Correct && elseResult.Correct;
            result.Type = CompilationUnit.GetTypeLCA(thenResult.Type, elseResult.Type);

            return result;
        }

        public SemanticCheckResult VisitIntConstant(ASTIntConstantNode IntConstant)
        {
            var semanticCheckResult = new SemanticCheckResult();
            semanticCheckResult.Correct = true;
            semanticCheckResult.Type = CompilationUnit.Int;
            return semanticCheckResult;
        }

        public SemanticCheckResult VisitIsvoid(ASTIsVoidNode IsVoid)
        {
            var res = IsVoid.Expression.Accept(this);
            return new SemanticCheckResult() { Correct = true, Type = CompilationUnit.Bool };
        }

        public SemanticCheckResult VisitLessEqual(ASTLessEqualNode LessEqual)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            result.Type = CompilationUnit.Bool;

            var integer = CompilationUnit.Int;

            var left = LessEqual.Left.Accept(this);
            var right = LessEqual.Right.Accept(this);

            result.Correct = left.Correct && right.Correct && left.Type == integer && right.Type == integer;
            return result;
        }

        public SemanticCheckResult VisitLessThan(ASTLessThanNode LessThan)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            result.Type = CompilationUnit.Bool;

            var integer = CompilationUnit.Int;

            var left = LessThan.Left.Accept(this);
            var right = LessThan.Right.Accept(this);

            result.Correct = left.Correct && right.Correct && left.Type == integer && right.Type == integer;
            return result;
        }

        public SemanticCheckResult VisitLetIn(ASTLetInNode LetIn)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitFormal(ASTFormalNode Formal)
        {
            var result = new SemanticCheckResult();
            result.Correct= CompilationUnit.IsTypeDef(Formal.Type);
            if (result.Correct)
                result.Type = CompilationUnit.GetTypeIfDef(Formal.Type);
            return result;
        }

        public SemanticCheckResult VisitMethod(ASTMethodNode Method)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            foreach (var item in Method.Formals)
                result.Correct = item.Accept(this).Correct;

            var exprResult = Method.Body.Accept(this);
            if (!CompilationUnit.IsTypeDef(Method.ReturnType))
            {
                result.Correct = false;
                return result;
            }
            var returnType = CompilationUnit.GetTypeIfDef(Method.ReturnType);
            result.Correct &= exprResult.Type.IsIt(returnType);

            return result; 
        }

        public SemanticCheckResult VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall)
        {
            var result = new SemanticCheckResult();
            var onResult = MethodCall.InvokeOnExpresion.Accept(this);
            result.Correct &= onResult.Correct;
            result.Correct &= onResult.Type.IsIt(CompilationUnit.GetTypeIfDef(MethodCall.Type));

            CoolMethod m = CompilationUnit.GetMethodIfDef(MethodCall.Type, MethodCall.MethodName);
            if (m.Params.Count == MethodCall.Arguments.Length)
                result.Correct = true;
            else
                result.Correct = false;
            for (int i = 0; i < m.Params.Count; i++)
            {
                var r = MethodCall.Arguments[i].Accept(this);
                result.Correct &= r.Correct;
                result.Correct &= (r.Type.IsIt(m.Params[i]));
            }
            result.Type = m.ReturnType;
            return result;
        }

        public SemanticCheckResult VisitMinus(ASTMinusNode Minus)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            result.Type = CompilationUnit.Int;

            var left = Minus.Left.Accept(this);
            var right = Minus.Right.Accept(this);

            result.Correct = left.Correct && right.Correct && left.Type == result.Type && right.Type == result.Type;
            return result;
        }

        public SemanticCheckResult VisitMultiply(ASTMultiplyNode Multiply)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            result.Type = CompilationUnit.Int;

            var left = Multiply.Left.Accept(this);
            var right = Multiply.Right.Accept(this);

            result.Correct = left.Correct && right.Correct && left.Type == result.Type && right.Type == result.Type;
            return result;
        }

        public SemanticCheckResult VisitNegative(ASTNegativeNode Negatve)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            result.Type = CompilationUnit.Int;

            var exp = Negatve.Expression.Accept(this);

            result.Correct = exp.Correct  && exp.Type == result.Type ;
            return result;
        }

        public SemanticCheckResult VisitNew(ASTNewNode New)
        {
            return new SemanticCheckResult() { Correct = CompilationUnit.IsTypeDef(New.Type), Type = CompilationUnit.GetTypeIfDef(New.Type) };
        }

        public SemanticCheckResult VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall)
        {
            var result = new SemanticCheckResult();
            CoolMethod m = CompilationUnit.GetMethodIfDef(OwnMethodCall.TypeEnvironment.CoolType,OwnMethodCall.Method);
            if (m.Params.Count==OwnMethodCall.Arguments.Length)
                result.Correct = true;
            else
                result.Correct = false;
            for (int i = 0; i < m.Params.Count; i++)
            {
                var r=OwnMethodCall.Arguments[i].Accept(this);
                result.Correct &= r.Correct;
                result.Correct &= (r.Type==m.Params[i]);
            }
            result.Type = m.ReturnType;
            return result;
        }

        public SemanticCheckResult VisitProgram(ASTProgramNode Program)
        {
            var result = new SemanticCheckResult();
            result.Correct = true;
            foreach (var item in Program.Clases)
                result.Correct &= item.Accept(this).Correct;
            return result;
        }

        public SemanticCheckResult VisitAtribute(ASTAtributeNode Atribute)
        {
            var result = new SemanticCheckResult();
            if (CompilationUnit.IsTypeDef(Atribute.Type))
                result.Correct = true;
            else
                result.Correct = false;
            var t = CompilationUnit.GetTypeIfDef(Atribute.Type);
            var r = Atribute.Init.Accept(this);
            result.Correct &=r.Correct;
            result.Correct &= (r.Type.IsIt(t));
            result.Type = t;
            return result;
        }

        public SemanticCheckResult VisitStringConstant(ASTStringConstantNode StringConstant)
        {
            var semanticCheckResult = new SemanticCheckResult();
            semanticCheckResult.Correct = true;
            semanticCheckResult.Type = CompilationUnit.String;
            return semanticCheckResult;
        }

        public SemanticCheckResult VisitWhile(ASTWhileNode While)
        {
            var result = new SemanticCheckResult();
            var cond = While.Condition.Accept(this);
            result.Correct = cond.Correct;
            var body = While.Body.Accept(this);
            result.Correct &= body.Correct;
            result.Type = CompilationUnit.Object;
            return result;
        }

        public SemanticCheckResult VisitId(ASTIdNode Id)
        {
            var result = new SemanticCheckResult();
            result.Correct = Id.TypeEnvironment.IsDefO(Id.Name);
            result.Type = CompilationUnit.GetTypeIfDef(Id.TypeEnvironment.GetTypeO(Id.Name));
            return result;
        }

        public SemanticCheckResult Visit(ASTNode Node)
        {
            throw new NotImplementedException();//Abstract so its not intended to be dynamic type of any 
        }

        public SemanticCheckResult VisitExpression(ASTExpressionNode Expression)
        {
            throw new NotImplementedException();//Expresion is abstract so its not intended to be dynamic type of any 
        }

        public SemanticCheckResult VisitDynamicMethodCall(ASTDynamicMethodCallNode MethodCall)
        {
            var result = new SemanticCheckResult();
            var onResult = MethodCall.InvokeOnExpresion.Accept(this);
            result.Correct &= onResult.Correct;
            CoolMethod m = CompilationUnit.GetMethodIfDef(onResult.Type.Name, MethodCall.MethodName);
            if (m.Params.Count == MethodCall.Arguments.Length)
                result.Correct = true;
            else
                result.Correct = false;
            for (int i = 0; i < m.Params.Count; i++)
            {
                var r = MethodCall.Arguments[i].Accept(this);
                result.Correct &= r.Correct;
                result.Correct &= (r.Type.IsIt(m.Params[i]));
            }
            result.Type = m.ReturnType;
            return result;
        }
    }
}
