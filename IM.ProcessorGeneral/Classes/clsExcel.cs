using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using System.Windows;
using System.Data;

namespace IM.ProcessorGeneral.Classes
{
  public class clsExcel
  {
    //    public static void ExportToExcel(string path, string Title, DataTable dtData)
    //    {
    //      FileInfo file = new FileInfo(path);
    //      using (ExcelPackage package = new ExcelPackage())
    //      {
    //        ExcelWorksheet ws = package.Workbook.Worksheets.Add("testsheet");

    //        ws.Cells["A5"].LoadFromDataTable(dtData, true, OfficeOpenXml.Table.TableStyles.Medium12);

    //        //Create pivot table at A20 using values from A1:F10 named pivTable
    //        var pivotTable = ws.PivotTables.Add(ws.Cells["G5"], ws.Cells["A5:E5"], "pivTable");
    //        ws.Workbook.Worksheets[0].Hidden = eWorkSheetHidden.Hidden;
    //        //Assign which Rows and Columns of A1:F10 are data or headers
    //        pivotTable.ShowHeaders = true;
    //        pivotTable.FirstHeaderRow = 1;
    //        pivotTable.FirstDataCol = 1;
    //        pivotTable.FirstDataRow = 2;

    //        //Row Labels
    //        pivotTable.RowFields.Add(pivotTable.Fields["SalesRoom"]);
    //        pivotTable.RowFields.Add(pivotTable.Fields["Program"]);
    //        pivotTable.RowFields.Add(pivotTable.Fields["BookType"]);
    //        pivotTable.DataOnRows = false;
    //        //Values
    //        pivotTable.DataFields.Add(pivotTable.Fields["Books"]);
    //        //ws.Cells["B1"].Value = "Number of Used Agencies";
    //        //ws.Cells["C1"].Value = "Active Agencies";
    //        //ws.Cells["D1"].Value = "Inactive Agencies";
    //        //ws.Cells["E1"].Value = "Total Hours Volunteered";
    //        //ws.Cells["B1:E1"].Style.Font.Bold = true;

    //        //int x = 2;
    //        //char pos = 'B';
    //        //foreach (object o in arr)
    //        //{
    //        //  String str = pos + x.ToString();
    //        //  ws.Cells[str].Value = o.ToString();
    //        //  if (pos > 'E')
    //        //  {
    //        //    pos = 'B';
    //        //    x++;
    //        //  }

    //        //  pos++;
    //        //}
    //        //newFile.Create();
    //        //newFile.MoveTo(@"C:/testSheet.xlsx");
    //        //package.SaveAs(newFile);
    //        package.SaveAs(file);
    //        /*Stream stream = File.Create(path);
    //        package.SaveAs(stream);
    //        stream.Close();*/

    //        //byte[] data = File.ReadAllBytes(path);
    //        //byte[] bin = package.GetAsByteArray();
    //        //String path = Path.GetTempPath();
    //        //File.WriteAllBytes(path, bin);
    //        //File.WriteAllBytes(path + "testsheet.xlsx", bin);
    //        //HttpContext.Current.Response.Write("<script>alert('"+ temp + "');</script>");
    //        //System.Diagnostics.Process.Start(path + "\\testsheet.xlsx");
    //    //  }
    //    //}

    //    //public static void ExportToExcel(string path, string Title, DataTable dtData, Dictionary<string, DateTime> DateFilters = null, Dictionary<string, string> filters = null, )
    //    //{
    //    //  FileInfo file = new FileInfo(path);
    //    //  using (ExcelPackage package = new ExcelPackage())
    //    //  {
    //    //    if (pivotColums != null)
    //    //    {
    //    //      ExcelWorksheet ws = package.Workbook.Worksheets.Add("Hoja0");
    //    //      ExcelWorksheet pivotws = package.Workbook.Worksheets.Add("Report");


    //    //      //Ingresamos la tabla con los datos en la hoja inicial.
    //    //      ws.Cells["A1"].LoadFromDataTable(dtData, true, OfficeOpenXml.Table.TableStyles.Custom);
    //    //      //Ocultamos la hoja inicial.
    //    //      ws.Hidden = eWorkSheetHidden.VeryHidden;
    //    //      try
    //    //      {
    //    //        //Create pivot table at A20 using values from A1:F10 named pivTable
    //    //        var pivotTable = pivotws.PivotTables.Add(pivotws.Cells["A1"], ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column], "pivTable");

    //    //        //Assign which Rows and Columns of A1:F10 are data or headers
    //    //        pivotTable.ShowHeaders = true;
    //    //        //Asignamos las columnas para realizar el pivote
    //    //        pivotColums.ForEach(col => pivotTable.ColumnFields.Add(pivotTable.Fields[col]));
    //    //        //Asignamos las filas que se mostraran.
    //    //        pivotRows.ForEach(row => pivotTable.RowFields.Add(pivotTable.Fields[row]));
    //    //        pivotTable.DataOnRows = false;
    //    //        //Asignamos el valor que se mostrara en las columnas.
    //    //        pivotTable.DataFields.Add(pivotTable.Fields[pivotValue]);
    //    //        pivotTable.ColumGrandTotals = true;
    //    //        pivotTable.RowGrandTotals = true;

    //    //        //Agregamos el Encabezado
    //    //        pivotws.InsertRow(1, 2);
    //    //        pivotws.Cells[1, pivotTable.Address.End.Column].Value = Title;
    //    //        pivotws.Cells[2, pivotTable.Address.End.Column].Value = DateTime.Now.ToLongDateString();

    //    //        //Agregamos los filtros.
    //    //        pivotws.InsertRow(3, filters.Count);
    //    //        //for (int i = 1; i < filters.Count; i++)
    //    //        //{
    //    //        //  pivotws.Cells[i, 1].Value = string.Format("{0}: {1}", filters[i].Keys[i])
    //    //        //}
    //    //      }
    //    //      catch (Exception ex)
    //    //      {

    //    //      }
    //    //    }
    //    //    else
    //    //    {

    //    //    }
    //    //    package.SaveAs(file);
    //    //  }
    //    //}
  }
}
