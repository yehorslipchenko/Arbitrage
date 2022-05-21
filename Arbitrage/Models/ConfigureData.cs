using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Arbitrage.Models
{
    public class ConfigureData
    {
        private string FilePath { get; }
        private List<string> Data { get; }
        private readonly int CountCurrency;
        
        public ConfigureData(string filePath, int countCurrency)
        {
            this.FilePath = filePath;
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

        public List<string> CreateWorkSet()
        {
            GenerateList();
            var WorkList = new List<string>();
            foreach (var str in Data)
            {
                var x = str.Split();
                var pair_number = "";
                var token0_address = "";
                var token1_address = "";
                var token0_reverse = "";
                var token1_reverse = "";
                for (int i = 0; i < x.Length; i++)
                {
                    bool cont = x[i].Contains("pair_number");
                    if (cont)
                    {
                        pair_number = x[i + 1];
                    }

                    if (x[i].Contains("token0_name"))
                    {
                        token0_address = x[i + 1];
                    }

                    if (x[i].Contains("token1_name"))
                    {
                        token1_address = x[i + 1];
                    }

                    if (x[i].Contains("token0_reserve"))
                    {
                        token0_reverse = x[i + 1];
                    }
                    
                    if (x[i].Contains("token1_reserve"))
                    {
                        token1_reverse = x[i + 1];
                    }
                    
                }
                
                var r_pair_number = pair_number.Replace(",", "");
                pair_number = r_pair_number.Replace("'", "");
                    
                var r_token0_address = token0_address.Replace(",", "");
                token0_address = r_token0_address.Replace("'", "");
                    
                var r_token1_address = token1_address.Replace(",", "");
                token1_address = r_token1_address.Replace("'", "");
                    
                var r_token0_reverse = token0_reverse.Replace(",", "");
                token0_reverse = r_token0_reverse.Replace("'", "");
                    
                var r_token1_reverse = token1_reverse.Replace(",", "");
                var n = token1_reverse.Replace("}", "");
                token1_reverse = n.Replace("'", "");

                var currency = $"{pair_number} {token0_address} {token1_address} {token0_reverse} {token1_reverse}";
                
                WorkList.Add(currency);
            }

            return WorkList;

        }

        public List<Currency> ParseToCurrency(List<string> Input)
        {
            var currency = new List<Currency>();
            if (currency == null) throw new ArgumentNullException(nameof(currency));
            foreach (var str in Input)
            {
                var str_currency = str.Split();
                var pair_number = int.Parse(str_currency[0]);
                var token0_address = str_currency[1];
                var token1_address = str_currency[2];
                var toke0_reverse = float.Parse(str_currency[3]);
                var token1_reverse = float.Parse(str_currency[4]);
                currency.Add(new Currency(pair_number,token0_address,token1_address,toke0_reverse,token1_reverse));
            }

            return currency;
        }
        
    }
}