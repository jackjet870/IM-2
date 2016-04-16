using System;
using System.Globalization;
using System.Threading;

using System.Resources;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Base.Language;
using System.Collections.Generic;


namespace IM.Base.Helpers
{
  public class LanguageHelper
  {
    
    public static string IDLanguage
    {
      get
      {
        return Thread.CurrentThread.CurrentUICulture.ToString();
      }
      set
      {        
        switch (value)
        {         
          case "EN"://Ingles
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("EN-US");
            break;
          case "ES"://Español
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ES-ES");
            break;
          case "PO"://Portugues
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("PT-BR");
            break;
        }
      }
    }

    public static string GetMessage(EnumMessage msg)
    {      
      return ResLanguage.ResourceManager.GetString(msg.ToString());
    }

    public static List<string> cultures()
    {
      // Obtiene los nombres de la culturas
      List<string> list = new List<string>();
      list.Add("CULTURE   SPEC.CULTURE  ENGLISH NAME");
      list.Add("--------------------------------------------------------------");
      foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
      {
        string specName = "(none)";
        try { specName = CultureInfo.CreateSpecificCulture(ci.Name).Name; } catch { }
        list.Add(String.Format("{0,-12}{1,-12}{2}", ci.Name, specName, ci.EnglishName));
      }
      return list;
    }
  }
}
