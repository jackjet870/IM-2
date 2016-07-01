using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// 
  /// </summary>
  /// <history>
  /// [vipacheco] 01/Junio/2016 Created
  /// </history>
  public class BRRoomCharges
  {

    #region UpdateRoomChargeConsecutive
    /// <summary>
    /// Actualiza el consecutivo de cargos a habitacion de una reservacion
    /// </summary>
    /// <param name="Hotel"></param>
    /// <param name="Folio"></param>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static void UpdateRoomChargeConsecutive(string Hotel, string Folio)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbContext.USP_OR_UpdateRoomChargesConsecutive(Hotel, Folio);
      }
    } 
    #endregion

  }
}
