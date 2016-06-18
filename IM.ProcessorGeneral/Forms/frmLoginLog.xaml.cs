using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.ProcessorGeneral.Classes;
using IM.Base.Forms;

namespace IM.ProcessorGeneral.Forms
{
  /// <summary>
  /// Interaction logic for frmLoginLog.xaml
  /// </summary>
  public partial class frmLoginLog : Window
  {
    private List<Location> _locations;
    private List<Personnel> _personnels;
    private List<string> _pcNames;
    CollectionViewSource lstLoginsLog;
    public frmReportQueue frmReportQ;
    public frmLoginLog()
    {
      InitializeComponent();
    }

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

    /// <summary>
    /// Asigna un valor al combobox Personnel seleccionando el usuario
    /// con el ID que se haya capturado en el textbox.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 27/Abr/2016 Created
    /// </history>
    private void TxtPeId_OnLostFocus(object sender, RoutedEventArgs e)
    {
      if (txtPeId.Text == "")
      {
        cbPersonnel.SelectedIndex = -1;
        return;
      }

      if (_personnels.Exists(c => c.peID == txtPeId.Text))
        cbPersonnel.SelectedValue = txtPeId.Text;
      else
      {
        UIHelper.ShowMessage("User ID does not exists.");
        txtPeId.Text = "";
        cbPersonnel.SelectedIndex = -1;
      }
    }

    /// <summary>
    /// Asigna el ID del personal a la caja de texto.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 27/Abr/2016 Created
    /// </history>
    private void CbPersonnel_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (cbPersonnel.SelectedIndex > 0)
      {
        txtPeId.Text = cbPersonnel.SelectedValue.ToString();
      }
    }

    /// <summary>
    /// Realiza la consulta y obtiene los datos segun los filtros ingresados en el formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 28/Abr/2016 Created
    /// </history>
    private async void BtnApplyFilter_OnClick(object sender, RoutedEventArgs e)
    {
      lstLoginsLog = ((CollectionViewSource) (FindResource("lstRptLoginsLog")));
      lstLoginsLog.Source = await BRGeneralReports.GetRptLoginsLog(dtmStart.SelectedDate.Value, dtmEnd.SelectedDate.Value,
        (cbLocation.Text != "") ? cbLocation.SelectedValue.ToString() : "ALL",
        (cbPcName.Text != "") ? cbPcName.SelectedValue.ToString() : "ALL",
        (cbPersonnel.Text != "") ? cbPersonnel.SelectedValue.ToString() : "ALL");
    }

    /// <summary>
    /// Exporta los datos a un documento de Excel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnPrint_OnClick(object sender, RoutedEventArgs e)
    {
      if (dgvLoginsLog.Items.Count <= 0) return;

      DataTable dtRptLoginsLog = TableHelper.GetDataTableFromList(lstLoginsLog.View.Cast<RptLoginLog>().ToList(), true,
        false, true);

      List<Tuple<string, string>> filters = new List<Tuple<string, string>>
      {
        Tuple.Create("Date Range",
          DateHelper.DateRange(dtmStart.SelectedDate.Value, dtmEnd.SelectedDate.Value))
      };

      string strReportName = "Logins Log";
      string dateFileName = DateHelper.DateRangeFileName(dtmStart.SelectedDate.Value, dtmEnd.SelectedDate.Value);

      string fileFullPath = EpplusHelper.CreateEmptyExcel(strReportName, dateFileName);
      frmReportQ.AddReport(fileFullPath, strReportName);
      try
      {
        var finfo = EpplusHelper.CreateGeneralRptExcel(filters, dtRptLoginsLog, strReportName, "", clsFormatReport.RptLoginsLog(), fileFullPath: fileFullPath);

        if (finfo == null)
        {
          finfo = EpplusHelper.CreateNoInfoRptExcel(filters, strReportName, fileFullPath);
        }
        frmReportQ.SetFileInfo(fileFullPath, finfo);
      }
      catch (Exception ex)
      {
        frmReportQ.SetFileInfoError(fileFullPath);
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error);
      }
    }

    /// <summary>
    /// Carga y configuracion de Locations.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/May/2016 Created
    /// </history>
    private async void LoadLocations()
    {
      _locations= await BRLocations.GetLocationsbyProgram("IH");
      _locations.Insert(0, new Location { loID = "" });
      cbLocation.ItemsSource = _locations;
    }

    /// <summary>
    /// Carga y configuracion de PCNames.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/May/2016 Created
    /// </history>
    private async void LoadPcNames()
    {
      _pcNames = await BRLoginLogs.GetLoginsLogPCName();
      _pcNames.Insert(0, "");
      cbPcName.ItemsSource = _pcNames;
    }

    /// <summary>
    /// Carga y configuracion de Personnels.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/May/2016 Created
    /// </history>
    private async void LoadPersonnel()
    {
      _personnels = await BRPersonnel.GetPersonnelAccess();
      cbPersonnel.ItemsSource = _personnels;
    }
  }
}
