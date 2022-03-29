using System.Collections.Generic;
using System.Linq;
using Syntax.Elements;
using Syntax.Types;
using Syntax.Rules;

namespace Syntax
{
    /// <summary>
    ///     Grammar syntax class
    /// </summary>
    public class GrammarSyntax
    {
        /// <summary>
        ///     Element number
        /// </summary>
        private int _elementNumber;

        /// <summary>
        ///     Element types
        /// </summary>
        private readonly Dictionary<string, ElementType> _elementTypes;

        /// <summary>
        ///     Empty rules
        /// </summary>

        private readonly List<Rule> _emptyRules;

        /// <summary>
        ///     Rules
        /// </summary>

        private readonly List<Rule> _rules;

        /// <summary>
        ///     Standart type
        /// </summary>

        private readonly ElementType _startElementType;

        /// <summary>
        ///     Grammar
        /// </summary>
        public Grammar Grammar => new Grammar(_startElementType, _rules, _elementTypes);

        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="baseElements">Base elements</param>
        public GrammarSyntax(IEnumerable<Element> baseElements)
        {
            var elements = new List<Element>(baseElements);

            _emptyRules = new List<Rule>
            {
                new LeftRule(null, null, null),
                new UpRule(null, null, null)
            };

            _rules = new List<Rule>();
            _elementNumber = 1;
            _elementTypes = TerminalElementCreator.GetTerminalElementTypes();
            _startElementType = ConnectElementToSyntax(elements);
        }

        /// <summary>
        ///     Connect element to syntax
        /// </summary>
        ///
        /// <param name="elements">Elements</param>
        ///
        /// <returns>ElementType</returns>
        private ElementType ConnectElementToSyntax(List<Element> elements)
        {
            if (elements.Count == 1) 
            {
                return elements[0].ElementType;
            }
           
            RuleSearchResult searchRuleResult = null;
            foreach (var candidate in elements)
            {
                searchRuleResult = SearchRule(elements, candidate);
                if (searchRuleResult != null)
                {
                    break;
                }
            }

            if (searchRuleResult == null) 
            {
                throw new InvalidElementException();
            }
           
            var result = new ElementType($"O{_elementNumber++}");

            _elementTypes.Add(result.Name, result);
            _rules.Add(searchRuleResult.GetRule(result));

            return result;
        }

        /// <summary>
        ///     Search rule
        /// </summary>
        ///
        /// <param name="elements">Elements</param>
        /// <param name="candidate">Candidate</param>
        ///
        /// <returns>RuleSearchResult</returns>
        private RuleSearchResult SearchRule(List<Element> elements, Element candidate)
        {
            foreach (var rule in _emptyRules)
            {
                if (IsFirstInRule(rule, candidate, elements))
                {
                    elements.Remove(candidate);

                    return new RuleSearchResult
                    {
                        FirstElementType = candidate.ElementType,
                        SecondElementType = ConnectElementToSyntax(elements),
                        EmptyRule = rule,
                        Elements = elements
                    };
                }

                if (!IsSecondInRule(rule, candidate, elements))
                {
                    continue;
                }

                elements.Remove(candidate);

                return new RuleSearchResult
                {
                    FirstElementType = ConnectElementToSyntax(elements),
                    SecondElementType = candidate.ElementType,
                    EmptyRule = rule,
                    Elements = elements
                };
            }

            return null;
        }

        /// <summary>
        ///     Is first in rule
        /// </summary>
        ///
        /// <param name="rule">Rule</param>
        /// <param name="candidate">Candidate</param>
        /// <param name="otherElements">Other elements</param>
        ///
        /// <returns>bool</returns>
        private static bool IsFirstInRule(Rule rule, Element candidate, List<Element> otherElements)
        {
            return otherElements.All(element => !IsDifferentElementFirstInRule(rule, candidate, element));
        }

        /// <summary>
        ///     Is second in rule
        /// </summary>
        ///
        /// <param name="rule">Rule</param>
        /// <param name="candidate">Candidate</param>
        /// <param name="otherElements">Other elements</param>
        ///
        /// <returns>bool</returns>
        private static bool IsSecondInRule(Rule rule, Element candidate, List<Element> otherElements)
        {
            return otherElements.All(element => !IsDifferentElementSecondInRule(rule, candidate, element));
        }

        /// <summary>
        ///     Is different element first in rule
        /// </summary>
        ///
        /// <param name="rule">Rule</param>
        /// <param name="candidate">Candidate</param>
        /// <param name="element">Element</param>
        ///
        /// <returns>bool</returns>
        private static bool IsDifferentElementFirstInRule(Rule rule, Element candidate, Element element)
        {
            return candidate.StartPosition != element.StartPosition &&
                   candidate.EndPosition != element.EndPosition &&
                   ! rule.IsRulePositionPare(candidate, element);
        }

        /// <summary>
        ///     Is different element second in rule
        /// </summary>
        ///
        /// <param name="rule">Rule</param>
        /// <param name="candidate">Candidate</param>
        /// <param name="element">Element</param>
        ///
        /// <returns>bool</returns>
        private static bool IsDifferentElementSecondInRule(Rule rule, Element candidate, Element element)
        {
            return candidate.StartPosition != element.StartPosition &&
                   candidate.EndPosition != element.EndPosition &&
                   ! rule.IsRulePositionPare(element, candidate);
        }
    }
}