using CrystalDecisions.CrystalReports.Engine;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchDepositRefund.xaml
  /// </summary>
  public partial class frmSearchDepositRefund : Window
  {

    private int _GuestID = 0;

    public frmSearchDepositRefund(int GuestID = 0)
    {
      // Se asigna el GuestID
      _GuestID = GuestID;

      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Load_RefundTypes();

      // Fechas
      dtpStart.Value = frmHost._dtpServerDate.AddDays(-30);
      dtpEnd.Value = frmHost._dtpServerDate;

      if (HasGuest())
      {
        Load_Refund();
        Load_BookingDeposits();
        txtID.Text = $"{_GuestID}";
        Controls_Mode(Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
      }
      else
      {
        Load_Refund();
        Load_BookingDeposits(-1);
        Controls_Mode(Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Visible);
      }

      ChangeLookUpStatus(!HasGuest());
    }

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
    private async void Load_Refund()
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
      List<DepositsRefund> lstRefund = await BRDepositsRefund.GetDepositsRefund(Guest, RefundID, Folio, Name, Reservation, OutInv, PR, dateFrom, dateTo);

      // Asignamos el resultado al grid
      CollectionViewSource _dsDepositsRefund = ((CollectionViewSource)(this.FindResource("dsRefund")));
      _dsDepositsRefund.Source = lstRefund;

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
      int _guestID = 0, _refundID = 0;

      // Si no esta asociado a un Guest
      if (!HasGuest())
        _guestID = GuestID;
      else
        _guestID = _GuestID;

      // Checamos si envio refund
      if (RefundID > 0)
        _refundID = RefundID;

      // Ejecutamos el procedimiento
      List<DepositToRefund> lstResult = await BRBookingDeposits.GetBookingDepositsByGuest(_guestID, _refundID);

      // Cargamos la informacion al Grid
      CollectionViewSource _dsDeposits = ((CollectionViewSource)(this.FindResource("dsDeposits")));
      _dsDeposits.Source = lstResult;

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


      txtFolio.Text = $"{Selected.drFolio}";
      txtID.Text = $"{Selected.guID}";

      Load_BookingDeposits(Selected.guID, Selected.drID);
      Controls_Mode(Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Visible, Visibility.Visible, ValidateGuest:true);

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

    private void Controls_Mode(Visibility _btnNew, Visibility _btnSave, Visibility _btnPrint, Visibility _btnCancel, Visibility _btnClose, Visibility _btnSearch = Visibility.Visible, bool ValidateGuest = false)
    {
      btnNew.Visibility = _btnNew;
      btnSave.Visibility = _btnSave;
      btnPrint.Visibility = _btnPrint;
      btnCancel.Visibility = _btnCancel;
      btnClose.Visibility = _btnClose;

      // solo para cuando se selecciona un row se realiza esta validacion.
      if (ValidateGuest)
      {
        if (HasGuest())
          btnSearch.Visibility = Visibility.Hidden;
        else
          btnSearch.Visibility = Visibility.Visible;
      }
      else
      {
        btnSearch.Visibility = _btnSearch;
      }
    }

    #region Read_Mode
    /// <summary>
    /// Activa los controles correspondientes a modo lectura
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void Read_Mode()
    {
      btnNew.Visibility = Visibility.Visible;
      btnSave.Visibility = Visibility.Hidden;
      btnPrint.Visibility = Visibility.Hidden;
      btnCancel.Visibility = Visibility.Hidden;
      btnClose.Visibility = Visibility.Visible;
      btnSearch.Visibility = Visibility.Hidden;
    }
    #endregion

    #region Search_Mode
    /// <summary>
    /// Activa los controles correspondientes a modo lectura
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void Search_Mode()
    {
      btnNew.Visibility = Visibility.Hidden;
      btnSave.Visibility = Visibility.Hidden;
      btnPrint.Visibility = Visibility.Hidden;
      btnCancel.Visibility = Visibility.Hidden;
      btnClose.Visibility = Visibility.Visible;
      btnSearch.Visibility = Visibility.Visible;
    }

    #endregion

    #region Edit_Mode
    /// <summary>
    /// Activa los controles correspondientes a modo edicion
    /// </summary>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void Edit_Mode()
    {
      btnNew.Visibility = Visibility.Hidden;
      btnSave.Visibility = Visibility.Visible;
      btnPrint.Visibility = Visibility.Hidden;
      btnCancel.Visibility = Visibility.Visible;
      btnClose.Visibility = Visibility.Visible;
      btnSearch.Visibility = Visibility.Hidden;
    }

    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Busca los Deposits Refund con los criterios ingresados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      Load_Refund();
    }
    #endregion

    #region btnNew_Click
    /// <summary>
    /// Agregar un nuevo refund
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      Controls_Mode(Visibility.Hidden, Visibility.Visible, Visibility.Hidden, Visibility.Visible, Visibility.Visible, Visibility.Hidden);

      // Habilitamos la columna
      bdRefundColumn.IsReadOnly = false;
    }
    #endregion

    #region btnClose_Click
    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void grdRefund_DoubleClick(object sender, RoutedEventArgs e)
    {
      Load_Selected();
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// Carga la informacion del refund seleccionado
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (HasGuest())
      {
        Controls_Mode(Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
      }
      else
      {
        Load_BookingDeposits(-1);
        Controls_Mode(Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Visible, Visibility.Visible, ValidateGuest: true);
        btnCancel.Visibility = Visibility.Hidden;
        btnPrint.Visibility = Visibility.Hidden;
        grdRefund.SelectedItem = null;

        txtID.Text = "";
        txtFolio.Text = "";
      }
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
  }
}
