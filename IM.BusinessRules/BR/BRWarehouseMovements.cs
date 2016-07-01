using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRWarehouseMovements
  {
    #region GetWarehouseMovements

    /// <summary>
    /// Método para obtener una lista de movimientos del almacén en una fecha específica.
    /// </summary>
    /// <param name="wmwh">ID del almacen.</param>
    /// <param name="wmD">Fecha del movimiento.</param>
    /// <returns> List<GetWhsMovs> </returns>
    /// <history>
    /// [edgrodriguez] 19/Feb/2016 Created
    /// </history>
    public static List<WarehouseMovementShort> GetWarehouseMovements(string wmwh, DateTime wmD)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetWhsMovs(wmwh, wmD).ToList();
      }
    }

    #endregion

    #region SaveWarehouseMovements

    /// <summary>
    /// Método para guardar nuevos movimientos en un almacén.
    /// </summary>
    /// <param name="lstWhsMovs">Lista de movimientos para agregar.</param>
    /// <history>
    /// [edgrodriguez] 22/Feb/2016 Created
    /// </history>
    public static void SaveWarehouseMovements(ref List<WarehouseMovement> lstWhsMovs)
    {
      IEnumerable<WarehouseMovement> lstResult;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        lstResult = dbContext.WarehouseMovements.AddRange(lstWhsMovs);
        dbContext.SaveChanges();
      }
      lstWhsMovs = lstResult.ToList();
    }

    #endregion
  }
}
