using SuperCOOL.Core;
using System;
using System.Collections.Generic;
using System.IO;
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

        class MessageErrorComparer : IEqualityComparer<Error>
        {
            public bool Equals(Error x, Error y)
            {
                return x.Message == y.Message;
            }

            public int GetHashCode(Error obj)
            {
                return obj.Message.GetHashCode();
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

    }
}
