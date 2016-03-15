using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

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

    /// <summary>
    /// Función encargada de actualizar las fechas de temporada hasta el año ingresado
    /// </summary>
    /// <param name="yearServer"></param>
    /// <history>
    /// [vipacheco] 07/03/2016 Created
    /// </history>
    public static void UpdateSeasonDates(int yearServer)
    {
      using (var model = new IMEntities())
      {
        model.USP_OR_ActualizarFechasTemporadas(yearServer);
      }
    }
  }
}
