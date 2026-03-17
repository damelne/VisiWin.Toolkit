using System;

namespace VisiWin.Toolkit.PlcSymbol
{

    /// <summary>
    /// Represents a PLC array symbol, providing type-safe access to individual elements.
    /// </summary>
    /// <remarks>
    /// This class encapsulates a base PLC symbol path representing an array and provides indexer access to individual elements. 
    /// It also supports optional maximum length validation for array indices.
    /// </remarks>
    public sealed class PlcArraySymbol
    {
        /// <summary>
        /// Gets the base PLC symbol path without index accessor.
        /// </summary>
        public string BasePath { get; }

        /// <summary>
        /// Gets the optional maximum length of the PLC array.
        /// </summary>
        /// <remarks>
        /// If <c>null</c>, no upper bound validation is performed.
        /// </remarks>
        public int? MaxLength { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlcArraySymbol"/> class.
        /// </summary>
        /// <param name="basePath">
        /// The base PLC symbol path (without index accessor).
        /// </param>
        /// <param name="maxLength">
        /// Optional maximum array length. If specified, index access is validated
        /// against this upper bound.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="basePath"/> is null or whitespace.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="maxLength"/> is less than or equal to zero.
        /// </exception>
        public PlcArraySymbol(string basePath, int? maxLength = null)
        {
            if (string.IsNullOrWhiteSpace(basePath))
                throw new ArgumentException("Base path must not be empty.", nameof(basePath));

            if (maxLength.HasValue && maxLength.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxLength));

            BasePath = basePath;
            MaxLength = maxLength;
        }

        /// <summary>
        /// Returns a strongly typed <see cref="PlcSymbol"/> representing
        /// the array element at the specified index.
        /// </summary>
        /// <param name="index">Zero-based array index.</param>
        /// <returns>A <see cref="PlcSymbol"/> representing the indexed element.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the index is negative or exceeds <see cref="MaxLength"/>.
        /// </exception>
        public PlcSymbol At(int index)
        {
            return new PlcSymbol(this[index]);
        }

        /// <summary>
        /// Gets the fully qualified PLC symbol path for the specified index.
        /// </summary>
        /// <param name="index">Zero-based array index.</param>
        /// <returns>
        /// A string representing the indexed PLC symbol path (e.g. "BasePath[3]").
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the index is negative or exceeds <see cref="MaxLength"/>.
        /// </exception>
        public string this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), "Index must be >= 0");

                if (MaxLength.HasValue && index >= MaxLength.Value)
                    throw new ArgumentOutOfRangeException(nameof(index),
                        $"Index must be < {MaxLength.Value}");

                return string.Format("{0}[{1}]", BasePath, index);
            }
        }

        /// <summary>
        /// Implicitly converts the array symbol to its base PLC symbol path.
        /// </summary>
        /// <param name="symbol">The array symbol.</param>
        public static implicit operator string(PlcArraySymbol symbol)
        {
            return symbol.BasePath;
        }

        /// <summary>
        /// Returns the base PLC symbol path.
        /// </summary>
        public override string ToString()
        {
            return BasePath;
        }
    }
}