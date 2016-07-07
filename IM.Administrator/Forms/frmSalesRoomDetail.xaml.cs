using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSalesRoomDetail.xaml
  /// </summary>
  public partial class frmSalesRoomDetail : Window
  {
    #region variables
    public SalesRoom salesRoom = new SalesRoom();//Objeto a guardar
    public SalesRoom oldSalesRoom = new SalesRoom();//Objeto con los datos iniciales 
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    public int nStatus = -1;
    public int nAppointment = -1;
    private bool _isClosing = false;
    #endregion
    public frmSalesRoomDetail()
    {
      InitializeComponent();
    }

    #region Method Form
    #region Accept
    /// <summary>
    /// Guarda|Actualiza un registro en la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.search)
      {
        if(enumMode!=EnumMode.add && ObjectHelper.IsEquals(salesRoom,oldSalesRoom))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Sales Room");
          if(strMsj=="")
          {
            int nRes = BRSalesRooms.SaveSalesRoom(salesRoom, (enumMode == EnumMode.edit));
            UIHelper.ShowMessageResult("Sales Room", nRes);
            if(nRes==1)
            {
              _isClosing = true;
              DialogResult = true;
              Close();
            }
          }
          else
          {
            UIHelper.ShowMessage(strMsj);
          }
        }
      }
      else
      {
        _isClosing = true;
        nStatus = Convert.ToInt32(cmbStatus.SelectedValue);
        nAppointment = Convert.ToInt32(cmbApp.SelectedValue);
        DialogResult = true;
        Close();
      }
    } 
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();      
    } 
    #endregion

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(salesRoom, oldSalesRoom);
      UIHelper.SetUpControls(salesRoom, this);
      if(enumMode==EnumMode.search)
      {
        Title = "Search";
        chksrA.Visibility = Visibility.Collapsed;
        chksrAppointment.Visibility = Visibility.Collapsed;
        chksrUseSistur.Visibility = Visibility.Collapsed;
        lblStatus.Visibility = Visibility.Visible;
        lblApp.Visibility = Visibility.Visible;
        lblOpera.Visibility = Visibility.Collapsed;
        txtsrPropertyOpera.Visibility = Visibility.Collapsed;
        cmbStatus.Visibility = Visibility.Visible;
        cmbApp.Visibility = Visibility.Visible;
        ComboBoxHelper.LoadComboDefault(cmbApp);
        ComboBoxHelper.LoadComboDefault(cmbStatus);
        cmbApp.SelectedValue = nAppointment;
        cmbStatus.SelectedValue = nStatus;
        cmbsrWH.Visibility = Visibility.Collapsed;
        lblWareHouse.Visibility = Visibility.Collapsed;
        lblBoss.Visibility = Visibility.Collapsed;
        cmbsrBoss.Visibility = Visibility.Collapsed;
        
      }
      else
      {        
        LoadBoss();
        LoadWarehouses();        
      }
      if(enumMode!=EnumMode.preview)
      {
        btnAccept.Visibility = Visibility.Visible;
      }
      txtsrID.IsEnabled = (enumMode != EnumMode.edit);
      LoadAreas();      
      LoadCurrency();
      DataContext = salesRoom;
    } 
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        Close();
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        if (enumMode != EnumMode.preview && enumMode != EnumMode.search)
        {
          if (!ObjectHelper.IsEquals(salesRoom, oldSalesRoom))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result != MessageBoxResult.Yes)
            {
              _isClosing = true;
            }
          }
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadAreas
    /// <summary>
    /// Carga el combobox de Areas
    /// </summary>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// [emoguel] modified 30/05/2016 Se volvió async
    /// </history>
    private async void LoadAreas()
    {
      try
      {
        List<Area> lstAreas = await BRAreas.GetAreas();
        if (enumMode == EnumMode.search && lstAreas.Count > 0)
        {
          lstAreas.Insert(0, new Area { arID = "", arN = "ALL" });
        }
        cmbsrar.ItemsSource = lstAreas;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Sales Room");
      }
    }
    #endregion

    #region LoadWareHouse
    /// <summary>
    /// llena el combobox de WareHouses
    /// </summary>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// </history>
    private async void LoadWarehouses()
    {
      try
      {
        List<Warehouse> lstWareHouses = await BRWarehouses.GetWareHouses();
        cmbsrWH.ItemsSource = lstWareHouses;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Sales Room");
      }
    }
    #endregion

    #region LoadCurrency
    /// <summary>
    /// Llena el combox currency
    /// </summary>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadCurrency()
    {
      try
      {
        List<Currency> lstCurrency = await BRCurrencies.GetCurrencies();
        if (enumMode == EnumMode.search && lstCurrency.Count > 0)
        {
          lstCurrency.Insert(0, new Currency { cuID = "", cuN = "ALL" });
        }
        cmbsrcu.ItemsSource = lstCurrency;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Sales Room");
      }
    }
    #endregion

    #region LoadBoss
    /// <summary>
    /// Llena el combobox de Boss
    /// </summary>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadBoss()
    {
      try
      {
        List<PersonnelShort> lstPersonnel = await BRPersonnel.GetPersonnel(roles: "Boss");
        cmbsrBoss.ItemsSource = lstPersonnel;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Sales Room");
      }
    }
    #endregion

    #endregion
  }
}
