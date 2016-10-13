using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace IM.Base.Forms
{
  /// <summary>
  /// Formulario que muestra los moviemientos de los Salesmen
  /// </summary>
  /// <history>
  /// [jorcanche]  created 07/07/2016
  /// </history>
  public partial class frmSalesmenChanges : Window
  {
    private string _membership;
    int ID;
    private string _movementType;

    public frmSalesmenChanges(int sale, string membership,string movementType="SL")
    {
      InitializeComponent();
      ID = sale;
      _membership = membership;
      _movementType = movementType;
    }

    /// <summary>
    /// Carga las variables del formulario
    /// </summary>
    /// <history>
    /// [jorcanche]  created 29062016
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Mouse.OverrideCursor = Cursors.Wait;
      if (_movementType == "SL")
      {
        Title = $"Salesmen Changes - Sale ID {ID} / Membership Number {_membership}";
      }
      else
      {
        Title = $"Salesmen Changes - Sale ID {ID}";
      }
      var salesSalesMan = await BRSalesSalesmen.GetSalesmenChanges(ID,_movementType);
      salesmenChangesDataGrid.ItemsSource = salesSalesMan;
      Mouse.OverrideCursor = null;
    }

    /// <summary>
    /// Imprime los datos que se encuentran en el DataGrid de SalesmenChanges
    /// </summary>
    /// <history>
    /// [jorcanche]  created 03/10/2016
    /// </history>
    private async void btnPrintSalesmenChangeslog_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (salesmenChangesDataGrid == null) return;
        if (salesmenChangesDataGrid.Items.Count == 0)
        {
          UIHelper.ShowMessage("There is no info to make a report");
          return;
        }        
        Mouse.OverrideCursor = Cursors.Wait;
        FileInfo fileInfo = await ReportBuilder.CreateCustomExcelAsync(
          TableHelper.GetDataTableFromList((List<SalesmenChanges>)salesmenChangesDataGrid.ItemsSource, true, true, true),
          new List<Tuple<string, string>> { Tuple.Create((_movementType == "SL") ?  "Sale Id" : "Guest Id", ID.ToString()) },
          "Salesmen Changes",
          DateHelper.DateRangeFileName(DateTime.Today, DateTime.Today),
          EpplusHelper.OrderColumns(salesmenChangesDataGrid.Columns.ToList(), ExportReports.RptSalesmenChanges()));
        if (fileInfo != null)
        {
          frmDocumentViewer documentViewver = new frmDocumentViewer(fileInfo, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly), false);
          documentViewver.Owner = this;
          documentViewver.ShowDialog();
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }
  }
}
