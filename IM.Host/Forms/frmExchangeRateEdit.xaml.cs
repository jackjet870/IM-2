using System.Windows;
using IM.Model;
using IM.BusinessRules.BR;
using System.Collections.Generic;
using System.Windows.Data;
using System;

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
    private ExchangeRateData _exchangeRateRow;
    List<Currency> _listCurrencies = new List<Currency>();

    #region CONTRUCTOR

    public frmExchangeRateEdit(ExchangeRateData _exchangeRateRow)
    {
      InitializeComponent();
      this.DataContext = _exchangeRateRow;
    }
    #endregion

    #region METODOS DE TIPO BOTON

    #region btnCancel_Click
    /// <summary>
    /// Función encargado de cerrar el Dialogo para editar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    /// Función encargado de guardar la informacion modificada.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      _exchangeRateRow = (ExchangeRateData)this.DataContext;

      ExchangeRate _exchangeRate = new ExchangeRate {
                                                    excu = _exchangeRateRow.excu,
                                                    exD = frmHost._dtpServerDate.Date,
                                                    exExchRate = Convert.ToDecimal(txtRate.Text)
                                                    };

      BRExchangeRate.SaveExchangeRate(false, _exchangeRate);

      //Guadarmos el Log del cambio.
      BRExchangeRatesLogs.SaveExchangeRateLog(_exchangeRateRow.excu, frmHost._dtpServerDate.Date, App.User.SalesRoom.srHoursDif, App.User.User.peID);

      Close();
    }
    #endregion

    #endregion
  }
}
