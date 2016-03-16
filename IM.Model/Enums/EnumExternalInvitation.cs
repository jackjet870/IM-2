using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para indicar si un reporte debe incluir las invitaciones externas.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 05/Mar/2016 Created
  /// </history>
  public enum EnumExternalInvitation
  {
    [Description("Exclude External Invitation")]
    extExclude,
    [Description("Include External Invitation")]
    extInclude,
    [Description("Only External Invitation")]
    extOnly
  }
}
