using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml;
using ServiceStack.OrmLite;
using StockInfoDownloader.Financials;

namespace StockInfoDownloader.Edgar
{
    public class EdgarParser
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private EdgarFiling _filing;
        private Dictionary<string, FinancialStatement> _statementCache;

        public EdgarParser(EdgarFiling filing)
        {
            _filing = filing;
            _statementCache = new Dictionary<string, FinancialStatement>();
        }

        public void Parse()
        {
            _statementCache.Clear();

            // parse the document
            XmlDocument doc = new XmlDocument();
            doc.Load(_filing.PathOnDisk);

            // cache the contexts for this document
            Dictionary<string, FactContext> contexts = BuildContextHash(doc.GetElementsByTagName("xbrli:context"), _filing.Ticker, StatementBase.SourceEdgar);

            if (contexts.Count > 0)
            {
                var keys = BalanceSheetElements.Keys.Concat(CashFlowElements.Keys).Concat(IncomeStatementElements.Keys).ToList();

                foreach (string key in keys)
                {
                    // find all elements matching the tag (from all time periods)
                    XmlNodeList nodeList = doc.GetElementsByTagName(key);
                    foreach (XmlNode node in nodeList)
                    {
                        // figure out the context for each element of that name
                        string contextId = node.Attributes["contextRef"].InnerText;
                        string val = node.InnerText;

                        // add it to the correct statement for that context period
                        AddToStatement(contexts[contextId], key, val);
                    }
                }
            }

            SaveStatements();
        }

        private void SaveStatements()
        {
            var factory = FinancialStatement.FinancialStatementFactory();

            using (IDbConnection db = factory.OpenDbConnection())
            {
                db.CreateTableIfNotExists<IncomeStatement>();
                db.CreateTableIfNotExists<BalanceSheet>();
                db.CreateTableIfNotExists<CashFlow>();

                foreach (string key in _statementCache.Keys)
                {
                    db.Delete<IncomeStatement>(statement => statement.HashKey == _statementCache[key].HashKey);
                    db.Delete<BalanceSheet>(statement => statement.HashKey == _statementCache[key].HashKey);
                    db.Delete<CashFlow>(statement => statement.HashKey == _statementCache[key].HashKey);
                }

                foreach (string key in _statementCache.Keys)
                {
                    db.Insert<IncomeStatement>(_statementCache[key].IncomeStatement);
                    db.Insert<BalanceSheet>(_statementCache[key].BalanceSheet);
                    db.Insert<CashFlow>(_statementCache[key].CashFlow);
                }
            }
        }

        private void AddToStatement(FactContext context, string elementName, string value)
        {
            double result = 0;
            if (!double.TryParse(value, out result))
                return; // NaN

            if (null == context)
                return; // no time period

            // look up financial statement, if there is one
            FinancialStatement statement;
            if (_statementCache.ContainsKey(context.HashKey))
                statement = _statementCache[context.HashKey];
            else
                statement = new FinancialStatement {
                    StartDate = context.StartDate,
                    EndDate = context.EndDate,
                    Ticker = this._filing.Ticker,
                    Source = StatementBase.SourceEdgar
                };
            
            // try to set the value on the statement
           if (SetStatementValue(statement, elementName, result))
               _statementCache[context.HashKey] = statement;
        }

        #region Specific Statement Property Caches

        private static Dictionary<string, PropertyInfo> _balanceSheetElements = null;

        private static Dictionary<string, PropertyInfo> BalanceSheetElements
        {
            get
            {
                if (null == _balanceSheetElements)
                {
                    _balanceSheetElements = EdgarElementsForType(typeof(BalanceSheet));
                }
                return _balanceSheetElements;
            }
        }

        private static Dictionary<string, PropertyInfo> _cashFlowElements = null;

        private static Dictionary<string, PropertyInfo> CashFlowElements
        {
            get
            {
                if (null == _cashFlowElements)
                {
                    _cashFlowElements = EdgarElementsForType(typeof(CashFlow));
                }
                return _cashFlowElements;
            }
        }

        private static Dictionary<string, PropertyInfo> _incomeStatementElements = null;

        private static Dictionary<string, PropertyInfo> IncomeStatementElements
        {
            get
            {
                if (null == _incomeStatementElements)
                {
                    _incomeStatementElements = EdgarElementsForType(typeof(IncomeStatement));
                }
                return _incomeStatementElements;
            }
        }

        #endregion

        #region Set statement value reflection helpers

        private static Dictionary<string, PropertyInfo> EdgarElementsForType(Type type)
        {
            var edgarElements = new Dictionary<string, PropertyInfo>();
            try
            {
                // add cash flow
                foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    // if there is a custom attribute associated
                    EdgarFilingAttribute attr = (EdgarFilingAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(EdgarFilingAttribute));

                    // and if it matches the element name we set the value
                    if (null != attr && !edgarElements.ContainsKey(attr.Name))
                        edgarElements.Add(attr.Name, propertyInfo);
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to set edgar value cache from attributes.", ex);
            }

            return edgarElements;
        }

        private static bool SetStatementValue(FinancialStatement statement, string elementName, double value)
        {
            try
            {
                // set the property value if it exists in the cache
                if (BalanceSheetElements.ContainsKey(elementName))
                {
                    BalanceSheetElements[elementName].SetValue(statement.BalanceSheet, value);
                    return true;
                }
                else if (CashFlowElements.ContainsKey(elementName))
                {
                    CashFlowElements[elementName].SetValue(statement.CashFlow, value);
                    return true;
                }
                else if (IncomeStatementElements.ContainsKey(elementName))
                {
                    IncomeStatementElements[elementName].SetValue(statement.IncomeStatement, value);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to set edgar value from attribute.", ex);
                return false;
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Build a hash of all document contexts so we can easily refer to them. This is a kind of XBRL hack (rather than relying on XSLT, etc).
        /// </summary>
        /// <param name="contexts"></param>
        /// <returns></returns>
        private static Dictionary<string, FactContext> BuildContextHash(XmlNodeList contexts, string ticker, string source)
        {
            var hash = new Dictionary<string, FactContext>();

            foreach (XmlNode node in contexts)
            {
                FactContext fc = new FactContext(node, ticker, source);
                hash.Add(fc.Name, fc);
            }

            // we went through every context
            return hash;
        }

    }
}
