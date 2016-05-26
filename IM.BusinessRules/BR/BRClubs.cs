using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRClubs
  {
    #region GetClubs

    /// <summary>
    /// Devuelve registros del catalogo Clubs
    /// </summary>
    /// <param name="club">Objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. registros inactivos | 1.Registros Activos</param>
    /// <returns>Devuelve Lista de Clubs</returns>
    /// <history>
    /// [emoguel] created 03/11/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// [aalcocer] 25/05/2016  Modified. Se agregó asincronía
    /// </history>
    public static async Task<List<Club>> GetClubs(Club club = null, int nStatus = -1)
    {
      var result = new List<Club>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = (from cb in dbContext.Clubs
                       select cb);

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnEstatus = Convert.ToBoolean(nStatus);
            query = query.Where(cb => cb.clA == blnEstatus);
          }

          if (club != null)//Si se tiene un objeto
          {
            if (club.clID > 0)//Filtro por ID
            {
              query = query.Where(cb => cb.clID == club.clID);
            }

            if (!string.IsNullOrWhiteSpace(club.clN))//Filtro por nombre
            {
              query = query.Where(cb => cb.clN.Contains(club.clN));
            }
          }

          result = query.OrderBy(cb => cb.clN).ToList();
        }
      });
      return result;
    }

    #endregion GetClubs

    #region SaveClub

    /// <summary>
    /// Agrega|Actualiza un club
    /// </summary>
    /// <param name="club">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. guarda</param>
    /// <param name="lstAdd">Agencias a agregar</param>
    /// <param name="lstDel">Agencias a eliminar</param>
    /// <returns>0. No se guardó | -1. Existe un registro con el mismo ID | >0 Se guardó</returns>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    public static int SaveClub(Club club, bool blnUpdate, List<Agency> lstAdd, List<Agency> lstDel)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        int nRes = 0;
        using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
          try
          {
            #region Update

            if (blnUpdate)
            {
              dbContext.Entry(club).State = EntityState.Modified;
            }

            #endregion Update

            #region Add

            else
            {
              Club clubVal = dbContext.Clubs.Where(cl => cl.clID == club.clID).FirstOrDefault();
              if (clubVal != null)//Validar que no exista un registro con el mismo ID
              {
                return -1;
              }
              else
              {
                dbContext.Clubs.Add(club);
              }
            }

            #endregion Add

            #region Agencies Add

            if (lstAdd.Count > 0)
            {
              dbContext.Agencies.AsEnumerable().Where(ag => lstAdd.Any(agg => agg.agID == ag.agID)).ToList().ForEach(ag => ag.agcl = club.clID);
            }

            #endregion Agencies Add

            #region Agencies Delete

            if (lstDel.Count > 0)
            {
              dbContext.Agencies.AsEnumerable().Where(ag => lstDel.Any(agg => agg.agID == ag.agID)).ToList().ForEach(ag => ag.agcl = null);
            }

            #endregion Agencies Delete

            nRes = dbContext.SaveChanges();
            transaction.Commit();
            return nRes;
          }
          catch (Exception e)
          {
            transaction.Rollback();
            return 0;
          }
        }
      }
    }

    #endregion SaveClub
  }
}