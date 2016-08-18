using IM.Host.Properties;
using IM.Model;
using PalaceResorts.Common.Notifications.WinForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IM.Host.Classes
{
  public class clsEmail
  {
    #region SendMail

    /// <summary>
    /// Envia el correo electronico de huespedes que se presentaron sin su invitacion impresa
    /// </summary>
    /// <param name="guest">Guest</param>
    /// <param name="salesRoom">SalesRoom</param>
    /// <param name="leadSource">LeadSource</param>
    /// <param name="personnel">Personal PRInvit1</param>
    /// <param name="bookingDepositTable">Datatable de Deposit</param>
    /// <returns>mensaje si ocurre error</returns>
    /// <history>
    /// [aalcocer] 18/08/2016 Created
    /// </history>
    internal static string SendMail(Guest guest, SalesRoomShort salesRoom, LeadSource leadSource, PersonnelShort personnel, DataTable bookingDepositTable)
    {
      Assembly assembly = Assembly.GetExecutingAssembly();

      string strtemplate;
      using (Stream stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Template.emailShowTemplate.html"))
      using (StreamReader reader = new StreamReader(stream))
      {
        strtemplate = reader.ReadToEnd();
      }

      var values = Regex.Matches(strtemplate, @"(\[.*?\])+").Cast<Match>().Select(m => m.Value).ToList();

      //Obtenemos los tipos de objeto
      var objects = new List<object> { guest, salesRoom, leadSource, personnel };

      //Remplazamos
      objects.ForEach(obj =>
      {
        Type type = obj.GetType();//Obtenemos el tipo de objeto

        foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => !pi.GetMethod.IsVirtual).Where(pi => values.Contains($"[{pi.Name}]")))
        {
          var value = type.GetProperty(pi.Name).GetValue(obj, null) ?? string.Empty;
          var strValue = value.ToString();
          if (value is DateTime)
            strValue = ((DateTime)value).ToString("dd/MM/yyyy");

          strtemplate = strtemplate.Replace($"[{pi.Name}]", strValue);
        }
      });

      strtemplate = strtemplate.Replace("[BookingDepositTable]", BookingDepositDataTableToHtml(bookingDepositTable));
      try
      {
        Notifier.SendMail(Settings.Default.Subject, strtemplate, Settings.Default.ToEmail.Cast<string>().ToArray(),
          string.Empty, string.Empty);
        return string.Empty;
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    #endregion SendMail

    #region BookingDepositDataTableToHtml

    /// <summary>
    /// Convierte el datatable de deposit a una tabla Html
    /// </summary>
    /// <param name="dt">Datatable de Deposit</param>
    /// <history>
    /// [aalcocer] 18/08/2016 Created
    /// </history>
    private static string BookingDepositDataTableToHtml(DataTable dt)
    {
      string html = "<table class='data' style=\"border: 1px\">";
      // encabezado
      html += "<tr>";
      for (int i = 0; i < dt.Columns.Count; i++)
        html += "<th>" + dt.Columns[i].ColumnName + "</th>";
      html += "</tr>";
      //valores
      html += "<tbody>";
      for (int i = 0; i < dt.Rows.Count; i++)
      {
        html += "<tr>";
        for (int j = 0; j < dt.Columns.Count; j++)
        {
          switch (j)
          {
            case 0:
              html += $"<td style = \"text-align: right\"><b>{dt.Rows[i][j]}</b></td>";
              break;

            case 1:
            case 2:
              html += $"<td style = \"text-align: right\">{dt.Rows[i][j]}</td>";
              break;

            default:
              html += $"<td>{dt.Rows[i][j]}</td>";
              break;
          }
        }
        html += "</tr>";
      }
      html += "</tbody>";
      html += "</table>";
      return html;
    }

    #endregion BookingDepositDataTableToHtml
  }
}