﻿using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Data;
using System.Linq;
using System.Windows;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;


namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmAddExchangeRate.xaml
  /// </summary>
  public partial class frmAddExchangeRate : Window
  {

    private List<string> _listExchangeRate;
    CollectionViewSource _dsCurrenciesAvailable;
    private decimal _exchangeMEX;
    private bool _boolDecimal = true;

    public frmAddExchangeRate(List<string> listExchangeRateData, decimal exchangeRateMEX)
    {
      _listExchangeRate = listExchangeRateData;
      _exchangeMEX = exchangeRateMEX;

       InitializeComponent();
    }

    /// <summary>
    /// Funcion Load de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 11/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsCurrenciesAvailable = ((CollectionViewSource)(this.FindResource("currencyViewSource")));
        
      _dsCurrenciesAvailable.Source = BRCurrencies.GetCurrencies(null, 1, _listExchangeRate);
    }

    /// <summary>
    /// Función encargado del evento close de window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 11/03/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    /// <summary>
    /// Función encargada de ejecutar el guardado de los datos introducidos por el usuario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 11/03/2016 Created
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      //Validamos el campo Rate
      if (!ValidateTextRate())
      {
        var _exD = (Currency)currencyComboBox.SelectionBoxItem;

        ExchangeRate _exchangeRate = new ExchangeRate
        {
          excu = _exD.cuID,
          exD = frmHost._dtpServerDate.Date,
          exExchRate = Convert.ToDecimal(txtBoxRate.Text),
        };

        //Guardamos el nuevo Exchange Rate Agregado.
        BRExchangeRate.SaveExchangeRate(true,_exchangeRate);

        //Guadarmos el Log del cambio.
        BRExchangeRatesLogs.SaveExchangeRateLog(_exD.cuID, frmHost._dtpServerDate.Date, frmHost._userData.SalesRoom.srHoursDif, frmHost._userData.User.peID);

        // Cerramos la ventana
        Close();
      }

    }

    /// <summary>
    /// Verifica las entradas de texto en el campo rate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/03/2016 Created
    /// </history>
    private void txtBoxRate_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      string _value = e.Text;

      // Se valida que solamente sean Digitos Numericos
      if (!char.IsDigit(Convert.ToChar(_value)) && !_value.Equals(".") || !_boolDecimal && !char.IsDigit(Convert.ToChar(_value)))
      {
        e.Handled = true;
      }
      if (_value.Equals(".") && _boolDecimal)
      {
        _boolDecimal = false;
      }

      // Se verifica que no sea un enter
      if (_value.Equals("\r"))
      {
        ValidateTextRate();
      }
    }

    /// <summary>
    /// Valida que el campo Rate no se encuentre vacio al momento de guardar los cambios
    /// </summary>
    /// <history>
    /// [vipacheco] 12/03/2016 Created
    /// </history>
    private bool ValidateTextRate()
    {
      if (!string.IsNullOrEmpty(txtBoxRate.Text) && !string.IsNullOrWhiteSpace(txtBoxRate.Text))
      {
        txtBoxPesos.Text = string.Format("$ {0}", Convert.ToDecimal(txtBoxRate.Text) / _exchangeMEX);
        return false;
      }
      else
      {
        txtBoxPesos.Text = "";
        UIHelper.ShowMessage("Rate Field Empty", MessageBoxImage.Warning);
        return true;
      }
    }

  }
}