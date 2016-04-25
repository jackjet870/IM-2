using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGiftsReceipts
  {

    #region GetGiftsReceipts
    /// <summary>
    /// Obtienen los Gifts Receipts Short
    /// </summary>
    /// <param name="guestID"> Clave del huesped </param>
    /// <param name="salesRoom"> Clave de la sala de ventas </param>
    /// <param name="receipt"> Clave del recibo </param>
    /// <param name="folio"> Folio de Palace Resorts </param>
    /// <param name="dateFrom"> Fecha desde </param>
    /// <param name="dateTo"> Fecha hasta </param>
    /// <param name="name"> Nombre </param>
    /// <param name="reservation"> Folio de reservacion </param>
    /// <returns>Lista de tipo GiftsReceptsShort</returns>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    public static List<GiftsReceiptsShort> GetGiftsReceipts(int? guestID = 0, string salesRoom = "ALL", int receipt = 0, string folio = "ALL",
                                                            DateTime? dateFrom = null, DateTime? dateTo = null, string name = "ALL",
                                                            string reservation = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGiftsReceipts(guestID, salesRoom, receipt, folio, dateFrom, dateTo, name, reservation).ToList();
      }
    }
    #endregion

    #region GetGiftReceipt
    /// <summary>
    /// Obtiene la información de un Gifts Receipt especifico.
    /// </summary>
    /// <param name="grSelected"></param>
    /// <returns> Gifts Receipt </returns>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    public static List<GiftsReceipt> GetGiftReceipt(int grSelected)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.GiftsReceipts.Where(x => x.grID == grSelected).ToList();
      }
    }
    #endregion

    #region GetGiftsReceiptsDetail
    /// <summary>
    /// Consulta los regalos de un recibo
    /// </summary>
    /// <param name="receipt"> Clave del recibo de regalos </param>
    /// <param name="package"> Indica si se desean los paquetes de regalos </param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 06/04/2016
    /// </history>
    public static List<GiftsReceiptDetailShort> GetGiftsReceiptsDetail(int receipt, bool package = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGiftsReceiptDetail(receipt, package).ToList();
      }
    }
    #endregion

    #region ValidateGiftsReceipt
    /// <summary>
    /// Valida que los datos de un recibo de regalos existan
    /// </summary>
    /// <param name="changedBy">Clave del usuario que esta haciendo el cambio</param>
    /// <param name="password">Contraseña del usuario que esta haciendo el cambio</param>
    /// <param name="guest">Clave del huesped</param>
    /// <param name="location">Clave de la locacion</param>
    /// <param name="salesroom">Clave de la sala de ventas</param>
    /// <param name="giftshost">Clave del Host de regalos</param>
    /// <param name="personnel">Clave del personal que ofrecio los regalos</param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    public static ValidationData ValidateGiftsReceipt(string changedBy, string password, int guest, string location, string salesroom, string giftshost, string personnel)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_ValidateGiftsReceipt(changedBy, password, guest, location, salesroom, giftshost, personnel).SingleOrDefault();
      }
    } 
    #endregion

  }
}
