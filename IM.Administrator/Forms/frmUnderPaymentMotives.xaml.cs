using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmUnderPaymentMotives.xaml
  /// </summary>
  public partial class frmUnderPaymentMotives : Window
  {
    #region Variables
    private UnderPaymentMotive _underPaymentMotiveFilter = new UnderPaymentMotive();//Objeto con filtros del grid
    private int _nStatus = -1;//Estatus del registro del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar
    #endregion
    public frmUnderPaymentMotives()
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
      _blnEdit = App.User.HasPermission(EnumPermission.Motives, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadUnderPaymentMotives();
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
      UnderPaymentMotive underPaymentMotive = (UnderPaymentMotive)dgrUnderPayMentMotive.SelectedItem;
      frmUnderPaymentMotiveDetail frmUndPayMotDetail = new frmUnderPaymentMotiveDetail();
      frmUndPayMotDetail.Owner = this;
      frmUndPayMotDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.preview;
      frmUndPayMotDetail.oldUnderPaymentMOtive = underPaymentMotive;
      if (frmUndPayMotDetail.ShowDialog() == true)
      {
        int nIndex = 0;
        List<UnderPaymentMotive> lstUnderPaymentMotive = (List<UnderPaymentMotive>)dgrUnderPayMentMotive.ItemsSource;
        if (ValidateFilter(frmUndPayMotDetail.underPaymentMotive))//Verificamos que cumpla con los filtros
        {
          ObjectHelper.CopyProperties(underPaymentMotive, frmUndPayMotDetail.underPaymentMotive);//Actualizamos los datos
          lstUnderPaymentMotive.Sort((x, y) => string.Compare(x.upN, y.upN));//Reordenamos la lista
          nIndex = lstUnderPaymentMotive.IndexOf(underPaymentMotive);//Buscamos la posición del registro
        }
        else
        {
          lstUnderPaymentMotive.Remove(underPaymentMotive);//Quitamos el registro de la lista
        }
        dgrUnderPayMentMotive.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrUnderPayMentMotive, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstUnderPaymentMotive.Count + " Under Payment Motives.";//Actualizamos el contador
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
      frmSearch.strID = (_underPaymentMotiveFilter.upID > 0) ? _underPaymentMotiveFilter.upID.ToString() : "";
      frmSearch.strDesc = _underPaymentMotiveFilter.upN;
      frmSearch.nStatus = _nStatus;
      frmSearch.enumWindow = EnumWindow.DefaultInt;
      if(frmSearch.ShowDialog()==true)
      {
        _underPaymentMotiveFilter.upID = Convert.ToInt32(frmSearch.strID);
        _underPaymentMotiveFilter.upN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadUnderPaymentMotives();
      }
    }

    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmUnderPaymentMotiveDetail frmUndPayMotDetail = new frmUnderPaymentMotiveDetail();
      frmUndPayMotDetail.Owner = this;
      frmUndPayMotDetail.enumMode = EnumMode.add;
      if(frmUndPayMotDetail.ShowDialog()==true)
      {
        UnderPaymentMotive underPaymentMotive = frmUndPayMotDetail.underPaymentMotive;
        if(ValidateFilter(underPaymentMotive))//verificamos que cumpla con los filtros actuales
        {
          List<UnderPaymentMotive> lstUnderPaymentMotives = (List<UnderPaymentMotive>)dgrUnderPayMentMotive.ItemsSource;
          lstUnderPaymentMotives.Add(underPaymentMotive);//Agregamos el registro
          lstUnderPaymentMotives.Sort((x, y) => string.Compare(x.upN, y.upN));//Reordenamos la lista
          int nIndex = lstUnderPaymentMotives.IndexOf(underPaymentMotive);//Obtenemos la posición del registro
          dgrUnderPayMentMotive.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrUnderPayMentMotive, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstUnderPaymentMotives.Count + " Under Payment Motives.";//Actualizamos el contador
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
      UnderPaymentMotive underPaymentMotive = (UnderPaymentMotive)dgrUnderPayMentMotive.SelectedItem;
      LoadUnderPaymentMotives(underPaymentMotive);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadUnderPaymentMotives
    /// <summary>
    /// Llena el grid de paymentMotives
    /// </summary>
    /// <param name="underPaymentMotive">objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 28/06/2016
    /// </history>
    private async void LoadUnderPaymentMotives(UnderPaymentMotive underPaymentMotive = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<UnderPaymentMotive> lstUnderPaymentMotive = await BRUnderPaymentMotives.getUnderPaymentMotives(_nStatus, _underPaymentMotiveFilter);
        dgrUnderPayMentMotive.ItemsSource = lstUnderPaymentMotive;
        if (lstUnderPaymentMotive.Count > 0 && underPaymentMotive != null)
        {
          underPaymentMotive = lstUnderPaymentMotive.Where(up => up.upID == underPaymentMotive.upID).FirstOrDefault();
          nIndex = lstUnderPaymentMotive.IndexOf(underPaymentMotive);
        }
        GridHelper.SelectRow(dgrUnderPayMentMotive, nIndex);
        StatusBarReg.Content = lstUnderPaymentMotive.Count + " Under Payment Motives.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Under Payment Motives");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un objeto underPaymentMotive cumpla con los filtros actuales
    /// </summary>
    /// <param name="underPaymentMotive">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private bool ValidateFilter(UnderPaymentMotive underPaymentMotive)
    {
      if (_nStatus != -1)//Filtro por estatus
      {
        if (underPaymentMotive.upA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (_underPaymentMotiveFilter.upID > 0)//Filtro por ID
      {
        if (underPaymentMotive.upID != _underPaymentMotiveFilter.upID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_underPaymentMotiveFilter.upN))
      {
        if (!underPaymentMotive.upN.Contains(_underPaymentMotiveFilter.upN,StringComparison.OrdinalIgnoreCase))
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
