using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using SuperCOOL.Core;
using SuperCOOL.Tests.Constant;

namespace SuperCOOL.Tests.CyclicalInheritanceTests
{
    public class CyclicalInheritanceTests
    {
        [Fact]
        public void CyclicalInheritanceTest()
        {
            Assert.True( DataTypes.cu.NotCyclicalInheritance() );
        }
    }
}