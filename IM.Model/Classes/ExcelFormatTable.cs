using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System;

namespace IM.Model.Classes
{
  /// <summary>
  /// Clase que sirve para definir el formato de las columnas a la hora de generar un Reporte
  /// </summary>
  /// <history>
  /// [erosado]  14/03/2016  Created
  /// [aalcocer] 28/03/2016 Modified. Se agregan mas propiedades y se establecen por default algunas propiedades
  /// </history>
  public class ExcelFormatTable
  {
    public string Title { get; set; }
    public EnumFormatTypeExcel Format { get; set; } = EnumFormatTypeExcel.General;
    public ExcelHorizontalAlignment Alignment { get; set; } = ExcelHorizontalAlignment.Left;
    public DataFieldFunctions Function { get; set; } = DataFieldFunctions.None;
    public eSubTotalFunctions SubTotalFunctions { get; set; } = eSubTotalFunctions.None;
    public bool SubtotalTop { get; set; } = false;
    public bool Outline { get; set; } = false;
    public bool Compact { get; set; } = false;
    public ePivotFieldAxis Axis { get; set; } = ePivotFieldAxis.Row;
    public int Order { get; set; }
    public string Formula { get; set; }
    public bool showAll { get; set; } = false;
    public eSortType Sort { get; set; } = eSortType.None;
    public string PropertyName { get; set; }
  }
}