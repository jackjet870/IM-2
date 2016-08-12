using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using System;
using System.Linq;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAssistanceStatus.xaml
  /// </summary>
  public partial class frmAssistancesStatus : Window
  {
    private AssistanceStatus _assistanceFilter = new AssistanceStatus();//Objeto a filtrar en el grid
    private int _nStatus = -1;//Estatus de los registros que se muestran en el grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permisos para editar|agregar
    public frmAssistancesStatus()
    {
      InitializeComponent();
    }

    #region eventos de los controles
    #region Loaded form
    /// <summary>
    /// carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadAssitance();
    }

    #endregion

    #region KeyBoardChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 09/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region KeyDown Form
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
    /// Muestra la ventana de AssistanceStatusDetail en modo ReadOnly
    /// </summary>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      AssistanceStatus assistance = (AssistanceStatus)dgrAssitances.SelectedItem;
      frmAssistanceStatusDetail frmAssistanceDetail = new frmAssistanceStatusDetail();
      frmAssistanceDetail.oldAssistance = assistance;
      frmAssistanceDetail.Owner = this;
      frmAssistanceDetail.mode = ((_blnEdit == true) ? EnumMode.Edit : EnumMode.ReadOnly);
      if (frmAssistanceDetail.ShowDialog() == true)
      {
        List<AssistanceStatus> lstAssistancesStatus = (List<AssistanceStatus>)dgrAssitances.ItemsSource;
        int nIndex = 0;
        if(!ValidateFilters(frmAssistanceDetail.assistance))//Validamos si cumple con los filtros
        {
          lstAssistancesStatus.Remove(assistance);//Quitamos el registro
        }
        else
        {
          ObjectHelper.CopyProperties(assistance, frmAssistanceDetail.assistance);
          lstAssistancesStatus.Sort((x, y) => string.Compare(x.atN, y.atN));//ordenamos la lista        
          nIndex = lstAssistancesStatus.IndexOf(assistance);
        }        
        dgrAssitances.Items.Refresh();//regrescamos el grid
        GridHelper.SelectRow(dgrAssitances, nIndex);
        StatusBarReg.Content = lstAssistancesStatus.Count + " Assistances Status.";//Actualizamos el contador
      }

    }

    #endregion

    #region Refresh grid
    /// <summary>
    /// Actualiza la lista de Status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created.
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      AssistanceStatus assitanceStatus = (AssistanceStatus)dgrAssitances.SelectedItem;
      LoadAssitance(assitanceStatus);
    }
    #endregion

    #region Add
    /// <summary>
    /// Muestra la ventana de AssistanceStatusDetail para agregar un registro nuevo
    /// </summary>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {

      frmAssistanceStatusDetail frmAssistanceDetail = new frmAssistanceStatusDetail();
      frmAssistanceDetail.Owner = this;
      frmAssistanceDetail.mode = EnumMode.Add;//insertar
      if (frmAssistanceDetail.ShowDialog() == true)
      {
        if (ValidateFilters(frmAssistanceDetail.assistance))//Validamos si cumple con los filtros
        {
          List<AssistanceStatus> lstAssistancesStatus = (List<AssistanceStatus>)dgrAssitances.ItemsSource;
          lstAssistancesStatus.Add(frmAssistanceDetail.assistance);//Agregamos el registro nuevo
          lstAssistancesStatus.Sort((x, y) => string.Compare(x.atN, y.atN));//ordenamos la lista
          int nIndex = lstAssistancesStatus.IndexOf(frmAssistanceDetail.assistance);//Obtenemos el index del registro nuevo
          dgrAssitances.Items.Refresh();//regrescamos el grid
          GridHelper.SelectRow(dgrAssitances, nIndex);
          StatusBarReg.Content = lstAssistancesStatus.Count + " Assistances Status.";//Actualizamos el contador
        }
      }

    }

    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda cargando 
    /// los datos de busqueda que ya se hayan realizado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 3/Mar/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.nStatus = _nStatus;
      frmSearch.strID = _assistanceFilter.atID;
      frmSearch.strDesc = _assistanceFilter.atN;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _assistanceFilter.atID = frmSearch.strID;
        _assistanceFilter.atN = frmSearch.strDesc;

        LoadAssitance();
      }
    }
    #endregion
    #endregion

    #region metodos
    #region Load Assistance
    /// <summary>
    /// carga la lista de Assistance Status
    /// </summary>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>    
    protected async void LoadAssitance(AssistanceStatus assistanceStatus=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<AssistanceStatus> lstAssistance =await BRAssistancesStatus.GetAssitanceStatus(_assistanceFilter, _nStatus);
        dgrAssitances.ItemsSource = lstAssistance;
        if (assistanceStatus != null && lstAssistance.Count > 0)
        {
          assistanceStatus = lstAssistance.Where(ass => ass.atID == assistanceStatus.atID).FirstOrDefault();
          nIndex = lstAssistance.IndexOf(assistanceStatus);
        }
        GridHelper.SelectRow(dgrAssitances, nIndex);
        StatusBarReg.Content = lstAssistance.Count + " Assistance Status.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Assistance Status");
      }
    }
    #endregion

    #region Validate Filters
    /// <summary>
    /// Valida si una entidad de tipo Area coincide con los filtros
    /// </summary>
    /// <param name="newAssistance">Objeto a validar</param>
    /// <returns>true. Si se muestra | false. Nose muestra</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private bool ValidateFilters(AssistanceStatus newAssistance)
    {
      if (_nStatus != -1)
      {
        if (newAssistance.atA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_assistanceFilter.atID))
      {
        if (_assistanceFilter.atID != newAssistance.atID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_assistanceFilter.atN))
      {
        if (!newAssistance.atN.Contains(_assistanceFilter.atN,StringComparison.OrdinalIgnoreCase))
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
