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
            Assigment.SymbolTable.IsDefObject(Assigment.Id.Name, out var symbolInfo);

            return symbolInfo.Kind == ObjectKind.Atribute
                ? (ASTCILNode) new ASTCILSetAttributeNode(symbolInfo.Name, symbolInfo.Type,
                    (ASTCILExpressionNode) Assigment.Expresion.Accept(this))
                : new ASTCILAssignmentNode(Assigment.Id.Name, (ASTCILExpressionNode) Assigment.Expresion.Accept(this));
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
                        (ASTCILExpressionNode) new ASTEqualNode
                        {
                            Left = new ASTStringConstantNode { Value = caseExpressionType },
                            Right = new ASTStringConstantNode { Value = caseExpression.Type }
                        }.Accept(this)
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
            var attributesInit = Class.Atributes.Select(x => (ASTCILExpressionNode) x.Accept(this));

            var methods = Class.Methods.Select(x => (ASTCILFuncNode) x.Accept(this))
                .Append(new ASTCILFuncNode(labelIlGenerator.GenerateInit(Class.TypeName), attributesInit));

            var attributesInfo = Class.SymbolTable.AllDefinedAttributes();
            compilationUnit.TypeEnvironment.GetTypeDefinition(Class.TypeName, Class.SymbolTable, out var type);
            var virtualTable = compilationUnit.MethodEnvironment.GetVirtualTable(type);
            return new ASTCILTypeNode(type, attributesInfo, virtualTable, methods);
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
            }).Append((ASTCILExpressionNode) LetIn.LetExp.Accept(this)));
        }

        public ASTCILNode VisitMethod(ASTMethodNode Method)
        {
            var type=compilationUnit.TypeEnvironment.GetContextType(Method.SymbolTable);
            compilationUnit.MethodEnvironment.GetMethodOnIt(type,Method.Name,out var coolMethod);
            return new ASTCILFuncNode(labelIlGenerator.GenerateFunc(coolMethod.Type.Name,coolMethod.Name),
                Method.Formals.Select(x => new ASTCILParamNode(x.name.Text, x.type.Text))
                    .Append((ASTCILExpressionNode) Method.Body.Accept(this))
            );
        }

        public ASTCILNode VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall)
        {
            return new ASTCILIfNode(
                new ASTCILIsVoidNode((ASTCILExpressionNode) MethodCall.InvokeOnExpresion.Accept(this)),
                new ASTCILRuntimeErrorNode(RuntimeErrors.DispatchOnVoid), new ASTCILFuncStaticCallNode(
                    MethodCall.MethodName, MethodCall.Type.Text,
                    new[] { (ASTCILExpressionNode)MethodCall.InvokeOnExpresion.Accept(this) }
                        .Concat(MethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this)))),
                labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitDynamicMethodCall(ASTDynamicMethodCallNode MethodCall)
        {
            return new ASTCILIfNode(
                new ASTCILIsVoidNode((ASTCILExpressionNode) MethodCall.InvokeOnExpresion.Accept(this)),
                new ASTCILRuntimeErrorNode(RuntimeErrors.DispatchOnVoid), new ASTCILFuncVirtualCallNode(
                    MethodCall.MethodName,
                    new[] { (ASTCILExpressionNode)MethodCall.InvokeOnExpresion.Accept(this) }
                        .Concat(MethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this)))),
                labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall)
        {
            return new ASTCILFuncVirtualCallNode(OwnMethodCall.Method.Text,
                    new[] { new ASTCILSelfNode() }
                    .Concat(OwnMethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this))));
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
            var type = context.SemanticCheckResult.Type.Name;
            if (context.SemanticCheckResult.Type is SelfType selftype)
            {
                type = selftype.ContextType.Name;
            }

            return new ASTCILBlockNode(new ASTCILExpressionNode[]
            {
                new ASTCILAllocateNode(type),
                new ASTCILFuncVirtualCallNode(labelIlGenerator.GenerateInit(type),Enumerable.Empty<ASTCILExpressionNode>())
            });
        }

        public ASTCILNode VisitProgram(ASTProgramNode Program)
        {
            return new ASTCILProgramNode(Program.Clases.Select(x => (ASTCILTypeNode) x.Accept(this)));
        }

        public ASTCILNode VisitAtribute(ASTAtributeNode Atribute)
        {
            if (Atribute.HasInit)
                return Atribute.Init.Accept(this);

            compilationUnit.TypeEnvironment.GetTypeForObject(Atribute.SymbolTable, Atribute.AttributeName,
                out var coolType);
            if (coolType.Equals(compilationUnit.TypeEnvironment.Int))
                return new ASTCILSetAttributeNode(Atribute.TypeName, Atribute.AttributeName,
                    new ASTCILIntConstantNode(0));
            if (coolType.Equals(compilationUnit.TypeEnvironment.String))
                return new ASTCILSetAttributeNode(Atribute.TypeName, Atribute.AttributeName,
                    new ASTCILStringConstantNode("", labelIlGenerator.GenerateEmptyStringData()));
            if (coolType.Equals(compilationUnit.TypeEnvironment.Bool))
                return new ASTCILSetAttributeNode(Atribute.TypeName, Atribute.AttributeName,
                    new ASTCILBoolConstantNode(false));
            return new ASTCILSetAttributeNode(Atribute.TypeName, Atribute.AttributeName, new ASTCILVoidNode());
        }

        public ASTCILNode VisitStringConstant(ASTStringConstantNode StringConstant)
        {
            return new ASTCILStringConstantNode(StringConstant.Value, labelIlGenerator.GenerateStringData());
        }

        public ASTCILNode VisitWhile(ASTWhileNode While)
        {
            var ifLabel = labelIlGenerator.GenerateIf();
            return new ASTCILIfNode((ASTCILExpressionNode) While.Condition.Accept(this),
                new ASTCILBlockNode(new[]
                {
                    (ASTCILExpressionNode) While.Body.Accept(this), new ASTCILGotoNode(ifLabel.end)
                }),
                new ASTCILVoidNode(), ifLabel) { Type = Types.Void };
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
