using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;
using OfficeOpenXml.VBA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Xml;

namespace IM.Base.Helpers
{
  public static class EpplusHelper
  {
    private static char separator = '_';
    private static double ColumnMaxWidth = 50;

    #region Public Methods

    #region CreateGeneralRptExcel

    /// <summary>
    ///   Crea un reporte en excel que tiene Filtros, Nombre de reporte, contenido, datos de impresion y nombre del sistema
    /// </summary>
    /// <param name="filter">Tupla de filtros: item1 = Nombre del filtro - item2 =filtros separados por comas (bp,mbp)
    ///   si seleccionan todos poner en item2 =ALL
    /// </param>
    /// <param name="dt">DataTable con la informacion del reporte</param>
    /// <param name="reportName">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Nombre del reporte</param>
    /// <param name="formatColumns">Lista de ExcelFormatTable donde definimos ("Titulo de la columna",IM.Model.Enums.EnumFormatTypeExcel, OfficeOpenXml.Style.Enum.ExcelHorizontalAlignment) </param>
    /// <param name="extraFieldHeader"></param>
    /// <param name="numRows"></param>
    /// <param name="fileFullPath">Opcional. Ruta completa del archivo</param>
    /// <returns>FileInfo con el path para abrir el excel</returns>
    /// <history>
    ///   [erosado] 12/Mar/2016 Created.
    ///   [erosado] 14/Mar/2016 Modified  Se Agrego el parametro formatColumns.
    ///   [erosado] 17/Mar/2016 Modified  Se Agrego que permite tipos Boleanos y Date formateados
    ///   [erosado] 18/Mar/2016 Modified  Se cambio el formato del reporte  se cambio de posicion el titulo del reporte y los
    ///   filtros
    ///   [erosado] 22/Mar/2016 Modified  Se agrego la validacion si tiene filtro los reportes o si no tiene.
    ///   [edgrodriguez] 23/Mar/2016 Modified. Se cambió el formateo de columnas. Ahora lo realiza desde un método.
    /// [erosado] 02/Mar/2016 Modified  Se cambio el formato del encabezado y los filtros.
    /// [aalcocer] 16/04/2016 Modified. Se cambia el contenido del reporte en a una tabla con encabezados integrado.
    ///                                 Se agrega la  opcion de mostrar totales de la tabla
    /// [aalcocer] 06/06/2016 Modified. Se agrega la opcion de generar el reporte en de ruta completa del archivo
    /// </history>
    public static FileInfo CreateGeneralRptExcel(List<Tuple<string, string>> filter, DataTable dt, string reportName, string dateRangeFileName, List<ExcelFormatTable> formatColumns,
      List<Tuple<string, dynamic, EnumFormatTypeExcel>> extraFieldHeader = null, int numRows = 0, string fileFullPath = null)
    {
      FileInfo pathFinalFile;
      #region Variables Atributos, Propiedades

      using (var pk = new ExcelPackage())
      {
        //Preparamos la hoja donde escribiremos
        var ws = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
        //Filas Para los filtros
        var filasTotalesFiltros = 0;

        //Renombramos las columnas.
        dt.Columns.Cast<DataColumn>().ToList().ForEach(c =>
        {
          c.ColumnName = formatColumns[dt.Columns.IndexOf(c)].Title;
        });

        #endregion Variables Atributos, Propiedades

        #region Report SuperHeader

        //Creamos la cabecera del reporte (Titulos, Filtros, Fecha y Hora de Impresion)
        CreateReportHeader(filter, reportName, ref ws, ref filasTotalesFiltros, extraFieldHeader, numRows);

        #endregion Report SuperHeader

        #region Contenido del reporte

        //Insertamos las filas que vamos a necesitar
        ws.InsertRow(filasTotalesFiltros + 3, dt.Rows.Count);

        //Agregamos el contenido empezando en la fila
        var range = ws.Cells[filasTotalesFiltros + 1, 1].LoadFromDataTable(dt, true);
        //El contenido lo convertimos a una tabla
        var table = ws.Tables.Add(range, null);
        table.TableStyle = TableStyles.Medium2;

        //Formateamos la tabla
        SetFormatTable(formatColumns, ref table);

        //Agrega totales
        if (formatColumns.Any(c => !string.IsNullOrEmpty(c.Formula) || c.TotalsRowFunction != RowFunctions.None))
        {
          table.ShowTotal = true;
          table.Columns[0].TotalsRowLabel = "TOTAL";
          for (var i = 1; i < formatColumns.Count; i++)
          {
            var c = table.Columns[i];
            var format = formatColumns[i];
            if (format.TotalsRowFunction != RowFunctions.None)
            {
              c.TotalsRowFunction = format.TotalsRowFunction;
              ws.Cells[table.Address.End.Row, table.Address.Start.Column + i].StyleName = c.DataCellStyleName;
            }
            else if (!string.IsNullOrEmpty(format.Formula))
            {
              c.TotalsRowFormula = format.Formula;
              ws.Cells[table.Address.End.Row, table.Address.Start.Column + i].StyleName = c.DataCellStyleName;
            }
          }
        }

        #endregion Contenido del reporte

        #region Formato de columnas Centrar y AutoAjustar

        //Auto Ajuste de columnas de  acuerdo a su contenido
        //ws.Cells[ws.Dimension.Address].AutoFitColumns();
        AutoFitColumns(ref ws, 0, 0, true);
        //Centramos el titulo de la aplicacion
        ws.Cells[1, 1, 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //Centramos el titulo del reporte
        ws.Cells[1, 4, 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //Centramos La etiqueta Filters
        if (filter.Count > 0)
        {
          ws.Cells[2, 1, filasTotalesFiltros + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
          ws.Cells[2, 1, filasTotalesFiltros + 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        #endregion Formato de columnas Centrar y AutoAjustar

        #region Generamos y Retornamos la ruta del archivo EXCEL

        if (fileFullPath == null)
        {
          var suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);
          pathFinalFile = SaveExcel(pk, suggestedFilaName);
        }
        else
          pathFinalFile = SaveExcelFilePath(pk, fileFullPath);
      }
      return pathFinalFile;

      #endregion Generamos y Retornamos la ruta del archivo EXCEL
    }

    #endregion CreateGeneralRptExcel

    #region CreatePivotRptExcel

    /// <summary>
    /// </summary>
    /// <param name="isPivot">Bandera para indicar si se requiere una tabla tipo Pivot.</param>
    /// <param name="filters">Filtros aplicados al reporte.</param>
    /// <param name="dtData">Datos que se utilizarán para construir el reporte.</param>
    /// <param name="reportName">Nombre del Reporte.</param>
    /// <param name="dateRangeFileName">Nombre del archivo del reporte</param>
    /// <param name="formatColumns">Formato de las filas y columnas del reporte</param>
    /// <param name="showRowGrandTotal">Opcion para mostrar totales generales de la filas</param>
    /// <param name="showColumnGrandTotal">Opcion para mostrar totales generales de la columna</param>
    /// <param name="showRowHeaders">Muestra el encabezado especial para la primera columna de la tabla</param>
    /// <param name="extraFieldHeader"></param>
    /// <param name="numRows"></param>
    /// <param name="fileFullPath">Opcional. Ruta completa del archivo</param>
    /// <returns> FileInfo </returns>
    /// <history>
    ///   [erosado]      14/03/2016 Created.
    ///   [edgrodriguez] 15/03/2016 Modified. Se agregáron los parametros pivotColumns,pivotRows,pivotValue,pivotColumnsCount
    ///   [edgrodriguez] 17/03/2016 Modified. Se agregó el parametros IsPivot, para indicar si se obtendra una tabla simple o una tabla de tipo pivot.
    ///   [edgrodriguez] 23/03/2016 Modified. Se agregó validaciones para reportes con varios campos Valor, se hizo un cambio en la edición del pivot desde el xml.
    ///                                       Se cambio el formateo de columnas desde un método.
    ///   [aalcocer]     01/04/2016 Modified. Se agregó validaciones que las columnas y valores tomen un determinado formato, se agrega la opcion de insertar valores calculados.
    ///   [aalcocer]     11/04/2016 Modified. Se agrega la opción de insertar Superheaders y borders a columnas.
    ///   [aalcocer]     16/04/2016 Modified. Se agrego la opcion de mostrar encabezado especial para la primera columna de la tabla
    ///   [aalcocer]     23/05/2016 Modified. Se agrega la opcion de mostrar distintos cálculos en los campos de valores de tabla dinámica
    ///   [aalcocer]     06/06/2016 Modified. Se agrega la opcion de generar el reporte en de ruta completa del archivo
    /// </history>
    public static FileInfo CreatePivotRptExcel(bool isPivot, List<Tuple<string, string>> filters, DataTable dtData,
      string reportName, string dateRangeFileName,
      List<ExcelFormatTable> formatColumns, bool showRowGrandTotal = false, bool showColumnGrandTotal = false, bool showRowHeaders = false,
      List<Tuple<string, dynamic, EnumFormatTypeExcel>> extraFieldHeader = null, int numRows = 0, string fileFullPath = null)
    {
      FileInfo pathFinalFile;
      using (var pk = new ExcelPackage())
      {
        //Preparamos la hoja donde escribiremos
        //la tabla dinamica.
        var wsPivot = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", ""));
        var wsData = pk.Workbook.Worksheets.Add("Hoja0");
        wsData.Hidden = eWorkSheetHidden.Hidden;

        var totalFilterRows = 0;
        CreateReportHeader(filters, reportName, ref wsPivot, ref totalFilterRows, extraFieldHeader, numRows);

        //Renombramos las columnas.
        dtData.Columns.Cast<DataColumn>().ToList().ForEach(c =>
        {
          var colName = formatColumns[dtData.Columns.IndexOf(c)];
          c.ColumnName = (string.IsNullOrWhiteSpace(colName.SuperHeader)) ? (formatColumns[dtData.Columns.IndexOf(c)].Title ?? formatColumns[dtData.Columns.IndexOf(c)].PropertyName) : (formatColumns[dtData.Columns.IndexOf(c)].PropertyName ?? formatColumns[dtData.Columns.IndexOf(c)].Title);
        });

        //Cargamos los datos en la hoja0.
        var rangeTable = wsData.Cells["A1"].LoadFromDataTable(dtData, true);
        //El contenido lo convertimos a una tabla
        var table = wsData.Tables.Add(rangeTable, null);
        table.TableStyle = TableStyles.None;
        //Formateamos la tabla
        SetFormatTable(formatColumns.Where(c => !(c.Axis == ePivotFieldAxis.Values && !string.IsNullOrEmpty(c.Formula))).ToList(), ref table);

        //Cargamos la tabla dinamica.
        var pivotTable = wsPivot.PivotTables.Add(wsPivot.Cells[totalFilterRows + 1, 1],
          wsData.Cells[1, 1, wsData.Dimension.End.Row, wsData.Dimension.End.Column], Regex.Replace(reportName, "[^a-zA-Z0-9_]+", ""));
        pivotTable.ApplyWidthHeightFormats = false;

        if (isPivot)
        {
          #region Formato Esquematico

          //Mostrar Encabezados
          pivotTable.ShowHeaders = true;

          #endregion Formato Esquematico
        }
        else
        {
          #region Formato Tabular

          if (!showRowHeaders) //Se maneja en formato XML ya que Epplus no cuenta con la propiedad para modificarlo.
            if (pivotTable.PivotTableXml.DocumentElement?.LastChild.Attributes != null)
              pivotTable.PivotTableXml.DocumentElement.LastChild.Attributes["showRowHeaders"].Value = "0";

          pivotTable.Compact = false;
          pivotTable.CompactData = true;
          pivotTable.Outline = false;
          pivotTable.OutlineData = false;
          pivotTable.Indent = 0;
          pivotTable.ShowMemberPropertyTips = false;
          pivotTable.DataOnRows = false;

          pivotTable.MultipleFieldFilters = true;

          var fistsRow = formatColumns.Where(c => c.Axis == ePivotFieldAxis.Row && !c.Compact)
            .OrderBy(c => c.Order).FirstOrDefault();
          pivotTable.RowHeaderCaption = fistsRow != null ? fistsRow.Title : string.Empty;

          #endregion Formato Tabular
        }

        //Mostrar Totales por Columna
        pivotTable.ColumGrandTotals = showColumnGrandTotal;
        //Mostrar Totales por Fila
        pivotTable.RowGrandTotals = showRowGrandTotal;

        pivotTable.TableStyle = TableStyles.Medium13;
        pivotTable.ShowDrill = false;
        pivotTable.EnableDrill = false;
        pivotTable.GridDropZones = false;
        pivotTable.DataCaption = string.Empty;
        pivotTable.ErrorCaption = "0";
        pivotTable.ShowError = true;

        //Asignamos las columnas para realizar el pivote
        formatColumns.Where(c => c.Axis == ePivotFieldAxis.Column)
          .OrderBy(c => c.Order)
          .ToList().ForEach(col =>
          {
            var ptfField = pivotTable.ColumnFields.Add(pivotTable.Fields[(string.IsNullOrWhiteSpace(col.SuperHeader) ? col.Title ?? col.PropertyName : col.PropertyName ?? col.Title)]);

            if (!isPivot) //Si se va manejar el formato tabular.
              ptfField.ShowAll = false;
            ptfField.SubtotalTop = col.SubtotalTop;
            ptfField.SubTotalFunctions = col.SubTotalFunctions;
            ptfField.Sort = col.Sort;

            #region Formato

            if (col.Format != EnumFormatTypeExcel.General)
            {
              //Formato
              var highlightedItemProperty = ptfField.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Single(pi => pi.Name == "TopNode");
              var ptfFieldXml = (XmlElement)highlightedItemProperty.GetValue(ptfField, null);
              var styles = pivotTable.WorkSheet.Workbook.Styles;
              var nFormatXml = styles.NumberFormats.ToList().Find(x => x.Format == GetFormat(col.Format));

              //Si existe el Formato
              if (nFormatXml != null)
              {
                var numFmtIdAttrib = pivotTable.PivotTableXml.CreateAttribute("numFmtId");
                numFmtIdAttrib.Value = nFormatXml.NumFmtId.ToString();
                ptfFieldXml.Attributes.Append(numFmtIdAttrib);
              }
            }

            #endregion Formato
          });

        //Asignamos las filas que se mostraran.
        formatColumns.Where(c => c.Axis == ePivotFieldAxis.Row)
          .OrderBy(c => c.Order)
          .ToList().ForEach(rowFormat =>
          {
            //Lo Agregamos a la lista de Filas.
            var ptfField = pivotTable.RowFields.Add(pivotTable.Fields[(string.IsNullOrWhiteSpace(rowFormat.SuperHeader) ? rowFormat.Title ?? rowFormat.PropertyName : rowFormat.PropertyName ?? rowFormat.Title)]);

            if (!isPivot) //Si se va manejar el formato tabular.
            {
              ptfField.Outline = rowFormat.Outline;
              ptfField.Compact = rowFormat.Compact;
              ptfField.ShowAll = rowFormat.ShowAll;
            }

            ptfField.SubtotalTop = rowFormat.SubtotalTop;
            ptfField.SubTotalFunctions = rowFormat.SubTotalFunctions;
            ptfField.Sort = rowFormat.Sort;

            #region Formato

            if (rowFormat.Format != EnumFormatTypeExcel.General)
            {
              //Formato
              var highlightedItemProperty = ptfField.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Single(pi => pi.Name == "TopNode");
              var ptfFieldXml = (XmlElement)highlightedItemProperty.GetValue(ptfField, null);
              var styles = pivotTable.WorkSheet.Workbook.Styles;
              var nFormatXml = styles.NumberFormats.ToList().Find(x => x.Format == GetFormat(rowFormat.Format));

              //Si existe el Formato
              if (nFormatXml != null)
              {
                var numFmtIdAttrib = pivotTable.PivotTableXml.CreateAttribute("numFmtId");
                numFmtIdAttrib.Value = nFormatXml.NumFmtId.ToString();
                ptfFieldXml.Attributes.Append(numFmtIdAttrib);
              }
            }

            #endregion Formato

            #region Salto de linea

            if (rowFormat.InsertBlankRow)
            {
              //Insertar salto de linea
              var highlightedItemProperty = ptfField.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Single(pi => pi.Name == "TopNode");
              var ptfFieldXml = (XmlElement)highlightedItemProperty.GetValue(ptfField, null);
              var insertBlankRowIdAttrib = pivotTable.PivotTableXml.CreateAttribute("insertBlankRow");
              insertBlankRowIdAttrib.Value = "1";
              ptfFieldXml.Attributes.Append(insertBlankRowIdAttrib);
            }

            #endregion Salto de linea
          });

        //Asignamos el valor que se mostrara en las columnas.
        formatColumns.Where(c => c.Axis == ePivotFieldAxis.Values && string.IsNullOrEmpty(c.Formula))
          .OrderBy(c => c.Order)
          .ToList().ForEach(valueFormat =>
          {
            var valueField = pivotTable.DataFields.Add(pivotTable.Fields[(string.IsNullOrWhiteSpace(valueFormat.SuperHeader) ? valueFormat.Title ?? valueFormat.PropertyName : valueFormat.PropertyName ?? valueFormat.Title)]);

            if (!isPivot)
            {
              valueField.Name = valueFormat.Title ?? valueFormat.PropertyName;
              valueField.BaseField = 0;
              valueField.BaseItem = 0;
              valueField.Function = valueFormat.Function;
            }
            valueField.Format = GetFormat(valueFormat.Format);
            valueField.Field.Sort = valueFormat.Sort;
            if (valueFormat.DataFieldShowDataAs != EnumDataFieldShowDataAs.Normal)
              valueField.SetDataFieldShowDataAsAttribute(pivotTable, valueFormat.DataFieldShowDataAs);
          });

        // Agregamos valores calculados
        formatColumns.Where(c => c.Axis == ePivotFieldAxis.Values && !string.IsNullOrEmpty(c.Formula))
          .OrderBy(c => c.Order).ToList()
          .ForEach(valueCalculated => pivotTable.AddCalculatedField(valueCalculated));

        //Agregamos SuperHeader fuera del Pivote
        if (formatColumns.Any(c => c.SuperHeader != null))
        {
          var formatHeaders = formatColumns.Where(c => c.SuperHeader != null).ToList();

          var startColumn = pivotTable.Address.Start.Column;
          var startRow = pivotTable.Address.Start.Row - 1;

          var rowCount = pivotTable.RowFields.Count(c => !(c.Compact && c.Outline));

          var formatHeadersValue = formatHeaders.Where(c => c.Axis == ePivotFieldAxis.Values).OrderBy(c => c.Order).GroupBy(c => c.SuperHeader).ToList();
          formatHeadersValue.ForEach(v =>
          {
            var columnasAgrupadas = new List<ExcelFormatTable>();
            var fromCol = 0;
            for (var i = 0; i < v.Count(); i++)
            {
              var value = v.ElementAt(i);
              columnasAgrupadas.Add(value);
              if (fromCol == 0)
                fromCol = value.Order + rowCount + startColumn;

              var valueNext = v.ElementAtOrDefault(i + 1);
              if (valueNext != null && valueNext.Order == value.Order + 1) continue;

              var toCol = value.Order + rowCount + startColumn;

              using (var range = wsPivot.Cells[startRow, fromCol, startRow, toCol])
              {
                range.Value = value.SuperHeader;
                range.Style.Font.Bold = true;
                range.Merge = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.Cyan);
                range.Style.Border.BorderAround(ExcelBorderStyle.Medium);
              }

              fromCol = 0;
              columnasAgrupadas.Clear();
            }
          });
        }

        AutoFitColumns(ref wsPivot, 0, 0, true);

        if (fileFullPath == null)
        {
          var suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);
          pathFinalFile = SaveExcel(pk, suggestedFilaName);
        }
        else
        {
          pathFinalFile = SaveExcelFilePath(pk, fileFullPath);
        }
      }

      return pathFinalFile;
    }

    #endregion CreatePivotRptExcel

    #region CreateExcelCustom

    /// <summary>
    /// Exporta un Datatable a Excel.
    /// </summary>
    /// <param name="dtTable"> Tabla con la informacion </param>
    /// <param name="filters"> Filtros aplicados al reporte </param>
    /// <param name="reportName"> Nombre del reporte </param>
    /// <param name="dateRangeFileName"></param>
    /// <param name="formatTable"> Formatos de campos del reporte </param>
    /// <param name="blnColumnGrandTotal"></param>
    /// <param name="blnRowGrandTotal"></param>
    /// <param name="blnShowSubtotal"></param>
    /// <param name="extraFieldHeader"></param>
    /// <param name="numRows"></param>
    /// <param name="fileFullPath">Opcional. Ruta completa del archivo</param>
    /// <returns> FileInfo </returns>
    /// <history>
    ///   [edgrodriguez] 28/03/2016  Created.
    ///   [edgrodriguez] 24/05/2016 Modified. Se agrega agrupaciones. Calculo de Subtotales por grupo. Y estilos.
    ///   [aalcocer]     06/06/2016 Modified. Se agrega la opcion de generar el reporte en de ruta completa del archivo
    ///   [ecanul] 13/06/2016 Modified. Se agrego la impresion de Superheaders
    /// </history>
    public static FileInfo CreateExcelCustom(DataTable dtTable, List<Tuple<string, string>> filters, string reportName,
      string dateRangeFileName, List<ExcelFormatTable> formatTable, bool blnColumnGrandTotal = false,
      bool blnRowGrandTotal = false, bool blnShowSubtotal = false,
      List<Tuple<string, dynamic, EnumFormatTypeExcel>> extraFieldHeader = null, int numRows = 0, string fileFullPath = null)
    {
      FileInfo pathFinalFile;
      using (var pk = new ExcelPackage())
      {
        var wsData = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
        var totalFilterRows = 0;

        //Creamos el encabezado
        CreateReportHeader(filters, reportName, ref wsData, ref totalFilterRows, extraFieldHeader, numRows);

        var rowNumber = totalFilterRows + 1;

        //Obtenemos las columnas y las ordenamos.
        var formatTableColumns = formatTable.Where(c => !c.IsGroup && c.Order > 0 && c.IsVisible)
          .OrderBy(row => row.Order).ToList();

        #region Creando Headers
        //Agregando los headers de acuerdo al orden configurado.
        //descartando los campos configurados como grupos.
        formatTable.OrderBy(c => c.Order).ToList().ForEach(format =>
          {

            if (format != null && format.Order > 0 && format.IsVisible && !format.IsGroup)
            {
              dtTable.Columns[format.PropertyName].SetOrdinal(format.Order - 1);
              using (var range = wsData.Cells[rowNumber, format.Order])
              {
                range.Value = format.Title;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 12;
                range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
              }
            }
            else if (format != null && !format.IsVisible)
            {
              dtTable.Columns.Remove(format.PropertyName);
            }
            else
              dtTable.Columns[format.PropertyName].SetOrdinal(dtTable.Columns.Count - 1);
          });
        rowNumber++;
        #endregion

        //Si existe algun grupo
        if (formatTable.Any(c => c.IsGroup))
        {
          #region Simple con Agrupado

          #region Formato para encabezados de grupo

          //Formato para los encabezados de grupo.
          var backgroundColorGroups = new List<ExcelFormatGroupHeaders> {
            new ExcelFormatGroupHeaders { BackGroundColor="#004E48", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#147F79", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#2D8B85", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#4CA09A", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            //Formato para la fila de Gran Total.
            new ExcelFormatGroupHeaders { BackGroundColor="#000000", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left }
        };
          #endregion

          #region Obtenemos los encabezados de grupo y sus valores
          //Creamos la sentencia Linq para obtener los campos que se agruparán.
          var qfields = string.Join(", ", formatTable
      .Where(c => c.IsGroup)
      .OrderBy(c => c.Order).Select(x => "it[\"" + x.PropertyName + "\"] as " + x.PropertyName));

          //Obtenemos las agrupaciones y los registros de cada agrupacion.
          var qTable = dtTable
            .AsEnumerable()
            .AsQueryable()
            .GroupBy("new(" + qfields + ")", "it")
            .Select("new(Key as qgroup, it as Values)");

          #endregion

          //Lista de formulas para cada grupo. Teniendo como items las columnas que tienen la propiedad SubtotalFunction.
          var subtotalFormulas = new Dictionary<string, string>[formatTable.Count(c => c.IsGroup)];
          //Lista de grupos.       
          var dynamicListData = qTable.OfType<dynamic>().ToList();
          //Total de columnas que no son grupo.
          var totalColumns = formatTable.Count(c => !c.IsGroup && c.IsVisible);
          var previousGroup = new string[subtotalFormulas.Length];
          //Recorremos la lista de grupos.
          for (var i = 0; i < dynamicListData.Count; i++)
          {
            var nextGroup = new string[subtotalFormulas.Length];
            //Obtenemos la informacion del item actual.
            object itemActual = dynamicListData[i].qgroup;
            //Obtenemos los headers de cada grupo.
            var groupsAct = itemActual.GetType().GetProperties().Select(c => c.GetValue(itemActual).ToString()).ToArray();
            //Si es el primer item de la lista o es el indice es mayor a cero y el primer grupo del item actual
            //es diferente al primer grupo del item anterior.
            if (i == 0 || (i > 0 && groupsAct[0] != previousGroup[0]))
            {
              //Dibujamos todos los headers de grupo.
              for (var j = 0; j < groupsAct.Length; j++)
              {
                wsData.Cells[rowNumber, 1].Value = groupsAct[j];
                using (var range = wsData.Cells[rowNumber, 1, rowNumber, totalColumns])
                {
                  range.Merge = true;
                  range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                  range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                  range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                  range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                  range.Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                }
                rowNumber++;
              }
              //asignamos el valor actual a la variable aux
              previousGroup = groupsAct;
            }
            else if (i > 0)
            {
              //Recorremos los encabezados(Niveles).
              for (var j = 0; j < groupsAct.Length; j++)
              {
                if (groupsAct[j] == previousGroup[j]) continue;
                //Si el nivel actual es diferente al valor anterior.
                if (groupsAct[j] != previousGroup[j])
                {
                  //Dibujamos el encabezado.
                  wsData.Cells[rowNumber, 1].Value = groupsAct[j];
                  using (var range = wsData.Cells[rowNumber, 1, rowNumber, totalColumns])
                  {
                    range.Merge = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                    range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                    range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                    range.Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                  }
                  rowNumber++;
                }
              }
              previousGroup = groupsAct;
            }
            //Obtenemos los datos.
            var dataValues = ((IEnumerable<DataRow>)dynamicListData[i].Values).CopyToDataTable();
            var camposGrupo = formatTable.Where(col => col.IsGroup).Select(col => col.PropertyName).ToList();
            //Eliminamos las columnas que fueron configuradas como grupo. Y obtenemos el formato de las columnas que se visualizaran en el reporte.
            dataValues.Columns.OfType<DataColumn>().ToList().ForEach(c =>
            {
              var format = formatTable.FirstOrDefault(f => f.PropertyName == c.ColumnName);

              if (camposGrupo.Contains(c.ColumnName) || (format != null && !format.IsVisible))
                dataValues.Columns.Remove(c);
              else
              {
                if (format != null && format.Order > 0)
                {
                  using (var range = wsData.Cells[rowNumber, format.Order, rowNumber + dataValues.Rows.Count, format.Order])
                  {
                    range.Style.Numberformat.Format = GetFormat(format.Format);
                  }
                }
              }
            });


            //Agregamos los datos al excel.
            using (var range = wsData.Cells[rowNumber, 1].LoadFromDataTable(dataValues, false))
            {
              //Aplicamos estilo a las celdas.
              range.Style.Border.Top.Style = range.Style.Border.Right.Style = range.Style.Border.Left.Style = range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
              range.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Font.Size = 9;
            }

            rowNumber += dataValues.Rows.Count;

            //Si se mostrará el subtotal de cada grupo.
            if (blnShowSubtotal)
            {
              //Obtenemos la fila inicial.
              var dataIniRow = rowNumber - dataValues.Rows.Count;
              //Si el indice siguiente es menor que la cantidad total de items.
              if (i + 1 < dynamicListData.Count)
              {
                //Obtenemos objeto con los encabezados de grupo.
                object nextItem = dynamicListData[i + 1].qgroup;
                //Obtenemos el arreglo de encabezados de grupo. Los niveles estan de acuerdo al indice del arreglo.
                nextGroup = nextItem.GetType().GetProperties().Select(c => c.GetValue(nextItem).ToString()).ToArray();
                //Si el primer nivel de cada arreglo son diferentes.
                if (groupsAct[0] != nextGroup[0])
                  nextGroup = new string[groupsAct.Length];//Limpiamos la lista
              }
              //Recorremos los niveles del arreglo actual.
              for (var j = groupsAct.Length - 1; j >= 0; j--)
              {
                //Si los valores del index actual de cada lista son diferentes o el valor del index de la siguiente lista esta vacia o nula.
                if (groupsAct[j] != nextGroup[j] || string.IsNullOrEmpty(nextGroup[j]) || j == groupsAct.Length - 1)
                {
                  //Recorremos las columnas.
                  foreach (var format in formatTableColumns)
                  {
                    using (var range = wsData.Cells[rowNumber, format.Order])
                    {
                      if (format.SubTotalFunctions == eSubTotalFunctions.None && format.Formula == null) continue;

                      var subtotalFormat = format.Format;
                      if (format.SubtotalWithCero)
                      {
                        switch (format.Format)
                        {
                          case EnumFormatTypeExcel.Number:
                            subtotalFormat = EnumFormatTypeExcel.NumberWithCero;
                            break;
                          case EnumFormatTypeExcel.DecimalNumber:
                            subtotalFormat = EnumFormatTypeExcel.DecimalNumberWithCero;
                            break;
                          case EnumFormatTypeExcel.Percent:
                            subtotalFormat = EnumFormatTypeExcel.PercentWithCero;
                            break;
                        }
                      }

                      //Le aplicacamos el formato a la celda.
                      range.Style.Numberformat.Format = GetFormat(subtotalFormat);

                      //Si no es calculada aplicamos la funcion configurada.
                      if (!format.IsCalculated)
                      {
                        var formula = "";
                        //Si es el ultimo nivel
                        if (j == groupsAct.Length - 1)
                          formula = wsData.Cells[dataIniRow, format.Order, rowNumber - 1, format.Order].Address;//Aplicamos la seleccion segun la cantidad de registros que tenga el grupo.
                        else
                        {
                          //Si son antes del ultimo nivel.
                          var index = j + 1;//Se obtiene el indice actual mas 1
                          while (formula == "")
                          {
                            //Si el indice devuelve un valor nulo, se incrementa.
                            if (subtotalFormulas[index] == null) { index++; continue; }
                            //Obtenemos las posiciones de cada subtotal.
                            formula = subtotalFormulas[index][format.PropertyName];

                            //Si no es el ultimo nivel y el nivel actual de cada arreglo son diferentes o el nivel actual del arreglo siguiente  es nulo.
                            if (j < groupsAct.Length - 1 && (groupsAct[j] != nextGroup[j] || string.IsNullOrEmpty(nextGroup[j])))
                            {
                              //Limpiamos la formula.
                              subtotalFormulas[index][format.PropertyName] = string.Empty;
                            }
                          }
                        }
                        switch (format.SubTotalFunctions)
                        {
                          case eSubTotalFunctions.Sum:
                            range.Formula = "=SUM(" + formula + ")";
                            break;
                          case eSubTotalFunctions.Avg:
                            range.Formula = "=AVERAGE(" + formula + ")";
                            break;
                          case eSubTotalFunctions.Count:
                            if (format.Format == EnumFormatTypeExcel.General)
                              range.Formula = (j == groupsAct.Length - 1) ? "= COUNTA(" + formula + ")" : "= SUM(" + formula + ")";
                            break;
                        }
                        if (subtotalFormulas[j] != null && subtotalFormulas[j].ContainsKey(format.PropertyName))
                        {
                          subtotalFormulas[j][format.PropertyName] += (subtotalFormulas[j][format.PropertyName] == string.Empty) ? range.Address : "," + range.Address;
                        }
                        else
                        {
                          if (subtotalFormulas[j] == null) subtotalFormulas[j] = new Dictionary<string, string>();
                          subtotalFormulas[j].Add(format.PropertyName, range.Address);
                        }
                      }
                      else
                        //Obtenemos la formula.
                        range.Formula = GetFormula(formatTable, format.Formula, rowNumber);
                    }
                  }
                  var firstSubtotalColumn = formatTable.First(c => c.SubTotalFunctions != eSubTotalFunctions.None || c.Formula != null).Order;
                  using (var range = wsData.Cells[rowNumber, firstSubtotalColumn, rowNumber, totalColumns])
                  {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                    range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                    range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                  }
                  rowNumber += 2;
                }
              }
            }
            rowNumber++;
          }

          if (blnRowGrandTotal)
          {
            formatTableColumns.ForEach(format =>
            {
              using (var range = wsData.Cells[rowNumber - 1, format.Order])
              {
                if (format.SubTotalFunctions == eSubTotalFunctions.None && format.Formula == null) return;

                if (!format.IsCalculated)
                {
                  switch (format.SubTotalFunctions)
                  {
                    case eSubTotalFunctions.Sum:
                      range.Formula = "=SUM(" + subtotalFormulas[0][format.PropertyName] + ")";
                      break;
                    case eSubTotalFunctions.Avg:
                      range.Formula = "=AVERAGE(" + subtotalFormulas[0][format.PropertyName] + ")";
                      break;
                    case eSubTotalFunctions.Count:
                      if (format.Format == EnumFormatTypeExcel.General)
                        range.Formula = "= SUM(" + subtotalFormulas[0][format.PropertyName] + ")";
                      break;
                  }
                }
                else
                  range.Formula = GetFormula(formatTableColumns, format.Formula, rowNumber - 1);

                range.Style.Numberformat.Format = GetFormat(format.Format);
              }
            });
            using (var range = wsData.Cells[rowNumber - 1, 1, rowNumber - 1, totalColumns])
            {
              range.Style.Fill.PatternType = ExcelFillStyle.Solid;
              range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].BackGroundColor));
              range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].FontColor));
              range.Style.Font.Bold = backgroundColorGroups[backgroundColorGroups.Count - 1].FontBold;
            }
          }
          //Ajustamos todas las columnas a su contenido.
          //wsData.Cells[totalFilterRows + 5, 1, rowNumber, totalColumns].AutoFitColumns();

