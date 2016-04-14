using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using IM.Base.Helpers;
using System.Windows;



namespace IM.Transfer.Clases
{
    public class Log
    {
        public class Transaction
        {
            public  DateTime Date { get; set; }
            public  string LogLevel { get; set; }
            public  string Message { get; set; }

        }

        public static string GetPath () {

            //string path = AppDomain.CurrentDomain.BaseDirectory.ToString();
            //string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            string path = AppContext.BaseDirectory.ToString();
            
            return path;
            

        }
        #region Metodos
        public static string ExistDirectory(string logName, DateTime date)
        {
            string path = GetPath();
            string namePathFolderLogYear = "Log"+logName+"(" + date.ToString("yyyy") + ")";
            string namePathFolderLogMonth = "Log"+logName+"(" + GetMonthName(date.Month) + ")";//String.Format("{0:y}", DateTime.Now)
            string namePathFileLog = "Log"+logName+ date.ToString("dd") + ".xml";//"yyyyMMdd
            string pathFolder = Path.Combine(path, "Log");
            string pathFolderYear = Path.Combine(pathFolder, namePathFolderLogYear);
            string pathFolderMonth = Path.Combine(pathFolderYear, namePathFolderLogMonth);
            string fileLogDay = Path.Combine(pathFolderMonth, namePathFileLog);
            Task.Factory.StartNew(() =>
            {
                if (!ExistFolder(pathFolder))
                {
                    CreateFolder(pathFolder);
                }
                if (!ExistFolder(pathFolderYear))
                {
                    CreateFolder(pathFolderYear);
                }
                if (!ExistFolder(pathFolderMonth))
                {
                    CreateFolder(pathFolderMonth);
                }
                if (!ExistFile(fileLogDay))
                {
                    //CreateFile(fileLogDay);
                    CreateXmlTransactions(fileLogDay);
                }
            });
            return fileLogDay;
        }

        public static bool ExistFolder(string path)
        {
            string pathFolder = path;
            bool status = false;
            try
            {
                // valida que exista la carpeta, segun la ruta que pasemos
                if ((Directory.Exists(pathFolder)))
                {
                    status = true;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;

        }

        public static bool CreateFolder(string path)
        {
            string pathFolder = @path;
            bool status = false;
            try
            {
                // crear la carpeta con el nombre especificado en la ruta
                Directory.CreateDirectory(pathFolder);
                status = true;  
            }
            catch (Exception)
            {
                status = false;
            }
            return status;

        }

        public static bool ExistFile(string path)
        {
            string pathFile = @path;
            bool notificationStatus = false;
            try
            {
                //valida que exista un archivo especificado en la ruta
                if ((File.Exists(pathFile)))
                {
                    notificationStatus = true;
                }

            }
            catch (Exception)
            {
                notificationStatus = false;
            }
            return notificationStatus;
        }

        public static bool CreateFile(string path)
        {
            string pathFile = @path;
            bool status = false;
            try
            {
                //si no existe el archivo de log se crea la creamos 
                if (!(File.Exists(pathFile)))
                {
                    File.Create(pathFile);
                    status = true;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        private static string GetMonthName(int monthNumber)
        {
            try
            {
                DateTimeFormatInfo dateFormat = CultureInfo.CurrentCulture.DateTimeFormat;
                string MounthName = dateFormat.GetMonthName(monthNumber);
                return MounthName;
            }
            catch
            {
                return "Desconocido";
            }
        }

        public static bool CreateXmlTransactions(string path)
        {
            bool status = false;
            try
            {
               
                XmlTextWriter writeXML = new XmlTextWriter(@path, System.Text.Encoding.UTF8);

                writeXML.Formatting = Formatting.Indented;
                writeXML.Indentation = 2;
                writeXML.WriteStartDocument(false);
                writeXML.WriteComment("Transactions List");

                writeXML.WriteStartElement("Transactions");
                writeXML.WriteEndElement();
                writeXML.WriteEndDocument();
                writeXML.Close();
                status = true;
            }
            catch (Exception eror)
            {
                UIHelper.ShowMessage(eror.Message.ToString(), MessageBoxImage.Error);
                status = false;
            }

            return status;
        }

        public static Transaction AddTransaction(string nameLog, DateTime date, string logLevel, string message)
        {
            Transaction newItemTransaction = new Transaction();

            XmlDocument XmlDoc;
            XmlNode Raiz;
            XmlNode ident;
            bool rta = false;

            try
            {
                XmlDoc = new XmlDocument();
                string _pathFileXML = ExistDirectory(nameLog, date);
                XmlDoc.Load(@_pathFileXML);
                Raiz = XmlDoc.DocumentElement;
                ident = Raiz;

                XmlElement newTransaction = XmlDoc.CreateElement("Transaction"); //Como vamos a llamar el nuevo nodo  
                newTransaction.InnerXml = "<Date></Date><LogLevel></LogLevel><Message></Message>"; // Este es el contenido que va a tener el nuevo nodo  

                newTransaction.AppendChild(XmlDoc.CreateWhitespace("\r\n"));
                newTransaction["Date"].InnerText = date.ToString();
                newTransaction["LogLevel"].InnerText = logLevel;
                newTransaction["Message"].InnerText = message;


                ident.InsertAfter(newTransaction, ident.LastChild);
                XmlTextWriter writeTransactions = new XmlTextWriter(@_pathFileXML, System.Text.Encoding.UTF8);
                XmlDoc.WriteTo(writeTransactions);
                writeTransactions.Close();
                
                newItemTransaction.Date = date;
                newItemTransaction.LogLevel = logLevel;
                newItemTransaction.Message = message;
                
                rta = true;
            }
            catch (Exception)
            {
                newItemTransaction = null;
            }
            return newItemTransaction;
        }

        public static DateTime dateBefore(int days, DateTime date) {
            DateTime dateBefore = DateTime.Today.AddDays(days * (-1));
            return dateBefore;
        }

        public static List<Transaction> readXML(string pathFile)
        {
            string path = pathFile.Replace(@"\", @"\\");


            XDocument xdoc = XDocument.Load(@path);
            List<Transaction> listTransactions =
                (from transaction in xdoc.Descendants("Transaction")
                 select new Transaction
                 {
                     Date = Convert.ToDateTime(transaction.Element("Date").Value),
                     LogLevel = Convert.ToString(transaction.Element("LogLevel").Value).Trim(),
                     Message = Convert.ToString(transaction.Element("Message").Value).Trim()
                 }
                ).ToList();
            return listTransactions;
        }

        public static List<Transaction> LoadHistoryLog(string logName, DateTime dateFrom, DateTime dateTo)
        {
            List<Transaction> listTransactions = new List<Transaction>();
            string path = GetPath();
            
            while (dateTo <= dateFrom)
            {
                string pathFile = @path + "\\" + "Log" + "\\" + "Log"+ logName + "(" + dateTo.ToString("yyyy") + ")" + "\\" + "Log" + logName + "(" + GetMonthName(dateTo.Month) + ")" + "\\" + "Log" + logName + "" + dateTo.ToString("dd") + ".xml";
                if (ExistFile(pathFile))
                {
                    
                    List<Transaction> listTransaction = readXML(pathFile);

                    listTransactions = listTransactions.Concat(listTransaction).ToList();
                }

                dateTo = dateTo.AddDays(1);
            }
            return listTransactions;
        }

        #endregion



    }
}
