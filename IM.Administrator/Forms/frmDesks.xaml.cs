using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
using System.Linq;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmDesks.xaml
  /// </summary>
  public partial class frmDesks : Window
  {
    private Desk _deskFilter = new Desk();//Objeto con los filtros de la ventana
    private int _nStatus = -1; //Estatus de los registros mostrados en el grid
    public frmDesks()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region Cell Double click
    /// <summary>
    /// Muestra la ventada detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Desk desk = (Desk)dgrDesks.SelectedItem;
      frmDeskDetail frmDeskDetail = new frmDeskDetail();
      frmDeskDetail.enumMode = ModeOpen.edit;
      frmDeskDetail.Owner = this;
      ObjectHelper.CopyProperties(frmDeskDetail.desk, desk);
      if(frmDeskDetail.ShowDialog()==true)
      {
        List<Desk> lstDesk = (List<Desk>)dgrDesks.ItemsSource;        
        int nIndex = 0;
        if (!ValidateFilters(frmDeskDetail.desk))
        {
          lstDesk.Remove(desk);//Quitamos el registro de la lista          
        }
        else
        {
          ObjectHelper.CopyProperties(desk, frmDeskDetail.desk);
          lstDesk.Sort((x, Y) => string.Compare(x.dkN, Y.dkN));//Ordenamos la lista 
          nIndex = lstDesk.IndexOf(desk);
        }        
        dgrDesks.Items.Refresh();//Actualizamos la vista del grid              
        GridHelper.SelectRow(dgrDesks, nIndex);
        StatusBarReg.Content = lstDesk.Count + " Desks.";//Actualizamos el contador   
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
    /// [created] 16/03/2016
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
    /// [emoguel] created 16/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Window Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2015
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadDesks();
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

    #region Search
    /// <summary>
    /// abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 16/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.sID = ((_deskFilter.dkID > 0)?_deskFilter.dkID.ToString():"");
      frmSearch.sDesc = _deskFilter.dkN;
      frmSearch.nStatus = _nStatus;
      frmSearch.sForm = "Desks";
      if (frmSearch.ShowDialog() == true)
      {
        _deskFilter.dkID = Convert.ToInt32(frmSearch.sID);
        _deskFilter.dkN = frmSearch.sDesc;
        _nStatus = frmSearch.nStatus;
        LoadDesks();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmDeskDetail frmDeskDetail = new frmDeskDetail();
      frmDeskDetail.enumMode = ModeOpen.add;
      frmDeskDetail.Owner = this;
      if(frmDeskDetail.ShowDialog()==true)
      {
        Desk desk = frmDeskDetail.desk;
        if (ValidateFilters(desk))//Valida si el registro nuevo cumple con los filtros
        { 
          List<Desk> lstDesk = (List<Desk>)dgrDesks.ItemsSource;
          lstDesk.Add(desk);//Agregamos el registro nuevo
          lstDesk.Sort((x, Y) => string.Compare(x.dkN, Y.dkN));//Ordenamos la lista       
          dgrDesks.Items.Refresh();//Actualizamos la vista del grid
          int nIndex = dgrDesks.Items.IndexOf(desk);//Obtenemos el index del registro nuevo
          GridHelper.SelectRow(dgrDesks, nIndex);//Seleccionamos el registro nuevo
          StatusBarReg.Content = lstDesk.Count + " Desks.";//Actualizamos el contador
        }
      }
    }
    #endregion

    
    #region Refresh
    /// <summary>
    /// Actualiza el grid de Desks
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadDesks();
    }
    #endregion
    #endregion

    #region Métodos
    #region LoadDesks
    /// <summary>
    /// Llena el grid de desks
    /// </summary>
    /// <history>
    /// [emoguel] created 17/03/2016
    /// </history>
    private void LoadDesks()
    {
      List<Desk> lstDesks = BRDesks.GetDesks(_deskFilter, _nStatus);
      dgrDesks.ItemsSource = lstDesks;
      if (lstDesks.Count > 0)
      {
        GridHelper.SelectRow(dgrDesks, 0);
      }

      StatusBarReg.Content = lstDesks.Count + " Desks.";
    }
    #endregion

    #region Validate Filters
    /// <summary>
    /// Valida si una entidad de tipo Desk coincide con los filtros
    /// </summary>
    /// <param name="newDesk">Objeto a validar</param>
    /// <returns>true. Si se muestra | false. Nose muestra</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private bool ValidateFilters(Desk newDesk)
    {
      if (_nStatus != -1)
      {
        if (newDesk.dkA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (_deskFilter.dkID > 0)
      {
        if (_deskFilter.dkID != newDesk.dkID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_deskFilter.dkN))
      {
        if (!newDesk.dkN.Contains(_deskFilter.dkN))
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
