using System;
using System.Collections.Generic;
using System.Text;

using SuperCOOL.CodeGeneration.MIPS.Registers;

namespace SuperCOOL.CodeGeneration.MIPS
{
    public class MipsGenerationHelper
    {
        public const int TRUE = 1, FALSE = 0, NULL = 0;

        private static readonly string ENDL = Environment.NewLine;

        //Variable
        internal static readonly int TypeInfoOffest=-4;

        //TypeInfo
        public const int TypeNameOffset=0;
        public const int SizeOfOffset=4;
        public const int VirtualTableOffset=8;

        private string body;

        private MipsGenerationHelper( string body = "" ) => this.body = body;

        public static MipsGenerationHelper NewScript() => new MipsGenerationHelper();

        public static implicit operator string( MipsGenerationHelper a ) => a.body;


        // System call codes
        private const int print_int = 1;
        private const int print_string = 4;
        private const int read_int = 5;
        private const int allocate = 9;
        private const int exit = 10;


        // Sections
        public MipsGenerationHelper Section( string section_name, string arg = "" ) // .{ section_name } arg
        {
            this.body += $".{ section_name }{ ( arg == "" ? "" : " " + arg ) }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper DataSection() => Section( "data" );

        public MipsGenerationHelper TextSection() => Section( "text" );

        public MipsGenerationHelper GlobalSection( string label ) => Section( "globl", label );


        // Tags
        public MipsGenerationHelper Tag( string tag_name, bool end_line = true, string offset = "" ) // main:
        {
            this.body += $"{ tag_name }:{ offset }{ ( end_line ? ENDL : "" ) }";
            return this;
        }

        public MipsGenerationHelper MainTag() => Tag( "main" );


        // Data Types
        public MipsGenerationHelper AddData( string name, string type, object value ) => this.Tag( name, false, " " )
                                                                                        .Section( type, $" { value }" );

        public MipsGenerationHelper AddStringType( string name, string value ) => AddData( name, "asciiz", value );

        public MipsGenerationHelper AddWordType( string name, object value ) => AddData( name, "word", value );


        // Exit
        public MipsGenerationHelper Exit() => this.LoadConstant( MipsRegisterSet.v0, exit )
                                             .SystemCall();


        // System call
        public MipsGenerationHelper SystemCall() // syscall
        {
            this.body += $"syscall{ ENDL }";
            return this;
        }


        // Functions Call
        // When changing stack in this method check correctness with return method.
        public MipsGenerationHelper Call( Register r ) => this.Push( MipsRegisterSet.ip )
                                                         .Push( MipsRegisterSet.bp )
                                                         .Move( MipsRegisterSet.bp, MipsRegisterSet.sp )
                                                         .JumpRegister( r );

        public MipsGenerationHelper Return() => this.Pop( MipsRegisterSet.bp )
                                               .Pop( MipsRegisterSet.t0 )
                                               .JumpRegister( MipsRegisterSet.t0 );


        // Getting args and variables in functions
        public MipsGenerationHelper GetParam( int offset ) => this.LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.bp, offset + 8 );

        public MipsGenerationHelper GetLocal( int offset ) => this.LoadFromMemory( MipsRegisterSet.a0, MipsRegisterSet.bp, -offset - 8 );


        // Read
        public MipsGenerationHelper ReadInt( Register r ) => this.LoadConstant( MipsRegisterSet.v0, read_int )
                                                                 .SystemCall()
                                                                 .Move( r, MipsRegisterSet.v0 );


        // Print
        public MipsGenerationHelper PrintInt() => this.LoadConstant( MipsRegisterSet.v0, print_int )
                                                 .SystemCall();
        public MipsGenerationHelper PrintInt( int d ) => this.LoadConstant( MipsRegisterSet.a0, d )
                                                           .PrintInt();
        public MipsGenerationHelper PrintString( string name ) => this.LoadConstant( MipsRegisterSet.v0, print_string )
                                                                 .LoadFromAddress( MipsRegisterSet.a0, name )
                                                                 .SystemCall();


