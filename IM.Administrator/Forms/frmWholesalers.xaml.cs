using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmWholesalers.xaml
  /// </summary>
  public partial class frmWholesalers : Window
  {
    #region Variables
    private WholesalerData _WholeSalerFilter = new WholesalerData { wscl=0};//Objeto con los filtros del grid
    private bool _blnEdit = false;//Para saber si se tiene permiso para editar|agregar
    private bool _blnDel = false;//para saber si se tiene permiso para Eliminar
    #endregion

    public frmWholesalers()
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
    /// [emoguel] created 07/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.WholeSalers, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      _blnDel = Context.User.HasPermission(EnumPermission.WholeSalers, EnumPermisionLevel.Special);
      LoadWholesalers();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
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
    /// [emoguel] created 07/06/2016
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
    /// [emoguel]07/06/2016
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
    /// [emoguel] created 07/06/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      WholesalerData wholesalerData = (WholesalerData)dgrWholesalers.SelectedItem;
      frmWholesalerDetail frmwholesalerDetail = new frmWholesalerDetail();
      frmwholesalerDetail.Owner = this;
      frmwholesalerDetail.enumMode = EnumMode.ReadOnly;
      frmwholesalerDetail.oldWholesaler = new Wholesaler {wsApplication=wholesalerData.wsApplication,wscl=wholesalerData.wscl,wsCompany=wholesalerData.wsCompany };
      frmwholesalerDetail.ShowDialog();
    }
    #endregion

    #region refresh
    /// <summary>
    /// Recarga los registros de la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      WholesalerData wholeSalerData = (WholesalerData)dgrWholesalers.SelectedItem;
      LoadWholesalers(wholeSalerData);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmWholesalerDetail frmWholesalerDetail = new frmWholesalerDetail();
      frmWholesalerDetail.Owner = this;
      frmWholesalerDetail.enumMode = EnumMode.Add;
      if(frmWholesalerDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmWholesalerDetail.wholesalerData))//Verificamos que cumpla con los filtros
        {
          List<WholesalerData> lstWholesalersData = (List<WholesalerData>)dgrWholesalers.ItemsSource;
          lstWholesalersData.Add(frmWholesalerDetail.wholesalerData);//Agregamos el registros
          lstWholesalersData.Sort((x, y) => string.Compare(x.Name,y.Name));//Ordenamos la lista
          int nIndex = lstWholesalersData.IndexOf(frmWholesalerDetail.wholesalerData);//Buscamos la posición del registro
          dgrWholesalers.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrWholesalers, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstWholesalersData.Count + " Wholesalers.";//Actualizamos el contador
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
    /// [emoguel] created 07/06/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmWholesalerDetail frmWholesalerDetail = new frmWholesalerDetail();
      frmWholesalerDetail.Owner = this;
      frmWholesalerDetail.enumMode = EnumMode.Search;
      ObjectHelper.CopyProperties(frmWholesalerDetail.wholesalerData,_WholeSalerFilter);
      if(frmWholesalerDetail.ShowDialog()==true)
      {
        ObjectHelper.CopyProperties(_WholeSalerFilter, frmWholesalerDetail.wholesalerData);
        LoadWholesalers();
      }
    }
    #endregion

    #region Del
    /// <summary>
    /// Elimina registros de la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private async void btnDel_Click(object sender, RoutedEventArgs e)
    {
      if (dgrWholesalers.SelectedItems.Count > 0)
      {
        List<WholesalerData> lstWholesalerDel = dgrWholesalers.SelectedItems.OfType<WholesalerData>().ToList();        
        List<Wholesaler> lstWholesaler = lstWholesalerDel.Select(tt => new Wholesaler { wscl = tt.wscl, wsApplication = tt.wsApplication, wsCompany = tt.wsCompany }).ToList();
        MessageBoxResult msgResult = MessageBoxResult.No;
        #region MessageBox
        if (lstWholesalerDel.Count == 1)
        {
          msgResult = UIHelper.ShowMessage("Are you sure you want to delete this Wholesaler?", MessageBoxImage.Question, "Delete");
        }
        else
        {
          msgResult = UIHelper.ShowMessage("Are you sure you want to delete these Wholesalers?", MessageBoxImage.Question, "Delete");
        }
        #endregion

        if (msgResult == MessageBoxResult.Yes)
        {
          int nRes = await BREntities.OperationEntities(lstWholesaler, EnumMode.Delete);

          if (nRes > 0)
          {
            if (nRes == 1)
            {
              UIHelper.ShowMessage("Wholesaler was Deleted.");
            }
            else
            {
              UIHelper.ShowMessage("Wholesalers were Deleted.");
            }
            List<WholesalerData> lstTeamstLog = (List<WholesalerData>)dgrWholesalers.ItemsSource;
            lstTeamstLog.RemoveAll(tl => lstWholesalerDel.Contains(tl));
            dgrWholesalers.Items.Refresh();
          }
        }
      }
      else
      {
        UIHelper.ShowMessage("Please select a Wholesaler.");
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadWholesalers
    /// <summary>
    /// Llena el grid de Wholesalers
    /// </summary>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private async void LoadWholesalers(WholesalerData wholesalersData=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<WholesalerData> lstWholesalers = await BRWholesalers.GetWholesalers(_WholeSalerFilter);
        dgrWholesalers.ItemsSource = lstWholesalers;
        if(lstWholesalers.Count>0)
        {
          if (wholesalersData != null)
          {
            wholesalersData = lstWholesalers.Where(wh => wh.wscl == wholesalersData.wscl && wh.wsApplication == wholesalersData.wsApplication && wh.wsCompany == wholesalersData.wsCompany).FirstOrDefault();
            nIndex = lstWholesalers.IndexOf(wholesalersData);
          }
          btnDel.IsEnabled = _blnDel;
          GridHelper.SelectRow(dgrWholesalers, nIndex);
        }
        else
        {
          btnDel.IsEnabled = false;
        }
        
        StatusBarReg.Content = lstWholesalers.Count + " Wholesalers.";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que cumpla con los filtros actuales
    /// </summary>
    /// <param name="wholesalerData">OBjeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [Emoguel] created 07/06/2016
    /// </history>
    private bool ValidateFilter(WholesalerData wholesalerData)
    {
      if (_WholeSalerFilter.wscl > 0)
      {
        if (_WholeSalerFilter.wscl != wholesalerData.wscl)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_WholeSalerFilter.Name))
      {
        if (!wholesalerData.Name.Contains(_WholeSalerFilter.Name))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_WholeSalerFilter.wsApplication))
      {
        if (_WholeSalerFilter.wsApplication != wholesalerData.wsApplication)
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
