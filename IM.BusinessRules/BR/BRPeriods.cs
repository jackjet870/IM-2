using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRPeriods
  {
    #region GetPeriods
    /// <summary>
    /// Obtiene registros del catalogo Periods
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="period">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Periods</returns>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    public static List<Period> GetPeriods(int nStatus = -1, Period period = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from pd in dbContext.Periods
                    select pd;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(pd => pd.pdA == blnStatus);
        }

        if (period != null)
        {
          if (!string.IsNullOrWhiteSpace(period.pdID))//Filtro por ID
          {
            query = query.Where(pd => pd.pdID == period.pdID);
          }

          if (!string.IsNullOrWhiteSpace(period.pdN))//Filtro por descripción
          {
            query = query.Where(pd => pd.pdN.Contains(period.pdN));
          }
        }

        return query.OrderBy(pd => pd.pdN).ToList();
      }
    }
    #endregion

    #region SavePeriod
    /// <summary>
    /// Guarda1 Actualiza un registro en el catalogo Periods
    /// </summary>
    /// <param name="period">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <returns>0. No se guardó | 1. Se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 07/04/2016
    /// </history>
    public static int SavePeriod(Period period,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)//Si es actualizar
        {
          dbContext.Entry(period).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Add
        else
        {
          Period periodVal = dbContext.Periods.Where(pd => pd.pdID == period.pdID).FirstOrDefault();
          if(periodVal!=null)//Validar si existe un registro con el mismo ID
          {
            return -1;
          }
          else
          {
            dbContext.Periods.Add(period);
          }
        } 
        #endregion
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