          #endregion Simple con Agrupado
        }
        else
        {
          #region Simple
          //Eliminamos las columnas que fueron configuradas como No Visibles. Y obtenemos el formato de las columnas que se visualizaran en el reporte.
          dtTable.Columns.OfType<DataColumn>().ToList().ForEach(c =>
          {
            var format = formatTableColumns.FirstOrDefault(f => f.PropertyName == c.ColumnName);
            if (format != null && !format.IsVisible)
            {
              dtTable.Columns.Remove(c);
            }
          });

          var columnIndex = 1;
          var rowEnd = rowNumber + dtTable.Rows.Count;
          dtTable.Columns.Cast<DataColumn>().ToList().ForEach(col =>
          {
            var columnN = col.ColumnName.Split(separator);
            if (columnN.Length == 1)
            {
              var format = GetFormat(formatTable.First(c => c.PropertyName == columnN[0]).Format);
              if (format != "")
              {
                using (var range = wsData.Cells[rowNumber, columnIndex, rowEnd, columnIndex])
                {
                  range.Style.Numberformat.Format = format;
                }
              }
            }
            else
            {
              var format = GetFormat(formatTable.First(c => c.PropertyName == columnN[columnN.Length - 1]).Format);
              if (format != "")
              {
                using (var range = wsData.Cells[rowNumber, columnIndex, rowEnd, columnIndex])
                {
                  range.Style.Numberformat.Format = format;
                }
              }
            }
            columnIndex++;
          });

          //El contenido lo convertimos a una tabla
          wsData.Cells[rowNumber, 1].LoadFromDataTable(dtTable, false);
          rowNumber += dtTable.Rows.Count;

          if (blnRowGrandTotal)
          {
            //Obtenemos la fila inicial.
            var dataIniRow = rowNumber - dtTable.Rows.Count;
            //Recorremos las columnas.
            formatTableColumns.ForEach(format =>
            {
              var subtotalFormat = format.Format;
              if (format.SubtotalWithCero)
              {
                switch (format.Format)
                {
                  case EnumFormatTypeExcel.Number:
                    subtotalFormat = EnumFormatTypeExcel.NumberWithCero;
                    break;

                  case EnumFormatTypeExcel.DecimalNumber:
                    subtotalFormat = EnumFormatTypeExcel.DecimalNumberWithCero;
                    break;

                  case EnumFormatTypeExcel.Percent:
                    subtotalFormat = EnumFormatTypeExcel.PercentWithCero;
                    break;
                }
              }

            //Le aplicacamos el formato a la celda.
            wsData.Cells[rowNumber, format.Order].Style.Numberformat.Format = GetFormat(subtotalFormat);
            //S no es una columna calculada.
            if (!format.IsCalculated)
              {
              //Aplicamos la funcion configurada.
              switch (format.Function)
                {
                  case DataFieldFunctions.Sum:
                    wsData.Cells[rowNumber, format.Order].Formula = "=SUM(" + wsData.Cells[dataIniRow, format.Order, rowNumber - 1, format.Order].Address + ")";
                    break;

                  case DataFieldFunctions.Average:
                    wsData.Cells[rowNumber, format.Order].Formula = "=AVERAGE(" + wsData.Cells[dataIniRow, format.Order, rowNumber - 1, format.Order].Address + ")";
                    break;

                  case DataFieldFunctions.Count:
                    if (format.Format == EnumFormatTypeExcel.General)
                      wsData.Cells[rowNumber, format.Order].Formula = "=COUNTA(" + wsData.Cells[dataIniRow, format.Order, rowNumber - 1, format.Order].Address + ")";
                    break;
                }
              }
              else
              //Aplicamos la formula configurada.
              wsData.Cells[rowNumber, format.Order].Formula = GetFormula(formatTable, format.Formula, rowNumber);
            });
          }

          #endregion Simple
        }

        //Ajustamos las celdas a su contenido.
        //wsData.Cells[totalFilterRows + 5, 1, rowNumber, dtTable.Columns.Count].AutoFitColumns();

