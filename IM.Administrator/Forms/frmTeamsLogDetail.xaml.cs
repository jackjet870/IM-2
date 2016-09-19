using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

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
      if(enumMode!=EnumMode.ReadOnly)
      {
        cmbtlpe.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;        
        cmbtlTeamType.IsEnabled = true;
        cmbtlTeam.IsEnabled = true;
        cmbtlPlaceID.IsEnabled = true;
        UIHelper.SetUpControls(teamLog, this);
        dptlDT.IsReadOnly = false;
        if (enumMode == EnumMode.Search)
        {
          Title = "Search";
          txttlID.Visibility = Visibility.Collapsed;
          lbltlID.Visibility = Visibility.Collapsed;
          cmbtlTeamType.Visibility = Visibility.Collapsed;
          lblTT.Visibility = Visibility.Collapsed;
          BindingOperations.ClearBinding(dptlDT, DateTimePicker.ValueProperty);
          dptlDT.KeyDown += dptlDT_KeyDown;
          dptlDT.AllowTextInput = true;
          dptlDT.FormatString = "ddd d MMM yyyy";
          dptlDT.TimePickerVisibility = Visibility.Collapsed;
          cmbtlTeam.Visibility = Visibility.Collapsed;
          cmbtlPlaceID.Visibility = Visibility.Collapsed;
          cmbtlChangedBy.IsEnabled = true;
          lblSr.Visibility = Visibility.Collapsed;
          lblTM.Visibility = Visibility.Collapsed;
          if (blnDate)
          {
            dptlDT.Value = teamLog.tlDT;
          }
        }
        else
        {
          teamLog.tlChangedBy = Context.User.User.peID;
          if (enumMode == EnumMode.Add)
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
        if (enumMode == EnumMode.Search)
        {
          if (dptlDT.Value != null)
          {
            blnDate = true;
            teamLog.tlDT = Convert.ToDateTime(dptlDT.Value);
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
          if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(teamLog, oldTeamLog))
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
        UIHelper.ShowMessage(ex);
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
              List<Model.Location> lstLocations =await BRLocations.GetLocations(1, blnTeamsLog: true);
              cmbtlPlaceID.ItemsSource = lstLocations;
              break;
            }
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
    private async void cmbtlPlaceID_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            List<TeamGuestServices> lstTeamsGuestServices = await BRTeamsGuestServices.GetTeamsGuestServices(1, new TeamGuestServices { tglo = teamLog.tlPlaceID });
            cmbtlTeam.ItemsSource = lstTeamsGuestServices;
            break;
          }
        case "SA":
          {
            cmbtlTeam.SelectedValuePath = "tsID";
            cmbtlTeam.DisplayMemberPath = "tsN";
            List<TeamSalesmen> lstTeamSalesMen = await BRTeamsSalesMen.GetTeamsSalesMen(1, new TeamSalesmen { tssr = teamLog.tlPlaceID });
            cmbtlTeam.ItemsSource = lstTeamSalesMen;
            break;
          }
      }

    }
    #endregion

    #region dptlDT_KeyDown
    /// <summary>
    /// Deja vacio el valor del dateTimePicker
    /// </summary>
    /// <history>
    /// [emoguel] 29/08/2016 created
    /// </history>
    private void dptlDT_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        e.Handled = true;
        dptlDT.Value = null;
      }
      else if (!(e.Key == Key.Escape))
      {
        e.Handled = true;
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
        if (enumMode != EnumMode.ReadOnly && enumMode != EnumMode.Search)
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
        if (enumMode == EnumMode.Search)
        {
          lstPersonnel.Insert(0, new PersonnelShort { peID = "", peN = "ALL" });
        }
        cmbtlChangedBy.ItemsSource = lstPersonnel;
        cmbtlpe.ItemsSource = lstPersonnel;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #endregion
  }
}
