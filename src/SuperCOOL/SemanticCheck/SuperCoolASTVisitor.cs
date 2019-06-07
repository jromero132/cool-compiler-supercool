using SuperCOOL.SemanticCheck.AST;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.SemanticCheck
{
    public class SuperCoolASTVisitor<T> : ISuperCoolASTVisitor<T>
    {
        public virtual T Visit(ASTNode Node)
        {
            throw new NotImplementedException();//ASTNode is Abstract
        }

        public virtual T VisitAdd(ASTAddNode Add)
        {
            Add.Left.Accept(this);
            Add.Right.Accept(this);
            return default (T);
        }

        public virtual T VisitAssignment(ASTAssingmentNode Assigment)
        {
            Assigment.Id.Accept(this);
            Assigment.Expresion.Accept(this);
            return default(T);
        }

        public virtual T VisitAtribute(ASTAtributeNode Atribute)
        {
            if (Atribute.HasInit)
                Atribute.Init.Accept(this);
            return default(T);
        }

        public virtual T VisitBlock(ASTBlockNode Block)
        {
            foreach (var item in Block.Expresions)
                item.Accept(this);
            return default(T);
        }

        public virtual T VisitBoolConstant(ASTBoolConstantNode BoolConstant)
        {
            return default(T);
        }

        public virtual T VisitBoolNot(ASTBoolNotNode BoolNot)
        {
            BoolNot.Expresion.Accept(this);
            return default(T);
        }

        public virtual T VisitCase(ASTCaseNode Case)
        {
            Case.ExpressionCase.Accept(this);
            foreach (var item in Case.Cases)
                item.Branch.Accept(this);
            return default(T);
        }

        public virtual T VisitClass(ASTClassNode Class)
        {
            foreach (var item in Class.Methods)
                item.Accept(this);
            foreach (var item in Class.Atributes)
                item.Accept(this);
            return default(T);
        }

        public virtual T VisitDivision(ASTDivideNode Division)
        {
            Division.Left.Accept(this);
            Division.Right.Accept(this);
            return default(T);
        }

        public virtual T VisitDynamicMethodCall(ASTDynamicMethodCallNode MethodCall)
        {
            MethodCall.InvokeOnExpresion.Accept(this);
            foreach (var item in MethodCall.Arguments)
                item.Accept(this);
            return default(T);
        }

        public virtual T VisitEqual(ASTEqualNode Equal)
        {
            Equal.Left.Accept(this);
            Equal.Right.Accept(this);
            return default(T);
        }

        public virtual T VisitExpression(ASTExpressionNode Expression)
        {
            throw new NotImplementedException();//ASTExpressionNode is Abstract
        }

        public virtual T VisitId(ASTIdNode Id)
        {
            return default(T);
        }

        public virtual T VisitIf(ASTIfNode If)
        {
            If.Condition.Accept(this);
            If.Then.Accept(this);
            If.Else.Accept(this);
            return default(T);
        }

        public virtual T VisitIntConstant(ASTIntConstantNode Int)
        {
            return default(T);
        }

        public virtual T VisitIsvoid(ASTIsVoidNode IsVoid)
        {
            IsVoid.Expression.Accept(this);
            return default(T);
        }

        public virtual T VisitLessEqual(ASTLessEqualNode LessEqual)
        {
            LessEqual.Left.Accept(this);
            LessEqual.Right.Accept(this);
            return default(T);
        }

        public virtual T VisitLessThan(ASTLessThanNode LessThan)
        {
            LessThan.Left.Accept(this);
            LessThan.Right.Accept(this);
            return default(T);
        }

        public virtual T VisitLetIn(ASTLetInNode LetIn)
        {
            foreach (var item in LetIn.Declarations)
                item.Expression?.Accept(this);
            LetIn.LetExp.Accept(this);
            return default(T);
        }

        public virtual T VisitMethod(ASTMethodNode Method)
        {
            Method.Body.Accept(this);
            return default(T);
        }

        public virtual T VisitMinus(ASTMinusNode Minus)
        {
            Minus.Left.Accept(this);
            Minus.Right.Accept(this);
            return default(T);
        }

        public virtual T VisitMultiply(ASTMultiplyNode Multiply)
        {
            Multiply.Left.Accept(this);
            Multiply.Right.Accept(this);
            return default(T);
        }

        public virtual T VisitNegative(ASTNegativeNode Negative)
        {
            Negative.Expression.Accept(this);
            return default(T);
        }

        public virtual T VisitNew(ASTNewNode context)
        {
            return default(T);
        }

        public virtual T VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall)
        {
            foreach (var item in OwnMethodCall.Arguments)
                item.Accept(this);
            return default(T);
        }

        public virtual T VisitProgram(ASTProgramNode Program)
        {
            foreach (var item in Program.Clases)
                item.Accept(this);
            return default(T);
        }

        public virtual T VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall)
        {
            foreach (var item in MethodCall.Arguments)
                item.Accept(this);
            return default(T);
        }

        public virtual T VisitStringConstant(ASTStringConstantNode StringConstant)
        {
            return default(T);
        }

        public virtual T VisitWhile(ASTWhileNode While)
        {
            While.Condition.Accept(this);
            While.Body.Accept(this);
            return default(T);
        }
    }
}
