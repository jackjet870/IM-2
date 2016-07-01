using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BREfficiencyTypes
  {
    #region GetEfficiencyTypes
    /// <summary>
    /// obtiene una lista de tipo EfficiencyType
    /// </summary>
    /// <param name="efficiencyType">Objeto con filtro adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registro inactivos | 1. Registros Activos</param>
    /// <returns>Devuelve una lista de tipo EfficiencyType</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// [emoguel] modified 09/06/2016-->Se volvió async
    /// </history>
    public async static Task<List<EfficiencyType>> GetEfficiencyTypes(EfficiencyType efficiencyType = null, int nStatus = -1)
    {
      List<EfficiencyType> lstEfficency = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var query = from eft in dbContext.EfficiencyTypes
                        select eft;

            if (nStatus != -1)//Filtro por estatus
          {
              bool blnEstatus = Convert.ToBoolean(nStatus);
              query = query.Where(eft => eft.etA == blnEstatus);
            }

            if (efficiencyType != null)//Si se tiene un objeto
          {
              if (!string.IsNullOrWhiteSpace(efficiencyType.etID))//Filtro por ID
            {
                query = query.Where(eft => eft.etID == efficiencyType.etID);
              }

              if (!string.IsNullOrWhiteSpace(efficiencyType.etN))//Filtro por descripcion
            {
                query = query.Where(eft => eft.etN.Contains(efficiencyType.etN));
              }
            }

            return query.OrderBy(eft => eft.etN).ToList();
          }
        });
      return lstEfficency;
    }
    #endregion    
  }
}
