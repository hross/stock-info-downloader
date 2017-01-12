using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StockInfoDownloader.Edgar
{
    public class FactContext
    {
        public string Ticker { get; set; }

        public string Source { get; set; }

        public string Name { get; set; }

        public string Identifier { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public TimeSpan Period
        {
            get
            {
                return EndDate - StartDate;
            }
        }

        /// <summary>
        /// a far from perfect way of ensuring we delete/clean up docs the same way.
        /// </summary>
        public string HashKey
        {
            get
            {
                return
                    this.StartDate.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    this.EndDate.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    this.Source +
                    this.Ticker;
            }
        }

        public bool IsInstant { get { return this.StartDate == this.EndDate; } }

        public FactContext(string ticker, string source) {
            this.StartDate = DateTime.MinValue;
            this.EndDate = DateTime.MinValue;
            this.Identifier = string.Empty;
            this.Name = string.Empty;
            this.Ticker = ticker;
            this.Source = source;
        }

        public FactContext(XmlNode contextNode, string ticker, string source) : this (ticker, source)
        {
            this.Name = contextNode.Attributes["id"].Value.Trim();

            foreach (XmlNode contextChild in contextNode.ChildNodes)
            {
                // find time period section
                if (contextChild.Name.ToLower() == "xbrli:period")
                {
                    foreach (XmlNode timeChild in contextChild)
                    {
                        // find the right part of time period
                        if (timeChild.Name == "xbrli:instant")
                        {
                            DateTime dt;
                            DateTime.TryParse(timeChild.InnerText, out dt);
                            this.StartDate = dt;
                            this.EndDate = dt;
                            break;
                        }
                        else if (timeChild.Name == "xbrli:endDate")
                        {
                            DateTime dt;
                            DateTime.TryParse(timeChild.InnerText, out dt);
                            this.EndDate = dt;
                        }
                        else if (timeChild.Name == "xbrli:startDate")
                        {
                            DateTime dt;
                            DateTime.TryParse(timeChild.InnerText, out dt);
                            this.StartDate = dt;
                        }
                    }
                }
                else if (contextChild.Name.ToLower() == "xbrli:entity")
                {
                    foreach (XmlNode entityChild in contextChild)
                    {
                        if (entityChild.Name == "xbrli:identifier")
                        {
                            this.Identifier = entityChild.InnerText.Trim();
                            break;
                        }
                    }
                }
            }
        }
    }
}
