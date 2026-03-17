using System;
using System.Windows.Markup;

namespace VisiWin.Toolkit.PlcSymbol
{
    /// <summary>
    /// Markup extension that extracts the path string from a <see cref="PlcSymbol"/>
    /// or <see cref="PlcArraySymbol"/> for use in XAML bindings.
    /// </summary>
    /// <remarks>
    /// This extension allows passing strongly typed PLC symbol definitions to
    /// markup extensions that only accept plain strings, without the need for
    /// additional static path properties.
    /// </remarks>
    /// <example>
    /// <code lang="xaml">
    /// &lt;!-- PlcSymbol --&gt;
    /// Text="{vw:VariableBinding VariableName={local:PlcPath {x:Static local:PlcSymbolDefinitions.Lifebit}}}"
    ///
    /// &lt;!-- PlcArraySymbol (returns BasePath) --&gt;
    /// Text="{vw:VariableBinding VariableName={local:PlcPath {x:Static local:PlcSymbolDefinitions.MyArray}}}"
    /// </code>
    /// </example>
    public class PlcSymbolPathExtension : MarkupExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlcSymbolPathExtension"/> class.
        /// </summary>
        public PlcSymbolPathExtension() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlcSymbolPathExtension"/> class
        /// with the specified symbol.
        /// </summary>
        /// <param name="symbol">
        /// A <see cref="PlcSymbol"/>, <see cref="PlcArraySymbol"/>, or <see cref="string"/>.
        /// </param>
        public PlcSymbolPathExtension(object symbol)
        {
            Symbol = symbol;
        }

        /// <summary>
        /// Gets or sets the PLC symbol whose path will be extracted.
        /// </summary>
        /// <remarks>
        /// Accepted types are <see cref="PlcSymbol"/>, <see cref="PlcArraySymbol"/>,
        /// and <see cref="string"/>. Any other type falls back to <see cref="object.ToString"/>.
        /// </remarks>
        public object Symbol { get; set; }

        /// <summary>
        /// Optional index for <see cref="PlcArraySymbol"/> access.
        /// </summary>
        public int Index { get; set; } = -1;

        /// <summary>
        /// Returns the PLC symbol path as a plain <see cref="string"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// A service provider helper that can provide services for the markup extension.
        /// Not used by this implementation.
        /// </param>
        /// <returns>
        /// The fully qualified PLC symbol path as a <see cref="string"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="Symbol"/> is <c>null</c>.
        /// </exception>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Symbol == null)
                throw new InvalidOperationException("Symbol must not be null.");

            if (Symbol is PlcSymbol plcSymbol)
                return plcSymbol.Path;

            if (Symbol is PlcArraySymbol arraySymbol)
            {
                return Index >= 0
                    ? arraySymbol[Index]   
                    : arraySymbol.BasePath;
            }

            if (Symbol is string str)
                return str;

            return Symbol.ToString();
        }
    }
}