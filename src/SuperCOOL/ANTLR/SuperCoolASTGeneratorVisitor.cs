using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using SuperCOOL.ANTLR;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.ANTLR
{
    public class SuperCoolASTGeneratorVisitor : SuperCOOLBaseVisitor<ASTNode>
    {
        SymbolTable CurrentTable;
        public List<(string type, string parent)> Types;
        public List<(string type,string method, string[] asrgTypes, string returnType)> Functions;

        public SuperCoolASTGeneratorVisitor()
        {
            CurrentTable = new SymbolTable();
            Types = new List<(string type, string parent)>();
            Functions = new List<(string type, string method, string[] asrgTypes, string returnType)>();
        }

        public override ASTNode VisitAdd([NotNull] SuperCOOLParser.AddContext context)
        {
            var result=new ASTAddNode() { };
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            AssignSymbolTable(left);
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            AssignSymbolTable(right);
            result.Left = left;
            result.Right = right;
            result.AddToken = context.ADD().Symbol;
            return result;
        }

        public override ASTNode VisitAssignment([NotNull] SuperCOOLParser.AssignmentContext context)
        {
            var result = new ASTAssingmentNode();
            ASTExpressionNode exp = (ASTExpressionNode)context.expression().Accept(this);
            AssignSymbolTable(exp);
            ASTIdNode id = new ASTIdNode() { Token = context.OBJECTID().Symbol};
            AssignSymbolTable(id);
            result.Id = id;
            result.Expresion = exp;
            result.AssigmentToken = context.ASSIGNMENT().Symbol;
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
                AssignSymbolTable(exp[i]);
            }
            result.Expresions = exp;
            return result;
        }

        public override ASTNode VisitBoolNot([NotNull] SuperCOOLParser.BoolNotContext context)
        {
            var result = new ASTBoolNotNode();
            var expresion=(ASTExpressionNode)context.expression().Accept(this);
            AssignSymbolTable(expresion);
            result.Expresion = expresion;
            result.NotToken = context.NOT().Symbol;
            return result;
        }

        public override ASTNode VisitCase([NotNull] SuperCOOLParser.CaseContext context)
        {
            var result = new ASTCaseNode();
            var expCases = (ASTExpressionNode)context.expression()[0].Accept(this);
            AssignSymbolTable(expCases);
            var cases = new (IToken, IToken, ASTExpressionNode)[context.expression().Length-1];
            for (int i = 1; i < context.expression().Length; i++)
            {
                var objectName = context.OBJECTID(i - 1).Symbol;
                var typename = context.TYPEID(i - 1).Symbol;
                EnterScope();
                CurrentTable.DefObject(objectName.Text, typename.Text);
                cases[i] = (objectName,typename,(ASTExpressionNode)context.expression(i).Accept(this));
                AssignSymbolTable(cases[i].Item3);
                ExitScope();
            }
            result.ExpressionCase = expCases;
            result.Cases = cases;
            return result;
        }

        public override ASTNode VisitClassDefine([NotNull] SuperCOOLParser.ClassDefineContext context)
        {
            var className = context.TYPEID(0).Symbol.Text;
            var result = new ASTClassNode();
            var methods = new List<ASTMethodNode>();
            var atributes = new List<ASTAtributeNode>();
            CurrentTable.DefObject("self", className);
            foreach (var item in context.feature())
                switch (item.RuleIndex)//TODO :verify this to doit better
                {
                    case 2  ://method
                        EnterScope();
                        var method = (ASTMethodNode)item.Accept(this);
                        methods.Add(method);
                        AssignSymbolTable(method);
                        DefFunc(className,method.Name, method.Formals.Select(x => x.type.Text).ToArray(), method.ReturnType);
                        ExitScope();
                        break;
                    case 3 ://property
                        var atribute = (ASTAtributeNode)item.Accept(this);
                        atributes.Add(atribute);
                        AssignSymbolTable(atribute);
                        CurrentTable.DefObject(atribute.AttributeName,atribute.TypeName);
                        break;
                }
            result.Type = context.TYPEID(0).Symbol;
            result.ParentType = context.TYPEID(1)?.Symbol??null;
            result.Methods = methods;
            result.Atributes = atributes;
            return result;
        }

        public override ASTNode VisitDivision([NotNull] SuperCOOLParser.DivisionContext context)
        {
            var result = new ASTDivideNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            AssignSymbolTable(left);
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            AssignSymbolTable(right);

            result.Left = left;
            result.Right = right;
            result.DivToken = context.DIVISION().Symbol;
            return result;
        }

        public override ASTNode VisitEqual([NotNull] SuperCOOLParser.EqualContext context)
        {
            var result = new ASTEqualNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            AssignSymbolTable(left);
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            AssignSymbolTable(right);

            result.Left = left;
            result.Right = right;
            result.EqualToken = context.EQUAL().Symbol;
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
            AssignSymbolTable(cond);
            var then = (ASTExpressionNode)context.expression(1).Accept(this);
            AssignSymbolTable(then);
            var @else = (ASTExpressionNode)context.expression(2).Accept(this);
            AssignSymbolTable(@else);

            result.Condition = cond;
            result.Then = then;
            result.Else = @else;
            result.IfToken = context.IF().Symbol;
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
            AssignSymbolTable(expression);

            result.Expression = expression;
            return  result;
        }

        public override ASTNode VisitLessEqual([NotNull] SuperCOOLParser.LessEqualContext context)
        {
            var result = new ASTLessEqualNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            AssignSymbolTable(left);
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            AssignSymbolTable(right);

            result.Left = left;
            result.Right = right;
            result.LessEqualToken = context.LESS_EQUAL().Symbol;
            return result;
        }

        public override ASTNode VisitLessThan([NotNull] SuperCOOLParser.LessThanContext context)
        {
            var result = new ASTLessThanNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            AssignSymbolTable(left);
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            AssignSymbolTable(right);

            result.Left = left;
            result.Right = right;
            result.LessThanToken = context.LESS_THAN().Symbol;
            return result;
        }

        public override ASTNode VisitLetIn([NotNull] SuperCOOLParser.LetInContext context)
        {
            var result = new ASTLetInNode();
            var expresions = context.expression();
            var declarations = new (IToken, IToken, ASTExpressionNode)[expresions.Length - 1];
            for (int i = 0; i < expresions.Length - 1; i++)
            {
                declarations[i] = (context.OBJECTID(i)?.Symbol,context.TYPEID(i).Symbol,(ASTExpressionNode)context.expression(i).Accept(this));
                AssignSymbolTable(declarations[i].Item3);
            }

            EnterScope();
            foreach (var declaration in declarations)
                CurrentTable.DefObject(declaration.Item1.Text,declaration.Item2.Text);

            var letExp = (ASTExpressionNode)expresions[expresions.Length - 1].Accept(this);
            AssignSymbolTable(letExp);
            ExitScope();

            result.Declarations = declarations;
            result.LetExp = letExp;
            return result;
        }

        public override ASTNode VisitMethod([NotNull] SuperCOOLParser.MethodContext context)
        {
            var result = new ASTMethodNode();
            var formals = context.formal().Select(x=>(x.OBJECTID().Symbol,x.TYPEID().Symbol)).ToList();
            EnterScope();
            foreach (var item in formals)
                CurrentTable.DefObject(item.Item1.Text,item.Item2.Text);

            var body = (ASTExpressionNode)context.expression().Accept(this);
            AssignSymbolTable(body);
            ExitScope();

            result.Method = context.OBJECTID().Symbol;
            result.Body = body;
            result.Return = context.TYPEID().Symbol;
            result.Formals = formals;
            return result;
        }

        public override ASTNode VisitMethodCall([NotNull] SuperCOOLParser.MethodCallContext context)
        {
            if (context.TYPEID() != null)
            {
                var result = new ASTStaticMethodCallNode();
                var expresions = context.expression();
                var invokeOnExpresion = (ASTExpressionNode)expresions[0].Accept(this);
                AssignSymbolTable(invokeOnExpresion);
                var methodName = context.OBJECTID().Symbol;
                var arguments = new ASTExpressionNode[expresions.Length - 1];
                for (int i = 1; i < expresions.Length; i++)
                {
                    arguments[i] = (ASTExpressionNode)expresions[i].Accept(this);
                    AssignSymbolTable(arguments[i]);
                }
                var type = context.TYPEID().Symbol;
                result.Method = methodName;
                result.Type= type;
                result.InvokeOnExpresion = invokeOnExpresion;
                result.Arguments = arguments;

                return result;
            }
            var dresult = new ASTDynamicMethodCallNode();
            var dexpresions = context.expression();
            var dinvokeOnExpresion = (ASTExpressionNode)dexpresions[0].Accept(this);
            AssignSymbolTable(dinvokeOnExpresion);
            var dmethodName = context.OBJECTID().Symbol;
            var darguments = new ASTExpressionNode[dexpresions.Length - 1];
            for (int i = 0; i < darguments.Length; i++)
            {
                darguments[i] = (ASTExpressionNode)dexpresions[i+1].Accept(this);
                AssignSymbolTable(darguments[i]);
            }
            dresult.Method = dmethodName;
            dresult.InvokeOnExpresion = dinvokeOnExpresion;
            dresult.Arguments = darguments;

            return dresult;
        }

        public override ASTNode VisitMinus([NotNull] SuperCOOLParser.MinusContext context)
        {
            var result = new ASTMinusNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            AssignSymbolTable(left);
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            AssignSymbolTable(right);

            result.Left = left;
            result.Right = right;
            result.MinusToken = context.MINUS().Symbol;
            return  result;
        }

        public override ASTNode VisitMultiply([NotNull] SuperCOOLParser.MultiplyContext context)
        {
            var result = new ASTMultiplyNode();
            ASTExpressionNode left = (ASTExpressionNode)context.expression()[0].Accept(this);
            AssignSymbolTable(left);
            ASTExpressionNode right = (ASTExpressionNode)context.expression()[1].Accept(this);
            AssignSymbolTable(right);

            result.Left = left;
            result.Right = right;
            result.MultToken = context.MULTIPLY().Symbol;
            return result;
        }

        public override ASTNode VisitNegative([NotNull] SuperCOOLParser.NegativeContext context)
        {
            var result=new ASTNegativeNode();
            var exp = context.expression().Accept(this);
            AssignSymbolTable(exp);
            result.Expression =exp;
            result.NegativeToken = context.INTEGER_NEGATIVE().Symbol;
            return result;
        }

        public override ASTNode VisitNew([NotNull] SuperCOOLParser.NewContext context)
        {
            return new ASTNewNode() { Type = context.TYPEID().Symbol };
        }

        public override ASTNode VisitOwnMethodCall([NotNull] SuperCOOLParser.OwnMethodCallContext context)
        {
            var result = new ASTOwnMethodCallNode();

            var exps = context.expression();
            var arguments = new ASTExpressionNode[exps.Length];
            for (int i = 0; i < exps.Length; i++)
            {
                arguments[i] = (ASTExpressionNode)exps[i].Accept(this);
                AssignSymbolTable(arguments[i]);
            }

            result.Method = context.OBJECTID().Symbol;
            result.Arguments=arguments ;
            return result;
        }

        public override ASTNode VisitParentheses([NotNull] SuperCOOLParser.ParenthesesContext context)
        {
            return context.expression().Accept(this);
        }
        public override ASTNode VisitClasses([NotNull] SuperCOOLParser.ClassesContext context)
        {
            var program =new ASTProgramNode();
            var clases = context.classDefine();
            var ProgramTable = new SymbolTable();
            foreach (var item in clases)
            {
                DefType(item.TYPEID(0).Symbol.Text, item.TYPEID(1)?.Symbol.Text);
                EnterScope();
                var classe = (ASTClassNode)item.Accept(this);
                AssignSymbolTable(classe);
                program.Clases.Add(classe);
                ExitScope();
            }

            AssignSymbolTable(program);
            return program;
        }

        public override ASTNode VisitEof([NotNull] SuperCOOLParser.EofContext context)
        {
            var prog= new ASTProgramNode();
            AssignSymbolTable(prog);
            return prog;
        }

        public override ASTNode VisitProperty([NotNull] SuperCOOLParser.PropertyContext context)
        {
            var result = new ASTAtributeNode();
            var init = (ASTExpressionNode)context.expression().Accept(this);
            AssignSymbolTable(init);

            result.Attribute = context.OBJECTID().Symbol;
            result.Type = context.TYPEID().Symbol;

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
            AssignSymbolTable(condition);
            var body = context.expression(1).Accept(this);
            AssignSymbolTable(body);

            result.Condition = condition;
            result.Body = body;
            return result;
        }

        public override ASTNode VisitId([NotNull] SuperCOOLParser.IdContext context)
        {
            return new ASTIdNode() { Token = context.OBJECTID().Symbol };
        }

        private void AssignSymbolTable(ASTNode node)
        {
            node.SymbolTable = CurrentTable;
        }

        private void EnterScope()
        {
            CurrentTable =(SymbolTable)CurrentTable.EnterScope();
        }

        private void ExitScope()
        {
            CurrentTable = (SymbolTable)CurrentTable.ExitScope();
        }

        private void DefType(string name, string parent)
        {
            Types.Add((name, parent));
        }

        private void DefFunc(string type,string name, string[] argsTypes, string returnType)
        {
            Functions.Add((type,name, argsTypes, returnType));
        }
    }
}
