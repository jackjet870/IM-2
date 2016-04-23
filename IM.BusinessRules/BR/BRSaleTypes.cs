using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRSaleTypes
  {
    #region GetSalesTypes
    /// <summary>
    /// Obtiene registros del catalogo SaleTypes
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Iniactivos  | 1. Activos</param>
    /// <param name="saleType">Objeto con  filtros adicionales</param>
    /// <returns>Lista de tipo SaleType>/returns>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    public static List<SaleType> GetSalesTypes(int nStatus = -1, SaleType saleType = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from st in dbContext.SaleTypes
                    select st;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(st => st.stA == blnStatus);
        }

        if (saleType != null)
        {
          if (!string.IsNullOrWhiteSpace(saleType.stID))//Filtro por ID
          {
            query = query.Where(st => st.stID == saleType.stID);
          }

          if (!string.IsNullOrWhiteSpace(saleType.stN))//Filtro por descripción
          {
            query = query.Where(st => st.stN.Contains(saleType.stN));
          }

          if (!string.IsNullOrWhiteSpace(saleType.ststc))//Filtro por categoria
          {
            query = query.Where(st => st.ststc == saleType.ststc);
          }
        }

        return query.OrderBy(st => st.stID).ToList();
      }
    }
    #endregion

    #region SaveSaleType
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo SaleTypes
    /// </summary>
    /// <param name="saleType">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0. No se guardó | 1. Se guardó  | -1. existe un registro con el mismo ID </returns>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    public static int SaveSaleType(SaleType saleType, bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)//Actualizar
        {
          dbContext.Entry(saleType).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Add
        else//Add
        {
          SaleType saleTypeVal = dbContext.SaleTypes.Where(st => st.stID == saleType.stID).FirstOrDefault();
          if(saleTypeVal!=null)//Verificamos que no exista un registro con el mismo ID
          {
            return -1;
          }
          else//Agregar el registro
          {
            dbContext.SaleTypes.Add(saleType);
          }
        } 
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
