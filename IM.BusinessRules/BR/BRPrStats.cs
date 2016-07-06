using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRPRStats
  {

    /// <summary>
    /// Obtiene el reporte PR Statistics
    /// </summary>
    /// <param name="dateFrom">FechaInicio</param>
    /// <param name="dateTo">Fecha Fin</param>
    /// <param name="filtros">
    /// filtros[0]= LeadSources IDs||filtros[1]= SaleRooms IDs||filtros[2]= Countries IDs||filtros[3]= Agencies IDs||filtros[4]= Markets IDs
    /// </param>
    /// <returns>List<Model.RptPRStats></returns>
    /// <History>
    /// [erosado] 08/03/2016  Created
    /// [erosado] 12/04/2016  Modified  Se agrego el tiempo de la conexion en Properties.Settings
    /// [erosado] 04/07/2016  Se agregó Async.
    /// </History> 
    public async static Task<List<RptPRStats>> GetPRStats(DateTime dateFrom, DateTime dateTo, List<Tuple<string, string>> filtros)
    {
      List<RptPRStats> result = new List<RptPRStats>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_RptPRStats;
          result = dbContext.USP_OR_RptPRStats(dateFrom, dateTo, filtros[1].Item2, filtros[2].Item2, filtros[3].Item2, filtros[4].Item2, filtros[5].Item2).ToList();
        }
      });
      return result;
    }
  }
}
