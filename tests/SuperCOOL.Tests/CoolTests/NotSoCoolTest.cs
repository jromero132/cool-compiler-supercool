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

      
    }
}
