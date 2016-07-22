using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using IM.Model;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using System.Linq;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmTeamsTransfer.xaml
  /// </summary>
  public partial class frmTeamsTransfer : Window
  {
    #region Atributos
    public TeamGuestServices oldTeam = new TeamGuestServices();//Objeto con los valores iniciales
    public TeamGuestServices team = new TeamGuestServices();//Objeto para llenar el formulario
    public List<Personnel> _lstOldPersonnel = new List<Personnel>();//Lista inicial de personnel
    private List<Personnel> _lstPersonnel = new List<Personnel>();//Lista de Personnel para el viewSource 
    private bool blnClosing = false;
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
        List<object> lstLocationsTo = new List<object>();
        lstLocationsTo.AddRange(lstLocationsFrom);
        cboPlaceIDFrom.ItemsSource = lstLocationsFrom;
        cboPlaceIDFrom.SelectedValuePath = "loID";
        cboPlaceIDFrom.DisplayMemberPath = "loN";

        cboPlaceIDTo.ItemsSource = lstLocationsTo;
        cboPlaceIDTo.SelectedValuePath = "loID";
        cboPlaceIDTo.DisplayMemberPath = "loN";

      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "TeamsPRs");
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
      if (!ValidateHelper.ValidateRequired(cboPlaceIDTo, "destination location"))
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
        int nRes = await BRTeamsGuestServices.Transfer(App.User.User.peID, cboPlaceIDTo.SelectedValue.ToString(), cboTeamTo.SelectedValue.ToString(), lstPersonnels);
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
      ObjectHelper.CopyProperties(team, oldTeam);
      LoadLocations();
      LoadPersonnel();
      DataContext = team;
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
      string loIDTo = cboPlaceIDTo.SelectedValue.ToString();
      List<TeamGuestServices> lstTeamTo = await BRTeamsGuestServices.GetTeamsGuestServices(1, teamGuestServices: new TeamGuestServices {  tglo = loIDTo });
      cboTeamTo.ItemsSource = lstTeamTo;
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
      string loIDFrom = cboPlaceIDFrom.SelectedValue.ToString();
      List<TeamGuestServices> lstTeamFrom = await BRTeamsGuestServices.GetTeamsGuestServices(1, teamGuestServices: new TeamGuestServices { tglo = loIDFrom });
      cboTeamFrom.ItemsSource = lstTeamFrom;
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
        grdFrom.Items.Refresh();
        lblIntegrantFrom.Content = "Integrants: " + _lstPersonnel.Count;
        lblIntegrantTo.Content = "Integrants: " + grdTo.Items.Count;
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
      grdFrom.Items.Refresh();
      lblIntegrantFrom.Content = "Integrants: " + _lstPersonnel.Count;
      lblIntegrantTo.Content = "Integrants: " + grdTo.Items.Count;
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
        grdFrom.Items.Refresh();
        lblIntegrantFrom.Content = "Integrants: " + _lstPersonnel.Count;
        lblIntegrantTo.Content = "Integrants: " + grdTo.Items.Count;
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
      _lstPersonnel.OrderBy(pe => pe.peN);
      grdFrom.Items.Refresh();
      lblIntegrantFrom.Content = "Integrants: " + _lstPersonnel.Count;
      lblIntegrantTo.Content = "Integrants: " + grdTo.Items.Count;
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
