﻿using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    /// filtros[0]= LeadSources IDs
    /// filtros[1]= SaleRooms IDs
    /// filtros[2]= Countries IDs
    /// filtros[3]= Agencies IDs
    /// filtros[4]= Markets IDs
    /// </param>
    /// <returns>List<Model.RptPRStats></returns>
    /// <History>
    /// [erosado] 08/03/2016  created
    /// </History> 
    public static List<Model.RptPRStats> GetPRStats(DateTime dateFrom, DateTime dateTo, List<Tuple<string, string>> filtros)
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          dbContext.Database.CommandTimeout = 120;
          return dbContext.USP_OR_RptPRStats(dateFrom, dateTo, filtros[0].Item2,filtros[1].Item2, filtros[2].Item2, filtros[3].Item2, filtros[4].Item2).ToList();
        }
    }
  }
}