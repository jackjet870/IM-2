using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Extensions;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmTeamsSalesmen.xaml
  /// Catalogo de equipos de vendedores
  /// </summary>
  /// <history>
  ///   [vku] 22/Jul/2016 Created
  /// </history>
  public partial class frmTeamsSalesmen : Window
  {
    #region Atributos
    private TeamSalesmen _teamFilter = new TeamSalesmen();
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion

    public frmTeamsSalesmen()
    {
      InitializeComponent();
    }

    #region Metodos

    #region LoadTeamsSalesmen
    /// <summary>
    ///   Carga los equipos de vendedores
    /// </summary>
    /// <param name="team"></param>}
    /// <history>
    ///   [vku] 22/Jul/2016 Created
    /// </history>
    public async void LoadTeamsSalesmen(TeamSalesmen team = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<TeamSalesmen> lstTeamsSalesmen = await BRTeamsSalesMen.GetTeamsSalesMen(_nStatus, _teamFilter);
        dgrTeams.ItemsSource = lstTeamsSalesmen;

        if (team != null && lstTeamsSalesmen.Count > 0)
        {
          team = lstTeamsSalesmen.FirstOrDefault(ts => ts.tsID == team.tsID);
          nIndex = lstTeamsSalesmen.IndexOf(team);
        }
        GridHelper.SelectRow(dgrTeams, nIndex);

        StatusBarReg.Content = lstTeamsSalesmen.Count + " Teams.";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Teams Salesmen");
      }
    }
    #endregion

    #region LoadSalesRoom
    /// <summary>
    ///   Carga las salas de venta
    /// </summary>
    /// <history>
    ///   [vku] 22/Jul/2016 Created
    /// </history>
    private async void LoadSalesRoom()
    {
      try
      {
        List<SalesRoomShort> lstSalesRoom = await BRSalesRooms.GetSalesRooms(status: 0);
        cbotssr.ItemsSource = lstSalesRoom;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Teams Salesmen");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    ///  valida que un objeto team cumpla con los filtros actuales
    /// </summary>
    /// <param name="team">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    private bool ValidateFilter(TeamSalesmen team)
    {
      if (_nStatus != -1)//Filtro por Status
      {
        if (team.tsA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_teamFilter.tsID))//Filtro por ID
      {
        if (team.tsID != _teamFilter.tsID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_teamFilter.tsN))//Filtro por descripción
      {
        if (!team.tsN.Contains(_teamFilter.tsN, StringComparison.OrdinalIgnoreCase))
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
    ///   [vku] 22/Jul/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadSalesRoom();
      LoadTeamsSalesmen();
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    ///   Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 22/Jul/2016 Created
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

    #region Row_KeyDown
    /// <summary>
    ///   Abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 22/Jul/2016 Created
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

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    ///   Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 22/Jul/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region btnRef_Click
    /// <summary>
    ///   Recarga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 22/Jul/2016 Created
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      TeamSalesmen team = (TeamSalesmen)dgrTeams.SelectedItem;
      LoadTeamsSalesmen(team);
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    ///   Abre la ventana detalle en modo agregar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 22/Jul/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmTeamSalesmenDetail frmTeamSalesmenDetail = new frmTeamSalesmenDetail();
      frmTeamSalesmenDetail.Owner = this;
      frmTeamSalesmenDetail.enumMode = EnumMode.add;
      if (frmTeamSalesmenDetail.ShowDialog() == true)
      {
        if (ValidateFilter(frmTeamSalesmenDetail.team))//Valida que cumpla con los filtros actuales
        {
          List<TeamSalesmen> lstTeams = (List<TeamSalesmen>)dgrTeams.ItemsSource;
          lstTeams.Add(frmTeamSalesmenDetail.team);//Agrega el registro
          lstTeams.Sort((x, y) => string.Compare(x.tsN, y.tsN));//ordena la lista
          int nIndex = lstTeams.IndexOf(frmTeamSalesmenDetail.team);//BUsca la posición del registro
          dgrTeams.Items.Refresh();//Refresca la vista
          GridHelper.SelectRow(dgrTeams, nIndex);//Selecciona el registro
          StatusBarReg.Content = lstTeams.Count + " Teams.";//Actualiza el contador
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
    ///   [vku] 22/Jul/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.strID = _teamFilter.tsID;
      frmSearch.strDesc = _teamFilter.tsN;
      frmSearch.nStatus = _nStatus;
      frmSearch.sSalesRoom = _teamFilter.tssr;
      frmSearch.enumWindow = EnumWindow.TeamsSalesmen;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _teamFilter.tsID = frmSearch.strID;
        _teamFilter.tsN = frmSearch.strDesc;
        _teamFilter.tssr = frmSearch.sSalesRoom;
        LoadTeamsSalesmen();
      }
    }
    #endregion

    #region Cell_DoubleClick
    /// <summary>
    ///   Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vku] 22/Jul/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      TeamSalesmen teamSalesmen = (TeamSalesmen)dgrTeams.SelectedItem;
      frmTeamSalesmenDetail frmTeamSalesmenDetail = new frmTeamSalesmenDetail();
      frmTeamSalesmenDetail.Owner = this;
      frmTeamSalesmenDetail.oldTeam = teamSalesmen;
      frmTeamSalesmenDetail.enumMode = EnumMode.edit;
      if (frmTeamSalesmenDetail.ShowDialog() == true)
      {
        int nIndex = 0;
        List<TeamSalesmen> lstTeams = (List<TeamSalesmen>)dgrTeams.ItemsSource;
        if (ValidateFilter(frmTeamSalesmenDetail.team))//Valida si cumple con los filtros
        {
          ObjectHelper.CopyProperties(teamSalesmen, frmTeamSalesmenDetail.team);//Actualiza los datos
          lstTeams.Sort((x, y) => string.Compare(x.tsN, y.tsN));//Ordena la lista
          nIndex = lstTeams.IndexOf(teamSalesmen);//busca la posición del registro
        }
        else
        {
          lstTeams.Remove(teamSalesmen);//Quita el registro
        }
        dgrTeams.Items.Refresh();//Actualiza la vista
        GridHelper.SelectRow(dgrTeams, nIndex);//Selecciona el registro
        StatusBarReg.Content = lstTeams.Count + " Teams.";//Actualiza el contador
      }
    }
    #endregion

    #endregion
  }
}
