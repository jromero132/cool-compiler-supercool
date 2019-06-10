using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Xunit;

using SuperCOOL.CodeGeneration.MIPS;
using SuperCOOL.CodeGeneration.MIPS.Registers;

namespace SuperCOOL.Tests.MipsGenerationTests
{
    public class MipsGenerationTests
    {
        [Fact]
        // Print 132
        public void MipsGenerationTestPrintInt1()
        {
            // Names
            var test_name = "test1";
            var file_name = Path.Combine( "TestCases", $"{ test_name }.asm" );
            var input_file = Path.Combine( "TestCases", $"{ test_name }.in" );
            var output_file = Path.Combine( "TestCases", $"{ test_name }.out" );
            var answer_file = Path.Combine( "TestCases", $"{ test_name }.ans" );

            // Code
            Helper.CreateFile( file_name,
                MipsGenerationHelper.NewScript().TextSection().MainTag()
               .PrintInt( 132 )
               .Exit()
                );

            // Answer
            Helper.CreateFile( answer_file, "132" );

            // Running
            Helper.RunSpim( file_name, output_file: output_file );

            // Normalize --> deleting unuseful lines at the beginning of the file
            Helper.Normalize( output_file );

            // Checking
            Assert.True( Helper.CompareFiles( output_file, answer_file ) );
        }

        [Fact]
        // Sum 13 + 2
        public void MipsGenerationTestAdd1()
        {
            // Names
            var test_name = "test2";
            var file_name = Path.Combine( "TestCases", $"{ test_name }.asm" );
            var input_file = Path.Combine( "TestCases", $"{ test_name }.in" );
            var output_file = Path.Combine( "TestCases", $"{ test_name }.out" );
            var answer_file = Path.Combine( "TestCases", $"{ test_name }.ans" );

            // Code
            Helper.CreateFile( file_name,
                MipsGenerationHelper.NewScript().TextSection().MainTag()
               .LoadConstant( MipsRegisterSet.a0, 13 )
               .Add( MipsRegisterSet.a0, 2 )
               .PrintInt()
               .Exit()
                );

            // Answer
            Helper.CreateFile( answer_file, "15" );

            // Running
            Helper.RunSpim( file_name, output_file: output_file );

            // Normalize --> deleting unuseful lines at the beginning of the file
            Helper.Normalize( output_file );

            // Checking
            Assert.True( Helper.CompareFiles( output_file, answer_file ) );
        }

        [Fact]
        // Read two numbers and sum them
        public void MipsGenerationTestAdd2()
        {
            // Names
            var test_name = "test3";
            var file_name = Path.Combine( "TestCases", $"{ test_name }.asm" );
            var input_file = Path.Combine( "TestCases", $"{ test_name }.in" );
            var output_file = Path.Combine( "TestCases", $"{ test_name }.out" );
            var answer_file = Path.Combine( "TestCases", $"{ test_name }.ans" );

            Helper.CreateFile( input_file, "13\r\n2" );

            // Code
            Helper.CreateFile( file_name,
                MipsGenerationHelper.NewScript().TextSection().MainTag()
               .ReadInt( MipsRegisterSet.a0 )
               .ReadInt( MipsRegisterSet.v0 )
               .Add( MipsRegisterSet.a0, MipsRegisterSet.v0 )
               .PrintInt()
               .Exit()
                );

            // Answer
            Helper.CreateFile( answer_file, "15" );

            // Running
            Helper.RunSpim( file_name, input_file: input_file, output_file: output_file );

            // Normalize --> deleting unuseful lines at the beginning of the file
            Helper.Normalize( output_file );

            // Checking
            Assert.True( Helper.CompareFiles( output_file, answer_file ) );
        }

        [Fact]
        // Read five numbers and sum them
        public void MipsGenerationTestAdd3()
        {
            // Names
            var test_name = "test4";
            var file_name = Path.Combine( "TestCases", $"{ test_name }.asm" );
            var input_file = Path.Combine( "TestCases", $"{ test_name }.in" );
            var output_file = Path.Combine( "TestCases", $"{ test_name }.out" );
            var answer_file = Path.Combine( "TestCases", $"{ test_name }.ans" );

            Helper.CreateFile( input_file, "1\r\n2\r\n3\r\n4\r\n5" );

            // Code
            Helper.CreateFile( file_name,
                MipsGenerationHelper.NewScript().TextSection().MainTag()
               .ReadInt( MipsRegisterSet.a0 )
               .ReadInt( MipsRegisterSet.v0 )
               .Add( MipsRegisterSet.a0, MipsRegisterSet.v0 )
               .ReadInt( MipsRegisterSet.v0 )
               .Add( MipsRegisterSet.a0, MipsRegisterSet.v0 )
               .ReadInt( MipsRegisterSet.v0 )
               .Add( MipsRegisterSet.a0, MipsRegisterSet.v0 )
               .ReadInt( MipsRegisterSet.v0 )
               .Add( MipsRegisterSet.a0, MipsRegisterSet.v0 )
               .PrintInt()
               .Exit()
                );

            // Answer
            Helper.CreateFile( answer_file, "15" );

            // Running
            Helper.RunSpim( file_name, input_file: input_file, output_file: output_file );

            // Normalize --> deleting unuseful lines at the beginning of the file
            Helper.Normalize( output_file );

            // Checking
            Assert.True( Helper.CompareFiles( output_file, answer_file ) );
        }

