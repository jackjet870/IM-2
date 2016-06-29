using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using System.Linq;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmGuestStatusTypes.xaml
  /// </summary>
  public partial class frmGuestStatusTypes : Window
  {
    #region Variables
    private GuestStatusType _guestStaTypFilter = new GuestStatusType();//Objeto con los filtros del grid
    private int _nStatus = -1;//estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar y actualizar
    #endregion
    public frmGuestStatusTypes()
    {
      InitializeComponent();
    }
    #region Eventos del formulario

    #region Window keyDown
    /// <summary>
    /// Verifica que teclas fueron presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            break;
          }
      }
    }
    #endregion

    #region Window Loaded
    /// <summary>
    /// llena el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(Model.Enums.EnumPermission.HostInvitations, Model.Enums.EnumPermisionLevel.ReadOnly);
      btnAdd.IsEnabled = _blnEdit;
      LoadGuestStatusTypes();
    }
    #endregion

    #region KeyBoardFocusChange
    /// <summary>
    /// Verifica teclas presionadas con la ventana minimizada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga el grid de guestStatusTypes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      GuestStatusType guestStaType = (GuestStatusType)dgrGuestStaTyp.SelectedItem;
      LoadGuestStatusTypes(guestStaType);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmGuestStatusTypeDetail frmGuesStaDet = new frmGuestStatusTypeDetail();
      frmGuesStaDet.Owner = this;
      frmGuesStaDet.enumMode = EnumMode.add;
      if(frmGuesStaDet.ShowDialog()==true)
      {
        if(ValidateFilter(frmGuesStaDet.guestStaTyp))//Verificamos que cumpla con los filtros
        {
          GuestStatusType guestStaTyp = frmGuesStaDet.guestStaTyp;
          List<GuestStatusType> lstGuestStaTyp = (List<GuestStatusType>)dgrGuestStaTyp.ItemsSource;
          lstGuestStaTyp.Add(guestStaTyp);//Agregamos el nuevo registro
          lstGuestStaTyp.Sort((x, y) => string.Compare(x.gsN, y.gsN));//Ordenamos la lista
          int nIndex =lstGuestStaTyp.IndexOf(guestStaTyp);//Buscamos la posición
          StatusBarReg.Content = lstGuestStaTyp.Count + " Guest Status Types.";//Actualizamos el contador
          dgrGuestStaTyp.Items.Refresh();
          GridHelper.SelectRow(dgrGuestStaTyp, nIndex);
        }
      }
    } 
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _guestStaTypFilter.gsID;
      frmSearch.strDesc = _guestStaTypFilter.gsN;
      frmSearch.nStatus = _nStatus;
      if (frmSearch.ShowDialog() == true)
      {
        _guestStaTypFilter.gsID = frmSearch.strID;
        _guestStaTypFilter.gsN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadGuestStatusTypes();
      }
    } 
    #endregion

    #region Cell Double Click
    /// <summary>
    /// Muestra la ventada efficiency detail dependiendo de los permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 24/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      GuestStatusType guestStaTyp = (GuestStatusType)dgrGuestStaTyp.SelectedItem;
      frmGuestStatusTypeDetail frmGuesStaDet = new frmGuestStatusTypeDetail();
      frmGuesStaDet.guestStaTypOld = guestStaTyp;
      frmGuesStaDet.Owner = this;
      frmGuesStaDet.enumMode = ((_blnEdit == true) ? EnumMode.edit : EnumMode.preview);
      if(frmGuesStaDet.ShowDialog()==true)
      {
        int nIndex = 0;
        List<GuestStatusType> lstGuestStaTyp = (List<GuestStatusType>)dgrGuestStaTyp.ItemsSource;
        if (!ValidateFilter(frmGuesStaDet.guestStaTyp))//Validamos si cumple con los filtros
        {
          lstGuestStaTyp.Remove(guestStaTyp);//Quitamos el registro de la lista
          StatusBarReg.Content =lstGuestStaTyp.Count+ " Guest Status Types.";
        }
        else
        {
          ObjectHelper.CopyProperties(guestStaTyp,frmGuesStaDet.guestStaTyp);//Actualizamos el valor del registro
          lstGuestStaTyp.Sort((x, y) => string.Compare(x.gsN, y.gsN));//Ordenamos la lista
          nIndex = lstGuestStaTyp.IndexOf(guestStaTyp);//Obtenemos la posicion del registro actualizado para seleccionarlo
        }
        dgrGuestStaTyp.Items.Refresh();//Refrescamos la vista
        GridHelper.SelectRow(dgrGuestStaTyp, nIndex);//Seleccionamos un registro

      }
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            Cell_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }

    #endregion
    #endregion

    #region Methods

    #region LoadGuestStatusType
    /// <summary>
    /// Llena el grid de guestStatusType
    /// </summary>
    /// <param name="nIndex">index de la fila a seleccionar</param>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    private async void LoadGuestStatusTypes(GuestStatusType guestStaTyp=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<GuestStatusType> lstGuestStaTyp =await BRGuestStatusTypes.GetGuestStatusTypes(_guestStaTypFilter, _nStatus);
        dgrGuestStaTyp.ItemsSource = lstGuestStaTyp;
        if (guestStaTyp != null && lstGuestStaTyp.Count > 0)
        {
          guestStaTyp = lstGuestStaTyp.Where(gs => gs.gsID == guestStaTyp.gsID).FirstOrDefault();
          nIndex = lstGuestStaTyp.IndexOf(guestStaTyp);
        }
        GridHelper.SelectRow(dgrGuestStaTyp, nIndex);
        StatusBarReg.Content = lstGuestStaTyp.Count + " Guest Status Types.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Guest Status types");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida si un registro cumple con los filtros de la ventana
    /// </summary>
    /// <param name="newGuestStaTyp">registro a comparar</param>
    /// <returns>True. Si cumple con los filtros | False. no cumple con los filtros</returns>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    private bool ValidateFilter(GuestStatusType newGuestStaTyp)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(newGuestStaTyp.gsA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_guestStaTypFilter.gsID))//Filtro por ID
      {
        if(_guestStaTypFilter.gsID!=newGuestStaTyp.gsID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_guestStaTypFilter.gsN))//Filtro por descripcion
      {
        if(!newGuestStaTyp.gsN.Contains(_guestStaTypFilter.gsN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }

      return true;
    }
    #endregion
    #endregion
  }
}
