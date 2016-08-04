using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace IM.Host.Classes
{
  /// <summary>
  /// clase con metodos en comun
  /// </summary>
  /// <history>
  /// [vipacheco] 26/Mayo/2016 Created
  /// </history>
  public class Common
  {

    #region IsExternalProduct
    /// <summary>
    /// Determina si un regalo es manejado por un sistema externo
    /// </summary>
    /// <param name="ExternalProduct"></param>
    /// <param name="_Gift"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static bool IsExternalProduct(EnumExternalProduct ExternalProduct, string _Gift, ref Gift _GiftResult)
    {
      string FieldName = "";
      bool blnIs = false;


      // si es el monedero electronico
      if (ExternalProduct == EnumExternalProduct.expElectronicPurse)
        FieldName = "giProductGiftsCard";
      // si son las promociones de PVP
      else
        FieldName = "giPVPPromotion";

      // localizamos el regalo
      _GiftResult = frmHost._lstGifts.Where(x => x.giID == _Gift).First();
      Type type = _GiftResult.GetType();

      blnIs = type.GetProperty(FieldName).GetValue(_GiftResult, null) == null ? false : true;


      return blnIs;
    }
    #endregion

    #region ValidateCloseDate
    /// <summary>
    /// Valida que una fecha de una entidad no este en una fecha cerrada con 7 dias
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="pDate"></param>
    /// <param name="pDateClose"></param>
    /// <param name="pDescription"></param>
    /// <param name="pCondition"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 08/Julio/2016 Created
    /// </history>
    public static bool ValidateCloseDate(EnumEntities pEntity, ref DateTimePicker pDate, DateTime pDateClose, string pDescription = "", bool pCondition = true)
    {
      int diffDay = 0;
      string items = "", description = "";

      // Dias transcurridos
      diffDay = (pDateClose - pDate.Value.Value.Date).Days;

      // si la condicion es valida y el editor esta habilitado
      if (pCondition && pDate.IsEnabled)
      {
        items = EnumToListHelper.GetEnumDescription(pEntity);

        // si no se envio una descripcion
        if (pDescription == "")
        {
          switch (pEntity)
          {
            case EnumEntities.Shows:
              description = "Show";
              break;
            case EnumEntities.MealTickets:
              description = "Meal Ticket";
              break;
            case EnumEntities.Sales:
              description = "Sale";
              break;
            case EnumEntities.GiftsReceipts:
              description = "Gift Receipt";
              break;
          }
        }
        else
          description = pDescription;

        // validamos que la fecha no sea mayor a la fecha actual
        if (pDate.Value.Value.Date > BRHelpers.GetServerDate())
        {
          UIHelper.ShowMessage(description + " can not be after today.", System.Windows.MessageBoxImage.Exclamation, "Intelligence Marketing");
          pDate.Focus();
          return false;
        }
        // validamos que la fecha no sea menor o igual a la fecha de cierre y no sea mayor o igual a 7 dias y si viene de Show
        else if (pEntity == EnumEntities.Shows)
        {
          pDate.Focus();
          return ValidateCloseDate7Days(pDate.Value.Value.Date, pDateClose.Date, diffDay);
        }
        // validamos que la fecha no sea menor o igual a la fecha de cierre
        else if (pDate.Value.Value.Date <= pDateClose.Date)
        {
          UIHelper.ShowMessage("It's not allowed to make " + items + " for a closed date.", System.Windows.MessageBoxImage.Exclamation);
          pDate.Focus();
          return false;
        }
      }
      return true;
    }
    #endregion

    #region ValidateCloseDate7Days
    /// <summary>
    /// Valida que la fecha cerrada que no pase de 7 dias
    /// </summary>
    /// <param name="pDate"></param>
    /// <param name="pClose"></param>
    /// <param name="pDiff"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 11/Julio/2016 Created
    /// </history>
    public static bool ValidateCloseDate7Days(DateTime pDate, DateTime pClose, int pDiff)
    {
      if (pDate <= pClose && pDiff > 7)
      {
        UIHelper.ShowMessage("Inside a closed date, is more than 7 days. \r\n (Last closing date: " + pClose + ")");
        return false;
      }
      else if (pDate <= pClose && pDiff < 7)
      {
        UIHelper.ShowMessage("Inside a closed date only shows 7 days ago. \r\n (Last closing date: )" + pClose + ")");
        return false;
      }

      return true;
    }
    #endregion

    #region IsClosed
    /// <summary>
    /// Evalua si el Mealticket no se ha cerrado!
    /// </summary>
    /// <param name="pdtmDate"></param>
    /// <param name="pdtmClose"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 23/03/2016 Created
    /// [vipacheco] 29/Julio/2016 Modified --> Migrado a esta clase comun
    /// </history>
    public static bool IsClosed(DateTime pdtmDate, DateTime pdtmClose)
    {
      bool blnClosed = false;
      DateTime _pdtmDate;

      if (DateTime.TryParse(pdtmDate + "", out _pdtmDate))
      {
        if (_pdtmDate <= pdtmClose)
        {
          blnClosed = true;
        }
      }
      return blnClosed;
    }
    #endregion

    #region GetFullName
    /// <summary>
    /// Obtiene el nombre completo de un huesped
    /// </summary>
    /// <param name="LastName"></param>
    /// <param name="FirstName"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 13/Mayo/2016 Created
    /// </history>
    public static string GetFullName(string LastName, string FirstName)
    {
      string FullName = "";

      // si tiene apellido
      if (!string.IsNullOrEmpty(LastName) && LastName != "")
      {
        FullName = LastName;

        // si tiene nombre
        if (!string.IsNullOrEmpty(FirstName) && FirstName != "")
        {
          FullName = FullName + " " + FirstName;
        }
      }
      return FullName;
    }
    #endregion

  }
}
