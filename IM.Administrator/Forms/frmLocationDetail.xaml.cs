using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmLocationDetail.xaml
  /// </summary>
  public partial class frmLocationDetail : Window
  {
    #region Variables
    public Location location = new Location();//Objeto a guardar en el catalogo locations
    public Location oldLocation = new Location();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    public int nStatus = -1;//sirve para cuando se abre en modo busqueda
    private bool _isClosing = false;
    #endregion
    public frmLocationDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        Close();
      }
    }
    #endregion

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(location, oldLocation);
      DataContext = location;
      LoadLocationCategories();
      LoadSalesRoom();
      if(enumMode!=EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        txtloN.IsEnabled = true;
        cmblolc.IsEnabled = true;
        cmblosr.IsEnabled = true;        
        txtloID.IsEnabled = (enumMode != EnumMode.Edit);        
        if(enumMode==EnumMode.Search)
        {
          lblLeaSrc.Visibility = Visibility.Collapsed;
          cmblols.Visibility = Visibility.Collapsed;
          lblSta.Visibility = Visibility.Visible;
          cmbSta.Visibility = Visibility.Visible;
          chkloA.Visibility = Visibility.Collapsed;
          chkAni.Visibility = Visibility.Collapsed;
          chkFly.Visibility = Visibility.Collapsed;
          chkReg.Visibility = Visibility.Collapsed;
          Title = "Search";
          ComboBoxHelper.LoadComboDefault(cmbSta);
          cmbSta.SelectedValue = nStatus;
        }
        else
        {
          chkloA.IsEnabled = true;
          chkAni.IsEnabled = true;
          chkFly.IsEnabled = true;
          chkReg.IsEnabled = true;
          cmblols.IsEnabled = true;
          LoadLeadSource();
        }
        UIHelper.SetUpControls(location, this);
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarada un Location
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] modified 30/05/2016 Se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.Search)
        {
          if (ObjectHelper.IsEquals(location, oldLocation) && enumMode != EnumMode.Add)
          {
            _isClosing = true;
            Close();
          }
          else
          {
            txtStatus.Text = "Saving Data...";
            skpStatus.Visibility = Visibility.Visible;
            int nRes = 0;
            string strMsj = ValidateHelper.ValidateForm(this, "Location");

            if (strMsj == "")
            {
              nRes = await BREntities.OperationEntity(location, enumMode);
              UIHelper.ShowMessageResult("Location", nRes);
              if (nRes > 0)
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
            skpStatus.Visibility = Visibility.Collapsed;
          }
        }
        else
        {
          nStatus = Convert.ToInt32(cmbSta.SelectedValue);
          _isClosing = true;
          DialogResult = true;
          Close();
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
    /// [emoguel] created 01/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica cambios antes de cerrar
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
        if (enumMode != EnumMode.ReadOnly && enumMode != EnumMode.Search)
        {
          if (!ObjectHelper.IsEquals(location, oldLocation))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result != MessageBoxResult.Yes)
            {
              e.Cancel = true;
            }
          }
        }
      }
    }
    #endregion
    #endregion

    #region Methods

    #region LoadSaleRoom
    /// <summary>
    /// Llena el comboBox de sales room
    /// </summary>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// [erosado] 24/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadSalesRoom()
    {
      try
      {
        skpStatus.Visibility = Visibility.Visible;
        List<SalesRoomShort> lstSalesRoom = await BRSalesRooms.GetSalesRooms(1);
        if (enumMode == EnumMode.Search && lstSalesRoom.Count > 0)
        {
          lstSalesRoom.Insert(0, new SalesRoomShort { srID = "", srN = "ALL" });
        }
        cmblosr.ItemsSource = lstSalesRoom;
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion
    
    #region LoadCategories
    /// <summary>
    /// Llena el combobox de Location categories
    /// </summary>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private async void LoadLocationCategories()
    {
      try
      {
        List<LocationCategory> lstLocCategories =await BRLocationsCategories.GetLocationsCategories();
        if (enumMode == EnumMode.Search && lstLocCategories.Count > 0)
        {
          lstLocCategories.Insert(0, new LocationCategory { lcID = "", lcN = "ALL" });
        }
        cmblolc.ItemsSource = lstLocCategories;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region Load LeadSource
    /// <summary>
    /// Llena el combobox de LeadSource
    /// </summary>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private async void LoadLeadSource()
    {
      try
      {
        List<LeadSource> lstLeadSource = await BRLeadSources.GetLeadSources(1, EnumProgram.All);
        cmblols.ItemsSource = lstLeadSource;
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
