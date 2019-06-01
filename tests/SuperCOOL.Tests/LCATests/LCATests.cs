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
            Assert.Equal( DataTypes.type1, DataTypes.cu.GetTypeLCA( DataTypes.type1, DataTypes.type1 ) );
            Assert.Equal( DataTypes.type1, DataTypes.cu.GetTypeLCA( DataTypes.type2, DataTypes.type3 ) );
            Assert.Equal( DataTypes.type1, DataTypes.cu.GetTypeLCA( DataTypes.type2, DataTypes.type13 ) );
            Assert.Equal( DataTypes.type7, DataTypes.cu.GetTypeLCA( DataTypes.type7, DataTypes.type9 ) );
            Assert.Equal( DataTypes.type3, DataTypes.cu.GetTypeLCA( DataTypes.type3, DataTypes.type13 ) );
            Assert.Equal( DataTypes.type3, DataTypes.cu.GetTypeLCA( DataTypes.type3, DataTypes.type14 ) );
            Assert.Equal( DataTypes.type3, DataTypes.cu.GetTypeLCA( DataTypes.type3, DataTypes.type17 ) );
            Assert.Equal( DataTypes.type3, DataTypes.cu.GetTypeLCA( DataTypes.type3, DataTypes.type16 ) );
            Assert.Equal( DataTypes.type14, DataTypes.cu.GetTypeLCA( DataTypes.type16, DataTypes.type17 ) );
            Assert.Equal( DataTypes.type1, DataTypes.cu.GetTypeLCA( DataTypes.type6, DataTypes.type8 ) );
            Assert.Equal( DataTypes.type2, DataTypes.cu.GetTypeLCA( DataTypes.type2, DataTypes.type10 ) );
            Assert.Equal( DataTypes.type2, DataTypes.cu.GetTypeLCA( DataTypes.type2, DataTypes.type11 ) );
            Assert.Equal( DataTypes.type2, DataTypes.cu.GetTypeLCA( DataTypes.type2, DataTypes.type8 ) );
            Assert.Equal( DataTypes.type7, DataTypes.cu.GetTypeLCA( DataTypes.type7, DataTypes.type11 ) );
            Assert.Equal( DataTypes.type1, DataTypes.cu.GetTypeLCA( DataTypes.type7, DataTypes.type16 ) );
        }
    }
}
