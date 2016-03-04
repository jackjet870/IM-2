using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRGuest
  {
    /// <summary>
    /// Obtiene el catalogo de Guests Arrivals
    /// </summary>
    /// <param name="Date">Fecha </param>
    /// <param name="LeadSource">LeadSource </param>
    /// <param name="Markets">Mercado </param>
    /// <param name="Available">Available </param>
    /// <param name="Contacted">Contacted</param>
    /// <param name="Invited">Invited</param>
    /// <param name="OnGroup">OnGroup</param>
    /// <returns>List<Model.GetGuestsArrivals></returns>
    public static List<Model.GetGuestsArrivals> GetGuestsArrivals(DateTime Date, string LeadSource, string Markets, int Available, int Contacted, int Invited, int OnGroup)
    {
      try
      {
        using (var g = new Model.IMEntities())
        {
          return g.USP_OR_GetArrivals(Date, LeadSource, Markets, Available, Contacted, Invited, OnGroup).ToList<Model.GetGuestsArrivals>();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Obtiene el catalogo de Guests Availables
    /// </summary>
    /// <param name="Date">Fecha </param>
    /// <param name="LeadSource">LeadSource </param>
    /// <param name="Markets">Mercado </param>
    /// <param name="Contacted">Contacted</param>
    /// <param name="Invited">Invited</param>
    /// <param name="OnGroup">OnGroup</param>
    /// <returns>List<Model.GetGuestsAvailables></returns>
    public static List<Model.GetGuestsAvailables> GetGuestsAvailables(DateTime Date, string LeadSource, string Markets, int Contacted, int Invited, int OnGroup)
    {
      try
      {
        using (var g = new Model.IMEntities())
        {
          return g.USP_OR_GetAvailables(Date, LeadSource, Markets, Contacted, Invited, OnGroup).ToList<Model.GetGuestsAvailables>();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Obtiene el catalogo de Guests Premanifests
    /// </summary>
    /// <param name="Date">Fecha </param>
    /// <param name="LeadSource">LeadSource </param>
    /// <param name="Markets">Mercado </param>
    /// <param name="OnGroup">OnGroup</param>
    /// <returns>List<Model.GetGuestsAvailables></returns>
    public static List<Model.GetGuestsPremanifest> GetGuestsPremanifests(DateTime Date, string LeadSource, string Markets, int OnGroup)
    {
      try
      {
        using (var g = new Model.IMEntities())
        {
          return g.USP_OR_GetPremanifest(Date, LeadSource, Markets, OnGroup).ToList<Model.GetGuestsPremanifest>();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Obtiene los Mail Outs disponibles de un Lead Source
    /// </summary>
    /// <param name="leadSourceID">Clave del Lead Source</param>
    /// <param name="guCheckInD">Fecha de llegada</param>
    /// <param name="guCheckOutD">Fecha de salida</param>
    /// <param name="guBookD">Fecha de booking</param>
    /// <returns> List<GuestsMailOuts></returns>
    ///  <history>
    ///   [aalcocer] 24/02/2016 Created
    /// </history>
    public static List<Model.GuestsMailOuts> GetGuestsMailOuts(string leadSourceID, DateTime guCheckInD, DateTime guCheckOutD, DateTime guBookD)
    {
      using (var g = new Model.IMEntities())
      {
        return g.USP_OR_GetGuestsMailOuts(leadSourceID, guCheckInD, guCheckOutD, guBookD).ToList();
      }
    }
  }
}