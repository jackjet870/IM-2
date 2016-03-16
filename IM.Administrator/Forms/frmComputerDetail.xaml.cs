using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Administrator.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmComputerDetail.xaml
  /// </summary>
  public partial class frmComputerDetail : Window
  {
    public Computer computer = new Computer();
    public ModeOpen mode;
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
      LoadDesks();
      DataContext = computer;
      if(mode==ModeOpen.add)
      {
        txtID.IsEnabled = true;
      }      
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
        Close();
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
      int nRes = 0;
      string sMsj = "";
      #region validar campos
      if (string.IsNullOrWhiteSpace(txtID.Text))
      {
        sMsj += "Specify the Computer ID. \n";
      }

      if (string.IsNullOrWhiteSpace(txtN.Text))
      {
        sMsj += "Specify the Computer Description. \n";
      }
      
      #endregion
      if(sMsj=="")
      {
        nRes = BRComputers.SaveComputer(computer, ((mode == ModeOpen.edit)));

        #region respuesta
        switch (nRes)//Se valida la respuesta de la operacion
        {
          case 0:
            {
              UIHelper.ShowMessage("Computer not saved");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Computer successfully saved");
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              UIHelper.ShowMessage("Computer ID already exist please select another one");
              break;
            }
        }
        #endregion
      }
      else
      {
        UIHelper.ShowMessage(sMsj.TrimEnd('\n'));
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
