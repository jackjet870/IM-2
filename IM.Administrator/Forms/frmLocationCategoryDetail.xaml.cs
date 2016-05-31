using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmLocationCategory.xaml
  /// </summary>
  public partial class frmLocationCategoryDetail : Window
  {
    #region variables
    public LocationCategory locationCategory = new LocationCategory();//Objeto a guardar
    public LocationCategory oldLocationCategory = new LocationCategory();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abre la ventana
    private List<Location> _oldLocations = new List<Location>();//Lista inicial de Locations
    bool isCellCancel = false;
    bool blnClosing = false;
    #endregion
    public frmLocationCategoryDetail()
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
    /// [emoguel] created 17/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(locationCategory, oldLocationCategory);
      UIHelper.SetUpControls(locationCategory, this);
      txtlcID.IsEnabled = (enumMode == EnumMode.add);
      DataContext = locationCategory;
      loadLocations();
    }
    #endregion

    #region keyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region dgrLocation_CellEditEnding
    /// <summary>
    /// VAlida que un location no se pueda seleccionar mas de una vex
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private void dgrLocation_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrLocation);
        e.Cancel = isRepeat;
      }
      else
      {
        isCellCancel = true;
      }

    }
    #endregion

    #region dgrLocation_RowEditEnding
    /// <summary>
    /// Verifica si se esta cancelando la edicion para que no se agregue una fila vacia
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void dgrLocation_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (isCellCancel)
      {
        dgrLocation.RowEditEnding -= dgrLocation_RowEditEnding;
        dgrLocation.CancelEdit();
        dgrLocation.RowEditEnding += dgrLocation_RowEditEnding;
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
    /// [emoguel] created 17/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      List<Location> lstLocations = (List<Location>)dgrLocation.ItemsSource;
      if (!ObjectHelper.IsEquals(locationCategory, oldLocationCategory) || !ObjectHelper.IsListEquals(lstLocations, _oldLocations))
      {
        MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
        if (result == MessageBoxResult.Yes)
        {
          if (!blnClosing) { blnClosing = true; Close(); }
        }
        else
        {
          blnClosing = false;
        }
      }
      else
      {
        if (!blnClosing) { blnClosing = true; Close(); }
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega| Actualiza los datos de un location
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<Location> lstLocations = (List<Location>)dgrLocation.ItemsSource;
      if (enumMode != EnumMode.add && ObjectHelper.IsEquals(locationCategory, oldLocationCategory) && ObjectHelper.IsListEquals(lstLocations, _oldLocations))
      {
        blnClosing = true;
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Location");
        if (strMsj == "")
        {
          List<Location> lstAdd = lstLocations.Where(lo => !_oldLocations.Any(loo => loo.loID == lo.loID)).ToList();
          List<Location> lstDel = _oldLocations.Where(lo => !lstLocations.Any(loo => loo.loID == lo.loID)).ToList();
          int nRes = BRLocationsCategories.SaveLocationCategories(locationCategory,lstAdd,lstDel,(enumMode==EnumMode.edit));
          UIHelper.ShowMessageResult("Location", nRes);
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
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Se ejecuta cuando se está cerrando la ventana
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
        blnClosing = true;        
        btnCancel_Click(null, null);
        if (!blnClosing)
        {
          e.Cancel = true;
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region loadLocations
    /// <summary>
    /// Carga el grid y el combo de locations
    /// </summary>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private void loadLocations()
    {
      List<Location> lstAllLocations = BRLocations.GetLocations();
      List<Location> lstLocations = (!string.IsNullOrWhiteSpace(locationCategory.lcID)) ? lstAllLocations.Where(lo => lo.lolc == locationCategory.lcID).ToList() : new List<Location>();
      dgrLocation.ItemsSource = lstLocations;
      cmbLocations.ItemsSource = lstAllLocations;
      _oldLocations = lstLocations.ToList();
    }
    #endregion

    #endregion
    
  }
}
