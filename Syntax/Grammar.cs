using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syntax.Elements;
using Syntax.Types;
using Syntax.Rules;

namespace Syntax
{
    /// <summary>
    ///     Grammar class
    /// </summary>
    public class Grammar
    {
        /// <summary>
        ///     Element types
        /// </summary>
        private Dictionary<string, ElementType> ElementTypes { get; set; }

        /// <summary>
        ///     Rules
        /// </summary>
        private List<Rule> Rules { get; set; }

        /// <summary>
        ///     Start element type
        /// </summary>
        private ElementType StartElementType { get; set; }

        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="startElementType">Start element type</param>
        /// <param name="rules">Rules</param>
        /// <param name="elementTypes">Element types</param>
        public Grammar(ElementType startElementType, List<Rule> rules, Dictionary<string, ElementType> elementTypes)
        {
            Rules = rules;
            StartElementType = startElementType;
            ElementTypes = elementTypes;
        }

        /// <summary>
        ///     Default constructor
        /// </summary>
        protected Grammar()
        {

        }

        /// <summary>
        ///     Get image
        /// </summary>
        ///
        /// <returns>Element</returns>
        public Element GetImage()
        {
            return GetElement(StartElementType);
        }

        /// <summary>
        ///     Get element
        /// </summary>
        ///
        /// <param name="elementType">Element type</param>
        ///
        /// <returns>Element</returns>
        private Element GetElement(ElementType elementType)
        {
            if (elementType is TerminalElementType terminalElementType)
            {
                return terminalElementType.StandartElement;
            }

            var rule = Rules.FirstOrDefault(x => x.StartElementType.Name == elementType.Name);

            return rule.TransformConnect(GetElement(rule.FirstArgumentType), GetElement(rule.SecondArgumentType));
        }

        /// <summary>
        ///     Check if drawing is correct
        /// </summary>
        ///
        /// <param name="baseElements">Base elements</param>
        ///
        /// <returns>RecognazingResult</returns>
        public RecognizingResult IsImage(IEnumerable<Element> baseElements)
        {
            var elements = new ConcurrentBag<Element>(baseElements);

            foreach (var rule in Rules)
            {
                var result = ContainingRuleArguments(elements, rule);
                elements = result.Elements;

                if (!result.IsElementFound)
                {
                    return new RecognizingResult(rule.StartElementType.Name, false);
                }
                    
            }

            return new RecognizingResult("", true);
        }

        /// <summary>
        ///     Check if drawing contains rule arguments
        /// </summary>
        ///
        /// <param name="elements">Elements</param>
        /// <param name="rule">Rules</param>
        ///
        /// <returns>ContainingRuleAgrumentsResult</returns>
        private static ContainingRuleArgumentsResult ContainingRuleArguments(ConcurrentBag<Element> elements, Rule rule)
        {
            var result = new ContainingRuleArgumentsResult
            {
                Elements = new ConcurrentBag<Element>(elements),
                IsElementFound = false
            };

            foreach (var firstElement in elements)
            {
                if (firstElement.ElementType.Name == rule.FirstArgumentType.Name)
                {
                    result = ContainingRuleArgumentsForFirstElement(elements, rule, firstElement,
                        result);
                }
            }

            return result;
        }

        /// <summary>
        ///     Check if drawing contains rule arguments for first element
        /// </summary>
        ///
        /// <param name="elements">Elements</param>
        /// <param name="rule">Rule</param>
        /// <param name="firstElement">First element</param>
        /// <param name="result">Result</param>
        ///
        /// <returns>ContainingRuleArgumentsResult</returns>
        private static ContainingRuleArgumentsResult ContainingRuleArgumentsForFirstElement(
            IEnumerable<Element> elements,
            Rule rule,
            Element firstElement,
            ContainingRuleArgumentsResult result
        ) {
            var element = firstElement;

            Parallel.ForEach(elements, (Element secondElement) =>
            {
                if (!rule.IsRulePare(element, secondElement))
                {
                    return;
                }
                
                result.Elements.Add(rule.Connect(element, secondElement));
                result.IsElementFound = true;
            });

            return result;
        }

        /// <summary>
        ///     To string
        /// </summary>
        ///
        /// <returns>string</returns>
        public override string ToString()
        {
            var result = new StringBuilder();

            foreach (var rule in Rules)
            {
                result.AppendFormat(
                    "{0} -> {1}({2}, {3}); {4}",
                    rule.StartElementType.Name,
                    rule.Name,
                    rule.FirstArgumentType.Name,
                    rule.SecondArgumentType.Name,
                    Environment.NewLine
                );
            }

            return result.ToString();
        }

        /// <summary>
        ///     Class for rule arguments check result
        /// </summary>
        private class ContainingRuleArgumentsResult
        {
            /// <summary>
            ///     Elements
            /// </summary>
            public ConcurrentBag<Element> Elements { get; set; }

            /// <summary>
            ///     Is element found
            /// </summary>
            public bool IsElementFound { get; set; }
        }
    }
}