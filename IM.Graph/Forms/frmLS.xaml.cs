using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace IM.Graph.Forms
{
  /// <summary>
  /// Ventana para seleccionar un Lead Source
  /// </summary>
  public partial class frmLS : Window
  {
    #region Atributos

    protected Window _frmBase;
    private IniFileHelper _iniFileHelper;

    #endregion Atributos

    #region Propiedades

    public LeadSource leadSource { get; set; }
    public bool IsValidated { get; set; }

    #endregion Propiedades

    #region Constructores y destructores

    /// <summary>
    /// Contructor Modificado
    /// </summary>
    /// <param name="pParentSplash">Instancia del frmSplash</param>
    /// <history>
    /// [aalcocer] 10/03/2016 Created
    /// </history>
    public frmLS(Window pParentSplash = null)
    {
      InitializeComponent();
      _frmBase = pParentSplash;
    }

    #endregion Constructores y destructores

    #region Metodos

    #region Validate

    /// <summary>
    ///Valida que haya seleccionado un Lead Source
    /// </summary>
    /// <returns>bool</returns>
    /// <history>
    /// [aalcocer] 10/03/2016 Created
    /// </history>
    private bool Validate()
    {
      bool res = true;

      if (cmbPlace.SelectedItem == null)
      {
        UIHelper.ShowMessage("Specify the Location.", MessageBoxImage.Warning);
        res = false;
      }
      else
        leadSource = (LeadSource)cmbPlace.SelectedItem;

      return res;
    }

    #endregion Validate

    #region LoadFromFile

    /// <summary>
    /// Carga el Lead Source del archivo de configuracion
    /// </summary>
    /// <history>
    /// [aalcocer] 10/03/2016 Created
    /// </history>
    private void LoadFromFile()
    {
      string strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (File.Exists(strArchivo))
      {
        _iniFileHelper = new IniFileHelper(strArchivo);
        cmbPlace.SelectedValue = _iniFileHelper.readText("FilterDate", "LeadSource", "");
        btnAceptar.Focus();
      }
      else
        cmbPlace.Focus();
    }

    #endregion LoadFromFile

    #endregion Metodos

    #region Eventos del formulario

    #region Window_Loaded

    /// <summary>
    /// Configura el formulario
    /// </summary>
    /// <history>
    /// [aalcocer] 10/03/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadFromFile();
    }

    #endregion Window_Loaded

    #region Window_ContentRendered

    /// <summary>
    /// Carga el formulario
    /// </summary>
    /// <history>
    /// [aalcocer] 10/03/2016 Created
    /// </history>
    private void Window_ContentRendered(object sender, EventArgs e)
    {
      this.Cursor = Cursors.Wait;
      List<LeadSource> leadsources = BRLeadSources.GetLeadSources(1);
      cmbPlace.ItemsSource = leadsources;
      this.Cursor = null;
    }

    #endregion Window_ContentRendered

    #region btnAceptar_Click

    /// <summary>
    /// Permite seleccionar un Lead Source
    /// </summary>
    /// <history>
    /// [aalcocer] 10/03/2016 Created
    /// </history>
    private void btnAceptar_Click(object sender, RoutedEventArgs e)
    {
      if (!Validate())
        return;
      IsValidated = true;
      Close();

      if (_frmBase != null)
        _frmBase.Hide();
    }

    #endregion btnAceptar_Click

    #region btnCancelar_Click

    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <history>
    /// [aalcocer] 10/03/2016 Created
    /// </history>
    private void btnCancelar_Click(object sender, RoutedEventArgs e)
    {
      if (_frmBase != null)
        _frmBase.Close();
      Close();
    }

    #endregion btnCancelar_Click

    #endregion Eventos del formulario
  }
}