using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model.Enums;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchDepositRefund.xaml
  /// </summary>
  public partial class frmSearchDepositRefund : Window
  {

    private int _GuestID = 0;
    private DataGridCellInfo _currentCell;
    private List<DepositsRefund> _lstRefund;

    public List<DepositToRefund> lstDeposits;
    public bool HasRefund = false; // Variable utilizada para saber si existen depositos

    #region Contructor
    /// <summary>
    /// Contructor
    /// </summary>
    /// <param name="GuestID"></param>
    public frmSearchDepositRefund(int GuestID = 0)
    {
      // Se asigna el GuestID
      _GuestID = GuestID;

      InitializeComponent();
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// 
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Load_RefundTypes();
      cborefundType.SelectedValue = "CS";

      // Fechas
      dtpStart.Value = frmHost.dtpServerDate.AddDays(-30);
      dtpEnd.Value = frmHost.dtpServerDate;

      if (HasGuest())
      {
        await Load_Refund();
        //Load_BookingDeposits();
        txtID.Text = $"{_GuestID}";
      }
      else
      {
        Controls_Mode(false, false, false, false, true);

      }

      ChangeLookUpStatus(!HasGuest());
    }
    #endregion

    #region Load_RefundTypes
    /// <summary>
    /// Carga la lista de Refund Types
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void Load_RefundTypes()
    {
      CollectionViewSource _dsRefundTypes = ((CollectionViewSource)(this.FindResource("dsRefundTypes")));
      _dsRefundTypes.Source = frmHost._lstRefundTypes;
    }
    #endregion

    #region Load_Refund
    /// <summary>
    /// Carga los datos de deposits refund en el grid
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private async Task Load_Refund()
    {
      int Guest = 0, RefundID = 0;
      string Folio = "", Name = "", Reservation = "", OutInv = "", PR = "";
      DateTime? dateFrom = null, dateTo = null;

      // Si no esta asociado a un Guest
      if (!HasGuest())
      {
        // Criterio de busqueda
        Guest = string.IsNullOrEmpty(txtguID.Text) ? 0 : Convert.ToInt32(txtguID.Text);
        RefundID = string.IsNullOrEmpty(txtRefundID.Text) ? 0 : Convert.ToInt32(txtRefundID.Text);
        Folio = string.IsNullOrEmpty(txtRefundFolio.Text) ? "" : txtRefundFolio.Text;
        Name = string.IsNullOrEmpty(txtName.Text) ? "" : txtName.Text;
        Reservation = string.IsNullOrEmpty(txtReservation.Text) ? "" : txtReservation.Text;
        OutInv = string.IsNullOrEmpty(txtOutInvt.Text) ? "" : txtOutInvt.Text;
        PR = string.IsNullOrEmpty(txtPR.Text) ? "" : txtPR.Text;
        dateFrom = dtpStart.Value.Value.Date;
        dateTo = dtpEnd.Value.Value.Date;
      }
      else
      {
        Guest = _GuestID;
      }
      // Ejecutamos el procedimiento
      _lstRefund = await BRDepositsRefund.GetDepositsRefund(Guest, RefundID, Folio, Name, Reservation, OutInv, PR, dateFrom, dateTo);

      // Asignamos el resultado al grid
      CollectionViewSource _dsDepositsRefund = ((CollectionViewSource)(this.FindResource("dsRefund")));
      _dsDepositsRefund.Source = _lstRefund;

      Load_BookingDeposits();

    }
    #endregion

    #region Load_BookingDeposits
    /// <summary>
    /// Carga los booking Deposits
    /// </summary>
    /// <param name="GuestID"></param>
    /// <param name="RefundID"></param>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private async void Load_BookingDeposits(int GuestID = -1, int RefundID = -1)
    {
      //if (_lstRefund.Count > 0)
      //{
        int guestID = 0, refundID = 0, folio = 0;

      // Si no esta asociado a un Guest
      if (!HasGuest())
        {
          // Obtenemos el selected del grid
          if (grdRefund.SelectedItem == null && grdRefund.Items.Count > 0)
          {
            grdRefund.SelectedIndex = 0;
          }
          DepositsRefund selected = grdRefund.SelectedItem as DepositsRefund;
          guestID = selected.guID;
          refundID = selected.drID;
          folio = selected.drFolio;
        }
      else
          guestID = _GuestID;

      // Checamos si envio refund
      if (RefundID > 0)
          refundID = RefundID;

        txtFolio.Text = $"{folio}";
        txtID.Text = $"{guestID}";

      // Ejecutamos el procedimiento
      List<DepositToRefund> lstResult = await BRBookingDeposits.GetDepositsToRefund(guestID, refundID);

      // Cargamos la informacion al Grid
      CollectionViewSource _dsDeposits = ((CollectionViewSource)(this.FindResource("dsDeposits")));
        _dsDeposits.Source = lstDeposits;

        // Verificamos si aun quedan Depositos por guardar
        var depositsID = grdDeposits.Items.Cast<DepositToRefund>().ToList();

        if (depositsID.Any(x => !x.bdRefund.Value))
          Controls_Mode(true, false, false, false, true, false);
        else
          Controls_Mode(false, false, depositsID.Any(x => x.bdRefund.Value) ? true : false, false, true, ValidateGuest: true);
      //}
    }
    #endregion

    #region Load_Selected
    /// <summary>
    /// Carga el item seleccionado
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void Load_Selected()
    {
      DepositsRefund Selected = grdRefund.SelectedItem as DepositsRefund;
      if (Selected == null)
      {
        UIHelper.ShowMessage("Select at least one Refund.", MessageBoxImage.Information, "Deposits Refund");
        return;
      }

      Load_BookingDeposits(Selected.guID, Selected.drID);
    }
    #endregion

    #region HasGuest
    /// <summary>
    /// Valida si se envio el GuestID
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private bool HasGuest()
    {
      if (_GuestID == 0)
        return false;
      else
        return true;
    }
    #endregion

    #region ChangeLookUpStatus
    /// <summary>
    /// Habilita o Deshabilita las cajas de texto necesarias
    /// </summary>
    /// <param name="Enable"></param>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void ChangeLookUpStatus(bool Enable)
    {
      txtRefundID.IsReadOnly = !Enable;
      txtRefundFolio.IsReadOnly = !Enable;
      txtName.IsReadOnly = !Enable;
      txtguID.IsReadOnly = !Enable;
      txtReservation.IsReadOnly = !Enable;
      txtOutInvt.IsReadOnly = !Enable;
      txtPR.IsReadOnly = !Enable;
      txtRefundFolio.IsReadOnly = !Enable;
      dtpStart.IsEnabled = Enable;
      dtpEnd.IsEnabled = Enable;
      cborefundType.IsEnabled = !Enable;
    }
    #endregion

    #region Controls_Mode
    /// <summary>
    /// Habilita o Deshabilita los botones del formulario
    /// </summary>
    /// <param name="pNew"></param>
    /// <param name="pSave"></param>
    /// <param name="pPrint"></param>
    /// <param name="pCancel"></param>
    /// <param name="pClose"></param>
    /// <param name="pSearch"></param>
    /// <param name="ValidateGuest"></param>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void Controls_Mode(bool pNew, bool pSave, bool pPrint, bool pCancel, bool pClose, bool pSearch = true, bool ValidateGuest = false)
    {
      btnNew.IsEnabled = pNew;
      btnSave.IsEnabled = pSave;
      btnPrint.IsEnabled = pPrint;
      btnCancel.IsEnabled = pCancel;
      btnClose.IsEnabled = pClose;

      // solo para cuando se selecciona un row se realiza esta validacion.
      if (ValidateGuest)
      {
        if (HasGuest())
          btnSearch.IsEnabled = false;
        else
          btnSearch.IsEnabled = true;
      }
      else
      {
        btnSearch.IsEnabled = pSearch;
      }
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Busca los Deposits Refund con los criterios ingresados
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private async void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      // Validamos las fechas
      if (DateHelper.ValidateValueDate(dtpStart, dtpEnd))
      {
        await Load_Refund();
    }
    }
    #endregion

    #region btnNew_Click
    /// <summary>
    /// Agregar un nuevo refund
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      Controls_Mode(false, true, false, true, true, false);

      // eliminamos los ya marcados
      var lstChecked = grdDeposits.Items.Cast<DepositToRefund>().ToList().Where(x => x.bdRefund == true).ToList();
      lstChecked.ForEach(x => lstDeposits.Remove(x));
      grdDeposits.Items.Refresh();

      // Habilitamos la columna
      bdRefundColumn.IsReadOnly = false;
    }
    #endregion

    #region btnClose_Click
    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    #endregion

    #region grdRefund_DoubleClick
    /// <summary>
    /// Función encargada de Cargar la informacion de acuerdo al refund seleccionado!
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void grdRefund_DoubleClick(object sender, RoutedEventArgs e)
    {
      Load_BookingDeposits();
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// Carga la informacion del refund seleccionado
    /// cambia de fila con el boton tab
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            DepositsRefund Selected = grdRefund.SelectedItem as DepositsRefund;
            Load_BookingDeposits(Selected.guID, Selected.drID);

            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }

    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cancela la seleccion actual del Grid
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (HasGuest())
      {
        // Verificamos si aun quedan Depositos por guardar
        var depositsID = grdDeposits.Items.Cast<DepositToRefund>().ToList();

        Load_BookingDeposits();
        if (depositsID.Any(x => !x.bdRefund.Value))
          Controls_Mode(true, false, false, false, true, false);
        else
          Controls_Mode(false, false, true, false, true, false);
      }
      else
      {
        Load_BookingDeposits(-1);
        Controls_Mode(false, false, true, true, true, ValidateGuest: true);
        btnCancel.IsEnabled = false;
        btnPrint.IsEnabled = false;
        grdRefund.SelectedItem = null;

        txtID.Text = "";
        txtFolio.Text = "";
      }
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    /// Guarda el refund
    /// </summary>
    /// <history>
    /// [vipacheco] 21/Julio/2016 Created
    /// </history>
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos los Deposit Autorizados
      List<int> depositsID = grdDeposits.Items.Cast<DepositToRefund>().ToList().Where(x => x.bdRefund == true).Select(s => s.bdID).ToList();
      string deposits = string.Join(",", depositsID);

      if (string.IsNullOrEmpty(deposits))
      {
        UIHelper.ShowMessage("Mark at least one deposit to create a refund.", MessageBoxImage.Information);
        return;
      }

      if (UIHelper.ShowMessage("Are you sure you want to refund this deposits? \r\n This change can not be undone.", MessageBoxImage.Question) == MessageBoxResult.Yes)
      {
        await Save(deposits);
      }
      await Load_Refund();
    }
    #endregion

    #region btnPrint_Click
    /// <summary>
    /// Imprime el reporte RefundsLetter
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Jul/2016 Created
    /// </history>
    private async void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      var Refund = grdRefund.SelectedItem as DepositsRefund;
      if (Refund != null)
      {
        var RptRefundLetter = await BRDepositsRefund.GetRptRefundLetter(Refund.drID);
        if (RptRefundLetter.Any())
        {
          var objRptRefundletter = RptRefundLetter[0].Cast<RptRefundLetter>().FirstOrDefault();
          var reportText = await BRReportsTexts.GetReportTexts("RefundLetter", objRptRefundletter.gula);

          var header = reportText[0].reText
            .Replace("[SaleRoom]", objRptRefundletter.SaleRoom)
            .Replace("[RefundDate]", objRptRefundletter.RefundDate.ToString())
            .Replace("[RefundID]", objRptRefundletter.RefundID.ToString())
            .Replace("[RefundFolio]", objRptRefundletter.RefundFolio.ToString())
            .Replace("[GuestID]", objRptRefundletter.GuestID.ToString())
            .Replace("[OutInvt]", objRptRefundletter.OutInvt)
            .Replace("[GuestNames]", objRptRefundletter.GuestNames)
            .Replace("[TotalAmount]", objRptRefundletter.TotalAmount.ToString("#,##0.00"))
            .Replace("[PRName]", objRptRefundletter.PRName);
          var footer = reportText[1].reText;

          var crptRefundLetter = new Reports.rptRefundLetter();
          crptRefundLetter.Database.Tables[0].SetDataSource(ObjectHelper.ObjectToList(objRptRefundletter));
          crptRefundLetter.Database.Tables[1].SetDataSource(ObjectHelper.ObjectToList(new Classes.objReportText { Header = header, Footer = footer }));
          crptRefundLetter.Subreports[0].SetDataSource(RptRefundLetter[1].Cast<RptRefundLetter_BookingDeposit>().ToList());

          CrystalReportHelper.SetLanguage(crptRefundLetter, objRptRefundletter.gula);

          //(crptRefundLetter.ReportDefinition.ReportObjects["txtHeader"] as FieldObject).ObjectFormat. = header;
          //(crptRefundLetter.ReportDefinition.ReportObjects["txtFooter"] as FieldObject).Text = footer;

          var frmReportViewer = new frmViewer(crptRefundLetter);
          frmReportViewer.Show();
        }
      }
      else
        UIHelper.ShowMessage("Select a Refund from the list.");
    } 
    #endregion

    #region Save
    /// <summary>
    /// Realiza el guardado total del proceso
    /// Actualiza los booking deposits, genera el depositrefund y lo asocia a los bookingdeposits
    /// Aumenta el contados de folios
    /// </summary>
    /// <param name="pDeposits"></param>
    /// <history>
    /// [vipacheco] 21/Junio/2016 Created
    /// </history>
    private async Task Save(string pDeposits)
    {
      // Obtenemos el folio a crear
      int folio = await BRRefundTypeFolios.GetRefundFolio(cborefundType.SelectedValue.ToString());

      // guardamos la devolucion de depositos y marcamos los depositos como devueltos
      await BRDepositsRefund.SaveDepositsRefund(Convert.ToInt32(txtID.Text), folio, cborefundType.SelectedValue.ToString(), pDeposits);

      // actualizamos el folio usado
      await BRRefundTypeFolios.UpdateRefundFolio(cborefundType.SelectedValue.ToString(), folio);

      HasRefund = true;
  }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica si existe algun deposito refund guardado.
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Julio/2016 Created
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      // Verificamos si esta en modo edicion
      if (btnSave.IsEnabled)
      {
        UIHelper.ShowMessage("This form is currently in edit mode. Please save or cancel your changes before closing it.", MessageBoxImage.Information, "Intelligence Marketing");
        e.Cancel = true;
}
      else
      {
        // Verificamos si aun quedan Depositos por guardar
        var depositsID = grdDeposits.Items.Cast<DepositToRefund>().ToList();

        if (depositsID.Any(x => x.bdRefund.Value))
        {
          HasRefund = true;
        }
        else
        {
          HasRefund = false;
        }
      }
    }
    #endregion

    #region grdDeposits_PreparingCellForEdit
    /// <summary>
    /// valida la celda
    /// </summary>
    /// <history>
    /// [vipacheco] 29/Julio/2016 Created
    /// </history>
    private void grdDeposits_PreparingCellForEdit(object sender, System.Windows.Controls.DataGridPreparingCellForEditEventArgs e)
    {
      DataGrid dataGrid = sender as DataGrid;
      DepositToRefund selectedDeposit = dataGrid.Items.CurrentItem as DepositToRefund;
      _currentCell = grdDeposits.CurrentCell;

      switch (_currentCell.Column.SortMemberPath)
      {
        case "bdRefund":
          if ((bool)selectedDeposit.bdRefund)
          {
            _currentCell.Column.IsReadOnly = true;
          }
          break;
      }
    }
    #endregion

    #region grdDeposits_CellEditEnding
    /// <summary>
    /// Habilita la celda para su verificacion.
    /// </summary>
    /// <history>
    /// [vipacheco] 29/Julio/2016 Created
    /// </history>
    private void grdDeposits_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      _currentCell.Column.IsReadOnly = false;
    } 
    #endregion

  }
}
