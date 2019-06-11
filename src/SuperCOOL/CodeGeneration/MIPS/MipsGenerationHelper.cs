﻿using SuperCOOL.CodeGeneration.MIPS.Registers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCOOL.CodeGeneration.MIPS
{
    public class MipsGenerationHelper
    {
        // Constants
        public const int TRUE = 1, FALSE = 0, NULL = 0;

        public static readonly string TAB = "\t";
        private static readonly string ENDL = Environment.NewLine;

        public const int BufferSize = 4096;
        public const string Exceptions = "____exceptions";

        //Variable
        internal static readonly int TypeInfoOffest = -4;

        //TypeInfo
        public const int TypeNameOffset = 0;
        public const int SizeOfOffset = 4;
        public const int VirtualTableOffset = 8;


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
        private const int allocate = 9;
        private const int exit = 10;


        // Sections
        public MipsGenerationHelper Section( string section_name, string arg = "" ) // .{ section_name } arg
        {
            this.body.Append( $".{ section_name }{ ( arg == "" ? "" : " " + arg ) }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper DataSection() => Section( "data" ); // .data

        public MipsGenerationHelper TextSection() => Section( "text" ); // .text

        public MipsGenerationHelper GlobalSection( string label ) => Section( "globl", label ); // .globl { label }


        // Tags
        public MipsGenerationHelper Tag( string tag_name ) // { tag_name }:
        {
            this.body.Append( $"{ tag_name }:{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper MainTag() => Tag( "main" ); // main:


        // Comments
        public MipsGenerationHelper Comment( string comment )
        {
            this.body.Append( $"# { comment }{ ENDL }" );
            return this;
        }


        // Newline
        public MipsGenerationHelper NewLine()
        {
            this.body.Append( ENDL );
            return this;
        }


        // Data Types
        public MipsGenerationHelper AddData( string name, IEnumerable<(string type, object value)> args ) => this.Tag( name ) + new StringBuilder( args.Select( x => $"{ TAB }.{ x.type } { x.value }{ ENDL }" ).Aggregate( ( x, y ) => x.ToString() + y.ToString() ) );

        public static (string, object) AddStringData( string value ) => ("asciiz", value);

        public static (string, object) AddIntData( object value ) => ("word", value);

        public static (string, object) AddDynamycString( int space ) => ("space", space);


        // Exit
        public MipsGenerationHelper Exit() => this.LoadConstant( MipsRegisterSet.v0, exit )
                                                  .SystemCall();


        // System call
        public MipsGenerationHelper SystemCall() // syscall
        {
            this.body.Append( $"syscall{ ENDL }" );
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
        public MipsGenerationHelper ReadInt( Register r )
        {
            this.LoadConstant( MipsRegisterSet.v0, read_int )
                .SystemCall();

            if( r != MipsRegisterSet.v0 )
                this.Move( r, MipsRegisterSet.v0 );

            return this;
        }


        // Print
        public MipsGenerationHelper PrintInt() => this.LoadConstant( MipsRegisterSet.v0, print_int )
                                                      .SystemCall();

        public MipsGenerationHelper PrintInt( int d ) => this.LoadConstant( MipsRegisterSet.a0, d )
                                                             .PrintInt();

        public MipsGenerationHelper PrintString( string name ) => this.LoadConstant( MipsRegisterSet.v0, print_string )
                                                                      .LoadFromAddress( MipsRegisterSet.a0, name )
                                                                      .SystemCall();

        public MipsGenerationHelper PrintString( Register r ) => this.LoadConstant( MipsRegisterSet.v0, print_string )
                                                                     .LoadFromMemory( MipsRegisterSet.a0, r )
                                                                     .SystemCall();


        // Move
        public MipsGenerationHelper Move( Register r1, Register r2 ) // r1 <- r2
        {
            if( r1 != r2 )
                this.body.Append( $"move { r1 }, { r2 }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper MoveFromLo( Register r ) // r <- $lo
        {
            this.body.Append( $"mflo { r }{ ENDL }" );
            return this;
        }


        // Load
        public MipsGenerationHelper LoadConstant( Register r, int c ) // r <- c
        {
            this.body.Append( $"li { r }, { c }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper LoadFromMemory( Register r, object d, int offset = 0 ) => LoadFromMemoryLabel( r, $"({d})", offset ); // r <- (d) 

        public MipsGenerationHelper LoadFromMemoryLabel( Register r, object d, int offset = 0 ) // r <- (d + offset)
        {
            this.body.Append( $"lw { r }, { ( offset == 0 ? "" : offset.ToString() ) }{ d }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper LoadFromAddress( Register r, string a ) // r <- a
        {
            this.body.Append( $"la { r }, { a }{ ENDL }" );
            return this;
        }


        // Save
        public MipsGenerationHelper SaveToMemory( Register r, object d, int offset = 0 ) // (d + offset) <- r
        {
            this.body.Append( $"sw { r }, { ( offset == 0 ? "" : offset.ToString() ) }({ d }){ ENDL }" );
            return this;
        }


        // Dynamic saving
        public MipsGenerationHelper Allocate( Register r )
        {
            this.LoadConstant( MipsRegisterSet.v0, allocate );

            if( MipsRegisterSet.a0 != r )
                this.Move( MipsRegisterSet.a0, r );

            return this.SystemCall()
                .Move( MipsRegisterSet.a0, MipsRegisterSet.v0 );
        }


        // Jumps
        public MipsGenerationHelper JumpToLabel( string label )
        {
            this.body.Append( $"j { label }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper JumpRegister( Register r )
        {
            this.body.Append( $"jr { r }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper BranchOnEquals( Register r, object v, string label )
        {
            if( r == v )
                return this.JumpToLabel( label );

            this.body.Append( $"beq { r }, { v }, { label }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper BranchLessThan( Register r, object v, string label )
        {
            if( r == v )
                return this;

            this.body.Append( $"blt { r }, { v }, { label }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper BranchLessEqual( Register r, object v, string label )
        {
            if( r == v )
                return this.JumpToLabel( label );

            this.body.Append( $"ble { r }, { v }, { label }{ ENDL }" );
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
            this.body.Append( $"nor { r_out }, { r }, { MipsRegisterSet.zero }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper Not( Register r ) => Not( r, r );


        // Bitwise operators
        public MipsGenerationHelper Or( Register r1, Register r2, Register r_out )
        {
            if( r1 == r2 || r1 == MipsRegisterSet.zero || r2 == MipsRegisterSet.zero )
                return this.Move( r_out, r1 == MipsRegisterSet.zero ? r2 : r1 );

            this.body.Append( $"or { r_out }, { r1 }, { r2 }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper Or( Register r1, Register r2 ) => Or( r1, r2, r1 );

        public MipsGenerationHelper OrConstant( Register r, int c, Register r_out )
        {
            if( c == 0 )
                return this.Move( r_out, r );

            this.body.Append( $"ori { r_out }, { r }, { c }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper OrConstant( Register r, int c ) => OrConstant( r, c, r );


        // Add instruction
        public MipsGenerationHelper Add( Register r, object v, Register r_out ) // r_out <- r + v
        {
            this.body.Append( $"add { r_out }, { r }, { v }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper Add( Register r, object v ) => this.Add( r, v, r ); // r <- r + v


        // Sub instruction
        public MipsGenerationHelper Sub( Register r, object v, Register r_out ) // r_out <- r - v
        {
            this.body.Append( $"sub { r_out }, { r }, { v }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper Sub( Register r, object v ) => this.Sub( r, v, r ); // r <- r - v


        // Mul instruction
        public MipsGenerationHelper Mul( Register r, object v, Register r_out ) // r_out <- r * v
        {
            this.body.Append( $"mul { r_out }, { r }, { v }{ ENDL }" );
            return this;
        }

        public MipsGenerationHelper Mul( Register r, object v ) => this.Mul( r, v, r ); // r <- r * v


        // Div instruction
        public MipsGenerationHelper Div( Register r, object v ) // a0 <- r / v
        {
            this.body.Append( $"div { r }, { v }{ ENDL }" );
            return this;
        }
    }
}