using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRChargeTos
  {
    #region GetChargeTos

    /// <summary>
    /// Devuelve la lista de chargeTo
    /// </summary>
    /// <param name="chargeTo">Entidad que contiene los filtros adicionales, puede ser null</param>
    /// <param name="nCxC">-1.Todos | 0. CxC=falso | 1. CxC=true</param>
    /// <returns>Lista de Charge To</returns>
    /// <history>
    /// [Emoguel] created 01/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto
    /// [aalcocer] 25/05/2016  Modified. Se agregó asincronía
    /// </history>
    public static async Task<List<ChargeTo>> GetChargeTos(ChargeTo chargeTo = null, int nCxC = -1)
    {
      List<ChargeTo> lstCharge = new List<ChargeTo>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from c in dbContext.ChargeTos
                      select c;

          if (nCxC != -1) //Filtra por CxC
          {
            bool blnCxc = Convert.ToBoolean(nCxC);
            query = query.Where(c => c.ctIsCxC == blnCxc);
          }

          if (chargeTo != null) //Si se recibió objeto
          {
            if (!string.IsNullOrWhiteSpace(chargeTo.ctID)) //Filtra por ID
            {
              query = query.Where(c => c.ctID == chargeTo.ctID);
            }

            if (chargeTo.ctPrice > 0) //Filtra por Price
            {
              query = query.Where(c => c.ctPrice == chargeTo.ctPrice);
            }
          }

          lstCharge = query.OrderBy(c => c.ctID).ToList();
        }
      });

      return lstCharge;
    }

    #endregion GetChargeTos
  }
}