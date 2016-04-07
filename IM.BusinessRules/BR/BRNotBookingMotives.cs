using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRNotBookingMotives
  {
    #region GetNotBookingMotives
    /// <summary>
    /// Obtiene registros del catalogo NotBoikingMotives
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1.Activos</param>
    /// <param name="notBookingMotive">Objeto con filtros adicionales</param>
    /// <returns>lista de tipo NotBookingMotive</returns>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    public static List<NotBookingMotive> GetNotBookingMotives(int nStatus = -1, NotBookingMotive notBookingMotive = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from nb in dbContext.NotBookingMotives
                    select nb;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(nb => nb.nbA == blnStatus);
        }

        #region Filtros adicionales
        if (notBookingMotive != null)
        {
          if (notBookingMotive.nbID > 0)//Filtro por ID
          {
            query = query.Where(nb => nb.nbID == notBookingMotive.nbID);
          }

          if (!string.IsNullOrEmpty(notBookingMotive.nbN))//Filtro por Descripción
          {
            query = query.Where(nb => nb.nbN.Contains(notBookingMotive.nbN));
          }
        }
        #endregion

        return query.OrderBy(nb => nb.nbN).ToList();
      }
    }
    #endregion

    #region SaveNotBookingMotive
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo NotBookingMotive 
    /// </summary>
    /// <param name="notBookingMotive">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0. No se guardó | 1. Se guardó </returns>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    public static int SaveNotBookingMotive(NotBookingMotive notBookingMotive,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {

        #region Update
        if (blnUpdate)//SI es actualizar
        {
          dbContext.Entry(notBookingMotive).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Insert
        else//Insertar
        {
          dbContext.NotBookingMotives.Add(notBookingMotive);
        } 
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
