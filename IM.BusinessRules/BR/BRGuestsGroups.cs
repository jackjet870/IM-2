using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRGuestsGroups
  {

    #region GetGuestsGroups
    /// <summary>
    /// Obtiene el listado de Grupos de Huespedes segun la busqueda indicada
    /// </summary>
    /// <param name="guest">Model.Guest con datos para la busqueda</param>
    /// <param name="guestsGroup">Model.GuestsGroup con datos para la busqueda</param>
    /// <returns>Lista de GuestsGroup</returns>
    public static List<GuestsGroup> GetGuestsGroups(Guest guest, GuestsGroup guestsGroup)
    {
      List<GuestsGroup> lstGuestsGroups;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        lstGuestsGroups = new List<GuestsGroup>();
        if (guestsGroup.gxID != 0) //Si mandan el ID del Grupo
        {
          lstGuestsGroups = (from gg in dbContext.GuestsGroups
                             where gg.gxID == guestsGroup.gxID
                             select gg).Distinct().ToList();
        }
        else
        {
          if (guestsGroup.gxN != "" && guestsGroup.gxN != null)//Si envian el nombre del grupo
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
          else if (guest.guLastName1 != "" && guest.guLastName1 != null) //Si se manda Last o First name 1 o 2 (Solo se valida 1 porque en caso de tener 1, se tiene los 4
          {
            lstGuestsGroups = (from gu in dbContext.Guests
                               from ggi in gu.GuestsGroups
                               join gg in dbContext.GuestsGroups
                               on ggi.gxID equals gg.gxID
                               where gu.guLastName1.Contains(guest.guLastName1) || gu.guFirstName1.Contains(guest.guFirstName1)
                               || gu.guLastname2.Contains(guest.guLastname2) || gu.guFirstName2.Contains(guest.guFirstName2)
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
    /// <history>[ECANUL] 28-03-2016 Created</history>
    public static int SageGuestGroup(GuestsGroup guestGroup, bool action, List<Guest> lstGuests)
    {
      int res = 0;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {       
        if (action)
        {//Actualiza
          //Actualiza los datos directos del gurpo
          dbContext.Entry(guestGroup).State = EntityState.Modified;
          res = dbContext.SaveChanges();
          ///////////////////////// Integrantes del grupo
          List<Guest> lgu = new List<Guest>();
          //Obtiene el Grupo con sus respectivos integrantes
          GuestsGroup gg = dbContext.GuestsGroups.Include(gu => gu.Guests).ToList().Find(ggt => ggt.gxID == guestGroup.gxID);
          //GuestsGroup gg = dbContext.GuestsGroups.Include(gu => gu.Guests.Where(gut => gut.Groups == guestGroup).ToList(); // .Where(ggt => ggt.Guests == guestGroup).Single();
          lgu = gg.Guests.ToList();
          //Borrar los integrantes anteriores 
          lgu.ForEach(gu =>
          {
            dbContext.Entry(gu).State = EntityState.Detached;
          });
          //Se borran todas las relaciones entre los grupos y los integrantes
          gg.Guests.Clear();
          //Se guarda los cambios (Grupos sin integrantes)
          dbContext.Entry(gg).State = EntityState.Modified;
          dbContext.SaveChanges();

          //Agrega los autores enviados en la lista
          lstGuests.ForEach(gu =>
          {
            gg.Guests.Add(gu);
          });
          res = dbContext.SaveChanges();
        }
        else
        {//Llena los Guests que integtran el Guests Group
          lstGuests.ForEach(gu =>
          {
            guestGroup.Guests.Add(gu);
          });
          //Guarda Todo el la bd
          dbContext.GuestsGroups.Add(guestGroup);
          res = dbContext.SaveChanges();
        }
      }
      return res;
    } 
    #endregion

  }
}