        [Fact]
        // Read four numbers (a, b, c, d) and return (a + b) - (c + d)
        public void MipsGenerationTestAddSub1()
        {
            // Names
            var test_name = "test5";
            var file_name = Path.Combine( "TestCases", $"{ test_name }.asm" );
            var input_file = Path.Combine( "TestCases", $"{ test_name }.in" );
            var output_file = Path.Combine( "TestCases", $"{ test_name }.out" );
            var answer_file = Path.Combine( "TestCases", $"{ test_name }.ans" );

            Helper.CreateFile( input_file, "10\r\n9\r\n2\r\n4" );

            // Code
            Helper.CreateFile( file_name,
                MipsGenerationHelper.NewScript().TextSection().MainTag()
               .ReadInt( MipsRegisterSet.a0 )
               .ReadInt( MipsRegisterSet.v0 )
               .Add( MipsRegisterSet.a0, MipsRegisterSet.v0 )
               .ReadInt( MipsRegisterSet.t0 )
               .ReadInt( MipsRegisterSet.v0 )
               .Add( MipsRegisterSet.t0, MipsRegisterSet.v0 )
               .Sub( MipsRegisterSet.a0, MipsRegisterSet.t0 )
               .PrintInt()
               .Exit()
                );

            // Answer
            Helper.CreateFile( answer_file, "13" );

            // Running
            Helper.RunSpim( file_name, input_file: input_file, output_file: output_file );

            // Normalize --> deleting unuseful lines at the beginning of the file
            Helper.Normalize( output_file );

            // Checking
            Assert.True( Helper.CompareFiles( output_file, answer_file ) );
        }

        [Fact]
        // Read two numbers (a, b) and return a + b, a - b, a * b, a / b
        public void MipsGenerationTestArithmetic1()
        {
            // Names
            var test_name = "test6";
            var file_name = Path.Combine( "TestCases", $"{ test_name }.asm" );
            var input_file = Path.Combine( "TestCases", $"{ test_name }.in" );
            var output_file = Path.Combine( "TestCases", $"{ test_name }.out" );
            var answer_file = Path.Combine( "TestCases", $"{ test_name }.ans" );

            Helper.CreateFile( input_file, "13\r\n2" );

            // Code
            Helper.CreateFile( file_name,
                MipsGenerationHelper.NewScript().DataSection()
               //.AddStringType( "newline", "\"\\n\"" ) //TODO
               .TextSection().MainTag()
               .ReadInt( MipsRegisterSet.t0 )
               .ReadInt( MipsRegisterSet.t1 )
               .Move( MipsRegisterSet.a0, MipsRegisterSet.t0 )
               .Add( MipsRegisterSet.a0, MipsRegisterSet.t1 )
               .PrintInt()
               .PrintString( "newline" )
               .Move( MipsRegisterSet.a0, MipsRegisterSet.t0 )
               .Sub( MipsRegisterSet.a0, MipsRegisterSet.t1 )
               .PrintInt()
               .PrintString( "newline" )
               .Move( MipsRegisterSet.a0, MipsRegisterSet.t0 )
               .Mul( MipsRegisterSet.a0, MipsRegisterSet.t1 )
               .PrintInt()
               .PrintString( "newline" )
               .Move( MipsRegisterSet.a0, MipsRegisterSet.t0 )
               .Div( MipsRegisterSet.a0, MipsRegisterSet.t1 )
               .MoveFromLo( MipsRegisterSet.a0 )
               .PrintInt()
               .Exit()
                );

            // Answer
            Helper.CreateFile( answer_file, "15\r\n11\r\n26\r\n6" );

            // Running
            Helper.RunSpim( file_name, input_file: input_file, output_file: output_file );

            // Normalize --> deleting unuseful lines at the beginning of the file
            Helper.Normalize( output_file );

            // Checking
            Assert.True( Helper.CompareFiles( output_file, answer_file ) );
        }
    }
}
