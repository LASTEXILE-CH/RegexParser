﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Util;

namespace RegexParser.Patterns
{
    public class CharClassPattern : BasePattern, IEquatable<CharClassPattern>
    {
        public CharClassPattern(bool isPositive, IEnumerable<char> charSet)
            : this(isPositive, charSet, null) { }

        public CharClassPattern(bool isPositive, IEnumerable<CharRange> charRanges)
            : this(isPositive, null, charRanges) { }

        public CharClassPattern(bool isPositive, IEnumerable<char> charSet, IEnumerable<CharRange> charRanges)
        {
            IsPositive = isPositive;

            if (charSet != null)
                CharSet = new string(charSet.OrderBy(c => c)
                                            .GroupBy(c => c)
                                            .Select(g => g.First())
                                            .ToArray());

            if (charRanges != null)
                CharRanges = charRanges.OrderBy(r => r.From)
                                       .ToArray();
        }

        public readonly bool IsPositive;
        public readonly string CharSet = "";
        public readonly CharRange[] CharRanges = new CharRange[] { };

        public bool IsMatch(char c)
        {
            return !IsPositive ^ isPositiveMatch(c);
        }

        private bool isPositiveMatch(char c)
        {
            foreach (var charRange in CharRanges)
                if (charRange.From <= c && c <= charRange.To)
                    return true;

            return CharSet != string.Empty && CharSet.Contains(c);
        }

        public override string ToString()
        {
            return string.Format("CharClass {{{0}\"{1}{2}\"}}",
                                 IsPositive ? "" : "^",
                                 string.Join("", CharRanges.Select(r => r.ToString()).ToArray()),
                                 CharSet);
        }


        public class CharRange : IEquatable<CharRange>
        {
            public CharRange(char from, char to)
            {
                From = from;
                To = to;
            }

            public readonly char From;
            public readonly char To;

            public override string ToString()
            {
                return string.Format("{0}-{1}", From, To);
            }

            #region CharRange Equality

            bool IEquatable<CharRange>.Equals(CharRange other)
            {
                return other != null && this.From == other.From && this.To == other.To;
            }

            public override int GetHashCode()
            {
                return HashCodeCombiner.Combine(From.GetHashCode(), To.GetHashCode());
            }

            public override bool Equals(object obj)
            {
                return ((IEquatable<CharRange>)this).Equals(obj as CharRange);
            }

            public static bool operator ==(CharRange op1, CharRange op2)
            {
                return object.ReferenceEquals(op1, op2) || op1.Equals(op2);
            }

            public static bool operator !=(CharRange op1, CharRange op2)
            {
                return !(op1 == op2);
            }

            #endregion
        }


        #region CharClassPattern Equality

        bool IEquatable<CharClassPattern>.Equals(CharClassPattern other)
        {
            return !object.ReferenceEquals(other, null) &&
                   this.IsPositive == other.IsPositive &&
                   this.CharSet == other.CharSet &&
                   this.CharRanges.Length == other.CharRanges.Length &&
                   Enumerable.Range(0, this.CharRanges.Length)
                             .All(i => this.CharRanges[i].Equals(other.CharRanges[i]));
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(IsPositive.GetHashCode(), CharSet.GetHashCode(),
                                            HashCodeCombiner.Combine(CharRanges.Select(r => r.GetHashCode()).ToArray()));
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<CharClassPattern>)this).Equals(obj as CharClassPattern);
        }

        public static bool operator ==(CharClassPattern op1, CharClassPattern op2)
        {
            return object.ReferenceEquals(op1, op2) || op1.Equals(op2);
        }

        public static bool operator !=(CharClassPattern op1, CharClassPattern op2)
        {
            return !(op1 == op2);
        }

        #endregion
    }
}