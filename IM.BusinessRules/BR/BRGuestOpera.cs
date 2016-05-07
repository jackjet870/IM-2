using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model.Helpers;
using IM.Model;


namespace IM.BusinessRules.BR
{
  public class BRGuestOpera
  {
    #region SaveGuestOpera
    /// <summary>
    /// Agrega la informaicon de la reservacion de Opera  a la tabla GuestOpera
    /// </summary>
    /// <history>
    /// [michan] 19/04/2016 Created
    /// </history>
    public static int SaveGuestOpera(GuestOpera guestOpera, bool? insert = true)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        int status = 0;
        
        if (insert != false)
        {
          //Si no existe, se agrega
          dbContext.GuestsOpera.Add(guestOpera);
          status = dbContext.SaveChanges();
        }
        else
        {
          //Si ya existe, se actualiza el valor
          //dbContext.GuestsOpera.Attach(guestOpera);
          dbContext.Entry(guestOpera).State = System.Data.Entity.EntityState.Modified;
          status = dbContext.SaveChanges();
        }
        return status;
      }
    }
    #endregion

    #region GetGuestOpera
    /// <summary>
    /// Obtiene un huesped en la tabla de GuestOpera por su Pk(gogu)
    /// </summary>
    /// <param name="guID">ID del huesped</param>
    /// <returns>Retorna el huesped si se encuentra, si no retorna null</returns>
    /// <history>
    /// [michan] 19/04/2016 Created
    /// </history>
    public static GuestOpera GetGuestOpera(int guID)
    {

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.GuestsOpera.Where(go => go.gogu == guID).FirstOrDefault();
      }
    }
    #endregion
  }
}
