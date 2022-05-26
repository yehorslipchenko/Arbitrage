using System;
using System.Collections.Generic;

namespace Arbitrage.Models
{
    public class Currency
    {
        public readonly int PairNumber;
        public readonly string Token0Address;
        public readonly string Token1Address;
        public readonly float Token0Reverse;
        public readonly float Token1Reverse;

        public Currency(int pairNumber, string token0Address, string token1Address, float token0Reverse,
            float token1Reverse)
        {
            PairNumber = pairNumber;
            Token0Address = token0Address ?? throw new ArgumentNullException(nameof(token0Address));
            Token1Address = token1Address ?? throw new ArgumentNullException(nameof(token1Address));
            Token0Reverse = token0Reverse;
            Token1Reverse = token1Reverse;
        }

        public static  List<Currency> Filter(List<Currency> input)
        {
            var filteredCurrency = new List<Currency>();
            foreach (var currency in input)
            {
                const float minValue_tokenReverse = (float) 1E+15;
                if (currency.Token0Reverse < minValue_tokenReverse || currency.Token1Reverse < minValue_tokenReverse)
                {
                    continue;
                }
                filteredCurrency.Add(currency);
            }

            return filteredCurrency;
        }
    }
}