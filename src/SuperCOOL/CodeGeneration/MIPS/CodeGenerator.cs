using SuperCOOL.CodeGeneration.CIL;
using SuperCOOL.CodeGeneration.CIL.AST;
using SuperCOOL.CodeGeneration.MIPS.Registers;
using System;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.MIPS
{
    class CodeGenerator : ICILVisitor<MipsProgram>
    {
        private ILabelILGenerator labelGenerator;
        public CompilationUnit CompilationUnit { get; }

        public CodeGenerator( ILabelILGenerator labelGenerator, CompilationUnit compilationUnit )
        {
            this.labelGenerator = labelGenerator;
            this.CompilationUnit = compilationUnit;
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

        public MipsProgram VisitAllocate( ASTCILAllocateNode Allocate )
        {
            throw new NotImplementedException();
        }

        // TODO
        public MipsProgram VisitAssignment( ASTCILAssignmentNode Assignment )
        {
            var result = Assignment.Expresion.Accept( this );

            Assignment.SymbolTable.IsDefObject( Assignment.Identifier, out var symbolInfo );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .SaveToMemory( MipsRegisterSet.a0, symbolInfo.Offset ) );
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
            var result = new MipsProgram();
            foreach (var arg in FuncStaticCall.Arguments.Reverse())
            {
                result += arg.Accept(this);//leave in a0 expresion result
                result.SectionCode.Append(MipsGenerationHelper.NewScript().Push(MipsRegisterSet.a0));
            }
            // moving self to a0 not necesary self is already in ao.
            //result.SectionCode.Append(MipsGenerationHelper.NewScript().LoadMemory(MipsRegisterSet.a0, MipsRegisterSet.fp, MipsGenerationHelper.SelfOffset));
            //loading self.typeInfo in a0
            result.SectionCode.Append(MipsGenerationHelper.NewScript().LoadFromMemory(MipsRegisterSet.a0,MipsRegisterSet.a0,MipsGenerationHelper.TypeInfoOffest));
            //loading typeInfo.virtual_table in a0
            result.SectionCode.Append(MipsGenerationHelper.NewScript().LoadFromMemory(MipsRegisterSet.a0,MipsRegisterSet.a0,MipsGenerationHelper.VirtualTableOffset));
            CompilationUnit.TypeEnvironment.GetTypeDefinition(FuncStaticCall.MethodName,FuncStaticCall.SymbolTable,out var coolType);
            var virtualTable=CompilationUnit.MethodEnvironment.GetVirtualTable(coolType);
            var virtualMethod = virtualTable.Single(x => x.Name == FuncStaticCall.MethodName);
            int index = virtualTable.IndexOf(virtualMethod);
            int offset = 4 * index;
            //loading virtual_table.f in a0
            result.SectionCode.Append(MipsGenerationHelper.NewScript().LoadFromMemory(MipsRegisterSet.a0, MipsRegisterSet.a0,offset));

            //TODO: call

            return result;
        }

        public MipsProgram VisitFuncVirtualCall( ASTCILFuncVirtualCallNode FuncVirtualCall )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitGetAttr( ASTCILGetAttrNode GetAttr )
        {
            var type=CompilationUnit.TypeEnvironment.GetContextType(GetAttr.SymbolTable);
            var attroffset = type.GetOffsetForAttribute(GetAttr.AttributeName);
            var result = new MipsProgram();
            //moving self to a0
            result.SectionCode.Append(MipsGenerationHelper.NewScript().LoadFromMemory(MipsRegisterSet.a0, MipsRegisterSet.bp));
            //moving self.attr to a0
            result.SectionCode.Append( MipsGenerationHelper.NewScript().LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, attroffset ) );
            return result;
        }

        public MipsProgram VisitGoto( ASTCILGotoNode Goto )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .JumpToLabel( Goto.Label ) );
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

        public MipsProgram VisitIsVoid( ASTCILIsVoidNode IsVoid )
        {
            throw new NotImplementedException();// TODO hay que hacer esto en IL
        }

        public MipsProgram VisitLessThanTwoVariables( ASTCILLessThanTwoVariablesNode LessThanTwoVariables )
        {
            throw new NotImplementedException();
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

        public MipsProgram VisitNew( ASTCILNewNode New )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitNode( ASTCILNode Node ) => throw new NotImplementedException();

        public MipsProgram VisitProgram( ASTCILProgramNode Program )
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

        public MipsProgram VisitObjectTypeName(ASTCILObjectTypeNameNode objectTypeName)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitObjectCopy(ASTCILObjectCopyNode objectCopy)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitStringConcat(ASTCILStringConcatNode stringConcat)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitStringSubStr(ASTCILStringSubStrNode stringSubStr)
        {
            throw new NotImplementedException();
        }
    }
}
