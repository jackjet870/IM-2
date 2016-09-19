using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmTeamsSalesmenDetail.xaml
  /// </summary>
  public partial class frmTeamSalesmenDetail : Window
  {
    #region Atributos
    public TeamSalesmen oldTeam = new TeamSalesmen();//Objeto con los valores iniciales
    public TeamSalesmen team = new TeamSalesmen();//Objeto para llenar el formulario
    private List<Personnel> _lstOldPersonnel = new List<Personnel>();//Lista inicial de personnel
    private List<Personnel> _lstPersonnel = new List<Personnel>();//Lista de Personnel para el viewSource 
    public EnumMode enumMode;
    private bool _blnIsCellCancel = false;
    private bool blnClosing = false;
    #endregion

    public frmTeamSalesmenDetail()
    {
      InitializeComponent();
    }

    #region Metodos

    #region LoadPersonnelForLeader
    /// <summary>
    ///  Carga los lideres de equipos
    /// </summary>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    public async void LoadPersonnelForLeader()
    {
      try
      {
        List<PersonnelShort> lstPersonnelForLeader = await BRPersonnel.GetPersonnel(status: 1);
        cbotsLeader.ItemsSource = lstPersonnelForLeader;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadSalesRoom
    /// <summary>
    ///  Carga las salas de venta
    /// </summary>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    public async void LoadSalesRoom()
    {
      try
      {
        List<SalesRoom> lstSalesRoom = await BRSalesRooms.GetSalesRooms(nStatus: -1);
        cbotssr.ItemsSource = lstSalesRoom;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadPost
    /// <summary>
    ///   Carga los puestos
    /// </summary>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    public async void LoadPost()
    {
      try
      {
        List<Post> lstPost = await BRPosts.GetPosts(nStatus: -1);
        cboPost.ItemsSource = lstPost;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadIntegrants
    /// <summary>
    ///   Carga los integrantes
    /// </summary>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    public async void LoadIntegrantes(TeamSalesmen team)
    {
      try
      {
        List<Personnel> lstPersonnel = await BRPersonnel.GetPersonnels(blnLiner : true);
        cboIntegrant.ItemsSource = lstPersonnel;
        _lstPersonnel = lstPersonnel.Where(pe => pe.peTeam == team.tsID && pe.peTeamType == EnumToListHelper.GetEnumDescription(EnumTeamType.TeamSalesmen) && pe.pePlaceID == team.tssr).OrderBy(pe => pe.pepo).ThenBy(pe => pe.peN).ToList();
        dgrIntegrants.ItemsSource = _lstPersonnel;
        List<Personnel> lstPersonnelIni = await BRPersonnel.GetPersonnels();
        _lstOldPersonnel = lstPersonnelIni.Where(pe => pe.peTeam == team.tsID && pe.peTeamType == EnumToListHelper.GetEnumDescription(EnumTeamType.TeamSalesmen) && pe.pePlaceID == team.tssr).OrderBy(pe => pe.pepo).ThenBy(pe => pe.peN).ToList(); //Cargamos la lista con los datos iniciales
        StatusBarReg.Content = _lstPersonnel.Count + " Integrants.";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
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
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(team, oldTeam);
      LoadPersonnelForLeader();
      LoadPost();
      LoadSalesRoom();
      LoadIntegrantes(team);
      #region Bloquear botones
      if (enumMode != EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        btnTransfer.Visibility = Visibility.Visible;
        txtsID.IsEnabled = (enumMode == EnumMode.Add);
        txtDescrip.IsEnabled = true;
        cbotsLeader.IsEnabled = true;
        cbotssr.IsEnabled = true;
        chkActive.IsEnabled = true;
        UIHelper.SetUpControls(team, this);
      }
      if (enumMode != EnumMode.Add)
      {
        Title += " (" + team.tsID + "," + team.tsN + ")";
      }
      #endregion
      DataContext = team;
    }
    #endregion

    #region Window_Keydown
    /// <summary>
    ///   Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
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
        case Key.Escape:
          btnCancel_Click(null, null);
          break;
      }
    }
    #endregion

    #region Window_IsKeyboardFocusesChanged
    /// <summary>
    ///   Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Window_Closing
    /// <summary>
    ///   Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
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

    #region dgrIntegrants_CellEditEnding
    /// <summary>
    ///    Valida que no se repita un integrante
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    private void dgrIntegrants_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        _blnIsCellCancel = false;
        if (e.Column.Header.ToString() == "Integrant")
        {
          bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrIntegrants, true);
          e.Cancel = isRepeat;
        }
      }
      else
      {
        _blnIsCellCancel = true;
      }
    }
    #endregion

    #region dgrIntegrants_RowEditEnding
    /// <summary>
    ///    Actualiza la fila seleccionada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    private void dgrIntegrants_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      dgrIntegrants.RowEditEnding -= dgrIntegrants_RowEditEnding;
      if (_blnIsCellCancel)
      {
        dgrIntegrants.CancelEdit();
      }
      else
      {
        dgrIntegrants.CommitEdit();
        dgrIntegrants.Items.Refresh();
        GridHelper.SelectRow(dgrIntegrants, dgrIntegrants.SelectedIndex);
      }
      dgrIntegrants.RowEditEnding += dgrIntegrants_RowEditEnding;
    }
    #endregion

    #region btnAccept_Click
    /// <summary>
    ///   Agrega o actualiza los registros del catalogo team
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<Personnel> lstPersonnels = (List<Personnel>)dgrIntegrants.ItemsSource;
      if (ObjectHelper.IsEquals(team, oldTeam) && enumMode != EnumMode.Add && ObjectHelper.IsListEquals(lstPersonnels, _lstOldPersonnel))
      {
        blnClosing = true;
        Close();
      }
      else
      {
        status.Visibility = Visibility.Visible;
        txtStatus.Text = "Saving Data...";
        string sMsj = ValidateHelper.ValidateForm(this, "Team");
        if (sMsj == "")
        {
          List<Personnel> lstAdd = lstPersonnels.Where(pe => !_lstOldPersonnel.Any(pee => pee.peID == pe.peID)).ToList();
          List<Personnel> lstDel = _lstOldPersonnel.Where(pe => !lstPersonnels.Any(pee => pee.peID == pe.peID)).ToList();
          List<Personnel> lstChanged = lstPersonnels.Where(pe => !_lstOldPersonnel.Any(pee => pee.peID == pe.peID && pee.pepo == pe.pepo)).ToList();
          int nRes = await BRTeamsSalesMen.SaveTeam(Context.User.User.peID, team, (enumMode == EnumMode.Edit), lstAdd, lstDel, lstChanged);
          status.Visibility = Visibility.Collapsed;
          UIHelper.ShowMessageResult("Team", nRes);
          if (nRes > 0)
          {
            blnClosing = true;
            DialogResult = true;
            Close();
          }
        }
        else
        {
          UIHelper.ShowMessage(sMsj);
        }
      }
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    ///   Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      List<Personnel> lstPersonnels = (List<Personnel>)dgrIntegrants.ItemsSource;
      if (!ObjectHelper.IsEquals(team, oldTeam) && !ObjectHelper.IsListEquals(lstPersonnels, _lstOldPersonnel))
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

    #region btnTransfer_Click
    /// <summary>
    ///   Abre la ventana para transferir integrantes a otro equipo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 23/Jul/2016 Created
    /// </history>
    private void btnTransfer_Click(object sender, RoutedEventArgs e)
    {
      List<Personnel> lstPersonnelTransfer = (List<Personnel>)dgrIntegrants.ItemsSource;
      frmTeamsTransfer frmTeamsTransfer = new frmTeamsTransfer();
      frmTeamsTransfer.Owner = this;
      frmTeamsTransfer.oldTeamSalesmen = oldTeam;
      frmTeamsTransfer._lstOldPersonnel = lstPersonnelTransfer;
      frmTeamsTransfer._enumTeamType = EnumTeamType.TeamSalesmen;
      if (frmTeamsTransfer.ShowDialog() == true)
      {
        LoadIntegrantes(oldTeam);
      }
    }
    #endregion

    #endregion
  }
}
