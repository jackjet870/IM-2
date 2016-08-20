using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Classes
{
  /// <summary>
  ///   Clase para el manejo de rango de fechas no asignadas
  /// </summary>
  /// <history>
  ///   [vku] 27/Jul/2016 Created
  /// </history>
  public class ItemSeasonDates
  {
    public int Num { get; set; }
    public DateTime Start { get; set; }
    public DateTime Finish { get; set; }
  }
}
