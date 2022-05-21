using System;
using System.Collections.Generic;

namespace Arbitrage.Models
{
    public class Currency
    {
        public int pair_number;
        public string token0_address;
        public string token1_address;
        public float token0_reverse;
        public float token1_reverse;

        public Currency(int pairNumber, string token0Address, string token1Address, float token0Reverse,
            float token1Reverse)
        {
            pair_number = pairNumber;
            token0_address = token0Address ?? throw new ArgumentNullException(nameof(token0Address));
            token1_address = token1Address ?? throw new ArgumentNullException(nameof(token1Address));
            token0_reverse = token0Reverse;
            token1_reverse = token1Reverse;
        }

        public static  List<Currency> Filter(List<Currency> input)
        {
            var filtered_currency = new List<Currency>();
            foreach (var cur in input)
            {
                const float minValue_tokenReverse = (float) 1E+15;
                if (cur.token0_reverse < minValue_tokenReverse || cur.token1_reverse < minValue_tokenReverse)
                {
                    continue;
                }
                filtered_currency.Add(cur);
            }

            return filtered_currency;
        }
    }
}