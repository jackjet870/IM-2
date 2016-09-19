using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Extensions;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSeasons.xaml
  /// Catalogo de temporadas
  /// </summary>
  /// <history>
  ///   [vku] 26/Jul/2016 Created
  /// </history>
  public partial class frmSeasons : Window
  {
    #region Variables
    private Season _seasonFilter = new Season();//objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//boleano para saber si se tiene permiso para editar
    #endregion

    public frmSeasons()
    {
      InitializeComponent();
    }

    #region Metodos

    #region LoadSeasons
    /// <summary>
    ///   Carga las temporadas
    /// </summary>
    /// <param name="season">Objeto a seleccionar</param>
    /// <history>
    ///   [vku] 26/Jul/2016 Created
    /// </history>
    protected async void LoadSeasons(Season season = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Season> lstSeasons = await BRSeasons.GetSeasons(_nStatus, _seasonFilter);
        dgrSeasons.ItemsSource = lstSeasons;
        if (lstSeasons.Count > 0 && season != null)
        {
          season = lstSeasons.Where(ss => ss.ssID == season.ssID).FirstOrDefault();
          nIndex = lstSeasons.IndexOf(season);
        }
        GridHelper.SelectRow(dgrSeasons, nIndex);
        StatusBarReg.Content = lstSeasons.Count + " Seasons.";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// valida que un objeto dept cumpla con los filtros actuales
    /// </summary>
    /// <param name="season">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    ///   [vku] 05/Ago/2016 Created 
    /// </history>
    private bool ValidateFilter(Season season)
    {
      if (_nStatus != -1)//Filtro por Status
      {
        if (season.ssA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_seasonFilter.ssID))//Filtro por ID
      {
        if (season.ssID != _seasonFilter.ssID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_seasonFilter.ssN))//Filtro por descripción
      {
        if (!season.ssN.Contains(_seasonFilter.ssN, StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }

      return true;
    }
    #endregion

    #endregion

    #region Eventos

    #region Window_Loaded
    /// <summary>
    ///   Carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 26/Jul/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(Model.Enums.EnumPermission.Sales, Model.Enums.EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadSeasons();
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    ///   Verifica las teclas presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 26/Jul/2016 Created
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

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    ///   Verifica teclas activas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 26/Jul/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Cell_DoubleClick
    /// <summary>
    ///   Abre la ventalla de detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 26/Jul/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Season season = (Season)dgrSeasons.SelectedItem;
      frmSeasonDetail frmSeasonDetail = new frmSeasonDetail();
      frmSeasonDetail.Owner = this;
      frmSeasonDetail.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      frmSeasonDetail.oldSeason = season;
      if (frmSeasonDetail.ShowDialog() == true)
      {
        List<Season> lstSeason = (List<Season>)dgrSeasons.ItemsSource;
        int nIndex = 0;
        if (!ValidateFilter(frmSeasonDetail.season))
        {
          lstSeason.Remove(season);//Quitamos el registro de la lista          
        }
        else
        {
          ObjectHelper.CopyProperties(season, frmSeasonDetail.season);
          lstSeason.Sort((x, Y) => string.Compare(x.ssN, Y.ssN));//Ordenamos la lista 
          nIndex = lstSeason.IndexOf(season);
        }
        dgrSeasons.Items.Refresh();//Actualizamos la vista del grid              
        GridHelper.SelectRow(dgrSeasons, nIndex);
        StatusBarReg.Content = lstSeason.Count + " Seasons.";//Actualizamos el contador   
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
    ///   [vku] 26/Jul/2016 Created
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

    #region btnRef_Click
    /// <summary>
    ///   Refresca los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 26/Jul/2016 Created
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Season season = (Season)dgrSeasons.SelectedItem;
      LoadSeasons(season);
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    ///   Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 26/Jul/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmSeasonDetail frmSeasonDetail = new frmSeasonDetail();
      frmSeasonDetail.Owner = this;
      frmSeasonDetail.enumMode = EnumMode.Add;
      if (frmSeasonDetail.ShowDialog() == true)
      {
        if (ValidateFilter(frmSeasonDetail.season))//Valida que cumpla con los filtros actuales
        {
          List<Season> lstSeason = (List<Season>)dgrSeasons.ItemsSource;
          lstSeason.Add(frmSeasonDetail.season);//Agrega el registro
          lstSeason.Sort((x, y) => string.Compare(x.ssN, y.ssN));//ordena la lista
          int nIndex = lstSeason.IndexOf(frmSeasonDetail.season);//BUsca la posición del registro
          dgrSeasons.Items.Refresh();//Refresca la vista
          GridHelper.SelectRow(dgrSeasons, nIndex);//Selecciona el registro
          StatusBarReg.Content = lstSeason.Count + " Seasons.";//Actualiza el contador
        }
      }
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    ///   Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 26/Jul/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.strID = _seasonFilter.ssID;
      frmSearch.strDesc = _seasonFilter.ssN;
      frmSearch.nStatus = _nStatus;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _seasonFilter.ssID = frmSearch.strID;
        _seasonFilter.ssN = frmSearch.strDesc;
        LoadSeasons();
      }
    }
    #endregion

    #endregion
  }
}
