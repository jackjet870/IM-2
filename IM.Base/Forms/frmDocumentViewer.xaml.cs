using Microsoft.Office.Interop.Excel;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Xps.Packaging;
using IM.Model.Enums;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using IM.Base.Helpers;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmDocumentViewver.xaml
  /// </summary>
  public partial class frmDocumentViewer : System.Windows.Window
  {
    #region Variables
    FileInfo _excelFile;
    string _fullPathAndName;
    private bool _exportExcel = false;
    public ExecuteCommandHelper ExportToExcel { get; set; }
    public ExecuteCommandHelper ExportToPdf { get; set; } 
    #endregion
    #region Eventos del formulario
    public frmDocumentViewer(FileInfo fileInfo, bool exportExcel, bool isProcessor = true)
    {
      InitializeComponent();
      _excelFile = fileInfo;
      _exportExcel = exportExcel;      
      _fullPathAndName = _excelFile.FullName.Replace(_excelFile.Extension, string.Empty);
      if (!isProcessor)
      {
        Closing += Window_Closed;
      }
      //Creamos los commands para exportar a Excel y Pdf
      ExportToExcel = new ExecuteCommandHelper(x => btnExportToExcel_Click(null,null));
      ExportToPdf = new ExecuteCommandHelper(x => btnExportToPdf_Click(null,null));
    }
    #region Window_Loaded
    /// <summary>
    /// Carga el documentViewver
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      string name = _excelFile.Name.Replace(_excelFile.Extension, string.Empty);
      Uid = name;
      Title = name;
      LoadXps();
    }
    #endregion

    #region btnExportToPdf_Click
    /// <summary>
    /// Exporta el archivo a formato pdf
    /// </summary>
    /// <history>
    /// [emoguel] created 02/09/2016
    /// </history>
    private async void btnExportToPdf_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Cursor = Cursors.Wait;
        if (_excelFile.Exists)//Si existe el excel crear el excel para general el pdf
        {
          await Task.Run(() =>
          {
            if (File.Exists($"{_fullPathAndName}.pdf"))//verificar si existe el pdf, solo copiar
            {
              exportFile(EnumFileFormat.Pdf);
            }
            else//Si no existe crear el PDF
            {
              CreateFile(EnumFileFormat.Pdf);
              exportFile(EnumFileFormat.Pdf);
            }
          });
        }

      }
      catch (Exception ex)
      {
        Helpers.UIHelper.ShowMessage(ex);
      }
      finally
      {
        Cursor = Cursors.Arrow;
      }
    }
    #endregion

    #region btnExportToExcel_Click
    /// <summary>
    /// Exporta el documento a Excel
    /// </summary>
    /// <history>
    /// [emoguel] 02/09/2016 created
    /// </history>
    private async void btnExportToExcel_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (_exportExcel)//Verificamos si tiene permiso para exportar a Excel
        {
          Cursor = Cursors.Wait;
          if (_excelFile.Exists)//Si existe el excel crear el excel para general el pdf
          {
            await Task.Run(() =>
            {
              if (_excelFile.Exists)//verificar si existe el pdf, solo copiar
            {
                exportFile(EnumFileFormat.Excel);//Exportamos a Excel
              }
            });
          }
        }
        else
        {
          Helpers.UIHelper.ShowMessage("You don't have permission to export to excel");
        }
      }
      catch (Exception ex)
      {
        Helpers.UIHelper.ShowMessage(ex);
      }
      finally
      {
        Cursor = Cursors.Arrow;
      }
    }
    #endregion

    #region Window_Closed
    /// <summary>
    /// Elimina los archivos con el nombre del reporte cuando no fue abierto desde 
    /// el report queue o desde un Processor
    /// </summary>
    /// <history>
    /// [emoguel] 07/09/2016 created
    /// </history>
    private void Window_Closed(object sender, EventArgs e)
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(Helpers.SettingsHelper.GetReportsPath());
        //Eliminamos archivos que no sean de la fecha de hoy
        //en caso de que se haya cambiado el nombre de la Maquina
        List<FileInfo> lstFiles = directoryInfo.Parent.GetFiles("*.*", SearchOption.AllDirectories).Where(f => f.CreationTime.Date != DateTime.Now.Date).ToList();
        lstFiles.ForEach(fi => fi.Delete());
        //Eliminamos los archivos que fueron creados hoy
        lstFiles = directoryInfo.GetFiles($"{Title}*").ToList();
        lstFiles.ForEach(f => f.Delete());
      }
      catch (Exception ex)
      {
        Helpers.UIHelper.ShowMessage(ex);
      }
    }
    #endregion
    #endregion

    #region eventos privados
    #region LoadXps
    /// <summary>
    /// Genera el xps a mostrar a partir del excel
    /// </summary>
    /// <history>
    /// [emogue] 01/09/2016 created
    /// </history>
    private async void LoadXps()
    {
      Cursor = Cursors.Wait;
      XpsDocument xps = null;
      try
      { 
        if (_excelFile.Exists)
        {
          if (!File.Exists($"{_fullPathAndName}.xps"))
          {
            await Task.Run(() =>
          {
            CreateFile(EnumFileFormat.Xps);
          });
          }
          xps = new XpsDocument($"{_fullPathAndName}.xps", FileAccess.Read);//Cargamos el xps
          documentViewer.Document = xps.GetFixedDocumentSequence();
        }
      }
      catch (Exception ex)
      {
        Helpers.UIHelper.ShowMessage(ex);
      }
      finally
      {
        if(xps!=null)
        {
          xps.Close();
        }
        Cursor = Cursors.Arrow;
      }

    }
    #endregion

    #region exportFile
    /// <summary>
    /// Copia un archivo a la ruta que seleccione el usuario
    /// </summary>
    /// <param name="extension">xps, excel, pdf</param>
    /// <history>
    /// [emoguel] 02/09/2016 created
    /// </history>
    public void exportFile(EnumFileFormat enumFileFormat)
    {
      SaveFileDialog dialog = new SaveFileDialog();//Cargamos el saveFileDialog
      switch (enumFileFormat)
      {
        case EnumFileFormat.Pdf:
          {
            dialog.Filter = "Pdf files(*.pdf) | *.pdf;";
            if (dialog.ShowDialog() == true)
            {
              File.Copy(_fullPathAndName + ".pdf", dialog.FileName, false);
            }
            break;
          }
        case EnumFileFormat.Excel:
          {
            dialog.Filter = "Excel files(*.xlsx) | *.xlsx;";
            if (dialog.ShowDialog() == true)
            {
              File.Copy(_fullPathAndName + ".xlsx", dialog.FileName, false);
            }
            break;
          }
      }
    }
    #endregion

    #region CreateFile
    /// <summary>
    /// Crea un documento dependiendo del fileformat que se requiera
    /// </summary>
    /// <param name="enumFileFormat">enumerdo con el formato para crear el documento</param>
    /// <history>
    /// [emoguel] 02/09/2016 created
    /// </history>
    private void CreateFile(EnumFileFormat enumFileFormat)
    {
      Workbook wb = null;
      Microsoft.Office.Interop.Excel.Application excel = null;
      try
      {
        if (_excelFile.Exists)
        {
          excel = new Microsoft.Office.Interop.Excel.Application();
          excel.Visible = false;
          excel.ScreenUpdating = false;
          excel.DisplayAlerts = false;

          wb = excel.Workbooks.Open(_excelFile.FullName, 0, true, Missing.Value, Missing.Value, Missing.Value, true, XlPlatform.xlWindows, Missing.Value, false, false, Missing.Value, false, true, false);//Cargamos el excel                              
          _Worksheet ws = ((_Worksheet)wb.ActiveSheet);
         
          ws.PageSetup.Orientation = XlPageOrientation.xlLandscape;
          ws.PageSetup.PaperSize = XlPaperSize.xlPaperLetter;//Tamaño carta
          ws.PageSetup.LeftMargin=0.64;
          ws.PageSetup.RightMargin = 0.64;
          ws.PageSetup.TopMargin = 1.91;
          ws.PageSetup.BottomMargin = 1.91;
          
          switch (enumFileFormat)
          {
            case EnumFileFormat.Pdf:
              {
                wb.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, $"{_fullPathAndName}.pdf", XlFixedFormatQuality.xlQualityStandard, false, true, Missing.Value, Missing.Value, false, Missing.Value);//Guardamos como PDF
                break;
              }
            case EnumFileFormat.Xps:
              {
                wb.ExportAsFixedFormat(XlFixedFormatType.xlTypeXPS, $"{_fullPathAndName}.xps", XlFixedFormatQuality.xlQualityStandard, false, true, Missing.Value, Missing.Value, false, Missing.Value);//Guardamos como XPS          
                break;
              }
          }
        }
      }
      catch (Exception ex)
      {
        Helpers.UIHelper.ShowMessage(ex);
      }
      finally
      {
        if (wb != null)
        {
          wb.Close();
        }
        if (excel != null)
        {
          excel.Quit();
        }
      }
    }
    #endregion

    #endregion
  }
}
