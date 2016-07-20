using IM.Base.Classes;
using IM.Base.Helpers;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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

    private ObservableCollection<objReportQueue> ObjReportQueues { get; } = new ObservableCollection<objReportQueue>();

    #endregion Atributos

    #region Constructores y destructores

    public frmReportQueue()
    {
      InitializeComponent();
      var objReportQueueViewSource = (CollectionViewSource)(FindResource("ObjReportQueueViewSource"));
      objReportQueueViewSource.Source = ObjReportQueues;
      ObjReportQueues.CollectionChanged += OnCollectionChanged;
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
    /// </history>
    private void BtnOpenReport_OnClick(object sender, RoutedEventArgs e)
    {
      var objReportQueue = ((FrameworkElement)sender).DataContext as objReportQueue;
      if (objReportQueue != null)
        ObjReportQueues.Where(x => x.Id == objReportQueue.Id).ToList().ForEach(x =>
        {
          x.FileInfo.Refresh();
          x.Exists = x.FileInfo.Exists;

          if (x.Exists)
            Process.Start(x.FileInfo.FullName);
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
      ObjReportQueues.Where(x => x.FileInfo != null).ToList().ForEach(x => ObjReportQueues.Remove(x));
    }

    #endregion BtnClearCompleted_OnClick

    #region BtnOpenFolder_OnClick

    /// <summary>
    /// Abre la carpeta donde estan los archivos
    /// </summary>
    /// <history>
    /// [aalcocer] 06/06/2016 Created
    /// [aalcocer] 13/06/2016 Modified. La ruta por default se obtiene en la configuracion
    /// </history>
    private void BtnOpenFolder_OnClick(object sender, RoutedEventArgs e)
    {
      if (ConfigRegistryHelper.ExistReportsPath())
      {
        string outputDir = ConfigRegistryHelper.GetReportsPath();
        Process.Start(outputDir);
      }
      else
      {
        UIHelper.ShowMessage("It is not configured path.", MessageBoxImage.Warning, Title);
        Hide();
      }
    }

    #endregion BtnOpenFolder_OnClick

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
          x.FileInfo = null;
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

    #region SetFileInfo

    /// <summary>
    /// Le agrega un archivo al reporte
    /// </summary>
    /// <param name="id">id del reporte</param>
    /// <param name="fileInfo">Archivo</param>
    /// <history>
    /// [aalcocer] 06/06/2016 Created
    /// </history>
    public void SetFileInfo(string id, FileInfo fileInfo)
    {
      ObjReportQueues.Where(x => x.Id == id).ToList().ForEach(x =>
      {
        fileInfo.Refresh();
        x.FileInfo = fileInfo;
        x.Exists = fileInfo.Exists;
      });
    }

    #endregion SetFileInfo

    #region SetFileInfoError

    /// <summary>
    ///Le actualiza el archivo del reporte cuando ocurre un error
    /// </summary>
    /// <param name="id">id del reporte</param>
    /// <history>
    /// [aalcocer] 13/06/2016 Created
    /// </history>
    public void SetFileInfoError(string id)
    {
      ObjReportQueues.Where(x => x.Id == id).ToList().ForEach(x =>
      {
        x.FileInfo = new FileInfo(id);
        x.FileInfo.Delete();
        x.Exists = false;
      });
    }

    #endregion SetFileInfoError

    #endregion Métodos Públicos
  }
}