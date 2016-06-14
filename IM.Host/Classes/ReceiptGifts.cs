using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IM.Host.Classes
{
  /// <summary>
  /// Clase encargada de procesar funciones en común con varias ventanas
  /// </summary>
  /// <history>
  /// [vipacheco] 14/Abril/2016 Created
  /// [vipacheco] 16/Mayo/2016 Modified --> Migrado a esta clase generica de Gifts Receipt
  /// </history>
  public class ReceiptGifts
  {

    #region CalculateCharge
    /// <summary>
    /// Calcula el cargo de regalos segun el tipo de calculo
    /// </summary>
    /// <history>
    /// [vipacheco] 14/Mayo/2016 Created
    /// </history>
    public static void CalculateCharge(int GuestID, ChargeTo ChargeTo, TextBox txtTotalCost, CheckBox chkgrExchange,
                                       TextBox txtgrgu, ref TextBox txtgrCxCGifts, ref TextBox txtTotalCxC, ref TextBox txtgrCxCAdj,
                                       ref bool ValidateMaxAuthGifts, ref TextBox txtgrls, ref TextBox txtgrMaxAuthGifts,
                                       ref TextBlock lblgrMaxAuthGifts)
    {
      decimal curCharge = 0;
      decimal curTotalCost = 0;
      decimal curMaxAuthGifts = 0;
      bool blnTour = false;

      //ChargeTo _ChargeTo = (ChargeTo)ChargeTo.SelectedItem;

      curTotalCost = txtTotalCost.Text != "" ? Convert.ToDecimal(txtTotalCost.Text.Trim(new char[] { '$' })) : 0;

      //Establecemos el monto maximo de regalos
      SetMaxAuthGifts(GuestID, ChargeTo, ref ValidateMaxAuthGifts, ref txtgrls, ref txtgrMaxAuthGifts, ref lblgrMaxAuthGifts);

      curMaxAuthGifts = txtgrMaxAuthGifts.Text != "" ? Convert.ToDecimal(txtgrMaxAuthGifts.Text.Trim(new char[] { '$' })) : 0;

      // Si no es un intercambio de regalos
      if (!chkgrExchange.IsChecked.Value)
      {
        // Localizamos a quien se carga
        switch (ChargeTo.ctCalcType)
        {
          // si el huesped tiene tour el cargo es por el total de regalos menos el monto maximo
          // autorizado. De lo contrario el cargo es por el total de regalos
          case "A":
            // Validamos si tiene tour
            Guest _guest = BRGuests.GetGuest(Convert.ToInt32(txtgrgu.Text));
            blnTour = _guest.guTour;
            if (blnTour)
              curCharge = CalculateChargeBasedOnMaxAuthGifts(curTotalCost, curMaxAuthGifts);
            else
              curCharge = curTotalCost;
            break;
          // El cargo es por el costo total de regalos
          case "B":
            curCharge = curTotalCost;
            break;
          // El cargo es por el costo total de regalos menos el monto maximo autorizado
          case "C":
            curCharge = CalculateChargeBasedOnMaxAuthGifts(curTotalCost, curMaxAuthGifts);
            break;
          // No generan cargo
          case "Z":
            curCharge = 0;
            break;
          default:
            break;
        }
      }
      txtgrCxCGifts.Text = string.Format("{0:C2}", curCharge);

      // Calculamos el total del cargo
      txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text != "" ? txtgrCxCGifts.Text.Trim(new char[] { '$' }) : "0") + Convert.ToDecimal(txtgrCxCAdj.Text != "" ? txtgrCxCAdj.Text.Trim(new char[] { '$' }) : "0"));
    }
    #endregion

    #region SetMaxAuthGifts
    /// <summary>
    /// Establece el monto maximo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 14/Mayo/2016 Created
    /// [vipacheco] 16/Mayo/2016 Modified --> Migrado a esta clase general de ReceiptGifts
    /// </history>
    public static void SetMaxAuthGifts(int GuestID, ChargeTo ChargeTo, ref bool ValidateMaxAuthGifts, ref TextBox txtgrls, ref TextBox txtgrMaxAuthGifts,
                                 ref TextBlock lblgrMaxAuthGifts)
    {
      decimal curMaxAuthGifts;
      bool blnWithMaxAuthGifts = false;

      curMaxAuthGifts = CalculateMaxAuthGifts(GuestID, txtgrls.Text, ChargeTo, ref blnWithMaxAuthGifts);
      txtgrMaxAuthGifts.Text = string.Format("${0}", curMaxAuthGifts);
      lblgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      txtgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      ValidateMaxAuthGifts = blnWithMaxAuthGifts;
    }
    #endregion

    #region CalculateMaxAuthGifts
    /// <summary>
    /// Calcula el monto maximo de regalos
    /// </summary>
    /// <param name="chargeTo"></param>
    /// <param name="leadSource"></param>
    /// <param name="withMaxAuthGifts"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// [vipacheco] 16/Mayo/2016 Modified --> Migrado a esta clase general de ReceiptGifts
    /// </history>
    public static decimal CalculateMaxAuthGifts(int GuestID, string leadSource, ChargeTo ChargeTo, ref bool withMaxAuthGifts)
    {
      decimal curMaxAuthGifts = 0;
      withMaxAuthGifts = true;

      switch (ChargeTo.ctCalcType)
      {
        //Monto maximo de regalos por Lead Source
        case "A":
          // Si tiene Lead Source
          if (leadSource != "")
          {
            LeadSource _leadSource = BRLeadSources.GetLeadSourceByID(leadSource);
            //si encontro el Lead Source
            if (_leadSource != null)
            {
              curMaxAuthGifts = _leadSource.lsMaxAuthGifts;
            }
          }
          break;
        // Monto maximo de regalos por Guest Status
        case "C":
          GuestStatus _guestsStatus = BRGuestStatus.GetGuestsStatus(GuestID);
          GuestStatusType _guestStatusType = BRGuestStatusTypes.GetGuestStatusTypeByID(_guestsStatus.gtgs);
          curMaxAuthGifts = _guestsStatus.gtQuantity * _guestStatusType.gsMaxAuthGifts;
          break;
        //  Sin monto maximo de regalos
        default:
          curMaxAuthGifts = 0;
          withMaxAuthGifts = false;
          break;
      }
      return curMaxAuthGifts;
    }
    #endregion

    #region CalculateChargeBasedOnMaxAuthGifts
    /// <summary>
    /// Calcula el cargo de recibo de regalos basado en el monto maximo de regalos
    /// </summary>
    /// <param name="curTotalCost"></param>
    /// <param name="curMaxAuthGifts"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 14/Abril/2016 Created
    /// [vipacheco] 16/Mayo/2016 Modified --> Migrado a esta clase general
    /// </history>
    private static decimal CalculateChargeBasedOnMaxAuthGifts(decimal curTotalCost, decimal curMaxAuthGifts)
    {
      // Si el costo total de regalos es mayor al monto maximo autorizado
      if (curTotalCost > curMaxAuthGifts)
        return curTotalCost - curMaxAuthGifts;
      // Si el monto de regalos esta dentro del monto maximo autorizado
      else
        return 0; // Por la naturaleza del calculo el cargo es siempre 0 si el total de regalos no es mayor al autorizado
    }
    #endregion

    #region CalculateTotalCharge
    /// <summary>
    /// Calcula el total del cargo
    /// </summary>
    /// <param name="txtTotalCxC"></param>
    /// <param name="txtgrCxCGifts"></param>
    /// <history>
    /// [vipacheco] 16/Mayo/2016 Created
    /// </history>
    public static void CalculateTotalCharge(ref TextBox txtTotalCxC, ref TextBox txtgrCxCGifts, ref TextBox txtgrCxCAdj)
    {
      txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text != "" ? txtgrCxCGifts.Text : "0") + Convert.ToDecimal(txtgrCxCAdj.Text != "" ? txtgrCxCAdj.Text : "0"));
    }
    #endregion

    #region CalculateTotalGifts
    /// <summary>
    /// Calcula el total de los Gifts
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="GiftsType"></param>
    /// <param name="txtTotalCost"></param>
    /// <param name="txtTotalPrice"></param>
    /// <param name="txtTotalToPay"></param>
    /// <param name="OnlyCancellled"></param>
    /// <param name="CancelField"></param>
    /// <history>
    /// [vipacheco] 14/Mayo/2016 Created
    /// </history>
    public static void CalculateTotalGifts(DataGrid Grid, EnumGiftsType GiftsType, ref TextBox txtTotalCost, ref TextBox txtTotalPrice, ref TextBox txtTotalToPay,
                                    bool OnlyCancellled = false, string CancelField = "")
    {
      Gifts.CalculateTotalGifts(Grid, GiftsType, ref txtTotalCost, ref txtTotalPrice, ref txtTotalToPay, OnlyCancellled, CancelField);
    } 
    #endregion

  }
}
