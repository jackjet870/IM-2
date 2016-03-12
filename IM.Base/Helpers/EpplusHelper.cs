using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Microsoft.Win32;
using IM.Base.Helpers;

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
    /// <param name="filtros">
    /// item1=nombre_Filtro
    /// item2=lista_filtros
    /// </param>
    /// <param name="dtContenido">DataTable</param>
    /// <param name="nombreReporte">Nombre del Reporte</param>
    /// <returns>FileInfo ruta para abrir el archivo excel</returns>
    /// <history>
    /// [erosado] 12/03/2016  Created.
    /// </history>
    public static FileInfo CreateRptExcelWithOutTemplate(List<Tuple<string, string>> filtros, DataTable dtContenido, Tuple<string, string> nombreReporte)
    {
      FileInfo pathFinalFile = null;
      ExcelPackage pk = new ExcelPackage();
      //Preparamos la hoja donde escribiremos
      ExcelWorksheet ws = pk.Workbook.Worksheets.Add(nombreReporte.Item1);

      //Numero de filtros
      int filterNumber = filtros.Count;

      //Numero de filas del contenido del datatable
      int dtRowsNumber = dtContenido.Rows.Count;
      //Numero de Columnas del contenido del datatable
      int dtColumnsNumber = dtContenido.Columns.Count;
      //Insertamos etiqueta de filtros
      ws.Cells[1, 1].Value = "Filters";
      ws.Cells[1, 1].Style.Font.Bold = true;
      ws.Cells[1, 1].Style.Font.Size = 14;

      //Insertamos las filas que necesitamos para los filtros
      ws.InsertRow(2, filtros.Count);

      int cFiltros = 1;
      foreach (Tuple<string, string> item in filtros)
      {
        cFiltros++;
        ws.Cells[string.Concat("A", cFiltros)].Value = item.Item1;
        ws.Cells[string.Concat("A", cFiltros)].Style.Font.Bold = true;
        ws.Cells[string.Concat("B", cFiltros)].Value = item.Item2;
      }

      //Nombre del reporte y rango de fecha
      ws.Cells[1, dtColumnsNumber].Value = nombreReporte.Item1;
      ws.Cells[1, dtColumnsNumber].Style.Font.Bold = true;
      ws.Cells[1, dtColumnsNumber].Style.Font.Size = 14;
      ws.Cells[1, dtColumnsNumber].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
      ws.Cells[2, dtColumnsNumber].Value = nombreReporte.Item2;
      ws.Cells[1, dtColumnsNumber].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
      ws.Cells[1, dtColumnsNumber, 2, dtColumnsNumber].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Double);

      //Agregamos el contenido
      ws.InsertRow(filterNumber + 2, dtContenido.Rows.Count);
      ws.Cells[filterNumber + 2, 1].LoadFromDataTable(dtContenido, true, OfficeOpenXml.Table.TableStyles.Medium2);

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

      string suggestedFilaName = string.Concat(nombreReporte.Item1, "_", nombreReporte.Item2);
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
