using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Tools
{
    public class LevenshteinDistanceCalculator
    {
        public const int MaxAllowedDistance = 3;

        public static int GetDistance(string stringA, string stringB)
        {
            var a = stringA.ToLower();

            var b = stringB.ToLower();

            if (string.IsNullOrEmpty(a))
            {
                return !string.IsNullOrEmpty(b) ? b.Length : 0;
            }

            if (string.IsNullOrEmpty(b))
            {
                return !string.IsNullOrEmpty(a) ? a.Length : 0;
            }

            var d = new int[a.Length + 1, b.Length + 1];

            var cost = 0;

            var min1 = 0;

            var min2 = 0;

            var min3 = 0;

            for (var i = 0; i <= d.GetUpperBound(0); i++)
            {
                d[i, 0] = i;
            }

            for (var i = 0; i <= d.GetUpperBound(1); i++)
            {
                d[0, i] = i;
            }

            for (var i = 1; i <= d.GetUpperBound(0); i++)
            {
                for (var j = 1; j <= d.GetUpperBound(1); j++)
                {
                    cost = Convert.ToInt32(a[i - 1] != b[j - 1]);

                    min1 = d[i - 1, j] + 1;

                    min2 = d[i, j - 1] + 1;

                    min3 = d[i - 1, j - 1] + cost;

                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }
    }
}
