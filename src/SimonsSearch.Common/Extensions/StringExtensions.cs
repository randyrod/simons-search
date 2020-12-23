using System;

namespace SimonsSearch.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsInvariant(this string me, string comparer)
        {
            return !string.IsNullOrWhiteSpace(me) &&
                   !string.IsNullOrWhiteSpace(comparer) &&
                   me.Contains(comparer, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}