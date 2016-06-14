using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// Clase manejador de reglas de tipo Seasons
  /// </summary>
  /// <history>
  /// [vipacheco] 07/03/2016 Created
  /// </history>
  public class BRSeasons
  {
    #region UpdateSeasonDates
    /// <summary>
    /// Función encargada de actualizar las fechas de temporada hasta el año ingresado
    /// </summary>
    /// <param name="yearServer"></param>
    /// <history>
    /// [vipacheco] 07/03/2016 Created
    /// </history>
    public async static Task UpdateSeasonDates(int yearServer)
    {
      await Task.Run(() =>
      {
        using (var model = new IMEntities(ConnectionHelper.ConnectionString))
        {
          model.USP_OR_ActualizarFechasTemporadas(yearServer);
        }
      });
    } 
    #endregion
  }
}
