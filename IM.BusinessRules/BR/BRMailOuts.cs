using System;
using IM.Model;
using IM.Model.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRMailOuts
  {
    #region ProcessMailOuts

    /// <summary>
    /// Procesa los MailOuts por la  Clave del Lead Source
    /// </summary>
    /// <param name="leadSourceID">Clave del Lead Source</param>
    /// <param name="date"> Opcional:Fecha </param>
    /// <history>
    ///   [aalcocer] 24/02/2016 Created
    /// </history>
    public static void ProcessMailOuts(string leadSourceID, DateTime? date = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbContext.spProcessMailOuts(leadSourceID, date);
      }
    }

    #endregion

    #region GetMailOuts
    /// <summary>
    /// Obtiene los Mail Outs
    /// </summary>
    /// <param name="leadSourceId">lsID o null si queremos todos</param>
    /// <param name="status">-1 Todos - 1 Activos - 0 Inactivos</param>
    /// <returns>List<MailOuts></MailOuts></returns>
    /// <history>
    /// [erosado] 14/04/2016  Created
    /// [erosado] 06/07/2016  Modified. Se agregó Async
    /// </history>
    public async static Task<List<MailOut>> GetMailOuts(string leadSourceId = null, int status = -1)
    {
      List<MailOut> result = new List<MailOut>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from mo in dbContext.MailOuts
                      select mo;
          if (leadSourceId != null)
          {
            query = query.Where(mo => mo.mols == leadSourceId);
          }
          if (status != -1)
          {
            bool blStatus = Convert.ToBoolean(status);
            query = query.Where(mo => mo.moA == blStatus);
          }
          result = query.ToList();
        }
      });
      return result;
    }

    #endregion

    #region InsertMailOut
    /// <summary>
    /// Elimina un MailOut de MailOuts y MailOutTexts 
    /// </summary>
    /// <param name="_mols">mols</param>
    /// <param name="_moCode">moCode</param>
    /// <returns>-1 Error -0 Ya existe ese MailOut - >0 Registros Afectados</returns>
    /// <history>
    /// [erosado] 20/04/2016  Created
    /// [erosado] 06/07/2016  Modified. Se agregó async.
    /// </history>
    public async static Task<int> InsertMailOut(string _mols, string _moCode)
    {
      int result = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              //Verificamos que no exista un MailOut igual al que queremos insertar
              if (dbContext.MailOuts.Any(x => x.mols == _mols && x.moCode == _moCode))
              {
                result = 0;
              }
              else
              {
                //Agregamos el nuevo MailOut
                dbContext.Entry(
                  new MailOut()
                  {
                    mols = _mols,
                    moCode = _moCode
                  }).State = System.Data.Entity.EntityState.Added;

                //Lista de Languages
                List<string> languages = dbContext.Languages.Select(x => x.laID).ToList();

                //Insertamos un MailOutText Por cada Lenguaje
                languages.ForEach(laID => dbContext.MailOutTexts.Add(
                  new MailOutText()
                  {
                    mtls = _mols,
                    mtmoCode = _moCode,
                    mtla = laID,
                    mtU = BRHelpers.GetServerDateTime()
                  }));

                //Guardamos Cambios
                dbContext.SaveChanges();
                transaction.Commit();
                result = 1;
              }
            }
            catch
            {
              transaction.Rollback();
              throw;
            }

          }
        }
      });
      return result;
    }
    #endregion

    #region UpdateMailOut
    /// <summary>
    /// Actualiza la informacion de un MailOut
    /// </summary>
    /// <param name="mo">MailOut</param>
    /// <returns>Registros Afectados</returns>
    /// <history>
    /// [erosado] 20/04/2016  Created
    /// [erosado] 06/07/2016  Modified. Se agregó Async.
    /// </history>
    public async static Task<int> UpdateMailOut(MailOut mo)
    {
      int result = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Entry(mo).State = System.Data.Entity.EntityState.Modified;
          result = dbContext.SaveChanges();
        }
      });

      return result;
    }
    #endregion

    #region DeleteMailOut
    /// <summary>
    /// Elimina un MailOut de MailOuts y MailOutTexts 
    /// </summary>
    /// <param name="mo">MailOut</param>
    /// <returns> Numero de registros afectados </returns>
    /// <history>
    /// [erosado] 20/04/2016  Created
    /// [erosado] 06/07/2016  Modified. Se agregó Async
    /// </history>
    public async static Task<int> DeleteMailOut(MailOut mo)
    {
      int result = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              //Eliminamos el MailOut 
              dbContext.Entry(mo).State = System.Data.Entity.EntityState.Deleted;
              //Obtenemos todos los MailOutText que correspondan al MailOut Que vamos a eliminar
              List<MailOutText> motlst = (from x in dbContext.MailOutTexts where x.mtls == mo.mols && x.mtmoCode == mo.moCode select x).ToList();
              //Eliminamos cada uno de los MailOutTexts
              motlst.ForEach(x => dbContext.Entry(x).State = System.Data.Entity.EntityState.Deleted);
              result = dbContext.SaveChanges();
              transaction.Commit();
            }
            catch
            {
              transaction.Rollback();
              throw;
            }
          }
        }
      });
      return result;
    }
    #endregion

  }
}