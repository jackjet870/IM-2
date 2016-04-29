using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

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
        btnCancel.Focus();
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
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (ObjectHelper.IsEquals(computer, oldComputer) && mode != EnumMode.add)
      {
        Close();
      }
      else
      {
        int nRes = 0;
        string sMsj = ValidateHelper.ValidateForm(this, "Computer");


        if (sMsj == "")
        {
          nRes = BREntities.OperationEntity(computer, mode);
          UIHelper.ShowMessageResult("Coomputer", nRes);
          if (nRes > 0)
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
      if (mode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(computer, oldComputer))
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

    #region Metodos
    #region LoadDesks
    /// <summary>
    /// llena el combo de desk
    /// </summary>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void LoadDesks()
    {
      List<Desk> lstDesk = BRDesks.GetDesks(new Desk());
      cmbDesk.ItemsSource = lstDesk;
    }
    #endregion    
    #endregion
  }
}
