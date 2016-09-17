using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Extensions;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmNotBookingMotives.xaml
  /// </summary>
  public partial class frmNotBookingMotives : Window
  {
    #region Variables
    private NotBookingMotive _notBookingMotFilter = new NotBookingMotive();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar
    #endregion
    public frmNotBookingMotives()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = (_notBookingMotFilter.nbID>0)?_notBookingMotFilter.nbID.ToString():"";
      frmSearch.strDesc = _notBookingMotFilter.nbN;
      frmSearch.nStatus = _nStatus;
      frmSearch.enumWindow = EnumWindow.DefaultInt;      
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _notBookingMotFilter.nbID = Convert.ToInt32(frmSearch.strID);
        _notBookingMotFilter.nbN = frmSearch.strDesc;
        LoadNotBookingMotives();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmNotBookingMotiveDetail frmNotBokMotDetail = new frmNotBookingMotiveDetail();
      frmNotBokMotDetail.Owner = this;
      frmNotBokMotDetail.enumMode = EnumMode.Add;
      if(frmNotBokMotDetail.ShowDialog()==true)
      {
        NotBookingMotive notBookingMotive = frmNotBokMotDetail.notBookingMotive;
        if(ValidateFilter(notBookingMotive))//verificamos que cumpla con los filtros actuales
        {
          List<NotBookingMotive> lstNotBokMotives = (List<NotBookingMotive>)dgrNotBokMotives.ItemsSource;
          lstNotBokMotives.Add(notBookingMotive);//Agregamos el registro
          lstNotBokMotives.Sort((x, y) => string.Compare(x.nbN, y.nbN));//Ordenamos la lista
          int nIndex = lstNotBokMotives.IndexOf(notBookingMotive);//Obtenemos el index del nuevo registro
          dgrNotBokMotives.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrNotBokMotives, nIndex);//Seleccionamos el nuevo registro
          StatusBarReg.Content = lstNotBokMotives.Count+" Not Booking Motives.";//Actualizamos el contador
        }
      }
    } 
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros del Grid de Not Booking Motive
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      NotBookingMotive notBookingMotive = (NotBookingMotive)dgrNotBokMotives.SelectedItem;
      LoadNotBookingMotives(notBookingMotive);
    } 
    #endregion

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(Model.Enums.EnumPermission.Motives, Model.Enums.EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadNotBookingMotives();
    }
    #endregion

    #region Cell Double click
    /// <summary>
    /// Muestra la ventada detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      NotBookingMotive notBookingMotive = (NotBookingMotive)dgrNotBokMotives.SelectedItem;
      frmNotBookingMotiveDetail frmNotBokMotDetail = new frmNotBookingMotiveDetail();
      frmNotBokMotDetail.Owner = this;
      frmNotBokMotDetail.enumMode = (_blnEdit == true) ? EnumMode.Edit : EnumMode.ReadOnly;
      frmNotBokMotDetail.oldNotBookingMotive = notBookingMotive;

      if (frmNotBokMotDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<NotBookingMotive> lstNotBokMotives = (List<NotBookingMotive>)dgrNotBokMotives.ItemsSource;
        if(!ValidateFilter(frmNotBokMotDetail.notBookingMotive))
        {
          lstNotBokMotives.Remove(notBookingMotive);//Quitamos el registro de la lista
        }
        else
        {
          ObjectHelper.CopyProperties(notBookingMotive, frmNotBokMotDetail.notBookingMotive);//Actualizamos los datos del registro
          lstNotBokMotives.Sort((x, y) => string.Compare(x.nbN, y.nbN));//Ordenamos la lista
          nIndex = lstNotBokMotives.IndexOf(notBookingMotive);//Buscamos la posicion del registro
        }
        dgrNotBokMotives.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrNotBokMotives,nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstNotBokMotives.Count + " Not Booking Motives.";
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
    /// [created] 05/04/2016
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

    #region KeyBoardFocuChange
    /// <summary>
    /// Verifica las teclas presinadas al cambio de ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window Key Down
    /// <summary>
    /// Verifica las teclas presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    #endregion

    #region Methods
    #region LoadNotBookingMotives
    /// <summary>
    /// llena el grid de Not Booking Motives
    /// </summary>
    /// <param name="notBookingMotive">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private async void LoadNotBookingMotives(NotBookingMotive notBookingMotive = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<NotBookingMotive> lstNotBokMotives =await BRNotBookingMotives.GetNotBookingMotives(_nStatus, _notBookingMotFilter);
        dgrNotBokMotives.ItemsSource = lstNotBokMotives;
        if (lstNotBokMotives.Count > 0 && notBookingMotive != null)
        {
          notBookingMotive = lstNotBokMotives.Where(nb => nb.nbID == notBookingMotive.nbID).FirstOrDefault();
          nIndex = lstNotBokMotives.IndexOf(notBookingMotive);
        }
        GridHelper.SelectRow(dgrNotBokMotives, nIndex);
        StatusBarReg.Content = lstNotBokMotives.Count + " Not Booking Motives.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Not Booking Motives");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un registro tipo notBookingMotive cumpla con los filtros actuales
    /// </summary>
    /// <param name="notBookingMotive">Filtro a validar</param>
    /// <returns>Truw. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 05/04/2016
    /// </history>
    private bool ValidateFilter(NotBookingMotive notBookingMotive)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(notBookingMotive.nbA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(_notBookingMotFilter.nbID>0)//Filtro por ID
      {
        if(_notBookingMotFilter.nbID!=notBookingMotive.nbID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_notBookingMotFilter.nbN))//Filtro por descripcion
      {
        if(!notBookingMotive.nbN.Contains(_notBookingMotFilter.nbN,StringComparison.OrdinalIgnoreCase))
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
