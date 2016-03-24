using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAreaDetalle.xaml
  /// </summary>
  public partial class frmAreaDetalle : Window
  {

    public Area area=new Area();//Variable que recibe la entidad
    public EnumMode mode;//Modo en el que se abrirá la ventana        
    public frmAreaDetalle()
    {
      InitializeComponent();
    }

    #region metodos
    #region LoadRegion
    /// <summary>
    /// Cargala lista de regiones
    /// </summary>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    protected void LoadRegions()
    {
      List<Region> lstRegions = BRRegions.GetRegions(new Region(),1);
      cmbRegion.ItemsSource = lstRegions;
    }
    #endregion
    #region modo de la ventana
    /// <summary>
    /// Abre la ventana dependiendo del modo que elija el usuario
    /// Preview|Edit|Add
    /// </summary>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    protected void OpenMode()
    {
      this.DataContext = area;
      switch (mode)
      {
        case EnumMode.preview://show
          {
            btnAccept.Visibility = Visibility.Hidden;            
            break;
          }
        case EnumMode.add://add
          {
            txtID.IsEnabled = true;
            LockControls(true);
            break;
          }
        case EnumMode.edit://Edit
          {
            txtID.IsEnabled = false;
            LockControls(true);            
            break;
          }
      }

    }
    #endregion
    #region Bloquear|Desbloquear controles
    /// <summary>
    /// Bloquea|Desbloquea los botones dependiendo del modo en que se habra
    /// </summary>
    /// <param name="bValue">true para desbloquear| false para bloquear</param>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    protected void LockControls(bool blnValue)
    {
      txtN.IsEnabled = blnValue;
      cmbRegion.IsEnabled = blnValue;
      chkA.IsEnabled = blnValue;      
    }
    #endregion
    #endregion
    #region evento botones

    #region Loaded
    /// <summary>
    /// Carga los controles al abrir la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadRegions();
      OpenMode();
    }
    #endregion
    
    #region Boton Aceptar
    /// <summary>
    /// Agrega o actualiza los registros del catalogo Areas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {      
      string sMsj = "";
      int nRes = 0;
      #region validar campos
      if (string.IsNullOrWhiteSpace(txtID.Text))//ID
      {
        sMsj += "Specify the Area ID. \n";
      }
      if (string.IsNullOrWhiteSpace(txtN.Text))
      {
        sMsj += "Specify the Area Description. \n";
      }
      if (cmbRegion.SelectedIndex < 0)
      {
        sMsj += "Specify the Region.";
      }
      #endregion
      if (sMsj == "")
      {
        nRes = BRAreas.SaveArea((mode==EnumMode.edit), area);        

        #region Respuesta
        switch (nRes)//Se valida la repuesta
        {
          case 0:
            {
              UIHelper.ShowMessage("Area not saved");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Area successfully saved");
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              UIHelper.ShowMessage("Area ID already exist please select another one");
              break;
            }
        }
        #endregion
      }
      else
      {
        UIHelper.ShowMessage(sMsj.TrimEnd('\n'));
      }
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventada detalle con el boton escape dependiendo del modo en que fue abierto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
          DialogResult = false;
          Close();
        
      }
    }
    #endregion

    #endregion    
  }
}
