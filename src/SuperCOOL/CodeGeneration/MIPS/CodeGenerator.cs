using SuperCOOL.CodeGeneration.CIL;
using SuperCOOL.CodeGeneration.CIL.AST;
using SuperCOOL.CodeGeneration.MIPS.Registers;
using SuperCOOL.CodeGeneration.CIL;
using System;

namespace SuperCOOL.CodeGeneration.MIPS
{
    class CodeGenerator : ICILVisitor<MipsProgram>
    {
        private ILabelILGenerator labelGenerator;
        public CodeGenerator( ILabelILGenerator labelGenerator ) => this.labelGenerator = labelGenerator;


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
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.a0, BoolConstant.Value ? MipsGenerationHelper.TRUE : MipsGenerationHelper.FALSE ) );
            return result;
        }

        public MipsProgram VisitBoolNot( ASTCILBoolNotNode BoolNot )
        {
            var result = BoolNot.Expression.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Not( MipsRegisterSet.a0 ) );
            return result;
        }

        public MipsProgram VisitBoolOrConstantVariable( ASTCILBoolOrConstantVariableNode BoolOrConstantVariable )
        {
            var result = BoolOrConstantVariable.Right.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .OrConstant( MipsRegisterSet.a0, BoolOrConstantVariable.Left ? MipsGenerationHelper.TRUE : MipsGenerationHelper.FALSE ) );
            return result;
        }

        public MipsProgram VisitBoolOrTwoConstant( ASTCILBoolOrTwoConstantNode BoolOrTwoConstant )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.a0, BoolOrTwoConstant.Left ? MipsGenerationHelper.TRUE : MipsGenerationHelper.FALSE )
                                                           .OrConstant( MipsRegisterSet.a0, BoolOrTwoConstant.Right ? MipsGenerationHelper.TRUE : MipsGenerationHelper.FALSE ) );
            return result;
        }

        public MipsProgram VisitBoolOrTwoVariables( ASTCILBoolOrTwoVariablesNode BoolOrTwoVariables )
        {
            var left = BoolOrTwoVariables.Left.Accept( this );
            left.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                         .Push( MipsRegisterSet.a0 ) );

            var right = BoolOrTwoVariables.Right.Accept( this );
            right.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                          .Pop( MipsRegisterSet.t0 )
                                                          .Or( MipsRegisterSet.a0, MipsRegisterSet.t0 ) );

            return left + right;
        }

        public MipsProgram VisitBoolOrVariableConstant( ASTCILBoolOrVariableConstantNode BoolOrVariableConstant )
        {
            var result = BoolOrVariableConstant.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .OrConstant( MipsRegisterSet.a0, BoolOrVariableConstant.Right ? MipsGenerationHelper.TRUE : MipsGenerationHelper.FALSE ) );
            return result;
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
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.a0, DivideTwoConstant.Right )
                                                           .Div( MipsRegisterSet.a0, DivideTwoConstant.Right ) );
            return result;
        }

        public MipsProgram VisitDivideTwoVariables( ASTCILDivideTwoVariablesNode DivideTwoVariables )
        {
            var left = DivideTwoVariables.Left.Accept( this );
            left.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                         .Push( MipsRegisterSet.a0 ) );

            var right = DivideTwoVariables.Right.Accept( this );
            right.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                          .Pop( MipsRegisterSet.t0 )
                                                          .Div( MipsRegisterSet.t0, MipsRegisterSet.a0 ) );

            return left + right;
        }

        public MipsProgram VisitDivideVariableConstant( ASTCILDivideVariableConstantNode DivideVariableConstant )
        {
            var result = DivideVariableConstant.Left.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Div( MipsRegisterSet.a0, DivideVariableConstant.Right ) );
            return result;
        }

        public MipsProgram VisitExpression( ASTCILExpressionNode Expression )
        {
            return Expression.Accept( this );
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
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Jump( Goto.Label ) );
            return result;
        }

        public MipsProgram VisitId( ASTCILIdNode Id )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIf( ASTCILIfNode If )
        {
            var @if = If.Condition.Accept( this );
            @if.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                        .BranchOnEquals( MipsRegisterSet.a0, MipsGenerationHelper.FALSE, If.ElseLabel ) );

            var then = If.Then.Accept( this );
            then.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                         .Tag( If.ElseLabel ) );

            var @else = If.Else.Accept( this );
            @else.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                          .Tag( If.EndLabel ) );

            return @if + then + @else;
        }

        public MipsProgram VisitIntConstant( ASTCILIntConstantNode IntConstant )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.a0, IntConstant.Value ) );
            return result;
        }

        public MipsProgram VisitIOInInt( ASTCILIOInIntNode IOInInt )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .ReadInt( MipsRegisterSet.a0 ) );
            return result;
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

        // TODO hay que hacer esto en IL
        public MipsProgram VisitIsVoid( ASTCILIsVoidNode IsVoid )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitLessThanConstantVariable( ASTCILLessThanConstantVariableNode LessThanConstantVariable )
        {
            var result = LessThanConstantVariable.Right.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.t0, LessThanConstantVariable.Left )
                                                           .BranchLessThan( MipsRegisterSet.t0,  ) );
            return result;
        }

        public MipsProgram VisitLessThanTwoConstant( ASTCILLessThanTwoConstantNode LessThanTwoConstant )
        {
            var (endIf, then) = this.labelGenerator.GenerateIf();
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.a0, LessThanTwoConstant.Left )
                                                           .BranchLessThan( MipsRegisterSet.a0, LessThanTwoConstant.Right, then )
                                                           .LoadConstant( MipsRegisterSet.a0, 0 )
                                                           .Jump( endIf )
                                                           .Tag( then )
                                                           .LoadConstant( MipsRegisterSet.a0, 1 )
                                                           .Tag( endIf ) );
            return result;
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

        public MipsProgram VisitNode( ASTCILNode Node ) => throw new NotImplementedException();

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
