using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Administrator.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmDeskDetail.xaml
  /// </summary>
  public partial class frmDeskDetail : Window
  {
    public Desk desk = new Desk();//Objeto a edita o agregar
    public ModeOpen enumMode;//modo en que se mostrará la ventana    
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
      DataContext = desk;
      txtID.Text = ((enumMode == ModeOpen.edit) ? desk.dkID.ToString() : "");      
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
        Close();
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
      string strMsj = "";
      int nRes = 0;

      if(string.IsNullOrWhiteSpace(txtN.Text))
      {
        strMsj += "Specify the Desk name \n";
      }

      if(strMsj=="")
      {
        List<Computer> lstComputers = (List<Computer>)dgrComputers.ItemsSource;
        List<string> lstIdsComputers = lstComputers.Select(cmp => cmp.cpID).ToList();
        nRes = BRDesks.SaveDesk(desk,(enumMode==ModeOpen.edit), lstIdsComputers);

        #region respuesta
        switch (nRes)//Se valida la respuesta de la operacion
        {
          case 0:
            {
              UIHelper.ShowMessage("Desk not saved");
              break;
            }          
          case 2:
            {
              if (enumMode == ModeOpen.add)
              {
                UIHelper.ShowMessage("Desk ID already exist please select another one");
              }
              else
              {
                UIHelper.ShowMessage("Desk successfully saved");
                DialogResult = true;
                Close();
              }
              break;
            }
          default:
            {
              UIHelper.ShowMessage("Desk successfully saved");
              DialogResult = true;
              Close();
              break;
            }
        }
        #endregion
      }
      else
      {
        UIHelper.ShowMessage(strMsj);
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
      if (enumMode == ModeOpen.edit)
      {
        computer.cpdk = desk.dkID;
        lstComputers= BRComputers.GetComputers(computer);
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
