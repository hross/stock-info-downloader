using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using StockInfoCommons.CompanyListings;
using StockInfoCommons.Edgar;
using StockInfoCommons.Financials;
using StockInfoCommons.GoogleFinance;
using StockInfoCommons.Metrics;
using StockInfoCommons.Simulation;
using System.Threading;

namespace WindowsFormsApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BTN_search_Click(object sender, EventArgs e)
        {

            this.txt_input_search.Enabled = false;
            this.BTN_search.Enabled = false;

            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(Program)).Location);

            EdgarDownloader edownloader = new EdgarDownloader(basePath + "\\stocks\\edgar\\filings", this.txt_input_search.Text);
            edownloader.Download();
            edownloader.Update();

            FinancialStatementService statementService = new FinancialStatementService();
            var statements = statementService.FinancialsFor(this.txt_input_search.Text);

            FinancialMetricService metricService = new FinancialMetricService();
            metricService.CalculateAndStoreMetrics(statements);

            FinancialModelService modelSerivce = new FinancialModelService();
            modelSerivce.UpdateGrahamAnalysis(this.txt_input_search.Text);
            modelSerivce.UpdateDcfAnalysis(this.txt_input_search.Text);

            this.txt_input_search.Enabled = true;
            this.BTN_search.Enabled = true;
        }
        

    }
}
