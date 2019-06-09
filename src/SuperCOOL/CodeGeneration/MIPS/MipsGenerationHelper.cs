using System;
using System.Collections.Generic;
using System.Text;

using SuperCOOL.CodeGeneration.MIPS.Registers;

namespace SuperCOOL.CodeGeneration.MIPS
{
    public class MipsGenerationHelper
    {
        private static readonly string ENDL = Environment.NewLine;

        private string body;

        private MipsGenerationHelper( string body = "" ) => this.body = body;

        public static MipsGenerationHelper NewScript() => new MipsGenerationHelper();

        public static implicit operator string( MipsGenerationHelper a ) => a.body;


        // System call codes
        private const int print_int = 1;
        private const int print_string = 4;
        private const int read_int = 5;
        private const int exit = 10;


        // Sections
        public MipsGenerationHelper Section( string section_name ) // .data
        {
            this.body += $".{ section_name }{ ENDL }";
            return this;
        }
        public MipsGenerationHelper DataSection() => Section( "data" );
        public MipsGenerationHelper TextSection() => Section( "text" );


        // Tags
        public MipsGenerationHelper Tag( string tag_name ) // main:
        {
            this.body += $"{ tag_name }:{ ENDL }";
            return this;
        }
        public MipsGenerationHelper MainTag() => Tag( "main" );


        // Data
        public MipsGenerationHelper StringType( string name, string value )
        {
            this.body += $"{ name }: .ascii \"{ value }\"{ ENDL }";
            return this;
        }


        // Exit
        public MipsGenerationHelper Exit() => this.LoadConstant( MipsRegisterSet.v0, exit )
                                             .SystemCall();


        // System call
        public MipsGenerationHelper SystemCall() // syscall
        {
            this.body += $"syscall{ ENDL }";
            return this;
        }


        // Read
        public MipsGenerationHelper ReadInt() => this.LoadConstant( MipsRegisterSet.v0, read_int )
                                                .SystemCall();


        // Print
        public MipsGenerationHelper PrintInt() => this.LoadConstant( MipsRegisterSet.v0, print_int )
                                                 .SystemCall();
        public MipsGenerationHelper PrintInt( int d ) => this.LoadConstant( MipsRegisterSet.a0, d )
                                                           .PrintInt();
        public MipsGenerationHelper PrintString( string name ) => this.LoadConstant( MipsRegisterSet.v0, print_string )
                                                                 .LoadAddress( MipsRegisterSet.a0, name )
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
        public MipsGenerationHelper LoadMemory( Register r, object d ) // r <- (d)
        {
            this.body += $"lw { r }, ({ d }){ ENDL }";
            return this;
        }
        public MipsGenerationHelper LoadAddress( Register r, string a ) // r <- a
        {
            this.body += $"la { r }, { a }{ ENDL }";
            return this;
        }


        // Save
        public MipsGenerationHelper SaveMemory( Register r, object d ) // (d) <- r
        {
            this.body += $"sw { r }, ({ d }){ ENDL }";
            return this;
        }


        // Stack
        public MipsGenerationHelper Push( Register r ) => this.Sub( MipsRegisterSet.sp, 4 )
                                                         .SaveMemory( r, MipsRegisterSet.sp );
        public MipsGenerationHelper Pop( Register r ) => this.LoadMemory( r, MipsRegisterSet.sp )
                                                        .Add( MipsRegisterSet.sp, 4 );
        public MipsGenerationHelper PushConstant( int c ) => this.LoadConstant( MipsRegisterSet.a0, c )
                                                            .Push( MipsRegisterSet.a0 );


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