using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using System;
using IM.Model.Helpers;
using System.Windows.Controls;
using System.Linq;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Formulario para configurar los parametros globales del sistema
  /// Interaction logic for frmConfigurationDetail.xaml
  /// </summary>
  /// <history>
  ///   [vku] 14/Jun/2016 Created
  /// </history>
  public partial class frmConfigurationDetails : Window
  {
    #region atributos
    public Configuration oldConfigurations = new Configuration();//Objeto con valores iniciales
    public Configuration configurations = new Configuration();//Objeto para llenar el formulario
    public EnumMode mode = EnumMode.edit;
    private bool _isClosing = false;
    #endregion

    public frmConfigurationDetails()
    {
      InitializeComponent();
    }

    #region eventos

    #region Window_Loaded
    /// <summary>
    ///   Carga los datos del formularion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 14/Jun/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadConfigurations();
      LoadWeekDays();
      LoadPersonelAdmin();
      LoadTourTimesSchema();
      LoadPersonelBoss();
      
      if (mode != EnumMode.preview)
      {
        btnAccept.Visibility = Visibility.Visible;
        txtocDBVersion.IsEnabled = true;
        cboocWeekDayStart.IsEnabled = true;
        cboocAdminUser.IsEnabled = true;
        txtocWelcomeCopies.IsEnabled = true;
        cboocTourTimesSchema.IsEnabled = true;
        cboocBoss.IsEnabled = true;
        txtocVATRate.IsEnabled = true;
        UIHelper.SetUpControls(configurations, this);
      }   
      skpStatus.Visibility = Visibility.Collapsed;
    }
    #endregion

    #region Window_Closing
    /// <summary>
    ///   Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 14/Jun/2016 Created
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

    #region btnCancel_Click
    /// <summary>
    ///   Cierra la ventana pero antes verifica que no se tengan cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 14/Jun/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if (mode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(configurations, oldConfigurations))
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

    #region btnAccept_Click
    /// <summary>
    ///   Guarda o actualiza el registro dependiendo del modo en que se abrió
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 14/Jun/2016 Created
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(configurations, oldConfigurations) && !Validation.GetHasError(txtocWelcomeCopies))
        {
          return;
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          string sMsj = ValidateHelper.ValidateForm(this, "Configuration");
          if (!string.IsNullOrWhiteSpace(txtocWelcomeCopies.Text.Trim()))
          {
            int nRes = Convert.ToInt32(txtocWelcomeCopies.Text.Trim());
            if (nRes > 255 || nRes < 1)
            {
              sMsj += ((sMsj == "") ? "" : " \n") + "Welcome Copies is out of range. Allowed values are 1 to 255.";
            }
          }
          if (sMsj == "")
          {
           int nRes = await BREntities.OperationEntity(configurations, mode);
            UIHelper.ShowMessageResult("Configurations", nRes);
            if (nRes > 0)
            {
              ObjectHelper.CopyProperties(oldConfigurations, configurations);
            }
          }
          else
          {
            UIHelper.ShowMessage(sMsj);
          }
          skpStatus.Visibility = Visibility.Collapsed;
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Configurations");
      }
    }
    #endregion

    #endregion

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
        List<Configuration> lstConfigurations = await BRConfiguration.GetConfigurations();
        configurations = (Configuration)lstConfigurations.FirstOrDefault();
        ObjectHelper.CopyProperties(oldConfigurations, configurations);
        DataContext = configurations;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Configuration");
      }
    }
    #endregion

    #region LoadWeekDays
    /// <summary>
    ///   Carga los dias de la semana
    /// </summary>
    /// <history>
    ///   [vku] 11/Jun/2016 Created
    /// </history>
    protected async void LoadWeekDays()
    {
      try
      {
        List<WeekDay> lstWeekDays = await BRWeekDays.GetWeekDays("EN");
        cboocWeekDayStart.ItemsSource = lstWeekDays;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Configuration");
      }
    }
    #endregion

    #region LoadPersonelAdmin
    /// <summary>
    ///   Carga los administradores
    /// </summary>
    /// <history>
    ///   [vku] 13/Jun/2016 Created
    /// </history>
    protected async void LoadPersonelAdmin()
    {
      try
      {
        List<PersonnelShort> lstPersonelAdmin = await BRPersonnel.GetPersonnelByRole("ADMIN");
        cboocAdminUser.ItemsSource = lstPersonelAdmin;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Configuration");
      }
    }
    #endregion

    #region LoadTourTimesSchema
    /// <summary>
    ///  Carga los esquemas de horarios de tour
    /// </summary>
    /// <history>
    ///   [vku] 13/Jun/2016 Created
    /// </history>
    protected async void LoadTourTimesSchema()
    {
      try
      {
        List<TourTimesSchema> lstTourTimesSchemas =await BRTourTimesSchemas.GetTourTimesSchemas(nStatus: 1);
        cboocTourTimesSchema.ItemsSource = lstTourTimesSchemas;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Configuration");
      }
    }
    #endregion

    #region LoadPersonelBoss
    /// <summary>
    ///   Carga los patrones
    /// </summary>
    /// <history>
    ///   [vku] 13/Jun/2016 Created
    /// </history>
    protected async void LoadPersonelBoss()
    {
      try
      {
        List<PersonnelShort> lstPersonelBoss = await BRPersonnel.GetPersonnelByRole("BOSS");
        cboocBoss.ItemsSource = lstPersonelBoss;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Configuration");
      }
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    ///   Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 14/Jun/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }

    #endregion

    #endregion
  }
}
