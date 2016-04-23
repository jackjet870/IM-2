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

    #region SaveType
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo RoomTypes
    /// </summary>
    /// <param name="roomType">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <returns>0. No se guardó | 1. se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    public static int SaveRoomType(RoomType roomType,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)
        {
          dbContext.Entry(roomType).State = System.Data.Entity.EntityState.Modified;
        } 
        #endregion
        #region Add
        else
        {
          RoomType roomTypeVal = dbContext.RoomTypes.Where(rt => rt.rtID == roomType.rtID).FirstOrDefault();
          if(roomTypeVal!=null)//Verificamos que cumpla con los filtros actuales
          {
            return -1;
          }
          else//Agregar
          {
            dbContext.RoomTypes.Add(roomType);
          }
        } 
        #endregion
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
