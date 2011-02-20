﻿using System;
using System.Collections.Generic;
using System.Linq;
using RegexParser.Patterns;
using RegexParser.Util;

namespace RegexParser.Transforms
{
    /// <summary>
    /// Class representing an Abstract Syntax Tree (AST) transform.
    /// </summary>
    public abstract class BaseASTTransform
    {
        public virtual BasePattern Transform(BasePattern pattern)
        {
            // The Identity transform

            if (pattern is GroupPattern)
                return new GroupPattern(((GroupPattern)pattern).Patterns
                                                               .Select(a => Transform(a)));

            else if (pattern is QuantifierPattern)
            {
                QuantifierPattern quant = (QuantifierPattern)pattern;

                return new QuantifierPattern(Transform(quant.ChildPattern),
                                             quant.MinOccurrences, quant.MaxOccurrences, quant.IsGreedy);
            }

            else if (pattern is AlternationPattern)
                return new AlternationPattern(((AlternationPattern)pattern).Alternatives
                                                                           .Select(a => Transform(a)));

            else
                return pattern;
        }
    }
}
