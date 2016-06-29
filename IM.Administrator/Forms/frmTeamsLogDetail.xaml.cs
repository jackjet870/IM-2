using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmTeamsLogDetail.xaml
  /// </summary>
  public partial class frmTeamLogDetail : Window
  {
    #region Variables
    public TeamLog teamLog = new TeamLog();//Objeto a guardar
    public TeamLog oldTeamLog = new TeamLog();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    public bool blnDate = false;//Para el modo busqueda
    private bool _isClosing = false;
    #endregion
    public frmTeamLogDetail()
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
    /// [emoguel] created 27/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(teamLog, oldTeamLog);
      LoadPersonnels();
      LoadTeamTypes();
      if(enumMode!=EnumMode.preview)
      {
        cmbtlpe.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        txttlDT.IsEnabled = true;
        cmbtlTeamType.IsEnabled = true;
        cmbtlTeam.IsEnabled = true;
        cmbtlPlaceID.IsEnabled = true;
        UIHelper.SetUpControls(teamLog, this);
        if (enumMode == EnumMode.search)
        {
          Title = "Search";
          txttlID.Visibility = Visibility.Collapsed;
          lbltlID.Visibility = Visibility.Collapsed;
          cmbtlTeamType.Visibility = Visibility.Collapsed;
          lblTT.Visibility = Visibility.Collapsed;
          txttlDT.Visibility = Visibility.Collapsed;
          dptlDT.Visibility = Visibility.Visible;
          cmbtlTeam.Visibility = Visibility.Collapsed;
          cmbtlPlaceID.Visibility = Visibility.Collapsed;
          cmbtlChangedBy.IsEnabled = true;
          lblSr.Visibility = Visibility.Collapsed;
          lblTM.Visibility = Visibility.Collapsed;
          if (blnDate)
          {
            dptlDT.SelectedDate = teamLog.tlDT;
          }
        }
        else
        {
          teamLog.tlChangedBy = App.User.User.peID;
          if (enumMode == EnumMode.add)
          {
            teamLog.tlDT = DateTime.Now;
          }
        }
      }
      DataContext = teamLog;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        Close();
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda| Actualiza un registro de Teams Log
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (enumMode == EnumMode.search)
        {
          if (dptlDT.SelectedDate != null)
          {
            blnDate = true;
            teamLog.tlDT = Convert.ToDateTime(dptlDT.SelectedDate);
          }
          else
          {
            blnDate = false;
          }
          _isClosing = true;
          DialogResult = true;
          Close();
        }
        else
        {
          btnAccept.Focus();
          if (enumMode != EnumMode.add && ObjectHelper.IsEquals(teamLog, oldTeamLog))
          {
            _isClosing = true;
            Close();
          }
          else
          {
            #region Insertar|Agregar
            string strMsj = ValidateHelper.ValidateForm(this, "Team Log");
            if (strMsj == "")
            {
              int nRes =await BREntities.OperationEntity(teamLog, enumMode);
              UIHelper.ShowMessageResult("Team Log", nRes);
              if (nRes == 1)
              {
                _isClosing = true;
                DialogResult = true;
                Close();
              }
            }
            else
            {
              UIHelper.ShowMessage(strMsj);
            }
            #endregion
          }
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Team Logs");
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();
    }
    #endregion

    #region cmbtlTeamType_SelectionChanged
    /// <summary>
    /// Carga los combobox de Places dependiendo el valor seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// [emoguel] created 30/05/2016 se volvió async
    /// </history>
    private async void cmbtlTeamType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      try
      {
        switch (teamLog.tlTeamType)
        {
          case "":
            {
              cmbtlTeam.ItemsSource = null;
              cmbtlPlaceID.ItemsSource = null;
              cmbtlPlaceID.Tag = "Location";
              break;
            }
          case "SA":
            {
              cmbtlPlaceID.Tag = "Sales Room";
              lblSr.Content = "Sales Room";
              cmbtlPlaceID.DisplayMemberPath = "srN";
              cmbtlPlaceID.SelectedValuePath = "srID";
              List<SalesRoom> lstSalesRoom = await BRSalesRooms.GetSalesRooms(1, blnTeamLog: true);
              cmbtlPlaceID.ItemsSource = lstSalesRoom;
              break;
            }
          case "GS":
            {
              cmbtlPlaceID.Tag = "Location";
              lblSr.Content = "Location";
              cmbtlPlaceID.DisplayMemberPath = "loN";
              cmbtlPlaceID.SelectedValuePath = "loID";
              List<Location> lstLocations =await BRLocations.GetLocations(1, blnTeamsLog: true);
              cmbtlPlaceID.ItemsSource = lstLocations;
              break;
            }
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Teams Log");
      }      
    }
    #endregion

    #region cmbtlPlaceID_SelectionChanged
    /// <summary>
    /// Carga el combo de Team dependiendo del PlcaeID seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    private void cmbtlPlaceID_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      switch(teamLog.tlTeamType)
      {
        case "":
          {
            cmbtlTeam.ItemsSource = null;
            break;
          }
        case "GS":
          {
            cmbtlTeam.SelectedValuePath = "tgID";
            cmbtlTeam.DisplayMemberPath = "tgN";
            List<TeamGuestServices> lstTeamsGuestServices = BRTeamsGuestServices.GetTeamsGuestServices(1, new TeamGuestServices { tglo = teamLog.tlPlaceID });
            cmbtlTeam.ItemsSource = lstTeamsGuestServices;
            break;
          }
        case "SA":
          {
            cmbtlTeam.SelectedValuePath = "tsID";
            cmbtlTeam.DisplayMemberPath = "tsN";
            List<TeamSalesmen> lstTeamSalesMen = BRTeamsSalesMen.GetTeamsSalesMen(1, new TeamSalesmen { tssr = teamLog.tlPlaceID });
            cmbtlTeam.ItemsSource = lstTeamSalesMen;
            break;
          }
      }

    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        if (enumMode != EnumMode.preview && enumMode != EnumMode.search)
        {
          if (!ObjectHelper.IsEquals(teamLog, oldTeamLog))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result != MessageBoxResult.Yes)
            {
              e.Cancel = true;
            }
          }
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadPersonnels
    /// <summary>
    /// Carga los combos de ChangedBy
    /// </summary>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadPersonnels()
    {
      try
      {
        List<PersonnelShort> lstPersonnel = await BRPersonnel.GetPersonnel();
        if (enumMode == EnumMode.search)
        {
          lstPersonnel.Insert(0, new PersonnelShort { peID = "", peN = "" });
        }
        cmbtlChangedBy.ItemsSource = lstPersonnel;
        cmbtlpe.ItemsSource = lstPersonnel;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Teams Log");
      }
    }
    #endregion

    #region LoadTeamTypes
    /// <summary>
    /// Llena el combobox de TeamType
    /// </summary>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    private async void LoadTeamTypes()
    {
      try
      {
        List<TeamType> lstTeamTypes =await BRTeamTypes.GetTeamTypes(1);
        cmbtlTeamType.ItemsSource = lstTeamTypes;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Team Log");
      }
    }
    #endregion

    #endregion
  }
}
