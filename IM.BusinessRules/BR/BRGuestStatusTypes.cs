using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRGuestStatusTypes
  {

    #region GetGuestStatusTypes
    /// <summary>
    /// Obtiene registros del catalogo GuestStatus
    /// </summary>
    /// <param name="guestStatusType">Objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1. Registros activos</param>
    /// <returns>Lista de tipo GuestStatusType</returns>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    public static List<GuestStatusType> GetGuestStatusTypes(GuestStatusType guestStatusType = null, int nStatus = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from gs in dbContext.GuestsStatusTypes
                    select gs;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(gs => gs.gsA == blnEstatus);
        }

        if (guestStatusType != null)//verificar si se tiene un objeto
        {
          if (!string.IsNullOrWhiteSpace(guestStatusType.gsID))//filtro por ID
          {
            query = query.Where(gs => gs.gsID == guestStatusType.gsID);
          }

          if (!string.IsNullOrWhiteSpace(guestStatusType.gsN))//Filtro por descripcion
          {
            query = query.Where(gs => gs.gsN.Contains(guestStatusType.gsN));
          }
        }

        return query.OrderBy(gs => gs.gsN).ToList();
      }
    }
    #endregion

    #region SaveGuestStatusType
    /// <summary>
    /// Guarda|actualiza un registro en el catalogo GuestStatus
    /// </summary>
    /// <param name="guestStatusType">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | false. Inserta</param>
    /// <returns>0. registro no guardado | 1. Registro guardado | 2. existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    public static int SaveGuestStatusType(GuestStatusType guestStatusType,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)//Actualizar
        {
          dbContext.Entry(guestStatusType).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region insert
        else//insertar
        {
          GuestStatusType guestStatusTypeVal = dbContext.GuestsStatusTypes.Where(gs => gs.gsID == guestStatusType.gsID).FirstOrDefault();

          if(guestStatusTypeVal!=null)//Se valida si existe un registro con el mismo ID
          {
            return 2;//Existe un registro con el mismo ID
          }
          else//crear el registro nuevo
          {
            dbContext.GuestsStatusTypes.Add(guestStatusType);
          }
        }
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion

    #region GuestStatustypeId
    /// <summary>
    /// Obtiene el tipo de  estatus del invitado por su ID
    /// </summary>
    /// <param name="gstId">Identificador del tipo de estado del invitado</param>
    /// <returns>GuestStatusType</returns>
    /// <history>
    /// [lchairez] 24/03/2016 Created.
    /// </history>
    public static GuestStatusType GetGuestStatusTypeId(string gstId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.GuestsStatusTypes.Where(g => g.gsID == gstId).SingleOrDefault();
      }
    }
    #endregion

    #region GetGuestStatus
    /// <summary>
    /// Obtiene los estatus del invitado
    /// </summary>
    /// <param name="guest">Invitado a consultar</param>
    /// <history>
    /// [lchairez] 04/04/2016 Created.
    /// </history>
    public static List<GuestStatus> GetGuestStatus(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.GuestsStatus.Where(g => g.gtgu == guestId).ToList();
      }
    }
    #endregion
  }
}
