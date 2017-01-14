using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ServiceStack.DataAnnotations;

namespace StockInfoCommons.Simulation
{
    public class GrahamAnalysis
    {
        public const double ZeroGrowthPe = 4.4;

        #region Inputs

        /// <summary>
        /// http://help.stockopedia.co.uk/knowledgebase/articles/54477-what-are-normalised-earnings-
        /// </summary>
        public double NormalizedEps { get; set; }

        /// <summary>
        /// Estimated growth rate (% expressed as decimal). E.g. 0.08 for 8%
        /// </summary>
        public double GrowthRate { get; set; }

        /// <summary>
        /// Treasury or Corp Bond Rate.
        /// </summary>
        public double RiskFreeRate { get; set; }
        
        #endregion

        #region Outputs

        public double Price { get { return NormalizedEps * (8.5 + 2 * GrowthRate * 100) * 4.4 / RiskFreeRate; } }

        #endregion

        #region Descriptions
        
        [AutoIncrement]
        public int Id { get; set; }

        public string Ticker { get; set; }

        public string Description { get; set; }

        public string Source { get; set; }

        public bool IsQuarterly { get; set; }

        #endregion

        public GrahamAnalysis()
        {
        }

        /// <summary>
        /// todo: remove this method...
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ticker"></param>
        /// <param name="timestamp"></param>
        /// <param name="quarterly"></param>
        /// <param name="quarter"></param>
        /// <param name="description"></param>
        public void Save(SqlConnection connection, string ticker, DateTime timestamp, bool quarterly, int quarter, string description)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO");
            sql.AppendLine("Graham(");
            sql.AppendLine("Ticker,Description,Timestamp,IsQuarterly,Quarter,");
            sql.AppendLine("NormEps,GrowthRate,RiskFreeRate,Price");
            sql.AppendLine(")VALUES(");
            sql.AppendLine("@ticker,@description,@timestamp,@isquarterly,@quarter,");
            sql.AppendLine("@normeps,@growthrate,@riskfreerate,@price");
            sql.AppendLine(")");

            // add it to the database
            SqlCommand insertCmd = new SqlCommand(sql.ToString(), connection);
            insertCmd.Connection = connection;

            insertCmd.Parameters.Add(new SqlParameter { ParameterName = "ticker", Value = ticker, SqlDbType = SqlDbType.NVarChar });
            insertCmd.Parameters.Add(new SqlParameter { ParameterName = "description", Value = description, SqlDbType = SqlDbType.NVarChar });
            insertCmd.Parameters.Add(new SqlParameter { ParameterName = "timestamp", Value = timestamp, SqlDbType = SqlDbType.DateTime });
            insertCmd.Parameters.Add(new SqlParameter { ParameterName = "isquarterly", Value = quarterly, SqlDbType = SqlDbType.Bit });
            insertCmd.Parameters.Add(new SqlParameter { ParameterName = "quarter", Value = quarter, SqlDbType = SqlDbType.Int });

            insertCmd.Parameters.Add(new SqlParameter { ParameterName = "normeps", Value = this.NormalizedEps, SqlDbType = SqlDbType.Float });
            insertCmd.Parameters.Add(new SqlParameter { ParameterName = "growthrate", Value = this.GrowthRate, SqlDbType = SqlDbType.Float });
            insertCmd.Parameters.Add(new SqlParameter { ParameterName = "riskfreerate", Value = this.RiskFreeRate, SqlDbType = SqlDbType.Float });
            insertCmd.Parameters.Add(new SqlParameter { ParameterName = "price", Value = this.Price, SqlDbType = SqlDbType.Float });

            insertCmd.ExecuteNonQuery();
        }
    }
}
