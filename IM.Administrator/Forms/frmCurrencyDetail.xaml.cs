using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCurrrencyDetail.xaml
  /// </summary>
  public partial class frmCurrencyDetail : Window
  {
    public Currency currency=new Currency();//objeto para agrear|actualizar
    public Currency oldCurrency = new Currency();//Objeto con los datos iniciales
    public EnumMode mode;//modo en el que se abrirá la ventana add|preview|edit 
    public frmCurrencyDetail()
    {
      InitializeComponent();
    }

    #region event controls
    #region WindowKeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);

      }
    }
    #endregion

    #region Loaded
    /// <summary>
    /// llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(currency, oldCurrency);
      if(mode!=EnumMode.preview)
      {
        txtcuID.IsEnabled = (mode == EnumMode.add);
        txtcuN.IsEnabled = true;
        chkA.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetMaxLength(currency, this);
      }
      DataContext = currency;
    }
    #endregion
    #region Accept

    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(currency,oldCurrency) && mode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string sMsj = ValidateHelper.ValidateForm(this, "Currency");
        int nRes = 0;

        if (sMsj == "")//Validar si hay cmapos vacios
        {

          nRes = BRCurrencies.saveCurrency(currency, (mode == EnumMode.edit));
          UIHelper.ShowMessageResult("Currency", nRes);
          if(nRes==1)
          {
            Close();
          }
          
        }
        else
        {
          UIHelper.ShowMessage(sMsj);
        }
      }
    }

    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana pero antes verifica que no se tengan cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if(mode!=EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(currency, oldCurrency))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            Close();
          }
        }
        else
        {
          Close();
        }
      }
      else
      {
        Close();
      }
    }
    #endregion
    #endregion
    
  }
}
