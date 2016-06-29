using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRShowProgramsCategories
  {
    #region GetShowProgramsCategories
    /// <summary>
    /// Obtiene registros del catalogo ShowProgramsCategories
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0.Inactivos | 1. todos</param>
    /// <param name="showProgramCategory">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo ShowProgramCategory</returns>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    public async static Task<List<ShowProgramCategory>> GetShowProgramsCategories(int nStatus = -1, ShowProgramCategory showProgramCategory = null)
    {
      List<ShowProgramCategory> lstShowCategories =
        await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
          {
            var query = from sg in dbContext.ShowProgramsCategories
                        select sg;

            if (nStatus != -1)//Filtro por estatus
            {
              bool blnStatus = Convert.ToBoolean(nStatus);
              query = query.Where(sk => sk.sgA == blnStatus);
            }

            if (showProgramCategory != null)//Filtro por ID
            {
              if (!string.IsNullOrWhiteSpace(showProgramCategory.sgID))
              {
                query = query.Where(sk => sk.sgID == showProgramCategory.sgID);
              }

              if (!string.IsNullOrWhiteSpace(showProgramCategory.sgN))//Filtro por descripción
              {
                query = query.Where(sk => sk.sgN.Contains(showProgramCategory.sgN));
              }
            }

            return query.OrderBy(sg => sg.sgN).ToList();
          }
        });
      return lstShowCategories;
    }
    #endregion

    #region SaveShowProgramcategory
    /// <summary>
    /// Guarda un registro en el catalogo ShowProgramcategories
    /// Asigan y Desasigna showprograms a su category
    /// </summary>
    /// <param name="showProgramcategory">Objeto a guardar</param>
    /// <param name="lstAdd">Lista para asignar</param>
    /// <param name="lstDel">Lista para desasignar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>-1. Existe un registro con el mismo ID | 0. No se guardó | 1. se guardó</returns>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    public static async Task<int> SaveShowProgramCategory(ShowProgramCategory showProgramcategory,List<ShowProgram> lstAdd,List<ShowProgram>lstDel,bool blnUpdate)
    {
      int nRes = await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region Actualizar
              if (blnUpdate)
              {
                dbContext.Entry(showProgramcategory).State = System.Data.Entity.EntityState.Modified;
              }
              #endregion
              #region Agregar
              else
              {
                if(dbContext.ShowProgramsCategories.Where(sg=>sg.sgID==showProgramcategory.sgID).FirstOrDefault()!=null)
                {
                  return -1;
                }
                else
                {
                  dbContext.ShowProgramsCategories.Add(showProgramcategory);
                }
              }
              #endregion

              #region AddShowProgram
              dbContext.ShowPrograms.AsEnumerable().Where(sk => lstAdd.Any(skk => sk.skID == skk.skID)).ToList().ForEach(sk =>
              {
                sk.sksg = showProgramcategory.sgID;
              });
              #endregion

              #region DelShowProgram
              dbContext.ShowPrograms.AsEnumerable().Where(sk => lstDel.Any(skk => sk.skID == skk.skID)).ToList().ForEach(sk =>
              {
                sk.sksg = null;
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
