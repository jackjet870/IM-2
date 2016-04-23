using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRReps
  {
    #region GetReps
    /// <summary>
    /// Devuelve la lista de Reps
    /// </summary>
    /// <param name="rep">Entidad con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1.Registros Activos</param>
    /// <returns>Lista de reps</returns>
    /// <history>
    /// [Emoguel] created 11/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto 
    /// </history>
    public static List<Rep> GetReps(Rep rep=null,int nStatus=-1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from r in dbContext.Reps select r;

        if(nStatus!=-1)//Filtro por Estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(r=>r.rpA==blnStatus);
        }

        if (rep != null)//Valida si se tiene un objeto
        {
          if (!string.IsNullOrWhiteSpace(rep.rpID))//Filtro por ID
          {
            query = query.Where(r => r.rpID == rep.rpID);
          }
        }

        return query.OrderBy(r=>r.rpID).ToList();
      }
    }
    #endregion

    #region SaveRep
    /// <summary>
    /// Actualiza|Guarda un registro en el catalogo Reps
    /// </summary>
    /// <param name="rep">Objeto a guardar</param>
    /// <param name="blnUpdate">Truw. Actualiza |False. Inserta</param>
    /// <returns>0. No se guuardó  | 1. Si se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// </history>
    public static int SaveReps(Rep rep,bool blnUpdate)
    {      
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)
        {
          dbContext.Entry(rep).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Save
        else
        {
          Rep repVal = dbContext.Reps.Where(rp => rp.rpID == rep.rpID).FirstOrDefault();
          if(repVal!=null)//Validamos si existe un registro con el mismo ID
          {
            return -1;
          }
          else//Agregamos
          {
            dbContext.Reps.Add(rep);
          }
        }
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
