using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace StockInfoCommons.Edgar
{
    /// <summary>
    /// Data from the SEC's RSS stream for each filing item.
    /// </summary>
    [XmlRoot("content")]
    public class EdgarFiling
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        [AutoIncrement]
        public int Id { get; set; }

        [XmlElement("accession-nunber")]
        public string AcessionNumber { get; set; }

        [XmlElement("act")]
        public string Act { get; set; }

        [XmlElement("file-number")]
        public string FileNumber { get; set; }

        [XmlElement("file-number-href")]
        public string FileNumberHref { get; set; }

        [XmlElement("filing-date")]
        public string FilingDate { get; set; }

        [XmlElement("filing-href")]
        public string FilingHref { get; set; }

        //todo we do not need this?
        //public string FilingDirectory
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(this.FilingHref)) return string.Empty;

        //        int pos = FilingHref.LastIndexOf("/");

        //        if (pos <= 0) return string.Empty;

        //        return FilingHref.Substring(0, pos);
        //    }
        //}

        [XmlElement("filing-type")]
        public string FilingType { get; set; }

        [XmlElement("film-number")]
        public string FilmNumber { get; set; }

        [XmlElement("form-name")]
        public string FormName { get; set; }

        [XmlElement("size")]
        public string Size { get; set; }

        [XmlElement("xbrl_href")]
        public string XbrlHref { get; set; }

        public string FileName { get; set; }

        public string PathOnDisk { get; set; }

        public string Ticker { get; set; }

        [Ignore]
        //public string FilingUrl { get { return this.FilingDirectory + "/" + this.FileName; } }
        public string FilingUrl { get; set; }

        public EdgarFiling()
        {
        }

        /// <summary>
        /// Create an object from SEC stream filing data.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static EdgarFiling Deserialize(SyndicationContent content)
        {
            return Deserialize(SerializeItem(content));
        }

        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns></returns>
        public static OrmLiteConnectionFactory EdgarDownloadFactory()
        {
            return new OrmLiteConnectionFactory(
                 ConfigurationManager.ConnectionStrings["StockScreenerConnection"].ConnectionString,
                SqlServerDialect.Provider);
        }

        #region Serialization Helpers

        private static EdgarFiling Deserialize(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            XmlNodeList nodeList = document.GetElementsByTagName("acession-number");

            EdgarFiling efi = null;
            try
            {


                using (var reader = XmlReader.Create(new StringReader(xml)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EdgarFiling));
                    efi = (EdgarFiling)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to deserialze content.", ex);
            }

            return efi;
        }


        private static string SerializeItem(SyndicationContent content)
        {
            var output = new StringBuilder();

            using (NoNamespaceXmlWriter writer = new NoNamespaceXmlWriter(new StringWriter(output)))
            {
                content.WriteTo(writer, "content", "");
            }

            return output.ToString();
        }

        public class NoNamespaceXmlWriter : XmlTextWriter
        {
            public NoNamespaceXmlWriter(System.IO.TextWriter output)
                : base(output) { Formatting = System.Xml.Formatting.Indented; }

            public override void WriteStartDocument() { }

            public override void WriteStartElement(string prefix, string localName, string ns)
            {
                base.WriteStartElement("", localName, "");
            }
        }

        #endregion
    }
}
