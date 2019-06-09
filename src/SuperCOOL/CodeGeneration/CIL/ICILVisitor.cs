using SuperCOOL.CodeGeneration.CIL.AST;

namespace SuperCOOL.CodeGeneration.CIL
{
    public interface ICILVisitor<Result>
    {
        Result VisitAddConstantVariable( ASTCILAddConstantVariableNode AddConstantVariable );
        Result VisitAddTwoConstant( ASTCILAddTwoConstantNode AddTwoConstant );
        Result VisitSelf( ASTCILSelfNode Self );
        Result VisitAddTwoVariables( ASTCILAddTwoVariablesNode AddTwoVariables );
        Result VisitAddVariableConstant( ASTCILAddVariableConstantNode AddVariableConstant );
        Result VisitAllocate( ASTCILAllocateNode Allocate );
        Result VisitAssignment( ASTCILAssignmentNode Assignment );
        Result VisitBlock( ASTCILBlockNode Block );
        Result VisitBoolConstant( ASTCILBoolConstantNode BoolConstant );
        Result VisitBoolNot( ASTCILBoolNotNode BoolNot );
        Result VisitBoolOrConstantVariable( ASTCILBoolOrConstantVariableNode BoolOrConstantVariable );
        Result VisitBoolOrTwoConstant( ASTCILBoolOrTwoConstantNode BoolOrTwoConstant );
        Result VisitBoolOrTwoVariables( ASTCILBoolOrTwoVariablesNode BoolOrTwoVariables );
        Result VisitBoolOrVariableConstant( ASTCILBoolOrVariableConstantNode BoolOrVariableConstant );
        Result VisitDivideConstantVariable( ASTCILDivideConstantVariableNode DivideConstantVariable );
        Result VisitDivideTwoConstant( ASTCILDivideTwoConstantNode DivideTwoConstant );
        Result VisitDivideTwoVariables( ASTCILDivideTwoVariablesNode DivideTwoVariables );
        Result VisitDivideVariableConstant( ASTCILDivideVariableConstantNode DivideVariableConstant );
        Result VisitEqual( ASTCILEqualNode Equal );
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
        Result VisitLessThanConstantVariable( ASTCILLessThanConstantVariableNode LessThanConstantVariable );
        Result VisitLessThanTwoConstant( ASTCILLessThanTwoConstantNode LessThanTwoConstant );
        Result VisitLessThanTwoVariables( ASTCILLessThanTwoVariablesNode LessThanTwoVariables );
        Result VisitLessThanVariableConstant( ASTCILLessThanVariableConstantNode LessThanVariableConstant );
        Result VisitLocal( ASTCILLocalNode Local );
        Result VisitMinusConstantVariable( ASTCILMinusConstantVariableNode MinusConstantVariable );
        Result VisitMinusTwoConstant( ASTCILMinusTwoConstantNode MinusTwoConstant );
        Result VisitMinusTwoVariables( ASTCILMinusTwoVariablesNode MinusTwoVariables );
        Result VisitMinusVariableConstant( ASTCILMinusVariableConstantNode MinusVariableConstant );
        Result VisitMultiplyConstantVariable( ASTCILMultiplyConstantVariableNode MultiplyConstantVariable );
        Result VisitMultiplyTwoConstant( ASTCILMultiplyTwoConstantNode MultiplyTwoConstant );
        Result VisitMultiplyTwoVariables( ASTCILMultiplyTwoVariablesNode MultiplyTwoVariables );
        Result VisitMultiplyVariableConstant( ASTCILMultiplyVariableConstantNode MultiplyVariableConstant );
        Result VisitNew( ASTCILNewNode New );
        Result VisitNode( ASTCILNode Node );
        Result VisitParam( ASTCILParamNode Param );
        Result VisitProgram( ASTCILProgramNode Program );
        Result VisitRuntimeError( ASTCILRuntimeErrorNode RuntimeError );
        Result VisitSetAttribute( ASTCILSetAttributeNode SetAttribute );
        Result VisitStringConstant( ASTCILStringConstantNode StringConstant );
        Result VisitType( ASTCILTypeNode Type );
        Result VisitVoid( ASTCILVoidNode Void );
    }
}
