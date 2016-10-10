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
      if (mode == EnumMode.Add)
      {
        txtcpID.IsEnabled = true;
      }
      DataContext = computer;
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
        if (ObjectHelper.IsEquals(computer, oldComputer) && mode != EnumMode.Add)
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
            UIHelper.ShowMessageResult("Computer", nRes);
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
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica que no haya cambios pendientes antes de cerrar
    /// </summary>
    /// <history>
    /// [emoguel] 04/10/2016 created
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        btnCancel.Focus();
        if (mode != EnumMode.ReadOnly)
        {
          if (!ObjectHelper.IsEquals(computer, oldComputer))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result == MessageBoxResult.No)
            {
              _isClosing = false;
              e.Cancel = true;
            }
          }
        }
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
    private async void LoadDesks()
    {
      try
      {
        List<Desk> lstDesk =await BRDesks.GetDesks(new Desk());
        cmbDesk.ItemsSource = lstDesk;
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #endregion
  }
}
