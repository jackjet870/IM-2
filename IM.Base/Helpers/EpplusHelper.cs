using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Microsoft.Win32;
using IM.Model.Enums;
using IM.Model.Classes;
using OfficeOpenXml.Table.PivotTable;
using System.Linq;
using System.Xml.Linq;
using System.Xml;

namespace IM.Base.Helpers
{
  public class EpplusHelper
  {
    #region SaveExcel
    /// <summary>
    /// Guarda un ExcelPackage en una ruta escogida por el usuario
    /// </summary>
    /// <param name="pk">ExcelPackage</param>
    /// <param name="suggestedName">Nombre sugerido</param>
    /// <returns>Ruta de archivo nuevo (FileInfo)</returns>
    /// <history>
    /// [erosado] 11/03/2016  Created
    /// </history>
    public static FileInfo SaveExcel(ExcelPackage pk, string suggestedName)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Excel files(*.xlsx or *.xls) | *.xlsx; *.xls;";
      saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      saveFileDialog.DefaultExt = ".xlsx";
      saveFileDialog.FileName = suggestedName;
      if (saveFileDialog.ShowDialog() == true)
      {
        FileInfo name = new FileInfo(saveFileDialog.FileName);
        pk.SaveAs(name);
        pk.Dispose();
        return name;
      }
      else
      {
        pk.Dispose();
        return null;
      }
    }

    #endregion

