using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
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
    public Contract oldContract = new Contract();//Objeto con los datos iniciales
    public EnumMode mode;//Modo en el que se abrirá la ventana
    public frmContractsDetail()
    {
      InitializeComponent();
    }


    #region event controls
    #region Accept
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(contract,oldContract) && mode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string sMsj = ValidateHelper.ValidateForm(this, "Contract");
        int nRes = 0;
        if (sMsj == "")//Todos los campos estan llenos
        {
          nRes = BRContracts.SaveContract(contract, (mode == EnumMode.edit));
          UIHelper.ShowMessageResult("Contract", nRes);
          if(nRes==1)
          {
            DialogResult = true;
            Close();
          }
        }
        else
        {//Hace falta llenar campos
          UIHelper.ShowMessage(sMsj);
        }
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
      ObjectHelper.CopyProperties(contract, oldContract);
      if(mode!=EnumMode.preview)
      {
        txtcnID.IsEnabled = (mode == EnumMode.add);
        txtcnMinDaysTours.IsEnabled = true;
        txtcnN.IsEnabled = true;
        cmbUnvMot.IsEnabled = true;
        chkA.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetUpControls(contract, this);
      }
      LoadUnvMotive();
      DataContext = contract;
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
      if(mode!=EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(contract, oldContract))
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
