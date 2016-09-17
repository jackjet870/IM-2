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
  /// Interaction logic for frmTeamPRsDetail.xaml
  /// </summary>
  public partial class frmTeamPRsDetail : Window
  {
    #region Atributos
    public TeamGuestServices oldTeam = new TeamGuestServices();//Objeto con los valores iniciales
    public TeamGuestServices team = new TeamGuestServices();//Objeto para llenar el formulario
    private List<Personnel> _lstOldPersonnel = new List<Personnel>();//Lista inicial de personnel
    private List<Personnel> _lstPersonnel = new List<Personnel>();//Lista de Personnel para el viewSource 
    public EnumMode enumMode;
    private bool _blnIsCellCancel = false;
    private bool blnClosing = false;
    #endregion

    public frmTeamPRsDetail()
    {
      InitializeComponent();
    }

    #region Metodos

    #region LoadPersonnelForLeader
    /// <summary>
    ///  Carga los lideres de equipos
    /// </summary>
    /// <history>
    ///   [vku] 11/Jul/2016 Created
    /// </history>
    public async void LoadPersonnelForLeader()
    {
      try
      { 
        List<PersonnelShort> lstPersonnelForLeader = await BRPersonnel.GetPersonnel(status: 1);
        cbotgLeader.ItemsSource = lstPersonnelForLeader;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadLocations
    /// <summary>
    ///   Carga las locaciones
    /// </summary>
    /// <history>
    ///   [vku] 12/Jul/2016 Created
    /// </history>
    public async void LoadLocations()
    {
      try
      {  
        List<Location> lstLocations = await BRLocations.GetLocations(nStatus: 1);
        cbotglo.ItemsSource = lstLocations;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadPost
    /// <summary>
    ///   Carga los post
    /// </summary>
    /// <history>
    ///   [vku] 13/Jul/2016 Created
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
    ///   [vku] 12/Jul/2016 Created
    /// </history>
    public async void LoadIntegrantes(TeamGuestServices team)
    {
      try
      {
        List<Personnel> lstPersonnel = await BRPersonnel.GetPersonnels(blnLiner: true);
        List<Personnel> lstPersonnelIni = await BRPersonnel.GetPersonnels(blnLiner: true);
        List<Personnel> lstliner = new List<Personnel>();
        lstliner.AddRange(lstPersonnel);
        cboIntegrant.ItemsSource = lstPersonnel;
        cboLiner.ItemsSource = lstliner;
        _lstPersonnel = lstPersonnel.Where(pe => pe.peTeam == team.tgID && pe.peTeamType == EnumToListHelper.GetEnumDescription(EnumTeamType.TeamPRs) && pe.pePlaceID == team.tglo).ToList();
        _lstOldPersonnel = lstPersonnelIni.Where(pe => pe.peTeam == team.tgID && pe.peTeamType == EnumToListHelper.GetEnumDescription(EnumTeamType.TeamPRs) && pe.pePlaceID == team.tglo).ToList(); //Cargamos la lista con los datos iniciales

        dgrIntegrants.ItemsSource = _lstPersonnel;

        StatusBarReg.Content = _lstPersonnel.Count + " Integrants.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #endregion

    #region Enventos

    #region Window_Closing
    /// <summary>
    ///   Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 14/Jul/2016 Created
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

    #region Window_KeyDown
    /// <summary>
    ///   Valida las teclas INS|MAYSU|LOCKNUM & Escape
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
        case Key.Escape:
          btnCancel_Click(null, null);
          break;
      }
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    ///  Carga todos los datos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 11/Jul/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(team, oldTeam);
      LoadPersonnelForLeader();
      LoadPost();
      LoadLocations();
      LoadIntegrantes(team);
      #region Bloquear botones
      if (enumMode != EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        btnTransfer.Visibility = Visibility.Visible;
        txtgID.IsEnabled = (enumMode == EnumMode.Add);
        txtDescrip.IsEnabled = true;
        cbotgLeader.IsEnabled = true;
        cbotglo.IsEnabled = true;
        chkActive.IsEnabled = true;
        UIHelper.SetUpControls(team, this);
      }
      if (enumMode != EnumMode.Add)
      {
        Title += " (" + team.tgID + "," + team.tgN + ")";
      }
      #endregion
      DataContext = team;
    }
    #endregion

    #region btnAccept_Click
    /// <summary>
    ///   Agrega o actualiza los registros del catalogo team
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 13/Jul/2016 Created
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
          btnAccept.Visibility = Visibility.Collapsed;
          List<Personnel> lstAdd = lstPersonnels.Where(pe => !_lstOldPersonnel.Any(pee => pee.peID == pe.peID)).ToList();
          List<Personnel> lstDel = _lstOldPersonnel.Where(pe => !lstPersonnels.Any(pee => pee.peID == pe.peID)).ToList();
          List<Personnel> lstChanged = lstPersonnels.Where(pe => !_lstOldPersonnel.Any(pee => pee.peLinerID == pe.peLinerID)).ToList();
          int nRes = await BRTeamsGuestServices.SaveTeam(Context.User.User.peID, team, (enumMode == EnumMode.Edit), lstAdd, lstDel, lstChanged);
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
        btnAccept.Visibility = Visibility.Visible;
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
    ///   [vku] 14/Jul/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      List<Personnel> lstPersonnels = (List<Personnel>)dgrIntegrants.ItemsSource;
      if (!ObjectHelper.IsEquals(team, oldTeam) || !ObjectHelper.IsListEquals(lstPersonnels, _lstOldPersonnel))
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

    #region dgrIntegrants_CellEditEnding
    /// <summary>
    ///  Valida que no este repetido un integrante
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 14/Jul/2016 Created
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
    ///   Actualiza la fila seleccionada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 14/Jul/2016 Created
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

    #region dgrIntegrants_BeginningEdit
    /// <summary>
    ///   Determina si se puede editar la informacion del grid de integrantes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dgrIntegrants_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (GridHelper.IsInEditMode(sender as DataGrid))
      {
        e.Cancel = true;
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
    ///   [vku] 20/Jul/2016 Created
    /// </history>
    private void btnTransfer_Click(object sender, RoutedEventArgs e)
    {
      List<Personnel> lstPersonnelTransfer = (List<Personnel>)dgrIntegrants.ItemsSource;
      frmTeamsTransfer frmTeamsTransfer = new frmTeamsTransfer();
      frmTeamsTransfer.Owner = this;
      frmTeamsTransfer.oldTeamGuestServices = oldTeam;
      frmTeamsTransfer._lstOldPersonnel = lstPersonnelTransfer;
      frmTeamsTransfer._enumTeamType = EnumTeamType.TeamPRs;
      if(frmTeamsTransfer.ShowDialog() == true)
      {
        LoadIntegrantes(oldTeam);
      }
    }
    #endregion

    #endregion 
  }
}
