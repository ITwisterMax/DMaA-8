using System;

namespace Syntax.Types
{
    /// <summary>
    ///     Invalid element exception class
    /// </summary>
    public class InvalidElementException : Exception
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public InvalidElementException() : base("Рисунок не распознан!")
        {
        }
    }
}