using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace OrderedJobs.Test
{
    internal class Descending : IComparer<char>
    {
        public int Compare([AllowNull] char x, [AllowNull] char y)
        {
            return y.CompareTo(x);
        }
    }
}