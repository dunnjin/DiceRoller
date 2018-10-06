using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiceRoller.Common
{
    public static class Extensions
    {
        public static IEnumerable<string> RegexMatches(this string source, string regex)
        {
            return Regex
                .Matches(source, regex)
                .OfType<Match>()
                .Where(m => m.Captures.Count > 0)
                .Select(l => string.Concat(l.Captures
                    .OfType<Capture>()
                    .Select(c => c.Value)))
                .ToList();
        }
    }
}
