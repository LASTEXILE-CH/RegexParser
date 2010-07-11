﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.ParserCombinators.ConsLists;

namespace RegexParser.Patterns
{
    public abstract class BasePattern
    {
        public static BasePattern CreatePattern(string patternText)
        {
            var result = new PatternParsers().WholePattern(new ArrayConsList<char>(patternText));
            
            return result.Rest.IsEmpty ? result.Value : null;
        }
    }
}
