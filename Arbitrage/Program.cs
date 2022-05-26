﻿using System;
using Arbitrage.Models;
using Arbitrage.Models.Graph;

namespace Arbitrage
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            
            // Welcome script and entering data
            Console.Write("Welcome to Arbitrage\n Please input a path to Data Set:");
            var path = Console.ReadLine();

            Console.Write("Also please input count of currencies:");
            var countCurrency = int.Parse(Console.ReadLine() ?? string.Empty);
            // Configuring data and create list of currency how strings 
            var configureData = new DataSet(path, countCurrency);
            var data = configureData.CreateDataSet();
            
            // Changing list of strings to List of currencies 
            var unFiltered = DataSet.ParseToCurrency(data);
            
            // Filtering
            var currency = Currency.Filter(unFiltered);
            
            // Create Graph
            var graph = new Graph();
            graph.CreateGraph(currency);

            // Message of that algorithm will been ready to start, output useful currencies from set and count of unuseful currencies
            Console.WriteLine(
                $"Algorithm had been started\nCount of currencies on start:{unFiltered.Count}\nCount of currencies after filter:{currency.Count}\nCount of thrown out currencies:{unFiltered.Count - currency.Count}");
            
            // Search arbitrage opportunity
            const int indexVertex = 0;
            var res = graph.BellmanFordAlgorithm(indexVertex);

            // Output of count of negative cycles
            Console.WriteLine(res != 0
                ? $"We found {res} negative cycles.\nIt means that market isn't efficient, and we have {res} arbitrage opportunities!"
                : "Market is not efficient!");
            
            return 0;
        }
    }
}

// /Users/egorslipchenko/Documents/C#/Arbitrage/Arbitrage/Data/DataSet.txt          3503
// /Users/egorslipchenko/Documents/C#/Arbitrage/Arbitrage/Data/SecondDataSet.txt      1137
