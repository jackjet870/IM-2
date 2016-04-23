using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System;

namespace IM.BusinessRules.BR
{
  public class BRWarehouses
  {
    #region GetWarehousesByUser

    /// <summary>
    /// Obtiene los almacenes de un usuario
    /// </summary>
    /// <param name="user">Usuario </param>
    /// <param name="regions">Region o default('ALL') </param>
    /// <history>
    /// [wtorres]  07/Mar/2016 Created
    /// </history>
    public static List<WarehouseByUser> GetWarehousesByUser(string user, string regions)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetWarehousesByUser(user, regions).ToList();
      }
    }

    #endregion

    #region GetWareHouses
    /// <summary>
    /// Obtiene registros del Catalogo WareHouses
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="wareHouse">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo WareHouse</returns>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// </history>
    public static List<Warehouse> GetWareHouses(int nStatus = -1, Warehouse wareHouse = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from wh in dbContext.Warehouses
                    select wh;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(wh => wh.whA == blnStatus);
        }

        if (wareHouse != null)//Validamos que se tenga el objeto
        {
          if (!string.IsNullOrWhiteSpace(wareHouse.whID))//filtro por ID
          {
            query = query.Where(wh => wh.whID == wareHouse.whID);
          }

          if (!string.IsNullOrWhiteSpace(wareHouse.whN))//Filtro por descripción
          {
            query = query.Where(wh => wh.whN.Contains(wareHouse.whN));
          }

          if (!string.IsNullOrWhiteSpace(wareHouse.whar))//Filtro por Area
          {
            query = query.Where(wh => wh.whar == wareHouse.whar);
          }
        }

        return query.OrderBy(wh => wh.whN).ToList();
      }
    } 
    #endregion
  }
}
