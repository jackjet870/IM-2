using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;

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
    /// [emoguel] modified 28/06/2016 -----> Se volvió async
    /// </history>
    public async static Task<List<SaleTypeCategory>> GetSaleCategories(int nStatus = -1, SaleTypeCategory saleTypeCategory = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
      });
    }
    #endregion

    #region SaveSaleTypeCategory
    /// <summary>
    /// Guarda un sale Type Category.
    /// agregar sale Types al sale type category
    /// Eliminar saleTypes del sale type category
    /// </summary>
    /// <param name="saleTypeCategory">Objeto a guardar en la BD</param>
    /// <param name="blnUpdate">
    /// True. Actualiza
    /// False. Agrega
    /// </param>
    /// <param name="lstAddSaleTypes">Sale Types a agregar</param>
    /// <param name="lstDelSaleTypes">Sale Types a Eliiminar</param>
    /// <returns>-1.- Existe un registro con el mismo ID| 0. No se guardó | >0. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 01-08-2016
    /// </history>
    public static async Task<int> SaveSaleTypeCategory(SaleTypeCategory saleTypeCategory, bool blnUpdate, List<SaleType> lstAddSaleTypes, List<SaleType> lstDelSaleTypes)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {

              if (blnUpdate)
              {
                dbContext.Entry(saleTypeCategory).State = EntityState.Modified;
              }
              else
              {
                if(dbContext.SaleTypesCategories.Where(stc=>stc.stcID==saleTypeCategory.stcID).FirstOrDefault()!=null)
                {
                  return -1;
                }
                else
                {
                  dbContext.Entry(saleTypeCategory).State = EntityState.Added;
                }
              }

              #region 
              //Agregar saleTypes
              if (lstAddSaleTypes.Count > 0)
              {
                lstAddSaleTypes.ForEach(st =>
                {
                  st.ststc = saleTypeCategory.stcID;
                  dbContext.Entry(st).State = EntityState.Modified;
                });
              }
              //Eliminar saleTypes
              if (lstDelSaleTypes.Count > 0)
              {
                lstDelSaleTypes.ForEach(st =>
                {
                  st.ststc = null;
                  dbContext.Entry(st).State = EntityState.Modified;
                });
              }
              #endregion

              int nRes = dbContext.SaveChanges();
              transacction.Commit();
              return nRes;
            }
            catch
            {
              transacction.Rollback();
              throw;
            }
          }
        }
      });
    } 
    #endregion
  }
}
