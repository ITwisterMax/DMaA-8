using System;
using System.Collections.Generic;
using Syntax.Elements;
using Syntax.Rules;

namespace Syntax.Types
{
    /// <summary>
    ///     Rule search result
    /// </summary>
    internal class RuleSearchResult
    {
        /// <summary>
        ///     First element type
        /// </summary>
        internal ElementType FirstElementType { get; set; }

        /// <summary>
        ///     Seecond element type
        /// </summary>
        internal ElementType SecondElementType { get; set; }

        /// <summary>
        ///     Empty rule
        /// </summary>
        internal Rule EmptyRule { get; set; }

        /// <summary>
        ///     Elements
        /// </summary>
        internal List<Element> Elements { get; set; }

        /// <summary>
        ///     Get rule
        /// </summary>
        ///
        /// <param name="result">Result</param>
        ///
        /// <returns>Rule</returns>
        internal Rule GetRule(ElementType result)
        {
            return (Rule) Activator.CreateInstance(EmptyRule.GetType(), result, FirstElementType, SecondElementType);
        }
    }
}