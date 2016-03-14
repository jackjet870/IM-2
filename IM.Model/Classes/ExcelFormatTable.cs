using IM.Model.Enums;
using OfficeOpenXml.Style;

namespace IM.Model.Classes
{

  /// <summary>
  /// Clase que sirve para definir el formato de las columnas a la hora de generar un Reporte
  /// </summary>
  /// <history>
  /// [erosado] 14/03/2016  Created
  /// </history>
  public class ExcelFormatTable
  {
    public string Title { get; set; }
    public EnumFormatTypeExcel Format { get; set; }
    public ExcelHorizontalAlignment Alignment { get; set; }

  }
}
