using SuperCOOL.CodeGeneration.CIL.AST;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;
using System;
using System.Linq;

namespace SuperCOOL.CodeGeneration
{
    class SuperCoolCILASTVisitor : ISuperCoolASTVisitor<ASTCILNode>
    {
        private ILabelILGenerator labelIlGenerator { get; }

        public SuperCoolCILASTVisitor(ILabelILGenerator labelIlGenerator)
        {
            this.labelIlGenerator = labelIlGenerator;
        }

        public ASTCILNode VisitAdd(ASTAddNode Add)
        {
            if (Add.Left is ASTIntConstantNode left && Add.Right is ASTIntConstantNode right)
                return new ASTCILAddTwoConstantNode(left.Value, right.Value);
            if (Add.Left is ASTIntConstantNode left2)
                return new ASTCILAddConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) VisitExpression(Add.Right));
            if (Add.Right is ASTIntConstantNode right2)
                return new ASTCILAddVariableConstantNode((ASTCILExpressionNode) VisitExpression(Add.Left),
                    right2.Value);
            return new ASTCILAddTwoVariablesNode((ASTCILExpressionNode) VisitExpression(Add.Left),
                (ASTCILExpressionNode) VisitExpression(Add.Right));
        }

        public ASTCILNode VisitAssignment(ASTAssingmentNode Assigment)
        {
            return new ASTCILAssingmentNode(new ASTCILIdNode(Assigment.Id.Name),
                (ASTCILExpressionNode) VisitExpression(Assigment.Expresion));
        }

        public ASTCILNode VisitBlock(ASTBlockNode Block)
        {
            return new ASTCILBlockNode(Block.Expresions.Select(x => (ASTCILExpressionNode) VisitExpression(x)));
        }

        public ASTCILNode VisitBoolNot(ASTBoolNotNode BoolNot)
        {
            return new ASTCILBoolNotNode((ASTCILExpressionNode) VisitExpression(BoolNot));
        }

        public ASTCILNode VisitCase(ASTCaseNode Case)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitClass(ASTClassNode Class)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitDivision(ASTDivideNode Division)
        {
            if (Division.Left is ASTIntConstantNode left && Division.Right is ASTIntConstantNode right)
                return new ASTCILDivideTwoConstantNode(left.Value, right.Value);
            if (Division.Left is ASTIntConstantNode left2)
                return new ASTCILDivideConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) VisitExpression(Division.Right));
            if (Division.Right is ASTIntConstantNode right2)
                return new ASTCILDivideVariableConstantNode((ASTCILExpressionNode) VisitExpression(Division.Left),
                    right2.Value);
            return new ASTCILDivideTwoVariablesNode((ASTCILExpressionNode) VisitExpression(Division.Left),
                (ASTCILExpressionNode) VisitExpression(Division.Right));
        }

        public ASTCILNode VisitEqual(ASTEqualNode Equal)
        {
            return new ASTCILIfNode(
                (ASTCILExpressionNode) VisitMinus(new ASTMinusNode { Left = Equal.Left, Right = Equal.Right }),
                new ASTCILBoolConstantNode(true), new ASTCILBoolConstantNode(false),
                labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitBoolConstant(ASTBoolConstantNode BoolConstant)
        {
            return new ASTCILBoolConstantNode(BoolConstant.Value);
        }

        public ASTCILNode VisitIf(ASTIfNode If)
        {
            return new ASTCILIfNode((ASTCILExpressionNode) VisitExpression(If.Condition),
                (ASTCILExpressionNode) VisitExpression(If.Then), (ASTCILExpressionNode) VisitExpression(If.Else),
                labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitIntConstant(ASTIntConstantNode Int)
        {
            return new ASTCILIntConstantNode(Int.Value);
        }

        public ASTCILNode VisitIsvoid(ASTIsVoidNode IsVoid)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitLessEqual(ASTLessEqualNode LessEqual)
        {
            return new ASTCILIfNode(
                new ASTCILBoolOrTwoVariablesNode(
                    (ASTCILExpressionNode) VisitExpression(new ASTLessThanNode
                        { Left = LessEqual.Left, Right = LessEqual.Right }),
                    (ASTCILExpressionNode) VisitExpression(new ASTEqualNode
                        { Left = LessEqual.Left, Right = LessEqual.Right })), new ASTCILIntConstantNode(1),
                new ASTCILIntConstantNode(0), labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitLessThan(ASTLessThanNode LessThan)
        {
            if (LessThan.Left is ASTIntConstantNode left && LessThan.Right is ASTIntConstantNode right)
                return new ASTCILLessThanTwoConstantNode(left.Value, right.Value);
            if (LessThan.Left is ASTIntConstantNode left2)
                return new ASTCILLessThanConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode)VisitExpression(LessThan.Right));
            if (LessThan.Right is ASTIntConstantNode right2)
                return new ASTCILLessThanVariableConstantNode((ASTCILExpressionNode)VisitExpression(LessThan.Left),
                    right2.Value);
            return new ASTCILLessThanTwoVariablesNode((ASTCILExpressionNode)VisitExpression(LessThan.Left),
                (ASTCILExpressionNode)VisitExpression(LessThan.Right));
        }

        public ASTCILNode VisitLetIn(ASTLetInNode LetIn)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitMethod(ASTMethodNode Method)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitDynamicMethodCall(ASTDynamicMethodCallNode MethodCall)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitMinus(ASTMinusNode Minus)
        {
            if (Minus.Left is ASTIntConstantNode left && Minus.Right is ASTIntConstantNode right)
                return new ASTCILMinusTwoConstantNode(left.Value, right.Value);
            if (Minus.Left is ASTIntConstantNode left2)
                return new ASTCILMinusConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) VisitExpression(Minus.Right));
            if (Minus.Right is ASTIntConstantNode right2)
                return new ASTCILMinusVariableConstantNode((ASTCILExpressionNode) VisitExpression(Minus.Left),
                    right2.Value);
            return new ASTCILMinusTwoVariablesNode((ASTCILExpressionNode) VisitExpression(Minus.Left),
                (ASTCILExpressionNode) VisitExpression(Minus.Right));
        }

        public ASTCILNode VisitMultiply(ASTMultiplyNode Multiply)
        {
            if (Multiply.Left is ASTIntConstantNode left && Multiply.Right is ASTIntConstantNode right)
                return new ASTCILMultiplyTwoConstantNode(left.Value, right.Value);
            if (Multiply.Left is ASTIntConstantNode left2)
                return new ASTCILMultiplyConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) VisitExpression(Multiply.Right));
            if (Multiply.Right is ASTIntConstantNode right2)
                return new ASTCILMultiplyVariableConstantNode((ASTCILExpressionNode) VisitExpression(Multiply.Left),
                    right2.Value);
            return new ASTCILMultiplyTwoVariablesNode((ASTCILExpressionNode) VisitExpression(Multiply.Left),
                (ASTCILExpressionNode) VisitExpression(Multiply.Right));
        }

        public ASTCILNode VisitNegative(ASTNegativeNode Negative)
        {
            if (Negative.Expression is ASTIntConstantNode constant)
                return new ASTCILMultiplyTwoConstantNode(constant.Value, -1);
            return new ASTCILMultiplyVariableConstantNode((ASTCILExpressionNode) VisitExpression(Negative), -1);
        }

        public ASTCILNode VisitNew(ASTNewNode context)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitProgram(ASTProgramNode Program)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitAtribute(ASTAtributeNode Atribute)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitStringConstant(ASTStringConstantNode StringConstant)
        {
            return new ASTCILStringConstantNode(StringConstant.Value);
        }

        public ASTCILNode VisitWhile(ASTWhileNode While)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitId(ASTIdNode Id)
        {
            return new ASTCILIdNode(Id.Name);
        }

        public ASTCILNode VisitFormal(ASTFormalNode Formal)
        {
            return new ASTCILFormalNode(Formal.Name, Formal.Type);
        }

        public ASTCILNode Visit(ASTNode Node)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitExpression(ASTExpressionNode Expression)
        {
            throw new NotImplementedException();
        }
    }
}
