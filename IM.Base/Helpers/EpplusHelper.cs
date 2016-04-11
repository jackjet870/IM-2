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
      //Numero de filas del contenido del datatable
      int dtRowsNumber = dt.Rows.Count;
      //Numero de Columnas del contenido del datatable
      int dtColumnsNumber = dt.Columns.Count;

      #endregion Variables Atributos, Propiedades

      #region Report SuperHeader

      //Creamos la cabecera del reporte (Titulos, Filtros, Fecha y Hora de Impresion)
      CreateReportHeader(filter, reportName, ref ws, ref FilasTotalesFiltros);

      #endregion Report SuperHeader

      #region Contenido del reporte

      //Insertamos las filas que vamos a necesitar
      ws.InsertRow(FilasTotalesFiltros + 3, dt.Rows.Count);

      //Formateamos las columnas.
      SetFormatColumns(formatColumns, ref ws, FilasTotalesFiltros, dtRowsNumber, dtColumnsNumber, false);

      //Agregamos el contenido empezando en la fila
      ws.Cells[FilasTotalesFiltros + 5, 1].LoadFromDataTable(dt, false, OfficeOpenXml.Table.TableStyles.Medium2);

      #endregion Contenido del reporte

      #region Formato de columnas Centrar y AutoAjustar

      //Auto Ajuste de columnas de  acuerdo a su contenido
      ws.Cells[1, 1, FilasTotalesFiltros + 3 + dtRowsNumber, dtColumnsNumber].AutoFitColumns();
      //Centramos el titulo de la aplicacion
      ws.Cells[1, 1, 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
      //Centramos el titulo del reporte
      ws.Cells[1, 4, 1, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

      //Centramos La etiqueta Filters
      if (filter.Count > 0)
      {
        ws.Cells[2, 1, FilasTotalesFiltros + 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        ws.Cells[2, 1, FilasTotalesFiltros + 1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
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
    /// <param name="pivotColums">Campos que se les realizará el pivot.</param>
    /// <param name="pivotRows">Campos que se mostrarán como filas.</param>
    /// <param name="pivotValue">Campo el cual será el valor de la columna pivot.</param>
    /// <param name="pivotColumnsCount">Cantidad de Columnas de la tabla ya connvertida a pivot.</param>
    /// <param name="pivotValueCalculated">Campos el cual se agregaran como columnas calculadas</param>
    /// <returns> FileInfo </returns>
    /// <history>
    ///   [erosado]      14/03/2016 Created.
    ///   [edgrodriguez] 15/03/2016 Modified. Se agregáron los parametros pivotColumns,pivotRows,pivotValue,pivotColumnsCount
    ///   [edgrodriguez] 17/03/2016 Modified. Se agregó el parametros IsPivot, para indicar si se obtendra una tabla simple o una tabla de tipo pivot.
    ///   [edgrodriguez] 23/03/2016 Modified. Se agregó validaciones para reportes con varios campos Valor, se hizo un cambio en la edición del pivot desde el xml.
    ///                                       Se cambio el formateo de columnas desde un método.
    ///   [aalocer]      01/04/2016 Modified. Se agregó validaciones que las columnas y valores tomen un determinado formato, se agrega la opcion de insertar valores calculados.
    ///   [aalocer]      11/04/2016 Modified. Se agrega la opción de insertar Superheaders y borders a columnas.
    /// </history>
    public static FileInfo CreatePivotRptExcel(bool isPivot, List<Tuple<string, string>> filters, DataTable dtData,
      string reportName, string dateRangeFileName,
      List<ExcelFormatTable> formatColumns, bool showRowGrandTotal = false, bool showColumnGrandTotal = false)
    {
      FileInfo pathFinalFile = null;
      ExcelPackage pk = new ExcelPackage();
      //Preparamos la hoja donde escribiremos
      //la tabla dinamica.
      ExcelWorksheet wsPivot = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
      ExcelWorksheet wsData = pk.Workbook.Worksheets.Add("Hoja0");
      wsData.Hidden = eWorkSheetHidden.Hidden;

      ////Numero de filtros
      //int filterNumber = filters.Count + 2;

      ////Numero de filas del contenido del datatable
      int dtRowsNumber = dtData.Rows.Count;
      int totalFilterRows = 0;
      ////Numero de Columnas del contenido del datatable
      int dtColumnsNumber = formatColumns.Count(c => c.Axis == ePivotFieldAxis.Column) == 0 ? dtData.Columns.Count : formatColumns.Count(c => c.Axis == ePivotFieldAxis.Column);
      CreateReportHeader(filters, reportName, ref wsPivot, ref totalFilterRows);

      //Renombramos las columnas.
      dtData.Columns.Cast<DataColumn>().ToList().ForEach(c =>
      {
        c.ColumnName = formatColumns[dtData.Columns.IndexOf(c)].PropertyName ?? formatColumns[dtData.Columns.IndexOf(c)].Title;
      });

      //Formateamos los campos.
      SetFormatColumns(formatColumns.Where(c => !(c.Axis == ePivotFieldAxis.Values && !string.IsNullOrEmpty(c.Formula))).ToList(), ref wsData, 0, dtRowsNumber, dtColumnsNumber, true);

      //Cargamos los datos en la hoja0.
      wsData.Cells["A1"].LoadFromDataTable(dtData, true, TableStyles.None);

      //Cargamos la tabla dinamica.
      var pivotTable = wsPivot.PivotTables.Add(wsPivot.Cells[totalFilterRows + 5, 1],
        wsData.Cells[1, 1, wsData.Dimension.End.Row, wsData.Dimension.End.Column], Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));

      if (isPivot)
      {
        #region Formato Esquematico

        //Mostrar Encabezados
        pivotTable.ShowHeaders = true;
        //Mostrar Totales por Columna
        pivotTable.ColumGrandTotals = true;
        //Mostrar Totales por Fila
        pivotTable.RowGrandTotals = true;

        #endregion Formato Esquematico
      }
      else
      {
        #region Formato Tabular

        //Se maneja en formato XML ya el Epplus no cuenta con la propiedad para modificarlo.
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
        pivotTable.RowHeaderCaption = string.Empty;

        pivotTable.MultipleFieldFilters = true;
        pivotTable.GridDropZones = false;
        pivotTable.TableStyle = TableStyles.Medium13;

        #endregion Formato Tabular
      }

      //Asignamos las columnas para realizar el pivote
      formatColumns.Where(c => c.Axis == ePivotFieldAxis.Column)
        .OrderBy(c => c.Order)
        .ToList().ForEach(col =>
        {
          ExcelPivotTableField ptfField = pivotTable.ColumnFields.Add(pivotTable.Fields[col.PropertyName ?? col.Title]);

          if (!isPivot) //Si se va manejar el formato tabular.
          {
            ptfField.ShowAll = false;
            ptfField.SubtotalTop = col.SubtotalTop;
            ptfField.SubTotalFunctions = col.SubTotalFunctions;
          }

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
          ptfField.SubtotalTop = rowFormat.SubtotalTop;
          ptfField.SubTotalFunctions = rowFormat.SubTotalFunctions;
        }
        ptfField.Sort = rowFormat.Sort;
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
          valueField.Format = GetFormat(valueFormat);
          valueField.BaseField = 0;
          valueField.BaseItem = 0;
          valueField.Function = valueFormat.Function;
        }
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

        int valuesCount = pivotTable.DataFields.Count;
        int rowCount = pivotTable.RowFields.Count(c => !(c.Compact && c.Outline));

        var formatHeadersRow = formatHeaders.Where(c => c.Axis == ePivotFieldAxis.Row).OrderBy(c => c.Order).ToList();
        foreach (var x in formatHeadersRow)
        {
        }

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

      try
      {
        pathFinalFile = SaveExcel(pk, suggestedFilaName);
      }
      catch (Exception ex)
      {
      }

      return pathFinalFile;
    }

    #endregion CreatePivotRptExcel

    #region createExcelCustom

    /// <summary>
    ///
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
    ///  <history>
    ///   [edgrodriguez] 28/03/2016  Created.
    /// </history>
    public static FileInfo createExcelCustom(DataTable dtTable, List<Tuple<string, string>> filters, string reportName, string dateRangeFileName, List<ExcelFormatTable> formatTable, bool blnIsPivot = false, List<object[]> larrPivotData = null, bool blnvalidateNull = false, bool blnColumnGrandTotal = false, bool blnRowsGrandTotal = false)
    {
      FileInfo pathFinalFile = null;
      ExcelPackage pk = new ExcelPackage();
      var wsData = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
      int totalFilterRows = 0;
      CreateReportHeader(filters, reportName, ref wsData, ref totalFilterRows);

      int ValuesCount = formatTable.Count(c => c.Axis == ePivotFieldAxis.Values);
      int ColumnsCount = formatTable.Count(c => c.Axis == ePivotFieldAxis.Column);

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

      if (blnIsPivot)
      {
        //Si recibimos columnas para pivot y valores de pivot.
        if (formatTable.Any(c => c.Axis == ePivotFieldAxis.Values) && formatTable.Any(c => c.Axis == ePivotFieldAxis.Column))
        {
          //Creamos la sentencia Linq para obtener los campos de pivot y valores de pivot.
          var qfields = string.Join(", ", formatTable
            .Where(c => c.Axis == ePivotFieldAxis.Column)
            .OrderBy(c => c.Order).Select(x => "it[\"" + x.PropertyName + "\"] as " + x.PropertyName));
          var qvalues = string.Join(", ", formatTable
            .Where(c => c.Axis == ePivotFieldAxis.Values)
            .OrderBy(c => c.Order).Select(x => "it[\"" + x.PropertyName + "\"] as " + x.PropertyName));

          //Obtenemos los datos. Agrupados por las columnas pivot y los seleccionamos los valores
          var qTable = dtTable
                .AsEnumerable()
                .AsQueryable()
                .GroupBy("new(" + qfields + ")", "it")
                .Select("new(Key as Group)");

          //Obtenemos los objetos agrupados.
          foreach (dynamic qData in qTable)
          {
            //Obtenemos los valores del Grupo.
            object qGroup = qData.Group;
            //Obtenemos los encabezados de cada columna Pivot.
            var headers = qGroup.GetType().GetProperties().Select(c => c.GetValue(qGroup) ?? null).ToList();

            int RowsHeader = (ValuesCount > 1) ? RowNumber - (ColumnsCount) : RowNumber - (ColumnsCount - 1);
            //Agregamos los valores de los header de las columna pivot.
            headers.ForEach(h =>
            {
              if (h != null && h.ToString() != "")
              {
                wsData.Cells[RowsHeader, ColumnNumber].Value = h;
                wsData.Cells[RowsHeader, ColumnNumber, RowsHeader, ColumnNumber + (ValuesCount - 1)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                wsData.Cells[RowsHeader, ColumnNumber, RowsHeader, ColumnNumber + (ValuesCount - 1)].Style.Font.Bold = true;
                wsData.Cells[RowsHeader, ColumnNumber, RowsHeader, ColumnNumber + (ValuesCount - 1)].Style.Font.Size = 12;
                wsData.Cells[RowsHeader, ColumnNumber, RowsHeader, ColumnNumber + (ValuesCount - 1)].Style.ShrinkToFit = true;
                wsData.Cells[RowsHeader, ColumnNumber, RowsHeader, ColumnNumber + (ValuesCount - 1)].Style.WrapText = true;
                wsData.Cells[RowsHeader, ColumnNumber, RowsHeader, ColumnNumber + (ValuesCount - 1)].Merge = true;
                RowsHeader++;
              }
            });

            if (ColumnsCount > 0 && ValuesCount > 1)
            {
              //Solo se realiza este proceso si hay mas de 1 columna de valor.
              if (headers.All(c => c != null && c.ToString() != ""))
              {
                //Agregamos las columnas de los valores.
                formatTable
                  .Where(c => c.Axis == ePivotFieldAxis.Values)
                  .OrderBy(c => c.Order).ToList().ForEach(c =>
                {
                  wsData.Cells[RowsHeader, ColumnNumber].Value = c.Title;
                  wsData.Cells[RowsHeader, ColumnNumber, RowsHeader, ColumnNumber + (ValuesCount - 1)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                  wsData.Cells[RowsHeader, ColumnNumber, RowsHeader, ColumnNumber + (ValuesCount - 1)].Style.Font.Bold = true;
                  wsData.Cells[RowsHeader, ColumnNumber, RowsHeader, ColumnNumber + (ValuesCount - 1)].Style.Font.Size = 12;
                  ColumnNumber++;
                });
              }
            }
            else
              ColumnNumber++;
          }
          RowNumber++;

          //Agregamos los valores de las filas y columnas.
          var lstPivotRowsIndex = formatTable
                  .Where(c => c.Axis == ePivotFieldAxis.Row)
                  .OrderBy(c => c.Order).ToList().Select(c => formatTable.IndexOf(c)).ToList();
          larrPivotData.ForEach(c =>
          {
            var columnRow = 1;
            int pivotValueCount = 0;
            //c.Cast<object>().ToList().ForEach(d =>
            foreach (var d in c)
            {
              if (lstPivotRowsIndex.Exists(idx => idx == (columnRow - 1)))
              {
                wsData.Cells[RowNumber, columnRow].Value = (d != null) ? d : "";
                //wsData.Cells[RowNumber, columnRow].Style.Numberformat.Format = GetFormat(formatTable.First(ft => ft.PropertyName == lstrPivotRows[columnRow - 1]));
                wsData.Cells[RowNumber, columnRow].AutoFitColumns();
                columnRow++;
              }
              else
              {
                if (blnvalidateNull)
                {
                  if (d != null && d.ToString() != "")
                  {
                    wsData.Cells[RowNumber, columnRow].Value = d;
                    //wsData.Cells[RowNumber, columnRow].Style.Numberformat.Format = GetFormat(formatTable.First(ft => ft.PropertyName == lstrPivotValues[pivotValueCount]));
                    wsData.Cells[RowNumber, columnRow].AutoFitColumns();
                    columnRow++;
                    pivotValueCount++;
                  }
                  else
                    columnRow++;
                }
                else
                {
                  wsData.Cells[RowNumber, columnRow].Value = (d != null) ? d : "";
                  wsData.Cells[RowNumber, columnRow].Style.Numberformat.Format = GetFormat(formatTable[columnRow]);
                  wsData.Cells[RowNumber, columnRow].AutoFitColumns();
                  columnRow++;
                  pivotValueCount++;
                }
                if (pivotValueCount == ValuesCount)
                  pivotValueCount = 0;
              }
            };
            RowNumber++;
          });

          if (blnColumnGrandTotal)
          {
            int DataIniRow = (RowNumber - larrPivotData.Count);

            for (int i = 1; i <= lstPivotRowsIndex.Count; i++)
            {
              ExcelFormatTable format = formatTable[lstPivotRowsIndex[i - 1]];
              //Formateamos toda la columna.
              wsData.Cells[DataIniRow, i, RowNumber, i].Style.Numberformat.Format = GetFormat(format);
              switch (format.Function)
              {
                case DataFieldFunctions.Sum:
                  wsData.Cells[RowNumber, i].Formula = "=SUM(" + wsData.Cells[DataIniRow, i, RowNumber - 1, i].Address + ")";
                  break;
              }
            }

            //Columnas Pivot
            if (ColumnsCount > 0)
            {
              int DataIniCol = lstPivotRowsIndex.Count + 1;
              int TotalColumns = larrPivotData.Max(c => c.Length);
              int j = 0;
              var pivotColumns = formatTable.Where(c => c.Axis == ePivotFieldAxis.Values).OrderBy(c => c.Order).ToList();
              for (int i = DataIniCol; i <= TotalColumns; i++)
              {
                ExcelFormatTable format = pivotColumns[j];
                //Formateamos toda la columna.
                wsData.Cells[DataIniRow, i, RowNumber, i].Style.Numberformat.Format = GetFormat(format);
                switch (format.Function)
                {
                  case DataFieldFunctions.Sum:
                    wsData.Cells[RowNumber, i].Formula = "=SUM(" + wsData.Cells[DataIniRow, i, RowNumber - 1, i].Address + ")";
                    break;
                }
                j++;
                if (j == ValuesCount)
                  j = 0;
              }
            }
          }
          wsData.Cells[(filters.Count > 0) ? filters.Count + 1 : 1, 1, RowNumber, ColumnNumber].AutoFitColumns();
        }
      }

      string suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);

      try
      {
        pathFinalFile = SaveExcel(pk, suggestedFilaName);
      }
      catch (Exception ex)
      {
      }

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

    #endregion Public Methods

    #region Private Methods

    #region SetFormatColumns

    /// <summary>
    ///   Se aplica el formato a las columnas de la tabla.
    /// </summary>
    /// <param name="formatColumns">Lista de Formatos de columna del reporte.</param>
    /// <param name="wsData">Hoja de Excel.</param>
    /// <param name="totalRowsFilter">Cantidad de Filtros.</param>
    /// <param name="dtRowsNumber">Cantidad de Filas</param>
    /// <param name="dtColumnsNumber">Cantidad de Columnas.</param>
    /// <param name="isPivot">Bandera para distinguir desde que método se hace referencia.</param>
    /// <history>
    ///   [edgrodriguez] 24/03/2016  Created.
    /// [erosado] 01/04/2016 Modified Se cambiaron valores estaticos para acomodar el contenido del reporte.
    /// </history>
    private static void SetFormatColumns(List<ExcelFormatTable> formatColumns, ref ExcelWorksheet wsData, int totalRowsFilter, int dtRowsNumber, int dtColumnsNumber, bool isPivot)
    {
      //Agregamos los Headers de la Tabla
      int contColum = 0;
      foreach (ExcelFormatTable item in formatColumns)
      {
        contColum++;
        if (!isPivot)
          wsData.Cells[totalRowsFilter + 4, contColum].Value = item.Title;

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
            if (!isPivot)
              wsData.Cells[totalRowsFilter + 5, contColum, totalRowsFilter + 5 + dtRowsNumber, contColum].Style.Font.Name = "Wingdings"; //Posicion de los headers de la tabla
            else
              wsData.Cells[2, contColum, dtRowsNumber + 1, contColum].Style.Font.Name = "Wingdings";
            break;

          case EnumFormatTypeExcel.Date:
            wsData.Column(contColum).Style.Numberformat.Format = "dd/MM/yyyy";
            break;

          default:
            break;
        }
        wsData.Column(contColum).Style.HorizontalAlignment = item.Alignment;

        if (!isPivot)
        {
          wsData.Cells[totalRowsFilter + 4, contColum].Style.Font.Bold = true;
          wsData.Cells[totalRowsFilter + 4, contColum].Style.Font.Size = 14;
        }
      }
      if (!isPivot)
      {
        wsData.Cells[totalRowsFilter + 4, 1, totalRowsFilter + 4, dtColumnsNumber].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Hair);
      }
      else
        wsData.Cells[1, 1, dtRowsNumber + 1, dtColumnsNumber].AutoFitColumns();
    }

    #endregion SetFormatColumns

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
          format = "0.0 %";
          break;

        case EnumFormatTypeExcel.Currency:
          format = "$#,##0.00";
          break;

        case EnumFormatTypeExcel.Number:
          format = "#";
          break;

        case EnumFormatTypeExcel.DecimalNumber:
          format = "0.00";
          break;

        case EnumFormatTypeExcel.Date:
          format = "dd/MM/yyyy";
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
        pivotTable.WorkSheet.Workbook.CodeModule.Code = "Private Sub Workbook_SheetPivotTableChangeSync(ByVal Sh As Object, ByVal Target As PivotTable)\r\nEnd Sub";
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
      sb.AppendLine("Set PvtTbl = Worksheets(\"" + pivotTable.WorkSheet.Name + "\").PivotTables(\"" + pivotTable.Name + "\")");
      sb.AppendLine("Worksheets(\"" + pivotTable.WorkSheet.Name + "\").Activate");

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
      sb.AppendLine("End Sub");
      sb.AppendLine("");
      module.Code += sb.ToString();
    }

    #endregion AddPivotBorderRange

    #endregion Private Methods
  }
}