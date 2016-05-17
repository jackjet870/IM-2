using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmDeskDetail.xaml
  /// </summary>
  public partial class frmDeskDetail : Window
  {
    #region variables
    public Desk desk = new Desk();//Objeto a edita o agregar
    public Desk oldDesk = new Desk();//Objeto con los datos iniciales
    public EnumMode enumMode;//modo en que se mostrará la ventana  
    private List<Computer> _oldLstComputers = new List<Computer>();//Lista incial de computadoras 
    #endregion
    public frmDeskDetail()
    {
      InitializeComponent();
    }

    #region eventos del formulario
    #region WindowLoaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(desk, oldDesk);
      DataContext = desk;
      txtdkID.Text = ((enumMode == EnumMode.edit) ? desk.dkID.ToString() : "");
      UIHelper.SetUpControls(desk, this);
      LoadGridComputers();
      LoadCmbComputers();
    } 
    #endregion

    #region Window Key Down
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Boton aceptar
    /// <summary>
    /// Agrega o actualiza un registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<Computer> lstComputers = (List<Computer>)dgrComputers.ItemsSource;
      if (enumMode != EnumMode.add && ObjectHelper.IsEquals(desk, oldDesk) && ObjectHelper.IsEquals(lstComputers, _oldLstComputers))
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Desk");
        int nRes = 0;

        if (strMsj == "")
        { 
          List<string> lstIdsComputers = lstComputers.Select(cmp => cmp.cpID).ToList();
          nRes = BRDesks.SaveDesk(desk, (enumMode == EnumMode.edit), lstIdsComputers);
          UIHelper.ShowMessageResult("Desk", nRes);
          if(nRes>0)
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

    #region EndEdit
    /// <summary>
    /// Valida si la computadora no está seleccionada en otra fila
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/03/2016
    /// </history>
    private void dgrComputers_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        List<Computer> lstComputers = (List<Computer>)dgrComputers.ItemsSource;//Los items del grid                   
        Computer computer = (Computer)dgrComputers.SelectedItem;//Valor que se está editando

        var Combobox = (ComboBox)e.EditingElement;
        Computer computerCombo = (Computer)Combobox.SelectedItem;//Valor seleccionado del combo

        if (computerCombo != null)//Se valida que no esté seleccionado en otra fila
        {
          if (computerCombo != computer)//Validar que se esté cambiando el valor
          {
            Computer ComputerVal = lstComputers.Where(c => c.cpID != computer.cpID && c.cpID == computerCombo.cpID).FirstOrDefault();
            if (ComputerVal != null)
            {
              UIHelper.ShowMessage("Computer must not be repeated");
              e.Cancel = true;
            }
          }
        }
      }

    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana pero antes verifica que no se tengan cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.preview)
      {
        List<Computer> lstComputers = (List<Computer>)dgrComputers.ItemsSource;        
        if (!ObjectHelper.IsEquals(desk, oldDesk) || !ObjectHelper.IsListEquals(lstComputers,_oldLstComputers))
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
      else
      {
        Close();
      }
    }
    #endregion
    #endregion

    #region Metodos
    #region LoadComputers
    /// <summary>
    /// Llena el grid de computadoras asignadas al desk
    /// </summary>
    /// <history>
    /// [emoguel] created 17/03/2016
    /// </history>
    private void LoadGridComputers()
    {
      Computer computer = new Computer();
      List<Computer> lstComputers = new List<Computer>();
      if (enumMode == EnumMode.edit)
      {
        computer.cpdk = desk.dkID;
        lstComputers= BRComputers.GetComputers(computer);
        _oldLstComputers = lstComputers.ToList();
      }
      
      dgrComputers.ItemsSource = lstComputers;
    }
    #endregion

    #region LoadCmbComputers
    /// <summary>
    /// Llena el combo de computers del grid
    /// </summary>
    /// <history>
    //// [emoguel] created 17/03/2016
    /// </history>
    private void LoadCmbComputers()
    {
      List<Computer> lstComputers = BRComputers.GetComputers();      
      cmbComputers.ItemsSource = lstComputers;
    }
    #endregion
    
    #endregion
  }
}
