using IM.Model.Classes;
using IM.Model.Enums;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;

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
      #endregion

      #region Report Header
      //Creamos la cabecera del reporte (Titulos, Filtros, Fecha y Hora de Impresion)
      CreateReportHeader(filter, reportName, ref ws, ref FilasTotalesFiltros);
      #endregion Filtros

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
      ws.Cells[2, 1, FilasTotalesFiltros, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
      ws.Cells[2, 1, FilasTotalesFiltros, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

      string suggestedFilaName = string.Concat(reportName, " ", dateRangeFileName);

      #endregion

      #region Generamos y Retornamos la ruta del archivo EXCEL

      pathFinalFile = SaveExcel(pk, suggestedFilaName);

      if (pathFinalFile != null)
      {
        return pathFinalFile;
      }
      return null;

      #endregion Generamos y Retornamos la ruta del archivo EXCEL
    }

    #endregion

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
    /// </history>
    public static FileInfo CreatePivotRptExcel(bool isPivot, List<Tuple<string, string>> filters, DataTable dtData,
      Tuple<string, string> reportName, List<string> pivotColums, List<string> pivotRows, List<string> pivotValue,
      List<ExcelFormatTable> formatColumns, int pivotColumnsCount = 0, List<string> pivotValueCalculated = null)
    {
      FileInfo pathFinalFile = null;
      ExcelPackage pk = new ExcelPackage();
      //Preparamos la hoja donde escribiremos
      //la tabla dinamica.
      ExcelWorksheet wsPivot = pk.Workbook.Worksheets.Add(reportName.Item1);
      ExcelWorksheet wsData = pk.Workbook.Worksheets.Add("Hoja0");
      wsData.Hidden = eWorkSheetHidden.Hidden;

      //Numero de filtros
      int filterNumber = filters.Count + 2;

      //Numero de filas del contenido del datatable
      int dtRowsNumber = dtData.Rows.Count;
      //Numero de Columnas del contenido del datatable
      int dtColumnsNumber = pivotColumnsCount == 0 ? dtData.Columns.Count : pivotColumnsCount;
      int cFiltros = 1;
      if (filters.Count > 0)
      {
        //Insertamos etiqueta de filtros
        wsPivot.Cells[1, 1].Value = "Filters";
        wsPivot.Cells[1, 1].Style.Font.Bold = true;
        wsPivot.Cells[1, 1].Style.Font.Size = 14;

        //Insertamos las filas que necesitamos para los filtros
        wsPivot.InsertRow(2, filters.Count + 3);

        foreach (var item in filters)
        {
          //Insertamos los filtros.
          cFiltros++;
          wsPivot.Cells[string.Concat("A", cFiltros)].Value = item.Item1;
          wsPivot.Cells[string.Concat("A", cFiltros)].Style.Font.Bold = true;
          wsPivot.Cells[string.Concat("B", cFiltros)].Value = item.Item2;
        }
      }
      else
        //Si no se enviaron filtros. Se insertan 2 filas en blanco.
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
      wsPivot.Cells[1, dtColumnsNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

      //Nombre del sistema.
      wsPivot.Cells[2, dtColumnsNumber].Value = "Intelligence Marketing";
      wsPivot.Cells[2, dtColumnsNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

      //Filtros de Fecha
      wsPivot.Cells[3, dtColumnsNumber].Value = reportName.Item2;
      wsPivot.Cells[3, dtColumnsNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

      //Agregamos el contenido
      wsPivot.InsertRow(filterNumber + 2, dtRowsNumber * 2);

      //Renombramos las columnas.
      dtData.Columns.Cast<DataColumn>().ToList().ForEach(c =>
      {
        c.ColumnName = formatColumns[dtData.Columns.IndexOf(c)].Title;
      });

      //Formateamos los campos.
      SetFormatColumns(formatColumns, ref wsData, filterNumber, dtRowsNumber, dtColumnsNumber, true);

      //Cargamos los datos en la hoja0.
      wsData.Cells["A1"].LoadFromDataTable(dtData, true, TableStyles.None);

      //Cargamos la tabla dinamica.
      var pivotTable = wsPivot.PivotTables.Add(wsPivot.Cells[filterNumber + 5, 1],
        wsData.Cells[1, 1, wsData.Dimension.End.Row, wsData.Dimension.End.Column], reportName.Item1);

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
        pivotTable.RowGrandTotals = pivotValue.Count > 0;

        pivotTable.ShowDrill = false;
        pivotTable.EnableDrill = false;
        pivotTable.ColumGrandTotals = pivotValue.Count > 0;
        pivotTable.MultipleFieldFilters = true;
        pivotTable.GridDropZones = false;
        pivotTable.TableStyle = TableStyles.Medium13;

        #endregion Formato Tabular
      }

      //Asignamos las columnas para realizar el pivote
      pivotColums.ForEach(col => { pivotTable.ColumnFields.Add(pivotTable.Fields[col]); });

      //Asignamos las filas que se mostraran.
      pivotRows.ForEach(row =>
      {
        //Lo Agregamos a la lista de Filas.
        ExcelPivotTableField ptfField = pivotTable.RowFields.Add(pivotTable.Fields[row]);

        if (!isPivot) //Si se va manejar el formato tabular.
        {
          ExcelFormatTable rowFormat = formatColumns.Single(c => c.Title == row);
          ptfField.Compact = rowFormat.Compact;
          ptfField.SubtotalTop = rowFormat.SubtotalTop;
          ptfField.Outline = rowFormat.Outline;
          ptfField.SubTotalFunctions = rowFormat.SubTotalFunctions;
          ptfField.ShowAll = false;
        }
      });

      //Asignamos el valor que se mostrara en las columnas.
      pivotValue.ForEach(value =>
      {
        ExcelPivotTableDataField valueField = pivotTable.DataFields.Add(pivotTable.Fields[value]);

        if (!isPivot)
        {
          ExcelFormatTable valueFormat = formatColumns.Single(c => c.Title == value);
          valueField.Name = valueFormat.Title;
          valueField.Format = GetFormat(valueFormat);
          valueField.Function = valueFormat.Function;
          valueField.BaseField = 0;
          valueField.BaseItem = 0;
        }
      });

      //Agregamos valores calculados
      pivotValueCalculated?.ForEach(valueCalculated =>
      {
        if (!isPivot)
        {
          ExcelFormatTable valueFormat = formatColumns.Single(c => c.Title == valueCalculated);
          pivotTable.AddCalculatedField(valueFormat);
        }
      });

      string suggestedFilaName = string.Concat(reportName.Item1, "_", DateTime.Now.ToString("yyyymmddhhmmss"));
      try
      {
        pathFinalFile = SaveExcel(pk, suggestedFilaName);
      }
      catch (Exception ex)
      {
      }

      return pathFinalFile;
    }

    #endregion

    #endregion

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
              wsData.Cells[totalRowsFilter + 5, contColum, totalRowsFilter +5 + dtRowsNumber, contColum].Style.Font.Name = "Wingdings"; //Posicion de los headers de la tabla
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
    #endregion

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
      ws.Cells[1, 4, 1, 9].Merge = true;
      ws.Cells[1, 4, 1, 9].Value = reportName;
      ws.Cells[1, 4, 1, 9].Style.Font.Bold = true;
      ws.Cells[1, 4, 1, 9].Style.Font.Size = 14;
      //Agregamos la linea Doble bajo los nombres de la aplicacion y del reporte
      ws.Cells[1, 1, 1, 9].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;//Doble Linea

      #endregion

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
      #endregion

      #region Datos de la impresion
      //Agregamos la etiqueta 
      ws.Cells[totalFilterRows + 2, 1].Value = "Print Date Time";
      ws.Cells[totalFilterRows + 2, 1].Style.Font.Bold = true;

      //Agregamos el valor de fecha y hora de impresion
      ws.Cells[totalFilterRows + 2, 2].Value = string.Format("{0:MM/dd/yyyy hh:mm:ss}", DateTime.Now);
      #endregion
    }

    #endregion

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

    #endregion

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

    #endregion
  }
}