using System.Windows;
using Syntax.Elements;

namespace Syntax.Types
{
    /// <summary>
    ///     Terminal element type class
    /// </summary>
    internal class TerminalElementType : ElementType
    {
        /// <summary>
        ///     Standart element line
        /// </summary>
        private readonly Line _standartElementLine;

        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="name">Name</param>
        /// <param name="standartElementLine">Standart element line</param>
        public TerminalElementType(string name, Line standartElementLine) : base(name)
        {
            _standartElementLine = standartElementLine;
        }

        /// <summary>
        ///     Standart element getter
        /// </summary>
        public Element StandartElement
        {
            get
            {
                return new Element(
                    this,
                    new Line(new Point(_standartElementLine.From.X, _standartElementLine.From.Y),
                    new Point(_standartElementLine.To.X, _standartElementLine.To.Y))
                );
            }
        }
    }
}