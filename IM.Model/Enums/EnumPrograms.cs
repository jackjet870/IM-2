using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerador para el manejo de los diferentes programas.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 05/Mar/2016 Created
  /// </history>
  public enum EnumPrograms
  {
    [Description("All Programs")]
    pgAll,
    [Description("Inhouse")]
    pgInhouse,
    [Description("Outhouse")]
    pgOuthouse
  }
}
