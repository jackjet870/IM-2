using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using IM.Base.Helpers;


namespace IM.Base.Helpers
{
  public class LogHelper
  {
    #region Class Transaction
    ///<summary>Clase para creacion de archivos de log</summary>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public class Transaction
    {
      public DateTime Date { get; set; }
      public string LogLevel { get; set; }
      public string Message { get; set; }

    }
    #endregion

    #region GetPath
    ///<summary>Obtiene el path de la aplicasión.</summary>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static string GetPath(string strLogName, DateTime? dtmDate = null)
    {
      //string path = AppDomain.CurrentDomain.BaseDirectory.ToString();
      //string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
      if (dtmDate == null) dtmDate = DateTime.Now;
      string path = AppContext.BaseDirectory.ToString();
      string pathFile = Path.Combine(Path.Combine(Path.Combine(path, "Log"), $"Log{strLogName}({dtmDate.Value.ToString("yyyy")})"), $"Log{strLogName}({DateHelper.GetMonthName(dtmDate.Value.Month)})");
      return pathFile;
    }
    #endregion

    #region ExistDirectory
    ///<summary>Validacion y creacion del registro del log</summary>
    ///<param name="logName">Nombre del log</param>
    ///<param name="date">Fecha del log</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static string ExistDirectory(string strLogName, DateTime dtmDate)
    {
      string pathFolder = GetPath(strLogName, dtmDate);
      string fileLogDay = Path.Combine(pathFolder, $"Log{strLogName}{dtmDate.ToString("dd")}.xml");
        if (!ExistFolder(pathFolder))//valida en la carpeta del año exista la carpeta del mes pasado en la fecha.
        {
          CreateFolder(pathFolder);//si no existe la carpeta del mes se crea
        }
        if (!ExistFile(fileLogDay))// valida la existencia del archivo de log del día.
        {
          //CreateFile(fileLogDay);
          CreateXmlTransactions(fileLogDay);//si no existe se crea el archivo.
        }
      
      return fileLogDay;
    }
    #endregion

    #region ExistFolder
    ///<summary>Metodo que valida la existencia de una carpeta en un directorio</summary>
    ///<param name="path">Se pasa la direccion con el nombre de la carpeta</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
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
    #endregion

    #region CreateFolder
    ///<summary>Metodo para crear una carpeta</summary>
    ///<param name="path">Se pasa el path junto con el nombre de la carpeta a crear</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
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
    #endregion

    #region ExistFile
    ///<summary>Metodo que valida la existencia de un archivo</summary>
    ///<param name="path">Se pasa el path junto el nombre del archivo a crear</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static bool ExistFile(string path)
    {
      string pathFile = path;
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
    #endregion

    #region CreateFile
    ///<summary>Metodo que crea un archivo en una ruta</summary>
    ///<param name="path">Path con el nombre del archivo</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static bool CreateFile(string path)
    {
      string pathFile = path;
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
    #endregion

    #region CreateXmlTransactions
    ///<summary>Metodo que crea archivo xml para log</summary>
    ///<param name="path">Path con nombre del archivo de log</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static bool CreateXmlTransactions(string path)
    {
      bool status = false;
      try
      {
        XmlTextWriter writeXML = new XmlTextWriter(path, System.Text.Encoding.UTF8);
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
      catch (Exception)
      {
        status = false;
      }

      return status;
    }
    #endregion

    #region AddTransaction
    ///<summary>Metodo que agrega registro al xml</summary>
    ///<param name="date">fecha de la transacción</param>
    ///<param name="logLevel">Tipo de log(Error, Succes, Info, Warning, Start, Stop, End)</param>
    ///<param name="message">Mensaje de la transacción</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static Transaction AddTransaction(string nameLog, DateTime date, string logLevel, string message)
    {
      Transaction newItemTransaction = new Transaction();
      XmlDocument XmlDoc;
      XmlNode Raiz;
      XmlNode ident;
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

      }
      catch (Exception)
      {
        newItemTransaction = null;
      }
      return newItemTransaction;
    }
    #endregion

    #region ReadXML
    ///<summary>Metodo para leear un archivo log</summary>
    ///<param name="pathFile">Ruta con el nombre del archivo</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static List<Transaction> ReadXML(string pathFile)
    {
      string path = pathFile;//.Replace(@"\", @"\\");
      XDocument xdoc = XDocument.Load(path);
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
    #endregion

    #region LoadHistoryLog
    ///<summary>Metodo para obtener registros de log</summary>
    ///<param name="logName">Nombre del log</param>
    ///<param name="dateFrom">Apartir de que fecha se obtiene el registro</param>
    ///<param name="dateTo">Fecha final donde se obtendran los registros</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public async static Task<List<Transaction>> LoadHistoryLog(string strLogName, DateTime dateFrom, DateTime dateTo)
    {
      List<Transaction> listTransactions = new List<Transaction>();
      await Task.Run(() => { 
        while (dateTo <= dateFrom)
        {
        
          string pathFile = Path.Combine(GetPath(strLogName, dateTo), $"Log{strLogName}{dateTo.ToString("dd")}.xml");
          if (ExistFile(pathFile))
          {
            List<Transaction> listTransaction = ReadXML(pathFile);
            if (listTransaction != null || listTransaction.Count > 0)
              listTransactions = listTransactions.Concat(listTransaction).ToList();
          }

          dateTo = dateTo.AddDays(1);
        }
      });
      return listTransactions;
    }
    #endregion


  }
}
