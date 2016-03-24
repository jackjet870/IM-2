using System.Windows;
using System.Windows.Input;
using IM.Administrator.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmEfficiencyTypeDetail.xaml
  /// </summary>
  public partial class frmEfficiencyTypeDetail : Window
  { 
    public EfficiencyType efficiencyType = new EfficiencyType();
    public EnumMode enumMode;//Modo en que se abre la ventana;

    public frmEfficiencyTypeDetail()
    {
      InitializeComponent();
    }
    #region Eventos del formulario
    #region Window KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        Close();
      }
    }
    #endregion
    #region Window Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      OpenMode();
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda o Actualiza dependiendo del modo de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      int nRes = 0;
      string strMsj = "";
      #region Validar Campos
      if(string.IsNullOrWhiteSpace(txtID.Text))//Validamos que tenga un ID
      {
        strMsj += "Specify the Efficiency Type ID \n";
      }

      if(string.IsNullOrWhiteSpace(txtN.Text))//Validamos que tenga la descripcion
      {
        strMsj += "Specify the Efficiency Type Description";
      }
      #endregion
      if(strMsj=="")
      {
        nRes = BREfficiencyTypes.SaveEfficiencyType(efficiencyType, (enumMode == EnumMode.edit));

        #region respuesta
        switch (nRes)//Se valida la respuesta de la operacion
        {
          case 0:
            {
              UIHelper.ShowMessage("Efficiency Type not saved");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Efficiency Type successfully saved");
              DialogResult = true;
              Close();
              break;
            }
          case 2:
            {
              UIHelper.ShowMessage("Efficiency Type ID already exist please select another one");              
              break;
            }
        }
        #endregion
      }
      else
      {
        UIHelper.ShowMessage(strMsj.TrimEnd('\n'));
      }
      
    }
    #endregion
    #endregion

    #region métodos
    #region OpenMode
    /// <summary>
    /// Cambia el estatus de los controles dependiendo del modo
    /// </summary>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private void OpenMode()
    {
      DataContext = efficiencyType;
      switch (enumMode)
      {

        case EnumMode.edit:
          {
            txtN.IsEnabled = true;
            chkA.IsEnabled = true;
            break;
          }
        case EnumMode.add:
          {
            txtID.IsEnabled = true;
            txtN.IsEnabled = true;
            chkA.IsEnabled = true;
            break;
          }
      }
    } 
    #endregion

    #endregion
  }
}
