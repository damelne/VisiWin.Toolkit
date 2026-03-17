using NUnit.Framework;
using Shouldly;
using System;

namespace VisiWin.Toolkit.PlcSymbol.Test
{
    public class PlcArraySymbolTests
    {
        [Test]
        public void Indexer_Should_Throw_When_Index_Negative()
        {
            var array = new PlcArraySymbol("My.Array");

            Should.Throw<ArgumentOutOfRangeException>(() => { var _ = array[-1]; })
                  .ParamName.ShouldBe("index");
        }

        [Test]
        public void Indexer_Should_Return_Correct_Path()
        {
            var array = new PlcArraySymbol("My.Array");
            array[3].ShouldBe("My.Array[3]");
        }

        [Test]
        public void At_Should_Return_PlcSymbol()
        {
            var array = new PlcArraySymbol("My.Array");
            var symbol = array.At(2);
            symbol.Path.ShouldBe("My.Array[2]");
        }
    }
}