        #region CreateSuperHeader
        // Se Agregan los superheaders
        if (formatTableColumns.Any(c => c.SuperHeader != null))
        {//Selecciona las columnas que tengan SuperHeader
          var formatHeaders = formatTableColumns.Where(c => c.SuperHeader != null).ToList();
          //Se obtienen los datos iniciales para empezar a pintar la tabla
          //El start column se usa como una variable por si se llega a insertar algun column antes de iniciar la tabla
          var startColumn = 0;
          var startRow = totalFilterRows;
          var rowCount = formatTableColumns.Count(c => !(c.IsGroup));
          //Selecciona las columnas que sean Values o Row y las agrupa por Order y SuperHeader
          var formatHeadersValue = formatHeaders.Where(c => c.Axis == ePivotFieldAxis.Values || c.Axis == ePivotFieldAxis.Row).OrderBy(c => c.Order).GroupBy(c => c.SuperHeader).ToList();
          //Recorre las folas obtenidas para pintarlas en el Excel
          formatHeadersValue.ForEach(v =>
          {
            var fromCol = 0;
            for (var i = 0; i < v.Count(); i++)
            {
            //Obtiene el valor del Superheader
            var value = v.ElementAt(i);
            //Si fromCol = 0 se le asigna el valor a iniciar
            if (fromCol == 0)
                fromCol = value.Order + startColumn;
            //Se obtiene el valor del siguiente elemento 
            var valueNext = v.ElementAtOrDefault(i + 1);
            //si el siguente elemento tiene el mismo superHeader y es el consecutivo del actual se pasa a este inmediatamente
            if (valueNext != null && valueNext.Order == value.Order + 1) continue;
            //Si no se cumple la condicion anterior se pinta el superheader
            //Se obtiene la Columna final del superheader
            var toCol = value.Order + startColumn;

              using (var range = wsData.Cells[startRow, fromCol, startRow, toCol])
              {
                range.Value = value.SuperHeader;
                range.Style.Font.Bold = true;
                range.Merge = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.Cyan);
                range.Style.Border.BorderAround(ExcelBorderStyle.Medium);
              }
              fromCol = 0;
            }
          });
        }
        #endregion

        //Ajustamos las columnas al contenido.
        AutoFitColumns(ref wsData, dtTable.Columns.Count,rowNumber);

        #region SaveFile

        if (fileFullPath == null)
        {
          var suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);
          pathFinalFile = SaveExcel(pk, suggestedFilaName);
        }
        else
          pathFinalFile = SaveExcelFilePath(pk, fileFullPath);
        #endregion
      }
      return pathFinalFile;
    }

    #endregion CreateExcelCustom

    #region CreateExcelCustomPivot

    /// <summary>
    /// Exporta un excel realizando un pivot a las columnas configuradas.
    /// </summary>
    /// <param name="dtTable"></param>
    /// <param name="filters"></param>
    /// <param name="reportName"></param>
    /// <param name="dateRangeFileName"></param>
    /// <param name="formatTable"></param>
    /// <param name="blnColumnGrandTotal"></param>
    /// <param name="blnRowGrandTotal"></param>
    /// <param name="blnShowSubtotal"></param>
    /// <param name="pivotedTable"></param>
    /// <param name="extraFieldHeader"></param>
    /// <param name="numRows"></param>
    /// <param name="fileFullPath">Opcional. Ruta completa del archivo</param>
    /// <returns> FileInfo </returns>
    /// <history>
    ///   [edgrodriguez] 11/04/2016  Created.
    ///   [aalcocer]    18/05/2016 Modified. Se agregan columnas calculadas
    ///   [edgrodriguez] 06/06/2016 Modified. Se agrega agrupaciones. Calculo de Subtotales por grupo. Y estilos.
    ///   [aalcocer]     06/06/2016 Modified. Se agrega la opcion de generar el reporte en de ruta completa del archivo
    /// </history>
    public static FileInfo CreateExcelCustomPivot(DataTable dtTable, List<Tuple<string, string>> filters,
      string reportName, string dateRangeFileName, List<ExcelFormatTable> formatTable, bool blnColumnGrandTotal = false,
      bool blnRowGrandTotal = false, bool blnShowSubtotal = false, DataTable pivotedTable = null,
      List<Tuple<string, dynamic, EnumFormatTypeExcel>> extraFieldHeader = null, int numRows = 0, string fileFullPath = null)
    {
      FileInfo pathFinalFile;

      using (var pk = new ExcelPackage())
      {

        var wsData = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
        var totalFilterRows = 0;

        //Creamos el encabezado del reporte. Filtros, Titulo.
        CreateReportHeader(filters, reportName, ref wsData, ref totalFilterRows, extraFieldHeader, numRows);

        //Obtenemos la fila inicial. Para dibujar la tabla.
        var rowNumber = totalFilterRows + 1 + formatTable.Count(c => c.Axis == ePivotFieldAxis.Column);

        //Obtenemos la tabla ya con las columnas pivote.
        if (pivotedTable == null)
          pivotedTable = GetPivotTable(formatTable, dtTable);

        var formatTableColumns = new List<ExcelFormatTable>();

        pivotedTable.Columns.Cast<DataColumn>().ToList().ForEach(col =>
        {
          var header = col.ColumnName.Split(separator);
          var format = formatTable.FirstOrDefault(ft => ft.PropertyName == ((header.Length == 1) ? header[0] : header[header.Length - 1]));
          string formulaPivot = format.Formula;
          if (header.Length > 1 && !string.IsNullOrEmpty(format.Formula))
          {
            var columns = Regex.Matches(format.Formula, @"(\[.*?\])+");
            foreach (var match in columns)
            {
              formulaPivot = formulaPivot.Replace(match.ToString(), $"[{string.Join("_", header.Take(header.Length - 1))}_{match.ToString().Replace("[", "").Replace("]", "")}]");
            }
          }
          if (format != null)
          {
            formatTableColumns.Add(new ExcelFormatTable
            {
              AggregateFunction = format.AggregateFunction,
              Alignment = format.Alignment,
              Axis = format.Axis,
              Format = format.Format,
              Formula = (string.IsNullOrEmpty(formulaPivot)) ? format.Formula : formulaPivot,
              Function = format.Function,
              IsCalculated = format.IsCalculated,
              IsGroup = format.IsGroup,
              IsVisible = format.IsVisible,
              Sort = format.Sort,
              SubTotalFunctions = format.SubTotalFunctions,
              SubtotalWithCero = format.SubtotalWithCero,
              Title = format.Title,
              PropertyName = col.ColumnName,
              Order = col.Ordinal + 1,
            });
          }
        });

        #region Creando Headers

        //Obtenemos los encabezados de la tabla pivote.
        var lstHeaders = pivotedTable.Columns.OfType<DataColumn>().Select(c => c.ColumnName.Split(separator)).ToList();
        //Obtenemos la cantidad maxima de superheaders.
        var iniValue = new int[pivotedTable.Columns.OfType<DataColumn>().Max(c => c.ColumnName.Split(separator).Length - 1)];
        var columnNumber = 1;
        //Recorremos los encabezados.
        foreach (var item in lstHeaders)
        {
          //Si solo hay un encabezado.
          if (item.Length == 1)
          {
            //Si no es un grupo.
            if (formatTable.First(c => c.PropertyName == item.First()).IsGroup || !formatTable.First(c => c.PropertyName == item.First()).IsVisible) continue;
            //Dibujamos el encabezado y aplicamos formato.
            using (var range = wsData.Cells[rowNumber, columnNumber])
            {
              range.Value = formatTable.First(c => c.PropertyName == item.First()).Title;
              range.Style.Fill.PatternType = ExcelFillStyle.Solid;
              range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
              range.Style.Font.Bold = true;
              range.Style.Font.Color.SetColor(Color.White);
              range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
            }
            columnNumber++;
          }
          //Si son mas de 1 encabezado.
          else if (item.Length > 1)
          {
            //Obtenemos la fila inicial para dibujar el encabezado.
            var rowHeader = rowNumber - (item.Length - 1);
            //Obtenemos el siguiente arreglo de encabezados.        
            var itemNext = (lstHeaders.IndexOf(item) + 1 < lstHeaders.Count) ? lstHeaders[lstHeaders.IndexOf(item) + 1] : null;

            //Si el arreglo posterior contiene valore.
            if (itemNext != null && itemNext.Length == item.Length)
            {
              //Recorremos la lista.
              for (var i = 0; i < item.Length; i++)
              {
                if (i < item.Length - 1)
                {
                  //Si el encabezado de la lista actual es igual al encabezado de la lista posterior y Si pertenecen al mismo encabezado superior
                  if (item[i] == itemNext[i] && (i == 0 || item[i - 1] == itemNext[i - 1]))
                  {
                    //Aumentamos la cantidad de celdas a combinar. (MERGE)
                    iniValue[i] = iniValue[i] + 1;
                  }
                  //Si el encabezado de la lista actual es diferente al encabezado de la lista posterior.
                  else
                  {
                    using (var range = wsData.Cells[rowHeader, columnNumber - iniValue[i], rowHeader, columnNumber])
                    {
                      //Dibujamos el encabezado.
                      range.Value = item[i];
                      //Combinamos las celdas.
                      range.Merge = true;
                      range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                      range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                      range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                      range.Style.Font.Bold = true;
                      range.Style.Font.Color.SetColor(Color.White);
                      range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
                    }
                    iniValue[i] = 0;//Reiniciamos el contador de celdas a combinar.
                  }
                }
                else if (i == item.Length - 1)
                {
                  using (var range = wsData.Cells[rowHeader, columnNumber])
                  {
                    range.Value = formatTable.First(c => c.PropertyName == item[i]).Title;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
                  }
                }
                rowHeader++;
              }
            }
            //Si el arreglo posterior esta vacio
            else
            {
              //Recorremos la lista de encabezados.
              for (var i = 0; i < item.Length; i++)
              {
                if (i < item.Length - 1)
                {
                  using (var range = wsData.Cells[rowHeader, columnNumber - iniValue[i], rowHeader, columnNumber])
                  {
                    //Dibujamos el encabezado.
                    range.Value = item[i];
                    //Combinamos las celdas.
                    range.Merge = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
                  }
                  iniValue[i] = 0;//Reiniciamos el contador de celdas a combinar.
                }
                else if (i == item.Length - 1)
                {
                  using (var range = wsData.Cells[rowHeader, columnNumber])
                  {
                    range.Value = formatTable.First(c => c.PropertyName == item[i]).Title;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
                  }
                }
                rowHeader++;
              }
            }
            columnNumber++;
          }
        }

        #endregion Creando Headers

        if (formatTable.Any(c => c.IsGroup))
        {
          var backgroundColorGroups = new List<ExcelFormatGroupHeaders> {
            new ExcelFormatGroupHeaders { BackGroundColor="#004E48", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#147F79", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#2D8B85", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#4CA09A", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left } };

          #region Agrupado

          rowNumber++;
          #region Obtenemos los encabezados de grupo y sus valores
          //Creamos la sentencia Linq para obtener los campos que se agruparán.
          var qfields = string.Join(", ", formatTable
            .Where(c => c.IsGroup)
            .OrderBy(c => c.Order).Select(x => "it[\"" + x.PropertyName + "\"] as " + x.PropertyName));

          //Obtenemos las agrupaciones y los registros de cada agrupacion.
          var qTable = pivotedTable
            .AsEnumerable()
            .AsQueryable()
            .GroupBy("new(" + qfields + ")", "it")
            .Select("new(Key as qgroup, it as Values)");

          #endregion

          //Lista de formulas para cada grupo. Teniendo como items las columnas que tienen la propiedad SubtotalFunction.
          var subtotalFormulas = new Dictionary<string, string>[formatTable.Count(c => c.IsGroup)];
          //Lista de grupos.       
          var dynamicListData = qTable.OfType<dynamic>().ToList();
          //Total de columnas que no son grupo.
          var totalColumns = pivotedTable.Columns.Count - formatTable.Count(c => c.IsGroup);
          var previousGroup = new string[subtotalFormulas.Length];
          //Recorremos la lista de grupos.
          for (var i = 0; i < dynamicListData.Count; i++)
          {
            var nextGroup = new string[subtotalFormulas.Length];
            //Obtenemos la informacion del item actual.
            object itemActual = dynamicListData[i].qgroup;
            //Obtenemos los headers de cada grupo.
            var groupsAct = itemActual.GetType().GetProperties().Select(c => c.GetValue(itemActual).ToString()).ToArray();
            //Si es el primer item de la lista o es el indice es mayor a cero y el primer grupo del item actual
            //es diferente al primer grupo del item anterior.
            if (i == 0 || (i > 0 && groupsAct[0] != previousGroup[0]))
            {
              //Dibujamos todos los headers de grupo.
              for (var j = 0; j < groupsAct.Length; j++)
              {

                wsData.Cells[rowNumber, 1].Value = groupsAct[j];
                using (var range = wsData.Cells[rowNumber, 1, rowNumber, totalColumns])
                {
                  range.Merge = true;
                  range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                  range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                  range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                  range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                  range.Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                }
                rowNumber++;
              }
              //asignamos el valor actual a la variable aux
              previousGroup = groupsAct;
            }
            else if (i > 0)
            {

              //Recorremos los encabezados(Niveles).
              for (var j = 0; j < groupsAct.Length; j++)
              {
                if (groupsAct[j] == previousGroup[j]) continue;
                //Si el nivel actual es diferente al valor anterior.
                if (groupsAct[j] != previousGroup[j])
                {
                  //Dibujamos el encabezado.
                  wsData.Cells[rowNumber, 1].Value = groupsAct[j];
                  using (var range = wsData.Cells[rowNumber, 1, rowNumber, totalColumns])
                  {
                    range.Merge = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                    range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                    range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                    range.Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                  }
                  rowNumber++;
                }
              }
              previousGroup = groupsAct;
            }
            //Obtenemos los datos.
            var dataValues = ((IEnumerable<DataRow>)dynamicListData[i].Values).CopyToDataTable();
            var camposGrupo = formatTable.Where(col => col.IsGroup).Select(col => col.PropertyName).ToList();

            //Eliminamos las columnas que fueron configuradas como grupo. Y obtenemos el formato de las columnas que se visualizaran en el reporte.
            dataValues.Columns.OfType<DataColumn>().ToList().ForEach(c =>
            {
              if (camposGrupo.Contains(c.ColumnName))
                dataValues.Columns.Remove(c);
              else
              {
                var format = formatTableColumns.FirstOrDefault(f => f.PropertyName == c.ColumnName);
                if (format != null && format.Order > 0)
                {
                  using (var range = wsData.Cells[rowNumber, format.Order, rowNumber + dataValues.Rows.Count, format.Order])
                  {
                    range.Style.Numberformat.Format = GetFormat(format.Format);
                  }
                }
              }
            });

            //Agregamos los datos al excel.
            using (var range = wsData.Cells[rowNumber, 1].LoadFromDataTable(dataValues, false))
            {
              //Aplicamos estilo a las celdas.
              range.Style.Border.Top.Style = range.Style.Border.Right.Style = range.Style.Border.Left.Style = range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
              range.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Font.Size = 9;
            }

            rowNumber += dataValues.Rows.Count;

            //Si se mostrará el subtotal de cada grupo.
            if (blnShowSubtotal)
            {
              //Obtenemos la fila inicial.
              var dataIniRow = rowNumber - dataValues.Rows.Count;
              //Si el indice siguiente es menor que la cantidad total de items.
              if (i + 1 < dynamicListData.Count)
              {
                //Obtenemos objeto con los encabezados de grupo.
                object nextItem = dynamicListData[i + 1].qgroup;
                //Obtenemos el arreglo de encabezados de grupo. Los niveles estan de acuerdo al indice del arreglo.
                nextGroup = nextItem.GetType().GetProperties().Select(c => c.GetValue(nextItem).ToString()).ToArray();
                //Si el primer nivel de cada arreglo son diferentes.
                if (groupsAct[0] != nextGroup[0])
                  nextGroup = new string[groupsAct.Length];//Limpiamos la lista
              }
              //Recorremos los niveles del arreglo actual.
              for (var j = groupsAct.Length - 1; j >= 0; j--)
              {
                //Si los valores del index actual de cada lista son diferentes o el valor del index de la siguiente lista esta vacia o nula.
                if (groupsAct[j] != nextGroup[j] || string.IsNullOrEmpty(nextGroup[j]))
                {
                  //Recorremos las columnas.
                  foreach (var format in formatTableColumns)
                  {
                    using (var range = wsData.Cells[rowNumber, format.Order])
                    {
                      if (format.SubTotalFunctions == eSubTotalFunctions.None && format.Formula == null) continue;

                      var subtotalFormat = format.Format;
                      if (format.SubtotalWithCero)
                      {
                        switch (format.Format)
                        {
                          case EnumFormatTypeExcel.Number:
                            subtotalFormat = EnumFormatTypeExcel.NumberWithCero;
                            break;
                          case EnumFormatTypeExcel.DecimalNumber:
                            subtotalFormat = EnumFormatTypeExcel.DecimalNumberWithCero;
                            break;
                          case EnumFormatTypeExcel.Percent:
                            subtotalFormat = EnumFormatTypeExcel.PercentWithCero;
                            break;
                        }
                      }

                      //Le aplicamos el formato a la celda.
                      range.Style.Numberformat.Format = GetFormat(subtotalFormat);


                      //Si no es calculada aplicamos la funcion configurada.
                      if (!format.IsCalculated)
                      {
                        var formula = "";
                        //Si es el ultimo nivel
                        if (j == groupsAct.Length - 1)
                          formula = wsData.Cells[dataIniRow, format.Order, rowNumber - 1, format.Order].Address;//Aplicamos la seleccion segun la cantidad de registros que tenga el grupo.
                        else
                        {
                          //Si son antes del ultimo nivel.
                          var index = j + 1;//Se obtiene el indice actual mas 1
                          while (formula == "")
                          {
                            //Si el indice devuelve un valor nulo, se incrementa.
                            if (subtotalFormulas[index] == null) { index++; continue; }
                            //Obtenemos las posiciones de cada subtotal.
                            formula = subtotalFormulas[index][format.PropertyName];

                            //Si no es el ultimo nivel y el nivel actual de cada arreglo son diferentes o el nivel actual del arreglo siguiente  es nulo.
                            if (j < groupsAct.Length - 1 && (groupsAct[j] != nextGroup[j] || string.IsNullOrEmpty(nextGroup[j])))
                            {
                              //Limpiamos la formula.
                              subtotalFormulas[index][format.PropertyName] = string.Empty;
                            }
                          }
                        }
                        switch (format.SubTotalFunctions)
                        {
                          case eSubTotalFunctions.Sum:
                            range.Formula = "=SUM(" + formula + ")";
                            break;
                          case eSubTotalFunctions.Avg:
                            range.Formula = "=AVERAGE(" + formula + ")";
                            break;
                          case eSubTotalFunctions.Count:
                            if (format.Format == EnumFormatTypeExcel.General)
                              range.Formula = (j == groupsAct.Length - 1) ? "= COUNTA(" + formula + ")" : "= SUM(" + formula + ")";
                            break;
                        }
                        if (subtotalFormulas[j] != null && subtotalFormulas[j].ContainsKey(format.PropertyName))
                        {
                          subtotalFormulas[j][format.PropertyName] += (subtotalFormulas[j][format.PropertyName] == string.Empty) ? range.Address : "," + range.Address;
                        }
                        else
                        {
                          if (subtotalFormulas[j] == null) subtotalFormulas[j] = new Dictionary<string, string>();
                          subtotalFormulas[j].Add(format.PropertyName, range.Address);
                        }
                      }
                      else
                        //Obtenemos la formula.
                        range.Formula = GetFormula(formatTableColumns, format.Formula, rowNumber);
                    }
                  }
                  var firstSubtotalColumn = formatTable.First(c => c.SubTotalFunctions != eSubTotalFunctions.None || c.Formula != null).Order;
                  using (var range = wsData.Cells[rowNumber, firstSubtotalColumn, rowNumber, totalColumns])
                  {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                    range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                    range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                  }
                  rowNumber += 2;
                }
              }
            }
            rowNumber++;
          }

          if (blnRowGrandTotal)
          {
            formatTableColumns.ForEach(format =>
            {
              using (var range = wsData.Cells[rowNumber - 1, format.Order])
              {
                if (!format.IsCalculated)
                {
                  switch (format.SubTotalFunctions)
                  {
                    case eSubTotalFunctions.Sum:
                      range.Formula = "=SUM(" + subtotalFormulas[0][format.PropertyName] + ")";
                      break;
                    case eSubTotalFunctions.Avg:
                      range.Formula = "=AVERAGE(" + subtotalFormulas[0][format.PropertyName] + ")";
                      break;
                    case eSubTotalFunctions.Count:
                      if (format.Format == EnumFormatTypeExcel.General)
                        range.Formula = "= SUM(" + subtotalFormulas[0][format.PropertyName] + ")";
                      break;
                  }
                }
                else
                  range.Formula = GetFormula(formatTableColumns, format.Formula, rowNumber - 1);
                range.Style.Numberformat.Format = GetFormat(format.Format);
              }
            });
            using (var range = wsData.Cells[rowNumber - 1, 1, rowNumber - 1, totalColumns])
            {
              range.Style.Fill.PatternType = ExcelFillStyle.Solid;
              range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].BackGroundColor));
              range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].FontColor));
              range.Style.Font.Bold = backgroundColorGroups[backgroundColorGroups.Count - 1].FontBold;
            }
          }
          //Ajustamos todas las columnas a su contenido.
          //wsData.Cells[totalFilterRows + 5, 1, rowNumber, totalColumns].AutoFitColumns();

          #endregion Agrupado
        }
        else
        {
          #region Agregando Datos

          //Eliminamos las columnas que fueron configuradas como No Visibles. Y obtenemos el formato de las columnas que se visualizaran en el reporte.
          pivotedTable.Columns.OfType<DataColumn>().ToList().ForEach(c =>
          {
            var format = formatTableColumns.FirstOrDefault(f => f.PropertyName == c.ColumnName);
            if (format != null && !format.IsVisible)
            {
              pivotedTable.Columns.Remove(c);
            }
          });

          rowNumber++;
          var columnIndex = 1;
          var rowEnd = rowNumber + pivotedTable.Rows.Count;
          pivotedTable.Columns.Cast<DataColumn>().ToList().ForEach(col =>
          {
            var columnN = col.ColumnName.Split(separator);
            if (columnN.Length == 1)
            {
              var format = GetFormat(formatTable.First(c => c.PropertyName == columnN[0]).Format);
              if (format != "")
              {
                using (var range = wsData.Cells[rowNumber, columnIndex, rowEnd, columnIndex])
                {
                  range.Style.Numberformat.Format = format;
                  range.Style.Font.Size = 9;
                }
              }
            }
            else
            {
              var format = GetFormat(formatTable.First(c => c.PropertyName == columnN[columnN.Length - 1]).Format);
              if (format != "")
              {
                using (var range = wsData.Cells[rowNumber, columnIndex, rowEnd, columnIndex])
                {
                  range.Style.Numberformat.Format = format;
                  range.Style.Font.Size = 9;
                }
              }
            }
            columnIndex++;
          });

          //El contenido lo convertimos a una tabla
          wsData.Cells[rowNumber, 1].LoadFromDataTable(pivotedTable, false);
          rowNumber += pivotedTable.Rows.Count;

          if (blnRowGrandTotal)
          {
            columnIndex = 1;
            pivotedTable.Columns.Cast<DataColumn>().ToList().ForEach(col =>
            {
              var columnN = col.ColumnName.Split(separator);
              var colN = (columnN.Length == 1) ? columnN[0] : columnN[columnN.Length - 1];

              var formatCol = formatTable.First(ft => ft.PropertyName == colN);
              if (!formatCol.IsGroup)
              {
                var subtotalFormat = formatCol.Format;
                if (formatCol.SubtotalWithCero)
                {
                  switch (formatCol.Format)
                  {
                    case EnumFormatTypeExcel.Number:
                      subtotalFormat = EnumFormatTypeExcel.NumberWithCero;
                      break;
                    case EnumFormatTypeExcel.DecimalNumber:
                      subtotalFormat = EnumFormatTypeExcel.DecimalNumberWithCero;
                      break;
                    case EnumFormatTypeExcel.Percent:
                      subtotalFormat = EnumFormatTypeExcel.PercentWithCero;
                      break;
                  }
                }
                using (var range = wsData.Cells[rowNumber, columnIndex])
                {
                  range.Style.Numberformat.Format = GetFormat(subtotalFormat);
                  switch (formatCol.Function)
                  {
                    case DataFieldFunctions.Sum:
                      range.Formula = "=SUM(" + wsData.Cells[rowNumber - pivotedTable.Rows.Count, columnIndex, rowNumber - 1, columnIndex].Address + ")";
                      break;
                  }
                }
                columnIndex++;
              }
            });
            using (var range = wsData.Cells[rowNumber, 1, rowNumber, pivotedTable.Columns.Count])
            {
              range.Style.Fill.PatternType = ExcelFillStyle.Solid;
              range.Style.Fill.BackgroundColor.SetColor(Color.Black);
              range.Style.Font.Bold = true;
              range.Style.Font.Color.SetColor(Color.White);
            }
            rowNumber++;
          }
          //wsData.Cells[totalFilterRows, 1, rowNumber, pivotedTable.Columns.Count].AutoFitColumns();

          #endregion Agregando Datos
        }

        AutoFitColumns(ref wsData, pivotedTable.Columns.Count,rowNumber);

        if (fileFullPath == null)
        {
          var suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);
          pathFinalFile = SaveExcel(pk, suggestedFilaName);
        }
        else
          pathFinalFile = SaveExcelFilePath(pk, fileFullPath);
      }
      return pathFinalFile;
    }

    #endregion CreateExcelCustomPivot

    #region ExportRptWeeklyMonthlyHostess

    /// <summary>
    /// Exporta un excel realizando un pivot a las columnas configuradas.
    /// </summary>
    /// <param name="dtTable"></param>
    /// <param name="filters"></param>
    /// <param name="reportName"></param>
    /// <param name="dateRangeFileName"></param>
    /// <param name="formatTable"></param>
    /// <param name="blnColumnGrandTotal"></param>
    /// <param name="blnRowGrandTotal"></param>
    /// <param name="blnShowSubtotal"></param>
    /// <param name="pivotedTable"></param>
    /// <param name="extraFieldHeader"></param>
    /// <param name="numRows"></param>
    /// <param name="fileFullPath">Opcional. Ruta completa del archivo</param>
    /// <returns> FileInfo </returns>
    /// <history>
    ///   [edgrodriguez] 09/06/2016  Created.
    /// </history>
    public static FileInfo ExportRptWeeklyMonthlyHostess(List<Tuple<DataTable, List<ExcelFormatTable>, string>> Data, List<Tuple<string, string>> filters,
      string reportName, string dateRangeFileName, bool blnColumnGrandTotal = false,
      bool blnRowGrandTotal = false, bool blnShowSubtotal = false, string fileFullPath = null)
    {
      FileInfo pathFinalFile;
      using (var pk = new ExcelPackage())
      {
        foreach (var pair in Data)
        {
          var wsData = pk.Workbook.Worksheets.Add(Regex.Replace(pair.Item3, "[^a-zA-Z0-9_]+", " "));
          var totalFilterRows = 0;

          //Creamos el encabezado del reporte. Filtros, Titulo.
          CreateReportHeader(filters, reportName, ref wsData, ref totalFilterRows);

          if (pair.Item1.Rows.Count > 0)
          {
            var columnNumber = 1;

            //Obtenemos la fila inicial. Para dibujar la tabla.
            var rowNumber = totalFilterRows + 1 + pair.Item2.Count(c => c.Axis == ePivotFieldAxis.Column);

            //Obtenemos la tabla ya con las columnas pivote.
            var pivotedTable = GetPivotTable(pair.Item2, pair.Item1);

            var formatTableColumns = new List<ExcelFormatTable>();

            pivotedTable.Columns.Cast<DataColumn>().ToList().ForEach(col =>
            {
              var header = col.ColumnName.Split(separator);
              var format = pair.Item2.FirstOrDefault(ft => ft.PropertyName == ((header.Length == 1) ? header[0] : header[header.Length - 1]));
              string formulaPivot = format.Formula;
              if (header.Length > 1 && !string.IsNullOrEmpty(format.Formula))
              {
                var columns = Regex.Matches(format.Formula, @"(\[.*?\])+");
                foreach (var match in columns)
                {
                  formulaPivot = formulaPivot.Replace(match.ToString(), $"[{string.Join("_", header.Take(header.Length - 1))}_{match.ToString().Replace("[", "").Replace("]", "")}]");
                }
              }

              if (format != null)
              {
                formatTableColumns.Add(new ExcelFormatTable
                {
                  AggregateFunction = format.AggregateFunction,
                  Alignment = format.Alignment,
                  Axis = format.Axis,
                  Format = format.Format,
                  Formula = (string.IsNullOrEmpty(formulaPivot)) ? format.Formula : formulaPivot,
                  Function = format.Function,
                  IsCalculated = format.IsCalculated,
                  IsGroup = format.IsGroup,
                  IsVisible = format.IsVisible,
                  Sort = format.Sort,
                  SubTotalFunctions = format.SubTotalFunctions,
                  SubtotalWithCero = format.SubtotalWithCero,

                  Title = format.Title,
                  PropertyName = col.ColumnName,
                  Order = col.Ordinal + 1,
                });
              }
            });

            #region Creando Headers

            //Obtenemos los encabezados de la tabla pivote.
            var lstHeaders = pivotedTable.Columns.OfType<DataColumn>().Select(c => c.ColumnName.Split(separator)).ToList();
            //Obtenemos la cantidad maxima de superheaders.
            var iniValue = new int[pivotedTable.Columns.OfType<DataColumn>().Max(c => c.ColumnName.Split(separator).Length - 1)];

            //Recorremos los encabezados.
            foreach (var item in lstHeaders)
            {
              //Si solo hay un encabezado.
              if (item.Length == 1)
              {
                //Si no es un grupo.
                if (pair.Item2.First(c => c.PropertyName == item.First()).IsGroup || !pair.Item2.First(c => c.PropertyName == item.First()).IsVisible) continue;
                //Dibujamos el encabezado y aplicamos formato.
                using (var range = wsData.Cells[rowNumber, columnNumber])
                {
                  range.Value = pair.Item2.First(c => c.PropertyName == item.First()).Title;
                  range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                  range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                  range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                  range.Style.Font.Bold = true;
                  range.Style.Font.Color.SetColor(Color.White);
                  range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
                }
                columnNumber++;
              }
              //Si son mas de 1 encabezado.
              else if (item.Length > 1)
              {
                //Obtenemos la fila inicial para dibujar el encabezado.
                var rowHeader = rowNumber - (item.Length - 1);
                //Obtenemos el siguiente arreglo de encabezados.        
                var itemNext = (lstHeaders.IndexOf(item) + 1 < lstHeaders.Count) ? lstHeaders[lstHeaders.IndexOf(item) + 1] : null;

                //Si el arreglo posterior contiene valore.
                if (itemNext != null && itemNext.Length == item.Length)
                {
                  //Recorremos la lista.
                  for (var i = 0; i < item.Length; i++)
                  {
                    if (i < item.Length - 1)
                    {
                      //Si el encabezado de la lista actual es igual al encabezado de la lista posterior y Si pertenecen al mismo encabezado superior
                      if (item[i] == itemNext[i] && (i == 0 || item[i - 1] == itemNext[i - 1]))
                      {
                        //Aumentamos la cantidad de celdas a combinar. (MERGE)
                        iniValue[i] = iniValue[i] + 1;
                      }
                      //Si el encabezado de la lista actual es diferente al encabezado de la lista posterior.
                      else
                      {
                        using (var range = wsData.Cells[rowHeader, columnNumber - iniValue[i], rowHeader, columnNumber])
                        {
                          //Dibujamos el encabezado.
                          range.Value = item[i];
                          //Combinamos las celdas.
                          range.Merge = true;
                          range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                          range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                          range.Style.Fill.BackgroundColor.SetColor(Color.Black);
                          range.Style.Font.Bold = true;
                          range.Style.Font.Color.SetColor(Color.White);
                          range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
                        }
                        iniValue[i] = 0;//Reiniciamos el contador de celdas a combinar.
                      }
                    }
                    else if (i == item.Length - 1)
                    {
                      using (var range = wsData.Cells[rowHeader, columnNumber])
                      {
                        range.Value = pair.Item2.First(c => c.PropertyName == item[i]).Title;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.Fill.BackgroundColor.SetColor(Color.Black);
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(Color.White);
                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
                      }
                    }
                    rowHeader++;
                  }
                }
                //Si el arreglo posterior esta vacio
                else
                {
                  //Recorremos la lista de encabezados.
                  for (var i = 0; i < item.Length; i++)
                  {
                    if (i < item.Length - 1)
                    {
                      using (var range = wsData.Cells[rowHeader, columnNumber - iniValue[i], rowHeader, columnNumber])
                      {
                        //Dibujamos el encabezado.
                        range.Value = item[i];
                        //Combinamos las celdas.
                        range.Merge = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.Fill.BackgroundColor.SetColor(Color.Black);
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(Color.White);
                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
                      }
                      iniValue[i] = 0;//Reiniciamos el contador de celdas a combinar.
                    }
                    else if (i == item.Length - 1)
                    {
                      using (var range = wsData.Cells[rowHeader, columnNumber])
                      {
                        range.Value = pair.Item2.First(c => c.PropertyName == item[i]).Title;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.Fill.BackgroundColor.SetColor(Color.Black);
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(Color.White);
                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
                      }
                    }
                    rowHeader++;
                  }
                }
                columnNumber++;
              }
            }

            #endregion Creando Headers

            if (pair.Item2.Any(c => c.IsGroup))
            {
              var backgroundColorGroups = new List<ExcelFormatGroupHeaders> {
            new ExcelFormatGroupHeaders { BackGroundColor="#004E48", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#147F79", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#2D8B85", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#4CA09A", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left } };

              #region Agrupado

              rowNumber++;
              #region Obtenemos los encabezados de grupo y sus valores
              //Creamos la sentencia Linq para obtener los campos que se agruparán.
              var qfields = string.Join(", ", pair.Item2
                .Where(c => c.IsGroup)
                .OrderBy(c => c.Order).Select(x => "it[\"" + x.PropertyName + "\"] as " + x.PropertyName));

              //Obtenemos las agrupaciones y los registros de cada agrupacion.
              var qTable = pivotedTable
                .AsEnumerable()
                .AsQueryable()
                .GroupBy("new(" + qfields + ")", "it")
                .Select("new(Key as qgroup, it as Values)");

              #endregion

              //Lista de formulas para cada grupo. Teniendo como items las columnas que tienen la propiedad SubtotalFunction.
              var subtotalFormulas = new Dictionary<string, string>[pair.Item2.Count(c => c.IsGroup)];
              //Lista de grupos.       
              var dynamicListData = qTable.OfType<dynamic>().ToList();
              //Total de columnas que no son grupo.
              var totalColumns = pivotedTable.Columns.Count - pair.Item2.Count(c => c.IsGroup);
              var previousGroup = new string[subtotalFormulas.Length];
              //Recorremos la lista de grupos.
              for (var i = 0; i < dynamicListData.Count; i++)
              {
                var nextGroup = new string[subtotalFormulas.Length];
                //Obtenemos la informacion del item actual.
                object itemActual = dynamicListData[i].qgroup;
                //Obtenemos los headers de cada grupo.
                var groupsAct = itemActual.GetType().GetProperties().Select(c => c.GetValue(itemActual).ToString()).ToArray();
                //Si es el primer item de la lista o es el indice es mayor a cero y el primer grupo del item actual
                //es diferente al primer grupo del item anterior.
                if (i == 0 || (i > 0 && groupsAct[0] != previousGroup[0]))
                {
                  //Dibujamos todos los headers de grupo.
                  for (var j = 0; j < groupsAct.Length; j++)
                  {

                    wsData.Cells[rowNumber, 1].Value = groupsAct[j];
                    using (var range = wsData.Cells[rowNumber, 1, rowNumber, totalColumns])
                    {
                      range.Merge = true;
                      range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                      range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                      range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                      range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                      range.Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                    }
                    rowNumber++;
                  }
                  //asignamos el valor actual a la variable aux
                  previousGroup = groupsAct;
                }
                else if (i > 0)
                {

                  //Recorremos los encabezados(Niveles).
                  for (var j = 0; j < groupsAct.Length; j++)
                  {
                    if (groupsAct[j] == previousGroup[j]) continue;
                    //Si el nivel actual es diferente al valor anterior.
                    if (groupsAct[j] != previousGroup[j])
                    {
                      //Dibujamos el encabezado.
                      wsData.Cells[rowNumber, 1].Value = groupsAct[j];
                      using (var range = wsData.Cells[rowNumber, 1, rowNumber, totalColumns])
                      {
                        range.Merge = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                        range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                        range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                        range.Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                      }
                      rowNumber++;
                    }
                  }
                  previousGroup = groupsAct;
                }
                //Obtenemos los datos.
                var dataValues = ((IEnumerable<DataRow>)dynamicListData[i].Values).CopyToDataTable();
                var camposGrupo = pair.Item2.Where(col => col.IsGroup).Select(col => col.PropertyName).ToList();

                //Eliminamos las columnas que fueron configuradas como grupo. Y obtenemos el formato de las columnas que se visualizaran en el reporte.
                dataValues.Columns.OfType<DataColumn>().ToList().ForEach(c =>
                {
                  var format = formatTableColumns.FirstOrDefault(f => f.PropertyName == c.ColumnName);
                  if (camposGrupo.Contains(c.ColumnName) || (format != null && !format.IsVisible))
                    dataValues.Columns.Remove(c);
                  else
                  {
                    if (format != null && format.Order > 0)
                    {
                      using (var range = wsData.Cells[rowNumber, format.Order, rowNumber + dataValues.Rows.Count, format.Order])
                      {
                        range.Style.Numberformat.Format = GetFormat(format.Format);
                      }
                    }
                  }
                });

                //Agregamos los datos al excel.
                using (var range = wsData.Cells[rowNumber, 1].LoadFromDataTable(dataValues, false))
                {
                  //Aplicamos estilo a las celdas.
                  range.Style.Border.Top.Style = range.Style.Border.Right.Style = range.Style.Border.Left.Style = range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                  range.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
                  range.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
                  range.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
                  range.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
                  range.Style.Font.Size = 9;
                }

                rowNumber += dataValues.Rows.Count;

                //Si se mostrará el subtotal de cada grupo.
                if (blnShowSubtotal)
                {
                  //Obtenemos la fila inicial.
                  var dataIniRow = rowNumber - dataValues.Rows.Count;
                  //Si el indice siguiente es menor que la cantidad total de items.
                  if (i + 1 < dynamicListData.Count)
                  {
                    //Obtenemos objeto con los encabezados de grupo.
                    object nextItem = dynamicListData[i + 1].qgroup;
                    //Obtenemos el arreglo de encabezados de grupo. Los niveles estan de acuerdo al indice del arreglo.
                    nextGroup = nextItem.GetType().GetProperties().Select(c => c.GetValue(nextItem).ToString()).ToArray();
                    //Si el primer nivel de cada arreglo son diferentes.
                    if (groupsAct[0] != nextGroup[0])
                      nextGroup = new string[groupsAct.Length];//Limpiamos la lista
                  }
                  //Recorremos los niveles del arreglo actual.
                  for (var j = groupsAct.Length - 1; j >= 0; j--)
                  {
                    //Si los valores del index actual de cada lista son diferentes o el valor del index de la siguiente lista esta vacia o nula.
                    if (groupsAct[j] != nextGroup[j] || string.IsNullOrEmpty(nextGroup[j]))
                    {
                      //Recorremos las columnas.
                      foreach (var format in formatTableColumns)
                      {
                        using (var range = wsData.Cells[rowNumber, format.Order])
                        {
                          if (format.SubTotalFunctions == eSubTotalFunctions.None && format.Formula == null) continue;

                          var subtotalFormat = format.Format;
                          if (format.SubtotalWithCero)
                          {
                            switch (format.Format)
                            {
                              case EnumFormatTypeExcel.Number:
                                subtotalFormat = EnumFormatTypeExcel.NumberWithCero;
                                break;
                              case EnumFormatTypeExcel.DecimalNumber:
                                subtotalFormat = EnumFormatTypeExcel.DecimalNumberWithCero;
                                break;
                              case EnumFormatTypeExcel.Percent:
                                subtotalFormat = EnumFormatTypeExcel.PercentWithCero;
                                break;
                            }
                          }

                          //Le aplicacamos el formato a la celda.
                          range.Style.Numberformat.Format = GetFormat(subtotalFormat);


                          //Si no es calculada aplicamos la funcion configurada.
                          if (!format.IsCalculated)
                          {
                            var formula = "";
                            //Si es el ultimo nivel
                            if (j == groupsAct.Length - 1)
                            {
                              formula = wsData.Cells[dataIniRow, format.Order, rowNumber - 1, format.Order].Address;//Aplicamos la seleccion segun la cantidad de registros que tenga el grupo.
                              if (!string.IsNullOrEmpty(format.Formula))
                              {
                                using (var range2 = wsData.Cells[rowNumber + 1, format.Order])
                                {
                                  range2.Formula = GetFormula(formatTableColumns, format.Formula, rowNumber);
                                  range2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                  range2.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                                  range2.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                                  range2.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                                  range2.Style.Numberformat.Format = GetFormat(EnumFormatTypeExcel.Percent);
                                }
                              }
                            }
                            else
                            {
                              //Si son antes del ultimo nivel.
                              var index = j + 1;//Se obtiene el indice actual mas 1
                              while (formula == "")
                              {
                                //Si el indice devuelve un valor nulo, se incrementa.
                                if (subtotalFormulas[index] == null) { index++; continue; }
                                //Obtenemos las posiciones de cada subtotal.
                                formula = subtotalFormulas[index][format.PropertyName];

                                //Si no es el ultimo nivel y el nivel actual de cada arreglo son diferentes o el nivel actual del arreglo siguiente  es nulo.
                                if (j < groupsAct.Length - 1 && (groupsAct[j] != nextGroup[j] || string.IsNullOrEmpty(nextGroup[j])))
                                {
                                  //Limpiamos la formula.
                                  subtotalFormulas[index][format.PropertyName] = string.Empty;
                                }
                              }
                            }
                            switch (format.SubTotalFunctions)
                            {
                              case eSubTotalFunctions.Sum:
                                range.Formula = "=SUM(" + formula + ")";
                                break;
                              case eSubTotalFunctions.Avg:
                                range.Formula = "=AVERAGE(" + formula + ")";
                                break;
                              case eSubTotalFunctions.Count:
                                if (format.Format == EnumFormatTypeExcel.General)
                                  range.Formula = (j == groupsAct.Length - 1) ? "= COUNTA(" + formula + ")" : "= SUM(" + formula + ")";
                                break;
                            }
                            if (subtotalFormulas[j] != null && subtotalFormulas[j].ContainsKey(format.PropertyName))
                            {
                              subtotalFormulas[j][format.PropertyName] += (subtotalFormulas[j][format.PropertyName] == string.Empty) ? range.Address : "," + range.Address;
                            }
                            else
                            {
                              if (subtotalFormulas[j] == null) subtotalFormulas[j] = new Dictionary<string, string>();
                              subtotalFormulas[j].Add(format.PropertyName, range.Address);
                            }
                          }
                          else
                            //Obtenemos la formula.
                            range.Formula = GetFormula(pair.Item2, format.Formula, rowNumber);
                        }
                      }
                      var firstSubtotalColumn = pair.Item2.First(c => c.SubTotalFunctions != eSubTotalFunctions.None || c.Formula != null).Order;
                      using (var range = wsData.Cells[rowNumber, firstSubtotalColumn, rowNumber, totalColumns])
                      {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                        range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                        range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                      }
                      rowNumber += 2;
                    }
                  }
                }
                rowNumber++;
              }

              if (blnRowGrandTotal)
              {
                formatTableColumns.ForEach(format =>
                {
                  using (var range = wsData.Cells[rowNumber - 1, format.Order])
                  {
                    if (!format.IsCalculated)
                    {
                      switch (format.SubTotalFunctions)
                      {
                        case eSubTotalFunctions.Sum:
                          range.Formula = "=SUM(" + subtotalFormulas[0][format.PropertyName] + ")";
                          break;
                        case eSubTotalFunctions.Avg:
                          range.Formula = "=AVERAGE(" + subtotalFormulas[0][format.PropertyName] + ")";
                          break;
                        case eSubTotalFunctions.Count:
                          if (format.Format == EnumFormatTypeExcel.General)
                            range.Formula = "= SUM(" + subtotalFormulas[0][format.PropertyName] + ")";
                          break;
                      }

                      if (!string.IsNullOrEmpty(format.Formula))
                      {
                        using (var range2 = wsData.Cells[rowNumber, format.Order])
                        {
                          range2.Formula = GetFormula(formatTableColumns, format.Formula, rowNumber - 1);
                          range2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                          range2.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].BackGroundColor));
                          range2.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].FontColor));
                          range2.Style.Font.Bold = backgroundColorGroups[backgroundColorGroups.Count - 1].FontBold;
                          range2.Style.Numberformat.Format = GetFormat(EnumFormatTypeExcel.Percent);
                        }
                      }
                    }
                    else
                      range.Formula = GetFormula(formatTableColumns, format.Formula, rowNumber - 1);
                    range.Style.Numberformat.Format = GetFormat(format.Format);
                  }
                });
                using (var range = wsData.Cells[rowNumber - 1, 1, rowNumber - 1, totalColumns])
                {
                  range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                  range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].BackGroundColor));
                  range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].FontColor));
                  range.Style.Font.Bold = backgroundColorGroups[backgroundColorGroups.Count - 1].FontBold;
                }
              }
              //Ajustamos todas las columnas a su contenido.
              //wsData.Cells[totalFilterRows + 5, 1, rowNumber, totalColumns].AutoFitColumns();

              #endregion Agrupado
            }
            AutoFitColumns(ref wsData, pivotedTable.Columns.Count, rowNumber);
          }
          else
          {
            using (var range = wsData.Cells[totalFilterRows + 1, 2, totalFilterRows + 4, 12])
            {
              range.Value = "There is no info to make a report";
              range.Style.Font.Bold = true;
              range.Style.Font.Size = 36;
              range.Merge = true;
              range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
              range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              range.Style.Fill.PatternType = ExcelFillStyle.Solid;
              range.Style.Fill.BackgroundColor.SetColor(Color.Cyan);
              range.Style.Border.BorderAround(ExcelBorderStyle.Medium);
              range.AutoFitColumns();
            }
          }
        }

        if (fileFullPath == null)
        {
          var suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);
          pathFinalFile = SaveExcel(pk, suggestedFilaName);
        }
        else
          pathFinalFile = SaveExcelFilePath(pk, fileFullPath);
      }
      return pathFinalFile;
    }

    #endregion CreateExcelCustomPivot

    #region ExportRptManifestRangeByLS

    /// <summary>
    /// Exporta un excel realizando un pivot a las columnas configuradas.
    /// </summary>
    /// <param name="dtTable"></param>
    /// <param name="filters"></param>
    /// <param name="reportName"></param>
    /// <param name="dateRangeFileName"></param>
    /// <param name="formatTable"></param>
    /// <param name="blnColumnGrandTotal"></param>
    /// <param name="blnRowGrandTotal"></param>
    /// <param name="blnShowSubtotal"></param>
    /// <param name="pivotedTable"></param>
    /// <param name="extraFieldHeader"></param>
    /// <param name="numRows"></param>
    /// <param name="fileFullPath">Opcional. Ruta completa del archivo</param>
    /// <returns> FileInfo </returns>
    /// <history>
    ///   [edgrodriguez] 13/06/2016  Created.
    /// </history>
    public static FileInfo ExportRptManifestRangeByLs(List<Tuple<DataTable, List<ExcelFormatTable>>> Data, List<Tuple<string, string>> filters,
      string reportName, string dateRangeFileName, bool blnColumnGrandTotal = false,
      bool blnRowGrandTotal = false, bool blnShowSubtotal = false, string fileFullPath = null)
    {
      FileInfo pathFinalFile;
      using (var pk = new ExcelPackage())
      {
        var wsData = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
        var totalFilterRows = 0;
        var dtTable = Data[0].Item1;
        var formatTable = Data[0].Item2;
        var dtTableBookings = Data[1].Item1.Rows.Count > 0 ? GetPivotTable(Data[1].Item2, Data[1].Item1) : Data[1].Item1;
        var formatBookings = Data[1].Item2;
        //Creamos el encabezado
        CreateReportHeader(filters, reportName, ref wsData, ref totalFilterRows);

        var bookTime = dtTableBookings.Columns.OfType<DataColumn>().Where(c => c.ColumnName.Contains("guBookTime")).Select(col => new { index = col.Ordinal, Name = col.ColumnName }).ToList();
        bookTime.ForEach(c =>
        {
          dtTableBookings.Columns.Remove(c.Name);
          var col = new DataColumn { ColumnName = c.Name, DefaultValue = c.Name.Split(separator)[0] };
          dtTableBookings.Columns.Add(col);
          col.SetOrdinal(c.index);
        });


        var rowNumber = totalFilterRows + 1;

        //Obtenemos las columnas y las ordenamos.
        var formatTableColumns = formatTable.Where(c => !c.IsGroup && c.Order > 0 && c.IsVisible)
          .OrderBy(row => row.Order).ToList();

        #region Creando Headers
        //Agregando los headers de acuerdo al orden configurado.
        //descartando los campos configurados como grupos.
        dtTable.Columns.OfType<DataColumn>().ToList().ForEach(c =>
        {
          c.SetOrdinal(dtTable.Columns.Count - 1);
        });

        dtTable.Columns.OfType<DataColumn>().ToList().ForEach(c =>
        {
          var format = formatTable.FirstOrDefault(f => f.PropertyName == c.ColumnName);

          if (format != null && format.Order > 0 && format.IsVisible && !format.IsGroup)
          {
            dtTable.Columns[c.ColumnName].SetOrdinal(format.Order - 1);
            using (var range = wsData.Cells[rowNumber, format.Order])
            {
              range.Value = format.Title;
              range.Style.Fill.PatternType = ExcelFillStyle.Solid;
              range.Style.Font.Bold = true;
              range.Style.Font.Size = 12;
              range.Style.Fill.BackgroundColor.SetColor(Color.Black);
              range.Style.Font.Color.SetColor(Color.White);
              range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
            }
          }
          else if (format != null && !format.IsVisible)
          {
            dtTable.Columns.Remove(c);
          }
          else
            dtTable.Columns[c.ColumnName].SetOrdinal(dtTable.Columns.Count - 1);
        });
        rowNumber++;
        #endregion

        //Si existe algun grupo
        if (formatTable.Any(c => c.IsGroup))
        {
          #region Simple con Agrupado

          #region Formato para encabezados de grupo

          //Formato para los encabezados de grupo.
          var backgroundColorGroups = new List<ExcelFormatGroupHeaders> {
            new ExcelFormatGroupHeaders { BackGroundColor="#004E48", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#147F79", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#2D8B85", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#4CA09A", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            //Formato para la fila de Gran Total.
            new ExcelFormatGroupHeaders { BackGroundColor="#000000", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left }
        };
          #endregion

          #region Obtenemos los encabezados de grupo y sus valores
          //Creamos la sentencia Linq para obtener los campos que se agruparán.
          var qfields = string.Join(", ", formatTable
      .Where(c => c.IsGroup)
      .OrderBy(c => c.Order).Select(x => "it[\"" + x.PropertyName + "\"] as " + x.PropertyName));

          //Obtenemos las agrupaciones y los registros de cada agrupacion.
          var qTable = dtTable
            .AsEnumerable()
            .AsQueryable()
            .GroupBy("new(" + qfields + ")", "it")
            .Select("new(Key as qgroup, it as Values)");

          #endregion

          //Lista de formulas para cada grupo. Teniendo como items las columnas que tienen la propiedad SubtotalFunction.
          var subtotalFormulas = new Dictionary<string, string>[formatTable.Count(c => c.IsGroup)];
          //Lista de grupos.       
          var dynamicListData = qTable.OfType<dynamic>().ToList();
          //Total de columnas que no son grupo.
          var totalColumns = formatTable.Count(c => !c.IsGroup && c.IsVisible);
          var previousGroup = new string[subtotalFormulas.Length];
          //Recorremos la lista de grupos.
          for (var i = 0; i < dynamicListData.Count; i++)
          {
            var nextGroup = new string[subtotalFormulas.Length];
            //Obtenemos la informacion del item actual.
            object itemActual = dynamicListData[i].qgroup;
            //Obtenemos los headers de cada grupo.
            var groupsAct = itemActual.GetType().GetProperties().Select(c => c.GetValue(itemActual).ToString()).ToArray();
            //Si es el primer item de la lista o es el indice es mayor a cero y el primer grupo del item actual
            //es diferente al primer grupo del item anterior.
            if (i == 0 || (i > 0 && groupsAct[0] != previousGroup[0]))
            {
              //Dibujamos todos los headers de grupo.
              for (var j = 0; j < groupsAct.Length; j++)
              {
                if (j == groupsAct.Length - 1
                  && (groupsAct[0].ToString() == "MANIFEST"
                  || groupsAct[0].ToString() == "COURTESY TOUR"
                  || groupsAct[0].ToString() == "SAVE TOUR")
                  && dtTableBookings.AsEnumerable().Count(c => c["LocationN"].ToString() == groupsAct[j]) > 0)
                {
                  using (var range = wsData.Cells[rowNumber, 1, rowNumber, totalColumns])
                  {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                    range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                    range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                    range.Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                    range.LoadFromDataTable(dtTableBookings.AsEnumerable().Where(c => c["LocationN"].ToString() == groupsAct[j]).CopyToDataTable(), false);
                  }
                }
                else
                {
                  wsData.Cells[rowNumber, 1].Value = groupsAct[j];
                  using (var range = wsData.Cells[rowNumber, 1, rowNumber, totalColumns])
                  {
                    range.Merge = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                    range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                    range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                    range.Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                  }
                }
                rowNumber++;
              }
              //asignamos el valor actual a la variable aux
              previousGroup = groupsAct;
            }
            else if (i > 0)
            {
              //Recorremos los encabezados(Niveles).
              for (var j = 0; j < groupsAct.Length; j++)
              {
                if (groupsAct[j] == previousGroup[j]) continue;
                //Si el nivel actual es diferente al valor anterior.
                if (groupsAct[j] != previousGroup[j])
                {
                  if (j == groupsAct.Length - 1
                     && (groupsAct[0].ToString() == "MANIFEST"
                  || groupsAct[0].ToString() == "COURTESY TOUR"
                  || groupsAct[0].ToString() == "SAVE TOUR")
                    && dtTableBookings.AsEnumerable().Count(c => c["LocationN"].ToString() == groupsAct[j]) > 0)
                  {
                    using (var range = wsData.Cells[rowNumber, 1, rowNumber, totalColumns])
                    {
                      range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                      range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                      range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                      range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                      range.Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                      range.LoadFromDataTable(dtTableBookings.AsEnumerable().Where(c => c["LocationN"].ToString() == groupsAct[j]).CopyToDataTable(), false);
                    }
                  }
                  else
                  {
                    //Dibujamos el encabezado.
                    wsData.Cells[rowNumber, 1].Value = groupsAct[j];
                    using (var range = wsData.Cells[rowNumber, 1, rowNumber, totalColumns])
                    {
                      range.Merge = true;
                      range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                      range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                      range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                      range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                      range.Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                    }
                  }
                  rowNumber++;
                }
              }
              previousGroup = groupsAct;
            }
            //Obtenemos los datos.
            var dataValues = ((IEnumerable<DataRow>)dynamicListData[i].Values).CopyToDataTable();
            var camposGrupo = formatTable.Where(col => col.IsGroup).Select(col => col.PropertyName).ToList();
            //Eliminamos las columnas que fueron configuradas como grupo. Y obtenemos el formato de las columnas que se visualizaran en el reporte.
            dataValues.Columns.OfType<DataColumn>().ToList().ForEach(c =>
            {
              if (camposGrupo.Contains(c.ColumnName))
                dataValues.Columns.Remove(c);
              else
              {
                var format = formatTable.FirstOrDefault(f => f.PropertyName == c.ColumnName);
                if (format != null && format.Order > 0)
                {
                  using (var range = wsData.Cells[rowNumber, format.Order, rowNumber + dataValues.Rows.Count, format.Order])
                  {
                    range.Style.Numberformat.Format = GetFormat(format.Format);
                  }
                }
              }
            });

            //Agregamos los datos al excel.
            using (var range = wsData.Cells[rowNumber, 1].LoadFromDataTable(dataValues, false))
            {
              //Aplicamos estilo a las celdas.
              range.Style.Border.Top.Style = range.Style.Border.Right.Style = range.Style.Border.Left.Style = range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
              range.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 2].BackGroundColor));
              range.Style.Font.Size = 9;
            }

            rowNumber += dataValues.Rows.Count;

            //Si se mostrará el subtotal de cada grupo.
            if (blnShowSubtotal)
            {
              //Obtenemos la fila inicial.
              var dataIniRow = rowNumber - dataValues.Rows.Count;
              //Si el indice siguiente es menor que la cantidad total de items.
              if (i + 1 < dynamicListData.Count)
              {
                //Obtenemos objeto con los encabezados de grupo.
                object nextItem = dynamicListData[i + 1].qgroup;
                //Obtenemos el arreglo de encabezados de grupo. Los niveles estan de acuerdo al indice del arreglo.
                nextGroup = nextItem.GetType().GetProperties().Select(c => c.GetValue(nextItem).ToString()).ToArray();
                //Si el primer nivel de cada arreglo son diferentes.
                if (groupsAct[0] != nextGroup[0])
                  nextGroup = new string[groupsAct.Length];//Limpiamos la lista
              }
              //Recorremos los niveles del arreglo actual.
              for (var j = groupsAct.Length - 1; j >= 0; j--)
              {
                //Si los valores del index actual de cada lista son diferentes o el valor del index de la siguiente lista esta vacia o nula.
                if (groupsAct[j] != nextGroup[j] || string.IsNullOrEmpty(nextGroup[j]) || j == groupsAct.Length - 1)
                {
                  //Recorremos las columnas.
                  foreach (var format in formatTableColumns)
                  {
                    using (var range = wsData.Cells[rowNumber, format.Order])
                    {
                      if (format.SubTotalFunctions == eSubTotalFunctions.None && format.Formula == null) continue;

                      var subtotalFormat = format.Format;
                      if (format.SubtotalWithCero)
                      {
                        switch (format.Format)
                        {
                          case EnumFormatTypeExcel.Number:
                            subtotalFormat = EnumFormatTypeExcel.NumberWithCero;
                            break;
                          case EnumFormatTypeExcel.DecimalNumber:
                            subtotalFormat = EnumFormatTypeExcel.DecimalNumberWithCero;
                            break;
                          case EnumFormatTypeExcel.Percent:
                            subtotalFormat = EnumFormatTypeExcel.PercentWithCero;
                            break;
                        }
                      }

                      //Le aplicacamos el formato a la celda.
                      range.Style.Numberformat.Format = GetFormat(subtotalFormat);

                      //Si no es calculada aplicamos la funcion configurada.
                      if (!format.IsCalculated)
                      {
                        var formula = "";
                        //Si es el ultimo nivel
                        if (j == groupsAct.Length - 1)
                          formula = wsData.Cells[dataIniRow, format.Order, rowNumber - 1, format.Order].Address;//Aplicamos la seleccion segun la cantidad de registros que tenga el grupo.
                        else
                        {
                          //Si son antes del ultimo nivel.
                          var index = j + 1;//Se obtiene el indice actual mas 1
                          while (formula == "")
                          {
                            //Si el indice devuelve un valor nulo, se incrementa.
                            if (subtotalFormulas[index] == null) { index++; continue; }
                            //Obtenemos las posiciones de cada subtotal.
                            formula = subtotalFormulas[index][format.PropertyName];

                            //Si no es el ultimo nivel y el nivel actual de cada arreglo son diferentes o el nivel actual del arreglo siguiente  es nulo.
                            if (j < groupsAct.Length - 1 && (groupsAct[j] != nextGroup[j] || string.IsNullOrEmpty(nextGroup[j])))
                            {
                              //Limpiamos la formula.
                              subtotalFormulas[index][format.PropertyName] = string.Empty;
                            }
                          }
                        }
                        switch (format.SubTotalFunctions)
                        {
                          case eSubTotalFunctions.Sum:
                            range.Formula = "=SUM(" + formula + ")";
                            break;
                          case eSubTotalFunctions.Avg:
                            range.Formula = "=AVERAGE(" + formula + ")";
                            break;
                          case eSubTotalFunctions.Count:
                            if (format.Format == EnumFormatTypeExcel.General || format.Format == EnumFormatTypeExcel.Boolean)
                              range.Formula = (j == groupsAct.Length - 1) ? "= COUNTA(" + formula + ")" : "= SUM(" + formula + ")";
                            break;
                        }
                        if (subtotalFormulas[j] != null && subtotalFormulas[j].ContainsKey(format.PropertyName))
                        {
                          subtotalFormulas[j][format.PropertyName] += (subtotalFormulas[j][format.PropertyName] == string.Empty) ? range.Address : "," + range.Address;
                        }
                        else
                        {
                          if (subtotalFormulas[j] == null) subtotalFormulas[j] = new Dictionary<string, string>();
                          subtotalFormulas[j].Add(format.PropertyName, range.Address);
                        }
                      }
                      else
                        //Obtenemos la formula.
                        range.Formula = GetFormula(formatTable, format.Formula, rowNumber);
                    }
                  }
                  var firstSubtotalColumn = formatTable.First(c => c.SubTotalFunctions != eSubTotalFunctions.None || c.Formula != null).Order;
                  using (var range = wsData.Cells[rowNumber, firstSubtotalColumn, rowNumber, totalColumns])
                  {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                    range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                    range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                  }

                  if (j == groupsAct.Length - 1)
                  {
                    rowNumber++;

                    var TotalShow = "COUNTA(" + wsData.Cells[dataIniRow, formatTable.FirstOrDefault(f => f.PropertyName == "Show").Order, rowNumber - 2, formatTable.FirstOrDefault(f => f.PropertyName == "Show").Order].Address + ")";
                    var Totaltour = $"SUM({wsData.Cells[rowNumber - 1, formatTable.FirstOrDefault(f => f.PropertyName == "Tour").Order].Address},{wsData.Cells[rowNumber - 1, formatTable.FirstOrDefault(f => f.PropertyName == "IO").Order].Address},{wsData.Cells[rowNumber - 1, formatTable.FirstOrDefault(f => f.PropertyName == "WO").Order].Address},{wsData.Cells[rowNumber - 1, formatTable.FirstOrDefault(f => f.PropertyName == "CTour").Order].Address},{wsData.Cells[rowNumber - 1, formatTable.FirstOrDefault(f => f.PropertyName == "STour").Order].Address})";
                    var TotalBookings = wsData.Cells[dataIniRow - 1, dtTableBookings.Columns.Count].Address;
                    var resch = wsData.Cells[rowNumber - 1, formatTable.FirstOrDefault(f => f.PropertyName == "Resch").Order].Address;
                    var direct = wsData.Cells[rowNumber - 1, formatTable.FirstOrDefault(f => f.PropertyName == "Direct").Order].Address;
                    var inOut = wsData.Cells[rowNumber - 1, formatTable.FirstOrDefault(f => f.PropertyName == "IO").Order].Address;

                    for (int k = 1; k <= 15; k++)
                    {
                      switch (k)
                      {
                        case 1:
                          wsData.Cells[rowNumber, k].Formula = $"= {Totaltour}";
                          break;
                        case 2:
                          wsData.Cells[rowNumber, k].Value = "Tour %";
                          wsData.Cells[rowNumber, k + 1].Formula = $"= IF({TotalBookings}=0,0,{Totaltour}/{TotalBookings})";
                          wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = GetFormat(EnumFormatTypeExcel.Percent);
                          break;
                        case 4:
                          wsData.Cells[rowNumber, k].Value = "Shows";
                          wsData.Cells[rowNumber, k + 1].Formula = $"= {TotalShow}";
                          wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = GetFormat(EnumFormatTypeExcel.Number);
                          break;
                        case 6:
                          wsData.Cells[rowNumber, k].Value = "Shows %";
                          wsData.Cells[rowNumber, k + 1].Formula = $"= IF({TotalBookings}=0,0,({TotalShow})/{TotalBookings})";
                          wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = GetFormat(EnumFormatTypeExcel.Percent);
                          break;
                        case 8:
                          wsData.Cells[rowNumber, k].Value = "Sin R/D";
                          wsData.Cells[rowNumber, k + 1].Formula = $"= IF({TotalBookings}=0,0,({TotalShow}-{resch}-{direct})/{TotalBookings})";
                          wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = GetFormat(EnumFormatTypeExcel.Percent);
                          break;
                        case 10:
                          wsData.Cells[rowNumber, k].Value = "Sin Dtas";
                          wsData.Cells[rowNumber, k + 1].Formula = $"= IF({TotalBookings}=0,0,({TotalShow}-{direct})/{TotalBookings})";
                          wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = GetFormat(EnumFormatTypeExcel.Percent);
                          break;
                        case 12:
                          wsData.Cells[rowNumber, k].Value = "Sin Rsch";
                          wsData.Cells[rowNumber, k + 1].Formula = $"= IF({TotalBookings}=0,0,({TotalShow}-{resch})/{TotalBookings})";
                          wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = GetFormat(EnumFormatTypeExcel.Percent);
                          break;
                        case 14:
                          wsData.Cells[rowNumber, k].Value = "Sin I&O";
                          wsData.Cells[rowNumber, k + 1].Formula = $"= IF({TotalBookings}=0,0,({TotalShow}-{inOut})/{TotalBookings})";
                          wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = GetFormat(EnumFormatTypeExcel.Percent);
                          break;

                      }
                    }
                  }
                  rowNumber += 2;
                }
              }
            }
            rowNumber++;
          }

          if (blnRowGrandTotal)
          {
            formatTableColumns.ForEach(format =>
            {
              using (var range = wsData.Cells[rowNumber - 1, format.Order])
              {
                if (format.SubTotalFunctions == eSubTotalFunctions.None && format.Formula == null) return;

                if (!format.IsCalculated)
                {
                  switch (format.SubTotalFunctions)
                  {
                    case eSubTotalFunctions.Sum:
                      range.Formula = "=SUM(" + subtotalFormulas[0][format.PropertyName] + ")";
                      break;
                    case eSubTotalFunctions.Avg:
                      range.Formula = "=AVERAGE(" + subtotalFormulas[0][format.PropertyName] + ")";
                      break;
                    case eSubTotalFunctions.Count:
                      if (format.Format == EnumFormatTypeExcel.General)
                        range.Formula = "= SUM(" + subtotalFormulas[0][format.PropertyName] + ")";
                      break;
                  }
                }
                else
                  range.Formula = GetFormula(formatTableColumns, format.Formula, rowNumber - 1);

                range.Style.Numberformat.Format = GetFormat(format.Format);
              }
            });
            using (var range = wsData.Cells[rowNumber - 1, 1, rowNumber - 1, totalColumns])
            {
              range.Style.Fill.PatternType = ExcelFillStyle.Solid;
              range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].BackGroundColor));
              range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].FontColor));
              range.Style.Font.Bold = backgroundColorGroups[backgroundColorGroups.Count - 1].FontBold;
            }
          }
          //Ajustamos todas las columnas a su contenido.
          //wsData.Cells[totalFilterRows + 5, 1, rowNumber, totalColumns].AutoFitColumns();

          #endregion Simple con Agrupado
        }
        else
        {
          #region Simple

          rowNumber++;
          //Recorremos los datarows del datatable.
          dtTable.AsEnumerable().ToList().ForEach(dr =>
          {
            var drColumn = 1;
          //Recorremos las columnas.
          formatTableColumns.ForEach(row =>
            {
            //Asignamos el valor y formato a la celda. 
            wsData.Cells[rowNumber, drColumn].Value = dr[row.PropertyName];
              wsData.Cells[rowNumber, drColumn].Style.Numberformat.Format = GetFormat(row.Format);
              drColumn++;
            });
            rowNumber++;
          });

          if (blnRowGrandTotal)
          {
            //Obtenemos la fila inicial.
            var dataIniRow = rowNumber - dtTable.Rows.Count;
            //Recorremos las columnas.
            formatTableColumns.ForEach(format =>
            {
              var subtotalFormat = format.Format;
              if (format.SubtotalWithCero)
              {
                switch (format.Format)
                {
                  case EnumFormatTypeExcel.Number:
                    subtotalFormat = EnumFormatTypeExcel.NumberWithCero;
                    break;

                  case EnumFormatTypeExcel.DecimalNumber:
                    subtotalFormat = EnumFormatTypeExcel.DecimalNumberWithCero;
                    break;

                  case EnumFormatTypeExcel.Percent:
                    subtotalFormat = EnumFormatTypeExcel.PercentWithCero;
                    break;
                }
              }

            //Le aplicacamos el formato a la celda.
            wsData.Cells[rowNumber, format.Order].Style.Numberformat.Format = GetFormat(subtotalFormat);
            //S no es una columna calculada.
            if (!format.IsCalculated)
              {
              //Aplicamos la funcion configurada.
              switch (format.Function)
                {
                  case DataFieldFunctions.Sum:
                    wsData.Cells[rowNumber, format.Order].Formula = "=SUM(" + wsData.Cells[dataIniRow, format.Order, rowNumber - 1, format.Order].Address + ")";
                    break;

                  case DataFieldFunctions.Average:
                    wsData.Cells[rowNumber, format.Order].Formula = "=AVERAGE(" + wsData.Cells[dataIniRow, format.Order, rowNumber - 1, format.Order].Address + ")";
                    break;

                  case DataFieldFunctions.Count:
                    if (format.Format == EnumFormatTypeExcel.General)
                      wsData.Cells[rowNumber, format.Order].Formula = "=COUNTA(" + wsData.Cells[dataIniRow, format.Order, rowNumber - 1, format.Order].Address + ")";
                    break;
                }
              }
              else
              //Aplicamos la formula configurada.
              wsData.Cells[rowNumber, format.Order].Formula = GetFormula(formatTable, format.Formula, rowNumber);
            });
          }

          #endregion Simple
        }
        //Ajustamos las celdas a su contenido.
        //wsData.Cells[totalFilterRows + 5, 1, rowNumber, dtTable.Columns.Count].AutoFitColumns();
        AutoFitColumns(ref wsData, dtTable.Columns.Count, rowNumber);

        if (fileFullPath == null)
        {
          var suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);
          pathFinalFile = SaveExcel(pk, suggestedFilaName);
        }
        else
          pathFinalFile = SaveExcelFilePath(pk, fileFullPath);
      }
      return pathFinalFile;
    }

    #endregion CreateExcelCustomPivot

    #region UpdateTableExcel

    /// <summary>
    /// Actualiza una tabla de un Stream de un archivo de Excel
    /// </summary>
    /// <param name="template">Stream del archivo excel</param>
    /// <param name="dt">DataTable con la tabla a actualizar (El nombre del DataTable debe ser el mismo que el nombre de la tabla de excel)</param>
    /// <returns>Stream</returns>
    /// <history>
    /// [aalcocer]  03/05/2016 Created.
    /// </history>
    public static Stream UpdateTableExcel(Stream template, DataTable dt)
    {
      using (var pk = new ExcelPackage(template))
      {
        //Preparamos la hoja donde escribiremos la tabla
        var ws = pk.Workbook.Worksheets.ToList().First(w => w.Tables.Any(t => t.Name == dt.TableName));

        var table = ws.Tables[dt.TableName];
        var start = table.Address.Start;
        var body = ws.Cells[start.Row + 1, start.Column];
        var outRange = body.LoadFromDataTable(dt, false);

        string newRange = $"{start.Address}:{outRange.End.Address}";

        var tableElement = table.TableXml.DocumentElement;
        if (tableElement != null)
        {
          tableElement.Attributes["ref"].Value = newRange;
          var xmlElement = tableElement["autoFilter"];
          if (xmlElement != null) xmlElement.Attributes["ref"].Value = newRange;
        }
        pk.Save();
        template = pk.Stream;
      }

      return template;
    }

    #endregion UpdateTableExcel

    #region CreateExcelFromTemplate

    /// <summary>
    /// Crea un reporte en Excel a partir de un Stream de Excel. Se agrega Filtros, Nombre de reporte, contenido, datos de impresion y nombre del sistema al archivo
    /// </summary>
    /// <param name="filter">Filtros aplicados al reporte.</param>
    /// <param name="template">Stream del archivo excel</param>
    /// <param name="reportName">Nombre del Reporte.</param>
    /// <param name="dateRangeFileName">Nombre del archivo del reporte</param>
    /// <param name="fileFullPath">Opcional. Ruta completa del archivo</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer]  03/05/2016 Created.
    /// [aalcocer]  06/06/2016 Modified. Se agrega la opcion de generar el reporte en de ruta completa del archivo
    /// </history>
    public static FileInfo CreateExcelFromTemplate(List<Tuple<string, string>> filter, Stream template, string reportName, string dateRangeFileName, string fileFullPath = null)
    {
      #region Variables Atributos, Propiedades

      var pk = new ExcelPackage(template);
      //Preparamos la hoja donde escribiremos
      var ws = pk.Workbook.Worksheets.First(w => w.Hidden == eWorkSheetHidden.Visible);
      ws.Name = Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " ");

      //Filas Para los filtros
      var filasTotalesFiltros = 0;

      #endregion Variables Atributos, Propiedades

      #region Report SuperHeader

      //Creamos la cabecera del reporte (Titulos, Filtros, Fecha y Hora de Impresion)
      CreateReportHeader(filter, reportName, ref ws, ref filasTotalesFiltros, null, 0);

      #endregion Report SuperHeader

      #region Formato de columnas Centrar y AutoAjustar

      //Auto Ajuste de columnas de  acuerdo a su contenido
      ws.Cells[ws.Dimension.Address].AutoFitColumns();
      //Centramos el titulo de la aplicacion
      ws.Cells[1, 1, 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
      //Centramos el titulo del reporte
      ws.Cells[1, 4, 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

      //Centramos La etiqueta Filters
      if (filter.Count > 0)
      {
        ws.Cells[2, 1, filasTotalesFiltros + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        ws.Cells[2, 1, filasTotalesFiltros + 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
      }

      #endregion Formato de columnas Centrar y AutoAjustar

      #region Generamos y Retornamos la ruta del archivo EXCEL

      FileInfo pathFinalFile;
      if (fileFullPath == null)
      {
        var suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);
        pathFinalFile = SaveExcel(pk, suggestedFilaName);
      }
      else
        pathFinalFile = SaveExcelFilePath(pk, fileFullPath);

      return pathFinalFile;

      #endregion Generamos y Retornamos la ruta del archivo EXCEL
    }

    #endregion CreateExcelFromTemplate

    #region CreateGraphExcel

    /// <summary>
    ///Crea un reporte en excel que tiene  tabla de datos de semana y mes con su grafica.
    /// De los reportes Not Booking Arrivals (Graphic) y  Unavailable Arrivals (Graphic)
    /// </summary>
    /// <param name="filter">Tupla de filtros: item1 = Nombre del filtro - item2 =filtros separados por comas (bp,mbp)</param>
    /// <param name="graphTotals">Tiene los totales del la semana y el mex</param>
    /// <param name="reportName">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Nombre del reporte con fecha</param>
    /// <param name="tupleGraph1">tuple la semana: item1:Rango de fecha de la semana - item2:DataTable con la informacion de la semana - item3:Lista de ExcelFormatTable donde definimos la estructura</param>
    /// <param name="tupleGraph2">tuple la semana: item1:Rango de fecha del mes- item2:DataTable con la informacion del mes - item3:Lista de ExcelFormatTable donde definimos la estructura</param>
    /// <param name="fileFullPath">Opcional. Ruta completa del archivo</param>
    /// <returns>FileInfo con el path para abrir el excel</returns>
    /// <history>
    ///   [aalcocer] 27/04/2016 Created.
    ///   [aalcocer]     06/06/2016 Modified. Se agrega la opcion de generar el reporte en de ruta completa del archivo
    /// </history>
    public static FileInfo CreateGraphExcel(List<Tuple<string, string>> filter, GraphTotals graphTotals, string reportName, string dateRangeFileName, Tuple<string, DataTable, List<ExcelFormatTable>> tupleGraph1, Tuple<string, DataTable, List<ExcelFormatTable>> tupleGraph2, string fileFullPath = null)
    {
      #region Variables Atributos, Propiedades

      var pk = new ExcelPackage();
      //Preparamos la hoja donde escribiremos
      var ws = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));

      //Filas Para los filtros
      var filasTotalesFiltros = 0;
      var filasTotales = 0;

      //Renombramos las columnas.
      tupleGraph1.Item2.Columns.Cast<DataColumn>().ToList().ForEach(c =>
      {
        c.ColumnName = tupleGraph1.Item3[tupleGraph1.Item2.Columns.IndexOf(c)].Title;
      });

      tupleGraph2.Item2.Columns.Cast<DataColumn>().ToList().ForEach(c =>
      {
        c.ColumnName = tupleGraph2.Item3[tupleGraph2.Item2.Columns.IndexOf(c)].Title;
      });

      #endregion Variables Atributos, Propiedades

      #region Report SuperHeader

      //Creamos la cabecera del reporte (Titulos, Filtros, Fecha y Hora de Impresion)
      CreateReportHeader(filter, reportName, ref ws, ref filasTotalesFiltros, null, 0);

      #endregion Report SuperHeader

      #region Contenido del reporte

      filasTotales += filasTotalesFiltros + 6;

      #region tabla 1

      //Agregamos el contenido empezando en la fila
      var range = ws.Cells[filasTotales, 1].LoadFromDataTable(tupleGraph1.Item2, true);
      //El contenido lo convertimos a una tabla
      var table = ws.Tables.Add(range, null);
      table.TableStyle = TableStyles.Medium2;

      //Formateamos la tabla
      SetFormatTable(tupleGraph1.Item3, ref table);
      //Agregamos el SuperHeader del la tabla
      range = ws.Cells[table.Address.Start.Row - 1, table.Address.Start.Column, table.Address.Start.Row - 1, table.Address.End.Column];
      range.Merge = true;
      range.Style.Fill.PatternType = ExcelFillStyle.Solid;
      range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#004E48"));
      range.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
      range.Style.Font.Size = 14;
      range.Style.Font.Bold = true;
      range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
      range.Value = tupleGraph1.Item1;

      //Agregamos el grafico
      var chart = ws.Drawings.AddChart("chart" + table.Name, eChartType.BarClustered) as ExcelBarChart;
      if (chart != null)
      {
        chart.Title.Text = "Total: " + graphTotals.TotalW;
        chart.SetPosition(table.Address.Start.Row - 2, 0, table.Address.End.Column + 1, 0);
        chart.SetSize(640, 40 + 20 * table.Address.Rows);
        chart.DataLabel.ShowValue = true;
        chart.DataLabel.ShowPercent = true;
        chart.Style = eChartStyle.Style26;
        chart.Legend.Remove();
        chart.Series.Add($"{table.Name}[{tupleGraph1.Item3.First(c => c.Axis == ePivotFieldAxis.Values).Title}]", $"{table.Name}[{tupleGraph1.Item3.First(c => c.Axis == ePivotFieldAxis.Column).Title}]");

        chart.Axis[0].Orientation = eAxisOrientation.MaxMin;
        chart.Axis[1].Deleted = true;
      }

      #endregion tabla 1

      filasTotales += table.Address.Rows + 6;

      #region tabla 2

      //Agregamos el contenido empezando en la fila
      range = ws.Cells[filasTotales, 1].LoadFromDataTable(tupleGraph2.Item2, true);
      //El contenido lo convertimos a una tabla
      table = ws.Tables.Add(range, null);
      table.TableStyle = TableStyles.Medium2;

      //Formateamos la tabla
      SetFormatTable(tupleGraph2.Item3, ref table);
      //Agregamos el SuperHeader del la tabla
      range = ws.Cells[table.Address.Start.Row - 1, table.Address.Start.Column, table.Address.Start.Row - 1, table.Address.End.Column];
      range.Merge = true;
      range.Style.Fill.PatternType = ExcelFillStyle.Solid;
      range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#004E48"));
      range.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
      range.Style.Font.Size = 14;
      range.Style.Font.Bold = true;
      range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
      range.Value = tupleGraph2.Item1;

      chart = ws.Drawings.AddChart("chart" + table.Name, eChartType.BarClustered) as ExcelBarChart;
      if (chart != null)
      {
        chart.Title.Text = "Total: " + graphTotals.TotalM;
        chart.SetPosition(table.Address.Start.Row - 2, 0, table.Address.End.Column + 1, 0);
        chart.SetSize(640, 40 + 20 * table.Address.Rows);
        chart.DataLabel.ShowValue = true;
        chart.DataLabel.ShowPercent = true;
        chart.Style = eChartStyle.Style26;
        chart.Legend.Remove();
        chart.Series.Add($"{table.Name}[{tupleGraph2.Item3.First(c => c.Axis == ePivotFieldAxis.Values).Title}]", $"{table.Name}[{tupleGraph2.Item3.First(c => c.Axis == ePivotFieldAxis.Column).Title}]");

        chart.Axis[0].Orientation = eAxisOrientation.MaxMin;
        chart.Axis[1].Deleted = true;
      }

      #endregion tabla 2

      #endregion Contenido del reporte

      #region Formato de columnas Centrar y AutoAjustar

      //Auto Ajuste de columnas de  acuerdo a su contenido
      ws.Cells[ws.Dimension.Address].AutoFitColumns();
      //Centramos el titulo de la aplicacion
      ws.Cells[1, 1, 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
      //Centramos el titulo del reporte
      ws.Cells[1, 4, 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

      //Centramos La etiqueta Filters
      if (filter.Count > 0)
      {
        ws.Cells[2, 1, filasTotalesFiltros + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        ws.Cells[2, 1, filasTotalesFiltros + 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
      }

      #endregion Formato de columnas Centrar y AutoAjustar

      #region Generamos y Retornamos la ruta del archivo EXCEL

      FileInfo pathFinalFile;
      if (fileFullPath == null)
      {
        var suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);
        pathFinalFile = SaveExcel(pk, suggestedFilaName);
      }
      else
        pathFinalFile = SaveExcelFilePath(pk, fileFullPath);

      return pathFinalFile;

      #endregion Generamos y Retornamos la ruta del archivo EXCEL
    }

    #endregion CreateGraphExcel

    #region CreateEmptyExcel

    /// <summary>
    /// Crea un Excel vacio para porterior reemplazar por un reporte
    /// </summary>
    /// <param name="reportName">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Fecha del reporte</param>
    /// <returns>Ruta completa del archivo</returns>
    /// <history>
    ///   [aalcocer] 03/06/2016 Created.
    ///   [aalcocer] 13/06/2016 Modified. La ruta por default se obtiene en la configuracion
    /// </history>
    public static string CreateEmptyExcel(string reportName, string dateRangeFileName)
    {
      var suggestedName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);

      string outputDir = ConfigRegistryHelper.GetReportsPath();
      int count = 1;

      string fullPath = $@"{outputDir}\{suggestedName}.xlsx";

      var fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
      var extension = Path.GetExtension(fullPath);
      var path = Path.GetDirectoryName(fullPath);
      var newFullPath = fullPath;

      while (File.Exists(newFullPath))
      {
        string tempFileName = $"{fileNameOnly}({count++})";
        newFullPath = Path.Combine(path, tempFileName + extension);
      }

      var file = File.Create(newFullPath);
      file.Close();
      return file.Name;
    }

    #endregion CreateEmptyExcel

    #region CreateNoInfoRptExcel

    /// <summary>
    ///   Crea un reporte en excel que el contenido informa que no existen informacion  para el reporte, tiene Filtros y Nombre del reporte.
    /// </summary>
    /// <param name="filter">Tupla de filtros </param>
    /// <param name="reportName">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <returns>FileInfo con el path para abrir el excel</returns>
    /// <history>
    /// [aalcocer] 06/06/2016 Created.
    /// </history>
    public static FileInfo CreateNoInfoRptExcel(List<Tuple<string, string>> filter, string reportName, string fileFullPath)
    {
      #region Variables Atributos, Propiedades

      var pk = new ExcelPackage();
      //Preparamos la hoja donde escribiremos
      var ws = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
      //Filas Para los filtros
      var filasTotalesFiltros = 0;

      #endregion Variables Atributos, Propiedades

      #region Report SuperHeader

      //Creamos la cabecera del reporte (Titulos, Filtros, Fecha y Hora de Impresion)
      CreateReportHeader(filter, reportName, ref ws, ref filasTotalesFiltros, null, 0);

      #endregion Report SuperHeader

      using (var range = ws.Cells[filasTotalesFiltros + 1, 2, filasTotalesFiltros + 4, 12])
      {
        range.Value = "There is no info to make a report";
        range.Style.Font.Bold = true;
        range.Style.Font.Size = 36;
        range.Merge = true;
        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(Color.Cyan);
        range.Style.Border.BorderAround(ExcelBorderStyle.Medium);
      }

      var pathFinalFile = SaveExcelFilePath(pk, fileFullPath);

      return pathFinalFile;
    }

    #endregion CreateNoInfoRptExcel

    #endregion Public Methods

    #region Private Methods

    #region SetFormatTable

    /// <summary>
    ///   Se aplica el formato a las columnas de la tabla.
    /// </summary>
    /// <param name="formatColumns">Lista de Formatos de columna del reporte.</param>
    /// <param name="tableData">Tabla de Excel.</param>
    /// <history>
    ///   [edgrodriguez] 24/03/2016  Created.
    /// [erosado] 01/04/2016 Modified Se cambiaron valores estaticos para acomodar el contenido del reporte.
    /// [aalcocer] 16/04/2016 Modified. Se cambio para que solo le de formato a la tabla
    /// </history>
    private static void SetFormatTable(List<ExcelFormatTable> formatColumns, ref ExcelTable tableData)
    {
      var excelWorkbook = tableData.WorkSheet.Workbook;
      //Agregamos estilo a los Headers de la Tabla
      var namedStyle = excelWorkbook.Styles.CreateNamedStyle(tableData.Name + "HeaderRowCellStyle");
      namedStyle.Style.Font.Bold = true;
      namedStyle.Style.Font.Size = 14;
      tableData.HeaderRowCellStyle = namedStyle.Name;

      var contColumn = 0;
      foreach (var item in formatColumns)
      {
        var tableStyle = excelWorkbook.Styles.CreateNamedStyle(tableData.Name + "TableColumnStyle" + contColumn);
        tableStyle.Style.HorizontalAlignment = item.Alignment;
        //tableStyle.Style.Font.Size = 9;
        switch (item.Format)
        {
          case EnumFormatTypeExcel.General:
          case EnumFormatTypeExcel.Boolean:
            break;

          case EnumFormatTypeExcel.Percent:
          case EnumFormatTypeExcel.Currency:
          case EnumFormatTypeExcel.Number:
          case EnumFormatTypeExcel.DecimalNumber:
          case EnumFormatTypeExcel.Date:
          case EnumFormatTypeExcel.Time:
          case EnumFormatTypeExcel.Month:
            tableStyle.Style.Numberformat.Format = GetFormat(item.Format);
            break;
        }
        tableData.Columns[contColumn].DataCellStyleName = tableStyle.Name;

        contColumn++;
      }
    }

    #endregion SetFormatTable

    #region GetFormat

    /// <summary>
    ///   Se obtiene el formato de la columna.
    /// </summary>
    /// <param name="item">Enumerado del formato de excel.</param>
    /// <returns>string</returns>
    /// <history>
    ///   [edgrodriguez] 24/03/2016  Created.
    ///   [ecanul] 07/05/2016 Modified -  Ahora pide el enumerado del formato y  no la celda de excel completa,
    ///                 Aregados valores para PercentWithCero, NumberWithCero, DecimalNumberWithCero
    ///   [ecanul] 09/05/2016 Modified - Agregado caso Id, para enumerar los tipo Id
    /// </history>
    private static string GetFormat(EnumFormatTypeExcel item)
    {
      var format = "";
      switch (item)
      {
        case EnumFormatTypeExcel.General:
          break;

        case EnumFormatTypeExcel.Percent:
          format = "0.0 %;-0.0 %;";
          break;

        case EnumFormatTypeExcel.PercentWithCero:
          format = "0.0 %;-0.0 %;0 %";
          break;

        case EnumFormatTypeExcel.Currency:
          format = "_-$ #,##0.00_-;-$ #,##0.00_-;_-$*  - ??_-;_-$*  - _-";
          break;

        case EnumFormatTypeExcel.Number:
          format = "#,##0;-#,##0;";
          break;

        case EnumFormatTypeExcel.NumberWithCero:
          format = "#,##0;-#,##0;0";
          break;

        case EnumFormatTypeExcel.DecimalNumber:
          format = "#,##0.00;-#,##0.00;";
          break;

        case EnumFormatTypeExcel.DecimalNumberWithCero:
          format = "#,##0.00;-#,##0.00; 0.00";
          break;

        case EnumFormatTypeExcel.Date:
          format = "dd/MM/yyyy;;;";
          break;

        case EnumFormatTypeExcel.Time:
          format = "hh:mm AM/PM";
          break;

        case EnumFormatTypeExcel.Month:
          format = "[$-409]mmmm";
          break;

        case EnumFormatTypeExcel.Id:
          format = "#";
          break;

        case EnumFormatTypeExcel.Day:
          format = "[$-409]dddd";
          break;

        case EnumFormatTypeExcel.DateTime:
          format = "dd/MM/yyyy hh:mm:ss AM/PM;;;";
          break;
      }

      return format;
    }

    #endregion GetFormat

    #region Create ReportHeader

    ///  <summary>
    ///  Crea la cabecera para el reporte
    ///  </summary>
    ///  <param name="filterList">Lista de filtros</param>
    ///  <param name="reportName">Nombre del reporte</param>
    ///  <param name="ws">ExcelWorkdsheet</param>
    ///  <param name="totalFilterRows">totalFilterRows</param>
    /// <param name="extraFieldHeader">"Titulo","Valor",Formato de Celda</param>
    /// <param name="numRows">Numero de Rows por Columna</param>
    /// <history>
    /// [ecanul] 16/05/2016 Modified Agregados parametros extraFieldHeader y numRows para agregar detalles al Header de los reportes
    /// [edgrodriguez] 09/06/2016 Modified Parametros extraFieldHeader y numRows se cambiaron a opcionales.
    /// </history>
    private static void CreateReportHeader(List<Tuple<string, string>> filterList, string reportName,
      ref ExcelWorksheet ws, ref int totalFilterRows, List<Tuple<string, dynamic, EnumFormatTypeExcel>> extraFieldHeader = null, int numRows = 0)
    {
      double filterNumber = filterList.Count;

      #region Titulo del reporte

      //Agregamos el Nombre de la Aplicacion en las columnas combinadas A:C en la fila 1
      ws.Cells[1, 1, 1, 3].Merge = true;
      ws.Cells[1, 1, 1, 3].Value = "Intelligence Marketing";
      ws.Cells[1, 1, 1, 3].Style.Font.Bold = true;
      ws.Cells[1, 1, 1, 3].Style.Font.Size = 14;
      //Agregamos el Nombre del Reporte en las columnas combinadas D:J en la fila 1
      ws.Cells[1, 4, 1, 19].Merge = true;
      ws.Cells[1, 4, 1, 19].Value = reportName;
      ws.Cells[1, 4, 1, 19].Style.Font.Bold = true;
      ws.Cells[1, 4, 1, 19].Style.Font.Size = 14;
      //Agregamos la linea Doble bajo los nombres de la aplicacion y del reporte
      ws.Cells[1, 1, 1, 19].Style.Border.Bottom.Style = ExcelBorderStyle.Double; //Doble Linea

      #endregion Titulo del reporte

      #region Filtros

      //Validamos que el reporte tenga filtros
      if (filterList.Count > 0)
      {
        //Calculamos el numero de filas para ingresar nuestros filtros
        var filterDivision = filterNumber / 4;
        var filasTotales = Math.Ceiling(filterDivision); //Obtenemos el numero de filas totales.
        totalFilterRows = Convert.ToInt16(filasTotales);
        //Insertamos las filas que necesitamos para los filtros apartir de la fila 2
        ws.InsertRow(2, totalFilterRows);

        //Agregamos el titulo Filters en la fila 2 Columna 1
        ws.Cells[2, 1].Value = "Filters:";
        ws.Cells[2, 1, totalFilterRows + 1, 1].Merge = true;
        ws.Cells[2, 1].Style.Font.Bold = true;
        ws.Cells[2, 1].Style.Font.Size = 14;

        //Separamos los filtros en listas de a 4 en 4
        double cIteraccionReal = 0; //Contador de vueltas en el foreach
        var cIteraccionColumn = 2;//Contador de columnas de escritura
        foreach (var item in filterList)
        {
          cIteraccionReal++;

          var rowDivision = cIteraccionReal / 4; //Obtenemos el numero de filtro a escribir
          var filaEscritura = Math.Ceiling(rowDivision); //Redondeamos Para saber en que fila se encuenta

          ws.Cells[Convert.ToInt16(filaEscritura) + 1, cIteraccionColumn].Value = item.Item1; //NameFilter
          ws.Cells[Convert.ToInt16(filaEscritura) + 1, cIteraccionColumn].Style.Font.Bold = true;//NameFilter
          ws.Cells[Convert.ToInt16(filaEscritura) + 1, cIteraccionColumn + 1].Value = item.Item2;//ValueFilter

          if (cIteraccionReal % 4 == 0)//Indica si Cambiamos en la misma fila
          {
            cIteraccionColumn = 2;
          }
          else//Indica si seguimos en la misma fila
          {
            cIteraccionColumn += 2;
          }
        }
      }

      #endregion Filtros

      #region Datos de la impresion

      //Agregamos la etiqueta

      totalFilterRows += 2;

      ws.Cells[totalFilterRows, 1].Value = "Print Date Time";
      ws.Cells[totalFilterRows, 1].Style.Font.Bold = true;

      //Agregamos el valor de fecha y hora de impresion
      ws.Cells[totalFilterRows, 2].Value = string.Format("{0:MM/dd/yyyy hh:mm:ss}", DateTime.Now);

      #endregion Datos de la impresion

      #region ExtraHeaderFile

      //Si el parametro extraFieldHeader tiene algo 
      if (extraFieldHeader != null && extraFieldHeader.Count > 0)
      {
        //Saltamos 2 lineas Para iniciar SubHeader
        totalFilterRows += 2;
        /** La idea es que quede de la siguiente manera 
         * /-/-/ /-/-/
         * /-/-/ /-/-/
         * /-/-/ /-/-/
         **/
        //Inserta desde el ultimo numero utilizado,cantidad de rows solicitada por el usuario
        ws.InsertRow(totalFilterRows, numRows);
        var staRow = totalFilterRows; //Deberia de ser 5
        var col = 1; //Siempre empieza en 1
        double count = 0; //Contador de vueltas del foreach
        foreach (var item in extraFieldHeader)
        {
          var style = ExcelBorderStyle.Thin;
          //wsData.Cells[rowNumber, drColumn].Style.Numberformat.Format = GetFormat(subtotalFormat);

          #region HeaderName

          ws.Cells[staRow, col].Value = item.Item1;
          ws.Cells[staRow, col].Style.Font.Bold = true;
          //estilos
          ws.Cells[staRow, col].Style.Border.Top.Style = style;
          ws.Cells[staRow, col].Style.Border.Left.Style = style;
          ws.Cells[staRow, col].Style.Border.Bottom.Style = style;
          ws.Cells[staRow, col].Style.Border.Right.Style = style;

          #endregion

          #region HeaderValue

          ws.Cells[staRow, col + 1].Value = item.Item2;
          //Estilos
          ws.Cells[staRow, col + 1].Style.Border.Top.Style = style;
          ws.Cells[staRow, col + 1].Style.Border.Left.Style = style;
          ws.Cells[staRow, col + 1].Style.Border.Bottom.Style = style;
          ws.Cells[staRow, col + 1].Style.Border.Right.Style = style;
          ws.Cells[staRow, col + 1].Style.Numberformat.Format = GetFormat(item.Item3);

          #endregion

          count++; //Incrementa el contador
          if (count < numRows)
            staRow++;
          else
          {
            staRow = 5;
            col = col + 3;
            count = 0;
          }
        }
        totalFilterRows += numRows - 1;
      }

      #endregion

      //Se saltan 2 lineas desde donde quedo (Fila 3 si es sin subheader o 3 + numRows)
      totalFilterRows += 2;
    }

    #endregion Create ReportHeader

    #region SaveExcel

    /// <summary>
    /// Guarda un ExcelPackage en una ruta escogida por el usuario
    /// </summary>
    /// <param name="pk">ExcelPackage</param>
    /// <param name="suggestedName">Nombre sugerido</param>
    /// <returns>Ruta de archivo nuevo (FileInfo)</returns>
    /// <history>
    /// [erosado] 11/03/2016  Created
    /// [erosado] 02/04/2016  Cambiamos el metodo a privado
    /// </history>
    private static FileInfo SaveExcel(ExcelPackage pk, string suggestedName)
    {
      var saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Excel files(*.xlsx or *.xls) | *.xlsx; *.xls;";
      saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      saveFileDialog.DefaultExt = ".xlsx";
      saveFileDialog.FileName = suggestedName;
      if (saveFileDialog.ShowDialog() == true)
      {
        var name = new FileInfo(saveFileDialog.FileName);
        try
        { pk.SaveAs(name); }
        catch (Exception e)
        {
          var message = UIHelper.GetMessageError(e);
          UIHelper.ShowMessage(message, System.Windows.MessageBoxImage.Error);
          return null;
        }
        return name;
      }
      else
      {
        return null;
      }
    }

    #endregion SaveExcel

    #region SaveExcelFilePath

    /// <summary>
    /// Guarda un ExcelPackage en una ruta especificada
    /// </summary>
    /// <param name="pk">ExcelPackage</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <returns>Ruta de archivo nuevo (FileInfo)</returns>
    /// <history>
    /// [aalcocer] 03/06/2016  Created
    /// </history>
    private static FileInfo SaveExcelFilePath(ExcelPackage pk, string fileFullPath)
    {
      var newFile = new FileInfo(fileFullPath);
      try
      {
        if (newFile.Exists)
        {
          newFile.Delete();
        }
        pk.SaveAs(newFile);
      }
      catch (Exception e)
      {
        var message = UIHelper.GetMessageError(e);
        UIHelper.ShowMessage(message, System.Windows.MessageBoxImage.Error);
        return null;
      }
      pk.Dispose();
      return newFile;
    }

    #endregion SaveExcelFilePath

    #region AddCalculatedField

    ///  <summary>
    ///  Agrega un campo calculado en una tabla dimanica
    ///  </summary>
    ///  <param name="pivotTable">La tabla dinamica</param>
    /// <param name="formatTable">Formato de la columna</param>
    /// <param name="caption"> Opcional. Nombre del atributo  a mostrar diferente a su nombre original.</param>
    /// <history>
    ///   [aalcocer] 01/04/2016  Created
    /// </history>
    private static void AddCalculatedField(this ExcelPivotTable pivotTable, ExcelFormatTable formatTable, string caption = null)
    {
      //En primer lugar, se agrega el elemento cacheFields campo calculado como un hijo del elemento Campos de caché en el PivotCache Definition1.xml
      var cacheFieldsElement = pivotTable.CacheDefinition.CacheDefinitionXml.GetElementsByTagName("cacheFields")[0] as XmlElement;

      // Añadir el elemento cacheField y tomar nota de que el índice
      var cacheFieldsCountAttribute = cacheFieldsElement.Attributes["count"];
      var count = Convert.ToInt32(cacheFieldsCountAttribute.Value);
      cacheFieldsElement.InnerXml += $"<cacheField name=\"{formatTable.Title}\" numFmtId=\"0\" formula=\"{formatTable.Formula}\" databaseField=\"0\"/>\n";
      var cacheFieldIndex = ++count;
      // Campos de actualización de memoria caché cuentan atributo
      cacheFieldsCountAttribute.Value = count.ToString();

      // A continuación, actualizar e insertar pivotTable1.xml elemento PivotField como un hijo del elemento PivotFields
      var pivotFieldsElement = pivotTable.PivotTableXml.GetElementsByTagName("pivotFields")[0] as XmlElement;
      var pivotFieldsCountAttribute = pivotFieldsElement.Attributes["count"];
      pivotFieldsElement.InnerXml += "<pivotField dataField=\"1\" compact=\"0\" outline=\"0\" subtotalTop=\"0\" dragToRow=\"0\" dragToCol=\"0\" dragToPage=\"0\" showAll=\"0\" includeNewItemsInFilter=\"1\" defaultSubtotal=\"0\"/> \n";
      //actualizar el atributo cantidad de  pivotFields
      pivotFieldsCountAttribute.Value = (int.Parse(pivotFieldsCountAttribute.Value) + 1).ToString();

      // También en pivotTable1.xml, inserte el <dataField> en la posición correcta, el FLD aquí apunta al caché de índice de Campo
      var dataFields = pivotTable.PivotTableXml.GetElementsByTagName("dataFields")[0] as XmlElement;

      // Crear el elemento dataField con los atributos
      var dataField = pivotTable.PivotTableXml.CreateElement("dataField", pivotTable.PivotTableXml.DocumentElement.NamespaceURI);
      dataField.RemoveAllAttributes();
      var nameAttrib = pivotTable.PivotTableXml.CreateAttribute("name");

      // Caché campo no puede tener el mismo nombre que el atributo dataField
      if (caption == null || caption == formatTable.Title)
        nameAttrib.Value = " " + formatTable.Title;
      else
        nameAttrib.Value = caption;
      dataField.Attributes.Append(nameAttrib);

      var fldAttrib = pivotTable.PivotTableXml.CreateAttribute("fld");

      fldAttrib.Value = (cacheFieldIndex - 1).ToString();
      dataField.Attributes.Append(fldAttrib);
      var baseFieldAttrib = pivotTable.PivotTableXml.CreateAttribute("baseField");
      baseFieldAttrib.Value = "0";
      dataField.Attributes.Append(baseFieldAttrib);
      var baseItemAttrib = pivotTable.PivotTableXml.CreateAttribute("baseItem");
      baseItemAttrib.Value = "0";
      dataField.Attributes.Append(baseItemAttrib);

      var styles = pivotTable.WorkSheet.Workbook.Styles;
      var nFormatXml = styles.NumberFormats.ToList().Find(x => x.Format == GetFormat(formatTable.Format));

      //Si no existe el Formato se crea uno
      if (nFormatXml == null)
      {
        var dataFieldAux = pivotTable.DataFields.First();
        var formatAux = dataFieldAux.Format;
        dataFieldAux.Format = GetFormat(formatTable.Format);
        dataFieldAux.Format = formatAux;
        nFormatXml = styles.NumberFormats.ToList().Find(x => x.Format == GetFormat(formatTable.Format));
      }

      var numFmtIdAttrib = pivotTable.PivotTableXml.CreateAttribute("numFmtId");
      numFmtIdAttrib.Value = nFormatXml.NumFmtId.ToString();
      dataField.Attributes.Append(numFmtIdAttrib);

      // Insertar elemento dataField a la posición correcta.
      if (formatTable.Order <= 0)
      {
        dataFields.PrependChild(dataField);
      }
      else if (formatTable.Order >= dataFields.ChildNodes.Count)
      {
        dataFields.AppendChild(dataField);
      }
      else
      {
        var insertBeforeThis = dataFields.ChildNodes.Item(formatTable.Order);
        if (insertBeforeThis != null)
          dataFields.InsertBefore(dataField, insertBeforeThis);
        else
          dataFields.AppendChild(dataField);
      }
    }

    #endregion AddCalculatedField

    #region AddPivotBorderRange

    /// <summary>
    /// Agrega una macro al excel para dibujar bordes a una tabla dinamica
    /// </summary>
    /// <param name="pivotTable">la tabla dinamica</param>
    /// <param name="columnasAgrupadas">las columnas las cuales se les agregará el border</param>
    /// <history>
    ///   [aalcocer] 11/04/2016  Created
    /// </history>
    private static void AddPivotBorderRange(this ExcelPivotTable pivotTable, List<ExcelFormatTable> columnasAgrupadas)
    {
      //Crea un vba project
      if (pivotTable.WorkSheet.Workbook.VbaProject == null)
      {
        pivotTable.WorkSheet.Workbook.CreateVBAProject();
        pivotTable.WorkSheet.Workbook.CodeModule.Code = "Private Sub Workbook_Open()\r\nEnd Sub";
        pivotTable.WorkSheet.Workbook.VbaProject.Modules.AddModule("EPPlusGeneratedCode");
      }

      var module = pivotTable.WorkSheet.Workbook.VbaProject.Modules.Single(m => m.Type == eModuleType.Module);
      var lines = pivotTable.WorkSheet.Workbook.CodeModule.Code.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

      var nombreSub = "BorderPivotTable" + (lines.Count - 1);
      lines.Insert(lines.Count - 1, nombreSub);
      pivotTable.WorkSheet.Workbook.CodeModule.Code = string.Join(Environment.NewLine, lines);

      var sb = new StringBuilder();
      sb.AppendLine("Public Sub " + nombreSub + "()");
      sb.AppendLine("Dim t_Range As Range");
      sb.AppendLine("Dim PvtTbl As PivotTable");
      sb.AppendLine("On Error GoTo ControlError");
      sb.AppendLine("Set PvtTbl = Worksheets(\"" + pivotTable.WorkSheet.Name + "\").PivotTables(\"" + pivotTable.Name + "\")");
      sb.AppendLine("Worksheets(\"" + pivotTable.WorkSheet.Name + "\").Activate");
      sb.AppendLine("PvtTbl.PivotCache.Refresh");

      sb.Append("Set t_Range = Union(");
      var pivotFieldList = new List<string>();
      columnasAgrupadas.ForEach(c => pivotFieldList.Add("PvtTbl.PivotFields(\"" + c.Title + "\").DataRange, PvtTbl.PivotFields(\"" + c.Title + "\").LabelRange"));
      sb.Append(string.Join(",", pivotFieldList));
      sb.AppendLine(")");

      sb.AppendLine("t_Range.Borders(xlDiagonalDown).LineStyle = xlNone");
      sb.AppendLine("t_Range.Borders(xlDiagonalUp).LineStyle = xlNone");

      sb.AppendLine("With t_Range.Borders(xlEdgeTop)");
      sb.AppendLine(" .LineStyle = xlContinuous");
      sb.AppendLine(" .Weight = xlMedium");
      sb.AppendLine(" .Color = vbCyan");
      sb.AppendLine("End With");
      sb.AppendLine("With t_Range.Borders(xlEdgeBottom)");
      sb.AppendLine(" .LineStyle = xlContinuous");
      sb.AppendLine(" .Weight = xlMedium");
      sb.AppendLine(" .Color = vbCyan");
      sb.AppendLine("End With");
      sb.AppendLine("With t_Range.Borders(xlEdgeRight)");
      sb.AppendLine(" .LineStyle = xlContinuous");
      sb.AppendLine(" .Weight = xlMedium");
      sb.AppendLine(" .Color = vbCyan");
      sb.AppendLine("End With");
      sb.AppendLine("With t_Range.Borders(xlEdgeLeft)");
      sb.AppendLine(" .LineStyle = xlContinuous");
      sb.AppendLine(" .Weight = xlMedium");
      sb.AppendLine(" .Color = vbCyan");
      sb.AppendLine("End With");

      sb.AppendLine("t_Range.Borders(xlInsideVertical).LineStyle = xlNone");
      sb.AppendLine("t_Range.Borders(xlInsideHorizontal).LineStyle = xlNone");
      sb.AppendLine("ControlError:");
      sb.AppendLine("End Sub");
      sb.AppendLine("");
      module.Code += sb.ToString();
    }

    #endregion AddPivotBorderRange

    #region GetPivotTable

    /// <summary>
    /// Obtiene el pivot de un datatable.
    /// </summary>
    /// <returns> DataTable </returns>
    /// <history>
    /// [edgrodriguez] 12/03/2016  Created. Se agragan columnas antes y despues pivote, los encabezados se ordenan
    /// [aalcocer] 18/05/2016 Modified.
    /// </history>
    public static DataTable GetPivotTable(List<ExcelFormatTable> formatTable, DataTable sourceTable)
    {
      var dt = new DataTable();

      var rowFields = formatTable.Where(c => c.Axis != ePivotFieldAxis.Column && c.Axis != ePivotFieldAxis.Values && c.IsVisible).ToList();
      var columnFields = formatTable.Where(c => c.Axis == ePivotFieldAxis.Column).OrderBy(c => c.Order).ToList();
      var dataFields = formatTable.Where(c => c.Axis == ePivotFieldAxis.Values).OrderBy(c => c.Order).ToList();

      //Nombra a las columnas con campo vacio para posterior quitarlas
      columnFields.ToList().ForEach(c =>
      {
        sourceTable.AsEnumerable().ToList().ForEach(dr =>
        {
          if (dr[c.PropertyName] == null || dr[c.PropertyName] == DBNull.Value || string.IsNullOrWhiteSpace(dr[c.PropertyName].ToString()))
            dr[c.PropertyName] = "NULL";
        });
      });

      //Datos no pivote
      var rowList = sourceTable.DefaultView.ToTable(true, rowFields.Select(c => c.PropertyName).ToArray()).AsEnumerable().ToList();

      // Lista de columnas separados por un caracter
      var columDataRowList = sourceTable.DefaultView.ToTable(true, columnFields.Select(c => c.PropertyName).ToArray()).AsEnumerable().ToList();

      //ordenar las columnas
      columDataRowList = columDataRowList.CopyToDataTable().SortDatatable(columnFields).AsEnumerable().ToList();

      var colList = columDataRowList.Select(x =>
        {
          var list = columnFields.Where(n => n.IsVisible).Select(n => x.Field<object>(n.PropertyName)).ToList();
          return new
          {
            Name = string.Join(separator.ToString(), list)
          };
        }).Distinct().ToList();

      //Columnas antes del pivote
      var rowFieldsBefore = rowFields.Where(c => !c.IsGroup && c.Order <= dataFields.Min(d => d.Order)).OrderBy(c => c.Order).ToList();
      rowFieldsBefore.ForEach(s =>
      {
        var sourcecol = sourceTable.Columns.OfType<DataColumn>().First(c => c.ColumnName == s.PropertyName);
        dt.Columns.Add(s.PropertyName, sourcecol.DataType);
      });

      //Columnas pivote
      colList.ForEach(col =>
        {
          dataFields.ForEach(dataF =>
          {
            var sourcecol = sourceTable.Columns.OfType<DataColumn>().First(c => c.ColumnName == dataF.PropertyName);
            dt.Columns.Add(col.Name.ToString() + separator + dataF.PropertyName, sourcecol.DataType);
          });// Cretes the result columns.//
        });

      //Columnas despues del pivote
      rowFields.Where(c => !rowFieldsBefore.Contains(c)).OrderBy(c => c.IsGroup).ThenBy(c => c.Order).ToList().ForEach(s =>
      {
        var sourcecol = sourceTable.Columns.OfType<DataColumn>().First(c => c.ColumnName == s.PropertyName);
        dt.Columns.Add(s.PropertyName, sourcecol.DataType);
      });

      rowList.ForEach(rowName =>
      {
        var row = dt.NewRow();
        var strFilter = string.Empty;

        rowFields.ForEach(field =>
        {
          row[field.PropertyName] = rowName[field.PropertyName];
          if (rowName[field.PropertyName] != DBNull.Value)
            strFilter += " and [" + field.PropertyName + "] = '" + rowName[field.PropertyName].ToString().Replace("'", "''") + "'";
        });

        strFilter = strFilter.Substring(5);

        colList.ForEach(col =>
        {
          var filter = strFilter;
          var strColValues = col.Name.ToString().Split(separator);

          columnFields.Where(n => n.IsVisible).Select((value, index) => new { Value = value, Index = index }).ToList().ForEach(item =>
          {
            filter += " and [" + item.Value.PropertyName + "] = '" + strColValues[item.Index].ToString().Replace("'", "''") + "'";
          });

          dataFields.ForEach(dataF =>
          {
            var colN = col.Name.ToString() + separator + dataF.PropertyName;
            var data = GetData(filter, dataF, sourceTable);
            row[colN] = data ?? DBNull.Value;
          });
        });

        dt.Rows.Add(row);
      });

      //Eliminamos las columnas con valores Nulos.
      var deleteNullCol = dt.Columns.OfType<DataColumn>().ToList().Where(c => c.ColumnName.Split(separator).Contains("NULL")).ToList();
      deleteNullCol.ForEach(c => dt.Columns.Remove(c.ColumnName));

      return dt;
    }

    #endregion GetPivotTable

    #region GetData

    /// <summary>
    /// Obtiene los valores de las columnas Pivot.
    /// </summary>
    /// <returns> object </returns>
    /// <history>
    ///   [edgrodriguez] 12/03/2016  Created.
    /// </history>
    private static object GetData(string filter, ExcelFormatTable format, DataTable sourceTable)
    {
      try
      {
        var filteredRows = sourceTable.Select(filter);
        var objList = filteredRows.Select(x => x.Field<object>(format.PropertyName)).ToArray();

        switch (format.AggregateFunction)
        {
          case DataFieldFunctions.Average:
            return !objList.Any() ? 0 : (object)(Convert.ToDecimal(objList.Sum(c => Convert.ToDecimal(c)) / objList.Count()));

          case DataFieldFunctions.Count:
            return objList.Count();

          case DataFieldFunctions.Max:
            return objList.Max(c => Convert.ToDecimal(c));

          case DataFieldFunctions.Min:
            return objList.Min(c => Convert.ToDecimal(c));

          case DataFieldFunctions.Sum:
            return objList.Sum(c => Convert.ToDecimal(c));

          default:
            return !objList.Any() ? null : objList.First();
        }
      }
      catch (Exception)
      {
        return "#Error";
      }
    }

    #endregion GetData

    #region GetExcelColumnName

    /// <summary>
    /// Obtiene la letra de una columna.
    /// </summary>
    /// <param name="columnNumber"></param>
    /// <returns> string </returns>
    /// <history>
    ///   [edgrodriguez] 15/03/2016  Created.
    /// </history>
    private static string GetExcelColumnName(int columnNumber)
    {
      var dividend = columnNumber;
      var columnName = string.Empty;
      int modulo;

      while (dividend > 0)
      {
        modulo = (dividend - 1) % 26;
        columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
        dividend = (int)((dividend - modulo) / 26);
      }

      return columnName;
    }

    #endregion GetExcelColumnName

    #region GetFormula

    /// <summary>
    /// Obtiene la formula para asignar a una celda.
    /// </summary>
    /// <param name="formatTable"></param>
    /// <param name="formula"></param>
    /// <param name="rowNumber"></param>
    /// <returns> string </returns>
    /// <history>
    ///   [edgrodriguez] 15/03/2016  Created.
    /// </history>
    private static string GetFormula(List<ExcelFormatTable> formatTable, string formula, int rowNumber)
    {
      var columns = Regex.Matches(formula, @"(\[.*?\])+");
      foreach (var match in columns)
      {
        var formatCol = formatTable.First(c => c.PropertyName == match.ToString().Replace("[", "").Replace("]", ""));
        formula = formula.Replace(match.ToString(), GetExcelColumnName(formatCol.Order) + rowNumber);
      }

      return formula;
    }

    #endregion GetFormula

    #region SortDatatable

    /// <summary>
    /// Ordena los datos de las columnas configuradas.
    /// </summary>
    /// <param name="formatTable"></param>
    /// <param name="dt"></param>
    /// <returns> string </returns>
    /// <history>
    ///   [edgrodriguez] 15/03/2016  Created.
    /// </history>
    private static DataTable SortDatatable(this DataTable dt, List<ExcelFormatTable> formatTable)
    {
      if (formatTable.All(c => c.Sort == eSortType.None))
        return dt;

      //Creamos la sentencia linq para ordenar los registros.
      var qOrder = string.Join(", ", formatTable
        .Where(c => c.Sort != eSortType.None)
        .OrderBy(c => c.Order).Select(x => x.PropertyName + ((x.Sort == eSortType.Descending) ? " desc" : " asc")));

      //Obtenemos las agrupaciones y los registros de cada agrupacion.
      var qTable = dt.DefaultView;
      qTable.Sort = qOrder;

      return qTable.ToTable();
    }

    #endregion 

    #region SetDataFieldShowDataAsAttribute

    /// <summary>
    /// Mostrar distintos cálculos en los campos de valores de tabla dinámica
    /// EPPlus no soporta " Mostrar valores como" para los vaores de la tabla dinamica.
    /// </summary>
    /// <param name="dataField">Campo de dato de la tabla dinamica</param>
    /// <param name="pivot">tabla dinamica</param>
    /// <param name="showDataAs">Tipo del calculo a mostrar</param>
    /// <history>
    ///   [aalcocer] 23/05/2016  Created.
    /// </history>
    private static void SetDataFieldShowDataAsAttribute(this ExcelPivotTableDataField dataField, ExcelPivotTable pivot, EnumDataFieldShowDataAs showDataAs)
    {
      if (pivot != null & pivot.DataFields != null && pivot.DataFields.Contains(dataField))
      {
        var showDataAsAttributeValue = EnumToListHelper.GetEnumDescription(showDataAs);
        var xml = pivot.PivotTableXml;
        var elements = xml.GetElementsByTagName("dataField");

        foreach (XmlNode elem in elements)
        {
          var fldAttribute = elem.Attributes["fld"];
          if (fldAttribute != null && fldAttribute.Value == dataField.Index.ToString())
          {
            var showDataAsAttribute = elem.Attributes["showDataAs"];
            if (showDataAsAttribute == null)
            {
              showDataAsAttribute = xml.CreateAttribute("showDataAs");
              elem.Attributes.InsertAfter(showDataAsAttribute, fldAttribute);
            }
            showDataAsAttribute.Value = showDataAsAttributeValue;
          }
        }
      }
    }

    #endregion SetDataFieldShowDataAsAttribute

    #region AutoFitColumns

    /// <summary>
    /// Aplica el auto ajuste de las columnas segun el contenido.
    /// Recorre las columnas y aplica el ajuste del texto al tamaño de la columna y
    /// se le asigna el tamaño por default a la columna.
    /// </summary>
    /// <history>
    ///   [edgrodriguez] 30/06/2016  Created.
    /// </history>
    private static void AutoFitColumns(ref ExcelWorksheet ws, int cols, int rows, bool isDynamicPivot = false)
    {
      using (var range = ws.Cells[1, 1, (isDynamicPivot)? ws.Cells.End.Row : ws.Dimension.Rows, (isDynamicPivot) ? ws.Cells.End.Column:ws.Dimension.Columns])
      {
        range.AutoFitColumns();
        if (isDynamicPivot) return;
        for (int i = 1; i <= cols; i++)
        {
          var column = ws.Column(i);
          column.Style.WrapText = column.Width > ColumnMaxWidth;
          column.Width = column.Width > ColumnMaxWidth ? ColumnMaxWidth : column.Width;
        }
      }
    }

    #endregion

    #region ReorderColumns
    /// <summary>
    /// Método que siver para ordenar la lista de ExcelFormat dependiedo de la posicion de las columnas de su grid
    /// </summary>
    /// <param name="lstColumns">Lista de columnas del grid</param>
    /// <param name="lstExcelFormatTable">Lista de excelformattable</param>
    /// <history>
    /// [emoguel] created 06/07/2016
    /// </history>
    public static void OrderColumns(List<DataGridColumn> lstColumns, List<ExcelFormatTable> lstExcelFormatTable)
    {
      lstColumns.ForEach(cl =>
      {
        lstExcelFormatTable.Where(eft => eft.PropertyName == cl.SortMemberPath).FirstOrDefault().Order = cl.DisplayIndex + 1;
      });
    }
    #endregion
    #endregion Private Methods
  }
}