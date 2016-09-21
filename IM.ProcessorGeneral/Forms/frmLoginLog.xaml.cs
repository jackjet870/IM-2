using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.ProcessorGeneral.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace IM.ProcessorGeneral.Forms
{
  /// <summary>
  /// Interaction logic for frmLoginLog.xaml
  /// </summary>
  public partial class frmLoginLog : Window
  {
    #region Atributos
    private List<Location> _locations;
    private List<Personnel> _personnels;
    private List<string> _pcNames;
    CollectionViewSource lstLoginsLog;
    public frmReportQueue frmReportQ;
    #endregion

    #region Constructor
    public frmLoginLog()
    {
      InitializeComponent();
    }
    #endregion

    #region Métodos del Formulario
    #region FrmLoginLog_OnContentRendered
    /// <summary>
    /// Inicializa los combobox.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 27/Abr/2016 Created
    /// </history>
    private void FrmLoginLog_OnContentRendered(object sender, EventArgs e)
    {
      LoadPersonnel();
      LoadPcNames();
      LoadLocations();
    }
    #endregion      

    #region BtnApplyFilter_OnClick
    /// <summary>
    /// Realiza la consulta y obtiene los datos segun los filtros ingresados en el formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 28/Abr/2016 Created
    /// </history>
    private async void BtnApplyFilter_OnClick(object sender, RoutedEventArgs e)
    {
      lstLoginsLog = ((CollectionViewSource)(FindResource("lstRptLoginsLog")));
      biWait.IsBusy = true;
      biWait.BusyContent = "Loading data...";
      lstLoginsLog.Source = await BRGeneralReports.GetRptLoginsLog(dtmStart.Value.Value, dtmEnd.Value.Value,
        cbLocation.SelectedValue.ToString(),
        cbPcName.SelectedValue.ToString(),
       cbPersonnel.SelectedValue.ToString());
      biWait.IsBusy = false;
    }
    #endregion

    #region BtnPrint_OnClick
    /// <summary>
    /// Exporta los datos a un documento de Excel.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 28/Abr/2016 Created
    /// </history>
    private async void BtnPrint_OnClick(object sender, RoutedEventArgs e)
    {
      biWait.IsBusy = true;
      if (dgvLoginsLog.Items.Count <= 0) return;
      biWait.BusyContent = "Loading Report...";

      string message = ValidateFields();

      if (message != "") { UIHelper.ShowMessage(message); return; }

      DataTable dtRptLoginsLog = TableHelper.GetDataTableFromList(lstLoginsLog.View.Cast<RptLoginLog>().ToList(), true,
        false, true);

      List<Tuple<string, string>> filters = new List<Tuple<string, string>>
      {
        Tuple.Create("Date Range",
          DateHelper.DateRange(dtmStart.Value.Value, dtmEnd.Value.Value))
      };

      string strReportName = "Logins Log";
      string dateFileName = DateHelper.DateRangeFileName(dtmStart.Value.Value, dtmEnd.Value.Value);

      string fileFullPath = EpplusHelper.CreateEmptyExcel(strReportName, dateFileName);
      frmReportQ.AddReport(fileFullPath, strReportName);
      try
      {
        
        var finfo = await EpplusHelper.CreateCustomExcel(dtRptLoginsLog, filters, strReportName, string.Empty, clsFormatReport.RptLoginsLog(), fileFullPath: fileFullPath, addEnumeration: true);

        if (finfo == null)
        {
          finfo = EpplusHelper.CreateNoInfoRptExcel(filters, strReportName, fileFullPath);
        }
        frmDocumentViewer frmDocumentViewver = new frmDocumentViewer(finfo,Context.User.HasPermission(EnumPermission.RptExcel,EnumPermisionLevel.ReadOnly));
        frmDocumentViewver.Show();
        frmReportQ.SetExist(finfo.FullName, finfo);

      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
        biWait.IsBusy = false;
      }
      biWait.IsBusy = false;
    }
    #endregion 
    #endregion

    #region Métodos Privados

    #region LoadLocations
    /// <summary>
    /// Carga y configuracion de Locations.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/May/2016 Created
    /// </history>
    private async void LoadLocations()
    {
      _locations = await BRLocations.GetLocationsbyProgram("IH");
      _locations.Insert(0, new Location { loID = "ALL", loN = "ALL" });
      cbLocation.ItemsSource = _locations;
      cbLocation.SelectedIndex = 0;
    }
    #endregion

    #region LoadPcNames
    /// <summary>
    /// Carga y configuracion de PCNames.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/May/2016 Created
    /// </history>
    private async void LoadPcNames()
    {
      _pcNames = await BRLoginLogs.GetLoginsLogPCName();
      _pcNames.Insert(0, "ALL");
      _pcNames.Remove("");
      cbPcName.ItemsSource = _pcNames;
      cbPcName.SelectedIndex = 0;
    }
    #endregion

    #region LoadPersonnel
    /// <summary>
    /// Carga y configuracion de Personnels.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/May/2016 Created
    /// </history>
    private async void LoadPersonnel()
    {
      _personnels = await BRPersonnel.GetPersonnelAccess();
      _personnels.Insert(0, new Personnel { peID = "ALL", peN = "ALL" });
      cbPersonnel.ItemsSource = _personnels;
      cbPersonnel.SelectedIndex = 0;
    }
    #endregion

    #region ValidateFields

    /// <summary>
    /// Valida si los grid tienen al menos un elemento seleccionado.
    /// </summary>
    /// <returns>Message/Empty</returns>
    /// <history>
    /// [edgrodriguez] 30/Jun/2016 Created
    /// </history>
    private string ValidateFields()
    {
      if (dtmEnd.Value.Value < dtmStart.Value.Value)
        return "End date must be greater than start date.";
      else
        return "";
    }

    #endregion ValidateFields 

    #endregion
  }
}
