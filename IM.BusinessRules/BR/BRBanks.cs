using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRBanks
  {

    #region GetBanks
    /// <summary>
    /// Obtiene la lista de bancos activos
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 15/Abril/2016 Created
    /// [emoguel] modified 30/04/2016 Se agregaron mas parametros de busqueda
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    public async static Task<List<Bank>> GetBanks(int status = -1,Bank bank=null,bool blnInclude=false)
    {
      List<Bank> lstBanks = new List<Bank>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = (from bk in dbContext.Banks
                       select bk);
          if (blnInclude)//Cargar los sales rooms
          {
            query = query.Include("SalesRooms");
          }
          if (status != -1)//Filtro por estatus
          {
            bool _status = Convert.ToBoolean(status);
            query = query.Where(bk => bk.bkA == _status);
          }

          if (bank != null)
          {
            if (!string.IsNullOrWhiteSpace(bank.bkID))//Filtro por ID
            {
              query = query.Where(bk => bk.bkID == bank.bkID);
            }

            if (!string.IsNullOrWhiteSpace(bank.bkN))//Filtro por descripción
            {
              query = query.Where(bk => bk.bkN.Contains(bank.bkN));
            }
          }

          lstBanks = query.OrderBy(bk => bk.bkN).ToList();
        }
      });
      return lstBanks;
    }
    #endregion

    #region SaveBank
    /// <summary>
    /// Agrega|Actualiza registros de 
    /// </summary>
    /// <param name="bank">objeto a guardar</param>
    /// <param name="blnUpdate">True Actualiza| False Agrega</param>
    /// <param name="lstSrAdd">Lista a agregar</param>
    /// <param name="lstSrDel">Lista a eliminar</param>
    /// <returns>0. No se guardó| 1. Se guardó |-1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    public async static Task<int> SaveBank(Bank bank, bool blnUpdate,List<SalesRoom> lstSrAdd, List<SalesRoom> lstSrDel)
    {
      int nRes = 0;      
      nRes=await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {          
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              Bank bankSave = null;
              string bankId = bank.bkID;
              #region Update
              if (blnUpdate)
              {
                bankSave = dbContext.Banks.Where(bk => bk.bkID == bankId).Include("SalesRooms").FirstOrDefault();
                bankSave.bkID = bank.bkID;
                bankSave.bkN = bank.bkN;
                bankSave.bkA = bank.bkA;
              }
              #endregion
              #region Add
              else
              {
                bankSave = dbContext.Banks.Where(bk => bk.bkID == bankId).Include("SalesRooms").FirstOrDefault();
                if (bankSave != null)
                {
                  return -1;
                }
                else
                {
                  bankSave = bank;
                  dbContext.Banks.Add(bankSave);
                }
              }
              #endregion

              #region BanksBySalesroom
              if (bankSave != null)
              {
                #region Added
                lstSrAdd.ForEach(sr =>
                {
                  bankSave.SalesRooms.Add(dbContext.SalesRooms.Where(srr => srr.srID == sr.srID).FirstOrDefault());
                });
                #endregion

                #region Delete
                lstSrDel.ForEach(sr =>
                                  bankSave.SalesRooms.Remove(bankSave.SalesRooms.Where(srr => srr.srID == sr.srID).FirstOrDefault())
                                  );
                #endregion
              }
              #endregion
              int n = dbContext.SaveChanges();
              transacction.Commit();
              return n;
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