    #region CreateRptExcelWithOutTemplate
    /// <summary>
    /// Crea un reporte en excel que tiene Filtros, Nombre de reporte, contenido, datos de impresion y nombre del sistema
    /// </summary>
    /// <param name="filter">Tupla de filtros: item1 = Nombre del filtro - item2 =filtros separados por comas (bp,mbp)
    /// si seleccionan todos poner en item2 =ALL
    /// </param>
    /// <param name="dt">DataTable con la informacion del reporte</param>
    /// <param name="reportName">Tupla que contiene item1=Nombre del reporte Item2= IM.Base.DateHelper.DateRange </param>
    /// <param name="formatColumns">Lista de ExcelFormatTable donde definimos ("Titulo de la columna",IM.Model.Enums.EnumFormatTypeExcel, OfficeOpenXml.Style.Enum.ExcelHorizontalAlignment) </param>
    /// <returns>FileInfo con el path para abrir el excel</returns>
    /// <history>
    /// [erosado] 12/Mar/2016  Created.
    /// [erosado] 14/Mar/2016  Modified  Se Agrego el parametro formatColumns.
    /// [erosado] 17/Mar/2016  Modified  Se Agrego que permite tipos Boleanos y Date formateados
    /// </history>
    public static FileInfo CreateRptExcelWithOutTemplate(List<Tuple<string, string>> filter, DataTable dt, Tuple<string, string> reportName,
      List<ExcelFormatTable> formatColumns)
    {
      FileInfo pathFinalFile = null;
      ExcelPackage pk = new ExcelPackage();
      //Preparamos la hoja donde escribiremos
      ExcelWorksheet ws = pk.Workbook.Worksheets.Add(reportName.Item1);

      //Numero de filtros
      int filterNumber = filter.Count;

      //Numero de filas del contenido del datatable
      int dtRowsNumber = dt.Rows.Count;
      //Numero de Columnas del contenido del datatable
      int dtColumnsNumber = dt.Columns.Count;

      if (filter.Count > 0)
      {
        //Insertamos etiqueta de filtros
        ws.Cells[1, dtColumnsNumber].Value = "Filters";
        ws.Cells[1, dtColumnsNumber].Style.Font.Bold = true;
        ws.Cells[1, dtColumnsNumber].Style.Font.Size = 14;
        //Insertamos las filas que necesitamos para los filtros
        ws.InsertRow(2, filter.Count);

        int cFiltros = 1;
        foreach (Tuple<string, string> item in filter)
        {
          cFiltros++;

          ws.Cells[1, dtColumnsNumber - 1].Value = item.Item1;
          ws.Cells[1, dtColumnsNumber - 1].Style.Font.Bold = true;
          ws.Cells[1, dtColumnsNumber].Value = item.Item2;

        }
      }
      ws.Cells[1, 1].Value = reportName.Item1;
      ws.Cells[1, 1].Style.Font.Bold = true;
      ws.Cells[1, 1].Style.Font.Size = 14;
      ws.Cells[1,1,1,1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
      ws.Cells[2, 1].Value = reportName.Item2;
      ws.Cells[2, 1,2,1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
      ws.Cells[1, 1, 1, 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;

      //Insertamos las filas que vamos a necesitar empezando en la fila 7
      ws.InsertRow(filterNumber + 2, dt.Rows.Count+1);

      //Agregamos los Headers de la Tabla
      int contColum = 0;
      foreach (ExcelFormatTable item in formatColumns)
      {
        contColum++;
        ws.Cells[filterNumber + 2, contColum].Value = item.Title;

        switch (item.Format)
        {
          case EnumFormatTypeExcel.General:
          break;
          case EnumFormatTypeExcel.Percent:
            ws.Column(contColum).Style.Numberformat.Format="0.0 %";
            break;
          case EnumFormatTypeExcel.Currency:
            ws.Column(contColum).Style.Numberformat.Format = "$#,##0.00";
            break;
          case EnumFormatTypeExcel.Number:
            ws.Column(contColum).Style.Numberformat.Format = "#";
            break;
          case EnumFormatTypeExcel.DecimalNumber:
            ws.Column(contColum).Style.Numberformat.Format = "0.00";
            break;
          case EnumFormatTypeExcel.Boolean:
            
            ws.Cells[filterNumber + 3, contColum, filterNumber + 3 + dtRowsNumber, contColum].Style.Font.Name = "Wingdings";
            break;
          case EnumFormatTypeExcel.Date:
            ws.Column(contColum).Style.Numberformat.Format = "dd/MM/yyyy";
            break;
          default:
            break;
        }
        ws.Column(contColum).Style.HorizontalAlignment = item.Alignment;
        ws.Cells[filterNumber + 2, contColum].Style.Font.Bold= true;
        ws.Cells[filterNumber + 2, contColum].Style.Font.Size =14;
      }
      //Borde de rango de columnas (titulos de la tabla)
      ws.Cells[filterNumber + 2, 1, filterNumber + 2,dtColumnsNumber].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Hair);

      //Agregamos el contenido empezando en la fila 8
      ws.Cells[filterNumber + 3, 1].LoadFromDataTable(dt, false, OfficeOpenXml.Table.TableStyles.Medium2);

      //Agregamos los datos de impresion
      ws.Cells[filterNumber + 3 + dtRowsNumber + 1, 1].Value = "Date Print";
      ws.Cells[filterNumber + 3 + dtRowsNumber + 1, 1].Style.Font.Bold = true;
      ws.Cells[filterNumber + 3 + dtRowsNumber + 1, 2].Value = string.Format(DateTime.Now.ToShortDateString(), "yyyy-MM-dd");
      ws.Cells[filterNumber + 3 + dtRowsNumber + 2, 1].Value = "Time Print";
      ws.Cells[filterNumber + 3 + dtRowsNumber + 2, 1].Style.Font.Bold = true;
      ws.Cells[filterNumber + 3 + dtRowsNumber + 2, 2].Value = string.Format(DateTime.Now.ToShortTimeString(), "hh:mm:ss");

      //Agregamos el nombre del sistema
      ws.Cells[filterNumber + 3 + dtRowsNumber + 1, dtColumnsNumber].Value = "Intelligence Marketing";
      ws.Cells[filterNumber + 3 + dtRowsNumber + 1, dtColumnsNumber].Style.Font.Bold = true;
      ws.Cells[filterNumber + 3 + dtRowsNumber + 1, dtColumnsNumber].Style.Font.Size = 14;

      ws.Cells[1, 1, (filterNumber + 3 + dtRowsNumber + 4), dtColumnsNumber].AutoFitColumns(); //Auto Ajuste de columnas de acuerdo a los headers

      string suggestedFilaName = string.Concat(reportName.Item1, "_", reportName.Item2);
      pathFinalFile = SaveExcel(pk, suggestedFilaName);

      if (pathFinalFile != null)
      {
        return pathFinalFile;
      }
      else
      {
        return null;
      }
    }

    #endregion

    #region CreatePivotRptExcel
    /// <summary>
    /// 
    /// </summary>
    /// <param name="IsPivot">Bandera para indicar si se requiere una tabla tipo Pivot.</param>
    /// <param name="filters">Filtros aplicados al reporte.</param>
    /// <param name="dtData">Datos que se utilizarán para construir el reporte.</param>
    /// <param name="reportName">Nombre del Reporte.</param>
    /// <param name="pivotColums">Campos que se les realizará el pivot.</param>
    /// <param name="pivotRows">Campos que se mostrarán como filas.</param>
    /// <param name="pivotValue">Campo el cual será el valor de la columna pivot.</param>
    /// <param name="pivotColumnsCount">Cantidad de Columnas de la tabla ya connvertida a pivot.</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [erosado] 14/03/2016  Created.
    /// [edgrodriguez] 15/03/2016 Modified. Se agregáron los parametros pivotColumns,pivotRows,pivotValue,pivotColumnsCount
    /// [edgrodriguez] 17/03/2016 Modified. Se agregó el parametros IsPivot, para indicar si se obtendra una tabla simple o una tabla de tipo pivot. 
    /// </history>
    public static FileInfo CreatePivotRptExcel(bool IsPivot, List<Tuple<string, string>> filters, DataTable dtData, Tuple<string, string> reportName, List<string> pivotColums, List<string> pivotRows, List<string> pivotValue, List<ExcelFormatTable> formatColumns, int pivotColumnsCount = 0)
    {
      FileInfo pathFinalFile = null;
      ExcelPackage pk = new ExcelPackage();
      //Preparamos la hoja donde escribiremos
      //la tabla dinamica.
      ExcelWorksheet wsPivot = pk.Workbook.Worksheets.Add(reportName.Item1);
      ExcelWorksheet wsData = pk.Workbook.Worksheets.Add("Hoja0");
      wsData.Hidden = eWorkSheetHidden.VeryHidden;

      //Numero de filtros
      int filterNumber = filters.Count + 2;

      //Numero de filas del contenido del datatable
      int dtRowsNumber = dtData.Rows.Count;
      //Numero de Columnas del contenido del datatable
      int dtColumnsNumber = (pivotColumnsCount == 0) ? dtData.Columns.Count : pivotColumnsCount;
      int cFiltros = 1;
      if (filters.Count > 0)
      {
        //Insertamos etiqueta de filtros
        wsPivot.Cells[1, 1].Value = "Filters";
        wsPivot.Cells[1, 1].Style.Font.Bold = true;
        wsPivot.Cells[1, 1].Style.Font.Size = 14;

        //Insertamos las filas que necesitamos para los filtros
        wsPivot.InsertRow(2, filters.Count + 3);

        foreach (Tuple<string, string> item in filters)
        {
          cFiltros++;
          wsPivot.Cells[string.Concat("A", cFiltros)].Value = item.Item1;
          wsPivot.Cells[string.Concat("A", cFiltros)].Style.Font.Bold = true;
          wsPivot.Cells[string.Concat("B", cFiltros)].Value = item.Item2;
        }
      }
      else
        wsPivot.InsertRow(2, filters.Count + 2);

      //Agregamos los datos de impresion
      wsPivot.Cells[string.Concat("A", cFiltros + 1)].Value = "Date Print";
      wsPivot.Cells[string.Concat("A", cFiltros + 1)].Style.Font.Bold = true;
      wsPivot.Cells[string.Concat("B", cFiltros + 1)].Value = string.Format(DateTime.Now.ToShortDateString(), "yyyy-MM-dd");
      wsPivot.Cells[string.Concat("A", cFiltros + 2)].Value = "Time Print";
      wsPivot.Cells[string.Concat("A", cFiltros + 2)].Style.Font.Bold = true;
      wsPivot.Cells[string.Concat("B", cFiltros + 2)].Value = string.Format(DateTime.Now.ToShortTimeString(), "hh:mm:ss");


      //Nombre del reporte y rango de fecha
      wsPivot.Cells[1, dtColumnsNumber].Value = reportName.Item1;
      wsPivot.Cells[1, dtColumnsNumber].Style.Font.Bold = true;
      wsPivot.Cells[1, dtColumnsNumber].Style.Font.Size = 14;
      wsPivot.Cells[1, dtColumnsNumber].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

      wsPivot.Cells[2, dtColumnsNumber].Value = "Intelligence Marketing";
      wsPivot.Cells[2, dtColumnsNumber].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

      wsPivot.Cells[3, dtColumnsNumber].Value = reportName.Item2;
      wsPivot.Cells[3, dtColumnsNumber].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


      //Agregamos el contenido
      wsPivot.InsertRow(filterNumber + 2, dtRowsNumber * 2);

      dtData.Columns.Cast<DataColumn>().ToList().ForEach(c =>
      {
        c.ColumnName = formatColumns[dtData.Columns.IndexOf(c)].Title;
      });

      //Cargamos los datos en la hoja0.      
      //var table= wsData.Tables.Add(wsData.Cells["A1"], "Data");
      wsData.Cells["A1"].LoadFromDataTable(dtData, true, OfficeOpenXml.Table.TableStyles.None);

      //Agregamos los Headers de la Tabla
      int contColum = 0;
      foreach (ExcelFormatTable item in formatColumns)
      {
        contColum++;
        //wsData.Cells[filterNumber + 6, contColum].Value = item.Title;

        switch (item.Format)
        {
          case EnumFormatTypeExcel.General:
            break;
          case EnumFormatTypeExcel.Percent:
            wsData.Column(contColum).Style.Numberformat.Format = "0.0 %";
            break;
          case EnumFormatTypeExcel.Currency:
            wsData.Column(contColum).Style.Numberformat.Format = "$#,##0.00";
            break;
          case EnumFormatTypeExcel.Number:
            wsData.Column(contColum).Style.Numberformat.Format = "#";
            break;
          case EnumFormatTypeExcel.DecimalNumber:
            wsData.Column(contColum).Style.Numberformat.Format = "0.00";
            break;
          case EnumFormatTypeExcel.Boolean:
            wsData.Cells[2, contColum, dtRowsNumber + 1, contColum].Style.Font.Name = "Wingdings";
            break;
          case EnumFormatTypeExcel.Date:
            wsData.Column(contColum).Style.Numberformat.Format = "dd/MM/yyyy";
            break;
          default:
            break;
        }
        wsData.Column(contColum).Style.HorizontalAlignment = item.Alignment;
      }
      //Borde de rango de columnas(titulos de la tabla)
      wsData.Cells[1, 1, dtRowsNumber + 1, dtColumnsNumber].AutoFitColumns();

      //Cargamos la tabla dinamica.
      var pivotTable = wsPivot.PivotTables.Add(wsPivot.Cells[filterNumber + 5, 1], wsData.Cells[1, 1, wsData.Dimension.End.Row, wsData.Dimension.End.Column], reportName.Item1);
      if (IsPivot)
      {
        //Mostrar Encabezados
        pivotTable.ShowHeaders = true;
        //Mostrar Totales por Columna
        pivotTable.ColumGrandTotals = true;
        //Mostrar Totales por Fila
        pivotTable.RowGrandTotals = true;
      }
      else
      {
        pivotTable.Compact = false;
        pivotTable.CompactData = true;
        pivotTable.Outline = false;
        pivotTable.OutlineData = false;
        pivotTable.Indent = 0;        
        pivotTable.ShowMemberPropertyTips = false;
        pivotTable.DataOnRows = false;
        pivotTable.RowGrandTotals = false;
        pivotTable.ShowDrill = false;
        pivotTable.EnableDrill = false;
        pivotTable.RowGrandTotals = false;
        pivotTable.ColumGrandTotals = false;
        pivotTable.MultipleFieldFilters = true;
        pivotTable.GridDropZones = false;
        pivotTable.TableStyle = OfficeOpenXml.Table.TableStyles.Medium13;
      }      

      //Asignamos las columnas para realizar el pivote
      pivotColums.ForEach(col => pivotTable.ColumnFields.Add(pivotTable.Fields[col]));

      //Asignamos las filas que se mostraran.
      pivotRows.ForEach(row =>
      {
        ExcelPivotTableField ptfField = pivotTable.Fields[row];
        pivotTable.RowFields.Add(ptfField);

        if (!IsPivot)
        {
          ptfField.Outline = false;
          ptfField.Compact = false;
          ptfField.ShowAll = false;
          ptfField.SubtotalTop = false;
          ptfField.SubTotalFunctions =eSubTotalFunctions.None;
        }
      }
      );

      //Asignamos el valor que se mostrara en las columnas.
      pivotValue.ForEach(value => pivotTable.DataFields.Add(pivotTable.Fields[value]));

      XDocument xdoc = new XDocument();
      using (var nodeReader = new XmlNodeReader(pivotTable.PivotTableXml))
      {
        nodeReader.MoveToContent();
        xdoc = XDocument.Load(nodeReader);
      }
      var xd = xdoc.Descendants().ToList();
      xdoc.Root.Descendants().ToList().ForEach(c => {
        switch (c.Name.LocalName) {
          //case "pivotTableDefinition":
          //  c.Attribute("useAutoFormatting").SetValue("1");
          //  c.Attribute("applyNumberFormats").SetValue("1");
          //  c.Attribute("applyFontFormats ").SetValue("1");
          //  c.Attribute("applyPatternFormats").SetValue("1");
          //  c.Attribute("applyWidthHeightFormats").SetValue("1");
          //  c.Attribute("applyAlignmentFormats").SetValue("1");
          //  break;
          case "pivotTableStyleInfo":
            c.Attribute("showRowHeaders").SetValue("0");

            break;
        }
      }
      );

      using (var xmlReader = xdoc.CreateReader())
      {
        pivotTable.PivotTableXml.Load(xmlReader);
      }

      string suggestedFilaName = string.Concat(reportName.Item1, "_", DateTime.Now.ToString("ddmmyyyyhhmmss"));
      pathFinalFile = SaveExcel(pk, suggestedFilaName);

      if (pathFinalFile != null)
      {
        return pathFinalFile;
      }
      else
      {
        return null;
      }
    }
    #endregion
  }
}
