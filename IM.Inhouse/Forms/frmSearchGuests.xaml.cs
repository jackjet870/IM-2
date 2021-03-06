﻿using IM.Base.Helpers;
using IM.BusinessRules.BR;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.Inhouse.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchGuests.xaml
  /// </summary>
  public partial class frmSearchGuests : Window
  {

    #region Atributos 
    public bool _Cancel
    {
      get; set;
    }
    public string _name
    {
      get
      {
        return (!txtName.Text.Equals(string.Empty) ? txtName.Text : "ALL");
      }
    }
    public string _room
    {
      get
      {
        return (!txtRoomNum.Text.Equals(string.Empty) ? txtRoomNum.Text : "ALL");
      }
    }
    public string _reservation
    {
      get
      {
        return (!txtReservation.Text.Equals(string.Empty) ? txtReservation.Text : "ALL");
      }
    }
    public int _guestID
    {
      get
      {       
         return (!txtguID.Text.Equals(string.Empty)? Convert.ToInt32(txtguID.Text) : 0);       
      }
    }
    public DateTime _dateFrom
    {
      get
      {
        return Convert.ToDateTime(dtpStart.Text);
      }
    }
    public DateTime _dateTo
    {
      get
      {
        return Convert.ToDateTime(dtpEnd.SelectedDate);
      }
    }
    #endregion

    #region Constructores y destructores
    public frmSearchGuests()
    {
      InitializeComponent();

    }
    #endregion

    #region Metodos

    #region ValidateCriteria
    /// <summary>
    /// Valida los campos del formulario
    /// </summary>
    ///<history>[jorcanche] 17/03/2016</history>
    public bool ValidateCriteria()
    {
      // validamos que haya al menos un criterio de busqueda
      if (string.IsNullOrEmpty(txtName.Text) && string.IsNullOrEmpty(txtRoomNum.Text) && string.IsNullOrEmpty(txtguID.Text) && string.IsNullOrEmpty(txtReservation.Text))
      {
        UIHelper.ShowMessage("Please specify a search criteria.", MessageBoxImage.Asterisk, "any criteria");
        txtName.Focus();
        return false;
      }
      else
      {
        return ValidateDateRange();
      }
    }
    #endregion

    #region ValidateDateRange
    /// <summary>
    /// Valida el valor de dtpStart sea mayo que dtpEnd
    /// </summary>
    /// <history>[jorcanche] 17/03/2016</history>

    public bool ValidateDateRange()
    {
      if (dtpStart.SelectedDate.Value > dtpEnd.SelectedDate.Value)
      {
        UIHelper.ShowMessage("Start Date can not be greater than End Date.", MessageBoxImage.Exclamation, "Date");
        DateTime dt = dtpEnd.SelectedDate.Value.AddDays(-1);
        dtpStart.SelectedDate = dt; 
        dtpStart.Focus();
        return false;
      }
      return true;
    }
    #endregion

    #region ValidateValueDate
    /// <summary>
    /// Valida que sea correcta la fecha proporcionada
    /// </summary>
    /// <param name="sender">Objeto de tipo DataPicker</param>
    /// <history>[jorcanche] 17/03/2016</history>
    public void ValidateValueDate(object sender)
    {
      //Obtener el valor actual del que tiene el dtpDate
      var picker = sender as DatePicker;
      if (!picker.SelectedDate.HasValue)
      {
        //Cuando el usuario ingresa una fecha invalida
        UIHelper.ShowMessage("Specify the Date");
        //Y le asignamos la fecha del servidor (la actual hora actual)
        picker.SelectedDate = BRHelpers.GetServerDate();
      }
    }
    #endregion

    #region search
    /// <summary>
    /// Valida la informacion ingresada y luego cierra la ventana 
    /// </summary>
    /// <history>
    /// [jorcanche] 01/04/2016
    /// </history>
    private void search()
    {
      if (ValidateCriteria())
      {
        Close();
        _Cancel = false;
      }
    }
    #endregion

    #region KeyDownEnter
    /// <summary>
    /// Si es precionado la tecla entre inicia las validiaciones
    /// </summary>
    /// <param name="e"></param>
    /// <history>
    /// [jorcanche] 01/04/2016
    /// </history>
    public void KeyDownEnter(KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        search();
      }
    } 
    #endregion

    #endregion

    #region Eventos del Formulario

    #region btnSearch_Click
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      search();
    }
    #endregion

    #region dtpStart_SelectedDateChanged
    private void dtpStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      ValidateValueDate(sender);
    }
    #endregion

    #region dtpEnd_SelectedDateChanged
    private void dtpEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      ValidateValueDate(sender);
    }
    #endregion

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      DateTime dateServer = BRHelpers.GetServerDate();
      //fecha inicial
      dtpStart.SelectedDate = dateServer.AddDays(-7);
      //fecha final
      dtpEnd.SelectedDate = dateServer;
      //Indicamos el focus al txtname
      txtName.Focus();
    }
    #endregion

    #region Window_Closing
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      _Cancel = true;
    }
    #endregion

    #region txtguID_KeyDown
    private void txtguID_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
        e.Handled = false;
      else
        e.Handled = true;
        KeyDownEnter(e);
    }

    #endregion

    #region txtName_KeyDown
    private void txtName_KeyDown(object sender, KeyEventArgs e)
    {
      KeyDownEnter(e);
    }
    #endregion

    #region txtRoomNum_KeyDown
    private void txtRoomNum_KeyDown(object sender, KeyEventArgs e)
    {
      KeyDownEnter(e);
    }

    #endregion

    #region txtReservation_KeyDown
    private void txtReservation_KeyDown(object sender, KeyEventArgs e)
    {
      KeyDownEnter(e);
    }

    #endregion

    #region dtpStart_KeyDown
    private void dtpStart_KeyDown(object sender, KeyEventArgs e)
    {
      KeyDownEnter(e);
    }
    #endregion

    #region dtpEnd_KeyDown
    private void dtpEnd_KeyDown(object sender, KeyEventArgs e)
    {
      KeyDownEnter(e);
    }
    #endregion

    #endregion

  }
}
