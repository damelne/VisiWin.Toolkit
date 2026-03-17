using System;

namespace VisiWin.Toolkit.PlcSymbol
{
    /// <summary>
    /// Represents a strongly typed PLC symbol path.
    /// </summary>
    /// <remarks>
    /// This class encapsulates a fully qualified PLC variable path and
    /// prevents uncontrolled usage of raw string literals throughout the codebase.
    /// </remarks>
    public sealed class PlcSymbol
    {
        /// <summary>
        /// Gets the fully qualified PLC symbol path.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlcSymbol"/> class.
        /// </summary>
        /// <param name="path">
        /// The fully qualified PLC symbol path.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="path"/> is null or whitespace.
        /// </exception>
        public PlcSymbol(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Symbol path must not be empty.", nameof(path));

            Path = path;
        }

        /// <summary>
        /// Returns the PLC symbol path as a string.
        /// </summary>
        /// <returns>The fully qualified PLC symbol path.</returns>
        public override string ToString()
        {
            return Path;
        }

        /// <summary>
        /// Implicitly converts the <see cref="PlcSymbol"/> to its underlying string path.
        /// </summary>
        /// <param name="symbol">The PLC symbol.</param>
        /// <returns>The fully qualified PLC symbol path.</returns>
        public static implicit operator string(PlcSymbol symbol)
        {
            return symbol.Path;
        }
    }
}