using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Xunit;

namespace SuperCOOL.Tests.CoolTests
{
    public class FullExecutionTest
    {
        [Fact]
        public void HelloWorld()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "hello_world.cl" ), "hello_world" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void IfSimpleExpression()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "if_expression.cl"), "if_expression");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void ArithmeticOperator()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "arithmetic_operator.cl" ), "arithmetic_operator" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void Assigment()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "assigment.cl"), "assigment");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void In_Int()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "in_int.cl"), "in_int");
            Assert.True(false); //TODO input
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void Complex()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "complex.cl" ), "complex" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void Primes()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "primes.cl" ), "primes" );
            Assert.True( test_case.RunTest() );
        }
    }
}
