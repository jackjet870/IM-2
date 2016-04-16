using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Enums;
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
      List<Region> lstRegions = BRRegions.GetRegions(1);
      cmbRegion.ItemsSource = lstRegions;
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
      DataContext = area;
      if(mode!=EnumMode.preview)
      {
        txtarN.IsEnabled = true;
        chkA.IsEnabled = true;
        cmbRegion.IsEnabled = true;
        txtarID.IsEnabled = (mode == EnumMode.add);
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetMaxLength(area, this);
      }      
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
