using SuperCOOL.CodeGeneration.CIL.AST;

namespace SuperCOOL.CodeGeneration.CIL
{
    public interface ICILVisitor<Result>
    {
        Result VisitSelf( ASTCILSelfNode Self );
        Result VisitAddTwoVariables( ASTCILAddTwoVariablesNode AddTwoVariables );
        Result VisitAllocate( ASTCILAllocateNode Allocate );
        Result VisitAssignment( ASTCILAssignmentNode Assignment );
        Result VisitBlock( ASTCILBlockNode Block );
        Result VisitBoolConstant( ASTCILBoolConstantNode BoolConstant );
        Result VisitBoolNot( ASTCILBoolNotNode BoolNot );
        Result VisitBoolOrTwoVariables( ASTCILBoolOrTwoVariablesNode BoolOrTwoVariables );
        Result VisitDivideTwoVariables( ASTCILDivideTwoVariablesNode DivideTwoVariables );
        Result VisitExpression( ASTCILExpressionNode Expression );
        Result VisitFunc( ASTCILFuncNode Func );
        Result VisitFuncStaticCall( ASTCILFuncStaticCallNode FuncStaticCall );
        Result VisitFuncVirtualCall( ASTCILFuncVirtualCallNode FuncVirtualCall );
        Result VisitGetAttr( ASTCILGetAttrNode GetAttr );
        Result VisitGoto( ASTCILGotoNode Goto );
        Result VisitId( ASTCILIdNode Id );
        Result VisitIf( ASTCILIfNode If );
        Result VisitIntConstant( ASTCILIntConstantNode IntConstant );
        Result VisitIOInInt( ASTCILIOInIntNode IOInInt );
        Result VisitIOInString( ASTCILIOInStringNode IOInString );
        Result VisitIOOutInt( ASTCILIOOutIntNode IOOutInt );
        Result VisitIOOutString( ASTCILIOOutStringNode IOOutString );
        Result VisitIsVoid( ASTCILIsVoidNode IsVoid );
        Result VisitLessThanTwoVariables( ASTCILLessThanTwoVariablesNode LessThanTwoVariables );
        Result VisitMinusTwoVariables( ASTCILMinusTwoVariablesNode MinusTwoVariables );
        Result VisitMultiplyTwoVariables( ASTCILMultiplyTwoVariablesNode MultiplyTwoVariables );
        Result VisitNew( ASTCILNewNode New );
        Result VisitNode( ASTCILNode Node );
        Result VisitProgram( ASTCILProgramNode Program );
        Result VisitRuntimeError( ASTCILRuntimeErrorNode RuntimeError );
        Result VisitSetAttribute( ASTCILSetAttributeNode SetAttribute );
        Result VisitStringConstant( ASTCILStringConstantNode StringConstant );
        Result VisitType( ASTCILTypeNode Type );
        Result VisitVoid( ASTCILVoidNode Void );
        Result VisitObjectTypeName(ASTCILObjectTypeNameNode objectTypeName);
        Result VisitObjectCopy(ASTCILObjectCopyNode objectCopy);
        Result VisitStringConcat(ASTCILStringConcatNode stringConcat);
        Result VisitStringSubStr(ASTCILStringSubStrNode stringSubStr);
    }
}
