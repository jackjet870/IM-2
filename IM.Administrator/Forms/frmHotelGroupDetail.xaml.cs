using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmHotelGroupDetail.xaml
  /// </summary>
  public partial class frmHotelGroupDetail : Window
  {
    #region Variables
    public HotelGroup hotelGroup = new HotelGroup();//Objeto a guardar
    public HotelGroup oldHotelGroup = new HotelGroup();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    private List<Hotel> _oldHotels = new List<Hotel>();//Hoteles iniciales
    #endregion
    public frmHotelGroupDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(hotelGroup, oldHotelGroup);
      UIHelper.SetUpControls(hotelGroup, this);
      if(enumMode!=EnumMode.preview)
      {
        txthoN.IsEnabled = true;
        chkhoA.IsEnabled = true;
        dgrHotels.IsReadOnly = false;
        txthoID.IsEnabled = (enumMode == EnumMode.add);
        btnAccept.Visibility = Visibility.Visible;
      }
      LoadHotels();
      DataContext = hotelGroup;
    }
    #endregion

    #region Window keyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/05/2016
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

    #region CellEditing
    /// <summary>
    /// Verifica que un Hotel no se seleccione 2 veces
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private void dgrHotels_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        List<Hotel> lstHotels = (List<Hotel>)dgrHotels.ItemsSource;//Los items del grid                   
        Hotel hotel = (Hotel)dgrHotels.SelectedItem;//Valor que se está editando

        var Combobox = (ComboBox)e.EditingElement;
        Hotel hotelCombo = (Hotel)Combobox.SelectedItem;//Valor seleccionado del combo

        if (hotelCombo != null)//Se valida que no esté seleccionado en otra fila
        {
          if (hotelCombo != hotel)//Validar que se esté cambiando el valor
          {
            Hotel hotelVal = lstHotels.Where(ho => ho.hoID != hotel.hoID && ho.hoID == hotelCombo.hoID).FirstOrDefault();
            if (hotelVal != null)
            {
              UIHelper.ShowMessage("Hotel must not be repeated");
              e.Cancel = true;
            }
          }
        }
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualza un hotelGroup y sus hotels asignados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<Hotel> lstHotels = (List<Hotel>)dgrHotels.ItemsSource;
      if (enumMode != EnumMode.add && ObjectHelper.IsEquals(hotelGroup, oldHotelGroup) && ObjectHelper.IsListEquals(_oldHotels, lstHotels))
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Hotel");        
        if (strMsj == "")
        {
          List<Hotel> lstAdd = lstHotels.Where(ho => !_oldHotels.Any(hoo => hoo.hoID == ho.hoID)).ToList();
          List<Hotel> lstDel = _oldHotels.Where(ho => !lstHotels.Any(hoo => hoo.hoID == ho.hoID)).ToList();
          int nRes = BRHotelGroups.SaveHotelGroup(hotelGroup,(enumMode==EnumMode.edit),lstAdd,lstDel);
          UIHelper.ShowMessageResult("Hotel Group", nRes);
          if (nRes > 0)
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
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verficando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.preview)
      {
        List<Hotel> lstHotels = (List<Hotel>)dgrHotels.ItemsSource;
        if (!ObjectHelper.IsEquals(hotelGroup, oldHotelGroup) || !ObjectHelper.IsListEquals(lstHotels, _oldHotels))
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
    #region LoadHotels
    /// <summary>
    /// Llena el grid de Hotels y el combobox de hotels
    /// </summary>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private void LoadHotels()
    {
      List<Hotel> lstAllHotels = BRHotels.GetHotels(nStatus: 1);      
      List<Hotel> lstHotels =(hotelGroup.hgID!=null)? lstAllHotels.Where(ho => ho.hoGroup == hotelGroup.hgID).ToList():new List<Hotel>();
      _oldHotels = lstHotels.ToList();
      dgrHotels.ItemsSource = lstHotels;
      cmbHotels.ItemsSource = lstAllHotels;
    } 
    #endregion
    #endregion
  }
}
