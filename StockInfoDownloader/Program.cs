using System.IO;
using StockInfoCommons.CompanyListings;
using StockInfoCommons.Edgar;
using StockInfoCommons.Financials;
using StockInfoCommons.GoogleFinance;
using StockInfoCommons.Metrics;
using StockInfoCommons.Simulation;
using System.Diagnostics;
using System;

namespace StockInfoDownloader
{
    class Program
    {

        static void Main(string[] args)
        {
            DBUtility db = new DBUtility();
            db.InitTables();


            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(Program)).Location);

            string symbol = "AAPL";

            System.Console.WriteLine("Search data for symbol: " + symbol);

            EdgarDownloader edownloader = new EdgarDownloader(basePath+ "\\stocks\\edgar\\filings", symbol);
            System.Console.WriteLine("Download data for symbol: " + symbol);
            edownloader.Download();
            System.Console.WriteLine("Update data for symbol: " + symbol);
            edownloader.Update();

            System.Console.WriteLine("Analysing financial statemant for symbol: " + symbol);

            FinancialStatementService statementService = new FinancialStatementService();
            var statements = statementService.FinancialsFor(symbol);

            System.Console.WriteLine("Analysing financial metrics for symbol: " + symbol);

            FinancialMetricService metricService = new FinancialMetricService();
            metricService.CalculateAndStoreMetrics(statements);

            System.Console.WriteLine("Analysing financial model for symbol: " + symbol);

            FinancialModelService modelSerivce = new FinancialModelService();
            modelSerivce.UpdateGrahamAnalysis(symbol);
            modelSerivce.UpdateDcfAnalysis(symbol);

            System.Console.WriteLine("Done");

            if (Debugger.IsAttached)
            {
                Console.WriteLine("... Press any key to continue ...");
                Console.ReadLine();
            }
        }
    }
}
