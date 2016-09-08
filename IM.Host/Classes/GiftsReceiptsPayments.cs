using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IM.Host.Classes
{
  public class GiftsReceiptsPayments
  {

    #region Validate
    /// <summary>
    /// Valida los pagos
    /// </summary>
    /// <param name="Grid"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 15/Junio/2016 Created
    /// </history>
    public static bool Validate(DataGrid Grid)
    {
      // validamos los datos del grid
      if (!GridHelper.Validate(Grid, false, 1, "Payments", "Payment", new List<string>() { "gycu", "gypt", "gysb" }))
      {
        return false;
      }
      // validamos los pagos segun su origen
      else if (!ValidateBySource(Grid))
      {
        return false;
      }
      return true;
    } 
    #endregion

    #region ValidateBySource
    /// <summary>
    /// Valida los pagos segun su origen
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 15/Mayo/2016
    /// </history>
    private static bool ValidateBySource(DataGrid Grid)
    {
      foreach (var Current in Grid.Items)
      {
        Type type = Current.GetType();
        var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();

        if (properties.Count > 0)
        {
          // Si se ingreso la cantidad pagada
          if ((decimal)type.GetProperty("gyAmount").GetValue(Current, null) > 0)
          {
            switch ((string)type.GetProperty("gysb").GetValue(Current, null))
            {
              case "DEP":
                // Verificamos que el pago tenga una forma de pago valida
                if ((string)type.GetProperty("gypt").GetValue(Current, null) != "CC" && (string)type.GetProperty("gypt").GetValue(Current, null) != "CS" && (string)type.GetProperty("gypt").GetValue(Current, null) != "TC")
                {
                  UIHelper.ShowMessage("Payment type invalid for source ", MessageBoxImage.Information);
                  return false;
                }
                // validamos la moneda
                // solo se permite tarjeta de credito en dolares americanos o pesos mexicanos
                else if ((string)type.GetProperty("gypt").GetValue(Current, null) == "CC" )
                {
                  if (((string)type.GetProperty("gycu").GetValue(Current, null) != "US" && (string)type.GetProperty("gycu").GetValue(Current, null) != "MEX"))
                  {
                    UIHelper.ShowMessage("Currency invalid for source ", MessageBoxImage.Information);
                    return false;
                  }
                }
                break;
              case "CXC":
              case "MK":
                if ((string)type.GetProperty("gypt").GetValue(Current, null) != "CS")
                {
                  UIHelper.ShowMessage("Payment type invalid for source ", MessageBoxImage.Information);
                  return false;
                }
                break;
            }

            // Validamos si es pago con CC y se anexe el banco
            if ((string)type.GetProperty("gypt").GetValue(Current, null) == "CC" && (string)type.GetProperty("gybk").GetValue(Current, null) == "")
            {
              UIHelper.ShowMessage("For payments with credit card select a bank option.", MessageBoxImage.Information);
              return false;
            }
          }
        }
      }

      return true;
    }
    #endregion

    #region SavePayments
    /// <summary>
    /// Realiza las validaciones de los campos introducidos en el grid de Payments y los guarda en la BD
    /// </summary>
    /// <param name="giftReceiptID">Clave del recibo de regalo</param>
    /// <param name="dtg">Datagrid de Payments</param>
    /// <param name="isNew">TRUE -> Es un nuevo GiftsReceipt o nuevo ExchangeRate | FALSE -> No es nuevo</param>
    /// <param name="_lstPaymentsDelete">Lista que contiene el registro de los Payments eliminados.</param>
    /// <history>
    /// [vipacheco] 27/Abril/2016 Created
    /// </history>
    public static async Task SavePayments(int giftReceiptID, DataGrid dtg, bool isNew, List<GiftsReceiptPaymentShort> _lstPaymentsDelete)
    {
      foreach (var item in dtg.Items)
      {
        GiftsReceiptPaymentShort currentItem;

        if (item is GiftsReceiptPaymentShort)
        {
          currentItem = item as GiftsReceiptPaymentShort;

          // Construimos la entidad tipo GiftsReceiptsPayments
          GiftsReceiptPayment giftsReceiptPayments = new GiftsReceiptPayment
          {
            gypt = currentItem.gypt,
            gycu = currentItem.gycu,
            gyAmount = currentItem.gyAmount,
            gyRefund = currentItem.gyRefund,
            gysb = string.IsNullOrEmpty(currentItem.gysb) ? null : currentItem.gysb,
            gype = currentItem.gype,
            gybk = currentItem.gybk,
          };
          if (currentItem.gygr == 0)
            giftsReceiptPayments.gygr = giftReceiptID;

          if (isNew) // Si es de nueva creacion se agregan todos.
          {
            await BREntities.OperationEntity(giftsReceiptPayments, Model.Enums.EnumMode.Add);
          }
          else // Si se estan editando
          {
            // Verificamos si el Gift se encuentra en la BD.
            GiftsReceiptPayment _giftPayment = BRGiftsReceiptsPayments.GetGiftReceiptPayment(currentItem.gygr, currentItem.gyID);

            if (_giftPayment != null) // Si existe este registro se verifica si algun campo se edito
            {
              if (_giftPayment.gyAmount != currentItem.gyAmount || _giftPayment.gypt != currentItem.gypt ||
                  _giftPayment.gybk != currentItem.gybk || _giftPayment.gycu != currentItem.gycu || _giftPayment.gype != currentItem.gype ||
                  _giftPayment.gyRefund != currentItem.gyRefund || _giftPayment.gysb != currentItem.gysb)
              {
                giftsReceiptPayments.gyID = currentItem.gyID;
                giftsReceiptPayments.gygr = currentItem.gygr;
                await BREntities.OperationEntity(giftsReceiptPayments, Model.Enums.EnumMode.Edit);
              }
            }
            else // registro nuevo
            {
              await BREntities.OperationEntity(giftsReceiptPayments, Model.Enums.EnumMode.Add);
            }

            // Se verifica si se elimino alguno de la lista original
            if (_lstPaymentsDelete.Count > 0)
            {
              await BREntities.OperationEntities(_lstPaymentsDelete, Model.Enums.EnumMode.Delete);
            }
          }
        }
      }
    }
    #endregion

  }
}
