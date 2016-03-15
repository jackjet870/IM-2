using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Microsoft.Win32;
using IM.Model.Enums;
using IM.Model.Classes;

namespace IM.Base.Helpers
{
  public class EpplusHelper
  {
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

    /// <summary>
    /// Crea un reporte en excel que tiene Filtros, Nombre de reporte, contenido, datos de impresion y nombre del sistema
    /// </summary>
    /// <param name="filter">
    /// item1=nombre_Filtro
    /// item2=lista_filtros
    /// </param>
    /// <param name="dt">DataTable</param>
    /// <param name="reportName">Nombre del Reporte</param>
    /// <param name="formatColumns">Lista de ExcelFormatTable </param>
    /// <returns>FileInfo ruta para abrir el archivo excel</returns>
    /// <history>
    /// [erosado] 12/03/2016  Created.
    /// [erosado] 14/03/2016  Modified  Se agrego el parametro formatColumns.
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
      //Insertamos etiqueta de filtros
      ws.Cells[1, 1].Value = "Filters";
      ws.Cells[1, 1].Style.Font.Bold = true;
      ws.Cells[1, 1].Style.Font.Size = 14;
      //Insertamos las filas que necesitamos para los filtros
      ws.InsertRow(2, filter.Count);

      int cFiltros = 1;
      foreach (Tuple<string, string> item in filter)
      {
        cFiltros++;
        ws.Cells[string.Concat("A", cFiltros)].Value = item.Item1;
        ws.Cells[string.Concat("A", cFiltros)].Style.Font.Bold = true;
        ws.Cells[string.Concat("B", cFiltros)].Value = item.Item2;
      }

      //Nombre del reporte y rango de fecha
      ws.Cells[1, dtColumnsNumber].Value = reportName.Item1;
      ws.Cells[1, dtColumnsNumber].Style.Font.Bold = true;
      ws.Cells[1, dtColumnsNumber].Style.Font.Size = 14;
      ws.Cells[1, dtColumnsNumber].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
      ws.Cells[2, dtColumnsNumber].Value = reportName.Item2;
      ws.Cells[1, dtColumnsNumber].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
      ws.Cells[1, dtColumnsNumber, 2, dtColumnsNumber].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Double);
      
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

      if (pathFinalFile != null || pathFinalFile.Exists)
      {
        return pathFinalFile;
      }
      else
      {
        return null;
      }
    }
  }
}
