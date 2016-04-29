using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRRoomTypes
  {
    #region GetRoomTypes
    /// <summary>
    /// Obtiene registros del catalogo RoomTypes
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="roomType">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo RoomType</returns>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    public static List<RoomType> GetRoomTypes(int nStatus = -1, RoomType roomType = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from rt in dbContext.RoomTypes
                    select rt;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(rt => rt.rtA == blnStatus);
        }

        if (roomType != null)
        {
          if (!string.IsNullOrWhiteSpace(roomType.rtID))//FIltro por ID
          {
            query = query.Where(rt => rt.rtID == roomType.rtID);
          }

          if (!string.IsNullOrWhiteSpace(roomType.rtN))//Filtro por estatus
          {
            query = query.Where(rt => rt.rtN.Contains(roomType.rtN));
          }
        }

        return query.OrderBy(rt => rt.rtN).ToList();
      }
    }
    #endregion
  }
}
