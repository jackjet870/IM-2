using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model.Helpers;
using IM.Model;
using System.Threading.Tasks;
using System.Data.Entity;
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
    //public async static Task<int> SaveGuestOpera(GuestOpera guestOpera, bool? insert = true)
    //{
    //  int status = 0;
    //  using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
    //  {
    //    if (insert != false)
    //    {
    //      //Si no existe, se agrega
    //      dbContext.GuestsOpera.Add(guestOpera);
    //      status = await dbContext.SaveChangesAsync();
    //    }
    //    else
    //    {
    //      //Si ya existe, se actualiza el valor
    //      //dbContext.GuestsOpera.Attach(guestOpera);
    //      dbContext.Entry(guestOpera).State = System.Data.Entity.EntityState.Modified;
    //      status = await dbContext.SaveChangesAsync();
    //    }
    //  }
      
    //  return status;
    //}
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
    //public async static Task<GuestOpera> GetGuestOpera(int guID)
    //{
    //  GuestOpera guestOpera = null;
    //  using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
    //  {
    //    guestOpera = await dbContext.GuestsOpera.SingleOrDefaultAsync(go => go.gogu == guID);
    //  }
    //  return guestOpera;
    //}
    #endregion
  }
}
