using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    /// </history>
    public static List<EfficiencyType> GetEfficiencyTypes(EfficiencyType efficiencyType = null, int nStatus = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
    }
    #endregion

    #region SaveEfficiencyType
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo efficiencyTypes
    /// </summary>
    /// <param name="efficiencyType">Objeto a guardar</param>
    /// <param name="blnUpdate">true. Actualiza | false. Inserta</param>
    /// <returns></returns>
    public static int SaveEfficiencyType(EfficiencyType efficiencyType,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        if (blnUpdate)//Si es actualizar
        {
          dbContext.Entry(efficiencyType).State = System.Data.Entity.EntityState.Modified;
          return dbContext.SaveChanges();
        }
        else//Si es Insertar
        {
          EfficiencyType efficiencyTypeVal = dbContext.EfficiencyTypes.Where(eft => eft.etID == efficiencyType.etID).FirstOrDefault();
          if(efficiencyTypeVal!=null)//Si existe un registro con el mismo id
          {
            return -1;
          }
          else//Insertar
          {
            dbContext.EfficiencyTypes.Add(efficiencyType);
            return dbContext.SaveChanges();
          }
        }
      }
    }
    #endregion
  }
}
