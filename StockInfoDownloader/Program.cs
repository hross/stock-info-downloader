using System.IO;
using StockInfoDownloader.CompanyListings;
using StockInfoDownloader.Edgar;
using StockInfoDownloader.Financials;
using StockInfoDownloader.GoogleFinance;
using StockInfoDownloader.Metrics;
using StockInfoDownloader.Simulation;

namespace StockInfoDownloader
{
    class Program
    {

        static void Main(string[] args)
        {
            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(Program)).Location);

            string symbol = "AAPL";

            //todo link to the programm folder
            //EdgarDownloader edownloader = new EdgarDownloader(@"C:\stocks\edgar\filings", symbol);
            EdgarDownloader edownloader = new EdgarDownloader(basePath+ "\\stocks\\edgar\\filings", symbol);
            edownloader.Download();
            edownloader.Update();

            FinancialStatementService statementService = new FinancialStatementService();
            var statements = statementService.FinancialsFor(symbol);

            FinancialMetricService metricService = new FinancialMetricService();
            metricService.CalculateAndStoreMetrics(statements);

            FinancialModelService modelSerivce = new FinancialModelService();
            modelSerivce.UpdateGrahamAnalysis(symbol);
            modelSerivce.UpdateDcfAnalysis(symbol);
        }
    }
}
