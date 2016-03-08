using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAreaDetalle.xaml
  /// </summary>
  public partial class frmAreaDetalle : Window
  {

    public Area area;//Variable que recibe la entidad
    public ModeOpen mode;//Modo en el que se abrirá la ventana    
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
      List<Region> lstRegions = BRRegions.GetRegions(1);
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
      switch (mode)
      {
        case ModeOpen.preview://show
          {
            btnAccept.Visibility = Visibility.Hidden;            
            this.DataContext = area;
            break;
          }
        case ModeOpen.add://add
          {
            txtID.IsEnabled = true;
            LockControls(true);
            break;
          }
        case ModeOpen.edit://Edit
          {
            txtID.IsEnabled = false;
            LockControls(true);
            this.DataContext = area;
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

    #region Boton cancelar
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      this.Close();
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
        switch (mode)
        {
          #region add
          case ModeOpen.add://add
            {

              area = new Area { arID = txtID.Text, arN = txtN.Text, arrg = cmbRegion.SelectedValue.ToString(), arA = chkA.IsChecked.Value };
              nRes = BRAreas.SaveArea(false, area);
              break;
            }
          #endregion
          #region Edit
          case ModeOpen.edit://edit
            {
              area = (Area)this.DataContext;
              nRes = BRAreas.SaveArea(true, area);
              break;
            }
            #endregion
        }

        #region Respuesta
        switch (nRes)//Se valida la repuesta
        {
          case 0:
            {
              MessageBox.Show("Area not saved", "", MessageBoxButton.OK, MessageBoxImage.Warning);
              break;
            }
          case 1:
            {
              MessageBox.Show("Area successfully saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              MessageBox.Show("Area ID already exist please select another one", "", MessageBoxButton.OK, MessageBoxImage.Warning);
              break;
            }
        }
        #endregion
      }
      else
      {
        MessageBox.Show(sMsj);
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
        if (mode == ModeOpen.preview)
        {
          DialogResult = false;
          this.Close();
        }
        else
        {
          MessageBoxResult msgResult = MessageBox.Show("Are you sure close this window?", "Close confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
          if (msgResult == MessageBoxResult.Yes)
          {
            DialogResult = false;
            this.Close();
          }
        }
      }
    } 
    #endregion
    #endregion

  }
}
