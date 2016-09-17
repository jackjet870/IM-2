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
  /// Interaction logic for frmFoliosCxC.xaml
  /// </summary>
  public partial class frmFoliosCXC : Window
  {
    private FolioCXC _folioFilter=new FolioCXC();//Filtro de folio
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar|actualizar

    public frmFoliosCXC()
    {
      InitializeComponent();
    }
    #region Eventos del formulario

    #region WindowLoaded
    /// <summary>
    /// Inicializa los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(Model.Enums.EnumPermission.FoliosCxC, Model.Enums.EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadFoliosCXC();
    }
    #endregion

    #region IsKeyboardFocusChange
    /// <summary>
    /// Verifica las teclas presionadas con la ventana minimizada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 2/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Window Keydown
    /// <summary>
    /// Verifica las teclas presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
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

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _folioFilter.fiFrom.ToString();
      frmSearch.strDesc = _folioFilter.fiTo.ToString();
      frmSearch.enumWindow = EnumWindow.FoliosCxC;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _folioFilter.fiFrom = Convert.ToInt32(frmSearch.strID);
        _folioFilter.fiTo = Convert.ToInt32(frmSearch.strDesc);
        _nStatus = frmSearch.nStatus;
        LoadFoliosCXC();
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
    /// [emoguel] created 22/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmFolioCXCDetail frmFolioDetail = new frmFolioCXCDetail();
      frmFolioDetail.Owner = this;
      frmFolioDetail.enumMode = EnumMode.Add;
      if(frmFolioDetail.ShowDialog()==true)
      {
        FolioCXC folioCXC = frmFolioDetail.folioCXC;

        if(ValidateFilter(folioCXC))
        {
          List<FolioCXC> lstFoliosCXC = (List<FolioCXC>)dgrFoliosCXC.ItemsSource;
          lstFoliosCXC.Add(folioCXC);
          lstFoliosCXC.Sort((x, y) => x.fiID.CompareTo(y.fiID));
          int nIndex = lstFoliosCXC.IndexOf(folioCXC);
          dgrFoliosCXC.Items.Refresh();
          GridHelper.SelectRow(dgrFoliosCXC, nIndex);
        }
      }
    }

    #endregion

    #region Refresh
    /// <summary>
    /// Recarga el grid de folios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      FolioCXC folioCxc = (FolioCXC)dgrFoliosCXC.SelectedItem;
      LoadFoliosCXC(folioCxc);
    }
    #endregion

    #region Cell Double click
    /// <summary>
    /// Muestra la ventada detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      FolioCXC folioCXC = (FolioCXC)dgrFoliosCXC.SelectedItem;
      frmFolioCXCDetail frmFolioDetail = new frmFolioCXCDetail();
      frmFolioDetail.Owner = this;
      frmFolioDetail.oldFolioCxc = folioCXC;
      frmFolioDetail.enumMode = ((_blnEdit==true)?EnumMode.Edit:EnumMode.ReadOnly);

      if(frmFolioDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<FolioCXC> lstFoliosCXC = (List<FolioCXC>)dgrFoliosCXC.ItemsSource;
        if(!ValidateFilter(frmFolioDetail.folioCXC))//Validar si cumple con los filtros
        {
          lstFoliosCXC.Remove(folioCXC);//Quitamos de la lista
        }
        else
        {
          ObjectHelper.CopyProperties(folioCXC, frmFolioDetail.folioCXC);//Actualizamos con los datos nuevos
          lstFoliosCXC.Sort((x, y) => x.fiID.CompareTo(y.fiID));
          nIndex = lstFoliosCXC.IndexOf(folioCXC);
        }

        dgrFoliosCXC.Items.Refresh();
        StatusBarReg.Content = lstFoliosCXC.Count+" Folios.";
        GridHelper.SelectRow(dgrFoliosCXC,nIndex);
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
    /// [created] 22/03/2016
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

    #region Métodos

    #region LoadFolios
    /// <summary>
    /// Llena el grid de folios
    /// </summary>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private async void LoadFoliosCXC(FolioCXC folioCxc=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<FolioCXC> lstFoliosCXC = await BRFoliosCXC.GetFoliosCXC(_folioFilter, _nStatus);
        dgrFoliosCXC.ItemsSource = lstFoliosCXC;
        if (folioCxc != null && lstFoliosCXC.Count > 0)
        {
          folioCxc = lstFoliosCXC.Where(fi => fi.fiID == folioCxc.fiID).FirstOrDefault();
          nIndex = lstFoliosCXC.IndexOf(folioCxc);
        }
        GridHelper.SelectRow(dgrFoliosCXC, nIndex);
        StatusBarReg.Content = lstFoliosCXC.Count + " Folios.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Folios CxC");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida si el registro nuevo cumple con los filtros actuales
    /// </summary>
    /// <param name="newFolio"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private bool ValidateFilter(FolioCXC newFolio)
    {

      if (_nStatus != -1)//Validamos que se tenga definido un estatus
      {
        bool blnEstatus = Convert.ToBoolean(_nStatus);
        if (newFolio.fiA != blnEstatus)
        {
          return false;
        }
      }

      if (_folioFilter.fiFrom > 0 && _folioFilter.fiTo > 0)//Validamos que se tenga un rango
      {
        if (!(newFolio.fiFrom >= _folioFilter.fiFrom && newFolio.fiTo <= _folioFilter.fiTo))
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
