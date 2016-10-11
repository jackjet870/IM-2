using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Base.Reports;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.Outhouse.Forms
{
  /// <summary>
  /// Interaction logic for frmReportsOutside.xaml
  /// </summary>
  public partial class frmReportsOutside : Window
  {

    #region Atributos
    public UserLogin User { get; set; }
    private DateTime _date;
    #endregion

    public frmReportsOutside(DateTime selectedDate)
    {
      InitializeComponent();
      DataContext = this;
      User = Context.User.User;
      _date = selectedDate;
    }

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [JORCANCHE] 22/AGO/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [JORCANCHE] 22/AGO/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
          break;
        case Key.Insert:
          KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
          break;
        case Key.NumLock:
          KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
          break;
      }
    } 
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Inicio del formulario.
    /// </summary>
    /// <history>
    /// [jorcanche] 31/ago/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      dtpDate.Value = _date;
    } 
    #endregion

    #region lstReports_Select
    /// <summary>
    /// Exporta el reporte seleccionado a un archivo de excel.
    /// </summary>
    /// <history>
    /// [jorcanche] 31/ago/2016 Created
    /// [emoguel] 08/09/2016 Ahora abre el visor de reportes
    /// </history>
    private async void lstReports_Select(object sender, RoutedEventArgs e)
    {
      if (e.GetType() == typeof(KeyEventArgs) && ((KeyEventArgs)e).Key != Key.Enter) return;
      var filters = new List<Tuple<string, string>>();
      var daterange = DateHelper.DateRange(dtpDate.Value.Value, dtpDate.Value.Value);
      var dateFileName = DateHelper.DateRangeFileName(dtpDate.Value.Value, dtpDate.Value.Value);
      FileInfo fileinfo = null;
      if (lstReports.SelectedItem  == null)
      {
        UIHelper.ShowMessage("You must select a report");
        return;
      }
      switch ((lstReports.SelectedItem as ListBoxItem).Content.ToString())
      {
        case "Deposits by PR":
          var lstRptDepPr = await BRReportsByLeadSource.GetRptDepositByPR(dtpDate.Value.Value, dtpDate.Value.Value, Context.User.LeadSource.lsID);
          if (lstRptDepPr.Any())
          {
            filters.Add(Tuple.Create("Filter Range", daterange));
            filters.Add(Tuple.Create("Lead Source", Context.User.LeadSource.lsID));

            var lstDepPr = lstRptDepPr[0] as List<RptDepositsByPR>;
            var currencies = lstRptDepPr[1] as List<Currency>;
            var payType = lstRptDepPr[2] as List<PaymentType>;

            lstDepPr.ForEach(c => {
              c.guPRInvit1 = $"{c.guPRInvit1} {c.peN}";
              c.gucu = currencies.First(cu => cu.cuID == c.gucu).cuN ?? "";
              c.gupt = payType.First(pt => pt.ptID == c.gupt).ptN ?? "";
            });

            fileinfo = await ReportBuilder.CreateCustomExcelAsync(TableHelper.GetDataTableFromList(lstDepPr, true, false),
              filters, "Deposits by PR", dateFileName, clsFormatReports.RptDepositByPr(), blnShowSubtotal: true, blnRowGrandTotal: true, isPivot: true, addEnumeration: true);
          }
          else
            UIHelper.ShowMessage("There is no data for make a report");          
          break;

      }

      if (fileinfo != null)
      {
        frmDocumentViewer documentViewer = new frmDocumentViewer(fileinfo, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly),false);
        documentViewer.Owner = this;
        documentViewer.ShowDialog();
      }
    } 
    #endregion
  }
}
