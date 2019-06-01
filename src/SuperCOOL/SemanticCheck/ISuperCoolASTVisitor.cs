using SuperCOOL.SemanticCheck.AST;

namespace SuperCOOL.SemanticCheck
{
    public interface ISuperCoolASTVisitor<Result>
    {
        Result VisitAdd(ASTAddNode Add);

        Result VisitAssignment(ASTAssingmentNode Assigment);

        Result VisitBlock(ASTBlockNode Block);

        Result VisitBoolNot(ASTBoolNotNode BoolNot);

        Result VisitCase(ASTCaseNode Case);

        Result VisitClass(ASTClassNode Class);

        Result VisitDivision(ASTDivideNode Division);

        Result VisitEqual(ASTEqualNode Equal);

        Result VisitBoolConstant(ASTBoolConstantNode BoolConstant);

        Result VisitIf(ASTIfNode If);

        Result VisitIntConstant(ASTIntConstantNode Int);

        Result VisitIsvoid(ASTIsVoidNode IsVoid);

        Result VisitLessEqual(ASTLessEqualNode LessEqual);

        Result VisitLessThan(ASTLessThanNode LessThan);

        Result VisitLetIn(ASTLetInNode LetIn);

        Result VisitMethod(ASTMethodNode Method);
        Result VisitStaticMethodCall(ASTStaticMethodCallNode MethodCall);
        Result VisitDynamicMethodCall(ASTDynamicMethodCallNode MethodCall);
        Result VisitMinus(ASTMinusNode Minus);
        Result VisitMultiply(ASTMultiplyNode Multiply);

        Result VisitNegative(ASTNegativeNode Negative);

        Result VisitNew(ASTNewNode context);

        Result VisitOwnMethodCall(ASTOwnMethodCallNode OwnMethodCall);

        Result VisitProgram(ASTProgramNode Program);

        Result VisitAtribute(ASTAtributeNode Atribute);

        Result VisitStringConstant(ASTStringConstantNode StringConstant);

        Result VisitWhile(ASTWhileNode While);

        Result VisitId(ASTIdNode Id);

        Result VisitFormal(ASTFormalNode Formal);

        Result Visit(ASTNode Node);

        Result VisitExpression(ASTExpressionNode Expression);
    }
}
