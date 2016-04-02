using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Administrator.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCountryDetail.xaml
  /// </summary>
  public partial class frmCountryDetail : Window
  {
    public EnumMode mode;//modo en el que se abrirá la ventana
    public Country country = new Country();//objeto con los datos del formulario
    public Country oldCountry = new Country();//Objeto con los datos iniciales

    public frmCountryDetail()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region Window Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(country, oldCountry);
      openMode();
      loadUnavailableMotives();
      LoadLanguages();
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana al presionar el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
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

    #region Boton aceptar
    /// <summary>
    /// Guarda|Actualiza un registro dependiendo del modo en que se abrió
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(country,oldCountry)&& mode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        int nRes = 0;
        string sMsj = ValidateHelper.ValidateForm(this, "Country");

        if (sMsj == "")
        {
          nRes = BRCountries.SaveCountry(country, (mode == EnumMode.edit));

          UIHelper.ShowMessageResult("Country", nRes);
          if(nRes==1)
          {
            DialogResult = true;
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
        if (!ObjectHelper.IsEquals(country, oldCountry))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Warning, "Closing window", MessageBoxButton.OKCancel);
          if (result == MessageBoxResult.OK)
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

    #region Métodos
    #region Load Unavailable motive
    /// <summary>
    /// Carga el combobox de unavailable motives
    /// </summary>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// </history>
    private void loadUnavailableMotives()
    {
      List<UnavailableMotive> lstUnavailableMotives = BRUnavailableMotives.GetUnavailableMotives();
      cmbUnvMot.ItemsSource = lstUnavailableMotives;
    }
    #endregion

    #region Load Languages
    /// <summary>
    /// Llena el combobox de languages
    /// </summary>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// </history>
    private void LoadLanguages()
    {
      List<LanguageShort> lstLanguages = BRLanguages.GetLanguages(0);
      cmbLang.ItemsSource = lstLanguages;
    }
    #endregion

    #region Open Mode
    /// <summary>
    /// Varifica en que modo se abrió la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// </history>
    private void openMode()
    {
      DataContext = country;
      switch(mode)
      {
        case EnumMode.add:
          {
            txtID.IsEnabled = true;
            cmbUnvMot.SelectedIndex = -1;
            lockControls(true);
            break;
          }
        case EnumMode.edit:
          {
            txtID.IsEnabled = false;
            lockControls(true);
            break;
          }

        case EnumMode.preview:
          {
            btnAccept.Visibility = Visibility.Hidden;
            break;
          }
      }
    }
    #endregion

    #region Lock Controls
    /// <summary>
    /// BLoquea| Desbloquea los controles
    /// </summary>
    /// <param name="blnValue"></param>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// </history>
    private void lockControls(bool blnValue)
    {
      txtN.IsEnabled = blnValue;
      txtNat.IsEnabled = blnValue;
      chkA.IsEnabled = blnValue;
      chkSil.IsEnabled = blnValue;
      cmbLang.IsEnabled = blnValue;
      cmbUnvMot.IsEnabled = blnValue;
    }
    #endregion

    #endregion
    
  }
}
