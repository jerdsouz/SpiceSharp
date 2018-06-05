﻿using System;

namespace SpiceSharp.Algebra
{
    /// <summary>
    /// Exception thrown when a matrix is singular
    /// </summary>
    public class SingularException : SparseException
    {
        /// <summary>
        /// Gets the index where the first zero-diagonal element was found
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SingularException()
            : base("Singular matrix")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index">Index</param>
        public SingularException(int index)
            : base("Singular matrix")
        {
            Index = index;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public SingularException(string message)
            : base(message)
        {
            Index = -1;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="message">Message</param>
        public SingularException(int index, string message)
            : base(message)
        {
            Index = index;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner exception</param>
        public SingularException(string message, Exception innerException)
            : base(message, innerException)
        {
            Index = -1;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner exception</param>
        public SingularException(int index, string message, Exception innerException)
            : base(message, innerException)
        {
            Index = index;
        }
    }
}