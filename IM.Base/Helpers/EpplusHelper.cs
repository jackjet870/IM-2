using IM.Model;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Threading.Tasks;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;

namespace IM.Base.Helpers
{
  public static class EpplusHelper
  {
    private static char separator = '|';
    
    #region Public Methods
    //Metodos Propios de IM

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
      ReportBuilder.CreateReportHeader(filter, reportName, ref ws, ref filasTotalesFiltros, null, 0);

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
      ReportBuilder.SetFormatTable(tupleGraph1.Item3, ref table);
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
      ReportBuilder.SetFormatTable(tupleGraph2.Item3, ref table);
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
        pathFinalFile = ReportBuilder.SaveExcel(pk, suggestedName: suggestedFilaName);
      }
      else
        pathFinalFile = ReportBuilder.SaveExcel(pk, filePath: fileFullPath);

      return pathFinalFile;

      #endregion Generamos y Retornamos la ruta del archivo EXCEL
    }

    #endregion CreateGraphExcel

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
    public static FileInfo ExportRptWeeklyMonthlyHostess(List<Tuple<DataTable, ExcelFormatItemsList, string>> Data, List<Tuple<string, string>> filters,
      string reportName, string dateRangeFileName, bool blnColumnGrandTotal = false,
      bool blnRowGrandTotal = false, bool blnShowSubtotal = false, string fileFullPath = null)
    {
      FileInfo pathFinalFile;
      using (var pk = new ExcelPackage())
      {
        var wsData = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
        var columnNumber = 1;
        var totalFilterRows = 0;
        var initialColumn = 0;
        //Creamos el encabezado del reporte. Filtros, Titulo.
        ReportBuilder.CreateReportHeader(filters, reportName, ref wsData, ref totalFilterRows);

        foreach (var pair in Data)
        {
          if (pair.Item1.Rows.Count > 0)
          {
            //Obtenemos la fila inicial. Para dibujar la tabla.
            var rowNumber = totalFilterRows + 1 + pair.Item2.Count(c => c.Axis == ePivotFieldAxis.Column);

            //Obtenemos la tabla ya con las columnas pivote.
            var dtTableAux = ReportBuilder.GetPivotTable(pair.Item2, pair.Item1);

            var formatTableColumns = new List<ExcelFormatTable>();

            //Generamos la nueva lista de formatos
            dtTableAux.Columns.Cast<DataColumn>().ToList().ForEach(col =>
            {
              var header = col.ColumnName.Split(separator);
              var format = pair.Item2.FirstOrDefault(ft => ft.PropertyName == ((header.Length == 1) ? header[0] : header[header.Length - 1]));
              if (format == null) return;

              string formulaPivot = format.Formula;
              if (header.Length > 1 && !string.IsNullOrEmpty(format.Formula))
              {
                var columns = Regex.Matches(format.Formula, @"(\[.*?\])+");
                foreach (var match in columns)
                {
                  formulaPivot = formulaPivot.Replace(match.ToString(), $"[{string.Join(separator.ToString(), header.Take(header.Length - 1))}{separator.ToString()}{match.ToString().Replace("[", "").Replace("]", "")}]");
                }
              }

              formatTableColumns.Add(new ExcelFormatTable
              {
                Title = format.Title,
                PropertyName = col.ColumnName,
                Alignment = format.Alignment,
                Format = format.Format,
                Axis = format.Axis,
                IsVisible = format.IsVisible,
                IsGroup = format.IsGroup,
                Function = format.Function,
                IsCalculated = format.IsCalculated,
                Formula = (string.IsNullOrEmpty(formulaPivot)) ? format.Formula : formulaPivot,
                Sort = format.Sort,
                SubtotalWithCero = format.SubtotalWithCero,
                SuperHeader = format.SuperHeader,
              });
            });

            formatTableColumns.ForEach(format =>
            {
              if (!format.IsVisible && !format.IsGroup)
              {
                //Eliminamos el campo del datatable.
                dtTableAux.Columns.Remove(format.PropertyName);
              }
            });
            var colsNames = dtTableAux.Columns.OfType<DataColumn>().Select(c => c.ColumnName).ToList();
            formatTableColumns = formatTableColumns.Where(c => colsNames.Contains(c.PropertyName)).ToList();

            #region Creando Headers

            //Obtenemos los encabezados de la tabla pivote.
            var lstHeaders = dtTableAux.Columns.OfType<DataColumn>().Select(c => c.ColumnName.Split(separator)).ToList();
            //Obtenemos la cantidad maxima de superheaders.
            var iniValue = new int[dtTableAux.Columns.OfType<DataColumn>().Max(c => c.ColumnName.Split(separator).Length - 1)];
            //Recorremos los encabezados.
            foreach (var item in lstHeaders)
            {
              //Si solo hay un encabezado.
              if (item.Length == 1)
              {
                //Si no es un grupo.
                if ((!pair.Item2.First(c => c.PropertyName == item.First()).IsGroup && !pair.Item2.First(c => c.PropertyName == item.First()).IsVisible) || !pair.Item2.First(c => c.PropertyName == item.First()).IsVisible) continue;
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
                        range.Value = pair.Item2.First(c => c.PropertyName == item[i]).Title;
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
                        range.Value = pair.Item2.First(c => c.PropertyName == item[i]).Title;
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

            if (formatTableColumns.Any(c => c.IsGroup))
            {
              #region Simple con Agrupado

              #region Formato para encabezados de grupo
              rowNumber++;
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
              int formatIndex = 1;
              //Creamos la sentencia Linq para obtener los campos que se agruparán.
              var qfields = string.Join(", ", formatTableColumns
          .Where(c => c.IsGroup)
          .Select(x => "it[\"" + x.PropertyName + "\"] as " + x.PropertyName));

              //Obtenemos las agrupaciones y los registros de cada agrupacion.
              var qTable = dtTableAux
                .AsEnumerable()
                .AsQueryable()
                .GroupBy("new(" + qfields + ")", "it")
                .Select("new(Key as qgroup, it as Values)");

              #endregion

              #region Encabezados de grupo, Insertamos los datos y calculamos los subtotales.
              #region Dibujamos los encabezados
              //Lista de formulas para cada grupo. Teniendo como items las columnas que tienen la propiedad SubtotalFunction.
              var subtotalFormulas = new Dictionary<string, string>[formatTableColumns.Count(c => c.IsGroup)];
              //Lista de grupos.       
              var dynamicListData = qTable.OfType<dynamic>().ToList();
              //Total de columnas que no son grupo.
              var totalColumns = formatTableColumns.Count(c => c.IsVisible);
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
                    wsData.Cells[rowNumber, initialColumn + 1].Value = groupsAct[j];
                    using (var range = wsData.Cells[rowNumber, initialColumn + 1, rowNumber, initialColumn + totalColumns])
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
                      wsData.Cells[rowNumber, initialColumn + 1].Value = groupsAct[j];
                      using (var range = wsData.Cells[rowNumber, initialColumn + 1, rowNumber, initialColumn + totalColumns])
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
                #endregion

                #region Ingresamos los valores de las columnas
                //Obtenemos los datos.
                var dataValues = ((IEnumerable<DataRow>)dynamicListData[i].Values).CopyToDataTable();
                var camposGrupo = formatTableColumns.Where(col => col.IsGroup).Select(col => col.PropertyName).ToList();
                //Eliminamos las columnas que fueron configuradas como grupo y son  No visibles. Y obtenemos el formato de las columnas que se visualizaran en el reporte.
                formatIndex = 1;
                dataValues.Columns.OfType<DataColumn>().ToList().ForEach(c =>
                {
                  var format = formatTableColumns.FirstOrDefault(f => f.PropertyName == c.ColumnName);
                  if (format == null) return;
                  //Si el campo se encuentra en la lista de Grupos y es No Visible o el campo es no Visible
                  if ((camposGrupo.Contains(c.ColumnName) && !format.IsVisible) || !format.IsVisible)
                  {
                    //Lo eliminamos del datatatable.
                    dataValues.Columns.Remove(c);
                  }
                  else
                  {
                    //Aplicamos el formato al campo.
                    using (var range = wsData.Cells[rowNumber, initialColumn + formatIndex, rowNumber + dataValues.Rows.Count, initialColumn + formatIndex])
                    {
                      range.Style.Numberformat.Format = ReportBuilder.GetFormat(format.Format);
                      formatIndex++;
                    }
                  }
                });


                //Agregamos los datos al excel.
                using (var range = wsData.Cells[rowNumber, initialColumn + 1].LoadFromDataTable(dataValues, false))
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
                #endregion

                #region Agregamos los subtotales de cada grupo.
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
                      formatIndex = 1;
                      foreach (var format in formatTableColumns)
                      {
                        //Si es una columna No Visible continuamos el ciclo.
                        if (!format.IsVisible) continue;
                        using (var range = wsData.Cells[rowNumber, initialColumn + formatIndex])
                        {
                          if (format.Function == DataFieldFunctions.None && string.IsNullOrWhiteSpace(format.Formula)) { formatIndex++; continue; }

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
                          range.Style.Numberformat.Format = ReportBuilder.GetFormat(subtotalFormat);

                          //Si no es calculada aplicamos la funcion configurada.
                          if (!format.IsCalculated)
                          {
                            var formula = "";
                            //Si es el ultimo nivel
                            if (j == groupsAct.Length - 1)
                            {
                              formula = wsData.Cells[dataIniRow, initialColumn + formatIndex, rowNumber - 1, initialColumn + formatIndex].Address;//Aplicamos la seleccion segun la cantidad de registros que tenga el grupo.
                              if (!string.IsNullOrEmpty(format.Formula))
                              {
                                using (var range2 = wsData.Cells[rowNumber + 1, initialColumn + formatIndex])
                                {
                                  range2.Formula = ReportBuilder.GetFormula(formatTableColumns, format.Formula, rowNumber, true, initialColumn);
                                  range2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                  range2.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                                  range2.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                                  range2.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                                  range2.Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
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
                            switch (format.Function)
                            {
                              case DataFieldFunctions.Sum:
                                range.Formula = "=SUM(" + formula + ")";
                                break;
                              case DataFieldFunctions.Average:
                                range.Formula = "=AVERAGE(" + formula + ")";
                                break;
                              case DataFieldFunctions.Count:
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
                            range.Formula = ReportBuilder.GetFormula(formatTableColumns, format.Formula, rowNumber, true, initialColumn);
                        }
                        formatIndex++;
                      }


                      var firstSubtotalColumn = formatTableColumns.FindIndex(c => c.Function != DataFieldFunctions.None || !string.IsNullOrWhiteSpace(c.Formula));
                      using (var range = wsData.Cells[rowNumber, initialColumn + firstSubtotalColumn, rowNumber, initialColumn + totalColumns])
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
                #endregion
              }
              #endregion

              if (blnRowGrandTotal)
              {
                formatIndex = 1;
                formatTableColumns.ForEach(format =>
                {
                  //Si es una columna No Visible continuamos el ciclo.
                  if (!format.IsVisible) return;
                  using (var range = wsData.Cells[rowNumber - 1, initialColumn + formatIndex])
                  {
                    if (format.Function == DataFieldFunctions.None && string.IsNullOrWhiteSpace(format.Formula)) { formatIndex++; return; }
                    if (!format.IsCalculated)
                    {
                      switch (format.Function)
                      {
                        case DataFieldFunctions.Sum:
                          range.Formula = "=SUM(" + subtotalFormulas[0][format.PropertyName] + ")";
                          break;
                        case DataFieldFunctions.Average:
                          range.Formula = "=AVERAGE(" + subtotalFormulas[0][format.PropertyName] + ")";
                          break;
                        case DataFieldFunctions.Count:
                          if (format.Format == EnumFormatTypeExcel.General)
                            range.Formula = "= SUM(" + subtotalFormulas[0][format.PropertyName] + ")";
                          break;
                      }

                      if (!string.IsNullOrEmpty(format.Formula))
                      {
                        using (var range2 = wsData.Cells[rowNumber, initialColumn + formatIndex])
                        {
                          range2.Formula = ReportBuilder.GetFormula(formatTableColumns, format.Formula, rowNumber - 1, true, initialColumn);
                          range2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                          range2.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].BackGroundColor));
                          range2.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].FontColor));
                          range2.Style.Font.Bold = backgroundColorGroups[backgroundColorGroups.Count - 1].FontBold;
                          range2.Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                        }
                      }
                    }
                    else
                      range.Formula = ReportBuilder.GetFormula(formatTableColumns, format.Formula, rowNumber - 1, true, initialColumn);

                    range.Style.Numberformat.Format = ReportBuilder.GetFormat(format.Format);
                  }
                  formatIndex++;
                });
                using (var range = wsData.Cells[rowNumber - 1, initialColumn + 1, rowNumber - 1, initialColumn + totalColumns])
                {
                  range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                  range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].BackGroundColor));
                  range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].FontColor));
                  range.Style.Font.Bold = backgroundColorGroups[backgroundColorGroups.Count - 1].FontBold;
                }
              }
              #endregion Simple con Agrupado          
            }
            else
            {
              #region Agregando Datos

              //Eliminamos las columnas que fueron configuradas como No Visibles. Y obtenemos el formato de las columnas que se visualizaran en el reporte.
              dtTableAux.Columns.OfType<DataColumn>().ToList().ForEach(c =>
              {
                var format = formatTableColumns.FirstOrDefault(f => f.PropertyName == c.ColumnName);
                if (format == null) return;
                if (!format.IsGroup && !format.IsVisible)
                  dtTableAux.Columns.Remove(c);
              });

              rowNumber++;
              var columnIndex = 1;
              var rowEnd = rowNumber + dtTableAux.Rows.Count;
              dtTableAux.Columns.Cast<DataColumn>().ToList().ForEach(col =>
              {
                var columnN = col.ColumnName.Split(separator);
                if (columnN.Length == 1)
                {
                  var format = ReportBuilder.GetFormat(pair.Item2.First(c => c.PropertyName == columnN[0]).Format);
                  if (format != "")
                  {
                    using (var range = wsData.Cells[rowNumber, initialColumn + columnIndex, rowEnd, initialColumn + columnIndex])
                    {
                      range.Style.Numberformat.Format = format;
                      range.Style.Font.Size = 9;
                    }
                  }
                }
                else
                {
                  var format = ReportBuilder.GetFormat(pair.Item2.First(c => c.PropertyName == columnN[columnN.Length - 1]).Format);
                  if (format != "")
                  {
                    using (var range = wsData.Cells[rowNumber, initialColumn + columnIndex, rowEnd, initialColumn + columnIndex])
                    {
                      range.Style.Numberformat.Format = format;
                      range.Style.Font.Size = 9;
                    }
                  }
                }
                columnIndex++;
              });

              //El contenido lo convertimos a una tabla
              wsData.Cells[rowNumber, initialColumn + 1].LoadFromDataTable(dtTableAux, false);
              rowNumber += dtTableAux.Rows.Count;

              if (blnRowGrandTotal)
              {
                columnIndex = 1;
                dtTableAux.Columns.Cast<DataColumn>().ToList().ForEach(col =>
                {
                  var columnN = col.ColumnName.Split(separator);
                  var colN = (columnN.Length == 1) ? columnN[0] : columnN[columnN.Length - 1];

                  var formatCol = pair.Item2.First(ft => ft.PropertyName == colN);
                  //Si es una columna No Visible continuamos el ciclo.
                  if (!formatCol.IsVisible) return;
                  if (formatCol.Function == DataFieldFunctions.None && string.IsNullOrWhiteSpace(formatCol.Formula)) { columnIndex++; return; }
                  if (!formatCol.IsCalculated)
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
                    using (var range = wsData.Cells[rowNumber, initialColumn + columnIndex])
                    {
                      range.Style.Numberformat.Format = ReportBuilder.GetFormat(subtotalFormat);
                      switch (formatCol.Function)
                      {
                        case DataFieldFunctions.Sum:
                          range.Formula = "=SUM(" + wsData.Cells[rowNumber - dtTableAux.Rows.Count, initialColumn + columnIndex, rowNumber - 1, initialColumn + columnIndex].Address + ")";
                          break;
                        case DataFieldFunctions.Average:
                          range.Formula = "=AVERAGE(" + wsData.Cells[rowNumber - dtTableAux.Rows.Count, initialColumn + columnIndex, rowNumber - 1, initialColumn + columnIndex].Address + ")";
                          break;
                        case DataFieldFunctions.Count:
                          if (formatCol.Format == EnumFormatTypeExcel.General)
                            range.Formula = "=COUNTA(" + wsData.Cells[rowNumber - dtTableAux.Rows.Count, initialColumn + columnIndex, rowNumber - 1, initialColumn + columnIndex].Address + ")";
                          break;
                      }
                    }
                    columnIndex++;
                  }
                });
                using (var range = wsData.Cells[rowNumber, initialColumn + 1, rowNumber, initialColumn + formatTableColumns.Count(c => c.IsVisible)])
                {
                  range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                  range.Style.Fill.BackgroundColor.SetColor(Color.Black);
                  range.Style.Font.Bold = true;
                  range.Style.Font.Color.SetColor(Color.White);
                }
                rowNumber++;
              }

              #endregion Agregando Datos
            }

            ReportBuilder.AutoFitColumns(ref wsData);

            initialColumn = formatTableColumns.Count(c => c.IsVisible) + 1;
            columnNumber++;
          }
        }

        if (fileFullPath == null)
        {
          var suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);
          pathFinalFile = ReportBuilder.SaveExcel(pk, suggestedName: suggestedFilaName); 
        }
        else
          pathFinalFile = ReportBuilder.SaveExcel(pk,filePath:fileFullPath);
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
    public static async Task<FileInfo> ExportRptManifestRangeByLs(List<Tuple<DataTable, ExcelFormatItemsList>> Data, List<Tuple<string, string>> filters,
      string reportName, string dateRangeFileName, bool blnColumnGrandTotal = false,
      bool blnRowGrandTotal = false, bool blnShowSubtotal = false, string fileFullPath = null)
    {
      return await Task.Run(() =>
      {
        FileInfo pathFinalFile;
        using (var pk = new ExcelPackage())
        {

          var wsData = pk.Workbook.Worksheets.Add(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "));
          wsData.Protection.IsProtected = true;
          var totalFilterRows = 0;
          var dtTable = Data[0].Item1;
          var formatTable = Data[0].Item2;
          var dtTableBookings = Data[1].Item1.Rows.Count > 0 ? ReportBuilder.GetPivotTable(Data[1].Item2, Data[1].Item1) : Data[1].Item1;
          var formatBookings = Data[1].Item2;
          var formatIndex = 1;
          //Creamos el encabezado
          ReportBuilder.CreateReportHeader(filters, reportName, ref wsData, ref totalFilterRows);

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
          var formatTableColumns = new List<ExcelFormatTable>();

          #region Creando Headers
          //Agregando los headers de acuerdo al orden de la lista.
          //descartando los campos configurados como grupos.
          formatIndex = 0;
          formatTable.ForEach(format =>
          {
            //Si el campo es Visibile
            if (format.IsVisible)
            {
              //Modificamos la posicion del campo en el datatable.
              dtTable.Columns[format.PropertyName].SetOrdinal(formatIndex);
              //Dibujamos el encabezado.
              using (var range = wsData.Cells[rowNumber, formatIndex + 1])
              {
                range.Value = format.Title;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 12;
                range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
                formatIndex++;
              }
            }
            //Si el formato es nulo o el formato es No Visible y no es un grupo.
            else if (!format.IsVisible && !format.IsGroup)
            {
              //Eliminamos el campo del datatable.
              dtTable.Columns.Remove(format.PropertyName);
            }
            //Modificamos la posicion del campo al final del datatable.
            else
              dtTable.Columns[format.PropertyName].SetOrdinal(dtTable.Columns.Count - 1);
          });
          rowNumber++;
          var colsNames = dtTable.Columns.OfType<DataColumn>().Select(c => c.ColumnName).ToList();
          formatTableColumns = formatTable.Where(c => colsNames.Contains(c.PropertyName)).ToList();

          #endregion

          //Si existe algun grupo
          if (formatTableColumns.Any(c => c.IsGroup))
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
            var qfields = string.Join(", ", formatTableColumns
        .Where(c => c.IsGroup).Select(x => "it[\"" + x.PropertyName + "\"] as " + x.PropertyName));

            //Obtenemos las agrupaciones y los registros de cada agrupacion.
            var qTable = dtTable
              .AsEnumerable()
              .AsQueryable()
              .GroupBy("new(" + qfields + ")", "it")
              .Select("new(Key as qgroup, it as Values)");

            #endregion

            //Lista de formulas para cada grupo. Teniendo como items las columnas que tienen la propiedad SubtotalFunction.
            var subtotalFormulas = new Dictionary<string, string>[formatTableColumns.Count(c => c.IsGroup && c.PropertyName != "ShowProgramN")];
            //Lista de grupos.       
            var dynamicListData = qTable.OfType<dynamic>().ToList();
            //Total de columnas que no son grupo.
            var totalColumns = formatTableColumns.Count(c => c.IsVisible);
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
                for (var j = 0; j < groupsAct.Length - 1; j++)
                {
                  if (j == groupsAct.Length - 2
                    && (groupsAct.Contains("MANIFEST")
                    || groupsAct.Contains("COURTESY TOUR")
                    || groupsAct.Contains("SAVE TOUR"))
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

                    wsData.Cells[rowNumber + 1, 1].Value = groupsAct[groupsAct.Length - 1];
                    using (var range = wsData.Cells[rowNumber + 1, 1, rowNumber + 1, totalColumns])
                    {
                      range.Merge = true;
                      range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                      range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j + 1].BackGroundColor));
                      range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j + 1].FontColor));
                      range.Style.Font.Bold = backgroundColorGroups[j + 1].FontBold;
                      range.Style.HorizontalAlignment = backgroundColorGroups[j + 1].TextAligment;
                    }
                    rowNumber++;
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
                    if (j == groupsAct.Length - 2)
                    {
                      wsData.Cells[rowNumber + 1, 1].Value = groupsAct[groupsAct.Length - 1];
                      using (var range = wsData.Cells[rowNumber + 1, 1, rowNumber + 1, totalColumns])
                      {
                        range.Merge = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j + 1].BackGroundColor));
                        range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j + 1].FontColor));
                        range.Style.Font.Bold = backgroundColorGroups[j + 1].FontBold;
                        range.Style.HorizontalAlignment = backgroundColorGroups[j + 1].TextAligment;
                      }
                      rowNumber++;
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
                for (var j = 0; j < groupsAct.Length - 1; j++)
                {
                  if (groupsAct[j] == previousGroup[j]) continue;
                  //Si el nivel actual es diferente al valor anterior.
                  if (groupsAct[j] != previousGroup[j])
                  {
                    if (j == groupsAct.Length - 2
                       && (groupsAct.Contains("MANIFEST")
                    || groupsAct.Contains("COURTESY TOUR")
                    || groupsAct.Contains("SAVE TOUR"))
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

                      wsData.Cells[rowNumber + 1, 1].Value = groupsAct[groupsAct.Length - 1];
                      using (var range = wsData.Cells[rowNumber + 1, 1, rowNumber + 1, totalColumns])
                      {
                        range.Merge = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j + 1].BackGroundColor));
                        range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j + 1].FontColor));
                        range.Style.Font.Bold = backgroundColorGroups[j + 1].FontBold;
                        range.Style.HorizontalAlignment = backgroundColorGroups[j + 1].TextAligment;
                      }
                      rowNumber++;
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

              #region Ingresamos los valores de las columnas
              //Obtenemos los datos.
              var dataValues = ((IEnumerable<DataRow>)dynamicListData[i].Values).CopyToDataTable();
              var camposGrupo = formatTableColumns.Where(col => col.IsGroup).Select(col => col.PropertyName).ToList();
              //Eliminamos las columnas que fueron configuradas como grupo y son  No visibles. Y obtenemos el formato de las columnas que se visualizaran en el reporte.
              formatIndex = 1;
              dataValues.Columns.OfType<DataColumn>().ToList().ForEach(c =>
              {
                var format = formatTableColumns.FirstOrDefault(f => f.PropertyName == c.ColumnName);
                if (format == null) return;
                //Si el campo se encuentra en la lista de Grupos y es No Visible o el campo es no Visible
                if ((camposGrupo.Contains(c.ColumnName) && !format.IsVisible) || !format.IsVisible)
                {
                  //Lo eliminamos del datatatable.
                  dataValues.Columns.Remove(c);
                }
                else
                {
                  //Aplicamos el formato al campo.
                  using (var range = wsData.Cells[rowNumber, formatIndex, rowNumber + dataValues.Rows.Count, formatIndex])
                  {
                    range.Style.Numberformat.Format = ReportBuilder.GetFormat(format.Format);
                    formatIndex++;
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
              #endregion

              //Si se mostrará el subtotal de cada grupo.
              if (blnShowSubtotal)
              {
                //Obtenemos la fila inicial.
                var dataIniRow = rowNumber - dataValues.Rows.Count;
                var countGroup = formatTableColumns.Count(c => c.IsGroup && !c.IsVisible);
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
                for (var j = groupsAct.Length - 2; j >= 1; j--)
                {
                  var Tours = 0;
                  var Shows = "";
                  var RealShows = "";
                  var Bookings = "";
                  var resch = "";
                  var direct = "";
                  var inOut = "";
                  var procGross = "";
                  var procSales = "";
                  var eff = "";
                  var closingFactor = "";

                  //Si los valores del index actual de cada lista son diferentes o el valor del index de la siguiente lista esta vacia o nula.
                  if (groupsAct[j] != nextGroup[j] || string.IsNullOrEmpty(nextGroup[j]) || j == groupsAct.Length - 2)
                  {
                    formatIndex = 1;
                    //Recorremos las columnas.
                    foreach (var format in formatTableColumns)
                    {
                      //Si es una columna No Visible continuamos el ciclo.
                      if (!format.IsVisible) continue;
                      using (var range = wsData.Cells[rowNumber, formatIndex])
                      {
                        if (format.Function == DataFieldFunctions.None && string.IsNullOrWhiteSpace(format.Formula)) { formatIndex++; continue; }

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
                        range.Style.Numberformat.Format = ReportBuilder.GetFormat(subtotalFormat);

                        //Si no es calculada aplicamos la funcion configurada.
                        if (!format.IsCalculated)
                        {
                          var formula = "";
                          //Si es el ultimo nivel
                          if (j == groupsAct.Length - 2)
                            formula = wsData.Cells[dataIniRow, formatIndex, rowNumber - 1, formatIndex].Address;//Aplicamos la seleccion segun la cantidad de registros que tenga el grupo.
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
                              if (j < groupsAct.Length - 2 && (groupsAct[j] != nextGroup[j] || string.IsNullOrEmpty(nextGroup[j])))
                              {
                                //Limpiamos la formula.
                                subtotalFormulas[index][format.PropertyName] = string.Empty;
                              }
                            }
                          }
                          switch (format.Function)
                          {
                            case DataFieldFunctions.Sum:
                              range.Formula = $"=SUM({formula})";
                              break;
                            case DataFieldFunctions.Average:
                              range.Formula = $"=AVERAGE({formula})";
                              break;
                            case DataFieldFunctions.Count:
                              if (format.Format == EnumFormatTypeExcel.General || format.Format == EnumFormatTypeExcel.Boolean)
                                range.Formula = (j == groupsAct.Length - 2) ? $"= COUNTA({ formula})" : $"= SUM({formula})";
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
                          range.Formula = ReportBuilder.GetFormula(formatTableColumns, format.Formula, rowNumber);
                      }
                      formatIndex++;
                    }
                    var firstSubtotalColumn = formatTableColumns.FindIndex(c => c.Function != DataFieldFunctions.None || !string.IsNullOrWhiteSpace(c.Formula)) - countGroup + 1;
                    using (var range = wsData.Cells[rowNumber, firstSubtotalColumn, rowNumber, dataValues.Columns.Count])
                    {
                      range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                      range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].BackGroundColor));
                      range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[j].FontColor));
                      range.Style.Font.Bold = backgroundColorGroups[j].FontBold;
                    }

                    if (j == groupsAct.Length - 2)
                    {
                      rowNumber++;
                      //Calculamos los factores
                      Tours = dataValues.AsEnumerable().Count(c => (c["Tour"] != DBNull.Value && !string.IsNullOrWhiteSpace(c["Tour"].ToString())) || (c["WO"] != DBNull.Value && !string.IsNullOrWhiteSpace(c["WO"].ToString())) || ((c["CTour"] != DBNull.Value && !string.IsNullOrWhiteSpace(c["CTour"].ToString())) || (c["STour"] != DBNull.Value && !string.IsNullOrWhiteSpace(c["STour"].ToString())) && (c["ProcGross"] != DBNull.Value && Convert.ToDecimal(c["ProcGross"]) > 0)));
                      Shows = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "Show") - countGroup + 1].Address;
                      RealShows = $"SUM({wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "Tour") - countGroup + 1].Address},{wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "IO") - countGroup + 1].Address},{wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "WO") - countGroup + 1].Address},{wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "CTour") - countGroup + 1].Address},{wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "STour") - countGroup + 1].Address})";
                      Bookings = wsData.Cells[dataIniRow - 2, dtTableBookings.Columns.Count].Address;
                      resch = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "Resch") - countGroup + 1].Address;
                      direct = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "Direct") - countGroup + 1].Address;
                      inOut = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "IO") - countGroup + 1].Address;
                      procGross = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "ProcGross") - countGroup + 1].Address;
                      procSales = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "ProcSales") - countGroup + 1].Address;
                      eff = (groupsAct.Contains("MANIFEST")) ? $"IF({RealShows}=0,0,{procGross}/{RealShows})" : $"IF({Shows}=0,0,{procGross}/{Shows})";
                      closingFactor = (groupsAct.Contains("MANIFEST")) ? $"IF({RealShows}=0,0,{procSales}/{RealShows})" : $"IF({Shows}=0,0,{procSales}/{Shows})";
                      for (int k = 1; k <= 21; k++)
                      {
                        switch (k)
                        {
                          case 1:
                            wsData.Cells[rowNumber, k].Formula = $"= {RealShows}";
                            break;
                          case 2:
                            wsData.Cells[rowNumber, k].Value = "Tour %";
                            wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,{RealShows}/{Bookings})";
                            wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                            break;
                          case 4:
                            wsData.Cells[rowNumber, k].Value = "Shows";
                            wsData.Cells[rowNumber, k + 1].Formula = $"= {Shows}";
                            wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Number);
                            break;
                          case 6:
                            wsData.Cells[rowNumber, k].Value = "Shows %";
                            wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,({Shows})/{Bookings})";
                            wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                            break;
                          case 8:
                            wsData.Cells[rowNumber, k].Value = "Sin R/D";
                            wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,({Shows}-{resch}-{direct})/{Bookings})";
                            wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                            break;
                          case 10:
                            wsData.Cells[rowNumber, k].Value = "Sin Dtas";
                            wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,({Shows}-{direct})/{Bookings})";
                            wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                            break;
                          case 12:
                            wsData.Cells[rowNumber, k].Value = "Sin Rsch";
                            wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,({Shows}-{resch})/{Bookings})";
                            wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                            break;
                          case 14:
                            wsData.Cells[rowNumber, k].Value = "Sin I&O";
                            wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,({Shows}-{inOut})/{Bookings})";
                            wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                            break;

                          case 16:
                            wsData.Cells[rowNumber, k].Value = "Eff";
                            wsData.Cells[rowNumber, k + 1].Formula = eff;
                            wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.DecimalNumber);
                            break;
                          case 18:
                            wsData.Cells[rowNumber, k].Value = "C %";
                            wsData.Cells[rowNumber, k + 1].Formula = closingFactor;
                            wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                            break;
                          case 20:
                            wsData.Cells[rowNumber, k].Formula = Bookings;
                            wsData.Cells[rowNumber, k].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Number);
                            wsData.Cells[rowNumber, k].Style.Font.Color.SetColor(Color.White);
                            wsData.Cells[rowNumber, k + 1].Value = Tours;
                            wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Number);
                            wsData.Cells[rowNumber, k + 1].Style.Font.Color.SetColor(Color.White);
                            if (subtotalFormulas[j] != null && subtotalFormulas[j].ContainsKey("TTours"))
                            {
                              subtotalFormulas[j]["TTours"] += (subtotalFormulas[j]["TTours"] == string.Empty) ? wsData.Cells[rowNumber, k + 1].Address : "," + wsData.Cells[rowNumber, k + 1].Address;
                            }
                            else
                            {
                              if (subtotalFormulas[j] == null) subtotalFormulas[j] = new Dictionary<string, string>();
                              subtotalFormulas[j].Add("TTours", wsData.Cells[rowNumber, k + 1].Address);
                            }
                            if (subtotalFormulas[j] != null && subtotalFormulas[j].ContainsKey("TBookings"))
                            {
                              subtotalFormulas[j]["TBookings"] += (subtotalFormulas[j]["TBookings"] == string.Empty) ? wsData.Cells[rowNumber, k].Formula : "," + wsData.Cells[rowNumber, k].Formula;
                            }
                            else
                            {
                              if (subtotalFormulas[j] == null) subtotalFormulas[j] = new Dictionary<string, string>();
                              subtotalFormulas[j].Add("TBookings", wsData.Cells[rowNumber, k].Formula);
                            }
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
              formatIndex = 1;
              var countGroup = formatTableColumns.Count(c => c.IsGroup && !c.IsVisible);
              formatTableColumns.ForEach(format =>
              {
                //Si es una columna No Visible continuamos el ciclo.
                if (!format.IsVisible) return;
                if (format.Function == DataFieldFunctions.None && string.IsNullOrWhiteSpace(format.Formula)) { formatIndex++; return; }

                using (var range = wsData.Cells[rowNumber - 1, formatIndex])
                {
                  if (!format.IsCalculated)
                  {
                    switch (format.Function)
                    {
                      case DataFieldFunctions.Sum:
                        if(format.PropertyName=="ProcSales"  || format.PropertyName == "ProcOriginal" || format.PropertyName == "ProcNew" || format.PropertyName == "ProcGross")
                        {
                          range.Formula = $"=SUM({string.Join(",", subtotalFormulas[1][format.PropertyName].Split(',').Select(c => $"SUMIF({c},\">0\")").ToList())})";
                        }
                        else
                          range.Formula = $"=SUM({subtotalFormulas[1][format.PropertyName]})";
                        break;
                      case DataFieldFunctions.Average:
                        range.Formula = $"=AVERAGE({subtotalFormulas[1][format.PropertyName]})";
                        break;
                      case DataFieldFunctions.Count:
                        if (format.Format == EnumFormatTypeExcel.General || format.Format == EnumFormatTypeExcel.Boolean)
                          range.Formula = $"=SUM({subtotalFormulas[1][format.PropertyName]})";
                        break;
                    }
                  }
                  else
                    range.Formula = ReportBuilder.GetFormula(formatTableColumns, format.Formula, rowNumber - 1);

                  range.Style.Numberformat.Format = ReportBuilder.GetFormat(format.Format);
                  formatIndex++;
                }
              });
              using (var range = wsData.Cells[rowNumber - 1, 1, rowNumber - 1, totalColumns])
              {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].BackGroundColor));
                range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(backgroundColorGroups[backgroundColorGroups.Count - 1].FontColor));
                range.Style.Font.Bold = backgroundColorGroups[backgroundColorGroups.Count - 1].FontBold;
              }

              var Tours = $"SUM({subtotalFormulas[1]["TTours"]})";
              var Shows = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "Show") - countGroup + 1].Address;
              var RealShows = $"SUM({wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "Tour") - countGroup + 1].Address},{wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "IO") - countGroup + 1].Address},{wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "WO") - countGroup + 1].Address},{wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "CTour") - countGroup + 1].Address},{wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "STour") - countGroup + 1].Address})";
              var Bookings = dtTableBookings.AsEnumerable().Sum(c => Convert.ToDecimal(c[$"Total{separator.ToString()}Bookings"])).ToString();
              var resch = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "Resch") - countGroup + 1].Address;
              var direct = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "Direct") - countGroup + 1].Address;
              var inOut = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "IO") - countGroup + 1].Address;
              var procGross = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "ProcGross") - countGroup + 1].Address;
              var procSales = wsData.Cells[rowNumber - 1, formatTableColumns.FindIndex(f => f.PropertyName == "ProcSales") - countGroup + 1].Address;
              var eff = $"IF({Tours}=0,0,{procGross}/{Tours})";
              var closingFactor = $"IF({Tours}=0,0,{procSales}/{Tours})";

              for (int k = 1; k <= 21; k++)
              {
                switch (k)
                {
                  case 1:
                    wsData.Cells[rowNumber, k].Formula = $"= {RealShows}";
                    break;
                  case 2:
                    wsData.Cells[rowNumber, k].Value = "Tour %";
                    wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,{RealShows}/{Bookings})";
                    wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                    break;
                  case 4:
                    wsData.Cells[rowNumber, k].Value = "Shows";
                    wsData.Cells[rowNumber, k + 1].Formula = $"= {Shows}";
                    wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Number);
                    break;
                  case 6:
                    wsData.Cells[rowNumber, k].Value = "Shows %";
                    wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,({Shows})/{Bookings})";
                    wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                    break;
                  case 8:
                    wsData.Cells[rowNumber, k].Value = "Sin R/D";
                    wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,({Shows}-{resch}-{direct})/{Bookings})";
                    wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                    break;
                  case 10:
                    wsData.Cells[rowNumber, k].Value = "Sin Dtas";
                    wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,({Shows}-{direct})/{Bookings})";
                    wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                    break;
                  case 12:
                    wsData.Cells[rowNumber, k].Value = "Sin Rsch";
                    wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,({Shows}-{resch})/{Bookings})";
                    wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                    break;
                  case 14:
                    wsData.Cells[rowNumber, k].Value = "Sin I&O";
                    wsData.Cells[rowNumber, k + 1].Formula = $"= IF({Bookings}=0,0,({Shows}-{inOut})/{Bookings})";
                    wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                    break;

                  case 16:
                    wsData.Cells[rowNumber, k].Value = "Eff";
                    wsData.Cells[rowNumber, k + 1].Formula = eff;
                    wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.DecimalNumber);
                    break;
                  case 18:
                    wsData.Cells[rowNumber, k].Value = "C %";
                    wsData.Cells[rowNumber, k + 1].Formula = closingFactor;
                    wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Percent);
                    break;
                  case 20:
                    wsData.Cells[rowNumber, k].Value = "Total Bookings";
                    wsData.Cells[rowNumber, k + 1].Formula = Bookings;
                    wsData.Cells[rowNumber, k + 1].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Number);
                    wsData.Cells[rowNumber, k + 2].Formula = Tours;
                    wsData.Cells[rowNumber, k + 2].Style.Numberformat.Format = ReportBuilder.GetFormat(EnumFormatTypeExcel.Number);
                    wsData.Cells[rowNumber, k + 2].Style.Font.Color.SetColor(Color.White);
                    break;
                }
              }

            }

            #endregion Simple con Agrupados
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
                wsData.Cells[rowNumber, drColumn].Style.Numberformat.Format = ReportBuilder.GetFormat(row.Format);
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
                //Si es una columna No Visible continuamos el ciclo.
                if (!format.IsVisible) return;
                if (format.Function == DataFieldFunctions.None && string.IsNullOrWhiteSpace(format.Formula)) return;
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
                wsData.Cells[rowNumber, format.Order].Style.Numberformat.Format = ReportBuilder.GetFormat(subtotalFormat);
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
                  wsData.Cells[rowNumber, format.Order].Formula = ReportBuilder.GetFormula(formatTable, format.Formula, rowNumber);
              });
            }

            #endregion Simple
          }
          ReportBuilder.AutoFitColumns(ref wsData);

          if (fileFullPath == null)
          {
            var suggestedFilaName = string.Concat(Regex.Replace(reportName, "[^a-zA-Z0-9_]+", " "), " ", dateRangeFileName);
            pathFinalFile = ReportBuilder.SaveExcel(pk, suggestedName: suggestedFilaName); 
          }
          else
            pathFinalFile = ReportBuilder.SaveExcel(pk, filePath:fileFullPath);
        }
        return pathFinalFile;
      });
    }

    #endregion CreateExcelCustomPivot

    #region OrderColumns
    /// <summary>
    /// Método que siver para ordenar la lista de ExcelFormat dependiedo de la posicion de las columnas de su grid
    /// </summary>
    /// <param name="lstColumns">Lista de columnas del grid</param>
    /// <param name="lstExcelFormatTable">Lista de excelformattable</param>
    /// <history>
    /// [emoguel] created 06/07/2016
    /// [edgrodriguez] Modified. 05/09/2016 Se agrega el uso del ExcelFormatItemsList
    /// </history>
    public static ExcelFormatItemsList OrderColumns(List<DataGridColumn> lstColumns, List<ExcelFormatTable> lstExcelFormatTable)
    {
      ExcelFormatItemsList lst = new ExcelFormatItemsList();
      lstColumns.OrderBy(c => c.DisplayIndex).ToList().ForEach(cl =>
      {
        lst.Add(lstExcelFormatTable.FirstOrDefault(c => c.PropertyName == cl.SortMemberPath));
      });
      return lst;
    }
    #endregion

    #endregion Public Methods
  }
}
