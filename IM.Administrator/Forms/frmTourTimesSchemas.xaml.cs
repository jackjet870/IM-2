using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmTourTimesSchemas.xaml
  /// </summary>
  public partial class frmTourTimesSchemas : Window
  {
    #region variables
    private TourTimesSchema _tourTimeSchemaFilter = new TourTimesSchema();//Objeto con filtros del grid
    private int _nStatus = -1;//Status de los registros del grid
    private bool _blnEdit = false;//Para saber si se tiene permiso para editar
    #endregion
    public frmTourTimesSchemas()
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
    /// [emoguel] created 28/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.TourTimes, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadSchemas();
    }
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 28/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region KeyDown Form
    /// <summary>
    /// Verfica teclas precionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            break;
          }
      }
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            Cell_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }

    #endregion

    #region DoubleClick cell
    /// <summary>
    /// Muestra la ventana detalle en modo edit
    /// </summary>
    /// <history>
    /// [emoguel] 28/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      TourTimesSchema tourTimeSchema = (TourTimesSchema)dgrTourTimesSchemas.SelectedItem;
      frmTourTimeSchemaDetail frmTourTimeSchemaDetail = new frmTourTimeSchemaDetail();
      frmTourTimeSchemaDetail.Owner = this;
      frmTourTimeSchemaDetail.oldTourTimeSchema = tourTimeSchema;
      frmTourTimeSchemaDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.add;
      if(frmTourTimeSchemaDetail.ShowDialog()==true)
      {
        List<TourTimesSchema> lstTourTimesSchemas = (List<TourTimesSchema>)dgrTourTimesSchemas.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmTourTimeSchemaDetail.tourTimeSchema))//Verificamos que cumpla con los filtros actuales
        {
          ObjectHelper.CopyProperties(tourTimeSchema, frmTourTimeSchemaDetail.tourTimeSchema);//Actualizamos los datos del objeto
          lstTourTimesSchemas.Sort((x, Y) => string.Compare(x.tcN, Y.tcN));//Reordenamos la lista
          nIndex = lstTourTimesSchemas.IndexOf(tourTimeSchema);//Obtenemos la posición del registro
        }
        else
        {
          lstTourTimesSchemas.Remove(tourTimeSchema);//Quitamos el registro
        }
        dgrTourTimesSchemas.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrTourTimesSchemas, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstTourTimesSchemas.Count + " Schemas.";//Actualizamos el contador
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = (_tourTimeSchemaFilter.tcID > 0) ? _tourTimeSchemaFilter.tcID.ToString() : "";
      frmSearch.nStatus = _nStatus;
      frmSearch.strDesc = _tourTimeSchemaFilter.tcN;
      frmSearch.enumWindow = EnumWindow.DefaultInt;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _tourTimeSchemaFilter.tcID = Convert.ToInt32(frmSearch.strID);
        _tourTimeSchemaFilter.tcN = frmSearch.strDesc;
        LoadSchemas();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmTourTimeSchemaDetail frmTourTimeSchema = new frmTourTimeSchemaDetail();
      frmTourTimeSchema.Owner = this;
      frmTourTimeSchema.enumMode = EnumMode.add;
      if (frmTourTimeSchema.ShowDialog() == true)
      {
        if (ValidateFilter(frmTourTimeSchema.tourTimeSchema))//Validamos que cumpla con los filtros actuales
        {
          List<TourTimesSchema> lstTourTimesSchemas = (List<TourTimesSchema>)dgrTourTimesSchemas.ItemsSource;
          lstTourTimesSchemas.Add(frmTourTimeSchema.tourTimeSchema);//Agregamos el registro
          lstTourTimesSchemas.Sort((x, y) => string.Compare(x.tcN, y.tcN));//Reordenamos la lista
          int nIndex = lstTourTimesSchemas.IndexOf(frmTourTimeSchema.tourTimeSchema);//Buscamos la posicion del registro
          dgrTourTimesSchemas.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrTourTimesSchemas, nIndex);//Seleccionar el registro
          StatusBarReg.Content = lstTourTimesSchemas.Count + " Schemas";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      TourTimesSchema tourTimesSchema = (TourTimesSchema)dgrTourTimesSchemas.SelectedItem;
      LoadSchemas(tourTimesSchema);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadSchemas
    /// <summary>
    /// Llena el grid de Schemas
    /// </summary>
    /// <param name="tourTimeSchema">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private async void LoadSchemas(TourTimesSchema tourTimeSchema = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        List<TourTimesSchema> lstTourTimesSchemas = await BRTourTimesSchemas.GetTourTimesSchemas(_nStatus, _tourTimeSchemaFilter);
        dgrTourTimesSchemas.ItemsSource = lstTourTimesSchemas;
        int nIndex = 0;
        if (lstTourTimesSchemas.Count > 0 && tourTimeSchema != null)
        {
          tourTimeSchema = lstTourTimesSchemas.Where(tc => tc.tcID == tourTimeSchema.tcID).FirstOrDefault();
          nIndex = lstTourTimesSchemas.IndexOf(tourTimeSchema);
        }
        GridHelper.SelectRow(dgrTourTimesSchemas, nIndex);
        StatusBarReg.Content = lstTourTimesSchemas.Count + " Schemas.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Schemas");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un TourTimeSchema cumpla con los filtros actuales
    /// </summary>
    /// <param name="tourTimeSchema">Objeto a validar</param>
    /// <returns>True. Si cumple | False. no cumple</returns>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private bool ValidateFilter(TourTimesSchema tourTimeSchema)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(tourTimeSchema.tcA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(_tourTimeSchemaFilter.tcID>0)//Filtro por ID
      {
        if(tourTimeSchema.tcID!=_tourTimeSchemaFilter.tcID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_tourTimeSchemaFilter.tcN))//Filtro por descripción
      {
        if(!tourTimeSchema.tcN.Contains(_tourTimeSchemaFilter.tcN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }
      return true;
    }
    #endregion
    #endregion
  }
}
