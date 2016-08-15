using System.Windows;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using System;
using System.Collections.Generic;
using IM.Base.Helpers;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmExchangeRateEdit.xaml
  /// </summary>
  /// <history>
  /// [vipacheco] 03/14/2016 Created
  /// </history>
  public partial class frmExchangeRateEdit : Window
  {
    #region Variables
    // Variable que indica el modo de apertura de la ventana
    private EnumMode _modeOpen;
    // Variable llenado cuando esta en modo Edition
    public ExchangeRateData exchangeDate;
    // Variable llenado cuando esta en modo Add
    public List<Currency> lstCurrencies; 
    #endregion

    #region CONTRUCTOR
    /// <summary>
    /// 
    /// </summary>
    /// <param name="modeOpen">Modo de apertura de la ventana</param>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Modified -> Se eliminaron parametros y se optimizo el contructor
    /// </history>
    public frmExchangeRateEdit(EnumMode modeOpen)
    {
      InitializeComponent();
      _modeOpen = modeOpen;
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga el formulario de acuerdo al modoOpen
    /// </summary>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      switch (_modeOpen)
      {
        case EnumMode.Add:
          cboCurrencies.IsEnabled = true;
          // Cargamos las lista de currencies faltantes por agregar
          cboCurrencies.ItemsSource = lstCurrencies;
          cboCurrencies.SelectedIndex = 0;
          break;
        case EnumMode.Edit:
          cboCurrencies.ItemsSource = frmHost._lstCurrencies;
          cboCurrencies.SelectedIndex = frmHost._lstCurrencies.IndexOf(frmHost._lstCurrencies.Find(x => x.cuID == exchangeDate.excu));
          // cargamos la informacion con el ExchangeData a editar
          DataContext = exchangeDate;
          break;
      }
    }
    #endregion

    #region METODOS DE TIPO BOTON

    #region btnCancel_Click
    /// <summary>
    /// Función encargado de cerrar el Dialogo para editar
    /// </summary>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    /// Función encargado de guardar la informacion modificada.
    /// </summary>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      var exchangeNew = DataContext as ExchangeRateData;

      ExchangeRate _exchangeRate = new ExchangeRate {
                                                    excu = exchangeNew.excu,
                                                    exD = frmHost.dtpServerDate.Date,
                                                    exExchRate = Convert.ToDecimal(txtRate.Text)
                                                    };

      //Guardamos el nuevo Exchange Rate Agregado.  // por el momento el SaveLog se encuentra en la transaccion BRExchangeRate.SaveExchangeRate
      BRExchangeRate.SaveExchangeRate(_modeOpen == EnumMode.Add ? true : false, _exchangeRate, exchangeNew.excu, frmHost.dtpServerDate.Date, App.User.SalesRoom.srHoursDif, App.User.User.peID);

      //Guadarmos el Log del cambio.
      //BRExchangeRatesLogs.SaveExchangeRateLog(exchangeNew.excu, frmHost.dtpServerDate.Date, App.User.SalesRoom.srHoursDif, App.User.User.peID);

      DialogResult = true;
      // Cerramos la ventana.
      Close();
    }
    #endregion

    #endregion

    #region txtRate_PreviewTextInput
    /// <summary>
    /// Verifica que las entradas de texto en el campo rate sean del tipo decimal
    /// </summary>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Created
    /// </history>
    private void txtRate_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyDecimals(e.Text, sender as TextBox);
    }
    #endregion

    #region txtRate_PreviewKeyDown
    /// <summary>
    /// Actualiza el peso cuando se le pulsa enter
    /// </summary>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Created
    /// </history>
    private void txtRate_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        ValidateTextRate();
      }
    }
    #endregion

    #region txtRate_LostFocus
    /// <summary>
    /// Actualiza el peso cuando se pierde el foco
    /// </summary>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Created
    /// </history>
    private void txtRate_LostFocus(object sender, RoutedEventArgs e)
    {
      ValidateTextRate();
    } 
    #endregion

    #region ValidateTextRate
    /// <summary>
    /// Valida que el campo Rate no se encuentre vacio al momento de guardar los cambios
    /// </summary>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Created
    /// </history>
    private bool ValidateTextRate()
    {
      if (!string.IsNullOrEmpty(txtRate.Text) && !string.IsNullOrWhiteSpace(txtRate.Text))
      {
        txtPesos.Text = string.Format("$ {0}", Convert.ToDecimal(txtRate.Text) / frmExchangeRate._exchangeRateMEX);
        return false;
      }
      else
      {
        txtRate.Text = "";
        UIHelper.ShowMessage("Rate Field Empty", MessageBoxImage.Warning);
        return true;
      }
    } 
    #endregion

  }
}
