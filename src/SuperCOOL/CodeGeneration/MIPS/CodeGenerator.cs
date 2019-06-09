using SuperCOOL.CodeGeneration.CIL;
using SuperCOOL.CodeGeneration.CIL.AST;
using SuperCOOL.CodeGeneration.MIPS.Registers;
using System;

namespace SuperCOOL.CodeGeneration.MIPS
{
    class CodeGenerator : ICILVisitor<MipsProgram>
    {
        public MipsProgram VisitAddConstantVariable( ASTCILAddConstantVariableNode AddConstantVariable )
        {
            var result = AddConstantVariable.Right.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Add( MipsRegisterSet.a0, AddConstantVariable.Left ) );
            return result;
        }

        public MipsProgram VisitAddTwoConstant( ASTCILAddTwoConstantNode AddTwoConstant )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.a0, AddTwoConstant.Left )
                                                           .Add( MipsRegisterSet.a0, AddTwoConstant.Right ) );
            return result;
        }

        public MipsProgram VisitAddTwoVariables( ASTCILAddTwoVariablesNode AddTwoVariables )
        {
            var left = AddTwoVariables.Left.Accept( this );
            left.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                         .Push( MipsRegisterSet.a0 ) );

            var right = AddTwoVariables.Right.Accept( this );
            right.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                          .Pop( MipsRegisterSet.t0 )
                                                          .Add( MipsRegisterSet.a0, MipsRegisterSet.t0 ) );

            return left + right;
        }

        public MipsProgram VisitAddVariableConstant( ASTCILAddVariableConstantNode AddVariableConstant )
        {
            var result = AddVariableConstant.Left.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Add( MipsRegisterSet.a0, AddVariableConstant.Right ) );
            return result;
        }

        public MipsProgram VisitAllocate( ASTCILAllocateNode Allocate )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitAssignment( ASTCILAssignmentNode Assignment )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBlock( ASTCILBlockNode Block )
        {
            var result = new MipsProgram();
            foreach( var item in Block.Expressions )
                result += item.Accept( this );
            return result;
        }

        public MipsProgram VisitBoolConstant( ASTCILBoolConstantNode BoolConstant )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolNot( ASTCILBoolNotNode BoolNot )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolOrConstantVariable( ASTCILBoolOrConstantVariableNode BoolOrConstantVariable )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolOrTwoConstant( ASTCILBoolOrTwoConstantNode BoolOrTwoConstant )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolOrTwoVariables( ASTCILBoolOrTwoVariablesNode BoolOrTwoVariables )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolOrVariableConstant( ASTCILBoolOrVariableConstantNode BoolOrVariableConstant )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitDivideConstantVariable( ASTCILDivideConstantVariableNode DivideConstantVariable )
        {
            var result = DivideConstantVariable.Right.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.t0, DivideConstantVariable.Left )
                                                           .Div( MipsRegisterSet.t0, MipsRegisterSet.a0 ) );
            return result;
        }

        public MipsProgram VisitDivideTwoConstant( ASTCILDivideTwoConstantNode DivideTwoConstant )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitDivideTwoVariables( ASTCILDivideTwoVariablesNode DivideTwoVariables )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitDivideVariableConstant( ASTCILDivideVariableConstantNode DivideVariableConstant )
        {
            var result = DivideVariableConstant.Left.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Div( MipsRegisterSet.a0, DivideVariableConstant.Right ) );
            return result;
        }

        public MipsProgram VisitExpression(ASTCILExpressionNode Expression)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitFunc( ASTCILFuncNode Func )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitFuncStaticCall( ASTCILFuncStaticCallNode FuncStaticCall )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitFuncVirtualCall( ASTCILFuncVirtualCallNode FuncVirtualCall )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitGetAttr( ASTCILGetAttrNode GetAttr )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitGoto( ASTCILGotoNode Goto )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitId( ASTCILIdNode Id )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIf( ASTCILIfNode If )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIntConstant( ASTCILIntConstantNode IntConstant )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIOInInt( ASTCILIOInIntNode IOInInt )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIOInString( ASTCILIOInStringNode IOInString )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIOOutInt( ASTCILIOOutIntNode IOOutInt )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIOOutString( ASTCILIOOutStringNode IOOutString )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIsVoid( ASTCILIsVoidNode IsVoid )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitLessThanConstantVariable( ASTCILLessThanConstantVariableNode LessThanConstantVariable )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitLessThanTwoConstant( ASTCILLessThanTwoConstantNode LessThanTwoConstant )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitLessThanTwoVariables( ASTCILLessThanTwoVariablesNode LessThanTwoVariables )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitLessThanVariableConstant( ASTCILLessThanVariableConstantNode LessThanVariableConstant )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitMinusConstantVariable(ASTCILMinusConstantVariableNode MinusConstantVariable)
        {
            var result = MinusConstantVariable.Right.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadMemory( MipsRegisterSet.t0, MinusConstantVariable.Left )
                                                           .Sub( MipsRegisterSet.t0, MipsRegisterSet.a0, MipsRegisterSet.a0 ) );
            return result;
        }

        public MipsProgram VisitMinusTwoConstant( ASTCILMinusTwoConstantNode MinusTwoConstant )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.a0, MinusTwoConstant.Left )
                                                           .Sub( MipsRegisterSet.a0, MinusTwoConstant.Right ) );
            return result;
        }

        public MipsProgram VisitMinusTwoVariables( ASTCILMinusTwoVariablesNode MinusTwoVariables )
        {
            var left = MinusTwoVariables.Left.Accept( this );
            left.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                         .Push( MipsRegisterSet.a0 ) );

            var right = MinusTwoVariables.Right.Accept( this );
            right.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                          .Pop( MipsRegisterSet.t0 )
                                                          .Sub( MipsRegisterSet.t0, MipsRegisterSet.a0, MipsRegisterSet.a0 ) );

            return left + right;
        }

        public MipsProgram VisitMinusVariableConstant( ASTCILMinusVariableConstantNode MinusVariableConstant )
        {
            var result = MinusVariableConstant.Left.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Sub( MipsRegisterSet.a0, MinusVariableConstant.Right ) );
            return result;
        }

        public MipsProgram VisitMultiplyConstantVariable( ASTCILMultiplyConstantVariableNode MultiplyConstantVariable )
        {
            var result = MultiplyConstantVariable.Right.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Mul( MipsRegisterSet.a0, MultiplyConstantVariable.Left ) );
            return result;
        }

        public MipsProgram VisitMultiplyTwoConstant( ASTCILMultiplyTwoConstantNode MultiplyTwoConstant )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.a0, MultiplyTwoConstant.Left )
                                                           .Mul( MipsRegisterSet.a0, MultiplyTwoConstant.Right ) );
            return result;
        }

        public MipsProgram VisitMultiplyTwoVariables( ASTCILMultiplyTwoVariablesNode MultiplyTwoVariables )
        {
            var left = MultiplyTwoVariables.Left.Accept( this );
            left.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                        .Push( MipsRegisterSet.a0 ) );

            var right = MultiplyTwoVariables.Right.Accept( this );
            right.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                          .Pop( MipsRegisterSet.t0 )
                                                          .Mul( MipsRegisterSet.a0, MipsRegisterSet.t0 ) );

            return left + right;
        }

        public MipsProgram VisitMultiplyVariableConstant( ASTCILMultiplyVariableConstantNode MultiplyVariableConstant )
        {
            var result = MultiplyVariableConstant.Left.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Mul( MipsRegisterSet.a0, MultiplyVariableConstant.Right ) );
            return result;
        }

        public MipsProgram VisitNew( ASTCILNewNode New )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitNode( ASTCILNode Node )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitProgram(ASTCILProgramNode Program)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitRuntimeError( ASTCILRuntimeErrorNode RuntimeError )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitSelf( ASTCILSelfNode Self )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitSetAttribute( ASTCILSetAttributeNode SetAttribute )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitStringConstant( ASTCILStringConstantNode StringConstant )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitType( ASTCILTypeNode Type )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitVoid( ASTCILVoidNode Void )
        {
            throw new NotImplementedException();
        }
    }
}
