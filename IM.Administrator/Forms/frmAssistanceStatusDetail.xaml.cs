using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAssistanceStatusDetail.xaml
  /// </summary>
  public partial class frmAssistanceStatusDetail : Window
  {
    #region Variables
    public EnumMode mode;
    public AssistanceStatus assistance = new AssistanceStatus();//Objeto a guardar|actualizar
    public AssistanceStatus oldAssistance = new AssistanceStatus();//Objeto con los datos iniciales 
    private bool _isClosing = false;
    #endregion
    public frmAssistanceStatusDetail()
    {
      InitializeComponent();
    }
    
    #region eventos de controles
    #region Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(assistance, oldAssistance);
      DataContext = assistance;
      if(mode!=EnumMode.ReadOnly)
      {
        txtatN.IsEnabled = true;
        chkA.IsEnabled = true;
        txtatID.IsEnabled = (mode == EnumMode.Add);
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetUpControls(assistance, this);
      }
      
    }

    #endregion
    #region Accept click
    /// <summary>
    /// Agrega o actualiza los registros del catalogo AssistantStatus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(assistance, oldAssistance) && mode != EnumMode.Add)
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string sMsj = ValidateHelper.ValidateForm(this, "Assistance Status");
          int nRes = 0;

          if (sMsj == "")//Validar si hay cmapos vacios
          {
            nRes = await BREntities.OperationEntity(assistance, mode);

            UIHelper.ShowMessageResult("Assistance Status", nRes);
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
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Assistance Status");
      }

    }

    #endregion
    
    #region KeyDown Form
    /// <summary>
    /// Cierra la ventana dependiendo del modo en el que fue abierta
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);       
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
      if(mode!=EnumMode.ReadOnly)
      {        
        if (!ObjectHelper.IsEquals(assistance, oldAssistance))
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

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/05/2016
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
  }
}
