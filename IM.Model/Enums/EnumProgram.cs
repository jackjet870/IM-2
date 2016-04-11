using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerador para el manejo de los diferentes programas.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 05/Mar/2016 Created
  /// [wtorres]      11/Abr/2016 Modified. Renombrado. Antes se llamaba EnumPrograms
  /// </history>
  public enum EnumProgram
    {
        [Description("ALL")]
        All,

        [Description("IH")]
        Inhouse,

        [Description("OUT")]
        Outhouse
    }
}