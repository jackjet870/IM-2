using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRFoliosCXCPR
  {
    #region GetPRByFoliosCXC
    /// <summary>
    /// Obtiene personnels ligados a FoliosCxCPR
    /// </summary>
    /// <param name="personnelShort">Objeto con filtros adicionales</param>
    /// <returns>Lista tipo PersonnelShort</returns>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    public async static Task<List<PersonnelShort>> GetPRByFoliosCXC(PersonnelShort personnelShort = null)
    {
      List<PersonnelShort> lstPersonnelShort = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
          {
            var query = from pe in dbContext.Personnels
                        join pr in dbContext.FoliosCxCPR
                        on pe.peID equals pr.fcppe
                        group pe by new { pe.peID, pe.peN } into p
                        select p;

            if (personnelShort != null)
            {
              if (!string.IsNullOrWhiteSpace(personnelShort.peID))//Filtro por ID
            {
                query = query.Where(pe => pe.Key.peID == personnelShort.peID);
              }

              if (!string.IsNullOrWhiteSpace(personnelShort.peN))//Filtro por descripción
            {
                query = query.Where(pe => pe.Key.peN.Contains(personnelShort.peN));
              }
            }

            List<PersonnelShort> lstPersonnelShorts = query.ToList().Select(pe => new PersonnelShort { peID = pe.Key.peID, peN = pe.Key.peN }).ToList();
            return lstPersonnelShorts.OrderBy(pe => pe.peN).ToList();
          }
        });
      return lstPersonnelShort;
    }
    #endregion

    #region GetFoliosCXCPR
    /// <summary>
    /// obtiene registros del catalogo FolcioCXCPR
    /// </summary>
    /// <param name="peID">PR asignado</param>
    /// <returns>Lista de FolioCxCPR</returns>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// [emoguel] modified 09/06/2016 --> se volvió async
    /// </history>
    public async static Task<List<FolioCxCPR>> GetFoliosCXCPR(string peID)
    {
      List<FolioCxCPR> lstFolios = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
          {
            return dbContext.FoliosCxCPR.Where(fc => fc.fcppe == peID).ToList();
          }
        });
      return lstFolios;
    }
    #endregion

    #region FoliosCxCPRIsValid
    /// <summary>
    /// Valida que el rango de folios By PR no lo tenga asignado otro PR
    /// </summary>
    /// <param name="prID">ID del PR</param>
    /// <param name="From">incio del rango a validar</param>
    /// <param name="To">Final del rango a validar</param>
    /// <param name="blnIsCancel">True. Cancel Folios | False. FolioCXCPR</param>
    /// <returns>ValidateFolioCxCPR</returns>
    /// <history>
    /// [emoguel] created 06/05/2016
    /// </history>
    public static ValidationFolioData ValidateFolio(string prID, int from, int to, bool blnIsCancel)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_ValidateFolioCxCPR(prID, from, to, blnIsCancel).FirstOrDefault();
      }
    }
    #endregion

    #region ValidateFolioRange
    /// <summary>
    /// Valida que el rango exista en el catalogo FoliosCxC
    /// </summary>
    /// <param name="from">inicio del rango a validar</param>
    /// <param name="to">Fin del rango a validar</param>
    /// <returns>True. Es valido | False. No es valido</returns>
    /// <history>
    /// [emoguel] created 06/05/2016
    /// </history>
    public static bool ValidateFolioRange(int from,int to)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var folio = dbContext.FoliosCXC.Where(f => f.fiFrom <= from && f.fiTo >= to).FirstOrDefault();
        return (folio != null);
      }
    }
    #endregion

    #region SaveFoliosCxCByPR
    /// <summary>
    /// Agrega Folios CxCPR y FolioCxCCancellation
    /// </summary>
    /// <param name="personnel"></param>
    /// <param name="lstFoliosCxCPR"></param>
    /// <param name="lstFoliosCancell"></param>
    /// <returns>0. No se guardó | >0. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// [emoguel] modified 09/06/2016 -->Se volvió async
    /// </history>
    public async static Task<int> SaveFoliosCxCByPR(PersonnelShort personnel,List<FolioCxCPR> lstFoliosCxCPR,List<FolioCxCCancellation> lstFoliosCancell)
    {
      int nRes = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
          {
            using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
              try
              {
                lstFoliosCxCPR.ForEach(fcp =>
                {
                  fcp.fcppe = personnel.peID;
                  dbContext.Entry(fcp).State = (fcp.fcpID > 0) ? EntityState.Modified : EntityState.Added;
                });

                lstFoliosCancell.ForEach(fcc =>
                {
                  fcc.fccpe = personnel.peID;
                  dbContext.Entry(fcc).State = (fcc.fccID > 0) ? EntityState.Modified : EntityState.Added;
                });

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
