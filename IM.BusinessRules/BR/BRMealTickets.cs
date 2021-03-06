﻿using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRMealTickets
  {

    #region GetMealTickets
    /// <summary>
    /// Función para la busqueda de un Meal Ticket de acuerdo a los parametros especificados!
    /// </summary>
    /// <param name="mealTicket"> ID del Meal Ticket </param>
    /// <param name="folio"> ID del Folio </param>
    /// <param name="rateType"> ID del Rate Type </param>
    /// <param name="dateIni"> Fecha Inicial del rango a evaluar </param>
    /// <param name="dateFinal"> Fecha Final del rango a evaluar </param>
    /// <returns> Lista de tipo MealTicket </returns>
    /// <history>
    /// [vipacheco] 18/03/2016 Created
    /// [vipacheco] 29/Julio/2016 Modified --> Se agrego asyncronia
    /// </history>
    public async static Task<List<MealTicket>> GetMealTickets(int mealTicket = 0, string folio = "", int rateType = 0, DateTime? dateIni = null, DateTime? dateFinal = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          IQueryable<MealTicket> query = Enumerable.Empty<MealTicket>().AsQueryable();

          #region Meal Ticket ID
          if (mealTicket != 0) // Busqueda solo con el Meal Ticket ID
            query = dbContext.MealTickets.Where(x => x.meID == mealTicket);
          #endregion

          #region Folio
          if (!query.Any() && folio != "") // Busqueda solo con el Folio
            query = dbContext.MealTickets.Where(x => x.meFolios == folio);
          else if (folio != "")
            query = query.Where(x => x.meFolios == folio);
          #endregion

          #region RateType
          if (!query.Any() && rateType != 0) // Busqueda solo con el Rate Type
            query = dbContext.MealTickets.Where(x => x.mera == rateType);
          else if (rateType != 0)
            query = query.Where(x => x.mera == rateType);
          #endregion

          #region Fechas
          if (dateIni != null && dateFinal != null) // Busqueda solo con el rango de fechas
            query = query.Where(x => x.meD >= dateIni && x.meD <= dateFinal);
          #endregion

          return query.ToList();
        }
      });
    }
    #endregion

    #region GetMealTicket
    /// <summary>
    /// Funcion para obtener la informacion de un MealTicket en especifico.
    /// </summary>
    /// <param name="guestID"></param>
    /// <returns> Un objeto de tipo MealTicket </returns>
    /// <history>
    /// [vipacheco] 29/03/2016 Created
    /// </history>
    public static List<MealTicket> GetMealTickets(int guestID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.MealTickets.Where(x => x.megu == guestID).ToList();
      }
    }
    #endregion

    #region InsertNewMealTicket
    /// <summary>
    /// Función que agrega un nuevo Meal Ticket
    /// </summary>
    /// <param name="pMealTicket"></param>
    /// <history>
    /// [vipacheco] 01/04/2016 Created
    /// </history>
    public static void InsertNewMealTicket(MealTicket pMealTicket)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbContext.MealTickets.Add(pMealTicket);

        dbContext.SaveChanges();
      }
    }
    #endregion

    #region UpdateMealTicket
    /// <summary>
    /// Función que actualiza un Meal Ticket creado anteriomente
    /// </summary>
    /// <param name="pMealTicket"></param>
    /// <history>
    /// [vipacheco] 01/04/2016 Created
    /// </history>
    public static void UpdateMealTicket(MealTicket pMealTicket)
    {
      using (var dbcontext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbcontext.Entry(pMealTicket).State = System.Data.Entity.EntityState.Modified;

        dbcontext.SaveChanges();
      }
    } 
    #endregion
  }
}
