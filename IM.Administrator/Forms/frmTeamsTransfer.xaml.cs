using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using IM.Model;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using System.Linq;
using System.ComponentModel;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmTeamsTransfer.xaml
  /// </summary>
  public partial class frmTeamsTransfer : Window
  {
    #region Atributos
    public TeamGuestServices oldTeamGuestServices = new TeamGuestServices();//Objeto con los valores iniciales
    public TeamGuestServices teamGuestServices = new TeamGuestServices();//Objeto para llenar el formulario

    public TeamSalesmen oldTeamSalesmen = new TeamSalesmen();//Objeto con los valores iniciales
    public TeamSalesmen teamSalesmen = new TeamSalesmen();//Objeto para llenar el formulario

    public List<Personnel> _lstOldPersonnel = new List<Personnel>();//Lista inicial de personnel
    private List<Personnel> _lstPersonnel = new List<Personnel>();//Lista de Personnel para el viewSource 
    private bool blnClosing = false;
    public EnumTeamType _enumTeamType;
    #endregion

    public frmTeamsTransfer()
    {
      InitializeComponent();
    }

    #region Metodos
    #region LoadPersonnel
    /// <summary>
    ///   Carga los integrantes a transferir
    /// </summary>
    /// <history>
    ///   [vku] 19/Jul/2016 Created
    /// </history>
    public async void LoadPersonnel()
    {
      List<Personnel> lstPersonnelFrom = await BRPersonnel.GetPersonnels();
      List<Personnel> lstPersonnelTo = new List<Personnel>();
      lstPersonnelTo.AddRange(lstPersonnelFrom);
      cboIntegrant.ItemsSource = lstPersonnelFrom;
      cboIntegrantTo.ItemsSource = lstPersonnelTo;
      _lstPersonnel.AddRange(_lstOldPersonnel);
      grdFrom.ItemsSource = _lstPersonnel;
      skpStatus.Visibility = Visibility.Collapsed;
      lblIntegrantFrom.Content = "Integrants: " + _lstPersonnel.Count;
    }
    #endregion

    #region LoadLocations
    /// <summary>
    ///   Carga las locaciones relacionados a TeamGuestServices
    /// </summary>
    /// <history>
    ///   [vku] 19/Jul/2016 Created
    /// </history>
    protected async void LoadLocations()
    {
      try
      {
        List<object> lstLocationsFrom = await BRLocations.GetLocationByTeamGuestService();
        cboPlaceIDFrom.ItemsSource = lstLocationsFrom;
        cboPlaceIDFrom.SelectedValuePath = "loID";
        cboPlaceIDFrom.DisplayMemberPath = "loN";

        List<object> lstLocationsTo = new List<object>();
        lstLocationsTo.AddRange(lstLocationsFrom);
        cboPlaceIDTo.ItemsSource = lstLocationsTo;
        cboPlaceIDTo.SelectedValuePath = "loID";
        cboPlaceIDTo.DisplayMemberPath = "loN";
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Teams PRs");
      }
    }
    #endregion

    #region LoadSalesRoom
    /// <summary>
    ///   Carga las salas de ventas relacionados a TeamSalesmen
    /// </summary>
    /// <history>
    ///   [vku] 25/Jul/2016 Created
    /// </history>
    public async void LoadSalesRoom()
    {
      try
      {
        List<object> lstSalesRoomFrom = await BRSalesRooms.GetSalesRoombyTeamSalesMen();
        cboPlaceIDFrom.ItemsSource = lstSalesRoomFrom;
        cboPlaceIDFrom.SelectedValuePath = "srID";
        cboPlaceIDFrom.DisplayMemberPath = "srN";

        List<object> lstSalesRoomTo = new List<object>();
        lstSalesRoomTo.AddRange(lstSalesRoomFrom);
        cboPlaceIDTo.ItemsSource = lstSalesRoomTo;
        cboPlaceIDTo.SelectedValuePath = "srID";
        cboPlaceIDTo.DisplayMemberPath = "srN";
      }catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Teams Salesmen");
      }
    }
    #endregion

    #region Validate
    /// <summary>
    ///   Valida los datos
    /// </summary>
    /// <returns></returns>
    /// <history>
    ///   [vku] 21/Jul/2016 Created
    /// </history>
    private bool Validate()
    {
      bool blnValidate;
      blnValidate = true;
      if (!ValidateHelper.ValidateRequired(cboPlaceIDTo, "destination " + lblPlaceIDFrom.Content.ToString()))
        blnValidate = false;
      else
      {
        if(!ValidateHelper.ValidateRequired(cboTeamTo, "destination team"))
          blnValidate = false;
        else
        {
          if(cboPlaceIDFrom.SelectedValue.ToString() == cboPlaceIDTo.SelectedValue.ToString() && cboTeamFrom.SelectedValue.ToString() == cboTeamTo.SelectedValue.ToString())
          {
            UIHelper.ShowMessage("Specify a diferent destination team.", MessageBoxImage.Information, "IM.Administrator");
            cboPlaceIDTo.Focus();
            blnValidate = false;
          }
          else
          {
            if(grdTo.Items.Count == 0)
            {
              UIHelper.ShowMessage("Specify at least one integrant to transfer.", MessageBoxImage.Information, "IM.Administrator");
              grdTo.Focus();
              blnValidate = false;
            }
          }    
        }
      }
      return blnValidate;
    }
    #endregion

    #region Sort
    /// <summary>
    ///   Ordena los integrantes por nombre
    /// </summary>
    /// <history>
    ///   [vku] 22/Jul/2016 Created
    /// </history>
    private void Sort()
    {
      grdFrom.Items.SortDescriptions.Clear();
      grdFrom.Items.SortDescriptions.Add(new SortDescription("peN", ListSortDirection.Ascending));
      grdFrom.Items.Refresh();

      grdTo.Items.SortDescriptions.Clear();
      grdTo.Items.SortDescriptions.Add(new SortDescription("peN", ListSortDirection.Ascending));
      grdTo.Items.Refresh();

      lblIntegrantFrom.Content = "Integrants: " + _lstPersonnel.Count;
      lblIntegrantTo.Content = "Integrants: " + grdTo.Items.Count;
    }
    #endregion
    #endregion

    #region Eventos

    #region btnTransfer_Click
    /// <summary>
    ///   Transfiere los integrantes de un equipo a otro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jul/2016 Created
    /// </history>
    private async void btnTransfer_Click(object sender, RoutedEventArgs e)
    {
      if (Validate())
      {
        skpStatus.Visibility = Visibility.Visible;
        txtStatus.Text = "Saving Data...";
        List<Personnel> lstPersonnels = grdTo.Items.Cast<Personnel>().ToList();
        int nRes = await BRTransfer.TransferTeamMembers(App.User.User.peID, cboPlaceIDTo.SelectedValue.ToString(), cboTeamTo.SelectedValue.ToString(), lstPersonnels, _enumTeamType);
        skpStatus.Visibility = Visibility.Collapsed;
        UIHelper.ShowMessageResult("Integrants", nRes);
        if(nRes > 0)
        {
          blnClosing = true;
          DialogResult = true;
          Close();
        }
      }
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    ///   Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jul/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (!blnClosing) { blnClosing = true; Close(); }
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    ///   Carga los datos en el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 19/Jul/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      switch (_enumTeamType)
      {
        case EnumTeamType.TeamPRs:
          {
            lblPlaceIDFrom.Content = "Location";
            lblPlaceIDTo.Content = "Location";
            ObjectHelper.CopyProperties(teamGuestServices, oldTeamGuestServices);
            LoadLocations();
            DataContext = teamGuestServices;
            cboPlaceIDFrom.SelectedValue = teamGuestServices.tglo; 
            break;
          }
        case EnumTeamType.TeamSalesmen:
          {
            lblPlaceIDFrom.Content = "Sales Room";
            lblPlaceIDTo.Content = "Sales Room";
            ObjectHelper.CopyProperties(teamSalesmen, oldTeamSalesmen);
            LoadSalesRoom();
            DataContext = teamSalesmen;
            cboPlaceIDFrom.SelectedValue = teamSalesmen.tssr;
            grdFrom.Items.SortDescriptions.Clear();
            grdFrom.Items.SortDescriptions.Add(new SortDescription("peN", ListSortDirection.Ascending));
            grdFrom.Items.Refresh();
            break;
          }
      }
      LoadPersonnel();
    }
    #endregion

    #region cboPlaceIDTo_SelectionChanged
    /// <summary>
    ///   Carga los equipos (hacia) segun el lugar seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 19/Jul/2016 Created
    /// </history>
    private async void cboPlaceIDTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      string placeIDTo = cboPlaceIDTo.SelectedValue.ToString();
      switch (_enumTeamType)
      {
        case EnumTeamType.TeamPRs:
          {
            List<TeamGuestServices> lstTeamTo = await BRTeamsGuestServices.GetTeamsGuestServices(1, teamGuestServices: new TeamGuestServices { tglo = placeIDTo });
            cboTeamTo.ItemsSource = lstTeamTo;
            cboTeamTo.SelectedValuePath = "tgID";
            cboTeamTo.DisplayMemberPath = "tgN";
            break;
          }
        case EnumTeamType.TeamSalesmen:
          {
            List<TeamSalesmen> lstTeamTo = await BRTeamsSalesMen.GetTeamsSalesMen(1, teamSalesMen: new TeamSalesmen { tssr = placeIDTo });
            cboTeamTo.ItemsSource = lstTeamTo;
            cboTeamTo.SelectedValuePath = "tsID";
            cboTeamTo.DisplayMemberPath = "tsN";
            break;
          }
      }
      
    }
    #endregion

    #region cboPlaceIDFrom_SelectionChanged
    /// <summary>
    ///   Carga los equipos (desde) segun el lugar seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void cboPlaceIDFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      string placeIDFrom = cboPlaceIDFrom.SelectedValue.ToString();
      switch (_enumTeamType)
      {
        case EnumTeamType.TeamPRs:
          {
            List<TeamGuestServices> lstTeamFrom = await BRTeamsGuestServices.GetTeamsGuestServices(1, teamGuestServices: new TeamGuestServices { tglo = placeIDFrom });
            cboTeamFrom.ItemsSource = lstTeamFrom;
            cboTeamFrom.SelectedValuePath = "tgID";
            cboTeamFrom.DisplayMemberPath = "tgN";
            cboTeamFrom.SelectedValue = teamGuestServices.tgID;
            break;
          }
        case EnumTeamType.TeamSalesmen:
          {
            List<TeamSalesmen> lstTeamFrom = await BRTeamsSalesMen.GetTeamsSalesMen(1, teamSalesMen: new TeamSalesmen { tssr = placeIDFrom });
            cboTeamFrom.ItemsSource = lstTeamFrom;
            cboTeamFrom.SelectedValuePath = "tsID";
            cboTeamFrom.DisplayMemberPath = "tsN";
            cboTeamFrom.SelectedValue = teamSalesmen.tsID;
            break;
          }    
      }
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    ///   Mueve los integrantes seleccionados del grid de integrantes origen al grid de integrantes destino
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 20/Jul/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      if(grdFrom.SelectedItems.Count > 0)
      {
        List<Personnel> selectedItems = new List<Personnel>();
        selectedItems.AddRange(grdFrom.SelectedItems.Cast<Personnel>().ToList());
        foreach (Personnel SelectedItem in selectedItems)
        {
          grdTo.Items.Add(SelectedItem);
          _lstPersonnel.Remove(SelectedItem);
        }
        Sort();
      }
    }
    #endregion

    #region btnAddAll_Click
    /// <summary>
    ///   Mueve todos los integrantes del grid de integrantes origen al grid de integrantes destino
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 20/Jul/2016 Created
    /// </history>
    private void btnAddAll_Click(object sender, RoutedEventArgs e)
    {
      List<Personnel> Items = new List<Personnel>();
      Items.AddRange((List<Personnel>)grdFrom.ItemsSource);
      foreach (Personnel Item in Items)
      {
        grdTo.Items.Add(Item);
        _lstPersonnel.Remove(Item);
      }
      Sort();
    }
    #endregion

    #region btnReturn_Click
    /// <summary>
    ///   Mueve los integrantes seleccionados del grid de integrantes destino al grid de integrantes origen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jul/2016 Created
    /// </history>
    private void btnReturn_Click(object sender, RoutedEventArgs e)
    {
      if (grdTo.SelectedItems.Count > 0)
      {
        List<Personnel> selectedItems = new List<Personnel>();
        selectedItems.AddRange(grdTo.SelectedItems.Cast<Personnel>().ToList());
        foreach (Personnel SelectedItem in selectedItems)
        {
          _lstPersonnel.Add(SelectedItem);
          grdTo.Items.Remove(SelectedItem);
        }
        Sort();
      }
    }
    #endregion

    #region btnReturnAll_Click
    /// <summary>
    ///   Mueve todos los integrantes del grid de integrantes destino al grid de integrantes origen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jul/2016 Created
    /// </history>
    private void btnReturnAll_Click(object sender, RoutedEventArgs e)
    {
      List<Personnel> Items = new List<Personnel>();
      Items.AddRange(grdTo.Items.Cast<Personnel>().ToList());
      foreach (Personnel Item in Items)
      {
        _lstPersonnel.Add(Item);
        grdTo.Items.Remove(Item);
      }
      Sort();
    }
    #endregion

    #region Window_Closing
    /// <summary>
    ///   Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jul/2016 Created
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

    #endregion
  }
}
