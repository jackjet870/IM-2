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
using System;

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
    private bool blnClosing = false;
    private bool isCellCancel = false;
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
        dgrHotels.BeginningEdit += GridHelper.dgr_BeginningEdit;
      }
      LoadHotels();
      DataContext = hotelGroup;
      dgrHotels.BeginningEdit += GridHelper.dgr_BeginningEdit;
    }
    #endregion

    #region CellEditing
    /// <summary>
    /// Verifica que un Hotel no se seleccione 2 veces
    /// </summary>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private void dgrHotels_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction==DataGridEditAction.Commit)//Verificar si se está cancelando la edición
      {
        isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrHotels);
        e.Cancel = isRepeat;
      }
      else
      {
        isCellCancel = true;
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
    /// [emoguel] created
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing && enumMode != EnumMode.preview)
      {
        btnCancel.Focus();
        List<Hotel> lstHotels = (List<Hotel>)dgrHotels.ItemsSource;
        if (!ObjectHelper.IsEquals(hotelGroup, oldHotelGroup) || !ObjectHelper.IsListEquals(lstHotels, _oldHotels))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrHotels.CancelEdit();
          }
        }
      }
    }
    #endregion

    #region dgrHotels_RowEditEnding
    /// <summary>
    /// No permite agregar filas vacias
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dgrHotels_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction==DataGridEditAction.Commit && isCellCancel)
      {
        dgrHotels.RowEditEnding -= dgrHotels_RowEditEnding;
        dgrHotels.CancelEdit();
        dgrHotels.RowEditEnding += dgrHotels_RowEditEnding;
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
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<Hotel> lstHotels = (List<Hotel>)dgrHotels.ItemsSource;
        if (enumMode != EnumMode.add && ObjectHelper.IsEquals(hotelGroup, oldHotelGroup) && ObjectHelper.IsListEquals(_oldHotels, lstHotels))
        {
          blnClosing = true;
          Close();
        }
        else
        {
          txtStatus.Text = "Saving Data...";
          skpStatus.Visibility = Visibility.Visible;
          btnAccept.Visibility = Visibility.Collapsed;
          string strMsj = ValidateHelper.ValidateForm(this, "Hotel", blnDatagrids: true);
          if (strMsj == "")
          {
            List<Hotel> lstAdd = lstHotels.Where(ho => !_oldHotels.Any(hoo => hoo.hoID == ho.hoID)).ToList();
            List<Hotel> lstDel = _oldHotels.Where(ho => !lstHotels.Any(hoo => hoo.hoID == ho.hoID)).ToList();
            int nRes = await BRHotelGroups.SaveHotelGroup(hotelGroup, (enumMode == EnumMode.edit), lstAdd, lstDel);
            UIHelper.ShowMessageResult("Hotel Group", nRes);
            if (nRes > 0)
            {
              blnClosing = true;
              DialogResult = true;
              Close();
            }
          }
          else
          {
            UIHelper.ShowMessage(strMsj);
          }
          skpStatus.Visibility = Visibility.Collapsed;
          btnAccept.Visibility = Visibility.Visible;
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Hotel Group");
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
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadHotels()
    {
      try
      {
        List<Hotel> lstAllHotels = await BRHotels.GetHotels(nStatus: 1);
        List<Hotel> lstHotels = (hotelGroup.hgID != null) ? lstAllHotels.Where(ho => ho.hoGroup == hotelGroup.hgID).ToList() : new List<Hotel>();
        _oldHotels = lstHotels.ToList();
        dgrHotels.ItemsSource = lstHotels;
        cmbHotels.ItemsSource = lstAllHotels;
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Hotel Group");
      }
    }
    #endregion

    #endregion
  }
}
