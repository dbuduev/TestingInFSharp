using System.Collections.Generic;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace TestingCSharp
{
    public class Shrinking
    {
        [Property]
        public void AtLeast(double x)
        {
            Assert.True(x > 4.3);
        }

        [Property]
        public void Weirdness(List<NonNull<string>> values)
        {
            if (values.Count > 3)
            {
                Assert.True(values[3].Get.Length < 3);
            }
        }
    }
}
