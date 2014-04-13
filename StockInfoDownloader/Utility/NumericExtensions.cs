
namespace StockInfoDownloader
{
    public static class NumericExtensions
    {
        public static double Normalize(this double value)
        {
            if (double.IsInfinity(value) || double.IsNaN(value))
                return 0;

            return value;
        }
    }
}
