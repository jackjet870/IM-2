using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRReimpresionMotives
  {
    #region GetReimpresionMotives
    /// <summary>
    /// Obtiene registros del catalogo ReimpresionMotives
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos |1. Activos</param>
    /// <param name="reimpresionMotive">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo ReimpresionMotive</returns>
    /// <history>
    /// [emogeul] created 16/04/2016
    /// </history>
    public static List<ReimpresionMotive> GetReimpresionMotives(int nStatus = -1, ReimpresionMotive reimpresionMotive = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from rm in dbContext.ReimpresionMotives
                    select rm;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(rm => rm.rmA == blnStatus);
        }

        if (reimpresionMotive != null)//Verificamos si se tiene un objeto
        {
          if (reimpresionMotive.rmID > 0)//Filtro por ID
          {
            query = query.Where(rm => rm.rmID == reimpresionMotive.rmID);
          }

          if (!string.IsNullOrWhiteSpace(reimpresionMotive.rmN))//Filtro por Descripcion
          {
            query = query.Where(rm => rm.rmN.Contains(reimpresionMotive.rmN));
          }
        }
        return query.OrderBy(rm => rm.rmN).ToList();
      }
    }
    #endregion

    #region SaveReimpresionMotive
    /// <summary>
    /// Actualiza|Agrega un registro al catalogo ReimpresionMotives
    /// </summary>
    /// <param name="reimpresionMotive">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza  | False. Agrega</param>
    /// <returns>0. No se guardó | 1. Se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 16/04/2016
    /// </history>
    public static int SaveReimpresionMotive(ReimpresionMotive reimpresionMotive,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)//Actualizar
        {
          dbContext.Entry(reimpresionMotive).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Add
        else
        {
          ReimpresionMotive reimpresionMotVal = dbContext.ReimpresionMotives.Where(rm => rm.rmID == reimpresionMotive.rmID).FirstOrDefault();
          if(reimpresionMotVal!=null)//Validar si existe un registro con el mismo ID
          {
            return -1;
          }
          else//Agregar
          {
            dbContext.ReimpresionMotives.Add(reimpresionMotive);
          }
        } 
        #endregion
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
