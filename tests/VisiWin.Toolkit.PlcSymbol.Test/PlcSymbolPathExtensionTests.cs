using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisiWin.Toolkit.PlcSymbol.Test
{
    public class PlcSymbolPathExtensionTests
    {
        [Test]
        public void ProvideValue_Should_Throw_When_Symbol_Is_Null()
        {
            var ext = new PlcSymbolPathExtension();

            Should.Throw<InvalidOperationException>(() => ext.ProvideValue(null))
                  .Message.ShouldBe("Symbol must not be null.");
        }

        [Test]
        public void ProvideValue_Should_Return_Path_For_PlcSymbol()
        {
            var symbol = new PlcSymbol("Test.Path");
            var ext = new PlcSymbolPathExtension(symbol);

            var result = ext.ProvideValue(null);

            result.ShouldBe("Test.Path");
        }

        [Test]
        public void ProvideValue_Should_Return_BasePath_For_PlcArraySymbol_Without_Index()
        {
            var array = new PlcArraySymbol("My.Array");
            var ext = new PlcSymbolPathExtension(array);

            var result = ext.ProvideValue(null);

            result.ShouldBe("My.Array");
        }

        [Test]
        public void ProvideValue_Should_Return_ElementPath_For_PlcArraySymbol_With_Index()
        {
            var array = new PlcArraySymbol("My.Array");
            var ext = new PlcSymbolPathExtension(array) { Index = 2 };

            var result = ext.ProvideValue(null);

            result.ShouldBe("My.Array[2]");
        }

        [Test]
        public void ProvideValue_Should_Return_String_As_Is()
        {
            var ext = new PlcSymbolPathExtension("JustAString");

            var result = ext.ProvideValue(null);

            result.ShouldBe("JustAString");
        }

        [Test]
        public void ProvideValue_Should_Fallback_To_ToString_For_Other_Types()
        {
            var obj = new { Name = "X" };
            var ext = new PlcSymbolPathExtension(obj);

            var result = ext.ProvideValue(null);

            result.ShouldBe(obj.ToString());
        }
    }
}
