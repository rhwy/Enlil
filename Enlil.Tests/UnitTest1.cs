using System;
using NFluent;
using Xunit;

namespace Enlil.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var c = new Class1();
            Check.That(c).IsNotNull();
        }
    }
}