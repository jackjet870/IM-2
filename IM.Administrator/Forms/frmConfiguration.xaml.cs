using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Formulario con el regitro unico de los parametros globales del sistema
  /// Interaction logic for frmConfiguration.xaml
  /// </summary>
  /// <history>
  ///   [vku] 13/Jun/2016 Created
  /// </history>
  public partial class frmConfiguration : Window
  {
    public frmConfiguration()
    {
      InitializeComponent();
    }

    #region Metodos

    #region LoadConfigurations
    /// <summary>
    ///  Carga el unico registro de configuration
    /// </summary>
    /// <history>
    ///   [vku] 13/Jun/2016 Created
    /// </history>
    protected async void LoadConfigurations()
    {
      try
      {
        status.Visibility = Visibility.Visible;
        List<Configuration> lstConfigurations = await BRConfiguration.GetConfigurations();
        dgrConfiguracion.ItemsSource = lstConfigurations;
        StatusBarReg.Content = lstConfigurations.Count + " Configuration.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Configuration");
      }
    }
    #endregion

    #endregion

    #region eventos

    #region Window_Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 13/Jun/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadConfigurations();
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 13/Jun/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            break;
          }
      }
    }
    #endregion

    #region btnRef_Click
    /// <summary>
    /// Recarga los datos del grid
    /// </summary>
    /// <history>
    ///   [vku] 13/Jun/2016 Created
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadConfigurations();
    }
    #endregion

    #region Cell_DoubleClick
    /// <summary>
    ///   Llena el grid con el unico registro de configuracion
    /// </summary>
    /// <history>
    ///   [vku] 14/Jun/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Configuration configuration = (Configuration)dgrConfiguracion.SelectedItem;
      frmConfigurationDetail frmConfigurationDetail = new frmConfigurationDetail();
      frmConfigurationDetail.oldConfigurations = configuration;
      frmConfigurationDetail.Owner = this;
      frmConfigurationDetail.mode = EnumMode.edit;
      if (frmConfigurationDetail.ShowDialog() == true)
      {
        int nIndex = 0;
        List<Configuration> lstConfigurations = (List<Configuration>)dgrConfiguracion.ItemsSource;//cateamos el itemsource
        ObjectHelper.CopyProperties(configuration, frmConfigurationDetail.configurations);
        nIndex = lstConfigurations.IndexOf(configuration);
       
        dgrConfiguracion.Items.Refresh();
        StatusBarReg.Content = lstConfigurations.Count + " Configurations.";//Actualizamos el contador
        GridHelper.SelectRow(dgrConfiguracion, nIndex);
      }
    }
    #endregion

    #region Row_KeyDown
    /// <summary>
    ///   Abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 14/Jun/2016 Created
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            Cell_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }
    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 13/Jun/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #endregion
  }
}
