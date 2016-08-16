using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado de los diferentes modulos de invitacion
  /// </summary>
  /// <history>
  /// [erosado] 10/Ago/2016 Created
  /// [wtorres] 16/Ago/2016 Modified. Agregue temporalmente los valores Animation, Regen y External
  /// </history>
  public enum EnumModule
  {
    InHouse,
    OutHouse,
    Host,

    //TODO: Eliminar los valores Animation, Regen y External cuando se elimine el formulario de invitacion de Chairez
    Animation,
    Regen,
    External
  }
}
