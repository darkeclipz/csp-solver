using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csp.Model
{
    public static class Domain
    {
        internal static IEnumerable<int> Binary
        {
            get
            {
                return Enumerable.Range(0, 2);
            }
        }

        internal static IEnumerable<int> Range(int start, int stop)
        {
            if (start < 0 || stop < 0 || stop <= start)
            {
                throw new ArgumentException("Invalid range specified, start and stop must be positive and stop > start.");
            }
            return Enumerable.Range(start, stop - start);
        }
    }
}
