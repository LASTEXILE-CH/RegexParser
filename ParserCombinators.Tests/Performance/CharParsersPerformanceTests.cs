﻿using System;
using Utility.BaseTypes;
using Utility.ConsLists;
using Utility.Tests.Performance;

namespace ParserCombinators.Tests.Performance
{
    /// <summary>
    /// Does NOT contain unit tests, but performance-comparison tests to be run from a console.
    /// </summary>
    public static class CharParsersPerformanceTests
    {
        public static void AnyCharTest()
        {
            CharParserTest(CharParsers.AnyChar,
                           200, 1000000);
            
            // 12.80 sec. (maxItemCount = 1,000,000)
            // 26.09 sec. (maxItemCount = 2,000,000)
            // for traversal, time grows linearly with 'maxItemCount';
        }

        public static void OneOfTest()
        {
            CharParserTest(CharParsers.OneOf("0123456789"),
                           200, 1000000);

            // 61.08 sec. (maxItemCount = 1,000,000)
        }

        public static void ManyCharsTest()
        {
            CharParserTest(CharParsers.Many1(CharParsers.AnyChar),
                           200, 1000000);

            // 17.97 sec. (maxItemCount = 1,000,000)
            // 37.55 sec. (maxItemCount = 2,000,000)
        }

        public static void CharParserTest<TTree>(Parser<char, TTree> parser, int times, int maxItemCount)
        {
            CharParserTest(parser, times, maxItemCount,
                           EnumerablePerformanceTests.RepeatDigitChars(maxItemCount).AsString());
        }

        public static void CharParserTest<TTree>(Parser<char, TTree> parser, int times, int maxItemCount, string inputText)
        {
            IConsList<char> consList = new ArrayConsList<char>(inputText);

            DateTime start = DateTime.Now;

            IConsList<char> rest = null;
            for (int i = 0; i < times; i++)
            {
                Result<char, TTree> result = parser(consList);

                while (result != null)
                {
                    rest = result.Rest;
                    result = parser(result.Rest);
                }
            }
            
            if (rest != null && !rest.IsEmpty)
                Console.WriteLine("The remaining list is NOT empty.\n");

            TimeSpan time = DateTime.Now - start;
            TimeSpan timePerCycle = new TimeSpan(0, 0, 0, 0, (int)(time.TotalMilliseconds / times));

            Console.WriteLine(time);
            Console.WriteLine();
            //Console.WriteLine("{0}  per cycle", timePerCycle);
            //Console.WriteLine();
        }
    }
}
