using SuperCOOL.CodeGeneration.CIL;
using SuperCOOL.CodeGeneration.CIL.AST;
using SuperCOOL.CodeGeneration.MIPS.Registers;
using SuperCOOL.Constants;
using SuperCOOL.Core;

using System;
using System.Collections.Generic;
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
            var label = labelGenerator.GenerateLabelTypeInfo( Allocate.Type );

            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadFromMemoryLabel( MipsRegisterSet.t0, label )
                                                           .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.t0, MipsGenerationHelper.SizeOfOffset )
                                                           .Add( MipsRegisterSet.a0, 4 )
                                                           .Allocate( MipsRegisterSet.a0 )
                                                           .SaveToMemory( MipsRegisterSet.t0, MipsRegisterSet.a0 )
                                                           .Add( MipsRegisterSet.a0, 4 ) );
            return result;
        }

        public MipsProgram VisitAssignment( ASTCILAssignmentNode Assignment )
        {
            var result = Assignment.Expresion.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Move( MipsRegisterSet.t0, MipsRegisterSet.a0 ) );

            Assignment.SymbolTable.IsDefObject( Assignment.Identifier, out var symbolInfo );
            if( symbolInfo.Kind == ObjectKind.Local )
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .GetLocal( symbolInfo.Offset ) );
            else
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .GetParam( symbolInfo.Offset ) );

            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .SaveToMemory( MipsRegisterSet.a0, MipsRegisterSet.t0 ) );
            return result;
        }

        public MipsProgram VisitBlock(ASTCILBlockNode Block)
        {
            var result = new MipsProgram();
            foreach (var e in Block.Expressions)
                result += e.Accept(this);

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

        public MipsProgram VisitFunc( ASTCILFuncNode Func )
        {
            var locals = Func.SymbolTable.GetLocals().Where( x => x.Kind == ObjectKind.Local ).Select( ( x, i ) => x.Offset = 4 * i );
            var parameters = Func.SymbolTable.AllDefinedObjects().Where( x => x.Kind == ObjectKind.Parameter ).Select( ( x, i ) => x.Offset = 4 * ( i + 1 ) );

            var result = new MipsProgram();
            var body = new MipsProgram();
            foreach (var item in Func.Body)
                body += item.Accept(this);

            var off = locals.Count() * 4;

            result.SectionTextGlobals.Append(MipsGenerationHelper.NewScript().GlobalSection(Func.Name));
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( Func.Name )
                                                                .Sub( MipsRegisterSet.sp, off, MipsRegisterSet.sp ) );
            result.SectionFunctions.Append( body.SectionCode );
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Return() );

            result.SectionData.Append(body.SectionData);
            return result;
        }

        public MipsProgram VisitFuncStaticCall( ASTCILFuncStaticCallNode FuncStaticCall )
        {
            var result = new MipsProgram();
            foreach( var arg in FuncStaticCall.Arguments.Reverse() )
            {
                result += arg.Accept( this ); // leave in a0 expresion result
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .Push( MipsRegisterSet.a0 ) );
            }

            var virtualTableLabel = labelGenerator.GenerateLabelVirtualTable( FuncStaticCall.Type );
            //loading static virtual_table in a0
            result.SectionCode.Append( MipsGenerationHelper.NewScript().LoadFromMemoryLabel( MipsRegisterSet.a0, virtualTableLabel ) );
            CompilationUnit.TypeEnvironment.GetTypeDefinition( FuncStaticCall.Type, FuncStaticCall.SymbolTable, out var coolType );
            var virtualTable = CompilationUnit.MethodEnvironment.GetVirtualTable( coolType );
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
            foreach( var arg in FuncVirtualCall.Arguments.Reverse() )
            {
                result += arg.Accept( this ); // leave in a0 expresion result
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .Push( MipsRegisterSet.a0 ) );
            }

            // moving self to a0 not necesary self is already in ao.
            //result.SectionCode.Append(MipsGenerationHelper.NewScript().LoadMemory(MipsRegisterSet.a0, MipsRegisterSet.fp, MipsGenerationHelper.SelfOffset));
            //loading self.typeInfo in a0
            result.SectionCode.Append( MipsGenerationHelper.NewScript().LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeInfoOffest ) );
            //loading typeInfo.virtual_table in a0
            result.SectionCode.Append( MipsGenerationHelper.NewScript().LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.VirtualTableOffset ) );
            var coolType=CompilationUnit.TypeEnvironment.GetContextType( FuncVirtualCall.SymbolTable);
            var virtualTable = CompilationUnit.MethodEnvironment.GetVirtualTable( coolType );
            var virtualMethod = virtualTable.Single( x => x.Name == FuncVirtualCall.MethodName);
            int index = virtualTable.IndexOf( virtualMethod );
            int offset = 4 * index;

            // loading virtual_table.f in a0
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, offset )
                                                           .Call( MipsRegisterSet.a0 )
                                                           .Add(MipsRegisterSet.sp, FuncVirtualCall.Arguments.Count * 4) );

            return result;
        }

        public MipsProgram VisitGetAttr( ASTCILGetAttrNode GetAttr )
        {
            var type = this.CompilationUnit.TypeEnvironment.GetContextType( GetAttr.SymbolTable );
            var attr_offset = type.GetOffsetForAttribute( GetAttr.AttributeName );

            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .GetParam( 0 )
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
            var locals = Id.SymbolTable.IsDefObject( Id.Name, out var info );
            var result = new MipsProgram();

            if( info.Kind == ObjectKind.Local )
            {
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .GetLocal( info.Offset ) );
            }

            if( info.Kind == ObjectKind.Parameter )
            {
                result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                               .GetParam( info.Offset ) );
            }

            return result;
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
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( IOInInt.Name )
                                                                .ReadInt( MipsRegisterSet.a0 )
                                                                .Return() );
            return result;
        }

        public MipsProgram VisitIOInString( ASTCILIOInStringNode IOInString )
        {
            //TODO: falta
            var result = new MipsProgram();
            result.SectionFunctions.Append(MipsGenerationHelper.NewScript()
                                                                .Tag(IOInString.Name)
                                                                .Return());
            return result;
        }

        public MipsProgram VisitIOOutInt( ASTCILIOOutIntNode IOOutInt )
        {
            //TODO: falta
            var result = new MipsProgram();
            result.SectionFunctions.Append(MipsGenerationHelper.NewScript()
                                                                .Tag(IOOutInt.Name)
                                                                .Return());
            return result;
        }

        public MipsProgram VisitIOOutString( ASTCILIOOutStringNode IOOutString )
        {
            //TODO: falta
            var result = new MipsProgram();
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( IOOutString.Name )
                                                                .GetParam( 4 )
                                                                .PrintString( MipsRegisterSet.a0 )
                                                                .Return() );
            return result;
        }

        public MipsProgram VisitIsVoid( ASTCILIsVoidNode IsVoid )
        {
            throw new NotImplementedException();// TODO hay que hacer esto en IL
        }

        public MipsProgram VisitLessThanTwoVariables( ASTCILLessThanTwoVariablesNode LessThanTwoVariables )
        {
            (string end_label, string else_label) = labelGenerator.GenerateIf();

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
            var bufferlabel = labelGenerator.GetBuffer();

            var result = new MipsProgram();
            result.SectionDataGlobals.Append(MipsGenerationHelper.NewScript().GlobalSection(bufferlabel));
            result.SectionData.Append( MipsGenerationHelper.NewScript()
                                                           .AddData( bufferlabel, new[] { MipsGenerationHelper.AddDynamycString( MipsGenerationHelper.BufferSize ) } ) );

            List<string> all = new List<string>();
            foreach( var item in RuntimeErrors.GetRuntimeErrorString )
            {
                var exception = labelGenerator.GetException();
                all.Add( exception );
                result.SectionDataGlobals.Append(MipsGenerationHelper.NewScript()
                    .GlobalSection(exception));
                result.SectionData.Append( MipsGenerationHelper.NewScript()
                                                               .AddData( exception, new[] { MipsGenerationHelper.AddStringData( item.Value ) } ) );
            }

            var exceptions = MipsGenerationHelper.Exceptions;
            result.SectionDataGlobals.Append(MipsGenerationHelper.NewScript()
                .GlobalSection(exceptions));
            result.SectionData.Append( MipsGenerationHelper.NewScript()
                                                           .AddData( exceptions, all.Select( x => MipsGenerationHelper.AddIntData( x ) ) ) );

            CompilationUnit.TypeEnvironment.GetTypeDefinition("Main", Program.SymbolTable, out var main);
            var entryPoint = new ASTCILBlockNode(new ASTCILExpressionNode[] {
                                                    new ASTCILAllocateNode(main.Name,main.SymbolTable),
                                                    new ASTCILFuncVirtualCallNode("_init",new ASTCILExpressionNode[]{ },main.SymbolTable),
                                                    new ASTCILFuncVirtualCallNode("main",new ASTCILExpressionNode[]{ },main.SymbolTable),
                                                    }, Program.SymbolTable);

            result.SectionCode.Append(MipsGenerationHelper.NewScript().MainTag()).Append(entryPoint.Accept(this).SectionCode);

            foreach( var item in Program.Types )
                result += item.Accept( this );

            return result;
        }

        public MipsProgram VisitRuntimeError( ASTCILRuntimeErrorNode RuntimeError )
        {
            //TODO: falta
            var result = new MipsProgram();
            return result;
        }

        public MipsProgram VisitSelf( ASTCILSelfNode Self )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .GetParam( 0 ) );
            return result;
        }

        public MipsProgram VisitSetAttribute( ASTCILSetAttributeNode SetAttribute )
        {
            var type = this.CompilationUnit.TypeEnvironment.GetContextType( SetAttribute.SymbolTable );
            var attr_offset = type.GetOffsetForAttribute( SetAttribute.AttributeName );

            var result = SetAttribute.Expression.Accept( this );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .Move( MipsRegisterSet.t0, MipsRegisterSet.a0 )
                                                           .GetParam( 0 )
                                                           .SaveToMemory( MipsRegisterSet.t0, MipsRegisterSet.a0, attr_offset ) );
            return result;
        }

        public MipsProgram VisitStringConstant( ASTCILStringConstantNode StringConstant )
        {
            var result = new MipsProgram();
            result.SectionData.Append( MipsGenerationHelper.NewScript()
                                                           .AddData( StringConstant.DataLabel, new List<(string, object)> { MipsGenerationHelper.AddStringData( StringConstant.Value ) } ) );
            result.SectionCode.Append( MipsGenerationHelper.NewScript()
                                                           .LoadFromAddress( MipsRegisterSet.a0, StringConstant.DataLabel ) );
            return result;
        }

        public MipsProgram VisitType(ASTCILTypeNode Type)
        {
            var result = new MipsProgram();
            foreach (var method in Type.Methods)
                result += method.Accept(this);

            var typeName = Type.Type.Name;
            var label_type_name = labelGenerator.GenerateLabelTypeName(typeName);
            result.SectionData.Append(MipsGenerationHelper.NewScript()
                .AddData(label_type_name, new[] { MipsGenerationHelper.AddStringData(typeName) }));

            var label_virtual_table = labelGenerator.GenerateLabelVirtualTable(typeName);
            result.SectionData.Append(MipsGenerationHelper.NewScript()
                .AddData(label_virtual_table,
                    Type.VirtualTable.Select(x =>
                        MipsGenerationHelper.AddIntData(labelGenerator.GenerateFunc(x.Type.Name, x.Name)))));

            var typeInfo_label = labelGenerator.GenerateLabelTypeInfo(typeName);
            result.SectionData.Append(MipsGenerationHelper.NewScript()
                .AddData(typeInfo_label, new[]
                {
                    MipsGenerationHelper.AddIntData(label_type_name),
                    MipsGenerationHelper.AddIntData(Type.AllocateSize),
                    MipsGenerationHelper.AddIntData(label_virtual_table)
                }));

            result.SectionDataGlobals.Append(MipsGenerationHelper.NewScript().GlobalSection(label_type_name)
                .GlobalSection(label_virtual_table).GlobalSection(typeInfo_label));

            return result;
        }

        public MipsProgram VisitVoid( ASTCILVoidNode Void )
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitObjectTypeName( ASTCILObjectTypeNameNode objectTypeName )
        {
            var result = new MipsProgram();
            result.SectionFunctions.Append( MipsGenerationHelper.NewScript()
                                                                .Tag( objectTypeName.Name )
                                                                .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.bp )
                                                                .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeInfoOffest )
                                                                .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeNameOffset )
                                                                .Return() );
            return result;
        }

        public MipsProgram VisitObjectCopy( ASTCILObjectCopyNode objectCopy )
        {
            //TODO: falta
            var result = new MipsProgram();
            result.SectionFunctions.Append(MipsGenerationHelper.NewScript()
                                                                .Tag(objectCopy.Name)
                                                                .GetParam(4)
                                                                .PrintString(MipsRegisterSet.a0)
                                                                .Return());
            return result;
        }

        public MipsProgram VisitStringConcat( ASTCILStringConcatNode stringConcat )
        {
            //TODO: falta
            var result = new MipsProgram();
            result.SectionFunctions.Append(MipsGenerationHelper.NewScript()
                                                                .Tag(stringConcat.Name)
                                                                .GetParam(4)
                                                                .PrintString(MipsRegisterSet.a0)
                                                                .Return());
            return result;
        }

        public MipsProgram VisitStringSubStr( ASTCILStringSubStrNode stringSubStr )
        {
            //TODO: falta
            var result = new MipsProgram();
            result.SectionFunctions.Append(MipsGenerationHelper.NewScript()
                                                                .Tag(stringSubStr.Name)
                                                                .GetParam(4)
                                                                .PrintString(MipsRegisterSet.a0)
                                                                .Return());
            return result;
        }
    }
}
