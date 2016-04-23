using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRSaleTypesCategories
  {
    #region GetSaleCategories
    /// <summary>
    /// Obtiene registros del catalogo SaleTypeCategories
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="saleTypeCategory">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo SaleTypeCategory</returns>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    public static List<SaleTypeCategory> GetSaleCategories(int nStatus = -1, SaleTypeCategory saleTypeCategory = null)
    {

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from stc in dbContext.SaleTypesCategories
                    select stc;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(stc => stc.stcA == blnStatus);
        }

        if (saleTypeCategory != null)//Verificamos que se tenga un objeto
        {
          if (!string.IsNullOrWhiteSpace(saleTypeCategory.stcID))
          {
            query = query.Where(stc => stc.stcID == saleTypeCategory.stcID);
          }

          if (!string.IsNullOrWhiteSpace(saleTypeCategory.stcN))//Filtro por descripción
          {
            query = query.Where(stc => stc.stcN.Contains(saleTypeCategory.stcN));
          }
        }

        return query.OrderBy(stc => stc.stcN).ToList();

      }
    } 
    #endregion
  }
}
