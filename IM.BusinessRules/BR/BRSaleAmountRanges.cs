using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRSaleAmountRanges
  {
    #region GetSalesAmountRanges
    /// <summary>
    /// Obtiene registros del catalogo SalesAmountRanges
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0.Inactivos | 1.Activos</param>
    /// <param name="salesAmountRanges">objeto con filtros adicionales</param>
    /// <returns>Lista de tipo SalesAmountRange</returns>
    /// <history>
    /// [emoguel] created 20/04/2016
    /// </history>
    public static List<SalesAmountRange> GetSalesAmountRanges(int nStatus = -1, SalesAmountRange salesAmountRanges = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from sn in dbContext.SalesAmountRanges
                    select sn;

        if (nStatus != -1)
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(sn => sn.snA == blnStatus);
        }

        if (salesAmountRanges != null)
        {
          if(salesAmountRanges.snID>0)//Filtro por ID
          {
            query = query.Where(sn => sn.snID == salesAmountRanges.snID);
          }

          if(!string.IsNullOrWhiteSpace(salesAmountRanges.snN))//Filtro por descripción
          {
            query = query.Where(sn => sn.snN.Contains(salesAmountRanges.snN));
          }

          if(salesAmountRanges.snFrom>0 && salesAmountRanges.snTo>0)//Filtro por rango
          {
            if (salesAmountRanges.snFrom == salesAmountRanges.snTo)
            {
              query = query.Where(sn => salesAmountRanges.snFrom >= sn.snFrom && salesAmountRanges.snTo <= sn.snTo);
            }
            else
            {
              query = query.Where(sn => sn.snFrom >= salesAmountRanges.snFrom && sn.snTo <= salesAmountRanges.snTo);
            }
          }
          
        }

        return query.OrderBy(sn => sn.snN).ToList();
      }
    }
    #endregion

    #region SaveSalesAmountRange
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo SalesAmountRanges
    /// </summary>
    /// <param name="salesAmountRange">Objeto a guardar</param>
    /// <param name="blnUpdate">Truw. Actualiza  | False. Agrega</param>
    /// <returns>0. No se guardó | 1. Se guardó  | -2. verificar el rango</returns>
    /// <history>
    /// [emoguel] crated 20/04/2016
    /// </history>
    public static int SaveSalesAmountRange(SalesAmountRange salesAmountRange,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {

        #region Validar Rango
        bool blnValid = false;
        var query = dbContext.SalesAmountRanges.Where(sn => sn.snFrom <= salesAmountRange.snFrom && salesAmountRange.snFrom <= sn.snTo);
        if(blnUpdate)
        {
          query=query.Where(sn => sn.snID != salesAmountRange.snID);
        }
        SalesAmountRange salAmoRange = query.FirstOrDefault();

        blnValid = (salAmoRange == null);
        #endregion

        if (blnValid)
        {
          dbContext.Entry(salesAmountRange).State = (blnUpdate) ? EntityState.Modified : EntityState.Added;
        }
        else
        {
          return -2;
        }
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
