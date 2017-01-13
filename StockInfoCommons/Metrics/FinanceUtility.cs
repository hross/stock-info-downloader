using System;

namespace StockInfoCommons.Metrics
{
    public class FinanceUtility
    {
        public static double FutureValue(double presentValue, double rate, double period)
        {
            return presentValue * Math.Pow((1 + rate), period);
        }

        public static double PresentValue(double futureValue, double rate, double period)
        {
            return futureValue / Math.Pow(1 + rate, period);
        }

        public static double CurrentReturnOnInvestmentCapital(double freeCashFlow, double shareHolderEquity, double totalLiabilities, double currentLiabilities)
        {
            return freeCashFlow / (shareHolderEquity + totalLiabilities - currentLiabilities);
        }

        public static double FreeCashFlow(double NetCashFlowFromOperations, double CapEx)
        {
            return NetCashFlowFromOperations + CapEx;
        }

        public static double ReceivablesPercentOfSales(double totalRevenue, double netReceivables)
        {
            return netReceivables / totalRevenue;
        }

        public static double CurrentRatio(double currentAssets, double currentLiabilities)
        {
            return currentAssets / currentLiabilities;
        }

        public static double PlantPropertyValue(double ppe, double depreciation)
        {
            return ppe + depreciation;
        }

        public static double GoodwillPercentAssets(double goodwill, double totalAssets)
        {
            return goodwill / totalAssets;
        }

        public static double IntangiblePercentAssets(double intangibles, double totalAssets)
        {
            return intangibles / totalAssets;
        }

        public static double ReturnOnAssets(double netIncome, double totalAssets)
        {
            return netIncome / totalAssets;
        }

        public static double YearsToPayDebt(double longTermDebt, double netIncome)
        {
            return longTermDebt / netIncome;
        }

        public static double PerpetualValue(double fcfFinal, double terminalGrowthRate, double discountRate)
        {
            return (fcfFinal * (1 + terminalGrowthRate)) / (discountRate - terminalGrowthRate);
        }
    }
}
