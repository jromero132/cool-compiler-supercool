using SuperCOOL.CodeGeneration.CIL.AST;
using SuperCOOL.Constants;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration
{
    class SuperCoolCILASTVisitor : ISuperCoolASTVisitor<ASTCILNode>
    {
        private ILabelILGenerator labelIlGenerator { get; }
        private CompilationUnit compilationUnit { get; }

        public SuperCoolCILASTVisitor(ILabelILGenerator labelIlGenerator, CompilationUnit compilationUnit)
        {
            this.labelIlGenerator = labelIlGenerator;
            this.compilationUnit = compilationUnit;
        }

        public ASTCILNode VisitAdd(ASTAddNode Add)
        {
            if (Add.Left is ASTIntConstantNode left && Add.Right is ASTIntConstantNode right)
                return new ASTCILAddTwoConstantNode(left.Value, right.Value);
            if (Add.Left is ASTIntConstantNode left2)
                return new ASTCILAddConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) Add.Right.Accept(this));
            if (Add.Right is ASTIntConstantNode right2)
                return new ASTCILAddVariableConstantNode((ASTCILExpressionNode) Add.Left.Accept(this),
                    right2.Value);
            return new ASTCILAddTwoVariablesNode((ASTCILExpressionNode) Add.Left.Accept(this),
                (ASTCILExpressionNode) Add.Right.Accept(this));
        }

        public ASTCILNode VisitAssignment(ASTAssingmentNode Assigment)
        {
            //TODO setattr if is a field and localAssigment if not
            return new ASTCILAssignmentNode(Assigment.Id.Name,
                (ASTCILExpressionNode) Assigment.Expresion.Accept(this));
        }

        public ASTCILNode VisitBlock(ASTBlockNode Block)
        {
            return new ASTCILBlockNode(Block.Expresions.Select(x => (ASTCILExpressionNode) x.Accept(this)));
        }

        public ASTCILNode VisitBoolNot(ASTBoolNotNode BoolNot)
        {
            return new ASTCILBoolNotNode((ASTCILExpressionNode) BoolNot.Accept(this));
        }

        public ASTCILNode VisitCase(ASTCaseNode Case)
        {
            var caseExpression = (ASTCILExpressionNode) Case.ExpressionCase.Accept(this);
            var caseLabels = labelIlGenerator.GenerateCase();
            var caseExpressionCheckVoid = new ASTCILIfNode(new ASTCILIsVoidNode(caseExpression),
                new ASTCILRuntimeErrorNode(RuntimeErrors.CaseVoidRuntimeError),
                new ASTCILBlockNode(Enumerable.Empty<ASTCILExpressionNode>()), labelIlGenerator.GenerateIf());
            var caseExpressionType = caseExpression.Type;
            var caseExpressions = new List<ASTCILExpressionNode>
                { caseExpression, caseExpressionCheckVoid };
            while (caseExpressionType != null)
            {

                foreach (var caseSubExpression in Case.Cases)
                {
                    caseExpressions.Add(new ASTCILIfNode(
                        new ASTCILEqualNode(new ASTCILStringConstantNode(caseExpressionType),
                            new ASTCILStringConstantNode(caseExpression.Type))
                        , new ASTCILBlockNode
                        (
                            new[]
                            {
                                new ASTCILLocalNode(caseSubExpression.Name.Text, caseSubExpression.Type.Text),
                                new ASTCILAssignmentNode(caseSubExpression.Name.Text,
                                    caseExpression),
                                (ASTCILExpressionNode) caseSubExpression.Branch.Accept(this),
                                new ASTCILGotoNode(caseLabels.endOfCase),
                            }
                        ), new ASTCILBlockNode(Enumerable.Empty<ASTCILExpressionNode>()),
                        labelIlGenerator.GenerateIf()));
                }

                compilationUnit.TypeEnvironment.GetTypeDefinition(caseExpressionType, Case.SymbolTable, out var type);
                caseExpressionType = type.Parent.Name;
            }

            caseExpressions.Add(new ASTCILRuntimeErrorNode(RuntimeErrors.CaseWithoutMatching));

            return new ASTCILBlockNode(caseExpressions);
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
                    (ASTCILExpressionNode) Division.Right.Accept(this));
            if (Division.Right is ASTIntConstantNode right2)
                return new ASTCILDivideVariableConstantNode((ASTCILExpressionNode) Division.Left.Accept(this),
                    right2.Value);
            return new ASTCILDivideTwoVariablesNode((ASTCILExpressionNode) Division.Left.Accept(this),
                (ASTCILExpressionNode) Division.Right.Accept(this));
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
            return new ASTCILIfNode((ASTCILExpressionNode) If.Condition.Accept(this),
                (ASTCILExpressionNode) If.Then.Accept(this), (ASTCILExpressionNode) If.Else.Accept(this),
                labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitIntConstant(ASTIntConstantNode Int)
        {
            return new ASTCILIntConstantNode(Int.Value);
        }

        public ASTCILNode VisitIsvoid(ASTIsVoidNode IsVoid)
        {
            return new ASTCILIsVoidNode((ASTCILExpressionNode) IsVoid.Expression.Accept(this));
        }

        public ASTCILNode VisitLessEqual(ASTLessEqualNode LessEqual)
        {
            return new ASTCILIfNode(
                new ASTCILBoolOrTwoVariablesNode(
                    (ASTCILExpressionNode) (new ASTLessThanNode
                        { Left = LessEqual.Left, Right = LessEqual.Right }).Accept(this),
                    (ASTCILExpressionNode) (new ASTEqualNode
                        { Left = LessEqual.Left, Right = LessEqual.Right }).Accept(this)), new ASTCILIntConstantNode(1),
                new ASTCILIntConstantNode(0), labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitLessThan(ASTLessThanNode LessThan)
        {
            if (LessThan.Left is ASTIntConstantNode left && LessThan.Right is ASTIntConstantNode right)
                return new ASTCILLessThanTwoConstantNode(left.Value, right.Value);
            if (LessThan.Left is ASTIntConstantNode left2)
                return new ASTCILLessThanConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) LessThan.Right.Accept(this));
            if (LessThan.Right is ASTIntConstantNode right2)
                return new ASTCILLessThanVariableConstantNode((ASTCILExpressionNode) LessThan.Left.Accept(this),
                    right2.Value);
            return new ASTCILLessThanTwoVariablesNode((ASTCILExpressionNode) LessThan.Left.Accept(this),
                (ASTCILExpressionNode) LessThan.Right.Accept(this));
        }

        public ASTCILNode VisitLetIn(ASTLetInNode LetIn)
        {
            return new ASTCILBlockNode(LetIn.Declarations.SelectMany(x => new ASTCILExpressionNode[]
            {
                new ASTCILLocalNode(x.Id.Text, x.Type.Text),
                new ASTCILAssignmentNode(x.Id.Text, (ASTCILExpressionNode) x.Expression.Accept(this))
            }).Concat(Enumerable.Repeat((ASTCILExpressionNode) LetIn.LetExp.Accept(this), 1)));
        }

        public ASTCILNode VisitMethod(ASTMethodNode Method)
        {
            throw new NotImplementedException();
        }

        public ASTCILNode VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall)
        {
            return new ASTCILIfNode(
                new ASTCILIsVoidNode((ASTCILExpressionNode) MethodCall.InvokeOnExpresion.Accept(this)),
                new ASTCILRuntimeErrorNode(RuntimeErrors.DispatchOnVoid), new ASTCILFuncStaticCallNode(
                    MethodCall.MethodName, MethodCall.Type.Text,
                    Enumerable.Repeat((ASTCILExpressionNode) MethodCall.InvokeOnExpresion.Accept(this), 1)
                        .Concat(MethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this)))),
                labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitDynamicMethodCall(ASTDynamicMethodCallNode MethodCall)
        {
            return new ASTCILIfNode(
                new ASTCILIsVoidNode((ASTCILExpressionNode) MethodCall.InvokeOnExpresion.Accept(this)),
                new ASTCILRuntimeErrorNode(RuntimeErrors.DispatchOnVoid), new ASTCILFuncVirtualCallNode(
                    MethodCall.MethodName,
                    Enumerable.Repeat((ASTCILExpressionNode) MethodCall.InvokeOnExpresion.Accept(this), 1)
                        .Concat(MethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this)))),
                labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall)
        {
            //TODO add self
            return new ASTCILFuncVirtualCallNode(OwnMethodCall.Method.Text,
                OwnMethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this)));
        }

        public ASTCILNode VisitMinus(ASTMinusNode Minus)
        {
            if (Minus.Left is ASTIntConstantNode left && Minus.Right is ASTIntConstantNode right)
                return new ASTCILMinusTwoConstantNode(left.Value, right.Value);
            if (Minus.Left is ASTIntConstantNode left2)
                return new ASTCILMinusConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) Minus.Right.Accept(this));
            if (Minus.Right is ASTIntConstantNode right2)
                return new ASTCILMinusVariableConstantNode((ASTCILExpressionNode) Minus.Left.Accept(this),
                    right2.Value);
            return new ASTCILMinusTwoVariablesNode((ASTCILExpressionNode) Minus.Left.Accept(this),
                (ASTCILExpressionNode) Minus.Right.Accept(this));
        }

        public ASTCILNode VisitMultiply(ASTMultiplyNode Multiply)
        {
            if (Multiply.Left is ASTIntConstantNode left && Multiply.Right is ASTIntConstantNode right)
                return new ASTCILMultiplyTwoConstantNode(left.Value, right.Value);
            if (Multiply.Left is ASTIntConstantNode left2)
                return new ASTCILMultiplyConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) Multiply.Right.Accept(this));
            if (Multiply.Right is ASTIntConstantNode right2)
                return new ASTCILMultiplyVariableConstantNode((ASTCILExpressionNode) Multiply.Left.Accept(this),
                    right2.Value);
            return new ASTCILMultiplyTwoVariablesNode((ASTCILExpressionNode) Multiply.Left.Accept(this),
                (ASTCILExpressionNode) Multiply.Right.Accept(this));
        }

        public ASTCILNode VisitNegative(ASTNegativeNode Negative)
        {
            if (Negative.Expression is ASTIntConstantNode constant)
                return new ASTCILMultiplyTwoConstantNode(constant.Value, -1);
            return new ASTCILMultiplyVariableConstantNode((ASTCILExpressionNode) Negative.Accept(this), -1);
        }

        public ASTCILNode VisitNew(ASTNewNode context)
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
            var ifLabel = labelIlGenerator.GenerateIf();
            return new ASTCILIfNode((ASTCILExpressionNode) While.Condition.Accept(this),
                new ASTCILBlockNode(new[]
                {
                    (ASTCILExpressionNode) While.Body.Accept(this), new ASTCILGotoNode(ifLabel)
                }),
                new ASTCILBlockNode(Enumerable.Empty<ASTCILExpressionNode>()), ifLabel) { Type = Types.Void };
        }

        public ASTCILNode VisitId(ASTIdNode Id)
        {
            return new ASTCILIdNode(Id.Name);
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
