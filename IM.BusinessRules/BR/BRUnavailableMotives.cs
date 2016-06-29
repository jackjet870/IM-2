using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRUnavailableMotives
  {
    #region GetUnavailableMotives
    /// <summary>
    /// Devuelve la lista de UnavailMots
    /// </summary>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1. Registros Activos</param>
    /// <returns>Lista de UnavailMots</returns>
    /// <history>
    /// [Emoguel] created 01/03/2016
    /// [emoguel] modified 30/05/2016 Se volvió async el método
    /// </history>
    public async static Task<List<UnavailableMotive>> GetUnavailableMotives(int nStatus=-1,UnavailableMotive unavailableMotive=null)    
    {
      List<UnavailableMotive> lstUnavailMot = new List<UnavailableMotive>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from um in dbContext.UnavailableMotives
                      select um;

          if (nStatus != -1)//Se filtra por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(um => um.umA == blnStatus);
          }

          if(unavailableMotive!=null)
          {
            if(unavailableMotive.umID>0)//filtro por ID
            {
              query = query.Where(um => um.umID == unavailableMotive.umID);
            }

            if(!string.IsNullOrWhiteSpace(unavailableMotive.umN))//Filtro por descripción
            {
              query = query.Where(um => um.umN.Contains(unavailableMotive.umN));
            }

          }
          lstUnavailMot = query.OrderBy(um => um.umN).ToList();
        }
      });

      return lstUnavailMot;      
    }
    #endregion

    #region GetUnavailableMotive

    /// <summary>
    /// Obtiene un motivo de indisponibilidad dada su clave
    /// </summary>
    /// <param name="id">Clave</param>
    /// <param name="nStatus">false: Registros inactivos | true: Registros Activos</param>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// </history>
    public static UnavailableMotive GetUnavailableMotive(int id, bool nStatus)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.UnavailableMotives.Where(u => u.umID == id && u.umA == nStatus).FirstOrDefault();
      }
    }

    #endregion

    #region SaveUnavailableMotive
    /// <summary>
    /// Guarda un registro en el catalogo UnavailableMotives
    /// </summary>
    /// <param name="unavailableMotive">objeto a guardar</param>
    /// <param name="lstAgencyAdd">Agencies a asignar</param>
    /// <param name="lstContractAdd">Contracts a asignar</param>
    /// <param name="lstCountryAdd">Countries a asignar</param>
    /// <param name="blnUpdate">True. Actualiza | false. Insertar</param>
    /// <returns>-1. Existe un registro con el mismo ID| 0. No se guardó | 1. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    public async static Task<int> SaveUnavailableMotives(UnavailableMotive unavailableMotive,List<Agency>lstAgencyAdd,List<Contract>lstContractAdd,List<Country>lstCountryAdd,bool blnUpdate)
    {
      int nRes = await Task.Run(() => {

        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region Update
              if (blnUpdate)
              {
                dbContext.Entry(unavailableMotive).State = System.Data.Entity.EntityState.Modified;
              }
              #endregion
              #region Insert
              else
              {
                if(dbContext.UnavailableMotives.Where(um=>um.umID==unavailableMotive.umID).FirstOrDefault()!=null)
                {
                  return -1;
                }
                else
                {
                  dbContext.UnavailableMotives.Add(unavailableMotive);
                }
              }
              #endregion

              #region Agencies
              dbContext.Agencies.AsEnumerable().Where(ag => lstAgencyAdd.Any(agg => agg.agID == ag.agID)).ToList().ForEach(ag => {
                ag.agum = unavailableMotive.umID;
              });
              #endregion

              #region Countries
              dbContext.Countries.AsEnumerable().Where(co => lstCountryAdd.Any(coo => coo.coID == co.coID)).ToList().ForEach(co => {
                co.coum = unavailableMotive.umID;
              });
              #endregion

              #region Contracts
              dbContext.Contracts.AsEnumerable().Where(cn => lstContractAdd.Any(cnn => cnn.cnID == cn.cnID)).ToList().ForEach(cn => {
                cn.cnum = unavailableMotive.umID;
              });
              #endregion

              int nSave = dbContext.SaveChanges();
              transacction.Commit();
              return nSave;
            }
            catch
            {
              transacction.Rollback();
              return 0;
            }
          }
        }
      });

      return nRes;
    }
    #endregion
  }
}
