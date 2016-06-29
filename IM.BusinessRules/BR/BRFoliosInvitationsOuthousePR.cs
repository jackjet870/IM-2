using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRFoliosInvitationsOuthousePR
  {

    #region GetPRbyFolioOuthouse
    /// <summary>
    /// Obtiene los pr que tengan foliosOutside
    /// </summary>
    /// <param name="personnelShort">objeto con filtros adicionales</param>
    /// <returns>Lista de tipo PersonnelShort</returns>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// </history>
    public async static Task<List<PersonnelShort>> GetPRbyFolioOuthouse(PersonnelShort personnelShort = null)
    {
      List<PersonnelShort> lstPersonnelShort = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
          {
            var query = from pe in dbContext.Personnels
                        join pr in dbContext.FoliosInvitationsOuthousePR
                        on pe.peID equals pr.fippe
                        group pe by new { pe.peID, pe.peN } into p
                        select p;

            if (personnelShort != null)
            {
              if (!string.IsNullOrWhiteSpace(personnelShort.peID))
              {
                query = query.Where(pe => pe.Key.peID == personnelShort.peID);
              }

              if (!string.IsNullOrWhiteSpace(personnelShort.peN))
              {
                query = query.Where(pe => pe.Key.peN == personnelShort.peN);
              }
            }

            List<PersonnelShort> lstPersonnelShorts = query.ToList().Select(pe => new PersonnelShort { peID = pe.Key.peID, peN = pe.Key.peN }).ToList();
            return lstPersonnelShorts.OrderBy(pe => pe.peN).ToList();
          }
        });
      return lstPersonnelShort;
    }
    #endregion

    #region GetSeriesFolioOuthouse
    /// <summary>
    /// Obtiene los diferentes folios en FoliosInvitationsOuthouse
    /// </summary>
    /// <returns>Lista de tipo string</returns>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// [emoguel] modifed 27/06/2016 --> se volvió async
    /// </history>
    public async static Task<List<string>> GetSeriesFolioOuthouse()
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from fo in dbContext.FoliosInvitationsOuthouse
                      group fo by new { fo.fiSerie } into se
                      select se;

          return query.ToList().Select(se => se.Key.fiSerie.ToString()).ToList();
        }
      });
    }
    #endregion

    #region GetFoliosByPR
    /// <summary>
    /// Obtiene los FoliosInvitatation outhuse ligados a un Pr
    /// </summary>
    /// <param name="idPr">Id del personnel relacionado</param>
    /// <returns>Lista de tipo FolioInvitationOuthousePR</returns>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    public async static Task<List<FolioInvitationOuthousePR>> GetFoliosByPr(string idPr)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from fip in dbContext.FoliosInvitationsOuthousePR
                      where fip.fippe == idPr
                      select fip;

          return query.OrderBy(fip => fip.fipID).ToList();
        }
      });
    }
    #endregion

    #region ValidateFolioRange
    /// <summary>
    /// Valida que el rango exista en el catalogo FoliosInvitationOuthouse
    /// </summary>
    /// <param name="serie">Serie a validar</param>
    /// <param name="from">inicio del rango a validar</param>
    /// <param name="to">Fin del rango a validar</param>
    /// <returns>True. Es valido | False. No es valido</returns>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    public static bool ValidateFolioRange(string serie,int from, int to)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var folio = dbContext.FoliosInvitationsOuthouse.Where(f => f.fiFrom <= from && f.fiTo >= to && f.fiSerie==serie).FirstOrDefault();
        return (folio != null);
      }
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
    public static ValidationFolioData ValidateFolio(string prID,string serie, int from, int to, bool blnIsCancel)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_ValidateFolioInvOutPR(prID, serie, from, to, blnIsCancel).FirstOrDefault();
      }
    }
    #endregion

    #region SaveFolios
    /// <summary>
    /// Agrega|Actualiza folios outhousePR y outhousecancellation
    /// </summary>
    /// <param name="idPr">id Del PR</param>
    /// <param name="lstAsigned">Lista de asignados</param>
    /// <param name="lstCancel">Lista de cancelados</param>
    /// <returns>0. No se guardó | >0. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    public async static Task<int> SaveFoliosOuthousePR(string idPr,List<FolioInvitationOuthousePR> lstAsigned, List<FolioInvitationOuthousePRCancellation> lstCancel)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region Assigados
              lstAsigned.ForEach(f =>
              {
                f.fippe = idPr;
                dbContext.Entry(f).State = (f.fipID > 0) ? EntityState.Modified : EntityState.Added;

              });
              #endregion
              #region Cancel
              lstCancel.ForEach(f =>
              {
                f.ficpe = idPr;
                dbContext.Entry(f).State = (f.ficID > 0) ? EntityState.Modified : EntityState.Added;
              });
              #endregion

              int nRes = dbContext.SaveChanges();
              transaction.Commit();
              return nRes;
            }
            catch
            {
              transaction.Rollback();
              return 0;
            }
          }
        }
      });
    }
    #endregion
  }
}
