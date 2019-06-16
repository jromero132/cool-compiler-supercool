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
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "in_int.cl"), "in_int",
                new List<string> { Path.Combine("Examples", "Cool", "in_int.in") });
            Assert.True(test_case.RunTest());
        }
        [Fact]
        public void In_String()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "in_string.cl"), "in_string", new List<string> { Path.Combine("Examples", "Cool", "in_string.in")});
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void ComparerOperator()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "comparer_operator.cl"), "comparer_operator");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void While()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "while.cl"), "while");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void ComplementOperator()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "complement_operator.cl"), "complement_operator");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void ObjectTypeName()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "objectType_name.cl"), "objectType_name");
            Assert.True(test_case.RunTest());
        }
        [Fact]
        public void Attributes()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "attributes.cl"), "attributes");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void Palindrome()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "palindrome.cl"), "palindrome",
                new List<string>
                {
                    Path.Combine("Examples", "Cool", "palindrome1.in"),
                    Path.Combine("Examples", "Cool", "palindrome2.in")
                });
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

        [Fact]
        public void Let()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "let.cl" ), "let" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void Allocate()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "allocate.cl"), "allocate");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void StaticDispatch()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "static_dispatch.cl"), "static_dispatch");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void BookList()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "book_list.cl"), "book_list");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void IO()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "io.cl"), "io");
            Assert.True(test_case.RunTest());
        }
    }
}
