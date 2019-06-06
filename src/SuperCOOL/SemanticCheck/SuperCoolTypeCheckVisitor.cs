using Antlr4.Runtime.Misc;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck.AST;
using System;

namespace SuperCOOL.SemanticCheck
{
    public class SuperCoolTypeCheckVisitor : ISuperCoolASTVisitor<SemanticCheckResult>
    {
        public SuperCoolTypeCheckVisitor(CompilationUnit compilationUnit)
        {
            CompilationUnit = compilationUnit;
        }

        public CompilationUnit CompilationUnit { get; }

        public SemanticCheckResult VisitAdd(ASTAddNode Add)
        {
            var left = Add.Left.Accept(this);
            var right = Add.Right.Accept(this);

            Add.SemanticCheckResult.Ensure(left,left.Type==CompilationUnit.TypeEnvironment.Int,"Left Expresion must have type Int");
            Add.SemanticCheckResult.Ensure(right,right.Type==CompilationUnit.TypeEnvironment.Int,"Right Expresion must have type Int");
            Add.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return Add.SemanticCheckResult;
        }

        public SemanticCheckResult VisitAssignment(ASTAssingmentNode Assingment)
        {
            var expresult = Assingment.Expresion.Accept(this);
            var idResult = Assingment.Id.Accept(this);

            Assingment.SemanticCheckResult.Ensure(idResult);
            Assingment.SemanticCheckResult.Ensure(expresult,expresult.Type.IsIt(idResult.Type),$"Type {expresult.Type} is not subtype of {idResult.Type}.");
            Assingment.SemanticCheckResult.EnsureReturnType(expresult.Type);
            return Assingment.SemanticCheckResult;
        }

        public SemanticCheckResult VisitBlock(ASTBlockNode Block)
        {
            foreach (var item in Block.Expresions)
            {
                var sem = item.Accept(this);
                Block.SemanticCheckResult.Ensure(sem);
                Block.SemanticCheckResult.EnsureReturnType(sem.Type);
            }
            return Block.SemanticCheckResult;
        }

        public SemanticCheckResult VisitBoolNot(ASTBoolNotNode BoolNode)
        {
            var exp = BoolNode.Accept(this);
            BoolNode.SemanticCheckResult.Ensure(exp,exp.Type == CompilationUnit.TypeEnvironment.Bool,"Expresion must be of tipe Bool");
            BoolNode.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Bool);
            return BoolNode.SemanticCheckResult;
        }

        public SemanticCheckResult VisitCase(ASTCaseNode Case)
        {
            var expresionCaseResult = Case.ExpressionCase.Accept(this);
            Case.SemanticCheckResult.Ensure(expresionCaseResult);

            var branchresults = new SemanticCheckResult[Case.Cases.Length];
            for (int i = 0; i < Case.Cases.Length; i++)
            {
                branchresults[i] = Case.Cases[i].Branch.Accept(this);
                Case.SemanticCheckResult.Ensure(branchresults[i],branchresults[i].Type.IsIt(CompilationUnit.TypeEnvironment.GetTypeForObject(Case.SymbolTable,Case.Cases[i].Type)), $"Type {branchresults[i].Type} is not subtype of {Case.Cases[i].Type}.");
            }

            Case.SemanticCheckResult.EnsureReturnType(branchresults[0].Type);
            for (int i = 1; i < Case.Cases.Length; i++)
                Case.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.GetTypeLCA(Case.SemanticCheckResult.Type,branchresults[i].Type));

