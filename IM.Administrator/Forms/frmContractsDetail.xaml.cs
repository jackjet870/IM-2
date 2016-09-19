using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Base.Helpers;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmContractsDetail.xaml
  /// </summary>
  public partial class frmContractsDetail : Window
  {
    #region Variables
    public Contract contract = new Contract();//Objeto que se muestra en la ventana
    public Contract oldContract = new Contract();//Objeto con los datos iniciales
    public EnumMode mode;//Modo en el que se abrirá la ventana
    private bool _isClosing = false; 
    #endregion
    public frmContractsDetail()
    {
      InitializeComponent();
    }


    #region event controls
    #region Accept
    /// <summary>
    /// Guarda un registro en el catalogo Contracts
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(contract, oldContract) && mode != EnumMode.Add)
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string sMsj = ValidateHelper.ValidateForm(this, "Contract");
          int nRes = 0;
          if (sMsj == "")//Todos los campos estan llenos
          {
            nRes = await BREntities.OperationEntity<Contract>(contract, mode);
            UIHelper.ShowMessageResult("Contract", nRes);
            if (nRes == 1)
            {
              _isClosing = true;
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
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
      if(mode!=EnumMode.ReadOnly)
      {
        txtcnID.IsEnabled = (mode == EnumMode.Add);
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
      btnCancel.Focus();
      if(mode!=EnumMode.ReadOnly)
      {
        if (!ObjectHelper.IsEquals(contract, oldContract))
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

    #region Metodos    

    #region LoadUnavailableMotives
    /// <summary>
    /// Carga la lista de unavailMotives
    /// </summary>
    /// <history>
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    protected async void LoadUnvMotive()
    {
      try
      {
        List<UnavailableMotive> lstUnaVailMots = await BRUnavailableMotives.GetUnavailableMotives();
        cmbUnvMot.ItemsSource = lstUnaVailMots;
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
