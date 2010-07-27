﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using RegexParser.Patterns;

namespace RegexParser.Matchers
{
    public class MatchParsers : CharParsers
    {
        public static Parser<char, string> CreateParser(BasePattern pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException("pattern.", "Pattern is null when creating match parser.");

            if (pattern is GroupPattern)
                return from vs in Sequence(((GroupPattern)pattern).Patterns.Select(p => CreateParser(p)))
                       select new string(vs.SelectMany(v => v).ToArray());

            else if (pattern is QuantifierPattern)
            {
                QuantifierPattern quant = (QuantifierPattern)pattern;

                return from vs in Count(quant.MinOccurrences, quant.MaxOccurrences, CreateParser(quant.ChildPattern))
                       select new string(vs.SelectMany(v => v).ToArray());
            }

            else if (pattern is CharPattern)
                return from c in Satisfy(((CharPattern)pattern).IsMatch)
                       select new string(c, 1);

            else
                return null;
        }
    }
}
