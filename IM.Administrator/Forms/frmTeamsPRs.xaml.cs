using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmTeamsPRs.xaml
  /// Catalogo de Equipos de PR's
  /// </summary>
  /// <history>
  ///   [vku] 09/Jul/2016 Created
  /// </history>
  public partial class frmTeamsPRs : Window
  {
    #region Atributos
    private TeamGuestServices _teamFilter = new TeamGuestServices();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion

    public frmTeamsPRs()
    {
      InitializeComponent();
    }

    #region Metodos

    #region LoadTeamsPRs
    /// <summary>
    ///   Carga los equipos de PRs
    /// </summary>
    /// <history>
    ///   [vku] 09/Jul/2016 Created
    /// </history>
    public async void LoadTeamsPRs(TeamGuestServices team = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<TeamGuestServices> lstTeamsPRs = await BRTeamsGuestServices.GetTeamsGuestServices(_nStatus, _teamFilter);
        dgrTeams.ItemsSource = lstTeamsPRs;

        if (team!= null && lstTeamsPRs.Count > 0)
        {
          team = lstTeamsPRs.FirstOrDefault(tg => tg.tgID == team.tgID);
          nIndex = lstTeamsPRs.IndexOf(team);
        }
        GridHelper.SelectRow(dgrTeams, nIndex);

        StatusBarReg.Content = lstTeamsPRs.Count + " Teams.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Teams PRs");
      }
    }
    #endregion

    #region LoadLocations
    /// <summary>
    ///   Carga las locaciones
    /// </summary>
    /// <history>
    ///   [vku] 13/Jul/2016 Created
    /// </history>
    public async void LoadLocations()
    {
      try
      {
        List<Location> lstLocations = await BRLocations.GetLocations(nStatus: -1);
        cbotglo.ItemsSource = lstLocations;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Teams PRs");
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
    ///   [vku] 14/Jul/2016 Created
    /// </history>
    private bool ValidateFilter(TeamGuestServices team)
    {
      if (_nStatus != -1)//Filtro por Status
      {
        if (team.tgA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_teamFilter.tgID))//Filtro por ID
      {
        if (team.tgID != _teamFilter.tgID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_teamFilter.tgN))//Filtro por descripción
      {
        if (!team.tgN.Contains(_teamFilter.tgN,StringComparison.OrdinalIgnoreCase))
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
      ///   [vku] 09/Jul/2016 Created
      /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadLocations();
      LoadTeamsPRs();
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    ///   Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 09/Jul/2016 Created
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
    /// abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 09/Jul/2016 Created
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
    ///   [vku] 09/Jul/2016 Created
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
    ///   [vku] 09/Jul/2016 Created
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      TeamGuestServices team = (TeamGuestServices)dgrTeams.SelectedItem;
      LoadTeamsPRs(team);
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    ///   Abre la ventana detalle en modo agregar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 09/Jul/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmTeamPRsDetail frmTeamPRsDetail = new frmTeamPRsDetail();
      frmTeamPRsDetail.Owner = this;
      frmTeamPRsDetail.enumMode = EnumMode.Add;
      if (frmTeamPRsDetail.ShowDialog() == true)
      {
        if (ValidateFilter(frmTeamPRsDetail.team))//Valida que cumpla con los filtros actuales
        {
          List<TeamGuestServices> lstTeams = (List<TeamGuestServices>)dgrTeams.ItemsSource;
          lstTeams.Add(frmTeamPRsDetail.team);//Agrega el registro
          lstTeams.Sort((x, y) => string.Compare(x.tgN, y.tgN));//ordena la lista
          int nIndex = lstTeams.IndexOf(frmTeamPRsDetail.team);//BUsca la posición del registro
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
    ///   [vku] 09/Jul/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.strID = _teamFilter.tgID;
      frmSearch.strDesc = _teamFilter.tgN;
      frmSearch.nStatus = _nStatus;
      frmSearch.sLocation = _teamFilter.tglo;
      frmSearch.enumWindow = EnumWindow.TeamsPRs;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _teamFilter.tgID = frmSearch.strID;
        _teamFilter.tgN = frmSearch.strDesc;
        _teamFilter.tglo = frmSearch.sLocation;
        LoadTeamsPRs();
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
    /// [vku] 09/Jul/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      TeamGuestServices teamPRs = (TeamGuestServices)dgrTeams.SelectedItem;
      frmTeamPRsDetail frmTeamPRsDetail = new frmTeamPRsDetail();
      frmTeamPRsDetail.Owner = this;
      frmTeamPRsDetail.oldTeam = teamPRs;
      frmTeamPRsDetail.enumMode = EnumMode.Edit;
     if (frmTeamPRsDetail.ShowDialog() == true)
      {
        int nIndex = 0;
        List<TeamGuestServices> lstTeams = (List<TeamGuestServices>)dgrTeams.ItemsSource;
        if (ValidateFilter(frmTeamPRsDetail.team))//Valida si cumple con los filtros
        {
          ObjectHelper.CopyProperties(teamPRs, frmTeamPRsDetail.team);//Actualiza los datos
          lstTeams.Sort((x, y) => string.Compare(x.tgN, y.tgN));//Ordena la lista
          nIndex = lstTeams.IndexOf(teamPRs);//busca la posición del registro
        }
        else
        {
          lstTeams.Remove(teamPRs);//Quita el registro
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
