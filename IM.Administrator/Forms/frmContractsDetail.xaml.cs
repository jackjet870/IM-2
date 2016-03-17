using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Administrator.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmContractsDetail.xaml
  /// </summary>
  public partial class frmContractsDetail : Window
  {
    public Contract contract=new Contract();//Objeto que se muestra en la ventana
    public ModeOpen mode;//Modo en el que se abrirá la ventana
    public frmContractsDetail()
    {
      InitializeComponent();
    }


    #region event controls
    #region Accept
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      string sMsj = "";
      #region validar campos
      if (string.IsNullOrWhiteSpace(txtID.Text))
      {
        sMsj += "Specify the Contract ID. \n";
      }

      if (string.IsNullOrWhiteSpace(txtN.Text))
      {
        sMsj += "Specify the Contract Description. \n";
      }

      if (cmbUnvMot.SelectedIndex < 0)
      {
        sMsj += "Specify the Contract unavailable motive . \n";
      }

      if (string.IsNullOrWhiteSpace(txtMinDays.Text))
      {
        sMsj += "Specify the Contract Min Days for Included Tours. \n";
      }

      #endregion
      int nRes = 0;
      if (sMsj == "")//Todos los campos estan llenos
      {
        nRes = BRContracts.SaveContract(contract, (mode==ModeOpen.edit));      

        #region respuesta
        switch (nRes)//Se valida la respuesta de la operacion
        {
          case 0:
            {
              UIHelper.ShowMessage("Contract not saved");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Contract successfully saved");
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              UIHelper.ShowMessage("Contract ID already exist please select another one");
              break;
            }
        }
        #endregion
      }
      else
      {//Hace falta llenar campos
        UIHelper.ShowMessage(sMsj.TrimEnd('\n'));
      }
    }

    #endregion
    #region LoadedWindow
    /// <summary>
    /// Llama eventos predeterminados al abrir la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      openMode();
      LoadUnvMotive();
    }
    #endregion

    #region PreviewtextInput
    /// <summary>
    /// Valida que el textbox solamente acepte números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtMinDays_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }
    #endregion

    #region WinKeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape dependiendo del modo en que se esté visualizando la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 03/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        DialogResult = false;
        Close();
      }
    }
    #endregion
    #endregion

    #region Metodos
    #region  OpenMode
    /// <summary>
    /// Abre la ventana dependiendo del modo que elija el usuario
    /// Preview|Edit|Add
    /// </summary>
    /// <history>
    /// [emoguel] 03/03/2016 Created
    /// </history>
    protected void openMode()
    {
      DataContext = contract;
      switch (mode)
      {
        case ModeOpen.preview://show
          {
            btnAccept.Visibility = Visibility.Hidden;                        
            break;
          }
        case ModeOpen.add://add
          {
            txtID.IsEnabled = true;
            LockControls(true);
            break;
          }
        case ModeOpen.edit://Edit
          {
            txtID.IsEnabled = false;
            LockControls(true);            
            break;
          }
      }

    }

    #endregion
    #region LockControls
    /// <summary>
    /// Bloquea|Desbloquea los botones dependiendo del modo en que se habra
    /// </summary>
    /// <param name="bValue">true para desbloquear| false para bloquear</param>
    /// <history>
    /// [emoguel] 03/03/2016 Created
    /// </history>
    protected void LockControls(bool blnValue)
    {
      txtN.IsEnabled = blnValue;
      cmbUnvMot.IsEnabled = blnValue;
      chkA.IsEnabled = blnValue;
      txtMinDays.IsEnabled = blnValue;
    }
    #endregion

    #region LoadUnavailableMotives
    protected void LoadUnvMotive()
    {
      List<UnavailableMotive> lstUnaVailMots = BRUnavailableMotives.GetUnavailableMotives();
      cmbUnvMot.ItemsSource = lstUnaVailMots;
    } 
    #endregion
    #endregion

  }
}
