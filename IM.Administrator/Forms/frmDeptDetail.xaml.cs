using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmDeptDetail.xaml
  /// </summary>
  public partial class frmDeptDetail : Window
  {
    #region variables
    public Dept dept = new Dept();//objeto a guardar
    public Dept oldDept = new Dept();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    private List<Personnel> _lstOldPersonnel = new List<Personnel>();//Lista inicial de personnel
    private List<Personnel> _lstPersonel = new List<Personnel>();//Lista de Personnel para el viewSource
    private bool _blnIsCellCancel=false;
    private bool blnClosing = false;
    #endregion
    public frmDeptDetail()
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
    /// [emoguel] created 03/05/2016
    /// [emoguel] modified 29/07/2016---->se agregó la suscripción al evento begin edit
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      txtdeID.IsEnabled = (enumMode == EnumMode.add);
      ObjectHelper.CopyProperties(dept, oldDept);
      UIHelper.SetUpControls(dept, this);
      DataContext = dept;
      LoadPersonnels(dept.deID);
      dgrPersonnel.BeginningEdit += GridHelper.dgr_BeginningEdit;
    }
    #endregion

    #region Accept
    // <summary>
    /// Agrega| Actualiza registros en el catalogo Dept
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// 
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<Personnel> lstPersonnels = (List<Personnel>)dgrPersonnel.ItemsSource;
      if (enumMode != EnumMode.add && ObjectHelper.IsEquals(dept, oldDept) && ObjectHelper.IsListEquals(lstPersonnels, _lstOldPersonnel))
      {
        blnClosing = true;
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Dept",blnDatagrids:true);
        if (strMsj == "")
        {
          btnAccept.Visibility = Visibility.Collapsed;
          List<Personnel> lstAdd = lstPersonnels.Where(pe => !_lstOldPersonnel.Any(pee => pee.peID == pe.peID)).ToList();
          List<Personnel> lstDel = _lstOldPersonnel.Where(pe => !lstPersonnels.Any(pee => pee.peID == pe.peID)).ToList();
          int nRes = await BRDepts.SaveDept(dept,(enumMode==EnumMode.edit),lstAdd,lstDel);
          UIHelper.ShowMessageResult("Dept", nRes);
          if (nRes > 0)
          {
            blnClosing = true;
            DialogResult = true;
            Close();
          }
        }
        else
        {
          UIHelper.ShowMessage(strMsj);
        }
        btnAccept.Visibility = Visibility.Visible;
      }
    }
    #endregion  

    #region EndEdit
    /// <summary>
    /// Valida que el Sales Room no esté seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    private void dgrPersonnels_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        _blnIsCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrPersonnel, true);
        e.Cancel = isRepeat;
      }
      else
      {
        _blnIsCellCancel = true;
      }
    }
    #endregion

    #region RowEndEdit
    /// <summary>
    /// Actualiza la fila seleccionada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/05/2016
    /// </history>
    private void dgrPersonnel_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        dgrPersonnel.RowEditEnding -= dgrPersonnel_RowEditEnding;
        if (_blnIsCellCancel)
        {
          dgrPersonnel.CancelEdit();
        }
        else
        {
          dgrPersonnel.CommitEdit();
          dgrPersonnel.Items.Refresh();
          GridHelper.SelectRow(dgrPersonnel, dgrPersonnel.SelectedIndex);
        }
        dgrPersonnel.RowEditEnding += dgrPersonnel_RowEditEnding;
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing)
      {
        List<Personnel> lstPersonnels = (List<Personnel>)dgrPersonnel.ItemsSource;
        if (!ObjectHelper.IsEquals(dept, oldDept) || !ObjectHelper.IsListEquals(lstPersonnels, _lstOldPersonnel))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrPersonnel.CancelEdit();
          }
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadPersonnels
    /// <summary>
    /// Llena los datos de Personnels
    /// </summary>
    /// <param name="deptID">Id del depto</param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    private async void LoadPersonnels(string deptID)
    {
      try
      {
        List<Personnel> lstAllPersonnels = await BRPersonnel.GetPersonnels();
        cmbPersonnel.ItemsSource = lstAllPersonnels;
        _lstPersonel = lstAllPersonnels.Where(pe => pe.pede == deptID).ToList();
        dgrPersonnel.ItemsSource = _lstPersonel;
        _lstOldPersonnel = lstAllPersonnels.Where(pe => pe.pede == deptID).ToList();//Cargamos la lista con los datos iniciales
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Depts");
      }
    }

    #endregion

    #endregion
  }
}
