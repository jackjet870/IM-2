using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRReasonCancellationFolios
  {
    #region GetReasonCancellationFolios
    /// <summary>
    /// Obtiene registros del Catalogo ReasonCancellationFolios
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos  | 1.Activos</param>
    /// <param name="reasonCancellationFolio">objeto con filtros especiales</param>
    /// <returns>Lista de tipo ReasonCancellationFolio</returns>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    public static List<ReasonCancellationFolio> GetReasonCancellationFolios(int nStatus = -1, ReasonCancellationFolio reasonCancellationFolio = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from rcf in dbContext.ReasonsCancellationFolios
                    select rcf;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(rcf => rcf.rcfA==blnStatus);
        }

        if (reasonCancellationFolio != null)//Verificamos que se tenga un objeto
        {
          if (!string.IsNullOrWhiteSpace(reasonCancellationFolio.rcfID))//Filtro por ID
          {
            query = query.Where(rcf => rcf.rcfID == reasonCancellationFolio.rcfID);
          }

          if (!string.IsNullOrWhiteSpace(reasonCancellationFolio.rcfN))//Filtro po descripción
          {
            query = query.Where(rcf => rcf.rcfN.Contains(reasonCancellationFolio.rcfN));
          }
        }
        return query.OrderBy(rcf => rcf.rcfN).ToList();
      }
    }

    #endregion

    #region SaveReasonCancellationFolio
    /// <summary>
    /// Agrega|Actualiza registros en la BD
    /// </summary>
    /// <param name="reasonCancellationFolio">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0. No se guardó | 1. Se guardó correctamenre  | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    public static int SaveReasonCancellationFolio(ReasonCancellationFolio reasonCancellationFolio,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)
        {
          dbContext.Entry(reasonCancellationFolio).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Add
        else
        {
          ReasonCancellationFolio resCabFolVal = dbContext.ReasonsCancellationFolios.Where(rcf => rcf.rcfID == reasonCancellationFolio.rcfID).FirstOrDefault();
          if(resCabFolVal!=null)//Validamos que no exista un registro con el mismo ID
          {
            return -1;
          }
          else//Agregar
          {
            dbContext.ReasonsCancellationFolios.Add(reasonCancellationFolio);
          }
        } 
        #endregion
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
