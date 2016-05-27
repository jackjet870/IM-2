using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para indicar si un reporte debe incluir las invitaciones externas.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 05/Mar/2016 Created
  /// [aalcocer]   27/05/2016 Modified. Se cambian los nombres
  /// </history>
  public enum EnumExternalInvitation
  {
    [Description("Include External Invitation")]
    Include,

    [Description("Exclude External Invitation")]
    Exclude,

    [Description("Only External Invitation")]
    Only
  }
}