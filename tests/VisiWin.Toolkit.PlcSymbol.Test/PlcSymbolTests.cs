using NUnit.Framework;
using Shouldly;
using System;

namespace VisiWin.Toolkit.PlcSymbol.Test
{
    public class Tests
    {
        [Test]
        public void Constructor_Should_Throw_When_Path_Is_Null()
        {
            Should.Throw<ArgumentException>(() => new PlcSymbol(null))
                  .ParamName.ShouldBe("path");
        }

        [Test]
        public void ToString_Should_Return_Path()
        {
            var symbol = new PlcSymbol("Test.Path");
            symbol.ToString().ShouldBe("Test.Path");
        }

        [Test]
        public void Implicit_Conversion_Should_Return_Path()
        {
            var symbol = new PlcSymbol("Test.Path");
            string result = symbol;
            result.ShouldBe("Test.Path");
        }
    }
}