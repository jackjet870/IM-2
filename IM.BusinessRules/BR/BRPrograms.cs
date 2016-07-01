using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRPrograms
  {
    #region GetPrograms
    /// <summary>
    /// Obtiene una lista de Programas
    /// </summary>
    /// <returns>List<Program></returns>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// [edgrodriguez] 21/05/2016 Modified. El metodo se volvió asincrónico.
    /// </history>
    public async static Task<List<Program>> GetPrograms()
    {
      List<Program> lstPrograms = new List<Program>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {

        lstPrograms = dbContext.Programs.ToList();        
        }
      });
      return lstPrograms;
    }
    #endregion

    #region SaveProgram
    /// <summary>
    /// Agrega|Actualiza un Program
    /// Asigna una lista de leadSource
    /// </summary>
    /// <param name="program">Objeto a guardar</param>
    /// <param name="lstAdd">LeadSources a asignar</param>
    /// <param name="blnUpdate">True. Actualiza | False. inserta</param>
    /// <returns>-1. Existe un registro con el mismo ID | 0. No se guardó | >0. Se guardó</returns>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    public async static Task<int> SaveProgram(Program program, List<LeadSource> lstAdd, bool blnUpdate)
    {
      int nRes = 0;
      nRes=await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region MyRegion
              if (blnUpdate)//Actualiza
              {
                dbContext.Entry(program).State = EntityState.Modified;
              }
              #endregion
              #region Add
              else
              {
                if (dbContext.Programs.Where(pg => pg.pgID == program.pgID).FirstOrDefault() != null)
                {
                  dbContext.Programs.Add(program);
                }
              }
              #endregion

              #region LeadSource
              dbContext.LeadSources.AsEnumerable().Where(ls => lstAdd.Any(lss => lss.lsID == ls.lsID)).ToList().ForEach(ls =>
              {
                ls.lspg = program.pgID;
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
