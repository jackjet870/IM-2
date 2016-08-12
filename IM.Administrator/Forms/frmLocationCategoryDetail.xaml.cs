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
using System;

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
      txtlcID.IsEnabled = (enumMode == EnumMode.Add);
      DataContext = locationCategory;
      loadLocations();
      dgrLocation.BeginningEdit += GridHelper.dgr_BeginningEdit;
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
      if (e.EditAction==DataGridEditAction.Commit)//Verificar si se está cancelando la edición
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
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void dgrLocation_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (isCellCancel)
        {
          dgrLocation.RowEditEnding -= dgrLocation_RowEditEnding;
          dgrLocation.CancelEdit();
          dgrLocation.RowEditEnding += dgrLocation_RowEditEnding;
        }
        else
        {
          cmbLocations.Header = "Location (" + (dgrLocation.Items.Count - 1) + ")";
        }
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
    private  async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<Location> lstLocations = (List<Location>)dgrLocation.ItemsSource;
      if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(locationCategory, oldLocationCategory) && ObjectHelper.IsListEquals(lstLocations, _oldLocations))
      {
        blnClosing = true;
        Close();
      }
      else
      {
        btnAccept.Visibility = Visibility.Collapsed;
        skpStatus.Visibility = Visibility.Visible;
        txtStatus.Text = "Saving data...";
        string strMsj = ValidateHelper.ValidateForm(this, "Location",blnDatagrids:true);
        if (strMsj == "")
        {
          List<Location> lstAdd = lstLocations.Where(lo => !_oldLocations.Any(loo => loo.loID == lo.loID)).ToList();
          List<Location> lstDel = _oldLocations.Where(lo => !lstLocations.Any(loo => loo.loID == lo.loID)).ToList();
          int nRes = await BRLocationsCategories.SaveLocationCategories(locationCategory,lstAdd,lstDel,(enumMode==EnumMode.Edit));
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
        skpStatus.Visibility = Visibility.Collapsed;
        btnAccept.Visibility = Visibility.Visible;
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
        List<Location> lstLocations = (List<Location>)dgrLocation.ItemsSource;
        if (!ObjectHelper.IsEquals(locationCategory, oldLocationCategory) || !ObjectHelper.IsListEquals(lstLocations, _oldLocations))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result != MessageBoxResult.Yes)
          {
            e.Cancel = true;
          }
          else
          {
            dgrLocation.CancelEdit();
          }
        }
      }
    }
    #endregion

    #region KeyDown
		// <summary>
    /// Cambia el contador de los registros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        var item = dgrLocation.SelectedItem;
        if (item.GetType().Name == "Location")
        {
          cmbLocations.Header = "Location (" + (dgrLocation.Items.Count - 2) + ")";
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
    private async void loadLocations()
    {
      try
      {
        List<Location> lstAllLocations = await BRLocations.GetLocations();
        List<Location> lstLocations = (!string.IsNullOrWhiteSpace(locationCategory.lcID)) ? lstAllLocations.Where(lo => lo.lolc == locationCategory.lcID).ToList() : new List<Location>();
        dgrLocation.ItemsSource = lstLocations;
        cmbLocations.ItemsSource = lstAllLocations;
        _oldLocations = lstLocations.ToList();
        skpStatus.Visibility = Visibility.Collapsed;
        btnAccept.Visibility = Visibility.Visible;
        cmbLocations.Header = "Location (" + lstLocations.Count + ")";
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Location Categories");
      }
    }
    #endregion

    #endregion
    
  }
}
