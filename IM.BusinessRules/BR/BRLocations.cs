﻿using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System;

namespace IM.BusinessRules.BR
{
  public class BRLocations
  {
    #region GetLocationsByUser

    /// <summary>
    /// Obtiene las locaciones de un usuario
    /// </summary>
    /// <param name="user">Usuario </param>
    /// <param name="programs"> Programa o default('ALL') </param>
    /// <history>
    /// [wtorres]  07/Mar/2016 Created
    /// </history>
    public static List<LocationByUser> GetLocationsByUser(string user="All", string programs="All")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetLocationsByUser(user, programs).ToList();
      }
    }

    #endregion

    #region GetLocations
    /// <summary>
    /// Obtiene registros del catalogo Locations
    /// </summary>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1.Registros Activos</param>
    /// <param name="location">Objeto con filtros adicionales</param>
    /// <param name="blnTeamsLog">para devolver la consulta que llena el combo en TeamsLog</param>
    /// <returns>Lista de tipo Location</returns>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// [emoguel] modified 27/04/2016 Se agregó el parametro blnTeamsLog para cargar el combobos de Locations en teamLog
    /// </history>
    public static List<Location> GetLocations(int nStatus=-1,Location location=null, bool blnTeamsLog=false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from lo in dbContext.Locations
                    select lo;
        if (blnTeamsLog)
        {
          query = from tg in dbContext.TeamsGuestServices
                  from lo in dbContext.Locations.Where(lo => lo.loID == tg.tglo).DefaultIfEmpty().Distinct()
                  select lo;
        }
        if (nStatus!=-1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(lo => lo.loA == blnEstatus);
        }
        #region Filtros adicionales

        if (location != null)//verificar si se tiene un objeto
        {
          if (!string.IsNullOrWhiteSpace(location.loID))//filtro por ID
          {
            query = query.Where(lo => lo.loID == location.loID);
          }

          if (!string.IsNullOrWhiteSpace(location.loN))//Filtro por Descripcion
          {
            query = query.Where(lo => lo.loN.Contains(location.loN));
          }

          if (!string.IsNullOrWhiteSpace(location.losr))//Filtro por sales room
          {
            query = query.Where(lo => lo.losr == location.losr);
          }

          if (!string.IsNullOrWhiteSpace(location.lolc))//Filtro por categoria
          {
            query = query.Where(lo => lo.lolc == location.lolc);
          }
        }

        #endregion
        return query.OrderBy(lo => lo.loN).ToList();
      }
    }

    #endregion
  }
}
