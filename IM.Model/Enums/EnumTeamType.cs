using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumera un Equipo, teamPRr = GS
  /// teamSalesmen = SA
  /// </summary>
  /// <history>
  /// [ecanul] 22/03/2016 Created 
  /// [ecanul] 18/04/2016 Modificated Agregada Descrpcion
  /// </history>
  public enum EnumTeamType
  {
    [Description("SA")]
    TeamSalesmen,
    [Description("GS")]
    TeamPRs
  }
}
