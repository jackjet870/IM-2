using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Helpers;
using IM.Base.Helpers;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmDepts.xaml
  /// </summary>
  public partial class frmDepts : Window
  {
    #region Variables
    private Dept _deptFilter = new Dept();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmDepts()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Llena los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadDepts();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/05/2016
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
    /// [emoguel] created 03/05/2016
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
    /// [emoguel] created 03/05/2016
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
    /// [emoguel] created 03/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Dept dept = (Dept)dgrDepts.SelectedItem;
      frmDeptDetail frmDeptDetail = new frmDeptDetail();
      frmDeptDetail.Owner = this;
      frmDeptDetail.enumMode = EnumMode.edit;
      frmDeptDetail.oldDept = dept;
      if(frmDeptDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Dept> lstDepts = (List<Dept>)dgrDepts.ItemsSource;
        if(ValidateFilter(frmDeptDetail.dept))//Validamos ci cumple con los filtros
        {
          ObjectHelper.CopyProperties(dept, frmDeptDetail.dept);//Actualizamos los datos
          lstDepts.Sort((x, y) => string.Compare(x.deN, y.deN));//Ordenamos la lista
          nIndex = lstDepts.IndexOf(dept);//buscamos la posición del registro
        }
        else
        {
          lstDepts.Remove(dept);//Quitamos el registro
        }
        dgrDepts.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrDepts, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstDepts.Count + " Depts.";//Actualizamos el contador
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <hisotory>
    /// [emoguel] created 03/05/2016
    /// </hisotory>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _deptFilter.deID;
      frmSearch.strDesc = _deptFilter.deN;
      frmSearch.nStatus = _nStatus;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _deptFilter.deID = frmSearch.strID;
        _deptFilter.deN = frmSearch.strDesc;
        LoadDepts();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana de detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmDeptDetail frmDeptDetail = new frmDeptDetail();
      frmDeptDetail.Owner = this;
      frmDeptDetail.enumMode = EnumMode.edit;
      if(frmDeptDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmDeptDetail.dept))//Valida que cumpla con los filtros actuales
        {
          List<Dept> lstDepts = (List<Dept>)dgrDepts.ItemsSource;
          lstDepts.Add(frmDeptDetail.dept);//Agregar el registro
          lstDepts.Sort((x, y) => string.Compare(x.deN, y.deN));//ordenar la lista
          int nIndex = lstDepts.IndexOf(frmDeptDetail.dept);//BUscamos la posición del registro
          dgrDepts.Items.Refresh();//Refrescar la vista
          GridHelper.SelectRow(dgrDepts, nIndex);//Seleccionar el registro
          StatusBarReg.Content = lstDepts.Count + " Depts.";//Actualizar el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Dept dept = (Dept)dgrDepts.SelectedItem;
      LoadDepts(dept);
    }
    #endregion
    #endregion

    #region Method
    #region LoadDepts
    /// <summary>
    /// Llena el grid de Depts
    /// </summary>
    /// <param name="dept">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    private void LoadDepts(Dept dept = null)
    {
      int nIndex = 0;
      List<Dept> lstDepts = BRDepts.GetDepts(_nStatus, _deptFilter);
      dgrDepts.ItemsSource = lstDepts;
      if (lstDepts.Count > 0 && dept != null)
      {
        dept = lstDepts.Where(de => de.deID == dept.deID).FirstOrDefault();
        nIndex = lstDepts.IndexOf(dept);
      }
      GridHelper.SelectRow(dgrDepts, nIndex);
      StatusBarReg.Content = lstDepts.Count + " Depts.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// valida que un objeto dept cumpla con los filtros actuales
    /// </summary>
    /// <param name="dept">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 04/05/2016
    /// </history>
    private bool ValidateFilter(Dept dept)
    {
      if(_nStatus!=-1)//Filtro por Status
      {
        if(dept.deA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_deptFilter.deID))//Filtro por ID
      {
        if(dept.deN!=_deptFilter.deID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_deptFilter.deN))//Filtro por descripción
      {
        if(!dept.deN.Contains(_deptFilter.deN))
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
