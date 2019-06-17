using SuperCOOL.CodeGeneration.CIL;
using SuperCOOL.CodeGeneration.CIL.AST;
using SuperCOOL.CodeGeneration.MIPS.Registers;
using SuperCOOL.Constants;
using SuperCOOL.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SuperCOOL.CodeGeneration.MIPS
{
    class CodeGenerator : ICILVisitor<MipsProgram>
    {
        private readonly Dictionary<string, string> StringConstantGenerated;
        private ILabelILGenerator labelGenerator;
        public CompilationUnit CompilationUnit { get; }

        public CodeGenerator( ILabelILGenerator labelGenerator, CompilationUnit compilationUnit )
        {
            this.labelGenerator = labelGenerator;
            this.CompilationUnit = compilationUnit;
            StringConstantGenerated = new Dictionary<string, string>();
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
            var label = labelGenerator.GenerateLabelTypeInfo( Allocate.Type.Name );

            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadFromAddress( MipsRegisterSet.t0, label )
                                                           .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.t0, MipsGenerationHelper.SizeOfOffset )
                                                           .Add( MipsRegisterSet.a0, 4 )
                                                           .Allocate( MipsRegisterSet.a0, MipsRegisterSet.a0 )
                                                           .SaveToMemory( MipsRegisterSet.t0, MipsRegisterSet.a0 )
                                                           .Add( MipsRegisterSet.a0, 4 ) );
            return result;
        }

        public MipsProgram VisitAssignment( ASTCILAssignmentNode Assignment )
        {
            var result = Assignment.Expresion.Accept( this );

            var symbolInfo = Assignment.Identifier;
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Move( MipsRegisterSet.t0, MipsRegisterSet.a0 ) );

            if( symbolInfo.Kind == ObjectKind.Local )
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .GetLocalAddress( symbolInfo.Offset ) );
            else
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .GetParamAddress( symbolInfo.Offset ) );

            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .SaveToMemory( MipsRegisterSet.t0, MipsRegisterSet.a0 ) );
            return result;
        }

        public MipsProgram VisitBlock( ASTCILBlockNode Block )
        {
            var result = new MipsProgram();
            foreach( var e in Block.Expressions )
                result += e.Accept( this );

            return result;
        }

        public MipsProgram VisitBoolConstant( ASTCILBoolConstantNode BoolConstant )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadConstant( MipsRegisterSet.a0,
                                                                BoolConstant.Value ? MipsGenerationHelper.TRUE : MipsGenerationHelper.FALSE ) );
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

        public MipsProgram VisitCase( ASTCILCaseNode caseNode )
        {
            var result = new MipsProgram();
            result += caseNode.ExpressionCase.Accept( this );
            var (endLabel, elseLabel, _) = labelGenerator.GenerateIf();
            var (endLabelRuntimeError, _, _) = labelGenerator.GenerateIf();
            //check if case expression is null and throw runtime error
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                .Move( MipsRegisterSet.a2, MipsRegisterSet.a0 )
                .IsVoid( labelGenerator.GenerateVoid(), elseLabel, endLabel )
                .BranchOnEquals( MipsRegisterSet.a0, 0, endLabelRuntimeError )
                .ThrowRuntimeError( RuntimeErrors.CaseVoidRuntimeError, labelGenerator )
                .Tag( endLabelRuntimeError )
                .LoadFromMemory( MipsRegisterSet.a1, MipsRegisterSet.a2, MipsGenerationHelper.TypeInfoOffest ) );

            var (endWhileLabel, elseWhileLabel, initWhileLabel) = labelGenerator.GenerateIf();

            //load type_name
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                .Tag( initWhileLabel )
                .Move( MipsRegisterSet.t2, MipsRegisterSet.a1 )
                .LoadFromMemory( MipsRegisterSet.a1, MipsRegisterSet.a1, MipsGenerationHelper.TypeNameOffset ) );
            foreach( var currentCase in caseNode.Cases )
            {
                var currentIfLabel = labelGenerator.GenerateIf();
                result.SectionCode.Append(
                    MipsGenerationHelper.NewScript()
                        .LoadFromAddress( MipsRegisterSet.t0,
                            labelGenerator.GenerateLabelTypeName( currentCase.type.Name ).@object )
                        .BranchOnEquals( MipsRegisterSet.a1, MipsRegisterSet.t0, currentIfLabel.@else )
                        .JumpToLabel( currentIfLabel.end )
                        .Tag( currentIfLabel.@else )
                        .GetLocalAddress( currentCase.symbolInfo.Offset )
                        .SaveToMemory( MipsRegisterSet.a2, MipsRegisterSet.a0 ) );
                result += currentCase.expression.Accept( this );
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                    .JumpToLabel( endWhileLabel )
                    .Tag( currentIfLabel.end ) );
            }
            var (endLabelNotMatch, elseLabelNotMatch, _) = labelGenerator.GenerateIf();
            var (endLabelRuntimeErrorNotMatch, _, _) = labelGenerator.GenerateIf();

            //move to parent and throw runtime error if not parent is void
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                .LoadFromMemory( MipsRegisterSet.a1, MipsRegisterSet.t2, MipsGenerationHelper.TypeInfoOffsetParent )
                .Move( MipsRegisterSet.a0, MipsRegisterSet.a1 )
                .IsVoid( labelGenerator.GenerateVoid(), elseLabelNotMatch, endLabelNotMatch )
                .BranchOnEquals( MipsRegisterSet.a0, 0, endLabelRuntimeErrorNotMatch )
                .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a2, MipsGenerationHelper.TypeInfoOffest )
                .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeNameOffset )
                .LoadFromMemory( MipsRegisterSet.t0, MipsRegisterSet.a0 )
                .ThrowRuntimeErrorAdditionalMsg( RuntimeErrors.CaseWithoutMatching, labelGenerator, MipsRegisterSet.t0 )
                .Tag( endLabelRuntimeErrorNotMatch )
                .JumpToLabel( initWhileLabel )
                .Tag( endWhileLabel ) );

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

        public MipsProgram VisitExpression( ASTCILExpressionNode Expression ) => Expression.Accept( this );

        public MipsProgram VisitEqualString( ASTCILEqualStringNode EqualString )
        {
            var left = EqualString.Left.Accept( this );
            left.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                         .Push( MipsRegisterSet.a0 ) );

            var right = EqualString.Right.Accept( this );
            right.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                          .Push( MipsRegisterSet.a0 )
                                                          .LoadFromAddress( MipsRegisterSet.a0, MipsGenerationHelper.StringEqualsLabel )
                                                          .Call( MipsRegisterSet.a0 )
                                                          .Return() );

            return left + right;
        }

        public MipsProgram VisitFunc( ASTCILFuncNode Func )
        {
            var result = new MipsProgram();
            var body = new MipsProgram();
            foreach( var item in Func.Body )
                body += item.Accept( this );

            var off = Func.Method.Locals.Count() * 4;

            result.SectionTextGlobals.Append( MipsGenerationHelper.NewScript().GlobalSection( Func.Tag ) );
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript().Tag( Func.Tag ) );
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript().Sub( MipsRegisterSet.sp, off, MipsRegisterSet.sp ) );
            result.SectionFunctions.Append( body.SectionCode );
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript().Return() );

            result.SectionData.Append( body.SectionData );
            return result;
        }

        public MipsProgram VisitFuncStaticCall( ASTCILFuncStaticCallNode FuncStaticCall )
        {
            var result = new MipsProgram();
            var cant = FuncStaticCall.Arguments.Count;
            result.SectionCode.Append( MipsGenerationHelper.NewScript().Sub( MipsRegisterSet.sp, 4 * cant ) );
            for( int i = 0 ; i < cant ; i++ )
            {
                result += FuncStaticCall.Arguments[ i ].Accept( this );// leave in a0 expresion result
                if( i == 0 )
                {
                    var (endLabel, elseLabel, _) = labelGenerator.GenerateIf();
                    result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                .LoadFromAddress( MipsRegisterSet.t0, labelGenerator.GenerateVoid() )
                                                .BranchOnEquals( MipsRegisterSet.t0, MipsRegisterSet.a0, elseLabel )
                                                .JumpToLabel( endLabel )
                                                .Tag( elseLabel )
                                                //.LoadConstant(MipsRegisterSet.a0, MipsGenerationHelper.TRUE) TODO: throw void execption dispatch
                                                .Tag( endLabel ) );
                }
                result.SectionCode.Append( MipsGenerationHelper.NewScript().SaveToMemory( MipsRegisterSet.a0, MipsRegisterSet.sp, 4 * ( i ) ) );
            }
            var virtualTableLabel = labelGenerator.GenerateLabelVirtualTable( FuncStaticCall.Type.Name );
            //loading static virtual_table in a0
            result.SectionCode.Append( MipsGenerationHelper.NewScript().LoadFromAddress( MipsRegisterSet.a0, virtualTableLabel ) );
            var virtualTable = CompilationUnit.MethodEnvironment.GetVirtualTable( FuncStaticCall.Type );
            var virtualMethod = virtualTable.Single( x => x.Name == FuncStaticCall.MethodName );
            int index = virtualTable.IndexOf( virtualMethod );
            int offset = 4 * index;

            // loading virtual_table.f in a0
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, offset )
                                                           .Call( MipsRegisterSet.a0 ) );

            return result;
        }

        public MipsProgram VisitFuncVirtualCall( ASTCILFuncVirtualCallNode FuncVirtualCall )
        {
            var result = new MipsProgram();
            var cant = FuncVirtualCall.Arguments.Count;
            result.SectionCode.Append( MipsGenerationHelper.NewScript().Sub( MipsRegisterSet.sp, 4 * cant ) );
            for( int i = 0 ; i < cant ; i++ )
            {
                result += FuncVirtualCall.Arguments[ i ].Accept( this );// leave in a0 expresion result
                if( i == 0 )
                {
                    var (endLabel, elseLabel, _) = labelGenerator.GenerateIf();
                    result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                .LoadFromAddress( MipsRegisterSet.t0, labelGenerator.GenerateVoid() )
                                                .BranchOnEquals( MipsRegisterSet.t0, MipsRegisterSet.a0, elseLabel )
                                                .JumpToLabel( endLabel )
                                                .Tag( elseLabel )
                                                //.LoadConstant(MipsRegisterSet.a0, MipsGenerationHelper.TRUE) TODO: throw void execption dispatch
                                                .Tag( endLabel ) );
                }
                result.SectionCode.Append( MipsGenerationHelper.NewScript().SaveToMemory( MipsRegisterSet.a0, MipsRegisterSet.sp, 4 * ( i ) ) );
            }

            // moving self to a0 
            result.SectionCode.Append( MipsGenerationHelper.NewScript().LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.sp ) );
            //loading self.typeInfo in a0
            result.SectionCode.Append( MipsGenerationHelper.NewScript().LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeInfoOffest ) );
            //loading typeInfo.virtual_table in a0
            result.SectionCode.Append( MipsGenerationHelper.NewScript().LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.VirtualTableOffset ) );
            var virtualTable = CompilationUnit.MethodEnvironment.GetVirtualTable( FuncVirtualCall.Type );
            var virtualMethod = virtualTable.Single( x => x.Name == FuncVirtualCall.MethodName );
            int index = virtualTable.IndexOf( virtualMethod );
            int offset = 4 * index;

            // loading virtual_table.f in a0
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, offset )
                                                           .Call( MipsRegisterSet.a0 )
                                                           .Add( MipsRegisterSet.sp, cant * 4 ) );

            return result;
        }

        public MipsProgram VisitGetAttr( ASTCILGetAttrNode GetAttr )
        {
            var attr_offset = GetAttr.Atribute.Offset;

            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .GetParam( MipsRegisterSet.a0, 0 )
                                                           .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, attr_offset ) );
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
            var info = Id.Name;
            var result = new MipsProgram();

            if( info.Kind == ObjectKind.Local )
            {
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .GetLocal( info.Offset ) );
            }

            if( info.Kind == ObjectKind.Parameter )
            {
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .GetParam( MipsRegisterSet.a0, info.Offset ) );
            }

            return result;
        }

        public MipsProgram VisitIf( ASTCILIfNode If )
        {
            var ifLabel = new MipsProgram();
            ifLabel.SectionCode.Append( MipsGenerationHelper.NewScript().Tag( If.IfLabel ) );
            var @if = ifLabel + If.Condition.Accept( this );
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
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( IOInInt.Tag )
                                                                .ReadInt( MipsRegisterSet.a0 )
                                                                .Return() );
            return result;
        }

        public MipsProgram VisitIOInString( ASTCILIOInStringNode IOInString )
        {
            var tags = labelGenerator.GenerateIf();
            var result = new MipsProgram();
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( IOInString.Tag )
                                                                .ReadString()
                                                                .StringLengthMethod()
                                                                .LoadFromAddress( MipsRegisterSet.a1, MipsGenerationHelper.BufferLabel )
                                                                .Add( MipsRegisterSet.a0, 1 )
                                                                .Move( MipsRegisterSet.t0, MipsRegisterSet.a0 )
                                                                .Allocate( MipsRegisterSet.a0, MipsRegisterSet.a2 )
                                                                .Copy( MipsRegisterSet.a1, MipsRegisterSet.a2, MipsRegisterSet.t0, tags.end, tags.@else )
                                                                .Sub( MipsRegisterSet.a2, MipsRegisterSet.a0 ) );

            result.SectionFunctions.Append( CreateString( MipsRegisterSet.a2, MipsRegisterSet.a0 ).SectionCode );
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Return() );
            return result;
        }

        private MipsProgram CreateString( Register r1, Register r2 )
        {
            string label_info = labelGenerator.GenerateLabelTypeInfo( Types.String );
            var result = new MipsProgram();

            if( r2 == MipsRegisterSet.t1 )
            {
                if( r1 == MipsRegisterSet.t2 )
                    result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                                   .Move( MipsRegisterSet.t0, r1 )
                                                                   .Move( MipsRegisterSet.t2, r2 )
                                                                   .Move( MipsRegisterSet.t1, MipsRegisterSet.t0 ) );
                else
                    result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                                   .Move( MipsRegisterSet.t2, r2 )
                                                                   .Move( MipsRegisterSet.t1, r1 ) );
            }
            else
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .Move( MipsRegisterSet.t1, r1 )
                                                               .Move( MipsRegisterSet.t2, r2 ) );

            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadFromAddress( MipsRegisterSet.t0, label_info )
                                                           .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.t0, MipsGenerationHelper.SizeOfOffset )
                                                           .Add( MipsRegisterSet.a0, 4 )
                                                           .Allocate( MipsRegisterSet.a0, MipsRegisterSet.a0 )
                                                           .SaveToMemory( MipsRegisterSet.t0, MipsRegisterSet.a0 )
                                                           .Add( MipsRegisterSet.a0, 4 )
                                                           .SaveToMemory( MipsRegisterSet.t1, MipsRegisterSet.a0 )
                                                           .SaveToMemory( MipsRegisterSet.t2, MipsRegisterSet.a0, 4 ) );
            return result;
        }

        public MipsProgram VisitIOOutInt( ASTCILIOOutIntNode IOOutInt )
        {
            var result = new MipsProgram();
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( IOOutInt.Tag )
                                                                .GetParam( MipsRegisterSet.a0, 4 )
                                                                .PrintInt( MipsRegisterSet.a0 )
                                                                .GetParam(MipsRegisterSet.a0, 0)
                                                                .Return() );
            return result;
        }

        public MipsProgram VisitIOOutString( ASTCILIOOutStringNode IOOutString )
        {
            var result = new MipsProgram();
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( IOOutString.Tag )
                                                                .GetParam( MipsRegisterSet.a0, 4 )
                                                                .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0 )
                                                                .PrintString( MipsRegisterSet.a0 )
                                                                .GetParam(MipsRegisterSet.a0,0)
                                                                .Return() );
            return result;
        }

        public MipsProgram VisitIsVoid( ASTCILIsVoidNode IsVoid )
        {
            var result = IsVoid.Expression.Accept( this );
            var (endLabel, elseLabel, _) = labelGenerator.GenerateIf();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                .IsVoid( labelGenerator.GenerateVoid(), elseLabel, endLabel ) );

            return result;
        }

        public MipsProgram VisitLessThanTwoVariables( ASTCILLessThanTwoVariablesNode LessThanTwoVariables )
        {
            (string end_label, string else_label, _) = labelGenerator.GenerateIf();

            var left = LessThanTwoVariables.Left.Accept( this );
            left.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                         .Push( MipsRegisterSet.a0 ) );

            var right = LessThanTwoVariables.Right.Accept( this );
            right.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                          .Pop( MipsRegisterSet.t0 )
                                                          .BranchLessThan( MipsRegisterSet.t0, MipsRegisterSet.a0, else_label )
                                                          .LoadConstant( MipsRegisterSet.a0, 0 )
                                                          .JumpToLabel( end_label )
                                                          .Tag( else_label )
                                                          .LoadConstant( MipsRegisterSet.a0, 1 )
                                                          .Tag( end_label ) );
            return left + right;
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

        public MipsProgram VisitNode( ASTCILNode Node ) => throw new NotImplementedException();

        public MipsProgram VisitProgram( ASTCILProgramNode Program )
        {
            var bufferlabel = MipsGenerationHelper.BufferLabel;

            var result = new MipsProgram();
            result.SectionDataGlobals.Append( MipsGenerationHelper.NewScript().GlobalSection( bufferlabel ) );
            result.SectionData.Append( MipsGenerationHelper.NewScript()
                                                           .AddData( bufferlabel, new[] { MipsGenerationHelper.AddDynamycString( MipsGenerationHelper.BufferSize ) } ) );

            List<string> all = new List<string>();
            foreach( var item in RuntimeErrors.GetRuntimeErrorString )
            {
                var exception = labelGenerator.GetException( item.Key );
                all.Add( exception );
                result.SectionDataGlobals.Append( MipsGenerationHelper.NewScript()
                    .GlobalSection( exception ) );
                result.SectionData.Append( MipsGenerationHelper.NewScript()
                                                               .AddData( exception, new[] { MipsGenerationHelper.AddStringData( item.Value ) } ) );
            }

            var exceptions = MipsGenerationHelper.Exceptions;
            result.SectionDataGlobals.Append( MipsGenerationHelper.NewScript()
                .GlobalSection( exceptions ) );
            result.SectionData.Append( MipsGenerationHelper.NewScript()
                                                           .AddData( exceptions, all.Select( x => MipsGenerationHelper.AddIntData( x ) ) ) );

            CompilationUnit.TypeEnvironment.GetTypeDefinition( "Main", null, out var main );
            var entryPoint = new ASTCILAllocateNode( main ).Accept( this ).SectionCode
                .Append( MipsGenerationHelper.NewScript().Push( MipsRegisterSet.a0 ) )
                .Append( new ASTCILFuncVirtualCallNode( main, Functions.Init, new ASTCILExpressionNode[] { } ).Accept( this ).SectionCode )
                .Append( new ASTCILFuncVirtualCallNode( main, "main", new ASTCILExpressionNode[] { } ).Accept( this ).SectionCode )
                .Append( MipsGenerationHelper.NewScript().Add( MipsRegisterSet.sp, 12 ) )
                .Append( MipsGenerationHelper.NewScript().Exit() );


            result.SectionCode.Append( MipsGenerationHelper.NewScript().MainTag() ).Append( entryPoint );

            var voidLabel = labelGenerator.GenerateVoid();
            result.SectionDataGlobals.Append( MipsGenerationHelper.NewScript()
                .GlobalSection( voidLabel ).GlobalSection( labelGenerator.GetNewLine() ) );
            result.SectionData.Append( MipsGenerationHelper.NewScript()
                .AddData( voidLabel, new[] { MipsGenerationHelper.AddStringData( String.Empty ) } ).AddData(
                    labelGenerator.GetNewLine(), new[]
                    {
                        MipsGenerationHelper.AddStringData(Environment.NewLine)
                    } ) );

            result.SectionTextGlobals.Append( MipsGenerationHelper.NewScript().GlobalSection( MipsGenerationHelper.StringEqualsLabel ) );
            result.SectionCode.Append( MipsGenerationHelper.NewScript().StringEquals() );

            foreach( var item in Program.Types )
                result += item.Accept( this );

            return result;
        }

        public MipsProgram VisitRuntimeError( ASTCILRuntimeErrorNode RuntimeError )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                .ThrowRuntimeError( RuntimeError.Id, labelGenerator ) );

            return result;
        }

        public MipsProgram VisitSelf( ASTCILSelfNode Self )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .GetParam( MipsRegisterSet.a0, 0 ) );
            return result;
        }

        public MipsProgram VisitSetAttribute( ASTCILSetAttributeNode SetAttribute )
        {
            var attr_offset = SetAttribute.Atribute.Offset;

            var result = SetAttribute.Expression.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Move( MipsRegisterSet.t0, MipsRegisterSet.a0 )
                                                           .GetParam( MipsRegisterSet.a0, 0 )
                                                           .SaveToMemory( MipsRegisterSet.t0, MipsRegisterSet.a0, attr_offset ) );
            return result;
        }

        public MipsProgram VisitStringConstant(ASTCILStringConstantNode StringConstant)
        {
            var result = new MipsProgram();
            if (!StringConstantGenerated.TryGetValue(StringConstant.Value, out var labelStringConstant))
            {
                result.SectionData.Append(MipsGenerationHelper.NewScript()
                    .AddData(StringConstant.ObjectLabel,
                        new[]
                        {
                            MipsGenerationHelper.AddIntData(
                                labelGenerator.GenerateLabelTypeInfo(CompilationUnit.TypeEnvironment.String.Name)),
                            MipsGenerationHelper.AddIntData(StringConstant.ValueLabel),
                            MipsGenerationHelper.AddIntData(StringConstant.Value.Length)
                        })
                    .Comment(StringConstant.Value)
                    .AddData(StringConstant.ValueLabel, new[]
                    {
                        MipsGenerationHelper.AddByteData(Regex.Unescape(StringConstant.Value).Select(x => (int) x))
                    }));
                StringConstantGenerated.Add(StringConstant.Value, StringConstant.ObjectLabel);
                labelStringConstant = StringConstant.ObjectLabel;
            }

            result.SectionCode.Append(MipsGenerationHelper.NewScript()
                .LoadFromAddress(MipsRegisterSet.a0, labelStringConstant)
                .Add(MipsRegisterSet.a0, 4));
            return result;
        }

        public MipsProgram VisitType( ASTCILTypeNode Type )
        {
            var result = new MipsProgram();
            foreach( var method in Type.Methods )
                result += method.Accept( this );

            var typeName = Type.Type.Name;
            var typeNameParent = Type.Type.Parent?.Name;
            var label_type_name = labelGenerator.GenerateLabelTypeName( typeName );
            result.SectionData.Append( MipsGenerationHelper.NewScript()
                .AddData( label_type_name.value, new[]
                {
                    MipsGenerationHelper.AddStringData(typeName)
                } ) );
            result.SectionData.Append( MipsGenerationHelper.NewScript()
                .AddData( label_type_name.@object, new[]
                {
                    MipsGenerationHelper.AddIntData(label_type_name.value),
                    MipsGenerationHelper.AddIntData(typeName.Length)
                } ) );

            var label_virtual_table = labelGenerator.GenerateLabelVirtualTable( typeName );

            result.SectionData.Append( MipsGenerationHelper.NewScript()
                .AddData( label_virtual_table,
                    Type.VirtualTable.Select( x =>
                         MipsGenerationHelper.AddIntData( labelGenerator.GenerateFunc( x.Type.Name, x.Name ) ) ) ) );

            var typeInfo_label = labelGenerator.GenerateLabelTypeInfo( typeName );
            var typeInfo_label_parent = typeNameParent != null ? labelGenerator.GenerateLabelTypeInfo( typeNameParent ) : labelGenerator.GenerateVoid();
            result.SectionData.Append( MipsGenerationHelper.NewScript()
                .AddData( typeInfo_label, new[]
                {
                    MipsGenerationHelper.AddIntData(label_type_name.@object),
                    MipsGenerationHelper.AddIntData(Type.Type.AllocateSize),
                    MipsGenerationHelper.AddIntData(label_virtual_table),
                    MipsGenerationHelper.AddIntData(typeInfo_label_parent)
                } ) );

            result.SectionDataGlobals.Append( MipsGenerationHelper.NewScript().GlobalSection( label_type_name.value )
                .GlobalSection( label_type_name.@object )
                .GlobalSection( label_virtual_table ).GlobalSection( typeInfo_label ) );

            return result;
        }

        public MipsProgram VisitVoid( ASTCILVoidNode Void )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript().LoadFromAddress( MipsRegisterSet.a0, labelGenerator.GenerateVoid() ) );
            return result;
        }

        public MipsProgram VisitObjectTypeName( ASTCILObjectTypeNameNode objectTypeName )
        {
            var result = new MipsProgram();
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( objectTypeName.Tag )
                                                                .GetParam( MipsRegisterSet.a0, 0 )
                                                                .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeInfoOffest )
                                                                .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeNameOffset )
                                                                .Return() );
            return result;
        }

        public MipsProgram VisitObjectCopy( ASTCILObjectCopyNode objectCopy )
        {
            var result = new MipsProgram();
            var tags = labelGenerator.GenerateIf();
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( objectCopy.Tag )
                                                                .GetParam( MipsRegisterSet.a0, 0 ) // a0<- self
                                                                .LoadFromMemory( MipsRegisterSet.t0, MipsRegisterSet.a0, MipsGenerationHelper.TypeInfoOffest )//t0 <- self.type_info
                                                                .LoadFromMemory( MipsRegisterSet.t1, MipsRegisterSet.t0, MipsGenerationHelper.SizeOfOffset )//t1 <- self.size
                                                                .Add( MipsRegisterSet.t1, 4 )
                                                                .Allocate( MipsRegisterSet.t1, MipsRegisterSet.a0 )//a0<- new (allocate does not use t0 or t1)
                                                                .SaveToMemory( MipsRegisterSet.t0, MipsRegisterSet.a0 )//putting self.typeInfo in a0 typeinfo area
                                                                .Add( MipsRegisterSet.a0, 4, MipsRegisterSet.t0 )//to copy in t0
                                                                .Sub( MipsRegisterSet.t1, 4 )//Size Copy in t1
                                                                .GetParam( MipsRegisterSet.a0, 0 )//From copy (self in a0)
                                                                .Copy( MipsRegisterSet.a0, MipsRegisterSet.t0, MipsRegisterSet.t1, tags.end, tags.@else )//word to word copy
                                                                .Move( MipsRegisterSet.a0, MipsRegisterSet.t0 )
                                                                .Return() );
            return result;
        }

        public MipsProgram VisitStringConcat( ASTCILStringConcatNode stringConcat )
        {
            var tags1 = labelGenerator.GenerateIf();
            var tags2 = labelGenerator.GenerateIf();
            var result = new MipsProgram();
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( stringConcat.Tag )
                                                                .GetParam( MipsRegisterSet.a1, 0 )
                                                                .GetParam( MipsRegisterSet.a2, 4 )
                                                                .LoadFromMemory( MipsRegisterSet.t1, MipsRegisterSet.a1, 4 )
                                                                .LoadFromMemory( MipsRegisterSet.t2, MipsRegisterSet.a2, 4 )
                                                                .Add( MipsRegisterSet.t2, 1 )
                                                                .Add( MipsRegisterSet.t1, MipsRegisterSet.t2, MipsRegisterSet.t0 )
                                                                .Allocate( MipsRegisterSet.t0, MipsRegisterSet.a0 )
                                                                .LoadFromMemory( MipsRegisterSet.a1, MipsRegisterSet.a1 )
                                                                .LoadFromMemory( MipsRegisterSet.a2, MipsRegisterSet.a2 )
                                                                .Copy( MipsRegisterSet.a1, MipsRegisterSet.a0, MipsRegisterSet.t1, tags1.end, tags1.@else )

                                                                .Copy( MipsRegisterSet.a2, MipsRegisterSet.a0, MipsRegisterSet.t2, tags2.end, tags2.@else )
                                                                .Sub( MipsRegisterSet.a0, MipsRegisterSet.t0 ) );
            result.SectionFunctions.Append( CreateString( MipsRegisterSet.a0, MipsRegisterSet.t0 ).SectionCode );
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Return() );
            return result;
        }

        public MipsProgram VisitStringSubStr( ASTCILStringSubStrNode stringSubStr )
        {
            var tag1 = labelGenerator.GenerateIf();
            var tag2 = labelGenerator.GenerateIf();
            var tag3 = labelGenerator.GenerateIf();
            var result = new MipsProgram();
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( stringSubStr.Tag )
                                                                .GetParam( MipsRegisterSet.a1, 0 )
                                                                .LoadFromMemory( MipsRegisterSet.t0, MipsRegisterSet.a1, 4 )
                                                                .LoadFromMemory( MipsRegisterSet.a1, MipsRegisterSet.a1 )
                                                                .GetParam( MipsRegisterSet.a2, 4 )
                                                                .BranchLessThan( MipsRegisterSet.a2, 0, tag1.@else )
                                                                .GetParam( MipsRegisterSet.a3, 8 )
                                                                .BranchLessThan( MipsRegisterSet.a3, 0, tag1.@else )
                                                                .Add( MipsRegisterSet.a2, MipsRegisterSet.a3, MipsRegisterSet.t1 )
                                                                .BranchLessThan( MipsRegisterSet.t0, MipsRegisterSet.t1, tag1.@else )
                                                                .Add( MipsRegisterSet.a3, 1, MipsRegisterSet.t1 )
                                                                .Allocate( MipsRegisterSet.t1, MipsRegisterSet.t0 )
                                                                .Copy( MipsRegisterSet.a1, MipsRegisterSet.t0, MipsRegisterSet.a3, tag2.end, tag2.@else )
                                                                .SaveByte( MipsRegisterSet.zero, MipsRegisterSet.t0 )
                                                                .Sub( MipsRegisterSet.t0, MipsRegisterSet.t1 )
                                                                .Add( MipsRegisterSet.t0, 1 ) );
            result.SectionFunctions.Append( CreateString( MipsRegisterSet.t0, MipsRegisterSet.a3 ).SectionCode );
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Return()
                                                                .Tag( tag1.@else )
                                                                .ThrowRuntimeError( RuntimeErrors.SubStringOutOfRange, labelGenerator ) );
            return result;
        }

        public MipsProgram VisitUnboxing(ASTCILUnboxingNode Unboxing)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoxing(ASTCILBoxingNode Boxing)
        {
            throw new NotImplementedException();
        }
    }
}
