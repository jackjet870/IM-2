using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmComputerDetail.xaml
  /// </summary>
  public partial class frmComputerDetail : Window
  {
    #region Variables
    public Computer computer = new Computer();//Objeto a guardar
    public Computer oldComputer = new Computer();//Objeto con los datos iniciales
    public EnumMode mode;
    private bool _isClosing = false;
    #endregion
    public frmComputerDetail()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region Window loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(computer, oldComputer);
      LoadDesks();
      UIHelper.SetUpControls(computer, this);
      if (mode == EnumMode.add)
      {
        txtcpID.IsEnabled = true;
      }
      DataContext = computer;
    }
    #endregion

    #region window keydown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// agregar|guarda en el catalogo Computers
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(computer, oldComputer) && mode != EnumMode.add)
        {
          _isClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          int nRes = 0;
          string sMsj = ValidateHelper.ValidateForm(this, "Computer");
          if (sMsj == "")
          {
            nRes = await BREntities.OperationEntity(computer, mode);
            UIHelper.ShowMessageResult("Coomputer", nRes);
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
          skpStatus.Visibility = Visibility.Collapsed;
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Computers");
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
      if (mode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(computer, oldComputer))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!_isClosing) { _isClosing = true; Close(); }
          }
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

    #endregion

    #region Metodos
    #region LoadDesks
    /// <summary>
    /// llena el combo de desk
    /// </summary>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private void LoadDesks()
    {
      try
      {
        List<Desk> lstDesk = BRDesks.GetDesks(new Desk());
        cmbDesk.ItemsSource = lstDesk;
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Computers");
      }
    }
    #endregion    
    #endregion
  }
}
