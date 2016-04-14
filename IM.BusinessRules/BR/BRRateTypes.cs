using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRRateTypes
  {
    #region GetRateType
    /// <summary>
    /// Obtiene registros del catalogo RateTypes
    /// </summary>
    /// <param name="rateType">objeto con filtros adicionales</param>
    /// <param name="nStatus"> -1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="raIDMayorA"> Estatus mayor a un ID especificado </param>
    /// <param name="orderByraN"> Estatus para ordenar por raN </param>
    /// <returns> List<RateType> </returns>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// [vipacheco] 18/03/2016 Modified --> Se agregó parametro raIDMayorA para la busqueda mayor a ese ID especificado
    /// [emoguel] 13/04/2016 Modified--->Se modificaron los tipos de variable y nombe de los parametros ID ((RateType)rateType) y raA ((int)nStatus) 
    /// </history>
    public static List<RateType> GetRateTypes(RateType rateType=null, int nStatus = -1, bool raIDMayorA = false, bool orderByraN = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from ra in dbContext.RateTypes
                    select ra;

        if(nStatus!=-1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(ra => ra.raA == blnStatus);
        }
        if(rateType!=null)//Validamos si tenemos un objeto
        {
          #region Filtro por ID
          if (rateType.raID > 0)
          {
            if (raIDMayorA)
            {
              query = query.Where(ra => ra.raID > ra.raID);//id mayor a
            }
            else
            {
              query = query.Where(ra => ra.raID == rateType.raID);//id igual a
            }
          }
          #endregion

          if(!string.IsNullOrWhiteSpace(rateType.raN))//Filtro por descripción
          {
            query = query.Where(ra => ra.raN.Contains(rateType.raN));
          }
        }

        if(orderByraN)//ordenar
        {
          query = query.OrderBy(ra => ra.raN);
        }        

        return query.ToList();
      }
    }
    #endregion

    #region SaveRateType
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo RateTypes
    /// </summary>
    /// <param name="rateType">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza| False. Agrega</param>
    /// <returns>0. No se guardó | 1. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 13/04/2016
    /// </history>
    public static int SaveRateType(RateType rateType,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Entry(rateType).State = (blnUpdate) ? EntityState.Modified : EntityState.Added;
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
