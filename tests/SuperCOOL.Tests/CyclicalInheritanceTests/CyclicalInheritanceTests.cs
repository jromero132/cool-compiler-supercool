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
        static readonly CoolType type1, type2;
        static readonly CompilationUnit cu;
        static CyclicalInheritanceTests()
        {

            type1 = new CoolType( "object", type2 )
            {
                Childs = new List<CoolType>()
                {
                    type2
                }
            };
            type2 = new CoolType( "b", type1 )
            {
                Childs = new List<CoolType>()
                {
                    type1
                }
            };
            type1 = new CoolType( "object", type2 )
            {
                Childs = new List<CoolType>()
                {
                    type2
                }
            };
            cu = new CompilationUnit
            {
                Types = new HashSet<CoolType>()
                {
                    type1,
                    type2
                }
            };
        }

        [Fact]
        public void CyclicalInheritanceTest()
        {
            Assert.True( DataTypes.cu.NotCyclicalInheritance() );
            Assert.False( cu.NotCyclicalInheritance() );
        }
    }
}