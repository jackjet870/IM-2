using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRReimpresionMotives
  {
    #region GetReimpresionMotives
    /// <summary>
    /// Obtiene registros del catalogo ReimpresionMotives
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos |1. Activos</param>
    /// <param name="reimpresionMotive">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo ReimpresionMotive</returns>
    /// <history>
    /// [emoguel] created 16/04/2016
    /// [emoguel] modified 28/06/2016 ---> Se volvió async
    /// </history>
    public async static Task<List<ReimpresionMotive>> GetReimpresionMotives(int nStatus = -1, ReimpresionMotive reimpresionMotive = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from rm in dbContext.ReimpresionMotives
                      select rm;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(rm => rm.rmA == blnStatus);
          }

          if (reimpresionMotive != null)//Verificamos si se tiene un objeto
          {
            if (reimpresionMotive.rmID > 0)//Filtro por ID
            {
              query = query.Where(rm => rm.rmID == reimpresionMotive.rmID);
            }

            if (!string.IsNullOrWhiteSpace(reimpresionMotive.rmN))//Filtro por Descripcion
            {
              query = query.Where(rm => rm.rmN.Contains(reimpresionMotive.rmN));
            }
          }
          return query.OrderBy(rm => rm.rmN).ToList();
        }
      });
    }
    #endregion


    #region UpdateReimpresionMotive
    /// <summary>
    /// Actualiza el motivo de reimpresion del recibo de regalos
    /// </summary>
    /// <param name="ReimpresionMotive"></param>
    /// <history>
    /// [vipacheco] 09/Junio/2016 Created
    /// </history>
    public static async Task UpdateReimpresionMotive(int ReceiptID, byte ReimpresionMotive)
    {
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          dbContext.USP_OR_UpdateGiftReceiptsReimpresionMotive(ReceiptID, ReimpresionMotive);
        }
      });
    }
    #endregion

    #region UpdateReimpresionNumber
    /// <summary>
    ///  Actualiza el contador de reimpresion del recibo de regalos
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 09/Junio/2016 Created
    /// </history>
    public static async Task UpdateReimpresionNumber(int ReceiptID)
    {
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          dbContext.USP_OR_UpdateGiftReceiptsReimpresionNumber(ReceiptID);
        }
      });
    } 
    #endregion

  }
}
