using SuperCOOL.CodeGeneration.CIL.AST;
using SuperCOOL.Constants;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using SuperCOOL.Core.Constants;

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
                return new ASTCILAddTwoConstantNode(left.Value, right.Value,Add.SymbolTable);
            if (Add.Left is ASTIntConstantNode left2)
                return new ASTCILAddConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) Add.Right.Accept(this), Add.SymbolTable);
            if (Add.Right is ASTIntConstantNode right2)
                return new ASTCILAddVariableConstantNode((ASTCILExpressionNode) Add.Left.Accept(this),
                    right2.Value, Add.SymbolTable);
            return new ASTCILAddTwoVariablesNode((ASTCILExpressionNode) Add.Left.Accept(this),
                (ASTCILExpressionNode) Add.Right.Accept(this),Add.SymbolTable);
        }

        public ASTCILNode VisitAssignment(ASTAssingmentNode Assigment)
        {
            Assigment.SymbolTable.IsDefObject(Assigment.Id.Name, out var symbolInfo);

            return symbolInfo.Kind == ObjectKind.Atribute
                ? (ASTCILNode)new ASTCILSetAttributeNode(symbolInfo.Name, symbolInfo.Type,
                    (ASTCILExpressionNode)Assigment.Expresion.Accept(this), Assigment.SymbolTable)
                : new ASTCILAssignmentNode(Assigment.Id.Name, (ASTCILExpressionNode) Assigment.Expresion.Accept(this),Assigment.SymbolTable);
        }

        public ASTCILNode VisitBlock(ASTBlockNode Block)
        {
            return new ASTCILBlockNode(Block.Expresions.Select(x => (ASTCILExpressionNode) x.Accept(this)),Block.SymbolTable);
        }

        public ASTCILNode VisitBoolNot(ASTBoolNotNode BoolNot)
        {
            return new ASTCILBoolNotNode((ASTCILExpressionNode) BoolNot.Accept(this),BoolNot.SymbolTable);
        }

        public ASTCILNode VisitCase(ASTCaseNode Case)
        {
            var caseExpression = (ASTCILExpressionNode) Case.ExpressionCase.Accept(this);
            var caseLabels = labelIlGenerator.GenerateCase();
            var caseExpressionCheckVoid = new ASTCILIfNode(new ASTCILIsVoidNode(caseExpression,Case.SymbolTable),
                new ASTCILRuntimeErrorNode(RuntimeErrors.CaseVoidRuntimeError, Case.SymbolTable),
                new ASTCILBlockNode(Enumerable.Empty<ASTCILExpressionNode>(),Case.SymbolTable), labelIlGenerator.GenerateIf(),Case.SymbolTable);
            var caseExpressionType = Case.ExpressionCase.SemanticCheckResult.Type.Name;
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
                            Right = new ASTStringConstantNode { Value = caseSubExpression.Type.Text }
                        }.Accept(this)
                        , new ASTCILBlockNode
                        (
                            new[]
                            {
                                new ASTCILAssignmentNode(caseSubExpression.Name.Text,
                                    caseExpression,Case.SymbolTable),
                                (ASTCILExpressionNode) caseSubExpression.Branch.Accept(this),
                                new ASTCILGotoNode(caseLabels.endOfCase,Case.SymbolTable),
                            },Case.SymbolTable
                        ), new ASTCILBlockNode(Enumerable.Empty<ASTCILExpressionNode>(),Case.SymbolTable),
                        labelIlGenerator.GenerateIf(),Case.SymbolTable));
                }

                compilationUnit.TypeEnvironment.GetTypeDefinition(caseExpressionType, Case.SymbolTable, out var type);
                caseExpressionType = type.Parent.Name;
            }

            caseExpressions.Add(new ASTCILRuntimeErrorNode(RuntimeErrors.CaseWithoutMatching, Case.SymbolTable));

            return new ASTCILBlockNode(caseExpressions,Case.SymbolTable);
        }

        public ASTCILNode VisitClass(ASTClassNode Class)
        {
            var attributesInit = Class.Atributes.Select(x => (ASTCILExpressionNode) x.Accept(this));

            var methods = Class.Methods.Select(x => (ASTCILFuncNode) x.Accept(this))
                .Append(new ASTCILFuncNode(labelIlGenerator.GenerateInit(Class.TypeName),
                    attributesInit, Class.SymbolTable));

            var attributesInfo = Class.SymbolTable.AllDefinedAttributes();
            compilationUnit.TypeEnvironment.GetTypeDefinition(Class.TypeName, Class.SymbolTable, out var type);
            var virtualTable = compilationUnit.MethodEnvironment.GetVirtualTable(type);
            return new ASTCILTypeNode(type, attributesInfo, virtualTable, methods, Class.SymbolTable);
        }

        public ASTCILNode VisitDivision(ASTDivideNode Division)
        {
            if (Division.Left is ASTIntConstantNode left && Division.Right is ASTIntConstantNode right)
                return new ASTCILDivideTwoConstantNode(left.Value, right.Value,Division.SymbolTable);
            if (Division.Left is ASTIntConstantNode left2)
                return new ASTCILDivideConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) Division.Right.Accept(this),Division.SymbolTable);
            if (Division.Right is ASTIntConstantNode right2)
                return new ASTCILDivideVariableConstantNode((ASTCILExpressionNode) Division.Left.Accept(this),
                    right2.Value,Division.SymbolTable);
            return new ASTCILDivideTwoVariablesNode((ASTCILExpressionNode) Division.Left.Accept(this),
                (ASTCILExpressionNode) Division.Right.Accept(this),Division.SymbolTable);
        }

        public ASTCILNode VisitEqual(ASTEqualNode Equal)
        {
            return new ASTCILIfNode(
                (ASTCILExpressionNode) VisitMinus(new ASTMinusNode { Left = Equal.Left, Right = Equal.Right }),
                new ASTCILBoolConstantNode(true,Equal.SymbolTable), new ASTCILBoolConstantNode(false,Equal.SymbolTable),
                labelIlGenerator.GenerateIf(),Equal.SymbolTable);
        }

        public ASTCILNode VisitBoolConstant(ASTBoolConstantNode BoolConstant)
        {
            return new ASTCILBoolConstantNode(BoolConstant.Value,BoolConstant.SymbolTable);
        }

        public ASTCILNode VisitIf(ASTIfNode If)
        {
            return new ASTCILIfNode((ASTCILExpressionNode) If.Condition.Accept(this),
                (ASTCILExpressionNode) If.Then.Accept(this), (ASTCILExpressionNode) If.Else.Accept(this),
                labelIlGenerator.GenerateIf(),If.SymbolTable);
        }

        public ASTCILNode VisitIntConstant(ASTIntConstantNode Int)
        {
            return new ASTCILIntConstantNode(Int.Value,Int.SymbolTable);
        }

        public ASTCILNode VisitIsvoid(ASTIsVoidNode IsVoid)
        {
            return new ASTCILIsVoidNode((ASTCILExpressionNode) IsVoid.Expression.Accept(this),IsVoid.SymbolTable);
        }

        public ASTCILNode VisitLessEqual(ASTLessEqualNode LessEqual)
        {
            return new ASTCILIfNode(
                new ASTCILBoolOrTwoVariablesNode(
                    (ASTCILExpressionNode) (new ASTLessThanNode
                        { Left = LessEqual.Left, Right = LessEqual.Right }).Accept(this),
                    (ASTCILExpressionNode) (new ASTEqualNode
                        { Left = LessEqual.Left, Right = LessEqual.Right }).Accept(this),LessEqual.SymbolTable), new ASTCILIntConstantNode(1,LessEqual.SymbolTable),
                new ASTCILIntConstantNode(0,LessEqual.SymbolTable), labelIlGenerator.GenerateIf(),LessEqual.SymbolTable);
        }

        public ASTCILNode VisitLessThan(ASTLessThanNode LessThan)
        {
            if (LessThan.Left is ASTIntConstantNode left && LessThan.Right is ASTIntConstantNode right)
                return new ASTCILLessThanTwoConstantNode(left.Value, right.Value, LessThan.SymbolTable);
            if (LessThan.Left is ASTIntConstantNode left2)
                return new ASTCILLessThanConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) LessThan.Right.Accept(this), LessThan.SymbolTable);
            if (LessThan.Right is ASTIntConstantNode right2)
                return new ASTCILLessThanVariableConstantNode((ASTCILExpressionNode) LessThan.Left.Accept(this),
                    right2.Value, LessThan.SymbolTable);
            return new ASTCILLessThanTwoVariablesNode((ASTCILExpressionNode) LessThan.Left.Accept(this),
                (ASTCILExpressionNode) LessThan.Right.Accept(this), LessThan.SymbolTable);
        }

        public ASTCILNode VisitLetIn(ASTLetInNode LetIn)
        {
            return new ASTCILBlockNode(LetIn.Declarations.SelectMany(x => new ASTCILExpressionNode[]
            {
                new ASTCILAssignmentNode(x.Id.Text, (ASTCILExpressionNode) x.Expression.Accept(this),LetIn.SymbolTable)
            }).Append((ASTCILExpressionNode) LetIn.LetExp.Accept(this)),LetIn.SymbolTable);
        }

        public ASTCILNode VisitMethod(ASTMethodNode Method)
        {
            var type = compilationUnit.TypeEnvironment.GetContextType(Method.SymbolTable);
            compilationUnit.MethodEnvironment.GetMethodOnIt(type, Method.Name, out var coolMethod);
            return new ASTCILFuncNode(labelIlGenerator.GenerateFunc(coolMethod.Type.Name, coolMethod.Name),
                new[] { (ASTCILExpressionNode) Method.Body.Accept(this) },
                Method.SymbolTable);
        }

        public ASTCILNode VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall)
        {
            return new ASTCILIfNode(
                new ASTCILIsVoidNode((ASTCILExpressionNode) MethodCall.InvokeOnExpresion.Accept(this),MethodCall.SymbolTable),
                new ASTCILRuntimeErrorNode(RuntimeErrors.DispatchOnVoid, MethodCall.SymbolTable), new ASTCILFuncStaticCallNode(
                    MethodCall.MethodName, MethodCall.Type.Text,
                    new[] { (ASTCILExpressionNode)MethodCall.InvokeOnExpresion.Accept(this)}
                        .Concat(MethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this))), MethodCall.SymbolTable),
                labelIlGenerator.GenerateIf(),MethodCall.SymbolTable);
        }

        public ASTCILNode VisitDynamicMethodCall(ASTDynamicMethodCallNode MethodCall)
        {
            return new ASTCILIfNode(
                new ASTCILIsVoidNode((ASTCILExpressionNode) MethodCall.InvokeOnExpresion.Accept(this),MethodCall.SymbolTable),
                new ASTCILRuntimeErrorNode(RuntimeErrors.DispatchOnVoid, MethodCall.SymbolTable), new ASTCILFuncVirtualCallNode(
                    MethodCall.MethodName,
                    new[] { (ASTCILExpressionNode)MethodCall.InvokeOnExpresion.Accept(this) }
                        .Concat(MethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this))),MethodCall.SymbolTable),
                labelIlGenerator.GenerateIf(),MethodCall.SymbolTable);
        }

        public ASTCILNode VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall)
        {
            return new ASTCILFuncVirtualCallNode(OwnMethodCall.Method.Text,
                    new[] { new ASTCILSelfNode(OwnMethodCall.SymbolTable) }
                    .Concat(OwnMethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this))),OwnMethodCall.SymbolTable);
        }

        public ASTCILNode VisitMinus(ASTMinusNode Minus)
        {
            if (Minus.Left is ASTIntConstantNode left && Minus.Right is ASTIntConstantNode right)
                return new ASTCILMinusTwoConstantNode(left.Value, right.Value,Minus.SymbolTable);
            if (Minus.Left is ASTIntConstantNode left2)
                return new ASTCILMinusConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) Minus.Right.Accept(this),Minus.SymbolTable);
            if (Minus.Right is ASTIntConstantNode right2)
                return new ASTCILMinusVariableConstantNode((ASTCILExpressionNode) Minus.Left.Accept(this),
                    right2.Value,Minus.SymbolTable);
            return new ASTCILMinusTwoVariablesNode((ASTCILExpressionNode) Minus.Left.Accept(this),
                (ASTCILExpressionNode) Minus.Right.Accept(this),Minus.SymbolTable);
        }

        public ASTCILNode VisitMultiply(ASTMultiplyNode Multiply)
        {
            if (Multiply.Left is ASTIntConstantNode left && Multiply.Right is ASTIntConstantNode right)
                return new ASTCILMultiplyTwoConstantNode(left.Value, right.Value,Multiply.SymbolTable);
            if (Multiply.Left is ASTIntConstantNode left2)
                return new ASTCILMultiplyConstantVariableNode(left2.Value,
                    (ASTCILExpressionNode) Multiply.Right.Accept(this),Multiply.SymbolTable);
            if (Multiply.Right is ASTIntConstantNode right2)
                return new ASTCILMultiplyVariableConstantNode((ASTCILExpressionNode) Multiply.Left.Accept(this),
                    right2.Value,Multiply.SymbolTable);
            return new ASTCILMultiplyTwoVariablesNode((ASTCILExpressionNode) Multiply.Left.Accept(this),
                (ASTCILExpressionNode) Multiply.Right.Accept(this),Multiply.SymbolTable);
        }

        public ASTCILNode VisitNegative(ASTNegativeNode Negative)
        {
            if (Negative.Expression is ASTIntConstantNode constant)
                return new ASTCILMultiplyTwoConstantNode(constant.Value, -1,Negative.SymbolTable);
            return new ASTCILMultiplyVariableConstantNode((ASTCILExpressionNode) Negative.Accept(this), -1,Negative.SymbolTable);
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
                new ASTCILAllocateNode(type,context.SymbolTable),
                new ASTCILFuncVirtualCallNode(labelIlGenerator.GenerateInit(type),Enumerable.Empty<ASTCILExpressionNode>(),context.SymbolTable)
            },context.SymbolTable);
        }

        public ASTCILNode VisitProgram(ASTProgramNode Program)
        {
            return new ASTCILProgramNode
            (
                Program.Clases.Select(x => (ASTCILTypeNode) x.Accept(this))
                    .Append(new ASTCILTypeNode(compilationUnit.TypeEnvironment.Int, Enumerable.Empty<SymbollInfo>(),
                        Enumerable.Empty<CoolMethod>(), Enumerable.Empty<ASTCILFuncNode>(), Program.SymbolTable))
                    .Append(new ASTCILTypeNode(compilationUnit.TypeEnvironment.Bool, Enumerable.Empty<SymbollInfo>(),
                        Enumerable.Empty<CoolMethod>(), Enumerable.Empty<ASTCILFuncNode>(), Program.SymbolTable))
                    .Append(new ASTCILTypeNode(compilationUnit.TypeEnvironment.IO, Enumerable.Empty<SymbollInfo>(),
                        compilationUnit.MethodEnvironment.GetVirtualTable(compilationUnit.TypeEnvironment.IO),
                        new ASTCILFuncNode[]
                        {
                            new ASTCILIOInIntNode(labelIlGenerator, Program.SymbolTable),
                            new ASTCILIOInStringNode(labelIlGenerator, Program.SymbolTable),
                            new ASTCILIOOutIntNode(labelIlGenerator, Program.SymbolTable),
                            new ASTCILIOOutStringNode(labelIlGenerator, Program.SymbolTable)
                        }, Program.SymbolTable))
                    .Append(new ASTCILTypeNode(compilationUnit.TypeEnvironment.Object,
                        Enumerable.Empty<SymbollInfo>(),
                        compilationUnit.MethodEnvironment.GetVirtualTable(compilationUnit.TypeEnvironment.Object),
                        new[]
                        {
                            //Abort function
                            new ASTCILFuncNode(labelIlGenerator.GenerateFunc(Types.Object, Functions.Abort),
                                new[] { new ASTCILRuntimeErrorNode(RuntimeErrors.ObjectAbort, Program.SymbolTable) },
                                Program.SymbolTable),
                            new ASTCILObjectTypeNameNode(labelIlGenerator, Program.SymbolTable),
                            new ASTCILObjectCopyNode(labelIlGenerator, Program.SymbolTable)
                        }, Program.SymbolTable))
                    .Append(new ASTCILTypeNode(compilationUnit.TypeEnvironment.String,
                        new[] { new SymbollInfo(Attributes.StringLength, Types.String, ObjectKind.Atribute), },
                        compilationUnit.MethodEnvironment.GetVirtualTable(compilationUnit.TypeEnvironment.String),
                        new []
                        {
                            new ASTCILFuncNode(labelIlGenerator.GenerateFunc(Types.String, Functions.Length), new[]
                            {
                                //Length function
                                new ASTCILGetAttrNode(Types.String, Attributes.StringLength, Program.SymbolTable),
                            }, Program.SymbolTable),
                        }, Program.SymbolTable)), Program.SymbolTable
            );
        }

        public ASTCILNode VisitAtribute(ASTAtributeNode Atribute)
        {
            if (Atribute.HasInit)
                return Atribute.Init.Accept(this);

            compilationUnit.TypeEnvironment.GetTypeForObject(Atribute.SymbolTable, Atribute.AttributeName,
                out var coolType);
            if (coolType.Equals(compilationUnit.TypeEnvironment.Int))
                return new ASTCILSetAttributeNode(Atribute.TypeName, Atribute.AttributeName,
                    new ASTCILIntConstantNode(0,Atribute.SymbolTable),Atribute.SymbolTable);
            if (coolType.Equals(compilationUnit.TypeEnvironment.String))
                return new ASTCILSetAttributeNode(Atribute.TypeName, Atribute.AttributeName,
                    new ASTCILStringConstantNode("",Atribute.SymbolTable, labelIlGenerator.GenerateEmptyStringData()),Atribute.SymbolTable);
            if (coolType.Equals(compilationUnit.TypeEnvironment.Bool))
                return new ASTCILSetAttributeNode(Atribute.TypeName, Atribute.AttributeName,
                    new ASTCILBoolConstantNode(false,Atribute.SymbolTable),Atribute.SymbolTable);
            return new ASTCILSetAttributeNode(Atribute.TypeName, Atribute.AttributeName, new ASTCILVoidNode(Atribute.SymbolTable),Atribute.SymbolTable);
        }

        public ASTCILNode VisitStringConstant(ASTStringConstantNode StringConstant)
        {
            return new ASTCILStringConstantNode(StringConstant.Value,StringConstant.SymbolTable, labelIlGenerator.GenerateStringData());
        }

        public ASTCILNode VisitWhile(ASTWhileNode While)
        {
            var ifLabel = labelIlGenerator.GenerateIf();
            return new ASTCILIfNode((ASTCILExpressionNode) While.Condition.Accept(this),
                new ASTCILBlockNode(new[]
                {
                    (ASTCILExpressionNode) While.Body.Accept(this), new ASTCILGotoNode(ifLabel.end,While.SymbolTable)
                },While.SymbolTable),
                new ASTCILVoidNode(While.SymbolTable), ifLabel,While.SymbolTable);
        }

        public ASTCILNode VisitId(ASTIdNode Id)
        {
            return new ASTCILIdNode(Id.Name,Id.SymbolTable);
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
