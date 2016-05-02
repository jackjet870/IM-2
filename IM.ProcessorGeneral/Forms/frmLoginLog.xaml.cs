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
    public frmLoginLog()
    {
      InitializeComponent();
      _locations= BRLocations.GetLocationsbyProgram("IH");
      _pcNames = BRLoginLogs.GetLoginsLogPCName();
      _personnels = BRPersonnel.GetPersonnelAccess();
    }

    /// <summary>
    /// Inicializa los combobox.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 27/Abr/2016 Created
    /// </history>
    private void FrmLoginLog_OnContentRendered(object sender, EventArgs e)
    {
      //Agregamos valores null
      _locations.Insert(0, new Location {loID = ""});
      _pcNames.Insert(0, "");
      //_personnels.Insert(0, new Personnel {peID = ""});

      cbLocation.ItemsSource = _locations;
      cbPcName.ItemsSource = _pcNames;
      cbPersonnel.ItemsSource = _personnels;

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
    private void BtnApplyFilter_OnClick(object sender, RoutedEventArgs e)
    {
      lstLoginsLog = ((CollectionViewSource) (FindResource("lstRptLoginsLog")));
      lstLoginsLog.Source = BRGeneralReports.GetRptLoginsLog(dtmStart.SelectedDate.Value, dtmEnd.SelectedDate.Value,
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

      Process.Start(EpplusHelper.CreateGeneralRptExcel(filters, dtRptLoginsLog, "Logins Log",
        DateHelper.DateRangeFileName(dtmStart.SelectedDate.Value, dtmEnd.SelectedDate.Value),
        clsFormatReport.RptLoginsLog()).FullName);
    }
  }
}
