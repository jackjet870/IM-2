using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmReasonCancellationFolios.xaml
  /// </summary>
  public partial class frmReasonCancellationFolios : Window
  {
    #region Variables
    private ReasonCancellationFolio _reasonCancelFolFilter = new ReasonCancellationFolio();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar
    #endregion
    public frmReasonCancellationFolios()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window Loaded
		// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.FoliosInvitationsOuthouse, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadReasonCancellationFolios();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 14/04/2016
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
    /// [emoguel] created 14/04/2016
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
    /// [emoguel] created 14/04/2016
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
    /// [emoguel] 14/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      ReasonCancellationFolio reasonCancellationFolio = (ReasonCancellationFolio)dgrReaCanFols.SelectedItem;
      frmReasonCancellationFolioDetail frmReaCanFolDetail = new frmReasonCancellationFolioDetail();
      frmReaCanFolDetail.Owner = this;
      frmReaCanFolDetail.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      frmReaCanFolDetail.oldReasonCanFol = reasonCancellationFolio;
      if(frmReaCanFolDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<ReasonCancellationFolio> lstReaCanFols = (List<ReasonCancellationFolio>)dgrReaCanFols.ItemsSource;
        if(ValidateFilter(frmReaCanFolDetail.reasonCancellationFolio))//Verificamos si cumple con los filtros actuales
        {
          ObjectHelper.CopyProperties(reasonCancellationFolio, frmReaCanFolDetail.reasonCancellationFolio);//Actualizamos los datos del registro
          lstReaCanFols.Sort((x, y) => string.Compare(x.rcfN, y.rcfN));//ordenamos la lista
          nIndex = lstReaCanFols.IndexOf(reasonCancellationFolio);//obtenemos la posicion del registro
        }
        else
        {
          lstReaCanFols.Remove(reasonCancellationFolio);
        }
        dgrReaCanFols.Items.Refresh();//Actualizamos los items del grid
        GridHelper.SelectRow(dgrReaCanFols, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstReaCanFols.Count + " Reason For Cancellation Of Folios.";//Actualizamos el contador
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
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _reasonCancelFolFilter.rcfID;
      frmSearch.strDesc = _reasonCancelFolFilter.rcfN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _reasonCancelFolFilter.rcfID = frmSearch.strID;
        _reasonCancelFolFilter.rcfN = frmSearch.strDesc;
        LoadReasonCancellationFolios();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmReasonCancellationFolioDetail frmResCanFolDetail = new frmReasonCancellationFolioDetail();
      frmResCanFolDetail.Owner = this;
      frmResCanFolDetail.enumMode = EnumMode.Add;
      if(frmResCanFolDetail.ShowDialog()==true)
      {
        ReasonCancellationFolio reasonCacellationFolio = frmResCanFolDetail.reasonCancellationFolio;
        if(ValidateFilter(reasonCacellationFolio))//Verificamos si cumple con los filtros
        {
          List<ReasonCancellationFolio> lstReaCanFols = (List<ReasonCancellationFolio>)dgrReaCanFols.ItemsSource;
          lstReaCanFols.Add(reasonCacellationFolio);//Agregamos el registro a la lista
          lstReaCanFols.Sort((x, y) => string.Compare(x.rcfN, y.rcfN));//Ordenamos la lista
          int nIndex = lstReaCanFols.IndexOf(reasonCacellationFolio);
          dgrReaCanFols.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrReaCanFols, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstReaCanFols.Count + " Reason For Cancellation Of Folios.";//Actualizamos el contador
        }
      }
    } 
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      ReasonCancellationFolio reasonCancellationFolio = (ReasonCancellationFolio)dgrReaCanFols.SelectedItem;
      LoadReasonCancellationFolios(reasonCancellationFolio);
    } 
    #endregion
    #endregion

    #region Method Form
    #region LoadReasonCancellationFolios
    /// <summary>
    /// Llena el grid de Folio Cancelation Folios
    /// </summary>
    /// <param name="reasonCancellationFollio">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private async void LoadReasonCancellationFolios(ReasonCancellationFolio reasonCancellationFollio = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<ReasonCancellationFolio> lstReaCanFols =await BRReasonCancellationFolios.GetReasonCancellationFolios(_nStatus, _reasonCancelFolFilter);
        dgrReaCanFols.ItemsSource = lstReaCanFols;
        if (lstReaCanFols.Count > 0 && reasonCancellationFollio != null)
        {
          reasonCancellationFollio = lstReaCanFols.Where(rcf => rcf.rcfID == reasonCancellationFollio.rcfID).FirstOrDefault();
          nIndex = lstReaCanFols.IndexOf(reasonCancellationFollio);
        }
        GridHelper.SelectRow(dgrReaCanFols, nIndex);
        StatusBarReg.Content = lstReaCanFols.Count + " Reason For Cancellation Of Folios.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Reason For Cancellation Folios");
      }
    }
    #endregion

    #region  ValidateFilter
    /// <summary>
    /// Valida que un objeto tipo reasonCancellationFolio
    /// cumpla con los filtros actuales
    /// </summary>
    /// <param name="reasonCancellationFolio">objeot a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private bool ValidateFilter(ReasonCancellationFolio reasonCancellationFolio)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(reasonCancellationFolio.rcfA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_reasonCancelFolFilter.rcfID))//Filtro por ID
      {
        if(reasonCancellationFolio.rcfID!=_reasonCancelFolFilter.rcfID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_reasonCancelFolFilter.rcfN))//Filtro por descripción
      {
        if(!reasonCancellationFolio.rcfN.Contains(_reasonCancelFolFilter.rcfN,StringComparison.OrdinalIgnoreCase))
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
