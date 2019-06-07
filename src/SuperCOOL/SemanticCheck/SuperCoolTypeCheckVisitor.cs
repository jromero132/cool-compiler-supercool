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

        private CompilationUnit CompilationUnit { get; }

        public SemanticCheckResult VisitAdd(ASTAddNode Add)
        {
            var left = Add.Left.Accept(this);
            var right = Add.Right.Accept(this);

            Add.SemanticCheckResult.Ensure(left,left.Type==CompilationUnit.TypeEnvironment.Int,
                new Error("Left Expresion must have type Int",ErrorKind.TypeError,Add.AddToken.Line,Add.AddToken.Column));
            Add.SemanticCheckResult.Ensure(right,right.Type==CompilationUnit.TypeEnvironment.Int,
                new Error("Right Expresion must have type Int",ErrorKind.TypeError, Add.AddToken.Line, Add.AddToken.Column));
            Add.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return Add.SemanticCheckResult;
        }

        public SemanticCheckResult VisitAssignment(ASTAssingmentNode Assingment)
        {
            var expresult = Assingment.Expresion.Accept(this);
            var idResult = Assingment.Id.Accept(this);

            Assingment.SemanticCheckResult.Ensure(idResult);
            Assingment.SemanticCheckResult.Ensure(expresult,expresult.Type.IsIt(idResult.Type),new Error($"Type {expresult.Type} is not subtype of {idResult.Type}.",ErrorKind.TypeError,Assingment.AssigmentToken.Line,Assingment.AssigmentToken.Column));
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
            BoolNode.SemanticCheckResult.Ensure(exp,exp.Type == CompilationUnit.TypeEnvironment.Bool,new Error("Expresion must be of tipe Bool",ErrorKind.TypeError,BoolNode.NotToken.Line,BoolNode.NotToken.Column));
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
                //TODO: check if this is necesary
                //Case.SemanticCheckResult.Ensure(branchresults[i],branchresults[i].Type.IsIt(CompilationUnit.TypeEnvironment.GetTypeForObject(Case.SymbolTable,Case.Cases[i].Type.Text)),
                //    new Error($"Type {branchresults[i].Type} is not subtype of {Case.Cases[i].Type}.",ErrorKind.TypeError,Case.Cases[i]));
            }

            Case.SemanticCheckResult.EnsureReturnType(branchresults[0].Type);
            for (int i = 1; i < Case.Cases.Length; i++)
                Case.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.GetTypeLCA(Case.SemanticCheckResult.Type,branchresults[i].Type));

            return Case.SemanticCheckResult;
        }

        public SemanticCheckResult VisitClass(ASTClassNode Class)
        {
            foreach (var item in Class.Methods)
                Class.SemanticCheckResult.Ensure(item.Accept(this));
            foreach (var item in Class.Atributes)
                Class.SemanticCheckResult.Ensure(item.Accept(this));
            CompilationUnit.TypeEnvironment.GetTypeDefinition(Class.TypeName,Class.SymbolTable, out var ret);
            Class.SemanticCheckResult.EnsureReturnType(ret);
            return Class.SemanticCheckResult;
        }

        public SemanticCheckResult VisitDivision(ASTDivideNode Divide)
        {
            var left = Divide.Left.Accept(this);
            var right = Divide.Right.Accept(this);

            Divide.SemanticCheckResult.Ensure(left, left.Type == CompilationUnit.TypeEnvironment.Int,new Error("Left Expresion must have type Int",ErrorKind.TypeError,Divide.DivToken.Line,Divide.DivToken.Column));
            Divide.SemanticCheckResult.Ensure(right, right.Type == CompilationUnit.TypeEnvironment.Int,new Error("Right Expresion must have type Int",ErrorKind.TypeError, Divide.DivToken.Line, Divide.DivToken.Column));
            Divide.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return Divide.SemanticCheckResult;
        }

        public SemanticCheckResult VisitEqual(ASTEqualNode Equal)
        {
            var left = Equal.Left.Accept(this);
            var right = Equal.Right.Accept(this);
            if (left.Type==CompilationUnit.TypeEnvironment.Bool || left.Type==CompilationUnit.TypeEnvironment.Int || left.Type==CompilationUnit.TypeEnvironment.String || right.Type == CompilationUnit.TypeEnvironment.Bool || right.Type == CompilationUnit.TypeEnvironment.Int || right.Type == CompilationUnit.TypeEnvironment.String)
                Equal.SemanticCheckResult.Ensure(left.Type == right.Type,
                    new Error($"{left.Type} and {right.Type} has different Types.",ErrorKind.TypeError,Equal.EqualToken.Line,Equal.EqualToken.Column));
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
            If.SemanticCheckResult.Ensure(conditionResult,conditionResult.Type == CompilationUnit.TypeEnvironment.Bool,
                new Error("Condition must be of Type Bool",ErrorKind.TypeError,If.IfToken.Line,If.IfToken.Column));
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

            LessEqual.SemanticCheckResult.Ensure(left, left.Type == CompilationUnit.TypeEnvironment.Int,
                new Error("Left Expresion must be of tipe Int.",ErrorKind.TypeError,LessEqual.LessEqualToken.Line,LessEqual.LessEqualToken.Column));
            LessEqual.SemanticCheckResult.Ensure(right, right.Type == CompilationUnit.TypeEnvironment.Int,
                new Error("Right Expresion must be of tipe Int.",ErrorKind.TypeError, LessEqual.LessEqualToken.Line, LessEqual.LessEqualToken.Column));
            LessEqual.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Bool);

            return LessEqual.SemanticCheckResult;
        }

        public SemanticCheckResult VisitLessThan(ASTLessThanNode LessThan)
        {
            var left = LessThan.Left.Accept(this);
            var right = LessThan.Right.Accept(this);

            LessThan.SemanticCheckResult.Ensure(left, left.Type == CompilationUnit.TypeEnvironment.Int,
                new Error( "Left Expresion must be of tipe Int.",ErrorKind.TypeError,LessThan.LessThanToken.Line,LessThan.LessThanToken.Column));
            LessThan.SemanticCheckResult.Ensure(right, right.Type == CompilationUnit.TypeEnvironment.Int,
                new Error("Right Expresion must be of tipe Int.",ErrorKind.TypeError, LessThan.LessThanToken.Line, LessThan.LessThanToken.Column));
            LessThan.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Bool);

            return LessThan.SemanticCheckResult;
        }

        public SemanticCheckResult VisitLetIn(ASTLetInNode LetIn)
        {
            var declarations = LetIn.Declarations;
            foreach (var item in declarations)
            {
                var b=CompilationUnit.TypeEnvironment.GetTypeDefinition(item.Type.Text,LetIn.SymbolTable, out var type);
                LetIn.SemanticCheckResult.Ensure(b,
                    new Error($"Missing declaration for type {item.Type.Text}.",ErrorKind.TypeError,item.Type.Line,item.Type.Column));
                if (b)
                {
                    var resultExp = item.Expression.Accept(this);
                    LetIn.SemanticCheckResult.Ensure(resultExp,resultExp.Type.IsIt(type),
                        new Error($"Type {resultExp} does not inherit from type {type}",ErrorKind.TypeError,item.Type.Line,item.Type.Column));
                }
            }

            var expr = LetIn.LetExp.Accept(this);
            LetIn.SemanticCheckResult.Ensure(expr);
            LetIn.SemanticCheckResult.EnsureReturnType(expr.Type);
            return LetIn.SemanticCheckResult;
        }

        public SemanticCheckResult VisitMethod(ASTMethodNode Method)
        {
            var exprResult = Method.Body.Accept(this);
            var isDefRet = CompilationUnit.TypeEnvironment.GetTypeDefinition(Method.ReturnType,Method.SymbolTable,out var ret);
            Method.SemanticCheckResult.Ensure(isDefRet,
                new Error($"Missing Declaration for type {Method.ReturnType}.",ErrorKind.TypeError,Method.Return.Line,Method.Return.Column));
            Method.SemanticCheckResult.EnsureReturnType(ret);
            if (isDefRet)
            {
                //Todo check if ret.IsIt(exprResult.Type)...
                Method.SemanticCheckResult.Ensure(exprResult,exprResult.Type.IsIt(ret),new Error($"Type {exprResult.Type} does not inherit from type {ret}.",ErrorKind.TypeError,Method.Return.Line,Method.Return.Column));
                Method.SemanticCheckResult.EnsureReturnType(ret);
            }

            return Method.SemanticCheckResult; 
        }

        public SemanticCheckResult VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall)
        {
            var onResult = MethodCall.InvokeOnExpresion.Accept(this);
            var isStaticDef = CompilationUnit.TypeEnvironment.GetTypeDefinition(MethodCall.TypeName,MethodCall.SymbolTable,out var staticType);
            MethodCall.SemanticCheckResult.Ensure(isStaticDef,
                new Error($"Missing declaration for type {MethodCall.TypeName}.",ErrorKind.TypeError,MethodCall.Type.Line,MethodCall.Type.Column));
            if (isStaticDef)
                MethodCall.SemanticCheckResult.Ensure(onResult, onResult.Type.IsIt(staticType),
                    new Error($"Type {onResult.Type} does not inherot from type {staticType}.",ErrorKind.TypeError,MethodCall.Type.Line,MethodCall.Type.Line));

            var isMetDef = CompilationUnit.MethodEnvironment.GetMethod(staticType, MethodCall.MethodName,out var method);
            MethodCall.SemanticCheckResult.Ensure(isMetDef, 
                new Error($"Missing declaration of method {MethodCall.MethodName} on type {MethodCall.TypeName}.",ErrorKind.MethodError,MethodCall.Method.Line,MethodCall.Method.Column));
            if (isMetDef)
            {
                MethodCall.SemanticCheckResult.Ensure(method.Params.Count == MethodCall.Arguments.Length,
                    new Error($"Mismatch parameters and argument count. Expected {method.Params.Count} and provided {MethodCall.Arguments.Length}",ErrorKind.SemanticError, MethodCall.Method.Line, MethodCall.Method.Column));

                for (int i = 0; i < method.Params.Count; i++)
                {
                    var r = MethodCall.Arguments[i].Accept(this);
                    MethodCall.SemanticCheckResult.Ensure(r, r.Type.IsIt(method.Params[i]),
                        new Error($"Paremeter {i} type mismatch. Type {r.Type} does not inherit from type {method.Params[i]}.",ErrorKind.MethodError, MethodCall.Method.Line, MethodCall.Method.Column));
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

            Minus.SemanticCheckResult.Ensure(left, left.Type == CompilationUnit.TypeEnvironment.Int,
                new Error("Left Expresion must have type Int",ErrorKind.TypeError,Minus.MinusToken.Line,Minus.MinusToken.Column));
            Minus.SemanticCheckResult.Ensure(right, right.Type == CompilationUnit.TypeEnvironment.Int,
                new Error("Right Expresion must have type Int",ErrorKind.TypeError, Minus.MinusToken.Line, Minus.MinusToken.Column));
            Minus.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return Minus.SemanticCheckResult;
        }

        public SemanticCheckResult VisitMultiply(ASTMultiplyNode Multiply)
        {
            SemanticCheckResult result = new SemanticCheckResult();

            var left = Multiply.Left.Accept(this);
            var right = Multiply.Right.Accept(this);

            Multiply.SemanticCheckResult.Ensure(left, left.Type == CompilationUnit.TypeEnvironment.Int,
                new Error("Left Expresion must have type Int.",ErrorKind.TypeError,Multiply.MultToken.Line,Multiply.MultToken.Column));
            Multiply.SemanticCheckResult.Ensure(right, right.Type == CompilationUnit.TypeEnvironment.Int,
                new Error("Right Expresion must have type Int.",ErrorKind.TypeError, Multiply.MultToken.Line, Multiply.MultToken.Column));
            Multiply.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return Multiply.SemanticCheckResult;
        }

        public SemanticCheckResult VisitNegative(ASTNegativeNode Negatve)
        {
            var exp = Negatve.Expression.Accept(this);

            Negatve.SemanticCheckResult.Ensure(exp, exp.Type == CompilationUnit.TypeEnvironment.Int,
                new Error("Expresion must have type Int",ErrorKind.TypeError,Negatve.NegativeToken.Line,Negatve.NegativeToken.Column));
            Negatve.SemanticCheckResult.EnsureReturnType(CompilationUnit.TypeEnvironment.Int);
            return Negatve.SemanticCheckResult;
        }

        public SemanticCheckResult VisitNew(ASTNewNode New)
        {
            New.SemanticCheckResult.Ensure(CompilationUnit.TypeEnvironment.GetTypeDefinition(New.TypeName,New.SymbolTable,out var type),
                new Error($"Missing declaration for type {New.TypeName}.",ErrorKind.TypeError,New.Type.Line,New.Type.Column));
            New.SemanticCheckResult.EnsureReturnType(type);
            return New.SemanticCheckResult;
        }

        public SemanticCheckResult VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall)
        {
            var selfcooltype = CompilationUnit.TypeEnvironment.GetContextType(OwnMethodCall.SymbolTable);
            var isdef = CompilationUnit.MethodEnvironment.GetMethod(selfcooltype,OwnMethodCall.MethodName,out var method);
            OwnMethodCall.SemanticCheckResult.Ensure(isdef,
                new Error($"Missing declaration for method {OwnMethodCall.MethodName} on type {CompilationUnit.TypeEnvironment.GetContextType(OwnMethodCall.SymbolTable)}.",ErrorKind.MethodError,OwnMethodCall.Method.Line,OwnMethodCall.Method.Column));
            if (isdef)
            {
                OwnMethodCall.SemanticCheckResult.Ensure(method.Params.Count == OwnMethodCall.Arguments.Length, 
                    new Error($"Mismatch parameters and argument count. Expected {method.Params.Count} and provided {OwnMethodCall.Arguments.Length}", ErrorKind.MethodError, OwnMethodCall.Method.Line, OwnMethodCall.Method.Column));

                for (int i = 0; i < method.Params.Count; i++)
                {
                    var r = OwnMethodCall.Arguments[i].Accept(this);
                    OwnMethodCall.SemanticCheckResult.Ensure(r, r.Type.IsIt(method.Params[i]),
                        new Error($"Paremeter {i} type mismatch. Type {r.Type} does not inherit from type {method.Params[i]}.",ErrorKind.MethodError,OwnMethodCall.Method.Line,OwnMethodCall.Method.Column));
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
            var isdef = CompilationUnit.TypeEnvironment.GetTypeDefinition(Atribute.TypeName,Atribute.SymbolTable,out var t);
            Atribute.SemanticCheckResult.Ensure(isdef,
                new Error ($"Missing declaration for type {Atribute.TypeName}.",ErrorKind.TypeError,Atribute.Type.Line, Atribute.Type.Column));
            if (isdef)
            {
                var r = Atribute.Init.Accept(this);
                Atribute.SemanticCheckResult.Ensure(r,r.Type.IsIt(t),
                    new Error($"Type {r} does not inherit from type {t}.",ErrorKind.AttributeError,Atribute.Attribute.Line,Atribute.Attribute.Column));
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
            Id.SemanticCheckResult.Ensure(CompilationUnit.TypeEnvironment.GetTypeForObject(Id.SymbolTable, Id.Name,out var type),
                new Error($"Missing declaration from object {Id.Name}.",ErrorKind.NameError,Id.Token.Line,Id.Token.Column));
            Id.SemanticCheckResult.EnsureReturnType(type);
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

            var isdef = CompilationUnit.MethodEnvironment.GetMethod(onResult.Type, MethodCall.MethodName,out var method);
            MethodCall.SemanticCheckResult.Ensure(isdef, 
                new Error($"Missing declaration for method {MethodCall.MethodName}.",ErrorKind.MethodError,MethodCall.Method.Line,MethodCall.Method.Column));
            if (isdef)
            {
                MethodCall.SemanticCheckResult.Ensure(method.Params.Count == MethodCall.Arguments.Length,
                    new Error($"Mismatch parameters and argument count. Expected {method.Params.Count} and provided {MethodCall.Arguments.Length}",ErrorKind.MethodError,MethodCall.Method.Line,MethodCall.Method.Column));

                for (int i = 0; i < method.Params.Count; i++)
                {
                    var r = MethodCall.Arguments[i].Accept(this);
                    MethodCall.SemanticCheckResult.Ensure(r, r.Type.IsIt(method.Params[i]),
                        new Error($"Paremeter {i} type mismatch. Type {r.Type} does not inherit from type {method.Params[i]}.",ErrorKind.TypeError,MethodCall.Method.Line,MethodCall.Method.Column));
                }

                MethodCall.SemanticCheckResult.EnsureReturnType(method.ReturnType);
            }
            return MethodCall.SemanticCheckResult;
        }

    }
}
