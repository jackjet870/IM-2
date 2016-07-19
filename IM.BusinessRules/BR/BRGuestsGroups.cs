using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using IM.Model;
using IM.Model.Helpers;
using IM.Base;

namespace IM.BusinessRules.BR
{
  public class BRGuestsGroups
  {
     #region GetGuestsGroup
    /// <summary>
    /// Obtiene un GuestsGroup segun el Id Mandado, Incluye Los guests integrantes del grupo
    /// </summary>
    /// <param name="id">id del grupo a buscar</param>
    /// <history>
    /// [ecanul] 20/06/2016 Created
    /// </history>
    public async static Task<GuestsGroup> GetGuestsGroup(int id)
    {
      GuestsGroup gGroup = new GuestsGroup();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          gGroup = (from gg in dbContext.GuestsGroups
                    where gg.gxID == id
                    select gg).Include("Guests").ToList().FirstOrDefault();
        }
      });
      return gGroup;
    } 
    #endregion

    #region GetGuestsGroups
    /// <summary>
    /// Obtiene el listado de Grupos de Huespedes segun la busqueda indicada
    /// </summary>
    /// <param name="guest">Model.Guest con datos para la busqueda</param>
    /// <param name="guestsGroup">Model.GuestsGroup con datos para la busqueda</param>
    /// <history>
    /// [ecanul]  28-03-2016 Created
    /// [ecanul] 17/06/2016 Modified. Implementado Asincronia
    /// </history>
    public async static Task<List<GuestsGroup>> GetGuestsGroups(Guest guest, GuestsGroup guestsGroup)
    {
      List<GuestsGroup> lstGuestsGroups = new List<GuestsGroup>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
            //lstGuestsGroups = new List<GuestsGroup>();
            if (guestsGroup.gxID != 0) //Si mandan el ID del Grupo
            {
              lstGuestsGroups = (from gg in dbContext.GuestsGroups
                                 where gg.gxID == guestsGroup.gxID
                                 select gg).Distinct().ToList();
            }
            else
            {
              if (!string.IsNullOrEmpty(guestsGroup.gxN))//Si envian el nombre del grupo
              {
                lstGuestsGroups = (from gu in dbContext.Guests
                                   from ggi in gu.GuestsGroups
                                   join gg in dbContext.GuestsGroups
                                   on ggi.gxID equals gg.gxID
                                   where gg.gxN.Contains(guestsGroup.gxN)
                                   && (gu.guCheckInD >= guest.guCheckInD && gu.guCheckInD <= guest.guCheckOutD)
                                   select gg).Distinct().ToList();
              }
              else if (guest.guID != 0) //Si se manda el ID de Guest
              {
                lstGuestsGroups = (from gu in dbContext.Guests
                                   from ggi in gu.GuestsGroups
                                   join gg in dbContext.GuestsGroups
                                   on ggi.gxID equals gg.gxID
                                   where gu.guID == guest.guID
                                   && (gu.guCheckInD >= guest.guCheckInD && gu.guCheckInD <= guest.guCheckOutD)
                                   select gg).Distinct().ToList();
              }
            //Si se manda Last o First name 1 o 2 (Solo se valida LastName1 porque es el unico que recibe informacuion de la interfaz
              else if (!string.IsNullOrEmpty( guest.guLastName1)) 
              {
                lstGuestsGroups = (from gu in dbContext.Guests
                                   from ggi in gu.GuestsGroups
                                   join gg in dbContext.GuestsGroups
                                   on ggi.gxID equals gg.gxID
                                   //se usa solo guLastName1 por que es el campo en el que se manda todo el texto
                                   where gu.guLastName1.Contains(guest.guLastName1) || gu.guFirstName1.Contains(guest.guLastName1)
                                   || gu.guLastname2.Contains(guest.guLastName1) || gu.guFirstName2.Contains(guest.guLastName1)
                                   && (gu.guCheckInD >= guest.guCheckInD && gu.guCheckInD <= guest.guCheckOutD)
                                   select gg).Distinct().ToList();
              }
              else//Si no puso nada en los texbox se hace una busqueda unicamente entre el rango de fechas
              {
                lstGuestsGroups = (from gu in dbContext.Guests
                                   from ggi in gu.GuestsGroups
                                   join gg in dbContext.GuestsGroups
                                   on ggi.gxID equals gg.gxID
                                   //Se usa guCheckOutD que trae la "guest" pero en realidad deberia ser otro rango de fechas para guCheckInD
                                   where gu.guCheckInD >= guest.guCheckInD && gu.guCheckInD <= guest.guCheckOutD
                                   select gg).Distinct().ToList();
              }
            }
          }
      });
      return lstGuestsGroups;
    }

    #endregion

    #region SaveGuestsGroup
    /// <summary>
    /// Guarda un GuestsGroup
    /// </summary>
    /// <param name="guestGroup">Datos del GuestsGroup a guardar</param>
    /// <param name="action">true Modifica | false Agrega</param>
    /// <returns>Int con valor guardado o no</returns>
    /// <history>
    /// [ECANUL] 28-03-2016 Created
    /// [ecaul] 21/06/2016 Modified, Corregido error de guardado. Implementado Asincronia
    /// </history>
    public async static Task<int> SaveGuestGroup(GuestsGroup guestGroup, bool action, List<Guest> lstGuestsAdd, List<Guest> lstGuestsDelete)
    {
      int res = 0;
      res = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transact = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              GuestsGroup gGroup = null;
              var ggID = guestGroup.gxID;
              //Agrega o modifica El Grupo (SOLO LA INFORMACION DEL GRUPO)
              #region GuestsGroups
              #region Update
              if (action)
              {
                gGroup = dbContext.GuestsGroups.Where(gg => gg.gxID == ggID).Include("Guests").FirstOrDefault();
                gGroup.gxID = guestGroup.gxID;
                gGroup.gxN = guestGroup.gxN;
              }

              #endregion
              #region Add
              else
              {
                gGroup = dbContext.GuestsGroups.Where(gg => gg.gxID == ggID).Include("Guests").FirstOrDefault();
                if (gGroup != null)
                  return -1;
                else
                {
                  gGroup = guestGroup;
                  dbContext.GuestsGroups.Add(gGroup);
                }
              }
              #endregion
              #endregion
              //Agrega o elimina la relacion Guest <=> GuestsGroup
              #region GuestsGroupsIntegrants
              if (gGroup != null)
              {
                //Agrega los Guest Nuevos al Grupo
                lstGuestsAdd.ForEach(gu => {
                  gu.guGroup = true;
                  dbContext.Entry(gu).State = EntityState.Modified;
                  gGroup.Guests.Add(gu);                  
                  }
                //dbContext.Guests
                );

                //Elimina los Guests Eliminados del Grupo
                #region Delete
                lstGuestsDelete.ForEach(gu =>
                {
                  gGroup.Guests.Remove(gGroup.Guests.Where(gd=>gd.guID==gu.guID).FirstOrDefault());
                });

                lstGuestsDelete.ForEach(async gu =>
                {
                  gu.guGroup = false;
                  await BREntities.OperationEntity(gu, Model.Enums.EnumMode.edit);
                });

                #endregion
              }
              #endregion
              int i = dbContext.SaveChanges();
              transact.Commit();
              return i;
            }
            catch
            {
              transact.Rollback();
              return 0;
            }
          }
        }
      });
      return res;
    } 
    #endregion

  }
}
