using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmHotelDetail.xaml
  /// </summary>
  public partial class frmHotelDetail : Window
  {
    #region variables
    public Hotel hotel = new Hotel();//Objeto a guardar|Actualizar|Filtrar
    public Hotel oldHotel = new Hotel();//Objeto con los datos iniciales
    public int nStatus = -1;//Para cuando se abra en modo search
    public EnumMode enumMode;//Modo en que se abrirá la ventana

    #endregion
    public frmHotelDetail()
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
    /// <history>
    /// [emoguel] created 30/03/2016
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

    #region Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(hotel, oldHotel);
      DataContext = hotel;
      
      LoadAreas();
      LoadGroups();
      if (enumMode != EnumMode.preview)
      {
        btnAccept.Visibility = Visibility.Visible;
        cmbHotelAr.IsEnabled = true;
        cmbHotelGr.IsEnabled = true;
        chkA.IsEnabled = true;
        if (enumMode != EnumMode.edit)
        {
          txtID.IsEnabled = true;
        }
        #region Modo Busqueda
        if (enumMode == EnumMode.search)
        {
          LoadStatus();
          cmbStatus.SelectedValue = nStatus;
          chkA.Visibility = Visibility.Collapsed;
          lblStatus.Visibility = Visibility.Visible;
          cmbStatus.Visibility = Visibility.Visible;
        }
        #endregion
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Actualiza|agrega un registro en Hotels
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (enumMode!=EnumMode.search)
      {       
        if (ObjectHelper.IsEquals(hotel, oldHotel)&& enumMode!=EnumMode.add)//si no modifico nada
        {
          Close();
        }
        else//si hubo cambios
        {
          int nRes = 0;
          string strMsj = ValidateHelper.ValidateForm(this, "Hotel");

          if (strMsj == "")//Guardar
          {
            nRes= BRHotels.SaveHotel(hotel, (enumMode == EnumMode.edit));
            UIHelper.ShowMessageResult("Hotel", nRes);
            if(nRes==1)
            {
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
        nStatus = Convert.ToInt32(cmbStatus.SelectedValue);
        DialogResult = true;
        Close();
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verficando que no haya cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.preview && enumMode!=EnumMode.search)
      {
        if (!ObjectHelper.IsEquals(hotel, oldHotel))
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

    #region Methods
    #region LoadGroups
    /// <summary>
    /// Llena el combo de Hotel Group
    /// </summary>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    private void LoadGroups()
    {
      List<HotelGroup> lstHoGroup = BRHotelGroups.GetHotelGroups(nStatus: 1);
      if (enumMode == EnumMode.search)
      {
        lstHoGroup.Insert(0, new HotelGroup { hgID = "", hgN = "" });
      }
      cmbHotelGr.ItemsSource = lstHoGroup;
    }
    #endregion

    #region LoadAreas
    /// <summary>
    /// Llena el combo de ares
    /// </summary>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    private void LoadAreas()
    {
      List<Area> lstAreas = BRAreas.GetAreas(nStatus: 1);
      if(enumMode==EnumMode.search)
      {
        lstAreas.Insert(0, new Area { arID = "", arN = "" });
      }

      cmbHotelAr.ItemsSource = lstAreas;
    }
    #endregion

    #region LoadStatus
    /// <summary>
    /// Llena la lista de estatus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    protected void LoadStatus()
    {
      List<object> lstStatus = new List<object>();
      lstStatus.Add(new { sName = "All", sValue = -1 });
      lstStatus.Add(new { sName = "Inactive", sValue = 0 });
      lstStatus.Add(new { sName = "Active", sValue = 1 });

      cmbStatus.ItemsSource = lstStatus;
    }

    #endregion

    #endregion
  }
}
