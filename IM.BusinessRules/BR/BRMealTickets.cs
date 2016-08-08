using IM.Model;
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
      List<MealTicket> lstResult = new List<MealTicket>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          #region Meal Ticket ID
          if (mealTicket != 0) // Busqueda solo con el Meal Ticket ID
            lstResult = dbContext.MealTickets.Where(x => x.meID == mealTicket).ToList();
          #endregion

          #region Folio
          if (lstResult.Count == 0 && folio != "") // Busqueda solo con el Folio
            lstResult = dbContext.MealTickets.Where(x => x.meFolios == folio).ToList();
          else if (folio != "")
            lstResult = lstResult.Where(x => x.meFolios == folio).ToList();
          #endregion

          #region RateType
          if (lstResult.Count == 0 && rateType != 0) // Busqueda solo con el Rate Type
            lstResult = dbContext.MealTickets.Where(x => x.mera == rateType).ToList();
          else if (rateType != 0)
            lstResult = lstResult.Where(x => x.mera == rateType).ToList();
          #endregion

          #region Fechas
          if (dateIni != null && dateFinal != null) // Busqueda solo con el rango de fechas
            lstResult = lstResult.Where(x => x.meD >= dateIni && x.meD <= dateFinal).ToList();
          #endregion
        }
      });

      return lstResult;
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
