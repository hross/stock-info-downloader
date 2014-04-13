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
            
            EdgarDownloader edownloader = new EdgarDownloader(@"C:\stocks\edgar\filings", "INTC");
            edownloader.Download();
            edownloader.Update();

            FinancialStatementService statementService = new FinancialStatementService();
            var statements = statementService.FinancialsFor("INTC");

            FinancialMetricService metricService = new FinancialMetricService();
            metricService.CalculateAndStoreMetrics(statements);

            FinancialModelService modelSerivce = new FinancialModelService();
            modelSerivce.UpdateGrahamAnalysis("INTC");
            modelSerivce.UpdateDcfAnalysis("INTC");

            CompanyDownloader companyDownloader = new CompanyDownloader(@"C:\stocks\", "http://www.nasdaq.com/screening/companies-by-name.aspx?letter=");
            companyDownloader.Download();
        }
    }
}
