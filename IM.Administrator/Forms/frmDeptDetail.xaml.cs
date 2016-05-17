using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;


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
    CollectionViewSource getPersonnelsView;
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
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      txtdeID.IsEnabled = (enumMode == EnumMode.add);
      ObjectHelper.CopyProperties(dept, oldDept);
      UIHelper.SetUpControls(dept, this);
      DataContext = dept;
      LoadPersonnels(dept.deID);
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] cretaed 03/05/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {

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
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<Personnel> lstPersonnels = (List<Personnel>)dgrPersonnel.ItemsSource;
      if (enumMode != EnumMode.add && ObjectHelper.IsEquals(dept, oldDept) && ObjectHelper.IsListEquals(lstPersonnels, _lstOldPersonnel))
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Dept");
        if (strMsj == "")
        {
          List<Personnel> lstAdd = lstPersonnels.Where(pe => !_lstOldPersonnel.Any(pee => pee.peID == pe.peID)).ToList();
          List<Personnel> lstDel = _lstOldPersonnel.Where(pe => !lstPersonnels.Any(pee => pee.peID == pe.peID)).ToList();
          int nRes = BRDepts.SaveDept(dept,(enumMode==EnumMode.edit),lstAdd,lstDel);
          UIHelper.ShowMessageResult("Dept", nRes);
          if (nRes > 0)
          {
            DialogResult = true;
            Close();
          }
        }
        else
        {
          UIHelper.ShowMessage(strMsj);
        }
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      List<Personnel> lstPersonnels = (List<Personnel>)getPersonnelsView.Source;//dgrPersonnel.ItemsSource;
      if (!ObjectHelper.IsEquals(dept, oldDept) || !ObjectHelper.IsListEquals(lstPersonnels, _lstOldPersonnel))
      {
        MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
        if (result == MessageBoxResult.Yes)
        {
          Close();
        }
      }
      else
      {
        Close();
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
          Personnel personnel = (Personnel)dgrPersonnel.SelectedItem;//Valor que se está editando

          var Combobox = (ComboBox)e.EditingElement;
          Personnel PersonnelCombo = (Personnel)Combobox.SelectedItem;//Valor seleccionado del combo

          if (PersonnelCombo != null)//Se valida que no esté seleccionado en otra fila
          {
            if (PersonnelCombo != personnel)//Validar que se esté cambiando el valor
            {
              Personnel personnelVal = _lstPersonel.Where(pe => pe.peID != personnel.peID && pe.peID == PersonnelCombo.peID).FirstOrDefault();
              if (personnelVal != null)
              {
                UIHelper.ShowMessage("Personnel must not be repeated");
                e.Cancel = true;
              }
              else
              {
                personnel = _lstPersonel[e.Row.GetIndex()];
                ObjectHelper.CopyProperties(personnel, PersonnelCombo);
              }
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
    private void LoadPersonnels(string deptID)
    {
      List<Personnel> lstAllPersonnels = BRPersonnel.GetPersonnels();
      cmbPersonnel.ItemsSource = lstAllPersonnels;
      getPersonnelsView = ((CollectionViewSource)(this.FindResource("getPersonnels")));
      _lstPersonel = lstAllPersonnels.Where(pe => pe.pede == deptID).ToList();
      getPersonnelsView.Source = _lstPersonel;      
      _lstOldPersonnel = lstAllPersonnels.Where(pe => pe.pede == deptID).ToList();//Cargamos la lista con los datos iniciales
    }
    #endregion

    #endregion
  }
}
