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
  /// Interaction logic for frmReps.xaml
  /// </summary>
  public partial class frmReps : Window
  {
    #region Variables
    private Rep _repFilter = new Rep();//Objeto con los filtros de la ventana
    private int _nStatus = -1;//Estatus de los filtros de la ventana
    private bool _blnEdit = false;
    #endregion
    public frmReps()
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
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.Agencies, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadReps();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 18/04/2016
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
    /// [emoguel] created 18/04/2016
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
    /// [emoguel] created 18/04/2016
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
    /// [emoguel] 18/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Rep rep = (Rep)dgrReps.SelectedItem;
      frmRepDetail frmRepDetail = new frmRepDetail();
      frmRepDetail.Owner = this;
      frmRepDetail.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      frmRepDetail.oldRep = rep;
      if(frmRepDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Rep> lstReps = (List<Rep>)dgrReps.ItemsSource;
        if(ValidateFilter(frmRepDetail.rep))//Validamos si cumple con los filtros
        {
          ObjectHelper.CopyProperties(rep, frmRepDetail.rep);//Actualizamos los datos
          lstReps.Sort((x, y) => string.Compare(x.rpID, y.rpID));//Ordenamos la lista
          nIndex = lstReps.IndexOf(rep);//Obtenemos la posicion del registros
        }
        else
        {
          lstReps.Remove(rep);//Quitamos el registro
        }
        dgrReps.Items.Refresh();
        GridHelper.SelectRow(dgrReps, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstReps.Count + " Reps.";//Actualizamos el contador
      }
      

    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _repFilter.rpID;
      frmSearch.nStatus = _nStatus;
      frmSearch.txtD.Visibility = Visibility.Collapsed;
      frmSearch.lblDes.Visibility = Visibility.Collapsed;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _repFilter.rpID = frmSearch.strID;
        LoadReps();
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
    /// [emoguel] created 18/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmRepDetail frmRepDetail = new frmRepDetail();
      frmRepDetail.Owner = this;
      frmRepDetail.enumMode = EnumMode.Add;
      if (frmRepDetail.ShowDialog() == true)
      {
        Rep rep = frmRepDetail.rep;
        if(ValidateFilter(rep))//Verificamos que cumpla con los filtros
        {
          List<Rep> lstReps = (List<Rep>)dgrReps.ItemsSource;
          lstReps.Add(rep);//Agregamos el registro
          lstReps.Sort((x, y) => string.Compare(x.rpID, y.rpID));//Ordenamos la lista
          int nIndex = lstReps.IndexOf(rep);//Obtenemos la posicion del registro
          dgrReps.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrReps, nIndex);//Seleccionamos el registro nuevo
          StatusBarReg.Content = lstReps.Count + " Reps.";//Actualizamos el contador
        }
      }

    }
    #endregion

    #region refresh
    /// <summary>
    /// Actualiza los registros del Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Rep rep = (Rep)dgrReps.SelectedItem;
      LoadReps(rep);
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadReps
    /// <summary>
    /// Llena el grid de Reps
    /// </summary>
    /// <param name="rep">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// </history>
    private async void LoadReps(Rep rep = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Rep> lstReps = await BRReps.GetReps(_repFilter, _nStatus);
        dgrReps.ItemsSource = lstReps;
        if (lstReps.Count > 0 && rep != null)
        {
          rep = lstReps.Where(rp => rp.rpID == rep.rpID).FirstOrDefault();
          nIndex = lstReps.IndexOf(rep);
        }
        GridHelper.SelectRow(dgrReps, nIndex);
        StatusBarReg.Content = lstReps.Count + " Reps.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Reps");
      }
    }
    #endregion
    #region ValidateFilter
    /// <summary>
    /// Valida que un registro Rep cumpla con los filtros actuales
    /// </summary>
    /// <param name="rep">Objeto a seleccionar</param>
    /// <returns>True. Si comple | false. No cumple</returns>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// </history>
    private bool ValidateFilter(Rep rep)
    {
      if(_nStatus!=-1)
      {
        if(rep.rpA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_repFilter.rpID))
      {
        if(_repFilter.rpID!=rep.rpID)
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
