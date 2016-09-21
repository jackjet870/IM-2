using IM.Base.Classes;
using IM.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmReportQueue.xaml
  /// </summary>
  public partial class frmReportQueue : Window
  {
    #region Atributos

    private ObservableCollection<objReportQueue> ObjReportQueues { get; set; } = new ObservableCollection<objReportQueue>();
    bool _exportExcel = true;

    #endregion Atributos

    #region Constructores y destructores

    public frmReportQueue(bool exportExcel)
    {
      InitializeComponent();
      var objReportQueueViewSource = (CollectionViewSource)(FindResource("ObjReportQueueViewSource"));
      objReportQueueViewSource.Source = ObjReportQueues;
      ObjReportQueues.CollectionChanged += OnCollectionChanged;
      _exportExcel = exportExcel;
    }

    #endregion Constructores y destructores

    #region Eventos del formulario

    #region Window_Closing

    /// <summary>
    /// Evita que la ventana se cierre, solo la oculta
    /// </summary>
    /// <history>
    /// [aalcocer] 06/06/2016 Created
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {      
      e.Cancel = true;
      Hide();
    }

    #endregion Window_Closing

    #region OnCollectionChanged

    /// <summary>
    /// Muestra la ventana cuando se agrega o quita un reporte de la cola
    /// </summary>
    /// <history>
    /// [aalcocer] 06/06/2016 Created
    /// </history>
    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Show();
    }

    #endregion OnCollectionChanged

    #region BtnOpenReport_OnClick

    /// <summary>
    /// Abre un reporte
    /// </summary>
    /// <history>
    /// [aalcocer] 06/06/2016 Created
    /// [emoguel] 10/09/2016 Modified. Ahora se abre el visor de reportes
    /// </history>
    private void BtnOpenReport_OnClick(object sender, RoutedEventArgs e)
    {
      var objReportQueue = ((FrameworkElement)sender).DataContext as objReportQueue;
      if (objReportQueue != null)
        ObjReportQueues.Where(x => x.Id == objReportQueue.Id).ToList().ForEach(x =>
        {
          FileInfo file = new FileInfo(x.Id);
          if (file.Exists)//Verificamos que el archivo exista
          {
            if (!UIHelper.IsOpenWindow(file.Name.Replace(file.Extension,string.Empty), true))//Verificamos si la ventana ya está abierta
            {
              //Abrimos el visor de reportes
              frmDocumentViewer frmDocumentViewver = new frmDocumentViewer(file,_exportExcel);              
              frmDocumentViewver.Show();
            }            
          }
          else
            UIHelper.ShowMessage("The file does not exist", MessageBoxImage.Warning);
        });
    }

    #endregion BtnOpenReport_OnClick

    #region BtnClearCompleted_OnClick

    /// <summary>
    /// Quita de la cola de reportes, los que ya han sido generados
    /// </summary>
    /// <history>
    /// [aalcocer] 06/06/2016 Created
    /// </history>
    private void BtnClearCompleted_OnClick(object sender, RoutedEventArgs e)
    {      
      //Eliminamos los archivos temporales que fueron creados
      DeleteReports(true);
    }

    #endregion BtnClearCompleted_OnClick

    #region Window_Closed
    /// <summary>
    /// Elimina los archivos temporales que fueron creados
    /// </summary>
    /// <history>
    /// [emoguel] created 10/09/2016
    /// </history>
    private void Window_Closed(object sender, EventArgs e)
    {
      DeleteReports();
    }
    #endregion

    #endregion Eventos del formulario

    #region Métodos Públicos

    #region AddReport

    /// <summary>
    /// Agrega un reporte a la cola
    /// </summary>
    /// <param name="id">id del reporte</param>
    /// <param name="reportname">Nombre del reporte</param>
    /// <history>
    /// [aalcocer] 06/06/2016 Created
    /// </history>
    public void AddReport(string id, string reportname)
    {
      if (ObjReportQueues.Any(x => x.Id == id))
      {
        ObjReportQueues.Where(x => x.Id == id).ToList().ForEach(x =>
        {
          x.ReportName = reportname;
          
        });
      }
      else
      {
        ObjReportQueues.Insert(0, new objReportQueue()
        {
          Id = id,
          ReportName = reportname
        });
      }
    }

    #endregion AddReport

    #region SetExist

    /// <summary>
    /// Le agrega un archivo al reporte
    /// </summary>
    /// <param name="id">id del reporte</param>
    /// <param name="fileInfo">Archivo</param>
    /// <history>
    /// [emoguel] 09/06/2016 created
    /// </history>
    public void SetExist(string id, FileInfo fileInfo)
    {
      ObjReportQueues.Where(x => x.Id == id).ToList().ForEach(x =>
      {
        x.Exists = fileInfo.Exists;
        x.FileName = fileInfo.Name.Replace(fileInfo.Extension,string.Empty);
      });
    }
    #endregion

    #region DeleteReports
    /// <summary>
    /// Elimina los archivos temporales y cierra las ventanas abiertas
    /// </summary>
    /// <history>
    /// [emoguel]  05/09/2016 Created
    /// </history>
    private void DeleteReports(bool btnClear=false)
    {
      try
      {
        List<string> lstName = ObjReportQueues.Where(x => !string.IsNullOrWhiteSpace(x.ReportName)).ToList().Select(x => x.FileName).ToList();//Buscamos el nombre de los reportes
        CloseWindows(lstName);//Cerramos las ventanas de reportes abiertos

        //Borrar archivos con fecha diferente a la de hoy
        DirectoryInfo directoryInfo = new DirectoryInfo(SettingsHelper.GetReportsPath());
        List<FileInfo> lstFiles = directoryInfo.Parent.GetFiles().Where(f => f.CreationTime.Date != DateTime.Now.Date).ToList();
        lstFiles.ForEach(fi => fi.Delete());//Eliminamos los archivos

        //Borrar archivos con el mismo nombre
        ObjReportQueues.Where(x => !string.IsNullOrWhiteSpace(x.ReportName)).ToList()
          .ForEach(x =>
          {     
            if(btnClear)
            {
              ObjReportQueues.Remove(x);
            }   
            lstFiles = directoryInfo.GetFiles($"{x.FileName}*").ToList();
            lstFiles.ForEach(f => f.Delete());            
          });      
        

      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region CloseWIndows
    /// <summary>
    /// Cierra todos los reportes abiertos
    /// </summary>
    /// <param name="lstNames">Lista del nombre de la ventana</param>
    /// <history>
    /// [emoguel] 09/09/2016 Created.
    /// </history>
    private void CloseWindows(List<string> lstNames)
    {      
      List<Window> lstWindows = Application.Current.Windows.OfType<Window>().Where(x => lstNames.Contains(x.Uid)).ToList();//buscamos una ventana con el mismo Nombre
      lstWindows.ForEach(wd => wd.Close());//Cerramos las ventanas
    }
    #endregion

    #endregion Métodos Públicos
  }
}