using Xunit;

namespace SuperCOOL.Tests
{
    public class SanityTest
    {
        [Fact]
        public void Sum()
        {
            Assert.Equal( 132, 11 * 12 );
        }
    }
}
