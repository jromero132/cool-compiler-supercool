using SuperCOOL.Core;
using System.Collections.Generic;
using Xunit;

using SuperCOOL.Tests.Constant;

namespace SuperCOOL.Tests.LCATests
{
    public class LCATests
    {
        [Fact]
        public void LCATest()
        {
            Assert.Equal( DataTypes.cu.Object, DataTypes.cu.GetTypeLCA(DataTypes.cu.Object, DataTypes.cu.Object) );
            Assert.Equal( DataTypes.cu.Object, DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("b"), DataTypes.cu.GetTypeIfDef("c")));
            Assert.Equal( DataTypes.cu.Object, DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("b"), DataTypes.cu.GetTypeIfDef("m")));
            Assert.Equal( DataTypes.cu.GetTypeIfDef("g"), DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("g"), DataTypes.cu.GetTypeIfDef("i")));
            Assert.Equal( DataTypes.cu.GetTypeIfDef("c"), DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("c"), DataTypes.cu.GetTypeIfDef("m")));
            Assert.Equal( DataTypes.cu.GetTypeIfDef("c"), DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("c"), DataTypes.cu.GetTypeIfDef("n")));
            Assert.Equal( DataTypes.cu.GetTypeIfDef("c"), DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("c"), DataTypes.cu.GetTypeIfDef("q")));
            Assert.Equal( DataTypes.cu.GetTypeIfDef("c"), DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("c"), DataTypes.cu.GetTypeIfDef("p")));
            Assert.Equal( DataTypes.cu.GetTypeIfDef("n"), DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("p"), DataTypes.cu.GetTypeIfDef("q")));
            Assert.Equal( DataTypes.cu.Object, DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("f"), DataTypes.cu.GetTypeIfDef("h")));
            Assert.Equal( DataTypes.cu.GetTypeIfDef("b"), DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("b"), DataTypes.cu.GetTypeIfDef("j")));
            Assert.Equal( DataTypes.cu.GetTypeIfDef("b"), DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("b"), DataTypes.cu.GetTypeIfDef("k")));
            Assert.Equal( DataTypes.cu.GetTypeIfDef("b"), DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("b"), DataTypes.cu.GetTypeIfDef("h")));
            Assert.Equal( DataTypes.cu.GetTypeIfDef("g"), DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("g"), DataTypes.cu.GetTypeIfDef("k")));
            Assert.Equal( DataTypes.cu.Object, DataTypes.cu.GetTypeLCA(DataTypes.cu.GetTypeIfDef("g"), DataTypes.cu.GetTypeIfDef("p")));
        }
    }
}
