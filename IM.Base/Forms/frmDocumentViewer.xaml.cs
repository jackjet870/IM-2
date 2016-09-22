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
using IM.Base.Helpers;
using IM.Model.Classes;
using System.Windows.Controls;
using System.Windows.Data;
using System.Diagnostics;

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
    #region frmDocumentViewer
    /// <summary>
    /// Constructor del formulario
    /// </summary>
    /// <param name="fileInfo">Archivo a mostrar</param>
    /// <param name="exportExcel">Permiso para exportar a excel</param>
    /// <param name="isProcessor">Si se abre desde un processor de reportes</param>
    /// <history>
    /// [emoguel] 13/09/2016 created
    /// </history>
    public frmDocumentViewer(FileInfo fileInfo, bool exportExcel, bool isProcessor = true)
    {
      InitializeComponent();
      _excelFile = fileInfo;
      _exportExcel = exportExcel;
      _fullPathAndName = _excelFile.FullName.Replace(_excelFile.Extension, string.Empty);
      if (!isProcessor)
      {
        Closing += Window_Closed;
        ShowInTaskbar = false;
      }
      //Creamos los commands para exportar a Excel y Pdf
      ExportToExcel = new ExecuteCommandHelper(x => btnExportToExcel_Click(null, null));
      ExportToPdf = new ExecuteCommandHelper(x => btnExportToPdf_Click(null, null));
    } 
    #endregion
    
    #region Window_Loaded
    /// <summary>
    /// Carga el documentViewver
    /// </summary>
    /// <history>
    /// [emoguel] 05/09/2016 created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      string name = _excelFile.Name.Replace(_excelFile.Extension, string.Empty);
      Title = $"Report Viewer - {name}";
      Uid = name;      
      LoadCombobox();
      UpdateLayout();
      LoadXps();
      documentViewer.Focus();//para que se activen los controles del toolbar
      documentViewer.FitToWidth();//Default para mostrar la pagina
      #region Agregarle los eventos a los combobox
      cmbOrientation.SelectionChanged += cmb_SelectionChanged;
      cmbPageSize.SelectionChanged += cmb_SelectionChanged;
      cmbScale.SelectionChanged += cmb_SelectionChanged;
      #endregion
    }
    #endregion

    #region btnExportToPdf_Click
    /// <summary>
    /// Exporta el archivo a formato pdf
    /// </summary>
    /// <history>
    /// [emoguel] created 02/09/2016
    /// </history>
    private void btnExportToPdf_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Mouse.OverrideCursor = Cursors.Wait;
        if (_excelFile.Exists)//Si existe el excel crear el excel para general el pdf
        {
          //await Task.Run(() =>
          //{
            if (File.Exists($"{_fullPathAndName}.pdf"))//verificar si existe el pdf, solo copiar
            {
              exportFile(EnumFileFormat.Pdf);
            }
            else//Si no existe crear el PDF
            {
              CreateFile(EnumFileFormat.Pdf);
              exportFile(EnumFileFormat.Pdf);
            }
          //});
        }

      }
      catch (Exception ex)
      {
        Helpers.UIHelper.ShowMessage(ex);
      }
      finally
      {
        Mouse.OverrideCursor = null;
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
          Mouse.OverrideCursor = Cursors.Wait;
          if (_excelFile.Exists)//Si existe el excel crear el excel para general el pdf
          {
            //await Task.Run(() =>
            //{
              if (_excelFile.Exists)//verificar si existe el pdf, solo copiar
            {
                exportFile(EnumFileFormat.Excel);//Exportamos a Excel
              }
            //});
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
        Mouse.OverrideCursor = null;
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
        lstFiles = directoryInfo.GetFiles($"{Uid}*").ToList();
        lstFiles.ForEach(f => f.Delete());
      }
      catch (Exception ex)
      {
        Helpers.UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region cmb_SelectionChanged
    /// <summary>
    /// Crea un nuevo xps con las opciones seleccionadas
    /// </summary>
    /// <history>
    /// [emoguel] created 14/19/2016
    /// </history>
    private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ComboBox combo = sender as ComboBox;
      if (combo.Name == nameof(cmbOrientation))
      {
        UpdateIcons((cmbOrientation.SelectedIndex == 0) ? "H" : "V");
        LoadXps();
      }
      else
      {
        LoadXps();
      }
    }
    #endregion

    #region ComboBoxItem_MouseClick
    /// <summary>
    /// Asigna el margen al documento, segun lo seleccionado en el combobox
    /// </summary>
    /// <history>
    /// [emoguel] created 17/09/2016
    /// </history>
    private void ComboBoxItem_MouseClick(object sender, MouseButtonEventArgs e)
    {
      ComboBoxItem comboItem = sender as ComboBoxItem;
      int index = cmbMargin.Items.IndexOf(comboItem.Content);      
      if ( index== 3)
      {
        Margin margin = cmbMargin.SelectedValue as Margin;
        frmMargin frmMargin = new frmMargin(margin);
        if (frmMargin.ShowDialog() == true)
        {
          cmbMargin.SelectedIndex = index;
          List<dynamic> lstObj = cmbMargin.ItemsSource.OfType<dynamic>().ToList();
          lstObj[3] = new { name = "Custom Margins", margin = margin };
          LoadXps();
        }
      }
      else
      {
        cmbMargin.SelectedIndex = index;
        LoadXps();        
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
    private void LoadXps()
    {
      Mouse.OverrideCursor = Cursors.Wait;
      XpsDocument xps = null;
      try
      { 
        if (_excelFile.Exists)
        {          
          CreateFile(EnumFileFormat.Xps);          
          if (File.Exists($"{_fullPathAndName}.xps"))
          {
            xps = new XpsDocument($"{_fullPathAndName}.xps", FileAccess.Read);//Cargamos el xps
            documentViewer.Document = xps.GetFixedDocumentSequence();
          }
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
        Mouse.OverrideCursor = null;
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
      dialog.FileName = Uid;
      switch (enumFileFormat)
      {
        case EnumFileFormat.Pdf:
          {
            dialog.Filter = "Pdf files(*.pdf) | *.pdf;";
            if (dialog.ShowDialog() == true)
            {
              File.Copy(_fullPathAndName + ".pdf", dialog.FileName, true); 
              if(File.Exists(dialog.FileName))
              {
                UIHelper.ShowMessage("Document sufesfully saved.");
                Process.Start(dialog.FileName);
              }     
              else
              {
                UIHelper.ShowMessage("Document not saved");
              }        
            }
            break;
          }
        case EnumFileFormat.Excel:
          {
            dialog.Filter = "Excel files(*.xlsx) | *.xlsx;";
            if (dialog.ShowDialog() == true)
            {
              File.Copy(_fullPathAndName + ".xlsx", dialog.FileName, true);

              if (File.Exists(dialog.FileName))
              {
                UIHelper.ShowMessage("Document sufesfully saved.");
                Process.Start(dialog.FileName);
              }
              else
              {
                UIHelper.ShowMessage("Document not saved.");
              }
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
          //Obtenemos la orientacion seleccionada
          XlPageOrientation pageOrientation =(XlPageOrientation)cmbOrientation.SelectedValue;
          //Obtenemos el tamaño de papel seleccionado
          XlPaperSize paperSize =(XlPaperSize)cmbPageSize.SelectedValue;
          //Obtenemos el margen seleccionado
          Margin margin = cmbMargin.SelectedValue as Margin;
          //Obtenemos la escala seleccionada
          EnumScale enumScale = (EnumScale)cmbScale.SelectedValue;          
          excel = new Microsoft.Office.Interop.Excel.Application();
          excel.Visible = false;
          excel.ScreenUpdating = false;
          excel.DisplayAlerts = false;
          
          wb = excel.Workbooks.Open(_excelFile.FullName, 0, false, Missing.Value, Missing.Value, Missing.Value, true, XlPlatform.xlWindows, Missing.Value, false, false, Missing.Value, false, true, false);//Cargamos el excel                              
          _Worksheet ws = ((_Worksheet)wb.ActiveSheet);

          ws.PageSetup.PaperSize = paperSize;//asignamos el tamaño de hoja
          ws.PageSetup.Orientation = pageOrientation;//asignamos orientación de la pagina
          ws.PageSetup.LeftMargin=excel.CentimetersToPoints(margin.left);//asignamos Margen Izquierdo
          ws.PageSetup.RightMargin =excel.CentimetersToPoints(margin.right);//asignamos Margen Derecho
          ws.PageSetup.TopMargin = excel.CentimetersToPoints(margin.top);//asignamos Margen de arriba
          ws.PageSetup.BottomMargin = excel.CentimetersToPoints(margin.bottom);//asignamos Margen de abajo  
          ws.PageSetup.Zoom = false;
          //Asignamos la escala seleccionada
          switch(enumScale)
          {
            case EnumScale.Noscaling:
              {
                ws.PageSetup.FitToPagesTall = false;
                ws.PageSetup.FitToPagesWide = false;
                break;
              }
            case EnumScale.FitSheetOnOnePage:
              {
                ws.PageSetup.FitToPagesTall = 1;
                ws.PageSetup.FitToPagesWide = 1;
                break;
              }
            case EnumScale.FitAllColumnsOnOnePage:
              {                
                ws.PageSetup.FitToPagesWide = 1;
                ws.PageSetup.FitToPagesTall = false;
                break;
              }
            case EnumScale.FitAllRowsOnOnePage:
              {
                ws.PageSetup.FitToPagesTall = 1;
                ws.PageSetup.FitToPagesWide = false;               
                break;
              }
          }
          
          ws.PageSetup.Order = XlOrder.xlOverThenDown;
          
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
        UIHelper.ShowMessage(ex);        
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

    #region LoadCombobox
    /// <summary>
    /// Llena los combos del toolbar
    /// </summary>
    /// <history>
    /// [emoguel] created 14/09/2016
    /// </history>
    private void LoadCombobox()
    {
      #region Orientation
      List<dynamic> lstOrientation = new List<dynamic>();
      lstOrientation.Add(new { description = "Orientation Horizontal", pageOrientation = XlPageOrientation.xlLandscape, img= "pack://application:,,,/IM.Styles;component/Images/32x32/BHorizontalPage.png" });
      lstOrientation.Add(new { description = "Orientation Vertical", pageOrientation = XlPageOrientation.xlPortrait, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BVerticalPage.png" });
      cmbOrientation.ItemsSource = lstOrientation;
      #endregion

      #region PageSize      
      List<dynamic> lstPaperSize = new List<dynamic>();
      lstPaperSize.Add(new { name = "Paper Letter", description="21.59 cm x 29.7 cm",  paperSize = XlPaperSize.xlPaperLetter, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BPageH.png" });
      lstPaperSize.Add(new { name = "Paper Letter Small", description = "21.59 cm x 27.94 cm", paperSize = XlPaperSize.xlPaperLetterSmall, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BPageH.png" });
      lstPaperSize.Add(new { name = "Paper Ledger", description = "43.18 cm x 27.94 cm", paperSize = XlPaperSize.xlPaperLedger, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BPageH.png" });
      lstPaperSize.Add(new { name = "Paper Legal", description = "21.59 cm x 35.56 cm", paperSize = XlPaperSize.xlPaperLegal, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BPageH.png" });
      lstPaperSize.Add(new { name = "Paper A3", description = "29.7 cm x 42 cm", paperSize = XlPaperSize.xlPaperA3, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BPageH.png" });
      lstPaperSize.Add(new { name = "Paper A4", description = "21 cm x 29.7 cm", paperSize = XlPaperSize.xlPaperA4, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BPageH.png" });
      lstPaperSize.Add(new { name = "Paper A5", description = "14.8 cm x 21 cm", paperSize = XlPaperSize.xlPaperA5, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BPageH.png" });
      lstPaperSize.Add(new { name = "Paper 11x17", description = "27.94 cm x 43.18 cm", paperSize = XlPaperSize.xlPaper11x17, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BPageH.png" });
      lstPaperSize.Add(new { name = "Paper Note", description = "21.59 cm x 27.94 cm", paperSize = XlPaperSize.xlPaperNote, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BPageH.png" });
      cmbPageSize.ItemsSource = lstPaperSize;
      #endregion      

      #region Margin
      var lstMargins = new List<object>();
      lstMargins.Add(new { name= "Normal Margins", margin=new Margin { left=1.78,right=1.78,top=1.91,bottom=1.91 }, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BNormalMarginH.png" });
      lstMargins.Add(new { name = "Wide Margins", margin = new Margin { left = 2.54, right = 2.54, top = 2.54, bottom = 2.54 }, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BWideMarginH.png" });
      lstMargins.Add(new { name = "Stretch Margins", margin = new Margin { left = 0.64, right = 0.64, top = 1.91, bottom = 1.91 }, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BStretchMarginH.png" });
      lstMargins.Add(new { name = "Custom Margins", margin = new Margin { left = 0, right = 0, top = 0, bottom = 0 }, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BCustomMarginH.png" });
      cmbMargin.ItemsSource = lstMargins;
      #endregion

      #region Scale
      List<dynamic> lstScales = new List<dynamic>();
      lstScales.Add(new { name = "No Scaling",scale=EnumScale.Noscaling, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BNoScalingH.png" });
      lstScales.Add(new { name = "Fit Sheet on One Page",scale=EnumScale.FitSheetOnOnePage, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BFitOnOnePageH.png" });
      lstScales.Add(new { name = "Fit All Columns on One Page",scale=EnumScale.FitAllColumnsOnOnePage, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BFitColumnOnPageH.png" });
      lstScales.Add(new { name = "Fit All Rows on One Page",scale=EnumScale.FitAllRowsOnOnePage, img = "pack://application:,,,/IM.Styles;component/Images/32x32/BFitRowsOnPageH.png" });
      cmbScale.ItemsSource = lstScales;
      #endregion
    }
    #endregion

    #region UpdateIcons
    /// <summary>
    /// Cambia los iconos de los combobox dependiendo de la opcion seleccionada
    /// </summary>
    /// <param name="Orientation">
    /// H .-Horizontal
    /// V .-Vertical
    /// </param>
    /// <history>
    /// [created] 21/09/2016 emoguel
    /// </history>
    private void UpdateIcons(string Orientation)
    {
      //Desuscribimos los combos del evento selectionchaged
      cmbPageSize.SelectionChanged -= cmb_SelectionChanged;
      cmbScale.SelectionChanged -= cmb_SelectionChanged;
      //Actualizamos las listas, como son listas dinamicas no se puede actualizar solo una propiedad
      #region Scale      
      int indexScale = cmbScale.SelectedIndex;
      List<dynamic> lstScaling = cmbScale.ItemsSource as List<dynamic>;
      lstScaling[0] = new { name = "No Scaling", scale = EnumScale.Noscaling, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BNoScaling{Orientation}.png" };
      lstScaling[1] = new { name = "Fit Sheet on One Page", scale = EnumScale.FitSheetOnOnePage, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BFitOnOnePage{Orientation}.png" };
      lstScaling[2] = new { name = "Fit All Columns on One Page", scale = EnumScale.FitAllColumnsOnOnePage, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BFitColumnOnPage{Orientation}.png" };
      lstScaling[3] = new { name = "Fit All Rows on One Page", scale = EnumScale.FitAllRowsOnOnePage, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BFitRowsOnPage{Orientation}.png" };      
      cmbScale.SelectedItem = lstScaling[indexScale];
      cmbScale.SelectedIndex = indexScale;
      #endregion

      #region Margin
      int indexMargin = cmbMargin.SelectedIndex;
      var lstMargins = cmbMargin.ItemsSource as List<dynamic>;
      lstMargins[0] = new { name = "Normal Margins", margin = new Margin { left = 1.78, right = 1.78, top = 1.91, bottom = 1.91 }, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BNormalMargin{Orientation}.png" };
      lstMargins[1] = new { name = "Wide Margins", margin = new Margin { left = 2.54, right = 2.54, top = 2.54, bottom = 2.54 }, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BWideMargin{Orientation}.png" };
      lstMargins[2] = new { name = "Stretch Margins", margin = new Margin { left = 0.64, right = 0.64, top = 1.91, bottom = 1.91 }, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BStretchMargin{Orientation}.png" };
      lstMargins[3] = new { name = "Custom Margins", margin = lstMargins[3].margin, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BCustomMargin{Orientation}.png" };
      cmbMargin.SelectedItem = lstMargins[indexMargin];
      cmbMargin.SelectedIndex = indexMargin;
      #endregion

      #region PageSize      
      int indexSize = cmbPageSize.SelectedIndex;
      List<dynamic> lstPaperSize = cmbPageSize.ItemsSource as List<dynamic>;
      lstPaperSize[0] = new { name = "Paper Letter", description = "21.59 cm x 29.7 cm", paperSize = XlPaperSize.xlPaperLetter, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BPage{Orientation}.png" };
      lstPaperSize[1] = new { name = "Paper Letter Small", description = "21.59 cm x 27.94 cm", paperSize = XlPaperSize.xlPaperLetterSmall, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BPage{Orientation}.png" };
      lstPaperSize[2] = new { name = "Paper Ledger", description = "43.18 cm x 27.94 cm", paperSize = XlPaperSize.xlPaperLedger, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BPage{Orientation}.png" };
      lstPaperSize[3] = new { name = "Paper Legal", description = "21.59 cm x 35.56 cm", paperSize = XlPaperSize.xlPaperLegal, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BPage{Orientation}.png" };
      lstPaperSize[4] = new { name = "Paper A3", description = "29.7 cm x 42 cm", paperSize = XlPaperSize.xlPaperA3, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BPage{Orientation}.png" };
      lstPaperSize[5] = new { name = "Paper A4", description = "21 cm x 29.7 cm", paperSize = XlPaperSize.xlPaperA4, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BPage{Orientation}.png" };
      lstPaperSize[6] = new { name = "Paper A5", description = "14.8 cm x 21 cm", paperSize = XlPaperSize.xlPaperA5, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BPage{Orientation}.png" };
      lstPaperSize[7] = new { name = "Paper 11x17", description = "27.94 cm x 43.18 cm", paperSize = XlPaperSize.xlPaper11x17, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BPage{Orientation}.png" };
      lstPaperSize[8] = new { name = "Paper Note", description = "21.59 cm x 27.94 cm", paperSize = XlPaperSize.xlPaperNote, img = $"pack://application:,,,/IM.Styles;component/Images/32x32/BPage{Orientation}.png" };
      cmbPageSize.SelectedItem = lstPaperSize[indexSize];
      cmbPageSize.SelectedIndex = indexSize;
      #endregion

      //Suscribimos los combos al evento selectionchaged
      cmbPageSize.SelectionChanged += cmb_SelectionChanged;
      cmbScale.SelectionChanged += cmb_SelectionChanged;
    } 
    #endregion
    #endregion

  }
}
