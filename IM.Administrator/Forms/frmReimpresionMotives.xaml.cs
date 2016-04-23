using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmReimpresionMotives.xaml
  /// </summary>
  public partial class frmReimpresionMotives : Window
  {
    #region Variables
    private ReimpresionMotive _reimpresionMotiveFilter = new ReimpresionMotive();//Objeto con los filtros de la venata
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmReimpresionMotives()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadReimpresionMotives();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 15/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region KeyDown Form
    /// <summary>
    /// Verfica teclas precionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 15/04/2016
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
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
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

    #region DoubleClick cell
    /// <summary>
    /// Muestra la ventana detalle en modo edit
    /// </summary>
    /// <history>
    /// [emoguel] 15/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      ReimpresionMotive reimpresionMotive = (ReimpresionMotive)dgrReimpresionMotives.SelectedItem;
      frmReimpresionMotiveDetail frmReimpresionMotive = new frmReimpresionMotiveDetail();
      frmReimpresionMotive.Owner = this;
      frmReimpresionMotive.enumMode = EnumMode.edit;
      frmReimpresionMotive.oldReimpresionMotive = reimpresionMotive;
      if(frmReimpresionMotive.ShowDialog()==true)
      {
        int nIndex = 0;
        List<ReimpresionMotive> lstReimpresionMotive = (List<ReimpresionMotive>)dgrReimpresionMotives.ItemsSource;
        if(ValidateFilter(frmReimpresionMotive.reimpresionMotive))//Verificamos que cumpla con los filtros actuales del grid
        {
          ObjectHelper.CopyProperties(reimpresionMotive, frmReimpresionMotive.reimpresionMotive);//Actualizamos los valores
          lstReimpresionMotive.Sort((x, y) => string.Compare(x.rmN, y.rmN));//Ordenamos la lista
          nIndex = lstReimpresionMotive.IndexOf(reimpresionMotive);//Obtenemos el index del registro
        }
        else
        {
          lstReimpresionMotive.Remove(reimpresionMotive);//Quitamos el registro
        }
        dgrReimpresionMotives.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrReimpresionMotives, nIndex);//Seleccionamos el contador
        StatusBarReg.Content = lstReimpresionMotive.Count + " Reimpresion Motives.";//Actualizamos el contador
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
    /// [emoguel] created 16/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.txtID.MaxLength = 3;
      frmSearch.strID = (_reimpresionMotiveFilter.rmID>0)?_reimpresionMotiveFilter.rmID.ToString():"";
      frmSearch.strDesc = _reimpresionMotiveFilter.rmN;
      frmSearch.nStatus = _nStatus;
      frmSearch.enumWindow = EnumWindow.DefaultByte;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _reimpresionMotiveFilter.rmID = Convert.ToByte(frmSearch.strID);
        _reimpresionMotiveFilter.rmN = frmSearch.strDesc;
        LoadReimpresionMotives();
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
    /// [emoguel] created 16/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmReimpresionMotiveDetail frmReimpresionMotive = new frmReimpresionMotiveDetail();
      frmReimpresionMotive.Owner = this;
      frmReimpresionMotive.enumMode = EnumMode.add;
      if(frmReimpresionMotive.ShowDialog()==true)
      {
        ReimpresionMotive reimpresionMotive = frmReimpresionMotive.reimpresionMotive;
        if(ValidateFilter(reimpresionMotive))//Verificamos si cumple con los filtros actuales
        {
          List<ReimpresionMotive> lstReimpresionMotive = (List<ReimpresionMotive>)dgrReimpresionMotives.ItemsSource;
          lstReimpresionMotive.Add(reimpresionMotive);//Agregamos el registro nuevo
          lstReimpresionMotive.Sort((x, y) => string.Compare(x.rmN, y.rmN));//Ordenamos la lista
          int nIndex = lstReimpresionMotive.IndexOf(reimpresionMotive);//Obtemos la posicion del nuevo registro
          dgrReimpresionMotives.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrReimpresionMotives, nIndex);//Seleccionamos el registro nuevo
          StatusBarReg.Content = lstReimpresionMotive.Count + " Reimpresion Motives.";//Actualizamos el contador
        }
      }
    } 
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros del grid 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/06/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      ReimpresionMotive reimpresionMotive = (ReimpresionMotive)dgrReimpresionMotives.SelectedItem;
      LoadReimpresionMotives(reimpresionMotive);
    } 
    #endregion
    #endregion

    #region
    #region LoadReimpresionMotives
    /// <summary>
    /// Llena el grid de Reimpresion Motives
    /// </summary>
    /// <param name="reimpresionMotive">registro a seleccionar</param>
    /// <history>
    /// [emoguel] created 16/04/2016
    /// </history>
    private void LoadReimpresionMotives(ReimpresionMotive reimpresionMotive = null)
    {
      int nIndex = 0;
      List<ReimpresionMotive> lstReimpresionMotive = BRReimpresionMotives.GetReimpresionMotives(_nStatus, _reimpresionMotiveFilter);
      dgrReimpresionMotives.ItemsSource = lstReimpresionMotive;

      if (lstReimpresionMotive.Count > 0 && reimpresionMotive != null)
      {
        reimpresionMotive = lstReimpresionMotive.Where(rm => rm.rmID == reimpresionMotive.rmID).FirstOrDefault();
        nIndex = lstReimpresionMotive.IndexOf(reimpresionMotive);
      }
      GridHelper.SelectRow(dgrReimpresionMotives, nIndex);
      StatusBarReg.Content = lstReimpresionMotive.Count + " Reimpresion Motives.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un objeto ReimpresionMotive
    /// cumpla con los filtros actuales
    /// </summary>
    /// <param name="reimpresionMotive">objeto a validar</param>
    /// <returns>True. si cump´le | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 16/04/2016
    /// </history>
    private bool ValidateFilter(ReimpresionMotive reimpresionMotive)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(reimpresionMotive.rmA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }
      if (_reimpresionMotiveFilter.rmID > 0)//filtro por ID
      {
        if (_reimpresionMotiveFilter.rmID != reimpresionMotive.rmID)
        {
          return false;
        }
      }
      if (!string.IsNullOrWhiteSpace(_reimpresionMotiveFilter.rmN))//Filtro por descripción
      {
        if (!reimpresionMotive.rmN.Contains(_reimpresionMotiveFilter.rmN))
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
