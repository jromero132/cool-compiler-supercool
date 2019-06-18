using SuperCOOL.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace SuperCOOL.Tests.CoolTests
{
    public class NotSoCoolTest
    {
        [Fact]
        public void  CyclicalInheritance()
        {
            var errors = Compiler.Compile(new[] { Path.Combine("Examples", "NotSoCool", "cyclical_inheritance.cl")}, out string code, out var limits);
            Assert.Contains(new Error("Detected Cyclical Inheritance", ErrorKind.SemanticError), errors,new MessageErrorComparer());
        }

        [Fact]
        public void PuntoyComa()
        {
            var errors = Compiler.Compile(new[] { Path.Combine("Examples", "NotSoCool", "punto_y_coma.cl") }, out string code, out var limits);
            Assert.Contains(new Error("", ErrorKind.SyntacticError,4,0), errors, new InespecificErrorComparer());
            Assert.Contains(new Error("", ErrorKind.SyntacticError,9,0), errors, new InespecificErrorComparer());
            Assert.Contains(new Error("", ErrorKind.SyntacticError,9,0), errors, new InespecificErrorComparer());
        }

        [Fact]
        public void MissingMain()
        {
            var errors = Compiler.Compile(new[] { Path.Combine("Examples", "NotSoCool", "atoi.cl") }, out string code, out var limits);
            Assert.Contains(new Error("No Entry Point Detected", ErrorKind.SemanticError), errors, new MessageErrorComparer());
        }

        [Fact]
        public void StrangeTokens()
        {
            var errors = Compiler.Compile(new[] { Path.Combine("Examples", "NotSoCool", "attributes.cl") }, out string code, out var limits);
            Assert.Contains(new Error("", ErrorKind.LexicographicError, 1, 0), errors, new InespecificErrorComparer());
            Assert.Contains(new Error("", ErrorKind.LexicographicError, 7, 0), errors, new InespecificErrorComparer());
            Assert.Contains(new Error("", ErrorKind.LexicographicError, 21, 0), errors, new InespecificErrorComparer());
        }

        [Fact]
        public void AssignSelf()
        {
            var errors = Compiler.Compile(new[] { Path.Combine("Examples", "NotSoCool", "assigment.cl") }, out string code, out var limits);
            Assert.Contains(new Error("Not allowed to assign self.", ErrorKind.SemanticError), errors, new MessageErrorComparer());
        }

        [Fact]
        public void WrongSelfType()
        {
            var errors = Compiler.Compile(new[] { Path.Combine("Examples", "NotSoCool", "book_list.cl") }, out string code, out var limits);
            Assert.Contains(new Error("Not Allowed SELF_TYPE", ErrorKind.SemanticError,24,0), errors, new MessageErrorComparer());
        }

        class MessageErrorComparer : IEqualityComparer<Error>
        {
            public bool Equals(Error x, Error y)
            {
                return x.Message == y.Message && x.ErrorKind==y.ErrorKind;
            }

            public int GetHashCode(Error obj)
            {
                return obj.GetHashCode();
            }
        }

        class InespecificErrorComparer : IEqualityComparer<Error>
        {
            public bool Equals(Error x, Error y)
            {
                return x.Line == y.Line && x.ErrorKind == y.ErrorKind;
            }
            public int GetHashCode(Error obj)
            {
                return obj.GetHashCode();
            }
        }

        class SpecificErrorComparer : IEqualityComparer<Error>
        {
            public bool Equals(Error x, Error y)
            {
                return x.Line == y.Line && x.ErrorKind == y.ErrorKind && x.Message==y.Message;
            }
            public int GetHashCode(Error obj)
            {
                return obj.GetHashCode();
            }
        }

    }
}
