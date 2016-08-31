using IM.Model.Enums;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Classes
{

  public class ExcelFormatItemsList:List<ExcelFormatTable>
  {
    /// <summary>
    /// Agrega un elemento ExcelFormatTable a la lista actual.
    /// </summary>
    /// <param name="title"> Encabezado </param>
    /// <param name="propertyName"> Nombre de la columna en el datatable </param>
    /// <param name="format"> Tipo de formato </param>
    /// <param name="aligment"> Alineado del texto </param>
    /// <param name="showSubTotalWithCero"> Mostrar la cantidad 0 en los subtotales. </param>
    /// <param name="axis"> Define si se manipulará como fila, columna o valor. (Tabla pivot)</param>
    /// <param name="isVisible"> Visibilidad del campo </param>
    /// <param name="isGroup"> Define si será un grupo </param>
    /// <param name="function"> Funcion que se aplicará al subtotal o total del campo </param>
    /// <param name="isCalculated"> Define si la columna sera calculada al vuelo </param>
    /// <param name="formula"> Se ingresa la formula que se aplicará al campo </param>
    /// <param name="sort"> Ordenamiento de los filtros de columna (Tabla pivot) </param>
    /// <param name="superHeader"> Header superior </param>
    /// <param name="outLine"></param>
    /// <param name="compact"></param>
    /// <param name="showAll"></param>
    /// <param name="subTotalTop"></param>
    /// <param name="insertBlankRow"></param>
    /// <param name="dataFieldShowDataAs"></param>
    /// <history>
    /// [edgrodriguez]  24/Ago/2016 Created
    /// </history>
    public void Add(string title, string propertyName, EnumFormatTypeExcel format = EnumFormatTypeExcel.General, ExcelHorizontalAlignment aligment = ExcelHorizontalAlignment.Left, 
      bool showSubTotalWithCero = false, ePivotFieldAxis axis = ePivotFieldAxis.None, bool isVisible = true, bool isGroup = false,
      DataFieldFunctions function = DataFieldFunctions.None, bool isCalculated = false, string formula = "", eSortType sort = eSortType.None,
      string superHeader = "", bool outLine = false, bool compact = false, bool showAll = false,
      bool subTotalTop = false, bool insertBlankRow = false, EnumDataFieldShowDataAs dataFieldShowDataAs = EnumDataFieldShowDataAs.Normal, DataFieldFunctions AggregateFormat=DataFieldFunctions.None)
    {
      Add(new ExcelFormatTable()
      {
        //Commons Properties
        Title = title,
        PropertyName = propertyName,
        Format = format,
        Alignment = aligment,
        Axis = axis,
        IsVisible = isVisible,
        IsGroup = isGroup,
        Function = function,
        IsCalculated = isCalculated,
        Formula = formula,
        Sort = sort,
        SubtotalWithCero = showSubTotalWithCero,
        SuperHeader = superHeader,
        //TotalsRowFunction = totalsRowFunction,

        //Pivot Properties
        Outline = outLine,
        Compact = compact,
        ShowAll = showAll,
        SubtotalTop = subTotalTop,
        InsertBlankRow = insertBlankRow,
        DataFieldShowDataAs = dataFieldShowDataAs,
        AggregateFunction = AggregateFormat
      });
    }
  }
}
