using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;
using System;

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
    private bool _isClosing = false;
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
      #region LockControls
      if(mode!=EnumMode.ReadOnly)
      {
        txtcoID.IsEnabled = (mode == EnumMode.Add);
        txtcoN.IsEnabled = true;
        txtcoNationality.IsEnabled = true;
        cmbLang.IsEnabled = true;
        cmbUnvMot.IsEnabled = true;
        chkA.IsEnabled = true;
        chkSil.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetUpControls(country, this);
      }
      #endregion
      loadUnavailableMotives();
      LoadLanguages();
      DataContext = country;
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
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(country, oldCountry) && mode != EnumMode.Add)
        {
          _isClosing = true;
          Close();
        }
        else
        {
          int nRes = 0;
          string sMsj = ValidateHelper.ValidateForm(this, "Country");

          if (sMsj == "")
          {
            nRes = await BREntities.OperationEntity(country, mode);

            UIHelper.ShowMessageResult("Country", nRes);
            if (nRes > 0)
            {
              _isClosing = true;
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
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
      btnCancel.Focus();
      if(mode!=EnumMode.ReadOnly)
      {
        if (!ObjectHelper.IsEquals(country, oldCountry))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!_isClosing) { _isClosing = true; Close(); }
          }
          else
          {
            _isClosing = false;
          }
        }
        else
        {
          if (!_isClosing) { _isClosing = true; Close(); }
        }
      }
      else
      {
        if (!_isClosing) { _isClosing = true; Close(); }
      }
    }
    #endregion

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        _isClosing = true;
        btnCancel_Click(null, null);
        if (!_isClosing)
        {
          e.Cancel = true;
        }
        else
        {
          _isClosing = false;
        }
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
    private async void loadUnavailableMotives()
    {
      try
      {
        List<UnavailableMotive> lstUnavailableMotives = await BRUnavailableMotives.GetUnavailableMotives();
        cmbUnvMot.ItemsSource = lstUnavailableMotives;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region Load Languages
    /// <summary>
    /// Llena el combobox de languages
    /// </summary>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadLanguages()
    {
      List<LanguageShort> lstLanguages =await BRLanguages.GetLanguages(0);
      cmbLang.ItemsSource = lstLanguages;
    }
    #endregion

    #endregion

  }
}
