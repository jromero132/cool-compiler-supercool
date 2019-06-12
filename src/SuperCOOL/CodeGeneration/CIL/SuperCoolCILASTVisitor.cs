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
            return new ASTCILAddTwoVariablesNode((ASTCILExpressionNode) Add.Left.Accept(this),
                (ASTCILExpressionNode) Add.Right.Accept(this));
        }

        public ASTCILNode VisitAssignment(ASTAssingmentNode Assigment)
        {
            Assigment.SymbolTable.IsDefObject(Assigment.Id.Name, out var symbolInfo);

            return symbolInfo.Kind == ObjectKind.Atribute
                ? (ASTCILNode)new ASTCILSetAttributeNode(symbolInfo,
                    (ASTCILExpressionNode)Assigment.Expresion.Accept(this))
                : new ASTCILAssignmentNode(symbolInfo, (ASTCILExpressionNode) Assigment.Expresion.Accept(this));
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
            var caseExpressionType = Case.ExpressionCase.SemanticCheckResult.Type.Name;
            var caseExpressions = new List<ASTCILExpressionNode>
                { caseExpression, caseExpressionCheckVoid };
            while (caseExpressionType != null)
            {

                foreach (var caseSubExpression in Case.Cases)
                {
                    var idInfo=Case.SymbolTable.GetObject(caseSubExpression.Name.Text);
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
                                new ASTCILAssignmentNode(idInfo,
                                    caseExpression),
                                (ASTCILExpressionNode) caseSubExpression.Branch.Accept(this),
                                new ASTCILGotoNode(caseLabels.endOfCase),
                            }), new ASTCILBlockNode(Enumerable.Empty<ASTCILExpressionNode>()),
                        labelIlGenerator.GenerateIf()));
                }

                compilationUnit.TypeEnvironment.GetTypeDefinition(caseExpressionType, Case.SymbolTable, out var type);
                caseExpressionType = type.Parent?.Name;
            }

            caseExpressions.Add(new ASTCILRuntimeErrorNode(RuntimeErrors.CaseWithoutMatching));

            return new ASTCILBlockNode(caseExpressions);
        }

        public ASTCILNode VisitClass(ASTClassNode Class)
        {
            var attributesInit = Class.Atributes.Select(x => (ASTCILExpressionNode) x.Accept(this));
            
            compilationUnit.TypeEnvironment.GetTypeDefinition(Class.TypeName,Class.SymbolTable, out var type);
            compilationUnit.MethodEnvironment.GetMethodIfDef(type, Functions.Init,out var methodInit);

            var methods = Class.Methods.Select(x => (ASTCILFuncNode) x.Accept(this))
                .Append(new ASTCILFuncNode(labelIlGenerator.GenerateInit(Class.TypeName),methodInit,
                    attributesInit));

            var virtualTable = compilationUnit.MethodEnvironment.GetVirtualTable(type);
            return new ASTCILTypeNode(type,virtualTable, methods);
        }

        public ASTCILNode VisitDivision(ASTDivideNode Division)
        {
            return new ASTCILDivideTwoVariablesNode((ASTCILExpressionNode) Division.Left.Accept(this),
                new ASTCILIfNode(
                    (ASTCILExpressionNode) new ASTEqualNode
                    {
                        Left = Division.Right,
                        Right = new ASTIntConstantNode { Value = 0, SymbolTable = Division.SymbolTable },
                        SymbolTable = Division.SymbolTable
                    }.Accept(this), new ASTCILRuntimeErrorNode(RuntimeErrors.DivisionBy0),
                    (ASTCILExpressionNode) Division.Right.Accept(this), labelIlGenerator.GenerateIf()));
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
                        { Left = LessEqual.Left, Right = LessEqual.Right }).Accept(this)),
                new ASTCILIntConstantNode(1),
                new ASTCILIntConstantNode(0), labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitLessThan(ASTLessThanNode LessThan)
        {
            return new ASTCILLessThanTwoVariablesNode((ASTCILExpressionNode) LessThan.Left.Accept(this),
                (ASTCILExpressionNode) LessThan.Right.Accept(this));
        }

        public ASTCILNode VisitLetIn(ASTLetInNode LetIn)
        {
            return new ASTCILBlockNode(LetIn.Declarations.SelectMany(x => {
                return new ASTCILExpressionNode[]{
                                new ASTCILAssignmentNode(x.Id,
                                    x.Expression!=null?(ASTCILExpressionNode) x.Expression?.Accept(this) :
                                    x.Type==compilationUnit.TypeEnvironment.Bool? new ASTCILBoolConstantNode(false):
                                    x.Type==compilationUnit.TypeEnvironment.Int? new ASTCILIntConstantNode(0):
                                    (ASTCILExpressionNode)new ASTCILVoidNode()
                                    )};
            }).Append((ASTCILExpressionNode)LetIn.LetExp.Accept(this))); 
        }

        public ASTCILNode VisitMethod(ASTMethodNode Method)
        {
            var type = compilationUnit.TypeEnvironment.GetContextType(Method.SymbolTable);
            compilationUnit.MethodEnvironment.GetMethodOnIt(type, Method.Name, out var coolMethod);
            return new ASTCILFuncNode(labelIlGenerator.GenerateFunc(coolMethod.Type.Name, coolMethod.Name), coolMethod,
                new[] { (ASTCILExpressionNode) Method.Body.Accept(this) });
        }

        public ASTCILNode VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall)
        {
            compilationUnit.TypeEnvironment.GetTypeDefinition(MethodCall.MethodName, null, out var type);
            return new ASTCILIfNode(
                new ASTCILIsVoidNode((ASTCILExpressionNode) MethodCall.InvokeOnExpresion.Accept(this)),
                new ASTCILRuntimeErrorNode(RuntimeErrors.DispatchOnVoid), new ASTCILFuncStaticCallNode(
                    MethodCall.MethodName,type,
                    new[] { (ASTCILExpressionNode)MethodCall.InvokeOnExpresion.Accept(this)}
                        .Concat(MethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this)))),
                labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitDynamicMethodCall(ASTDynamicMethodCallNode MethodCall)
        {
            return new ASTCILIfNode(
                new ASTCILIsVoidNode((ASTCILExpressionNode) MethodCall.InvokeOnExpresion.Accept(this)),
                new ASTCILRuntimeErrorNode(RuntimeErrors.DispatchOnVoid), new ASTCILFuncVirtualCallNode(compilationUnit.TypeEnvironment.GetContextType(MethodCall.SymbolTable),
                    MethodCall.MethodName,
                    new[] { (ASTCILExpressionNode)MethodCall.InvokeOnExpresion.Accept(this) }
                        .Concat(MethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this)))),
                labelIlGenerator.GenerateIf());
        }

        public ASTCILNode VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall)
        {
            var self=compilationUnit.TypeEnvironment.GetContextType(OwnMethodCall.SymbolTable);
            return new ASTCILFuncVirtualCallNode(compilationUnit.TypeEnvironment.GetContextType(OwnMethodCall.SymbolTable),
                    OwnMethodCall.Method.Text,
                    new[] { new ASTCILSelfNode() }
                    .Concat(OwnMethodCall.Arguments.Select(a => (ASTCILExpressionNode) a.Accept(this))));
        }

        public ASTCILNode VisitMinus(ASTMinusNode Minus)
        {
            return new ASTCILMinusTwoVariablesNode((ASTCILExpressionNode) Minus.Left.Accept(this),
                (ASTCILExpressionNode) Minus.Right.Accept(this));
        }

        public ASTCILNode VisitMultiply(ASTMultiplyNode Multiply)
        {
            return new ASTCILMultiplyTwoVariablesNode((ASTCILExpressionNode) Multiply.Left.Accept(this),
                (ASTCILExpressionNode) Multiply.Right.Accept(this));
        }

        public ASTCILNode VisitNegative(ASTNegativeNode Negative)
        {
            return new ASTCILMultiplyTwoVariablesNode((ASTCILExpressionNode) Negative.Expression.Accept(this),
                new ASTCILIntConstantNode(-1));
        }

        public ASTCILNode VisitNew(ASTNewNode context)
        {
            var type = context.SemanticCheckResult.Type;
            if (context.SemanticCheckResult.Type is SelfType selftype)
                type = selftype.ContextType;

            return new ASTCILBlockNode(new ASTCILExpressionNode[]
            {
                new ASTCILAllocateNode(type),
                new ASTCILFuncVirtualCallNode(type,labelIlGenerator.GenerateInit(type.Name),Enumerable.Empty<ASTCILExpressionNode>())
            });
        }

        public ASTCILNode VisitProgram(ASTProgramNode Program)
        {
            return new ASTCILProgramNode
            (
                Program.Clases.Select(x => (ASTCILTypeNode)x.Accept(this))
                    .Append(new ASTCILTypeNode(compilationUnit.TypeEnvironment.Int,
                        compilationUnit.MethodEnvironment.GetVirtualTable(compilationUnit.TypeEnvironment.Int), Enumerable.Empty<ASTCILFuncNode>()))
                    .Append(new ASTCILTypeNode(compilationUnit.TypeEnvironment.Bool,
                        compilationUnit.MethodEnvironment.GetVirtualTable(compilationUnit.TypeEnvironment.Bool), Enumerable.Empty<ASTCILFuncNode>()))
                    .Append(new ASTCILTypeNode(compilationUnit.TypeEnvironment.IO,
                        compilationUnit.MethodEnvironment.GetVirtualTable(compilationUnit.TypeEnvironment.IO),
                        new ASTCILFuncNode[]
                        {
                            new ASTCILIOInIntNode(compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.IO,Functions.InInt), labelIlGenerator),
                            new ASTCILIOInStringNode(compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.IO,Functions.InString),labelIlGenerator),
                            new ASTCILIOOutIntNode(compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.IO,Functions.OutInt),labelIlGenerator),
                            new ASTCILIOOutStringNode(compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.IO,Functions.OutString),labelIlGenerator),
                            new ASTCILFuncNode(labelIlGenerator.GenerateInit(compilationUnit.TypeEnvironment.IO.Name),compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.IO,Functions.Init),
                                                new[]{new ASTCILBlockNode(Enumerable.Empty<ASTCILExpressionNode>())})
                        }))
                    .Append(new ASTCILTypeNode(compilationUnit.TypeEnvironment.Object,
                        compilationUnit.MethodEnvironment.GetVirtualTable(compilationUnit.TypeEnvironment.Object),
                        new[]
                        {
                            //Abort function
                            new ASTCILFuncNode(labelIlGenerator.GenerateFunc(Types.Object, Functions.Abort),
                                        compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.Object,Functions.Abort),
                                new[] { new ASTCILRuntimeErrorNode(RuntimeErrors.ObjectAbort)}),
                            new ASTCILObjectTypeNameNode(compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.Object,Functions.Type_Name),labelIlGenerator),
                            new ASTCILObjectCopyNode(compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.Object,Functions.Copy),labelIlGenerator),
                            new ASTCILFuncNode(labelIlGenerator.GenerateInit(compilationUnit.TypeEnvironment.Object.Name),
                                                compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.Object,Functions.Init),
                                                new[]{new ASTCILBlockNode(Enumerable.Empty<ASTCILExpressionNode>())})
                        }))
                    .Append(new ASTCILTypeNode(compilationUnit.TypeEnvironment.String,
                        compilationUnit.MethodEnvironment.GetVirtualTable(compilationUnit.TypeEnvironment.String),
                        new []
                        {
                            //Length function
                            new ASTCILFuncNode(labelIlGenerator.GenerateFunc(Types.String, Functions.Length),
                            compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.String,Functions.Length),
                            new[]
                            {
                                new ASTCILGetAttrNode(compilationUnit.TypeEnvironment.String.SymbolTable.GetObject( Attributes.StringLength))
                            }),
                            new ASTCILStringConcatNode(compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.String,Functions.Concat),labelIlGenerator),
                            new ASTCILStringSubStrNode(compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.String,Functions.Substr),labelIlGenerator),
                            new ASTCILFuncNode(labelIlGenerator.GenerateInit(compilationUnit.TypeEnvironment.String.Name),
                                                compilationUnit.MethodEnvironment.GetMethod(compilationUnit.TypeEnvironment.String,Functions.Init),
                                                new[]{new ASTCILBlockNode(Enumerable.Empty<ASTCILExpressionNode>())})
                        })));
        }

        public ASTCILNode VisitAtribute(ASTAtributeNode Atribute)
        {
            if (Atribute.HasInit)
                return Atribute.Init.Accept(this);

            var atribute=Atribute.SymbolTable.GetObject(Atribute.AttributeName);
            if (atribute.Type.Equals(compilationUnit.TypeEnvironment.Int.Name))
                return new ASTCILSetAttributeNode(atribute,
                    new ASTCILIntConstantNode(0));
            if (atribute.Equals(compilationUnit.TypeEnvironment.String.Name))
                return new ASTCILSetAttributeNode(atribute,
                    new ASTCILStringConstantNode("", labelIlGenerator.GenerateEmptyStringData()));
            if (atribute.Equals(compilationUnit.TypeEnvironment.Bool.Name))
                return new ASTCILSetAttributeNode(atribute,
                    new ASTCILBoolConstantNode(false));
            return new ASTCILSetAttributeNode(atribute, new ASTCILVoidNode());
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
                new ASTCILVoidNode(), ifLabel);
        }

        public ASTCILNode VisitId(ASTIdNode Id)
        {
             Id.SymbolTable.IsDefObject(Id.Name, out var info);
            if (info.Kind == ObjectKind.Atribute)
                return new ASTCILGetAttrNode(info);
            return new ASTCILIdNode(info);
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
