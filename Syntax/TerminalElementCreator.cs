using System;
using System.Collections.Generic;
using System.Windows;
using Syntax.Elements;
using Syntax.Types;

namespace Syntax
{
    /// <summary>
    ///     Terminal element creator class
    /// </summary>
    public static class TerminalElementCreator
    {
        /// <summary>
        ///     Element types
        /// </summary>
        private static Dictionary<string, ElementType> _elementTypes;

        /// <summary>
        ///     Element types
        /// </summary>
        private static Dictionary<string, ElementType> ElementTypes => _elementTypes ??= GetTerminalElementTypes();

        /// <summary>
        ///     Get terminal element types
        /// </summary>
        ///
        /// <returns>Dictionary<string, ElementType></returns>
        public static Dictionary<string, ElementType> GetTerminalElementTypes()
        {
            return new Dictionary<string, ElementType>()
            {
                {"a1", new TerminalElementType("a1", new Line(new Point(0, 0), new Point(10, 0)))},
                {"a2", new TerminalElementType("a2", new Line(new Point(0, 0), new Point(0, 10)))},
                {"a3", new TerminalElementType("a3", new Line(new Point(0, 0), new Point(10, 10)))},
                {"a4", new TerminalElementType("a4", new Line(new Point(10, 0), new Point(0, 10)))}
            };
        }

        /// <summary>
        ///     Get terminal element
        /// </summary>
        ///
        /// <param name="line">Line</param>
        ///
        /// <returns>Element</returns>
        public static Element GetTerminalElement(Line line)
        {
            var resultName = GetTerminalElementName(line);

            return new Element(ElementTypes[resultName], line);
        }

        /// <summary>
        ///     Get terminal element name
        /// </summary>
        ///
        /// <param name="line">Line</param>
        ///
        /// <returns>string</returns>
        private static string GetTerminalElementName(Line line)
        {
            var deltaX = line.From.X - line.To.X;
            var deltaY = line.From.Y - line.To.Y;

            if (Math.Abs(deltaY) < 1 || Math.Abs(deltaY / deltaX) < 0.2) 
            {
                return "a1"; 
            }

            if (Math.Abs(deltaX) < 1 || Math.Abs(deltaX) < 1 || Math.Abs(deltaX / deltaY) < 0.2) 
            { 
                return "a2"; 
            } 

            var highPoint = line.To.Y > line.From.Y ? line.To : line.From;
            var lowPoint = line.To.Y < line.From.Y ? line.To : line.From;

            return highPoint.X < lowPoint.X ? "a4" : "a3";
        }
    }
}