using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado que indica si un reporte debe incluir los tours de cortesia y  rescate. 
  /// </summary>
  ///  <history>
  /// [erodriguez] 05/03/2016 Created
  /// </history>
  public enum EnumSaveCourtesyTours
  {
    [Description("Include Save Courtesy Tours")]
    sctIncludeSaveCourtesyTours,
    [Description("Exclude Save Courtesy Tours")]
    sctExcludeSaveCourtesyTours,
    [Description("Exclude Save Courtesy Tours Without Sales")]
    sctExcludeSaveCourtesyToursWithoutSales
  }
}
