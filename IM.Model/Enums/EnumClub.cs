using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de los diferentes Clubes
  /// </summary>
  /// <history>
  /// [ecanul] 05/04/2016 Created
  /// [ecanul] 19/04/2016 Modificated Agregado Descripcion
  /// </history>
  public enum EnumClub
  {
    [Description("None")]
    None = 0,
    [Description("Premier")]
    PalacePremier = 1,
    [Description("Elite")]
    PalaceElite = 2,
    [Description("Legendary")]
    Legendary = 3
  }
}
