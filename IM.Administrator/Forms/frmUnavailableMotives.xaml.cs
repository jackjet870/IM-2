using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Enums;
using IM.Model.Extensions;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmUnavailableMotives.xaml
  /// </summary>
  public partial class frmUnavailableMotives : Window
  {
    #region Variables
    private UnavailableMotive _unavailableMotive = new UnavailableMotive();//Contiene los filtros del grid
    private int _nStatus = -1;//status de los registros del grid
    private bool _blnEdit = false;//boleano para saber si se tiene permiso para editar|agregar
    #endregion

    public frmUnavailableMotives()
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
    /// [emoguel] created 04/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Motives, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadUnavailableMotives();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window keyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/06/2016
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

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/06/2016
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

    #region DobleClic Grid
    /// <summary>
    /// Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      UnavailableMotive unavailableMotive = (UnavailableMotive)dgrUnavailableMotives.SelectedItem;
      frmUnavailableMotiveDetail frmUnavailableMotDetail = new frmUnavailableMotiveDetail();
      frmUnavailableMotDetail.Owner = this;
      frmUnavailableMotDetail.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      frmUnavailableMotDetail.oldUnavailableMotive = unavailableMotive;
      if(frmUnavailableMotDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<UnavailableMotive> lstUnavailableMotives = (List<UnavailableMotive>)dgrUnavailableMotives.ItemsSource;
        if(ValidateFilter(frmUnavailableMotDetail.unavailableMotive))//Verificamos que cumpla con ls filtros
        {
          ObjectHelper.CopyProperties(unavailableMotive, frmUnavailableMotDetail.unavailableMotive);//Actualizamos los datos
          lstUnavailableMotives.Sort((x, y) => string.Compare(x.umN, y.umN));//Ordenamos la lista
          nIndex = lstUnavailableMotives.IndexOf(unavailableMotive);//Buscamos el contador
        }
        else
        {
          lstUnavailableMotives.Remove(unavailableMotive);
        }
        dgrUnavailableMotives.Items.Refresh();///Actualizamos la vista
        GridHelper.SelectRow(dgrUnavailableMotives, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstUnavailableMotives.Count + " Unavailable Motives";//Actualizamos el contador
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      UnavailableMotive unavailableMotive = (UnavailableMotive)dgrUnavailableMotives.SelectedItem;
      LoadUnavailableMotives(unavailableMotive);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la venta detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmUnavailableMotiveDetail frmUnavailableMotiveDetail = new frmUnavailableMotiveDetail();
      frmUnavailableMotiveDetail.Owner = this;
      frmUnavailableMotiveDetail.enumMode = EnumMode.Add;
      if(frmUnavailableMotiveDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmUnavailableMotiveDetail.unavailableMotive))//verificamos que cumpla con los filtros
        {
          List<UnavailableMotive> lstUnavailableMotives = (List<UnavailableMotive>)dgrUnavailableMotives.ItemsSource;
          lstUnavailableMotives.Add(frmUnavailableMotiveDetail.unavailableMotive);//Agregamos el registro
          lstUnavailableMotives.Sort((x, y) => string.Compare(x.umN, y.umN));//Ordenamos la lista
          int nIndex = lstUnavailableMotives.IndexOf(frmUnavailableMotiveDetail.unavailableMotive);//Obtenemos la posicion del registro
          dgrUnavailableMotives.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrUnavailableMotives, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstUnavailableMotives.Count + " Unavailable Motives.";//Actualizamos el contador
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
    /// [emoguel] created 04/06/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = (_unavailableMotive.umID > 0) ? _unavailableMotive.umID.ToString() : "";
      frmSearch.strDesc = _unavailableMotive.umN;
      frmSearch.nStatus = _nStatus;
      frmSearch.enumWindow = EnumWindow.DefaultByte;
      if(frmSearch.ShowDialog()==true)
      {
        _unavailableMotive.umID = Convert.ToByte(frmSearch.strID);
        _unavailableMotive.umN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadUnavailableMotives();
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadUnavailableMotives
    /// <summary>
    /// Llena el grid de unavailableMotives
    /// </summary>
    /// <param name="unavailableMotive">Objeto a seleccionar</param>
    /// <history>
    /// [emogue] created 06/06/2016
    /// </history>
    private async void LoadUnavailableMotives(UnavailableMotive unavailableMotive=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<UnavailableMotive> lstUnavailableMotives = await BRUnavailableMotives.GetUnavailableMotives(_nStatus,_unavailableMotive);
        dgrUnavailableMotives.ItemsSource = lstUnavailableMotives;
        if (lstUnavailableMotives.Count > 0 && unavailableMotive != null)
        {
          unavailableMotive = lstUnavailableMotives.Where(um => um.umID == unavailableMotive.umID).FirstOrDefault();
          nIndex = lstUnavailableMotives.IndexOf(unavailableMotive);
        }
        GridHelper.SelectRow(dgrUnavailableMotives, nIndex);
        StatusBarReg.Content = lstUnavailableMotives.Count + " Unavailable Motives.";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Unavailables Motives");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que cumpla con los filtros actuales
    /// </summary>
    /// <param name="unavailableMotive">objeto a validar</param>
    /// <returns>True. Si cumple| False. No cumple</returns>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    private bool ValidateFilter(UnavailableMotive unavailableMotive)
    {
      if(_nStatus!=-1)//filtro por estatus
      {
        if(unavailableMotive.umA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(_unavailableMotive.umID>0)//filtro por ID
      {
        if(unavailableMotive.umID!=_unavailableMotive.umID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_unavailableMotive.umN))//Filtro por Descripción
      {
        if(unavailableMotive.umN.Contains(_unavailableMotive.umN,StringComparison.OrdinalIgnoreCase))
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
