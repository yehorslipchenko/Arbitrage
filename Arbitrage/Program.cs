using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arbitrage.Models;
using Arbitrage.Models.Graph;

namespace Arbitrage
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            // Welcome script and entering data
            Console.Write("Welcome to Arbitrage\n Please input a path to Data Set:");
            var path = Console.ReadLine();
            Console.Write("Also please input count of currencies:");
            var count_currency = int.Parse(Console.ReadLine() ?? string.Empty);
            
            // Configuring data and create list of currency how strings 
            var configureData = new ConfigureData(path, count_currency);
            var Data = configureData.CreateWorkSet();
            
            // Changing list of strings to List of currencies 
            var unFilteredFCurrencies = configureData.ParseToCurrency(Data);
            
            // Filtering and counting useful values
            var countUFCurrencies = unFilteredFCurrencies.Count;
            var currency = Currency.Filter(unFilteredFCurrencies);
            var countUCurrencies = currency.Count;
            
            // Message of that algorithm will been ready to start, output useful currencies from set and count of unuseful currencies
            Console.WriteLine($"Algorithm had been started\n" +
                              $"Count of currencies on start:{countUFCurrencies}\n" +
                              $"Count of currencies after filter:{countUCurrencies}\n" +
                              $"Count of thrown out currencies:{countUFCurrencies - countUCurrencies}");
            
            
            
            
            
            // Create Graph
            var graph = new Graph();
            graph.CreateGraph(currency);
        }
    }
}

// /Users/egorslipchenko/Documents/C#/Arbitrage/Arbitrage/Data/DataSet.txt          3503
// /Users/egorslipchenko/Documents/C#/Arbitrage/Arbitrage/Data/TestDataSet.txt      11

