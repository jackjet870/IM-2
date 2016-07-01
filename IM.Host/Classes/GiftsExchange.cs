using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IM.Host.Classes
{
  /// <summary>
  /// Clase para el manejo de productos externos de intercambio
  /// </summary>
  /// <history>
  /// [vipacheco] 18/Mayo/2016 Created
  /// </history>
  public class GiftsExchange
  {

    #region Validate
    /// <summary>
    /// Valida los regalos de intercambio de un recibo de regalos
    /// </summary>
    /// <returns></returns>
    public static bool Validate(DataGrid Grid)
    {
      return GridHelper.Validate(Grid, false, 1, "Gifts", "Gift", new List<string> { "gegi" });
    } 
    #endregion

    #region GetItems
    /// <summary>
    /// Obtiene los elementos
    /// </summary>
    /// <param name="Grid"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static List<string> GetItems(DataGrid Grid)
    {
      return GridHelper.GetItems(Grid, "gegi");
    } 
    #endregion

    public async static void Save(int ReceiptID, DataGrid Grid)
    {
      bool MustSave = false;
      // preparamos los regalos de paquetes de regalos del recibo de regalos para guardarse
      GiftsReceiptsPacks.PrepareToSave(ReceiptID, ref MustSave);

      // guardamos los regalos
      foreach (GiftsReceiptDetail Current in Grid.Items)
      {
        // Asignamos el ID generado
        Current.gegr = ReceiptID;
        await BREntities.OperationEntity(Current, Model.Enums.EnumMode.add);
      }
    }

  }
}
