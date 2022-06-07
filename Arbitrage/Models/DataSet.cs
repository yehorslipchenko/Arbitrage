using System;
using System.Collections.Generic;
using System.IO;

namespace Arbitrage.Models
{
    public class DataSet
    {
        private string FilePath { get; }
        private List<string> Data { get; }
        private readonly int CountCurrency;
        
        public DataSet(string filePath, int countCurrency)
        {
            FilePath = filePath;
            CountCurrency = countCurrency;
            Data = new List<string>();
        }

        private void GenerateList()
        {
            var sr = new StreamReader(FilePath);
            for (var i = 0; i < CountCurrency; i++)
            {
                Data.Add(sr.ReadLine());
            }
            sr.Close();
            
        }

        public IEnumerable<string> CreateDataSet()
        {
            GenerateList();
            var dataSet = new List<string>();
            foreach (var str in Data)
            {
                var x = str.Split();
                var pairNumber = "";
                var token0Address = "";
                var token1Address = "";
                var token0Reverse = "";
                var token1Reverse = "";
                
                for (var i = 0; i < x.Length; i++)
                {
                    var cont = x[i].Contains("pair_number");
                    if (cont)
                    {
                        pairNumber = x[i + 1];
                    }

                    if (x[i].Contains("token0_name"))
                    {
                        token0Address = x[i + 1];
                    }

                    if (x[i].Contains("token1_name"))
                    {
                        token1Address = x[i + 1];
                    }

                    if (x[i].Contains("token0_reserve"))
                    {
                        token0Reverse = x[i + 1];
                    }
                    
                    if (x[i].Contains("token1_reserve"))
                    {
                        token1Reverse = x[i + 1];
                    }
                    
                }
                
                var r_pair_number = pairNumber.Replace(",", "");
                pairNumber = r_pair_number.Replace("'", "");
                    
                var r_token0_address = token0Address.Replace(",", "");
                token0Address = r_token0_address.Replace("'", "");
                    
                var r_token1_address = token1Address.Replace(",", "");
                token1Address = r_token1_address.Replace("'", "");
                    
                var r_token0_reverse = token0Reverse.Replace(",", "");
                token0Reverse = r_token0_reverse.Replace("'", "");
                    
                var r_token1_reverse = token1Reverse.Replace(",", "");
                var n = token1Reverse.Replace("}", "");
                token1Reverse = n.Replace("'", "");

                var currency = $"{pairNumber} {token0Address} {token1Address} {token0Reverse} {token1Reverse}";
                
                dataSet.Add(currency);
            }

            return dataSet;

        }

        public static List<Pair> ParseToCurrency(IEnumerable<string> Input)
        {
            var currency = new List<Pair>();
            if (currency == null) throw new ArgumentNullException(nameof(currency));
            
            foreach (var str in Input)
            {
                var strCurrency = str.Split();
                var pairNumber = int.Parse(strCurrency[0]);
                var token0Address = strCurrency[1];
                var token1Address = strCurrency[2];
                var toke0Reverse = float.Parse(strCurrency[3]);
                var token1Reverse = float.Parse(strCurrency[4]);
                currency.Add(new Pair(pairNumber,token0Address,token1Address,toke0Reverse,token1Reverse));
            }
            return currency;
        }
        
    }
}