using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using SuperCOOL.ANTLR;
using SuperCOOL.SemanticCheck.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.ANTLR
{
    public class SuperCoolASTGeneratorVisitor : SuperCOOLBaseVisitor<ASTNode>
    {
        public override ASTNode VisitAdd([NotNull] SuperCOOLParser.AddContext context)
        {
            var result=new ASTAddNode() { };
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            left.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            right.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            result.Left = left;
            result.Right = right;

            return result;
        }

        public override ASTNode VisitAssignment([NotNull] SuperCOOLParser.AssignmentContext context)
        {
            var result = new ASTAssingmentNode();
            ASTExpressionNode exp = (ASTExpressionNode)context.expression().Accept(this);
            exp.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            ASTIdNode id = new ASTIdNode() { Name= context.OBJECTID().Symbol.Text};
            id.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            result.Id = id;
            result.Expresion = exp;

            return result;
        }

        public override ASTNode VisitBlock([NotNull] SuperCOOLParser.BlockContext context)
        {
            var result = new ASTBlockNode();
            var exps = context.expression();
            ASTExpressionNode[] exp=new ASTExpressionNode[exps.Length];
            for (int i = 0; i < exps.Length; i++)
            {
                exp[i] = (ASTExpressionNode)exps[i].Accept(this);
                exp[i].TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            }
            result.Expresions = exp;
            return result;
        }

        public override ASTNode VisitBoolNot([NotNull] SuperCOOLParser.BoolNotContext context)
        {
            var result = new ASTBoolNotNode();
            var expresion=(ASTExpressionNode)context.expression().Accept(this);
            expresion.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            result.Expresion = expresion;
            return result;
        }

        public override ASTNode VisitCase([NotNull] SuperCOOLParser.CaseContext context)
        {
            var result = new ASTCaseNode();
            var expCases = (ASTExpressionNode)context.expression()[0].Accept(this);
            expCases.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            var cases = new (string, string, ASTExpressionNode)[context.expression().Length-1];
            for (int i = 1; i < context.expression().Length; i++)
            {
                cases[i] = (context.OBJECTID(i-1).Symbol.Text,context.TYPEID(i-1).Symbol.Text,(ASTExpressionNode)context.expression(i).Accept(this));
                cases[i].Item3.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            }
            result.ExpressionCase = expCases;
            result.Cases = cases;
            return result;
        }

        public override ASTNode VisitClassDefine([NotNull] SuperCOOLParser.ClassDefineContext context)
        {
            var result = new ASTClassNode();
            var methods = new List<ASTMethodNode>();
            var atributes = new List<ASTAtributeNode>();
            foreach (var item in context.feature())
                switch (item.Accept(this))
                {
                    case ASTMethodNode method :
                        method.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
                        methods.Add(method);
                        result.TypeEnvironment.AddMethod(method.Name, method.Formals.ConvertAll((x) => x.Type));
                        break;
                    case ASTAtributeNode atribute:
                        atribute.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
                        atributes.Add(atribute);
                        result.TypeEnvironment.AddObject(atribute.Name, atribute.Type);
                        break;
                }
            result.TypeName = context.TYPEID(0).Symbol.Text;
            result.ParentTypeName = context.TYPEID(0).Symbol.Text;
            result.Methods = methods;
            result.Atributes = atributes;
            return  result;
        }

        public override ASTNode VisitDivision([NotNull] SuperCOOLParser.DivisionContext context)
        {
            var result = new ASTDivideNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            left.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            right.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;

            result.Left = left;
            result.Right = right;
            return result;
        }

        public override ASTNode VisitEqual([NotNull] SuperCOOLParser.EqualContext context)
        {
            var result = new ASTEqualNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            left.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            right.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;

            result.Left = left;
            result.Right = right;
            return result;
        }

        public override ASTNode VisitErrorNode([NotNull] IErrorNode node)
        {
            throw new NotImplementedException();
        }

        public override ASTNode VisitFalse([NotNull] SuperCOOLParser.FalseContext context)
        {
            return new ASTBoolConstantNode() {Value=false };
        }

        public override ASTNode VisitIf([NotNull] SuperCOOLParser.IfContext context)
        {
            var result = new ASTIfNode();
            var cond = (ASTExpressionNode)context.expression(0).Accept(this);
            cond.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            var then = (ASTExpressionNode)context.expression(1).Accept(this);
            then.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            var @else = (ASTExpressionNode)context.expression(2).Accept(this);
            @else.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;

            result.Condition = cond;
            result.Then = then;
            result.Else = @else;

            return result;
        }

        public override ASTNode VisitInt([NotNull] SuperCOOLParser.IntContext context)
        {
            var value = int.Parse(context.INT().Symbol.Text);
            return new ASTIntConstantNode() {Value=value };
        }

        public override ASTNode VisitIsvoid([NotNull] SuperCOOLParser.IsvoidContext context)
        {
            var result = new ASTIsVoidNode();
            var expression = context.expression().Accept(this);
            expression.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;

            result.Expression = expression;
            return  result;
        }

        public override ASTNode VisitLessEqual([NotNull] SuperCOOLParser.LessEqualContext context)
        {
            var result = new ASTLessEqualNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            left.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            right.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;

            result.Left = left;
            result.Right = right;
            return result;
        }

        public override ASTNode VisitLessThan([NotNull] SuperCOOLParser.LessThanContext context)
        {
            var result = new ASTLessThanNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            left.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            right.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;

            result.Left = left;
            result.Right = right;
            return result;
        }

        public override ASTNode VisitLetIn([NotNull] SuperCOOLParser.LetInContext context)
        {
            var result = new ASTLetInNode();
            var expresions = context.expression();
            var declarations = new (string, string, ASTExpressionNode)[expresions.Length - 1];
            var letExp = (ASTExpressionNode)expresions[expresions.Length - 1].Accept(this);
            letExp.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            for (int i = 0; i < expresions.Length - 1; i++)
            {
                declarations[i] = (context.OBJECTID(i).Symbol.Text,context.TYPEID(i).Symbol.Text,(ASTExpressionNode)context.expression(i).Accept(this));
                declarations[i].Item3.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
                letExp.TypeEnvironment.AddObject(declarations[i].Item1, declarations[i].Item2);
            }

            result.Declarations = declarations;
            result.LetExp = letExp;
            return result;
        }

        public override ASTNode VisitMethod([NotNull] SuperCOOLParser.MethodContext context)
        {
            var result = new ASTMethodNode();
            var body = (ASTExpressionNode)context.expression().Accept(this);
            body.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            var formals = new List<ASTFormalNode>();
            foreach (var formal in context.formal())
                formals.Add((ASTFormalNode)formal.Accept(this));

            result.Name = context.OBJECTID().Symbol.Text;
            result.Body = body;
            result.ReturnType = context.TYPEID().Symbol.Text;
            result.Formals = formals;
            return result;
        }

        public override ASTNode VisitMethodCall([NotNull] SuperCOOLParser.MethodCallContext context)
        {
            var result = new ASTMethodCallNode();
            var expresions = context.expression();
            var type = context.TYPEID().Symbol.Text;
            var methodName = context.OBJECTID().Symbol.Text;
            var invokeOnExpresion = (ASTExpressionNode)expresions[0].Accept(this);
            invokeOnExpresion.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            var arguments = new ASTExpressionNode[expresions.Length-1];
            for (int i = 1; i < expresions.Length; i++)
            {
                arguments[i] = (ASTExpressionNode)expresions[i].Accept(this);
                arguments[i].TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            }
            result.MethodName = methodName;
            result.Type = type;
            result.InvokeOnExpresion = invokeOnExpresion;
            result.Arguments = arguments;

            return result;
        }

        public override ASTNode VisitMinus([NotNull] SuperCOOLParser.MinusContext context)
        {
            var result = new ASTMinusNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            left.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            right.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;

            result.Left = left;
            result.Right = right;

            return  result;
        }

        public override ASTNode VisitMultiply([NotNull] SuperCOOLParser.MultiplyContext context)
        {
            var result = new ASTMultiplyNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            left.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            right.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;

            result.Left = left;
            result.Right = right;
            return result;
        }

        public override ASTNode VisitNegative([NotNull] SuperCOOLParser.NegativeContext context)
        {
            var result=new ASTNegativeNode();
            var exp = context.expression().Accept(this);
            exp.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            result.Expression =exp;
            return result;
        }

        public override ASTNode VisitNew([NotNull] SuperCOOLParser.NewContext context)
        {
            return new ASTNewNode() { Type = context.TYPEID().Symbol.Text };
        }

        public override ASTNode VisitOwnMethodCall([NotNull] SuperCOOLParser.OwnMethodCallContext context)
        {
            var result = new ASTOwnMethodCallNode();

            var exps = context.expression();
            var arguments = new ASTExpressionNode[exps.Length];
            for (int i = 0; i < exps.Length; i++)
            {
                arguments[i] = (ASTExpressionNode)exps[i].Accept(this);
                arguments[i].TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            }

            result.Method = context.OBJECTID().Symbol.Text;
            result.Arguments=arguments ;
            return result;
        }

        public override ASTNode VisitParentheses([NotNull] SuperCOOLParser.ParenthesesContext context)
        {
            return context.expression().Accept(this);
        }

        public override ASTNode VisitProgram([NotNull] SuperCOOLParser.ProgramContext context)
        {
            return VisitProgramBlocks(context.programBlocks());
        }

        public override ASTNode VisitClasses([NotNull] SuperCOOLParser.ClassesContext context)
        {
            var program = (ASTProgramNode)context.programBlocks().Accept(this);
            var classe = (ASTClassNode)context.classDefine().Accept(this);
            program.Clases.Add(classe);
            return program;
        }

        public override ASTNode VisitEof([NotNull] SuperCOOLParser.EofContext context)
        {
            return new ASTProgramNode();
        }

        public override ASTNode VisitProperty([NotNull] SuperCOOLParser.PropertyContext context)
        {
            var result = new ASTAtributeNode();
            var init = (ASTExpressionNode)context.expression().Accept(this);
            init.TypeEnvironment.ParentEnvironment = init.TypeEnvironment;

            result.Name = context.OBJECTID().Symbol.Text;
            result.Type = context.TYPEID().Symbol.Text;

            return result;
        }

        public override ASTNode VisitString([NotNull] SuperCOOLParser.StringContext context)
        {
            return new ASTStringConstantNode() { Value = context.STRING().Symbol.Text };
        }

        public override ASTNode VisitTrue([NotNull] SuperCOOLParser.TrueContext context)
        {
            return new ASTBoolConstantNode() { Value = true };
        }

        public override ASTNode VisitWhile([NotNull] SuperCOOLParser.WhileContext context)
        {
            var result = new ASTWhileNode();
            var condition= context.expression(1).Accept(this);
            condition.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;
            var body = context.expression(1).Accept(this);
            body.TypeEnvironment.ParentEnvironment = result.TypeEnvironment;

            result.Condition = condition;
            result.Body = body;
            return result;
        }

        public override ASTNode VisitId([NotNull] SuperCOOLParser.IdContext context)
        {
            return new ASTIdNode() { Name = context.OBJECTID().Symbol.Text };
        }

        public override ASTNode VisitFormal([NotNull] SuperCOOLParser.FormalContext context)
        {
            return new ASTFormalNode() { Name = context.OBJECTID().Symbol.Text, Type = context.TYPEID().Symbol.Text };
        }
    }
}
