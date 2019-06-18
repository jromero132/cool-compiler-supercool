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
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "if_expression.cl" ), "if_expression" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void ArithmeticOperator()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "arithmetic_operator.cl" ),
                "arithmetic_operator" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void Assigment()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "assigment.cl" ), "assigment" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void In_Int()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "in_int.cl" ), "in_int",
                new List<string> { Path.Combine( "Examples", "Cool", "in_int.in" ) } );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void In_String()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "in_string.cl" ), "in_string",
                new List<string> { Path.Combine( "Examples", "Cool", "in_string.in" ) } );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void ComparerOperator()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "comparer_operator.cl" ), "comparer_operator" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void While()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "while.cl" ), "while" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void ComplementOperator()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "complement_operator.cl" ), "complement_operator" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void ObjectTypeName()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "objectType_name.cl" ), "objectType_name" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void Attributes()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "attributes.cl" ), "attributes" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void Palindrome()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "palindrome.cl" ), "palindrome",
                new List<string>
                {
                    Path.Combine("Examples", "Cool", "palindrome.in1"),
                    Path.Combine("Examples", "Cool", "palindrome.in2")
                } );
            Assert.True( test_case.RunTest() );
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
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "allocate.cl" ), "allocate" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void StaticDispatch()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "static_dispatch.cl" ), "static_dispatch" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void BookList()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "book_list.cl" ), "book_list" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void IO()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "io.cl" ), "io" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void Concat()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "concat.cl" ), "concat", new List<string>
            {
                Path.Combine("Examples", "Cool", "concat.in"),
            });
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void Case()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "case.cl" ), "case" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void CaseWithVoidExpr()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "case_void_expression.cl" ), "case_void_expression" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void Substring()
        {
            TestCase test_case = new TestCase( Path.Combine( "Examples", "Cool", "substring.cl" ), "substring" );
            Assert.True( test_case.RunTest() );
        }

        [Fact]
        public void Factorial()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "factorial.cl"), "factorial");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void HairScary()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "hair_scary.cl"), "hair_scary");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void Lam()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "lam.cl"), "lam");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void Life()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "life.cl"), "life",new List<string>
            {
                Path.Combine("Examples", "Cool", "life.in"),
            });
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void List()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "list.cl"), "list");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void NewComplex()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "new_complex.cl"), "new_complex");
            Assert.True(test_case.RunTest());
        }

        [Fact]
        public void SortList()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "sort_list.cl"), "sort_list", new List<string>
            {
                Path.Combine("Examples", "Cool", "sort_list.in"),
            });
            Assert.True(test_case.RunTest());
        }


        [Fact]
        public void Boxing()
        {
            TestCase test_case = new TestCase(Path.Combine("Examples", "Cool", "boxing.cl"), "boxing");
            Assert.True(test_case.RunTest());
        }
    }
}
