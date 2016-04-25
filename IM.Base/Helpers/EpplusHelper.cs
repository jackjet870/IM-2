using IM.Model.Classes;
using IM.Model.Enums;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
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
using System.Xml;

namespace IM.Base.Helpers
{
  public static class EpplusHelper
  {
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
    /// </history>
    public static FileInfo CreateGeneralRptExcel(List<Tuple<string, string>> filter, DataTable dt, string reportName, string dateRangeFileName, List<ExcelFormatTable> formatColumns)
    {
      #region Variables Atributos, Propiedades

      FileInfo pathFinalFile = null;
      ExcelPackage pk = new ExcelPackage();
      //Preparamos la hoja donde escribiremos
      ExcelWorksheet ws = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
      //Filas Para los filtros
      int FilasTotalesFiltros = 0;

      //Renombramos las columnas.
      dt.Columns.Cast<DataColumn>().ToList().ForEach(c =>
      {
        c.ColumnName = formatColumns[dt.Columns.IndexOf(c)].Title;
      });

      #endregion Variables Atributos, Propiedades

      #region Report SuperHeader

      //Creamos la cabecera del reporte (Titulos, Filtros, Fecha y Hora de Impresion)
      CreateReportHeader(filter, reportName, ref ws, ref FilasTotalesFiltros);

      #endregion Report SuperHeader

      #region Contenido del reporte

      //Insertamos las filas que vamos a necesitar
      ws.InsertRow(FilasTotalesFiltros + 3, dt.Rows.Count);

      //Agregamos el contenido empezando en la fila
      ExcelRangeBase range = ws.Cells[FilasTotalesFiltros + 5, 1].LoadFromDataTable(dt, true);
      //El contenido lo convertimos a una tabla
      ExcelTable table = ws.Tables.Add(range, null);
      table.TableStyle = TableStyles.Medium2;

      //Formateamos la tabla
      SetFormatTable(formatColumns, ref table);

      //Agrega totales
      if (formatColumns.Any(c => !string.IsNullOrEmpty(c.Formula) || c.TotalsRowFunction != RowFunctions.None))
      {
        table.ShowTotal = true;
        table.Columns[0].TotalsRowLabel = "TOTAL";
        for (int i = 1; i < formatColumns.Count; i++)
        {
          ExcelTableColumn c = table.Columns[i];
          ExcelFormatTable format = formatColumns[i];
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
      ws.Cells[ws.Dimension.Address].AutoFitColumns();
      //Centramos el titulo de la aplicacion
      ws.Cells[1, 1, 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
      //Centramos el titulo del reporte
      ws.Cells[1, 4, 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

      //Centramos La etiqueta Filters
      if (filter.Count > 0)
      {
        ws.Cells[2, 1, FilasTotalesFiltros + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        ws.Cells[2, 1, FilasTotalesFiltros + 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
      }

      string suggestedFilaName = string.Concat(reportName, " ", dateRangeFileName);

      #endregion Formato de columnas Centrar y AutoAjustar

      #region Generamos y Retornamos la ruta del archivo EXCEL

      pathFinalFile = SaveExcel(pk, suggestedFilaName);

      if (pathFinalFile != null)
      {
        return pathFinalFile;
      }
      return null;

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
    /// <returns> FileInfo </returns>
    /// <history>
    ///   [erosado]      14/03/2016 Created.
    ///   [edgrodriguez] 15/03/2016 Modified. Se agregáron los parametros pivotColumns,pivotRows,pivotValue,pivotColumnsCount
    ///   [edgrodriguez] 17/03/2016 Modified. Se agregó el parametros IsPivot, para indicar si se obtendra una tabla simple o una tabla de tipo pivot.
    ///   [edgrodriguez] 23/03/2016 Modified. Se agregó validaciones para reportes con varios campos Valor, se hizo un cambio en la edición del pivot desde el xml.
    ///                                       Se cambio el formateo de columnas desde un método.
    ///   [aalocer]      01/04/2016 Modified. Se agregó validaciones que las columnas y valores tomen un determinado formato, se agrega la opcion de insertar valores calculados.
    ///   [aalocer]      11/04/2016 Modified. Se agrega la opción de insertar Superheaders y borders a columnas.
    ///   [aalcocer]     16/04/2016 Modified. Se agrego la opcion de mostrar encabezado especial para la primera columna de la tabla
    /// </history>
    public static FileInfo CreatePivotRptExcel(bool isPivot, List<Tuple<string, string>> filters, DataTable dtData,
      string reportName, string dateRangeFileName,
      List<ExcelFormatTable> formatColumns, bool showRowGrandTotal = false, bool showColumnGrandTotal = false, bool showRowHeaders = false)
    {
      FileInfo pathFinalFile = null;
      ExcelPackage pk = new ExcelPackage();
      //Preparamos la hoja donde escribiremos
      //la tabla dinamica.
      ExcelWorksheet wsPivot = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", ""));
      ExcelWorksheet wsData = pk.Workbook.Worksheets.Add("Hoja0");
      wsData.Hidden = eWorkSheetHidden.Hidden;

      int totalFilterRows = 0;

      CreateReportHeader(filters, reportName, ref wsPivot, ref totalFilterRows);

      //Renombramos las columnas.
      dtData.Columns.Cast<DataColumn>().ToList().ForEach(c =>
      {
        c.ColumnName = formatColumns[dtData.Columns.IndexOf(c)].PropertyName ?? formatColumns[dtData.Columns.IndexOf(c)].Title;
      });

      //Cargamos los datos en la hoja0.
      ExcelRangeBase rangeTable = wsData.Cells["A1"].LoadFromDataTable(dtData, true);
      //El contenido lo convertimos a una tabla
      ExcelTable table = wsData.Tables.Add(rangeTable, null);
      table.TableStyle = TableStyles.None;
      //Formateamos la tabla
      SetFormatTable(formatColumns.Where(c => !(c.Axis == ePivotFieldAxis.Values && !string.IsNullOrEmpty(c.Formula))).ToList(), ref table);

      //Cargamos la tabla dinamica.
      ExcelPivotTable pivotTable = wsPivot.PivotTables.Add(wsPivot.Cells[totalFilterRows + 5, 1],
        wsData.Cells[1, 1, wsData.Dimension.End.Row, wsData.Dimension.End.Column], Regex.Replace(reportName, "[^a-zA-Z0-9_]+", ""));

      if (isPivot)
      {
        #region Formato Esquematico

        //Mostrar Encabezados
        pivotTable.ShowHeaders = true;
        //Mostrar Totales por Columna
        pivotTable.ColumGrandTotals = showColumnGrandTotal;
        //Mostrar Totales por Fila
        pivotTable.RowGrandTotals = showRowGrandTotal;

        #endregion Formato Esquematico
      }
      else
      {
        #region Formato Tabular

        if (!showRowHeaders) //Se maneja en formato XML ya el Epplus no cuenta con la propiedad para modificarlo.
          pivotTable.PivotTableXml.DocumentElement.LastChild.Attributes["showRowHeaders"].Value = "0";

        pivotTable.Compact = false;
        pivotTable.CompactData = true;
        pivotTable.Outline = false;
        pivotTable.OutlineData = false;
        pivotTable.Indent = 0;
        pivotTable.ShowMemberPropertyTips = false;
        pivotTable.DataOnRows = false;
        pivotTable.RowGrandTotals = showRowGrandTotal;
        pivotTable.ShowDrill = false;
        pivotTable.EnableDrill = false;
        pivotTable.ColumGrandTotals = showColumnGrandTotal;

        pivotTable.DataCaption = string.Empty;

        pivotTable.MultipleFieldFilters = true;
        pivotTable.GridDropZones = false;
        pivotTable.TableStyle = TableStyles.Medium13;

        ExcelFormatTable fistsRow = formatColumns.Where(c => c.Axis == ePivotFieldAxis.Row && !c.Compact)
          .OrderBy(c => c.Order).FirstOrDefault();
        pivotTable.RowHeaderCaption = fistsRow != null ? fistsRow.Title : string.Empty;

        #endregion Formato Tabular
      }

      //Asignamos las columnas para realizar el pivote
      formatColumns.Where(c => c.Axis == ePivotFieldAxis.Column)
        .OrderBy(c => c.Order)
        .ToList().ForEach(col =>
        {
          ExcelPivotTableField ptfField = pivotTable.ColumnFields.Add(pivotTable.Fields[col.PropertyName ?? col.Title]);

          if (!isPivot) //Si se va manejar el formato tabular.
            ptfField.ShowAll = false;
          ptfField.SubtotalTop = col.SubtotalTop;
          ptfField.SubTotalFunctions = col.SubTotalFunctions;
          ptfField.Sort = col.Sort;
        });

      //Asignamos las filas que se mostraran.
      formatColumns.Where(c => c.Axis == ePivotFieldAxis.Row)
        .OrderBy(c => c.Order)
        .ToList().ForEach(rowFormat =>
      {
        //Lo Agregamos a la lista de Filas.
        ExcelPivotTableField ptfField = pivotTable.RowFields.Add(pivotTable.Fields[rowFormat.PropertyName ?? rowFormat.Title]);

        if (!isPivot) //Si se va manejar el formato tabular.
        {
          ptfField.Outline = rowFormat.Outline;
          ptfField.Compact = rowFormat.Compact;
          ptfField.ShowAll = rowFormat.showAll;
        }

        ptfField.SubtotalTop = rowFormat.SubtotalTop;
        ptfField.SubTotalFunctions = rowFormat.SubTotalFunctions;
        ptfField.Sort = rowFormat.Sort;

        #region Formato

        if (rowFormat.Format != EnumFormatTypeExcel.General)
        {
          //Formato
          PropertyInfo highlightedItemProperty = ptfField.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Single(pi => pi.Name == "TopNode");
          XmlElement ptfFieldXml = (XmlElement)highlightedItemProperty.GetValue(ptfField, null);
          var styles = pivotTable.WorkSheet.Workbook.Styles;
          ExcelNumberFormatXml nFormatXml = styles.NumberFormats.ToList().Find(x => x.Format == GetFormat(rowFormat));

          //Si existe el Formato
          if (nFormatXml != null)
          {
            XmlAttribute numFmtIdAttrib = pivotTable.PivotTableXml.CreateAttribute("numFmtId");
            numFmtIdAttrib.Value = nFormatXml.NumFmtId.ToString();
            ptfFieldXml.Attributes.Append(numFmtIdAttrib);
          }
        }

        #endregion Formato

        #region Salto de linea

        if (rowFormat.InsertBlankRow)
        {
          //Insertar salto de linea
          PropertyInfo highlightedItemProperty = ptfField.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Single(pi => pi.Name == "TopNode");
          XmlElement ptfFieldXml = (XmlElement)highlightedItemProperty.GetValue(ptfField, null);
          XmlAttribute insertBlankRowIdAttrib = pivotTable.PivotTableXml.CreateAttribute("insertBlankRow");
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
        ExcelPivotTableDataField valueField = pivotTable.DataFields.Add(pivotTable.Fields[valueFormat.PropertyName ?? valueFormat.Title]);

        if (!isPivot)
        {
          valueField.Name = valueFormat.Title;
          valueField.BaseField = 0;
          valueField.BaseItem = 0;
          valueField.Function = valueFormat.Function;
        }
        valueField.Format = GetFormat(valueFormat);
        valueField.Field.Sort = valueFormat.Sort;
      });

      //Agregamos valores calculados
      formatColumns.Where(c => c.Axis == ePivotFieldAxis.Values && !string.IsNullOrEmpty(c.Formula))
      .OrderBy(c => c.Order)
      .ToList()
        .ForEach(valueCalculated =>
      {
        if (!isPivot)
        {
          pivotTable.AddCalculatedField(valueCalculated);
        }
      });

      //Agregamos SuperHeader fuera del Pivote
      if (formatColumns.Any(c => c.SuperHeader != null))
      {
        var formatHeaders = formatColumns.Where(c => c.SuperHeader != null).ToList();

        int startColumn = pivotTable.Address.Start.Column;
        int startRow = pivotTable.Address.Start.Row - 1;

        int rowCount = pivotTable.RowFields.Count(c => !(c.Compact && c.Outline));

        var formatHeadersValue = formatHeaders.Where(c => c.Axis == ePivotFieldAxis.Values).OrderBy(c => c.Order).GroupBy(c => c.SuperHeader).ToList();
        formatHeadersValue.ForEach(v =>
        {
          List<ExcelFormatTable> columnasAgrupadas = new List<ExcelFormatTable>();
          int fromCol = 0;
          for (int i = 0; i < v.Count(); i++)
          {
            ExcelFormatTable value = v.ElementAt(i);
            columnasAgrupadas.Add(value);
            if (fromCol == 0)
              fromCol = value.Order + rowCount + startColumn;

            ExcelFormatTable valueNext = v.ElementAtOrDefault(i + 1);
            if (valueNext != null && valueNext.Order == value.Order + 1) continue;

            int toCol = value.Order + rowCount + startColumn;

            using (ExcelRange range = wsPivot.Cells[startRow, fromCol, startRow, toCol])
            {
              range.Value = value.SuperHeader;
              range.Style.Font.Bold = true;
              range.Merge = true;
              range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              range.Style.Fill.PatternType = ExcelFillStyle.Solid;
              range.Style.Fill.BackgroundColor.SetColor(Color.Cyan);
              range.Style.Border.BorderAround(ExcelBorderStyle.Medium);
            }
            pivotTable.AddPivotBorderRange(columnasAgrupadas);
            fromCol = 0;
            columnasAgrupadas.Clear();
          }
        });
      }

      string suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);

      if (pk.Workbook.VbaProject != null) suggestedFilaName += ".xlsm";

      pathFinalFile = SaveExcel(pk, suggestedFilaName);

      return pathFinalFile;
    }

    #endregion CreatePivotRptExcel

    #region createExcelCustom

    /// <summary>
    /// Exporta un Datatable a Excel.
    /// </summary>
    /// <param name="dtTable"> Tabla con la informacion </param>
    /// <param name="filters"> Filtros aplicados al reporte </param>
    /// <param name="reportName"> Nombre del reporte </param>
    /// <param name="formatTable"> Formatos de campos del reporte </param>
    /// <param name="blnIsPivot"> Bandera para indicar si es de tipo Pivot o tabla normal </param>
    /// <param name="larrPivotData"> Informacion acomodada segun la estructura del Pivot </param>
    /// <param name="blnvalidateNull"> Bandera para indicar si se validan los datos nulos </param>
    /// <param name="blnColumnGrandTotal"> Bandera para indicar si se muestra el grandTotal de las columnas </param>
    /// <param name="blnRowsGrandTotal">Bandera para indicar si se muestra el GrandTotal por Fila </param>
    /// <returns> FileInfo </returns>
    /// <history>
    ///   [edgrodriguez] 28/03/2016  Created.
    /// </history>
    public static FileInfo createExcelCustom(DataTable dtTable, List<Tuple<string, string>> filters, string reportName, string dateRangeFileName, List<ExcelFormatTable> formatTable, bool blnColumnGrandTotal = false, bool blnRowGrandTotal = false, bool blnShowSubtotal = false)
    {
      FileInfo pathFinalFile = null;
      ExcelPackage pk = new ExcelPackage();
      var wsData = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
      int totalFilterRows = 0;
      CreateReportHeader(filters, reportName, ref wsData, ref totalFilterRows);

      int RowNumber = totalFilterRows + 5;
      int ColumnNumber = 1;

      //Agregando Columnas sin Pivot
      formatTable
        .Where(c => c.Axis == ePivotFieldAxis.Row)
        .OrderBy(c => c.Order)
        .ToList()
        .ForEach(c =>
      {
        wsData.Cells[RowNumber, ColumnNumber].Value = c.Title;
        wsData.Cells[RowNumber, ColumnNumber].Style.Font.Bold = true;
        wsData.Cells[RowNumber, ColumnNumber].Style.Font.Size = 12;
        ColumnNumber++;
      });

      //Si existe algun grupo
      if (formatTable.Any(c => c.IsGroup))
      {
        #region Simple con Agrupado

        List<ExcelFormatGroupHeaders> backgroundColorGroups = new List<ExcelFormatGroupHeaders> {
            new ExcelFormatGroupHeaders { BackGroundColor="#004E48", FontBold = true, TextAligment = ExcelHorizontalAlignment.Center },
            new ExcelFormatGroupHeaders { BackGroundColor="#147F79", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#2D8B85", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#4CA09A", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left } };

        RowNumber++;
        //Creamos la sentencia Linq para obtener los campos de pivot y valores de pivot.
        var qfields = string.Join(", ", formatTable
          .Where(c => c.IsGroup)
          .OrderBy(c => c.Order).Select(x => "it[\"" + x.PropertyName + "\"] as " + x.PropertyName));
        //Creamos la sentencia linq para ordenar los registros.
        var qOrder = string.Join(", ", formatTable
          .Where(c => c.IsGroup)
          .OrderBy(c => c.Order).Select(x => "Key." + x.PropertyName + ((x.Sort == eSortType.Descending) ? " desc" : " asc")));

        //Obtenemos las agrupaciones y los registros de cada agrupacion.
        var qTable = dtTable
                  .AsEnumerable()
                  .AsQueryable()
                  .GroupBy("new(" + qfields + ")", "it")
                  .OrderBy(qOrder)
                  .Select("new(Key as qgroup, it as Values)");
        //Lista con los headers de cada agrupacion.
        List<string> headers = new List<string>();
        Dictionary<string, List<DataRow>> Values = new Dictionary<string, List<DataRow>>();
        foreach (dynamic item in qTable)
        {
          //obtenemos los grupos.
          object gKey = item.qgroup;
          //Concatenamos los grupos
          var groups = string.Join("|", gKey.GetType().GetProperties().Select(c => c.GetValue(gKey).ToString()).ToList());
          if (!headers.Contains(groups))
          {
            headers.Add(groups);
            Values.Add(groups, (item.Values as IEnumerable<DataRow>).ToList());
          }
        }
        //Obtenemos la cantidad de columnas.
        int totalColumns = formatTable.Where(c => c.Axis == ePivotFieldAxis.Row && !c.IsGroup).Count();
        for (int i = 0; i < headers.Count; i++)
        {
          string[] posAct = headers[i].Split('|');
          if (i == 0)
          {
            for (int j = 0; j < posAct.Length; j++)
            {
              wsData.Cells[RowNumber, 1].Value = posAct[j];
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Merge = true;
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Font.Bold = backgroundColorGroups[j].FontBold;
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
              RowNumber++;
            }
          }
          else if (i > 0)
          {
            string[] posAnt = headers[i - 1].Split('|');
            for (int j = 0; j < posAct.Length; j++)
            {
              if (posAct[j] != posAnt[j])
              {
                wsData.Cells[RowNumber, 1].Value = posAct[j];
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Merge = true;
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Font.Bold = backgroundColorGroups[j].FontBold;
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                RowNumber++;
              }
            }
          }

          Values[headers[i]].ToList().ForEach(dr =>
          {
            int drColumn = 1;
            formatTable.Where(c => c.Axis == ePivotFieldAxis.Row && !c.IsGroup)
            .OrderBy(row => row.Order).ToList().ForEach(row =>
            {
              if (!row.IsCalculated)
                wsData.Cells[RowNumber, drColumn].Value = dr[row.PropertyName];
              else
              {
                wsData.Cells[RowNumber, drColumn].Formula = GetFormula(formatTable, row.Formula, RowNumber);
                //wsData.Cells[RowNumber, drColumn].Calculate();
              }

              wsData.Cells[RowNumber, drColumn].Style.Numberformat.Format = GetFormat(row);
              drColumn++;
            });
            RowNumber++;
          });

          if (blnShowSubtotal)
          {
            int dataIniRow = RowNumber - Values[headers[i]].Count;
            formatTable.Where(c => c.Axis == ePivotFieldAxis.Row && !c.IsGroup)
            .OrderBy(row => row.Order).ToList().ForEach(format =>
            {
              wsData.Cells[RowNumber, format.Order].Style.Numberformat.Format = GetFormat(format);
              if (!format.IsCalculated)
              {
                switch (format.SubTotalFunctions)
                {
                  case eSubTotalFunctions.Sum:
                    wsData.Cells[RowNumber, format.Order].Formula = "=SUM(" + wsData.Cells[dataIniRow, format.Order, RowNumber - 1, format.Order].Address + ")";
                    break;

                  case eSubTotalFunctions.Avg:
                    wsData.Cells[RowNumber, format.Order].Formula = "=AVERAGE(" + wsData.Cells[dataIniRow, format.Order, RowNumber - 1, format.Order].Address + ")";
                    break;
                }
              }
              else
                wsData.Cells[RowNumber, format.Order].Formula = GetFormula(formatTable, format.Formula, RowNumber);
            });
            RowNumber++;
          }
          RowNumber++;
        }
        wsData.Cells[totalFilterRows + 5, 1, RowNumber, totalColumns].AutoFitColumns();

        #endregion Simple con Agrupado
      }
      else
      {
        #region Simple

        RowNumber++;
        dtTable.AsEnumerable().ToList().ForEach(dr =>
        {
          int drColumn = 1;
          formatTable.Where(c => c.Axis == ePivotFieldAxis.Row && !c.IsGroup)
          .OrderBy(row => row.Order).ToList().ForEach(row =>
          {
            wsData.Cells[RowNumber, drColumn].Value = dr[row.PropertyName];
            wsData.Cells[RowNumber, drColumn].Style.Numberformat.Format = GetFormat(row);
            drColumn++;
          });
          RowNumber++;
        });

        if (blnRowGrandTotal)
        {
          int dataIniRow = RowNumber - dtTable.Rows.Count;
          formatTable.Where(c => c.Axis == ePivotFieldAxis.Row && !c.IsGroup)
          .OrderBy(row => row.Order).ToList().ForEach(format =>
          {
            wsData.Cells[RowNumber, format.Order].Style.Numberformat.Format = GetFormat(format);
            if (!format.IsCalculated)
            {
              switch (format.Function)
              {
                case DataFieldFunctions.Sum:
                  wsData.Cells[RowNumber, format.Order].Formula = "=SUM(" + wsData.Cells[dataIniRow, format.Order, RowNumber - 1, format.Order].Address + ")";
                  break;

                case DataFieldFunctions.Average:
                  wsData.Cells[RowNumber, format.Order].Formula = "=AVERAGE(" + wsData.Cells[dataIniRow, format.Order, RowNumber - 1, format.Order].Address + ")";
                  break;
              }
            }
            else
              wsData.Cells[RowNumber, format.Order].Formula = GetFormula(formatTable, format.Formula, RowNumber);
          });
        }

        #endregion Simple
      }

      wsData.Cells[totalFilterRows + 5, 1, RowNumber, dtTable.Columns.Count].AutoFitColumns();

      string suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);

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

    #endregion createExcelCustom

    #region createExcelCustomPivot

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
    /// <returns> FileInfo </returns>
    /// <history>
    ///   [edgrodriguez] 11/04/2016  Created.
    /// </history>
    public static FileInfo createExcelCustomPivot(DataTable dtTable, List<Tuple<string, string>> filters, string reportName, string dateRangeFileName, List<ExcelFormatTable> formatTable, bool blnColumnGrandTotal = false, bool blnRowGrandTotal = false, bool blnShowSubtotal = false)
    {
      FileInfo pathFinalFile = null;
      ExcelPackage pk = new ExcelPackage();
      var wsData = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
      int totalFilterRows = 0;
      CreateReportHeader(filters, reportName, ref wsData, ref totalFilterRows);

      int RowNumber = totalFilterRows + 5 + formatTable.Count(c => c.Axis == ePivotFieldAxis.Column);

      DataTable pivotedTable = GetPivotTable(formatTable, dtTable);

      #region Creando Headers

      List<string[]> Headers = pivotedTable.Columns.OfType<DataColumn>().Select(c => c.ColumnName.Split('|')).ToList();
      int[] IniValue = new int[pivotedTable.Columns.OfType<DataColumn>().Max(c => c.ColumnName.Split('|').Length - 1)];
      int ColumnNumber = 1;
      foreach (string[] item in Headers)
      {
        if (item.Length == 1)
        {
          if (!formatTable.First(c => c.PropertyName == item[0]).IsGroup)
          {
            wsData.Cells[RowNumber, ColumnNumber].Value = formatTable.First(c => c.PropertyName == item[0]).Title;
            wsData.Cells[RowNumber, ColumnNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ColumnNumber++;
          }
        }
        else if (item.Length > 1)
        {
          int rowHeader = RowNumber - (item.Length - 1);
          string[] itemNext = (Headers.IndexOf(item) + 1 < Headers.Count) ? Headers[Headers.IndexOf(item) + 1] : null;

          if (itemNext != null)
          {
            for (int i = 0; i < item.Length; i++)
            {
              if (i < item.Length - 1)
              {
                if (item[i] == itemNext[i])
                {
                  IniValue[i] = IniValue[i] + 1;
                }
                else if (item[i] != itemNext[i])
                {
                  wsData.Cells[rowHeader, ColumnNumber - IniValue[i], rowHeader, ColumnNumber].Value = item[i];
                  wsData.Cells[rowHeader, ColumnNumber - IniValue[i], rowHeader, ColumnNumber].Merge = true;
                  wsData.Cells[rowHeader, ColumnNumber - IniValue[i], rowHeader, ColumnNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                  IniValue[i] = 0;
                }
              }
              else if (i == item.Length - 1)
              {
                wsData.Cells[rowHeader, ColumnNumber].Value = formatTable.First(c => c.PropertyName == item[i]).Title;
                wsData.Cells[rowHeader, ColumnNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              }
              rowHeader++;
            }
          }
          else
          {
            for (int i = 0; i < item.Length; i++)
            {
              if (i < item.Length - 1)
              {
                wsData.Cells[rowHeader, ColumnNumber - IniValue[i], rowHeader, ColumnNumber].Value = item[i];
                wsData.Cells[rowHeader, ColumnNumber - IniValue[i], rowHeader, ColumnNumber].Merge = true;
                wsData.Cells[rowHeader, ColumnNumber - IniValue[i], rowHeader, ColumnNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                IniValue[i] = 0;
              }
              else if (i == item.Length - 1)
              {
                wsData.Cells[rowHeader, ColumnNumber].Value = formatTable.First(c => c.PropertyName == item[i]).Title;
                wsData.Cells[rowHeader, ColumnNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              }
              rowHeader++;
            }
          }
          ColumnNumber++;
        }
      }

      #endregion Creando Headers

      if (formatTable.Any(c => c.IsGroup))
      {
        List<ExcelFormatGroupHeaders> backgroundColorGroups = new List<ExcelFormatGroupHeaders> {
            new ExcelFormatGroupHeaders { BackGroundColor="#004E48", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#147F79", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#2D8B85", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left },
            new ExcelFormatGroupHeaders { BackGroundColor="#4CA09A", FontBold = true, TextAligment = ExcelHorizontalAlignment.Left } };

        #region Agrupado

        RowNumber++;
        //Creamos la sentencia Linq para obtener los campos de pivot y valores de pivot.
        var qfields = string.Join(", ", formatTable
          .Where(c => c.IsGroup)
          .OrderBy(c => c.Order).Select(x => "it[\"" + x.PropertyName + "\"] as " + x.PropertyName));
        //Creamos la sentencia linq para ordenar los registros.
        var qOrder = string.Join(", ", formatTable
          .Where(c => c.IsGroup)
          .OrderBy(c => c.Order).Select(x => "Key." + x.PropertyName + ((x.Sort == eSortType.Descending) ? " desc" : " asc")));

        //Obtenemos las agrupaciones y los registros de cada agrupacion.
        var qTable = pivotedTable
                  .AsEnumerable()
                  .AsQueryable()
                  .GroupBy("new(" + qfields + ")", "it")
                  .OrderBy(qOrder)
                  .Select("new(Key as qgroup, it as Values)");
        //Lista con los headers de cada agrupacion.
        List<string> headers = new List<string>();
        Dictionary<string, List<DataRow>> Values = new Dictionary<string, List<DataRow>>();
        foreach (dynamic item in qTable)
        {
          //obtenemos los grupos.
          object gKey = item.qgroup;
          //Concatenamos los grupos
          var groups = string.Join("|", gKey.GetType().GetProperties().Select(c => c.GetValue(gKey).ToString()).ToList());
          if (!headers.Contains(groups))
          {
            headers.Add(groups);
            Values.Add(groups, (item.Values as IEnumerable<DataRow>).ToList());
          }
        }
        //Obtenemos la cantidad de columnas.
        int totalColumns = pivotedTable.Columns.Count - (formatTable.Count(c => c.IsGroup) - 1);
        for (int i = 0; i < headers.Count; i++)
        {
          string[] posAct = headers[i].Split('|');
          if (i == 0)
          {
            for (int j = 0; j < posAct.Length; j++)
            {
              wsData.Cells[RowNumber, 1].Value = posAct[j];
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Merge = true;
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Font.Bold = backgroundColorGroups[j].FontBold;
              wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
              RowNumber++;
            }
          }
          else if (i > 0)
          {
            string[] posAnt = headers[i - 1].Split('|');
            for (int j = 0; j < posAct.Length; j++)
            {
              if (posAct[j] != posAnt[j])
              {
                wsData.Cells[RowNumber, 1].Value = posAct[j];
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Merge = true;
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.Font.Bold = backgroundColorGroups[j].FontBold;
                wsData.Cells[RowNumber, 1, RowNumber, totalColumns].Style.HorizontalAlignment = backgroundColorGroups[j].TextAligment;
                RowNumber++;
              }
            }
          }

          Values[headers[i]].ToList().ForEach(dr =>
          {
            int drColumn = 1;
            Headers.ForEach(col =>
            {
              var formatCol = formatTable.First(ft => ft.PropertyName == ((col.Length == 1) ? col[0] : col[formatTable.Count(f => f.Axis == ePivotFieldAxis.Column)]));
              if (!formatCol.IsGroup)
              {
                wsData.Cells[RowNumber, drColumn].Value = dr[Headers.IndexOf(col)];
                wsData.Cells[RowNumber, drColumn].Style.Numberformat.Format = GetFormat(formatCol);
                drColumn++;
              }
            });
            RowNumber++;
          });

          if (blnShowSubtotal)
          {
            int dataIniRow = RowNumber - Values[headers[i]].Count;
            int drColumn = 1;
            Headers.ForEach(col =>
            {
              var format = formatTable.First(ft => ft.PropertyName == ((col.Length == 1) ? col[0] : col[formatTable.Count(f => f.Axis == ePivotFieldAxis.Column)]));
              if (!format.IsGroup)
              {
                wsData.Cells[RowNumber, drColumn].Style.Numberformat.Format = GetFormat(format);
                switch (format.SubTotalFunctions)
                {
                  case eSubTotalFunctions.Sum:
                    wsData.Cells[RowNumber, drColumn].Formula = "=SUM(" + wsData.Cells[dataIniRow, drColumn, RowNumber - 1, drColumn].Address + ")";
                    break;
                }
                drColumn++;
              }
            });
            RowNumber++;
          }
          RowNumber++;
        }
        wsData.Cells[totalFilterRows + 5, 1, RowNumber, totalColumns].AutoFitColumns();

        #endregion Agrupado
      }
      else
      {
        #region Agregando Datos

        RowNumber++;
        pivotedTable.AsEnumerable().ToList().ForEach(dr =>
        {
          int drColumn = 1;
          Headers.ForEach(col =>
          {
            var formatCol = formatTable.First(ft => ft.PropertyName == ((col.Length == 1) ? col[0] : col[formatTable.Count(f => f.Axis == ePivotFieldAxis.Column)]));
            wsData.Cells[RowNumber, drColumn].Value = dr[Headers.IndexOf(col)];
            wsData.Cells[RowNumber, drColumn].Style.Numberformat.Format = GetFormat(formatCol);
            drColumn++;
          });
          RowNumber++;
        });

        if (blnRowGrandTotal)
        {
          int dataIniRow = RowNumber - pivotedTable.Rows.Count;
          int drColumn = 1;
          Headers.ForEach(col =>
          {
            var format = formatTable.First(ft => ft.PropertyName == ((col.Length == 1) ? col[0] : col[formatTable.Count(f => f.Axis == ePivotFieldAxis.Column)]));
            if (!format.IsGroup)
            {
              wsData.Cells[RowNumber, drColumn].Style.Numberformat.Format = GetFormat(format);
              switch (format.Function)
              {
                case DataFieldFunctions.Sum:
                  wsData.Cells[RowNumber, drColumn].Formula = "=SUM(" + wsData.Cells[dataIniRow, drColumn, RowNumber - 1, drColumn].Address + ")";
                  break;
              }
              drColumn++;
            }
          });
          RowNumber++;
        }

        wsData.Cells[totalFilterRows + 5, 1, RowNumber, pivotedTable.Columns.Count].AutoFitColumns();

        #endregion Agregando Datos
      }
      string suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);

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

    #endregion createExcelCustomPivot

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
      ExcelWorkbook excelWorkbook = tableData.WorkSheet.Workbook;
      //Agregamos estilo a los Headers de la Tabla
      ExcelNamedStyleXml namedStyle = excelWorkbook.Styles.CreateNamedStyle("HeaderRowCellStyle");
      namedStyle.Style.Font.Bold = true;
      namedStyle.Style.Font.Size = 14;
      tableData.HeaderRowCellStyle = namedStyle.Name;

      int contColumn = 0;
      foreach (ExcelFormatTable item in formatColumns)
      {
        ExcelNamedStyleXml tableStyle = excelWorkbook.Styles.CreateNamedStyle("TableColumnStyle" + contColumn);
        tableStyle.Style.HorizontalAlignment = item.Alignment;
        switch (item.Format)
        {
          case EnumFormatTypeExcel.General:
            break;

          case EnumFormatTypeExcel.Percent:
          case EnumFormatTypeExcel.Currency:
          case EnumFormatTypeExcel.Number:
          case EnumFormatTypeExcel.DecimalNumber:
          case EnumFormatTypeExcel.Date:
          case EnumFormatTypeExcel.Time:
          case EnumFormatTypeExcel.Month:
            tableStyle.Style.Numberformat.Format = GetFormat(new ExcelFormatTable { Format = item.Format });
            break;

          case EnumFormatTypeExcel.Boolean:
            tableStyle.Style.Font.Name = "Wingdings";

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
    /// <param name="item">Formato de Excel.</param>
    /// <returns>string</returns>
    /// <history>
    ///   [edgrodriguez] 24/03/2016  Created.
    /// </history>
    private static string GetFormat(ExcelFormatTable item)
    {
      string format = "";
      switch (item.Format)
      {
        case EnumFormatTypeExcel.General:
          break;

        case EnumFormatTypeExcel.Percent:
          format = "0.0 %;-0.0 %;";
          break;

        case EnumFormatTypeExcel.Currency:
          format = "_-$ #,##0.00_-;-$ #,##0.00_-;_-$*  - ??_-;_-@_-";
          break;

        case EnumFormatTypeExcel.Number:
          format = "#";
          break;

        case EnumFormatTypeExcel.DecimalNumber:
          format = "0.00;-0.00;";
          break;

        case EnumFormatTypeExcel.Date:
          format = "dd/MM/yyyy";
          break;

        case EnumFormatTypeExcel.Time:
          format = "hh:mm AM/PM";
          break;

        case EnumFormatTypeExcel.Month:
          format = "[$-409]mmmm";
          break;
      }

      return format;
    }

    #endregion GetFormat

    #region Create ReportHeader

    /// <summary>
    /// Crea la cabecera para el reporte
    /// </summary>
    /// <param name="filterList">Lista de filtros</param>
    /// <param name="reportName">Nombre del reporte</param>
    /// <param name="ws">ExcelWorkdsheet</param>
    /// <param name="totalFilterRows">totalFilterRows</param>
    ///
    private static void CreateReportHeader(List<Tuple<string, string>> filterList, string reportName, ref ExcelWorksheet ws, ref int totalFilterRows)
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
      ws.Cells[1, 1, 1, 19].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;//Doble Linea

      #endregion Titulo del reporte

      #region Filtros

      //Validamos que el reporte tenga filtros
      if (filterList.Count > 0)
      {
        //Calculamos el numero de filas para ingresar nuestros filtros
        double filterDivision = filterNumber / 4;
        double filasTotales = Math.Ceiling(filterDivision); //Obtenemos el numero de filas totales.
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
        int cIteraccionColumn = 2;//Contador de columnas de escritura
        foreach (Tuple<string, string> item in filterList)
        {
          cIteraccionReal++;

          double rowDivision = cIteraccionReal / 4; //Obtenemos el numero de filtro a escribir
          double filaEscritura = Math.Ceiling(rowDivision); //Redondeamos Para saber en que fila se encuenta

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
      ws.Cells[totalFilterRows + 2, 1].Value = "Print Date Time";
      ws.Cells[totalFilterRows + 2, 1].Style.Font.Bold = true;

      //Agregamos el valor de fecha y hora de impresion
      ws.Cells[totalFilterRows + 2, 2].Value = string.Format("{0:MM/dd/yyyy hh:mm:ss}", DateTime.Now);

      #endregion Datos de la impresion
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

    #endregion SaveExcel

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
      XmlElement cacheFieldsElement = pivotTable.CacheDefinition.CacheDefinitionXml.GetElementsByTagName("cacheFields")[0] as XmlElement;

      // Añadir el elemento cacheField y tomar nota de que el índice
      XmlAttribute cacheFieldsCountAttribute = cacheFieldsElement.Attributes["count"];
      int count = Convert.ToInt32(cacheFieldsCountAttribute.Value);
      cacheFieldsElement.InnerXml += $"<cacheField name=\"{formatTable.Title}\" numFmtId=\"0\" formula=\"{formatTable.Formula}\" databaseField=\"0\"/>\n";
      int cacheFieldIndex = ++count;
      // Campos de actualización de memoria caché cuentan atributo
      cacheFieldsCountAttribute.Value = count.ToString();

      // A continuación, actualizar e insertar pivotTable1.xml elemento PivotField como un hijo del elemento PivotFields
      XmlElement pivotFieldsElement = pivotTable.PivotTableXml.GetElementsByTagName("pivotFields")[0] as XmlElement;
      XmlAttribute pivotFieldsCountAttribute = pivotFieldsElement.Attributes["count"];
      pivotFieldsElement.InnerXml += "<pivotField dataField=\"1\" compact=\"0\" outline=\"0\" subtotalTop=\"0\" dragToRow=\"0\" dragToCol=\"0\" dragToPage=\"0\" showAll=\"0\" includeNewItemsInFilter=\"1\" defaultSubtotal=\"0\"/> \n";
      //actualizar el atributo cantidad de  pivotFields
      pivotFieldsCountAttribute.Value = (int.Parse(pivotFieldsCountAttribute.Value) + 1).ToString();

      // También en pivotTable1.xml, inserte el <dataField> en la posición correcta, el FLD aquí apunta al caché de índice de Campo
      XmlElement dataFields = pivotTable.PivotTableXml.GetElementsByTagName("dataFields")[0] as XmlElement;

      // Crear el elemento dataField con los atributos
      XmlElement dataField = pivotTable.PivotTableXml.CreateElement("dataField", pivotTable.PivotTableXml.DocumentElement.NamespaceURI);
      dataField.RemoveAllAttributes();
      XmlAttribute nameAttrib = pivotTable.PivotTableXml.CreateAttribute("name");

      // Caché campo no puede tener el mismo nombre que el atributo dataField
      if (caption == null || caption == formatTable.Title)
        nameAttrib.Value = " " + formatTable.Title;
      else
        nameAttrib.Value = caption;
      dataField.Attributes.Append(nameAttrib);

      XmlAttribute fldAttrib = pivotTable.PivotTableXml.CreateAttribute("fld");

      fldAttrib.Value = (cacheFieldIndex - 1).ToString();
      dataField.Attributes.Append(fldAttrib);
      XmlAttribute baseFieldAttrib = pivotTable.PivotTableXml.CreateAttribute("baseField");
      baseFieldAttrib.Value = "0";
      dataField.Attributes.Append(baseFieldAttrib);
      XmlAttribute baseItemAttrib = pivotTable.PivotTableXml.CreateAttribute("baseItem");
      baseItemAttrib.Value = "0";
      dataField.Attributes.Append(baseItemAttrib);

      var styles = pivotTable.WorkSheet.Workbook.Styles;
      ExcelNumberFormatXml nFormatXml = styles.NumberFormats.ToList().Find(x => x.Format == GetFormat(formatTable));

      //Si no existe el Formato se crea uno
      if (nFormatXml == null)
      {
        ExcelPivotTableDataField dataFieldAux = pivotTable.DataFields.First();
        string formatAux = dataFieldAux.Format;
        dataFieldAux.Format = GetFormat(formatTable);
        dataFieldAux.Format = formatAux;
        nFormatXml = styles.NumberFormats.ToList().Find(x => x.Format == GetFormat(formatTable));
      }

      XmlAttribute numFmtIdAttrib = pivotTable.PivotTableXml.CreateAttribute("numFmtId");
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
        XmlNode insertBeforeThis = dataFields.ChildNodes.Item(formatTable.Order);
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

      ExcelVBAModule module = pivotTable.WorkSheet.Workbook.VbaProject.Modules.Single(m => m.Type == eModuleType.Module);
      List<string> lines = pivotTable.WorkSheet.Workbook.CodeModule.Code.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

      string nombreSub = "BorderPivotTable" + (lines.Count - 1);
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
      List<string> pivotFieldList = new List<string>();
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
    ///   [edgrodriguez] 12/03/2016  Created.
    /// </history>
    private static DataTable GetPivotTable(List<ExcelFormatTable> formatTable, DataTable _SourceTable)
    {
      DataTable dt = new DataTable();
      ExcelFormatTable[] RowFields = formatTable.Where(c => c.Axis == ePivotFieldAxis.Row).OrderBy(c => c.Order).ToArray();
      ExcelFormatTable[] ColumnFields = formatTable.Where(c => c.Axis == ePivotFieldAxis.Column).OrderBy(c => c.Order).ToArray();
      ExcelFormatTable[] DataFields = formatTable.Where(c => c.Axis == ePivotFieldAxis.Values).OrderBy(c => c.Order).ToArray();

      ColumnFields.ToList().ForEach(c =>
      {
        _SourceTable.AsEnumerable().ToList().ForEach(dr =>
        {
          if (dr[c.PropertyName] == null || dr[c.PropertyName] == DBNull.Value || dr[c.PropertyName].ToString() == "")
            dr[c.PropertyName] = "NULL";
        });
      });

      string Separator = "|";
      var RowList = _SourceTable.DefaultView.ToTable(true, RowFields.Select(c => c.PropertyName).ToArray()).AsEnumerable().ToList();
      for (int index = RowFields.Count() - 1; index >= 0; index--)
        RowList = RowList.OrderBy(x => x.Field<object>(RowFields[index].PropertyName)).ToList();
      // Gets the list of columns .(dot) separated.
      var ColList = (from x in _SourceTable.AsEnumerable()
                     select new
                     {
                       Name = ColumnFields.Select(n => x.Field<object>(n.PropertyName))
                             .Aggregate((a, b) => a += Separator + b.ToString())
                     })
                         .Distinct()
                         .OrderBy(m => m.Name);

      foreach (var s in RowFields)
      {
        DataColumn _Sourcecol = _SourceTable.Columns.OfType<DataColumn>().First(c => c.ColumnName == s.PropertyName);
        dt.Columns.Add(s.PropertyName, _Sourcecol.DataType);
      }

      foreach (var col in ColList)
        foreach (var dataF in DataFields)
        {
          DataColumn _Sourcecol = _SourceTable.Columns.OfType<DataColumn>().First(c => c.ColumnName == dataF.PropertyName);
          dt.Columns.Add(col.Name.ToString() + Separator + dataF.PropertyName, _Sourcecol.DataType);
        } // Cretes the result columns.//

      foreach (var RowName in RowList)
      {
        DataRow row = dt.NewRow();
        string strFilter = string.Empty;

        foreach (var Field in RowFields)
        {
          row[Field.PropertyName] = RowName[Field.PropertyName];
          if (RowName[Field.PropertyName] != DBNull.Value)
            strFilter += " and " + Field.PropertyName + " = '" + RowName[Field.PropertyName].ToString() + "'";
        }
        strFilter = strFilter.Substring(5);

        foreach (var col in ColList)
        {
          string filter = strFilter;
          string[] strColValues = col.Name.ToString().Split(Separator.ToCharArray(), StringSplitOptions.None);

          for (int i = 0; i < ColumnFields.Length; i++)

            filter += " and " + ColumnFields[i].PropertyName + " = '" + strColValues[i] + "'";
          foreach (var dataF in DataFields)
          {
            string colN = col.Name.ToString() + Separator + dataF.PropertyName;
            row[colN] = GetData(filter, dataF, _SourceTable);
          }
        }

        dt.Rows.Add(row);
      }
      //Eliminamos las columnas con valores Nulos.
      var deleteNullCol = dt.Columns.OfType<DataColumn>().ToList().Where(c => c.ColumnName.Split(Separator.ToCharArray(), StringSplitOptions.None).Contains("NULL")).ToList();
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
    private static object GetData(string Filter, ExcelFormatTable format, DataTable _SourceTable)
    {
      try
      {
        DataRow[] FilteredRows = _SourceTable.Select(Filter);
        object[] objList = FilteredRows.Select(x => x.Field<object>(format.PropertyName)).ToArray();

        switch (format.AggregateFunction)
        {
          case DataFieldFunctions.Average:
            return objList.Count() == 0 ? 0 : (object)(Convert.ToDecimal(objList.Sum(c => Convert.ToDecimal(c)) / objList.Count()));

          case DataFieldFunctions.Count:
            return objList.Count();

          case DataFieldFunctions.Max:
            return objList.Max(c => Convert.ToDecimal(c));

          case DataFieldFunctions.Min:
            return objList.Min(c => Convert.ToDecimal(c));

          case DataFieldFunctions.Sum:
            return objList.Sum(c => Convert.ToDecimal(c));

          default:
            return objList.Count() == 0 ? 0 : objList.First();
        }
      }
      catch (Exception ex)
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
      int dividend = columnNumber;
      string columnName = String.Empty;
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
    /// <param name="columnNumber"></param>
    /// <returns> string </returns>
    /// <history>
    ///   [edgrodriguez] 15/03/2016  Created.
    /// </history>
    private static string GetFormula(List<ExcelFormatTable> formatTable, string Formula, int rowNumber)
    {
      var Columns = Regex.Matches(Formula, @"(\[.*?\])+");
      foreach (var match in Columns)
      {
        var formatCol = formatTable.First(c => c.PropertyName == match.ToString().Replace("[", "").Replace("]", ""));
        Formula = Formula.Replace(match.ToString(), GetExcelColumnName(formatCol.Order) + rowNumber);
      }

      return Formula;
    }

    #endregion GetFormula

    #endregion Private Methods
  }
}