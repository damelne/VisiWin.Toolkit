using VisiWin.Toolkit.PlcSymbol;

namespace HMI.PlcSymbols
{
    public static class Definitions
    {
        public static PlcSymbol MyVariable { get; } = new PlcSymbol("MyVariable");

        public static PlcArraySymbol MyArrayVariable { get; } = new PlcArraySymbol("MyArray", 3);
    }
}