using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRAgencies
  {
    #region GetAgencies

    /// <summary>
    /// Obtiene el catalogo de agencias
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    public static List<AgencyShort> GetAgencies(int status)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetAgencies(Convert.ToByte(status)).ToList();
      }
    }

    #endregion GetAgencies

    #region GetAgencies

    /// <summary>
    /// Obtiene el catalogo de agencias con todas sus columnas
    /// </summary>
    /// <param name="agency">Objeto con filtros extra, puede ser null</param>
    /// <param name="nStatus">-1. Todos los registros | 0.- Registros inactivos | 1. Registros Activos</param>
    /// <returns>Devuelve una lista de Agency</returns>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// </history>
    public static List<Agency> GetAgencies(Agency agency = null, int nStatus = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from ag in dbContext.Agencies
                    select ag;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(ag => ag.agA == blnStatus);
        }

        if (agency != null)//Validacion del objeto
        {
          if (!string.IsNullOrWhiteSpace(agency.agID))//Filtro por ID
          {
            query = query.Where(ag => ag.agID == agency.agID);
          }

          if (!string.IsNullOrWhiteSpace(agency.agN))//Filtro por Nombre(Descripcion)
          {
            query = query.Where(ag => ag.agN.Contains(agency.agN));
          }

          if (!string.IsNullOrWhiteSpace(agency.agse))//Filtro segment by agency
          {
            query = query.Where(ag => ag.agse == agency.agse);
          }
        }
        return query.OrderBy(ag => ag.agN).ToList();
      }
    }

    #endregion GetAgencies

    #region SaveAgency

    /// <summary>
    /// Guarda o actualiza un registro del catalogo Agencies
    /// </summary>
    /// <param name="agency">Entidad a guardar o actualizar</param>
    /// <param name="blnUpd">True. Actuliza la entidad | False. Agrega el registro a la BD</param>
    /// <param name="blnMarket">True. Actualiza el mercado de los huspedes</param>
    /// <param name="blnUnavailMots">True. Cambia el motivo de indisponibilidad</param>
    /// <returns>0. No se pudo guardar | 1.Se guardo | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    public static int SaveAgency(Agency agency, bool blnUpd, bool blnUnavailMots = false, bool blnMarket = false)
    {
      int nRes = 0;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        if (blnUpd)//Actualizar
        {
          #region actualizar

          if (blnMarket || blnUnavailMots)//Si es necesario abrir la transaccion
          {
            #region Transacction

            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
              try
              {
                dbContext.Entry(agency).State = System.Data.Entity.EntityState.Modified;//Se modifica agency

                #region unavailableMotives

                if (blnUnavailMots)
                {
                  #region Disponibilidad de huespedes

                  //actualizamos el motivo de indisponibilidad de los huespedes,
                  //la disponibilidad y la disponibilidad de origen de los huespedes
                  var lstGuestUM = (from guest in dbContext.Guests
                                    join agen in dbContext.Agencies
                                    on guest.guag equals agen.agID
                                    where agen.agID == agency.agID
                                    && guest.guPRAvail == null
                                    && guest.guInfo == false
                                    select new { guest, agen }).ToList();//Buscamos los huspedes a cambiar el motivo
                  //Actualizamos los registros
                  lstGuestUM.ForEach(guest =>
                  {
                    guest.guest.guum = agency.agum;
                    guest.guest.guAvail = (guest.guest.guCheckIn == true && guest.agen.agum == 0);
                    guest.guest.guOriginAvail = (guest.guest.guCheckIn == true && guest.agen.agum == 0);
                  });

                  #endregion Disponibilidad de huespedes

                  #region Disponibilidad por sistema de huespedes

                  //Listamos los huespedes a actualizar
                  var lstGuestBS = (from guest in dbContext.Guests
                                    join agen in dbContext.Agencies
                                    on guest.guag equals agen.agID
                                    where agen.agID == agency.agID
                                    select new { guest, agen }).ToList();

                  //Actualizamos la lista de huespedes
                  lstGuestBS.ForEach(guest => guest.guest.guAvailBySystem = (guest.guest.guCheckIn == true && guest.agen.agum == 0));

                  #endregion Disponibilidad por sistema de huespedes
                }

                #endregion unavailableMotives

                #region Markets

                if (blnMarket)
                {
                  var lstGuestMkt = (from guest in dbContext.Guests
                                     join agen in dbContext.Agencies
                                     on guest.guag equals agen.agID
                                     where agen.agID == agency.agID
                                     && guest.gumk != agen.agmk
                                     select guest).ToList();//Recuperamos los registros relacionados de guest
                  lstGuestMkt.ForEach(guest => guest.gumk = agency.agmk);//Actualizamos su valor
                }

                #endregion Markets

                nRes = dbContext.SaveChanges();//Guardar cambios
                transaction.Commit();//se hace el commit
              }
              catch
              {
                transaction.Rollback();
                nRes = 0;
              }
            }

            #endregion Transacction
          }
          else
          {//No es necesario iniciar la transaccion
            dbContext.Entry(agency).State = System.Data.Entity.EntityState.Modified;
            nRes = dbContext.SaveChanges();
          }

          #endregion actualizar
        }
        else//Insertar
        {
          #region Insertar

          Agency agencyVal = dbContext.Agencies.Where(ag => ag.agID == agency.agID).FirstOrDefault();
          if (agencyVal != null)//Se valida que no exista un registro con el mismo ID
          {
            nRes = 2;//Existe un registro con el mismo ID
          }
          else//No existe registro con el mismo ID
          {
            dbContext.Agencies.Add(agency);
            nRes = dbContext.SaveChanges();
          }

          #endregion Insertar
        }
        return nRes;
      }
    }

    #endregion SaveAgency

    #region GetAgenciesShortById

    /// <summary>
    ///Método para obtener una lista de Agencias por id.
    /// </summary>
    /// <param name="agIDList">Lista de id de Agency</param>
    /// <returns>List<AgencyShort></returns>
    /// <history>
    /// [aalcocer] 23/03/2016 Created
    /// </history>
    public static List<AgencyShort> GetAgenciesShortById(IEnumerable<string> agIDList)
    {
      List<AgencyShort> lstgetAgencies;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        lstgetAgencies = dbContext.Agencies.Where(x => agIDList.Contains(x.agID)).
          Select(x => new
          {
            x.agID,
            x.agN
          }).AsEnumerable().
          Select(x => new AgencyShort
          {
            agID = x.agID,
            agN = x.agN
          }).ToList();
      }
      return lstgetAgencies;
    }

    #endregion GetAgenciesShortById
  }
}