        // Move
        public MipsGenerationHelper Move( Register r1, Register r2 ) // r1 <- r2
        {
            this.body += $"move { r1 }, { r2 }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper MoveFromLo( Register r ) // r <- $lo
        {
            this.body += $"mflo { r }{ ENDL }";
            return this;
        }


        // Load
        public MipsGenerationHelper LoadConstant( Register r, int c ) // r <- c
        {
            this.body += $"li { r }, { c }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper LoadFromMemory( Register r, object d, int offset = 0 ) // r <- (d)
        {
            this.body += $"lw { r }, { ( offset == 0 ? "" : offset.ToString() ) }({ d }){ ENDL }";
            return this;
        }

        public MipsGenerationHelper LoadFromAddress( Register r, string a ) // r <- a
        {
            this.body += $"la { r }, { a }{ ENDL }";
            return this;
        }


        // Save
        public MipsGenerationHelper SaveToMemory( Register r, object d, int offset = 0 ) // (d) <- r
        {
            this.body += $"sw { r }, { ( offset == 0 ? "" : offset.ToString() ) }({ d }){ ENDL }";
            return this;
        }


        // Dynamic saving
        public MipsGenerationHelper Allocate( int n ) => this.LoadConstant( MipsRegisterSet.v0, allocate )
                                                        .LoadConstant( MipsRegisterSet.a0, n << 2 )
                                                        .Move( MipsRegisterSet.a0, MipsRegisterSet.v0 );


        // Jumps
        public MipsGenerationHelper JumpToLabel( string label )
        {
            this.body += $"j { label }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper JumpRegister( Register r )
        {
            this.body += $"jr { r }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper BranchOnEquals( Register r, object v, string label )
        {
            this.body += $"beq { r }, { v }, { label }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper BranchLessThan( Register r, object v, string label )
        {
            this.body += $"blt { r }, { v }, { label }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper BranchLessEqual( Register r, object v, string label )
        {
            this.body += $"ble { r }, { v }, { label }{ ENDL }";
            return this;
        }


        // Stack
        public MipsGenerationHelper Push( Register r ) => this.Sub( MipsRegisterSet.sp, 4 )
                                                         .SaveToMemory( r, MipsRegisterSet.sp );
        public MipsGenerationHelper Pop( Register r ) => this.LoadFromMemory( r, MipsRegisterSet.sp )
                                                        .Add( MipsRegisterSet.sp, 4 );
        public MipsGenerationHelper PushConstant( int c ) => this.LoadConstant( MipsRegisterSet.a0, c )
                                                            .Push( MipsRegisterSet.a0 );


        // Boolean operators
        public MipsGenerationHelper Not( Register r, Register r_out )
        {
            this.body += $"nor { r_out }, { r }, { MipsRegisterSet.zero }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper Not( Register r ) => Not( r, r );


        // Bitwise operators
        public MipsGenerationHelper Or( Register r1, Register r2, Register r_out )
        {
            this.body += $"or { r_out }, { r1 }, { r2 }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper Or( Register r1, Register r2 ) => Or( r1, r2, r1 );

        public MipsGenerationHelper OrConstant( Register r, int c, Register r_out )
        {
            this.body += $"ori { r_out }, { r }, { c }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper OrConstant( Register r, int c ) => OrConstant( r, c, r );


        // Add instruction
        public MipsGenerationHelper Add( Register r, object v, Register r_out ) // r_out <- r + v
        {
            this.body += $"add { r_out }, { r }, { v }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper Add( Register r, object v ) => this.Add( r, v, r ); // r <- r + v


        // Sub instruction
        public MipsGenerationHelper Sub( Register r, object v, Register r_out ) // r_out <- r - v
        {
            this.body += $"sub { r_out }, { r }, { v }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper Sub( Register r, object v ) => this.Sub( r, v, r ); // r <- r - v


        // Mul instruction
        public MipsGenerationHelper Mul( Register r, object v, Register r_out ) // r_out <- r * v
        {
            this.body += $"mul { r_out }, { r }, { v }{ ENDL }";
            return this;
        }

        public MipsGenerationHelper Mul( Register r, object v ) => this.Mul( r, v, r ); // r <- r * v


        // Div instruction
        public MipsGenerationHelper Div( Register r, object v ) // a0 <- r / v
        {
            this.body += $"div { r }, { v }{ ENDL }";
            return this;
        }
    }
}