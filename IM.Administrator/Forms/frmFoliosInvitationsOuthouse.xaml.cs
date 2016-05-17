using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmFolioInvitationsOutside.xaml
  /// </summary>
  public partial class frmFoliosInvitationsOuthouse : Window
  {
    #region Variables
    private FolioInvitationOuthouse _folioInvOutFilter = new FolioInvitationOuthouse();//Objeto con el filtro de los registros
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//boleano para saber si se tiene permiso para editar o agregar
    #endregion
    public frmFoliosInvitationsOuthouse()
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

    #region Window Loaded
    /// <summary>
    /// llena el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(Model.Enums.EnumPermission.FolioInvitationsOuthouse, Model.Enums.EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadFoliosInvitationOuthouse();
    } 
    #endregion

    #region KeyBoardFocusChange
    /// <summary>
    /// Verifica teclas presionadas con la ventana minimizada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
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
    /// Actualiza el grid de foliosInvOutside
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      FolioInvitationOuthouse folioInvOut = (FolioInvitationOuthouse)dgrFoliosInvOut.SelectedItem;
      LoadFoliosInvitationOuthouse(folioInvOut);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana de detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmFolioInvitationOuthouseDetail frmFolioDetail = new frmFolioInvitationOuthouseDetail();
      frmFolioDetail.Owner = this;
      frmFolioDetail.enumMode = EnumMode.add;
      if(frmFolioDetail.ShowDialog()==true)
      {
        
        if(ValidateFilter(frmFolioDetail.folioInvOut))//Verficamos que cumpla con los filtros
        {
          FolioInvitationOuthouse folioInvOut = frmFolioDetail.folioInvOut;
          List<FolioInvitationOuthouse> lstFoliosInvOut = (List<FolioInvitationOuthouse>)dgrFoliosInvOut.ItemsSource;
          lstFoliosInvOut.Add(folioInvOut);//Agregamos el registro a la lista
          lstFoliosInvOut = lstFoliosInvOut.OrderBy(fi => fi.fiSerie).ThenBy(fi => fi.fiID).ToList();//Reordenamos la lista
          dgrFoliosInvOut.ItemsSource = lstFoliosInvOut;
          dgrFoliosInvOut.Items.Refresh();//Recargamos el grid
          int nIndex = lstFoliosInvOut.IndexOf(folioInvOut);//obtenemos el index del nuevo registro
          GridHelper.SelectRow(dgrFoliosInvOut, nIndex);//Seleccionamos el nuevo registro
          StatusBarReg.Content = lstFoliosInvOut.Count + " Folio Invitations.";//Actualizamos el contador
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
      frmSearch.strID = _folioInvOutFilter.fiFrom.ToString();
      frmSearch.strDesc = _folioInvOutFilter.fiTo.ToString();
      frmSearch.strSerie = _folioInvOutFilter.fiSerie;
      frmSearch.enumWindow =EnumWindow.FoliosInvitationOuthouse;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _folioInvOutFilter.fiSerie = frmSearch.strSerie;
        _folioInvOutFilter.fiFrom = Convert.ToInt32(frmSearch.strID);
        _folioInvOutFilter.fiTo = Convert.ToInt32(frmSearch.strDesc);
        _nStatus = frmSearch.nStatus;
        LoadFoliosInvitationOuthouse();
      }
    } 
    #endregion

    #region Cell Double click
    /// <summary>
    /// Muestra la ventada detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      FolioInvitationOuthouse folioInvOut = (FolioInvitationOuthouse)dgrFoliosInvOut.SelectedItem;
      frmFolioInvitationOuthouseDetail frmFolioDetail = new frmFolioInvitationOuthouseDetail();
      frmFolioDetail.oldFolioInvOut = folioInvOut;
      frmFolioDetail.Owner = this;
      frmFolioDetail.enumMode = ((_blnEdit)?EnumMode.edit:EnumMode.preview);//Asignamos el modo
      if(frmFolioDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<FolioInvitationOuthouse> lstFoliosInvOut = (List<FolioInvitationOuthouse>)dgrFoliosInvOut.ItemsSource;
        if(!ValidateFilter(frmFolioDetail.folioInvOut))//Verificamos si cumple con los filtros
        {
          lstFoliosInvOut.Remove(folioInvOut);//Quitamos el registro
          StatusBarReg.Content = lstFoliosInvOut.Count + " Folio Invitatios.";//Actualizamos el registro
        }
        else
        {
          ObjectHelper.CopyProperties(folioInvOut, frmFolioDetail.folioInvOut);//Asignamos los nuevos valores
          lstFoliosInvOut = lstFoliosInvOut.OrderBy(fi => fi.fiSerie).ThenBy(fi => fi.fiID).ToList();
          dgrFoliosInvOut.ItemsSource = lstFoliosInvOut;
          nIndex = lstFoliosInvOut.IndexOf(folioInvOut);//Buscamos el index del registro
        }
        dgrFoliosInvOut.Items.Refresh();//refrescamos el grid
        GridHelper.SelectRow(dgrFoliosInvOut, nIndex);//Seleccionamos el registro
        
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
    #region LoadFoliosInvitationsOuthouse
    /// <summary>
    /// llena el grid de FolioInvitations
    /// </summary>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void LoadFoliosInvitationOuthouse(FolioInvitationOuthouse folioInvOut=null)
    {
      int nIndex = 0;
      List<FolioInvitationOuthouse> lstFoliosInvOut = BRFoliosInvitationsOuthouse.GetFoliosInvittionsOutside(_folioInvOutFilter, _nStatus);
      dgrFoliosInvOut.ItemsSource = lstFoliosInvOut;
      if(folioInvOut!=null && lstFoliosInvOut.Count>0)        
      {
        folioInvOut = lstFoliosInvOut.Where(fi => fi.fiID == folioInvOut.fiID).FirstOrDefault();
        nIndex = lstFoliosInvOut.IndexOf(folioInvOut);
      }
      GridHelper.SelectRow(dgrFoliosInvOut, nIndex);
      StatusBarReg.Content = lstFoliosInvOut.Count + " Folio Invitations.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un registro nuevo cumpla con los filtros del grid
    /// </summary>
    /// <param name="newFolioInvOut">Objeto a evaluar</param>
    /// <returns>True. Cumple con los filtros | False. No cumple con los filtros</returns>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private bool ValidateFilter(FolioInvitationOuthouse newFolioInvOut)
    {
      if (_nStatus != -1)//Filtro por estatus
      {
        if (newFolioInvOut.fiA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (_folioInvOutFilter.fiID > 0)//Filtro por ID
      {
        if (_folioInvOutFilter.fiID != newFolioInvOut.fiID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_folioInvOutFilter.fiSerie))//Filtro por serie
      {
        if (_folioInvOutFilter.fiSerie != newFolioInvOut.fiSerie)
        {
          return false;
        }
      }

      if (_folioInvOutFilter.fiFrom > 0 && _folioInvOutFilter.fiTo > 0)//Filtro por rango de folios
      { 
        if (_folioInvOutFilter.fiFrom == _folioInvOutFilter.fiTo)
        {
          if(!(_folioInvOutFilter.fiFrom>=newFolioInvOut.fiFrom && _folioInvOutFilter.fiTo<=newFolioInvOut.fiTo))
          {
            return false;
          }
        }
        else
        {
          if (!(newFolioInvOut.fiFrom >= _folioInvOutFilter.fiFrom && newFolioInvOut.fiTo <= _folioInvOutFilter.fiTo))
          {
            return false;
          }
        }
      }

      return true;
    } 
    #endregion
    #endregion

  }
}
