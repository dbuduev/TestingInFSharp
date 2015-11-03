using System.Linq;
using FsCheck.Xunit;
using Xunit;

namespace TestingCSharp
{
    public class SimpleTests
    {
        [Property]
        public void Doubles(int x)
        {
            Assert.Equal(x + x, 2 * x);
        }

        [Property]
        public void Triples(int x)
        {
            Assert.Equal(x + x, x * 3);
        }




        [Property]
        public void ReversingDoesNotChangeTheLength(int[] list)
        {
            Assert.Equal(list.Count(), list.Reverse().Count());
        }

        [Property]
        public void ReversingDoesNotChangeIt(int[] list)
        {
            Assert.True(list.SequenceEqual(list.Reverse()));
        }
    }
}