            return Case.SemanticCheckResult;
        }

        public SemanticCheckResult VisitClass(ASTClassNode Class)
        {
            string inherit= Class.ParentTypeName;
            Class.SemanticCheckResult.Ensure(CompilationUnit.TypeEnvironment.GetTypeDefinition(inherit,out var _),$"Type {inherit} is not defined.");

            foreach (var item in Class.Methods)
                Class.SemanticCheckResult.Ensure(item.Accept(this));
            foreach (var item in Class.Atributes)
                Class.SemanticCheckResult.Ensure(item.Accept(this));
            CompilationUnit.TypeEnvironment.GetTypeDefinition(Class.TypeName, out var ret);
            Class.SemanticCheckResult.EnsureReturnType(ret);
            return Class.SemanticCheckResult;
        }

        public SemanticCheckResult VisitDivision(ASTDivideNode Divide)
        {
            var left = Divide.Left.Accept(this);
            var right = Divide.Right.Accept(this);

            Divide.SemanticCheckResult.Ensure(left, left.Type == CompilationUnit.TypeEnvironment.Int, "Left Expresion must have type Int");
            Divide.SemanticCheckResult.Ensure(right, right.Type == CompilationUnit.TypeEnvironment.Int, "Right Expresion must have type Int");
            Divide.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return Divide.SemanticCheckResult;
        }

        public SemanticCheckResult VisitEqual(ASTEqualNode Equal)
        {
            var left = Equal.Left.Accept(this);
            var right = Equal.Right.Accept(this);
            if (left.Type==CompilationUnit.TypeEnvironment.Bool || left.Type==CompilationUnit.TypeEnvironment.Int || left.Type==CompilationUnit.TypeEnvironment.String || right.Type == CompilationUnit.TypeEnvironment.Bool || right.Type == CompilationUnit.TypeEnvironment.Int || right.Type == CompilationUnit.TypeEnvironment.String)
            {
                Equal.SemanticCheckResult.Ensure(left.Type == right.Type, $"{left.Type} and {right.Type} has different Types.");
            }
            Equal.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Bool);
            return Equal.SemanticCheckResult;
        }

        public SemanticCheckResult VisitBoolConstant(ASTBoolConstantNode BoolConstant)
        {
            BoolConstant.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Bool);
            return BoolConstant.SemanticCheckResult;
        }

        public SemanticCheckResult VisitIf(ASTIfNode If)
        {
            var conditionResult = If.Condition.Accept(this);
            var thenResult = If.Then.Accept(this);
            var elseResult = If.Else.Accept(this);

            If.SemanticCheckResult.Ensure(thenResult);
            If.SemanticCheckResult.Ensure(elseResult);
            If.SemanticCheckResult.Ensure(conditionResult,conditionResult.Type == CompilationUnit.TypeEnvironment.Bool,"Condition must be of Type Bool");
            If.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.GetTypeLCA(thenResult.Type, elseResult.Type));

            return If.SemanticCheckResult;
        }

        public SemanticCheckResult VisitIntConstant(ASTIntConstantNode IntConstant)
        {
            IntConstant.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return IntConstant.SemanticCheckResult;
        }

        public SemanticCheckResult VisitIsvoid(ASTIsVoidNode IsVoid)
        {
            var res = IsVoid.Expression.Accept(this);
            IsVoid.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Bool);
            return IsVoid.SemanticCheckResult;
        }

        public SemanticCheckResult VisitLessEqual(ASTLessEqualNode LessEqual)
        {
            var left = LessEqual.Left.Accept(this);
            var right = LessEqual.Right.Accept(this);

            LessEqual.SemanticCheckResult.Ensure(left, left.Type == CompilationUnit.TypeEnvironment.Int, "Left Expresion must be of tipe Int.");
            LessEqual.SemanticCheckResult.Ensure(right, right.Type == CompilationUnit.TypeEnvironment.Int, "Right Expresion must be of tipe Int.");
            LessEqual.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Bool);

            return LessEqual.SemanticCheckResult;
        }

        public SemanticCheckResult VisitLessThan(ASTLessThanNode LessThan)
        {
            var left = LessThan.Left.Accept(this);
            var right = LessThan.Right.Accept(this);

            LessThan.SemanticCheckResult.Ensure(left, left.Type == CompilationUnit.TypeEnvironment.Int, "Left Expresion must be of tipe Int.");
            LessThan.SemanticCheckResult.Ensure(right, right.Type == CompilationUnit.TypeEnvironment.Int, "Right Expresion must be of tipe Int.");
            LessThan.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Bool);

            return LessThan.SemanticCheckResult;
        }

        public SemanticCheckResult VisitLetIn(ASTLetInNode LetIn)
        {
            var declarations = LetIn.Declarations;
            foreach (var item in declarations)
            {
                var b=CompilationUnit.TypeEnvironment.GetTypeDefinition(item.Type, out var type);
                LetIn.SemanticCheckResult.Ensure(b,$"Missing declaration for type {item.Type}.");
                if (b)
                {
                    var resultExp = item.Expression.Accept(this);
                    LetIn.SemanticCheckResult.Ensure(resultExp,resultExp.Type.IsIt(type),$"Type {resultExp} does not inherit from type {type}");
                }
            }

            var expr = LetIn.LetExp.Accept(this);
            LetIn.SemanticCheckResult.Ensure(expr);
            LetIn.SemanticCheckResult.EnsureReturnType(expr.Type);
            return LetIn.SemanticCheckResult;
        }

        public SemanticCheckResult VisitMethod(ASTMethodNode Method)
        {
            foreach (var item in Method.Formals)
                Method.SemanticCheckResult.Ensure(CompilationUnit.TypeEnvironment.GetTypeDefinition(item.type,out var _), $"Mising declaration for type {item.type}.");

            var exprResult = Method.Body.Accept(this);
            var isDefRet = CompilationUnit.TypeEnvironment.GetTypeDefinition(Method.ReturnType,out var ret);
            Method.SemanticCheckResult.Ensure(isDefRet, $"Missing Declaration for type {Method.ReturnType}.");
            Method.SemanticCheckResult.EnsureReturnType(ret);
            if (isDefRet)
            {
                Method.SemanticCheckResult.Ensure(exprResult,exprResult.Type.IsIt(ret),$"Type {exprResult.Type} does not inherit from type {ret}.");
                Method.SemanticCheckResult.EnsureReturnType(ret);
            }

            return Method.SemanticCheckResult; 
        }

        public SemanticCheckResult VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall)
        {
            var onResult = MethodCall.InvokeOnExpresion.Accept(this);
            var isStaticDef = CompilationUnit.TypeEnvironment.GetTypeDefinition(MethodCall.Type,out var staticType);
            MethodCall.SemanticCheckResult.Ensure(isStaticDef, $"Missing declaration for type {MethodCall.Type}.");
            if (isStaticDef)
                MethodCall.SemanticCheckResult.Ensure(onResult, onResult.Type.IsIt(staticType), $"Type {onResult.Type} does not inherot from type {staticType}.");

            var isMetDef = CompilationUnit.TypeEnvironment.GetMethod(staticType, MethodCall.MethodName,out var method);
            MethodCall.SemanticCheckResult.Ensure(isMetDef, $"Missing declaration of method {MethodCall.MethodName} on type {MethodCall.Type}.");
            if (isMetDef)
            {
                MethodCall.SemanticCheckResult.Ensure(method.Params.Count == MethodCall.Arguments.Length, $"Mismatch parameters and argument count. Expected {method.Params.Count} and provided {MethodCall.Arguments.Length}");

                for (int i = 0; i < method.Params.Count; i++)
                {
                    var r = MethodCall.Arguments[i].Accept(this);
                    MethodCall.SemanticCheckResult.Ensure(r, r.Type.IsIt(method.Params[i]),$"Paremeter {i} type mismatch. Type {r.Type} does not inherit from type {method.Params[i]}.");
                }

                MethodCall.SemanticCheckResult.EnsureReturnType(method.ReturnType);
            }
            return MethodCall.SemanticCheckResult;
        }

        public SemanticCheckResult VisitMinus(ASTMinusNode Minus)
        {
            SemanticCheckResult result = new SemanticCheckResult();

            var left = Minus.Left.Accept(this);
            var right = Minus.Right.Accept(this);

            Minus.SemanticCheckResult.Ensure(left, left.Type == CompilationUnit.TypeEnvironment.Int, "Left Expresion must have type Int");
            Minus.SemanticCheckResult.Ensure(right, right.Type == CompilationUnit.TypeEnvironment.Int, "Right Expresion must have type Int");
            Minus.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return Minus.SemanticCheckResult;
        }

        public SemanticCheckResult VisitMultiply(ASTMultiplyNode Multiply)
        {
            SemanticCheckResult result = new SemanticCheckResult();

            var left = Multiply.Left.Accept(this);
            var right = Multiply.Right.Accept(this);

            Multiply.SemanticCheckResult.Ensure(left, left.Type == CompilationUnit.TypeEnvironment.Int, "Left Expresion must have type Int.");
            Multiply.SemanticCheckResult.Ensure(right, right.Type == CompilationUnit.TypeEnvironment.Int, "Right Expresion must have type Int.");
            Multiply.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return Multiply.SemanticCheckResult;
        }

        public SemanticCheckResult VisitNegative(ASTNegativeNode Negatve)
        {
            var exp = Negatve.Expression.Accept(this);

            Negatve.SemanticCheckResult.Ensure(exp, exp.Type == CompilationUnit.TypeEnvironment.Int, "Expresion must have type Int");
            Negatve.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return Negatve.SemanticCheckResult;
        }

        public SemanticCheckResult VisitNew(ASTNewNode New)
        {
            New.SemanticCheckResult.Ensure(CompilationUnit.TypeEnvironment.GetTypeDefinition(New.Type,out var type), $"Missing declaration for type {New.Type}.");
            New.SemanticCheckResult.EnsureReturnType(type);
            return New.SemanticCheckResult;
        }

        public SemanticCheckResult VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall)
        {
            var selfcooltype = CompilationUnit.TypeEnvironment.GetTypeForSelf(OwnMethodCall.SymbolTable);
            var isdef = CompilationUnit.TypeEnvironment.GetMethod(selfcooltype,OwnMethodCall.Method,out var method);
            OwnMethodCall.SemanticCheckResult.Ensure(isdef, $"Missing declaration for method {OwnMethodCall.Method} on type {CompilationUnit.TypeEnvironment.GetTypeForSelf(OwnMethodCall.SymbolTable)}.");
            if (isdef)
            {
                OwnMethodCall.SemanticCheckResult.Ensure(method.Params.Count == OwnMethodCall.Arguments.Length, $"Mismatch parameters and argument count. Expected {method.Params.Count} and provided {OwnMethodCall.Arguments.Length}");

                for (int i = 0; i < method.Params.Count; i++)
                {
                    var r = OwnMethodCall.Arguments[i].Accept(this);
                    OwnMethodCall.SemanticCheckResult.Ensure(r, r.Type.IsIt(method.Params[i]), $"Paremeter {i} type mismatch. Type {r.Type} does not inherit from type {method.Params[i]}.");
                }

                OwnMethodCall.SemanticCheckResult.EnsureReturnType(method.ReturnType);
            }
            return OwnMethodCall.SemanticCheckResult;
        }

        public SemanticCheckResult VisitProgram(ASTProgramNode Program)
        {
            foreach (var item in Program.Clases)
                Program.SemanticCheckResult.Ensure(item.Accept(this));
            return Program.SemanticCheckResult;
        }

        public SemanticCheckResult VisitAtribute(ASTAtributeNode Atribute)
        {
            var isdef = CompilationUnit.TypeEnvironment.GetTypeDefinition(Atribute.Type,out var t);
            Atribute.SemanticCheckResult.Ensure(isdef, $"Missing declaration for type {Atribute.Type}.");
            if (isdef)
            {
                var r = Atribute.Init.Accept(this);
                Atribute.SemanticCheckResult.Ensure(r,r.Type.IsIt(t),$"Type {r} does not inherit from type {t}.");
                Atribute.SemanticCheckResult.EnsureReturnType(t);
            }
            return Atribute.SemanticCheckResult; ;
        }

        public SemanticCheckResult VisitStringConstant(ASTStringConstantNode StringConstant)
        {
            StringConstant.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.String);
            return StringConstant.SemanticCheckResult; 
        }

        public SemanticCheckResult VisitWhile(ASTWhileNode While)
        {
            var cond = While.Condition.Accept(this);
            While.SemanticCheckResult.Ensure(cond);
            var body = While.Body.Accept(this);
            While.SemanticCheckResult.Ensure(body);
            While.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Object);
            return While.SemanticCheckResult;
        }

        public SemanticCheckResult VisitId(ASTIdNode Id)
        {
            Id.SemanticCheckResult.Ensure(Id.SymbolTable.IsDefObject(Id.Name,out var _), $"Missing declaration from object {Id.Name}.");
            Id.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.GetTypeForObject(Id.SymbolTable,Id.Name));
            return Id.SemanticCheckResult;
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
            var onResult = MethodCall.InvokeOnExpresion.Accept(this);
            MethodCall.SemanticCheckResult.Ensure(onResult);

            var isdef = CompilationUnit.TypeEnvironment.GetMethod(onResult.Type, MethodCall.MethodName,out var method);
            MethodCall.SemanticCheckResult.Ensure(isdef, $"Missing declaration for method {MethodCall.MethodName}.");
            if (isdef)
            {
                MethodCall.SemanticCheckResult.Ensure(method.Params.Count == MethodCall.Arguments.Length, $"Mismatch parameters and argument count. Expected {method.Params.Count} and provided {MethodCall.Arguments.Length}");

                for (int i = 0; i < method.Params.Count; i++)
                {
                    var r = MethodCall.Arguments[i].Accept(this);
                    MethodCall.SemanticCheckResult.Ensure(r, r.Type.IsIt(method.Params[i]), $"Paremeter {i} type mismatch. Type {r.Type} does not inherit from type {method.Params[i]}.");
                }

                MethodCall.SemanticCheckResult.EnsureReturnType(method.ReturnType);
            }
            return MethodCall.SemanticCheckResult;
        }

    }
}
