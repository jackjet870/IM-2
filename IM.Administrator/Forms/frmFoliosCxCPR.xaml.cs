using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmFolioCxCPR.xaml
  /// </summary>
  public partial class frmFoliosCxCPR : Window
  {
    #region Variables
    private PersonnelShort _prFilter = new PersonnelShort();//Objeto con los filtros del grid
    private bool _blnEdit = false;//Filtro para saber si se tiene permiso para editar
    #endregion
    public frmFoliosCxCPR()
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
    /// [emoguel] created 05/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.FolioCXC, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadPR();
    }
    #endregion

    #region Cell Double click
    /// <summary>
    /// Muestra la ventada detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      PersonnelShort personnelShort = (PersonnelShort)dgrPRs.SelectedItem;
      frmFolioCXCPRDetail frmFolioDetail = new frmFolioCXCPRDetail();
      frmFolioDetail.Owner = this;
      frmFolioDetail.enumMode = (_blnEdit)? EnumMode.edit:EnumMode.preview;
      frmFolioDetail.personnel = personnelShort;
      frmFolioDetail.ShowDialog();
    }

    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [created] 05/05/2016
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

    #region IsKeyboardFocusChange
    /// <summary>
    /// Verifica las teclas presionadas con la ventana minimizada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 05/05/2016
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
    /// [emoguel] created 05/05/2016
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

    #region Refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      PersonnelShort personnelShort = (PersonnelShort)dgrPRs.SelectedItem;
      LoadPR(personnelShort);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmFolioCXCPRDetail frmFolioDetail = new frmFolioCXCPRDetail();
      frmFolioDetail.Owner = this;
      frmFolioDetail.enumMode = EnumMode.add;
      if (frmFolioDetail.ShowDialog() == true)
      {
        if (ValidateFilter(frmFolioDetail.personnel))//Verificamos que cumpla con los filtros actuales
        {
          List<PersonnelShort> lstPersonnel = (List<PersonnelShort>)dgrPRs.ItemsSource;
          lstPersonnel.Add(frmFolioDetail.personnel);//Agregamos el registro
          lstPersonnel.Sort((x, y) => string.Compare(x.peN, y.peN));//Ordenamos la lista
          int nIndex = lstPersonnel.IndexOf(frmFolioDetail.personnel);//Obtenemos la posición
          dgrPRs.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPRs, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstPersonnel.Count + " PRs.";//Actualizamos el contador
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
    /// [emoguel] created 05/05/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmFolioCXCPRDetail frmFolioDetail = new frmFolioCXCPRDetail();
      frmFolioDetail.Owner = this;
      frmFolioDetail.personnel = _prFilter;
      frmFolioDetail.enumMode = EnumMode.search;
      if(frmFolioDetail.ShowDialog()==true)
      {
        _prFilter = frmFolioDetail.personnel;
        LoadPR();
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadPR
    /// <summary>
    /// Llena el grid de Prs
    /// </summary>
    /// <param name="personnelShort">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    private void LoadPR(PersonnelShort personnelShort = null)
    {
      int nIndex = 0;
      List<PersonnelShort> lstPrs = BRFoliosCXCPR.GetPRByFoliosCXC(_prFilter);
      dgrPRs.ItemsSource = lstPrs;
      if (lstPrs.Count > 0 && personnelShort != null)
      {
        personnelShort = lstPrs.Where(pe => pe.peID == personnelShort.peID).FirstOrDefault();
        nIndex = lstPrs.IndexOf(personnelShort);
      }
      GridHelper.SelectRow(dgrPRs, nIndex);
      StatusBarReg.Content = lstPrs.Count + " PRs.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Verifica que cumpla con los filtros actuales
    /// </summary>
    /// <param name="personnel">objeto a validar</param>
    /// <returns>True. SI cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// </history>
    private bool ValidateFilter(PersonnelShort personnel)
    {
      if (!string.IsNullOrWhiteSpace(_prFilter.peID))
      {
        if (personnel.peID != _prFilter.peID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_prFilter.deN))
      {
        if (_prFilter.deN != personnel.deN)
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
