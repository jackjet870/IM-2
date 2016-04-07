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

    public Area area=new Area();//Variable que se va a guardar o actualizar
    public Area oldArea = new Area();//Variable con los atributos iniciales
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
      ObjectHelper.CopyProperties(area, oldArea);
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
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(area,oldArea)&& mode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string sMsj = ValidateHelper.ValidateForm(this, "Area");
        int nRes = 0;
        
        if (sMsj == "")
        {
          nRes = BRAreas.SaveArea((mode == EnumMode.edit), area);

          UIHelper.ShowMessageResult("Area", nRes);
          if(nRes==1)
          {
            DialogResult = true;
            Close();
          }

        }
        else
        {
          UIHelper.ShowMessage(sMsj);
        }
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
        btnCancel.Focus();
        btnCancel_Click(null, null);
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
    /// [emoguel] 29/03/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if(mode!=EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(area, oldArea))
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
  }
}
