using SuperCOOL.CodeGeneration.MIPS.Registers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.MIPS
{
    public class MipsGenerationHelper
    {
        // Constants
        public const int TRUE = 1, FALSE = 0, NULL = 0;

        public static readonly string TAB = "\t";
        private static readonly string ENDL = Environment.NewLine;

        // Read string buffer
        public const int BufferSize = 4096;
        public const string BufferLabel = "_____buffer";
        public const string StringLengthMethodLabel = "_____string_length";
        public const string StringLengthMethodWhile = "_____string_length_while";
        public const string StringLengthMethodWhileEnd = "_____string_length_while_end";
        public const string StringEqualsLabel = "_____string_equals";
        public const string StringEqualsLabelWhile = "_____string_equals_while";
        public const string StringEqualsLabelFalse = "_____string_equals_false";
        public const string StringEqualsLabelEnd = "_____string_equals_end";

        // Exceptions
        public const string Exceptions = "____exceptions";

        //Variable
        internal static readonly int TypeInfoOffest = -4;

        //TypeInfo
        public const int TypeNameOffset = 0;
        public const int SizeOfOffset = 4;
        public const int VirtualTableOffset = 8;
        public const int TypeInfoOffsetParent = 12;


        private StringBuilder body;

        private MipsGenerationHelper( StringBuilder body = null ) => this.body = new StringBuilder().Append( body ?? new StringBuilder() );

        public static MipsGenerationHelper NewScript() => new MipsGenerationHelper();

        public static implicit operator string( MipsGenerationHelper a ) => a.body.ToString();

        public static MipsGenerationHelper operator +( MipsGenerationHelper a, StringBuilder b ) =>
            new MipsGenerationHelper( new StringBuilder().Append( a ).Append( b ) );

        public static MipsGenerationHelper operator +( StringBuilder a, MipsGenerationHelper b ) =>
            new MipsGenerationHelper( new StringBuilder().Append( a ).Append( b ) );


        // System call codes
        private const int print_int = 1;
        private const int print_string = 4;
        private const int read_int = 5;
        private const int read_string = 8;
        private const int allocate = 9;
        private const int exit = 10;


        // Sections
        public MipsGenerationHelper Section( string section_name, string arg = "", bool Indented = false ) // .{ section_name } arg
        {
            this.body.Append( $"{( Indented ? TAB : "" )}.{ section_name }{ ( arg == "" ? "" : " " + arg ) }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper DataSection() => Section( "data" ); // .data

        public MipsGenerationHelper TextSection() => Section( "text" ); // .text

        public MipsGenerationHelper GlobalSection( string label ) => Section( "globl", label, true ); // .globl { label }


        // Tags
        public MipsGenerationHelper Tag( string tag_name ) // { tag_name }:
        {
            this.body.Append( $"{ TAB + tag_name }:{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper MainTag() => Tag( "main" ); // main:


        // Comments
        public MipsGenerationHelper Comment( string comment )
        {
            this.body.Append( $"{ TAB }# { comment }{ ENDL }" );
            return this;
        }


        // Newline
        public MipsGenerationHelper NewLine()
        {
            this.body.Append( ENDL );
            return this;
        }


        // Data Types
        public MipsGenerationHelper AddData( string name, IEnumerable<(string type, object value)> args ) => this.Tag( name ) + new StringBuilder( args.Select( x => $"{ TAB + TAB }.{ x.type } { x.value }{ ENDL }" ).Aggregate( ( x, y ) => x.ToString() + y.ToString() ) );

        public static (string, object) AddStringData( string value ) => ($"asciiz", $"\"{ value }\"");

        public static (string, object) AddByteData(IEnumerable<int> bytes) => ($"byte", $"{string.Join(',', bytes)},0");

        public static (string, object) AddIntData( object value ) => ($"word", value);

        public static (string, object) AddDynamycString( int space ) => ($"space", space);


        // Exit
        public MipsGenerationHelper Exit() => this.LoadConstant( MipsRegisterSet.v0, exit )
                                                  .SystemCall();


        // System call
        public MipsGenerationHelper SystemCall() // syscall
        {
            this.body.Append( $"{ TAB + TAB }syscall{ ENDL }" );
            return this;
        }


        // Functions Call
        // When changing stack in this method check correctness with return method.
        public MipsGenerationHelper Call( Register r ) => this.Push( MipsRegisterSet.ip )
                                                              .Push( MipsRegisterSet.bp )
                                                              .Move( MipsRegisterSet.bp, MipsRegisterSet.sp )
                                                              .JumpToLabelInRegistry( r )
                                                              .Move( MipsRegisterSet.sp, MipsRegisterSet.bp )
                                                              .Pop( MipsRegisterSet.bp )
                                                              .Pop( MipsRegisterSet.ip );

        public MipsGenerationHelper Return() => this.JumpRegister( MipsRegisterSet.ip );

        // Getting args and variables in functions
        public MipsGenerationHelper GetParam( Register r, int offset ) => this.LoadFromMemory( r, MipsRegisterSet.bp, offset + 8 );
        public MipsGenerationHelper GetParamAddress( int offset ) => this.Move( MipsRegisterSet.a0, MipsRegisterSet.bp ).Add( MipsRegisterSet.a0, offset + 8 );

        public MipsGenerationHelper GetLocal( int offset ) => this.LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.bp, -offset );

        public MipsGenerationHelper GetLocalAddress( int offset ) =>
            this.Move( MipsRegisterSet.a0, MipsRegisterSet.bp ).Sub( MipsRegisterSet.a0, offset );

        // Read
        public MipsGenerationHelper ReadInt( Register r ) => this.LoadConstant( MipsRegisterSet.v0, read_int )
                                                                 .SystemCall()
                                                                 .Move( r, MipsRegisterSet.v0 );

        // TODO
        public MipsGenerationHelper ReadString()
        {
            this.LoadConstant( MipsRegisterSet.v0, read_string )
                .LoadFromAddress( MipsRegisterSet.a0, BufferLabel )
                .LoadConstant( MipsRegisterSet.a1, BufferSize )
                .SystemCall();
            return this;
        }


        // String length
        public MipsGenerationHelper StringLengthMethod() => this.Move( MipsRegisterSet.a1, MipsRegisterSet.a0 )
                                                                .Tag( StringLengthMethodWhile )
                                                                .LoadByte( MipsRegisterSet.t0, MipsRegisterSet.a1 )
                                                                .BranchOnEquals( MipsRegisterSet.t0, MipsRegisterSet.zero, StringLengthMethodWhileEnd )
                                                                .BranchOnEquals( MipsRegisterSet.t0, 10, StringLengthMethodWhileEnd )
                                                                .Add( MipsRegisterSet.a1, 1 )
                                                                .JumpToLabel( StringLengthMethodWhile )
                                                                .Tag( StringLengthMethodWhileEnd )
                                                                .SaveByte( MipsRegisterSet.zero, MipsRegisterSet.a1 )
                                                                .Sub( MipsRegisterSet.a1, MipsRegisterSet.a0, MipsRegisterSet.a0 );


        // Print
        public MipsGenerationHelper PrintInt( Register r ) => this.LoadConstant( MipsRegisterSet.v0, print_int )
                                                                  .Move( MipsRegisterSet.a0, r )
                                                                  .SystemCall();

        public MipsGenerationHelper PrintInt( int d ) => this.LoadConstant( MipsRegisterSet.a0, d )
                                                             .PrintInt( MipsRegisterSet.a0 );

        public MipsGenerationHelper PrintString( string name ) => this.LoadConstant( MipsRegisterSet.v0, print_string )
                                                                      .LoadFromAddress( MipsRegisterSet.a0, name )
                                                                      .SystemCall();

        public MipsGenerationHelper PrintString( Register r ) => this.Move( MipsRegisterSet.a0, r ).LoadConstant( MipsRegisterSet.v0, print_string )
                                                     .SystemCall();


        // Move
        public MipsGenerationHelper Move( Register r1, Register r2 ) // r1 <- r2
        {
            if( r1 != r2 )
                this.body.Append( $"{ TAB + TAB }move { r1 }, { r2 }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper MoveFromLo( Register r ) // r <- $lo
        {
            this.body.Append( $"{ TAB + TAB }mflo { r }{ ENDL }" );
            return this;
        }


        // Load
        public MipsGenerationHelper LoadConstant( Register r, int c ) // r <- c
        {
            this.body.Append( $"{ TAB + TAB }li { r }, { c }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper LoadFromMemory( Register r, object d, int offset = 0 ) => LoadFromMemoryLabel( r, $"({d})", offset ); // r <- (d) 

        public MipsGenerationHelper LoadFromMemoryLabel( Register r, object d, int offset = 0 ) // r <- (d + offset)
        {
            this.body.Append( $"{ TAB + TAB }lw { r }, { ( offset == 0 ? "" : offset.ToString() ) }{ d }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper LoadFromAddress( Register r, string a, int offset = 0 ) // r <- a
        {
            this.body.Append( $"{ TAB + TAB }la { r }, { a }{ ENDL }" );
            if( offset != 0 )
                return Add( r, offset );
            return this;
        }

        public MipsGenerationHelper LoadByte( Register r1, Register r2 ) // r1 <- (r2)
        {
            this.body.Append( $"{ TAB + TAB }lb { r1 }, ({ r2 }){ ENDL }" );
            return this;
        }


        // Save
        public MipsGenerationHelper SaveToMemory( Register r, object d, int offset = 0 ) // (d + offset) <- r
        {
            this.body.Append( $"{ TAB + TAB }sw { r }, { ( offset == 0 ? "" : offset.ToString() ) }({ d }){ ENDL }" );
            return this;
        }

        public MipsGenerationHelper SaveByte( Register r1, Register r2, int offset = 0 ) // r2 <- (r1)
        {
            this.body.Append( $"{ TAB + TAB }sb { r1 }, { ( offset == 0 ? "" : offset.ToString() ) }({ r2 }){ ENDL }" );
            return this;
        }


        // Dynamic saving
        public MipsGenerationHelper Allocate( Register r1, Register r2 ) => this.LoadConstant( MipsRegisterSet.v0, allocate )
                                                                                .Move( MipsRegisterSet.a0, r1 )
                                                                                .SystemCall()
                                                                                .Move( r2, MipsRegisterSet.v0 );


        // Jumps
        public MipsGenerationHelper JumpToLabel( string label )
        {
            this.body.Append( $"{ TAB + TAB}j { label }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper JumpToLabelInRegistry( Register r )
        {
            this.body.Append( $"{ TAB + TAB }jalr { r }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper JumpRegister( Register r )
        {
            this.body.Append( $"{ TAB + TAB }jr { r }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper BranchOnEquals( Register r, object v, string label )
        {
            if( r == v )
                return this.JumpToLabel( label );

            this.body.Append( $"{ TAB + TAB }beq { r }, { v }, { label }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper BranchNotEquals( Register r, object v, string label )
        {
            if( r == v )
                return this;

            this.body.Append( $"{ TAB + TAB }bne { r }, { v }, { label }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper BranchNotEqualZero( Register r, string label )
        {
            this.body.Append( $"{ TAB + TAB }bnez { r }, { label }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper BranchLessThan( Register r, object v, string label )
        {
            if( r == v )
                return this;

            this.body.Append( $"{ TAB + TAB }blt { r }, { v }, { label }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper BranchLessEqual( Register r, object v, string label )
        {
            if( r == v )
                return this.JumpToLabel( label );

            this.body.Append( $"{ TAB + TAB }ble { r }, { v }, { label }{ ENDL }" );
            return this;
        }


        // Stack
        public MipsGenerationHelper Push( Register r ) => this.Sub( MipsRegisterSet.sp, 4 )
                                                              .SaveToMemory( r, MipsRegisterSet.sp );

        public MipsGenerationHelper Pop( Register r ) => this.LoadFromMemory( r, MipsRegisterSet.sp )
                                                             .Add( MipsRegisterSet.sp, 4 );

        public MipsGenerationHelper PushConstant( int c ) => this.Sub( MipsRegisterSet.sp, 4 )
                                                                 .LoadConstant( MipsRegisterSet.sp, c );


        // Boolean operators
        public MipsGenerationHelper Not( Register r, Register r_out )
        {
            this.LoadConstant( MipsRegisterSet.t0, 1 ).Sub( MipsRegisterSet.t0, MipsRegisterSet.a0, MipsRegisterSet.a0 );
            return this;
        }

        public MipsGenerationHelper Not( Register r ) => Not( r, r );


        // Bitwise operators
        public MipsGenerationHelper Or( Register r1, Register r2, Register r_out )
        {
            if( r1 == r2 || r1 == MipsRegisterSet.zero || r2 == MipsRegisterSet.zero )
                return this.Move( r_out, r1 == MipsRegisterSet.zero ? r2 : r1 );

            this.body.Append( $"{ TAB + TAB }or { r_out }, { r1 }, { r2 }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper Or( Register r1, Register r2 ) => Or( r1, r2, r1 );

        public MipsGenerationHelper OrConstant( Register r, int c, Register r_out )
        {
            if( c == 0 )
                return this.Move( r_out, r );

            this.body.Append( $"{ TAB + TAB }ori { r_out }, { r }, { c }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper OrConstant( Register r, int c ) => OrConstant( r, c, r );


        // Add instruction
        public MipsGenerationHelper Add( Register r, object v, Register r_out ) // r_out <- r + v
        {
            this.body.Append( $"{ TAB + TAB }add { r_out }, { r }, { v }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper Add( Register r, object v ) => this.Add( r, v, r ); // r <- r + v


        // Sub instruction
        public MipsGenerationHelper Sub( Register r, object v, Register r_out ) // r_out <- r - v
        {
            this.body.Append( $"{ TAB + TAB }sub { r_out }, { r }, { v }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper Sub( Register r, object v ) => this.Sub( r, v, r ); // r <- r - v


        // Mul instruction
        public MipsGenerationHelper Mul( Register r, object v, Register r_out ) // r_out <- r * v
        {
            this.body.Append( $"{ TAB + TAB }mul { r_out }, { r }, { v }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper Mul( Register r, object v ) => this.Mul( r, v, r ); // r <- r * v


        // Div instruction
        public MipsGenerationHelper Div( Register r, object v ) // a0 <- r / v
        {
            this.body.Append( $"{TAB + TAB}div {r}, {v}{ENDL}" ).Append( $"{TAB + TAB}mflo $a0{ENDL}" );
            return this;
        }


        // Copy
        public MipsGenerationHelper Copy( Register r1, Register r2, Register r3, string endtag, string @else ) =>
            this.Tag( @else )//start tag
                .BranchLessEqual( r3, 0, endtag )//if size is 0 go to end tag
                .LoadByte( MipsRegisterSet.v0, r1 )//v0<- (from)
                .SaveByte( MipsRegisterSet.v0, r2 )//(to)<-v0
                .Add( r1, 1 )
                .Add( r2, 1 )
                .Sub( r3, 1 )
                .JumpToLabel( @else )
                .Tag( endtag );


        // Void
        public MipsGenerationHelper IsVoid( string voidLabel, string elseLabel, string endLabel ) => this
            .LoadFromAddress( MipsRegisterSet.t0, voidLabel )
            .BranchOnEquals( MipsRegisterSet.t0, MipsRegisterSet.a0, elseLabel )
            .LoadConstant( MipsRegisterSet.a0, MipsGenerationHelper.FALSE )
            .JumpToLabel( endLabel )
            .Tag( elseLabel )
            .LoadConstant( MipsRegisterSet.a0, MipsGenerationHelper.TRUE )
            .Tag( endLabel );


        // Runtime error
        public MipsGenerationHelper ThrowRuntimeError( int id, ILabelILGenerator labelGenerator )
        {
            switch( id )
            {
                case RuntimeErrors.ObjectAbort:
                    this.PrintString( labelGenerator.GetException( id ) )
                        .GetParam( MipsRegisterSet.a0, 0 )
                        .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeInfoOffest )
                        .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeNameOffset )
                        .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0 )
                        .PrintString( MipsRegisterSet.a0 )
                        .PrintString( labelGenerator.GetNewLine() )
                        .Exit();
                    break;
                default:
                    this.PrintString( labelGenerator.GetException( id ) ).Exit();
                    break;
            }

            return this;
        }

        public MipsGenerationHelper ThrowRuntimeErrorAdditionalMsg( int id, ILabelILGenerator labelGenerator, Register r )
        {
            switch( id )
            {
                case RuntimeErrors.ObjectAbort:
                    this.PrintString( labelGenerator.GetException( id ) )
                        .GetParam( MipsRegisterSet.a0, 0 )
                        .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeInfoOffest )
                        .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0, MipsGenerationHelper.TypeNameOffset )
                        .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0 )
                        .PrintString( MipsRegisterSet.a0 )
                        .PrintString( labelGenerator.GetNewLine() )
                        .Exit();
                    break;
                case RuntimeErrors.CaseWithoutMatching:
                    this.PrintString( labelGenerator.GetException( id ) )
                        .PrintString( r )
                        .PrintString( labelGenerator.GetNewLine() )
                        .Exit();
                    break;
                default:
                    this.PrintString( labelGenerator.GetException( id ) ).Exit();
                    break;
            }
            return this;
        }


        //String equals --> en a0 y a1 los strings
        public MipsGenerationHelper StringEquals() => this.Tag( StringEqualsLabel )
                                                          .GetParam( MipsRegisterSet.a0, 0 )
                                                          .LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.a0 )
                                                          .GetParam( MipsRegisterSet.a1, 4 )
                                                          .LoadFromMemory( MipsRegisterSet.a1, MipsRegisterSet.a1 )
                                                          .Tag(StringEqualsLabelWhile)
                                                          .LoadByte( MipsRegisterSet.t0, MipsRegisterSet.a0 )
                                                          .LoadByte( MipsRegisterSet.t1, MipsRegisterSet.a1 )
                                                          .BranchNotEquals( MipsRegisterSet.t0, MipsRegisterSet.t1, StringEqualsLabelFalse )
                                                          .Add( MipsRegisterSet.a0, 1 )
                                                          .Add( MipsRegisterSet.a1, 1 )
                                                          .BranchNotEquals( MipsRegisterSet.t0, 0, StringEqualsLabelWhile)
                                                          .Add( MipsRegisterSet.zero, 1, MipsRegisterSet.a0 )
                                                          .JumpToLabel( StringEqualsLabelEnd )
                                                          .Tag( StringEqualsLabelFalse )
                                                          .Move( MipsRegisterSet.a0, MipsRegisterSet.zero )
                                                          .Tag(StringEqualsLabelEnd)
                                                          .Return();
    }
}