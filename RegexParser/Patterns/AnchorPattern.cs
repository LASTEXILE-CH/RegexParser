﻿using System;
using Utility.PrettyPrint;

namespace RegexParser.Patterns
{
    public enum AnchorType
    {
        StartOfStringOrLine,
        StartOfString,
        StartOfLine,

        EndOfStringOrLine,
        EndOfString,
        EndOfLine,
        EndOfStringOrBeforeEndingNewline,

        ContiguousMatch,
        WordBoundary,
        NonWordBoundary,
    }

    public class AnchorPattern : BasePattern, IEquatable<AnchorPattern>
    {
        public AnchorPattern(AnchorType anchorType)
            : base(PatternType.Anchor, 0)
        {
            AnchorType = anchorType;
        }

        public AnchorType AnchorType { get; private set; }

        public override PPElement ToPrettyPrint()
        {
            return new PPText(Type.ToString(), string.Format("Anchor ({0})", AnchorType.ToString()));
        }

        bool IEquatable<AnchorPattern>.Equals(AnchorPattern other)
        {
            return other != null && this.AnchorType == other.AnchorType;
        }

        public override int GetHashCode()
        {
            return AnchorType.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<AnchorPattern>)this).Equals(obj as AnchorPattern);
        }
    }
}
