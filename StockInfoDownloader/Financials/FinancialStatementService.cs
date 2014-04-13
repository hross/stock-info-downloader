using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using ServiceStack.OrmLite;

namespace StockInfoDownloader.Financials
{
    public class FinancialStatementService
    {
        private OrmLiteConnectionFactory _factory;

        public FinancialStatementService()
        {
            _factory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["StockScreenerConnection"].ConnectionString, SqlServerDialect.Provider);
        }

        public List<FinancialStatement> FinancialsFor(string ticker, DateTime since = default(DateTime))
        {
            if (since == default(DateTime))
                since = SqlDateTime.MinValue.Value;

            var financials = new List<FinancialStatement>();

            using (IDbConnection db = _factory.OpenDbConnection())
            {
                var incomeStatements = db.Select<IncomeStatement>(income =>
                    income.StartDate > since && income.Ticker == ticker
                );

                var balanceSheets = db.Select<BalanceSheet>(bs =>
                    bs.StartDate > since && bs.Ticker == ticker
                );

                var cashFlows = db.Select<CashFlow>(cf =>
                    cf.StartDate > since && cf.Ticker == ticker
                );

                foreach (var bs in balanceSheets)
                {
                    var incomeStatement = incomeStatements.FirstOrDefault(income => income.HashKey == bs.HashKey);
                    var cashFlow = cashFlows.FirstOrDefault(cf => cf.HashKey == bs.HashKey);

                    if (null != cashFlow && null != incomeStatement)
                    {
                        financials.Add(new FinancialStatement { BalanceSheet = bs, IncomeStatement = incomeStatement, CashFlow = cashFlow });
                    }
                }
            }

            return financials;
        }
    }
}
