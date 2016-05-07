using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;
using System;

namespace IM.Model.Classes
{
  /// <summary>
  /// Clase que sirve para definir el formato de las columnas a la hora de generar un Reporte
  /// </summary>
  /// <history>
  /// [erosado]       14/03/2016 Created
  /// [aalcocer]      28/03/2016 Modified. Se agregan mas propiedades y se establecen por default algunas propiedades
  /// [edgrodriguez]  12/04/2016 Modified. Se agregan las propiedaes IsGroup, AggregateFunction
  /// [edgrodriguez]  15/04/2016 Modified. Se agrega la propiedad IsCalculated
  /// [aalcocer]      18/04/2016 Modified. Se agrega la propiedad TotalsRowFunction
  /// </history>
  public class ExcelFormatTable
  {
    public string Title { get; set; }
    public EnumFormatTypeExcel Format { get; set; } = EnumFormatTypeExcel.General;
    public bool SubtotalWithCero { get; set; } = false;
    public ExcelHorizontalAlignment Alignment { get; set; } = ExcelHorizontalAlignment.Left;
    public DataFieldFunctions Function { get; set; } = DataFieldFunctions.None;
    public eSubTotalFunctions SubTotalFunctions { get; set; } = eSubTotalFunctions.None;
    public bool SubtotalTop { get; set; } = false;
    public bool Outline { get; set; } = false;
    public bool Compact { get; set; } = false;
    public ePivotFieldAxis Axis { get; set; } = ePivotFieldAxis.None;
    public int Order { get; set; }
    public string Formula { get; set; }
    public bool ShowAll { get; set; } = false;
    public eSortType Sort { get; set; } = eSortType.None;
    public string PropertyName { get; set; }
    public string SuperHeader { get; set; }
    public bool IsGroup { get; set; } = false;
    public DataFieldFunctions AggregateFunction { get; set; } = DataFieldFunctions.None;
    public bool IsCalculated { get; set; }
    public RowFunctions TotalsRowFunction { get; set; } = RowFunctions.None;

    public bool InsertBlankRow { get; set; } = false;
  }
}