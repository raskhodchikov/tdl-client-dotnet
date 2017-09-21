using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace TDL.Test.Specs.Utils
{
    internal static class SpecFlowTableExtensions
    {
        /// <summary>
        /// Converts a single columned table to an enumerable of strings including header.
        /// </summary>
        public static IEnumerable<string> ToStringsIncludingHeader(this Table table)
        {
            if (table.Header.Count == 0)
            {
                yield break;
            }

            if (table.Header.Count > 1)
            {
                throw new ArgumentException($"Table is not single columned. {table.Header.Count} columns in table.");
            }

            yield return table.Header.First();

            foreach (var row in table.Rows)
            {
                yield return row.First().Value;
            }
        }
    }
}
