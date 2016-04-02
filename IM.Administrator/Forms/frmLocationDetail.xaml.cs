using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Administrator.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model;
using System.Windows.Controls;
using System.Linq;

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
        btnCancel_Click(null, null);
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
      if(enumMode!=EnumMode.preview)
      {
        btnAccept.Visibility = Visibility.Visible;
        txtDes.IsEnabled = true;
        cmbLocCat.IsEnabled = true;
        cmbSalRom.IsEnabled = true;        
        txtID.IsEnabled = (enumMode != EnumMode.edit);        
        if(enumMode==EnumMode.search)
        {
          lblLeaSrc.Visibility = Visibility.Collapsed;
          cmbLeadSr.Visibility = Visibility.Collapsed;
          lblSta.Visibility = Visibility.Visible;
          cmbSta.Visibility = Visibility.Visible;
          chkA.Visibility = Visibility.Collapsed;
          chkAni.Visibility = Visibility.Collapsed;
          chkFly.Visibility = Visibility.Collapsed;
          chkReg.Visibility = Visibility.Collapsed;
          Title = "Search";
          LoadStatus();
          cmbSta.SelectedValue = nStatus;
        }
        else
        {
          chkA.IsEnabled = true;
          chkAni.IsEnabled = true;
          chkFly.IsEnabled = true;
          chkReg.IsEnabled = true;
          cmbLeadSr.IsEnabled = true;
          LoadLeadSource();
        }
      }
    }
    #endregion

    #region Accept
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (enumMode!=EnumMode.search)
      {
        if(ObjectHelper.IsEquals(location,oldLocation) && enumMode!=EnumMode.add)
        {
          Close();          
        }
        else
        {
          int nRes = 0;
          string strMsj = ValidateHelper.ValidateForm(this,"Location");
          
          if(strMsj=="")
          {
            nRes = BRLocations.SaveLocation(location, (enumMode == EnumMode.edit));
            #region respuesta
            switch (nRes)
            {
              case 0:
                {
                  UIHelper.ShowMessage("Location not saved.");
                  break;
                }
              case 1:
                {
                  UIHelper.ShowMessage("Location successfully saved.");
                  DialogResult = true;
                  Close();
                  break;
                }
              case 2:
                {
                  UIHelper.ShowMessage("Location ID already exist please select another one.");
                  break;
                }
            }
            #endregion
          }
          else
          {
            UIHelper.ShowMessage(strMsj);
          }
        }
      }
      else
      {
        nStatus = Convert.ToInt32(cmbSta.SelectedValue);
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
    /// [emoguel] created 01/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.preview && enumMode != EnumMode.search)
      {
        if (!ObjectHelper.IsEquals(location, oldLocation))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Warning, "Closing window", MessageBoxButton.OKCancel);
          if (result == MessageBoxResult.OK)
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

    #region Methods
    #region LoadStatus
    /// <summary>
    /// Llena la lista de estatus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    protected void LoadStatus()
    {
      List<object> lstStatus = new List<object>();
      lstStatus.Add(new { sName = "All", sValue = -1 });
      lstStatus.Add(new { sName = "Inactive", sValue = 0 });
      lstStatus.Add(new { sName = "Active", sValue = 1 });      
      cmbSta.ItemsSource = lstStatus;
    }

    #endregion

    #region LoadSaleRoom
    /// <summary>
    /// Llena el comboBox de sales room
    /// </summary>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void LoadSalesRoom()
    {
      List<SalesRoomShort> lstSalesRoom = BRSalesRooms.GetSalesRooms(1);
      if(enumMode==EnumMode.search && lstSalesRoom.Count>0)
      {
        lstSalesRoom.Insert(0, new SalesRoomShort { srID = "", srN = "" });
      }
      cmbSalRom.ItemsSource = lstSalesRoom;
    }
    #endregion
    
    #region LoadCategories
    /// <summary>
    /// Llena el combobox de Location categories
    /// </summary>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void LoadLocationCategories()
    {
      List<LocationCategory> lstLocCategories = BRLocationsCategories.GetLocationsCategories();
      if(enumMode==EnumMode.search && lstLocCategories.Count>0)
      {
        lstLocCategories.Insert(0, new LocationCategory { lcID = "", lcN = "" });
      }
      cmbLocCat.ItemsSource = lstLocCategories;
    }
    #endregion

    #region Load LeadSource
    /// <summary>
    /// Llena el combobox de LeadSource
    /// </summary>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void LoadLeadSource()
    {
      List<LeadSource> lstLeadSource = BRLeadSources.GetLeadSources(1);
      cmbLeadSr.ItemsSource = lstLeadSource;
    }
    #endregion
    #endregion
  }
}
