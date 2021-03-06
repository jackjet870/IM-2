﻿using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace IM.Inhouse.Forms
{
  /// <summary>
  /// Interaction logic for frmGuestsGroups.xaml
  /// </summary>
  public partial class frmGuestsGroups : Window
  {
    #region Atributos
    int _groupID, _guestID, _guestIDToAdd;
    DateTime _date;
    EnumAction _enumAction;
    public GuestsGroup _guestsGroup = new GuestsGroup();
    GuestsGroup _oldGuestsGroup = new GuestsGroup();
    public Guest _guest = new Guest();
    List<GuestsGroup> _lstGuestsGroups;
    List<Guest> _lstGuest;
    List<Guest> _lstGuestTemp;
    List<Guest> _oldListGuests = new List<Guest>();
    bool _mode; //false nuevo | true modifica

    #endregion

    #region Metodos

    #region ValidateGroupSelected

    /// <summary>
    /// Valida que el gridGuestGroup no este vacio 
    /// </summary>
    /// <history>
    /// [ecanul] 28/03/2016 Created
    /// </history>
    bool ValidateGroupSelected()
    {
      bool valido = false;      
      if (dtgGuestsGroup.SelectedItems.Count != 0)
        valido = true;
      return valido;
    }

    #endregion

    #region LoadGrdGuestsGroups

    /// <summary>
    /// Carga el DataGrid GuestsGroups
    /// </summary>
    /// <history>
    /// [ECANUL] 28-03-2016 Created
    /// [ecaul]17/06/2016 Modified. Implementado Asincronia
    /// </history>
    private async Task<int> LoadGrdGuestsGroups()
    {
      StaStart("Loading Groups...");
      if (!string.IsNullOrEmpty(txtgxID.Text))
        _guestsGroup.gxID = Convert.ToInt32(txtgxID.Text);
      _lstGuestsGroups = await BRGuestsGroups.GetGuestsGroups(_guest, _guestsGroup);
      dtgGuestsGroup.ItemsSource = _lstGuestsGroups;

      string s = dtgGuestsGroup.Items.Count == 1 ? "Group" : "Groups";
      StatusBarReg.Content = $"{dtgGuestsGroup.Items.Count} {s}";
      StaEnd();
      txtgxID.Text = string.Empty;
      return _lstGuestsGroups.Count();
    }

    #endregion

    #region ClearControls

    /// <summary>
    /// Limpia los controles y los vuelve a su estado por defecto
    /// </summary>
    /// <history>[ECANUL] 30-03-2016 Created</history>
    void ClearControls()
    {
      txtgxID.Text = "";
      txtgxN.Text = "";
      txtguID.Text = "";
      txtguLastName1.Text = "";
      txtID.Text = "";
      txtDescription.Text = "";
    }

    #endregion

    #region LoadGridGuestsGroupsIntegrants

    /// <summary>
    /// Carga el Grid GuestsGroupsIntegrants
    /// </summary>
    /// <param name="gx">Grupo al que pertenecen los integrantes</param>
    /// <history>[ECANUL] 29-03-2016 Created</history>
    void LoadGridGuestsGroupsIntegrants(GuestsGroup gx)
    {
      StaStart("Loading Integrants...");
      _lstGuest = BRGuests.GetGuestsGroupsIntegrants(gx);
      if (_lstGuest.Count != 0)
      {
        if (_enumAction == EnumAction.AddTo)
          AddGuestToGridGuestsGroupsIntegrants(_guestID);
        else
          dtgGuestGroupIntegrants.ItemsSource = _lstGuest;
        lblIntegrants.Content = "Integrants: " + dtgGuestGroupIntegrants.Items.Count;
      }
      _oldListGuests = (List<Guest>)dtgGuestGroupIntegrants.ItemsSource;
      dtgGuestGroupIntegrants.IsEnabled = true;
      StaEnd();
    }

    #endregion

    #region LoadGuestsGroupsInfo

    /// <summary>
    /// Garga en los controles la informacion del GuestsGroupSeleccionado
    /// </summary>
    /// <history>[ECANUL] 30-03-2016 Created</history>
    void LoadGuestsGroupsInfo()
    {
      StaStart("Loading Guests Group Info...");
      GuestsGroup gx = new GuestsGroup();
      List<GuestsGroup> guestGroup = dtgGuestsGroup.SelectedItems.OfType<GuestsGroup>().ToList();
      if (guestGroup.Count == 0)//Si no se ha seleccionado ningun "Row" y se invoca la pocicion 0 por defecto
        guestGroup = (List<GuestsGroup>)dtgGuestsGroup.ItemsSource;
      if (dtgGuestsGroup.Items.Count != 0)
      {
        txtID.Text = guestGroup[0].gxID.ToString();
        txtDescription.Text = guestGroup[0].gxN;
        gx.gxID = guestGroup[0].gxID;
        gx.gxN = guestGroup[0].gxN;
        LoadGridGuestsGroupsIntegrants(gx);
      }
      StaEnd();
    }

    #endregion

    #region AddGuestToGridGuestsGroupsIntegrants

    /// <summary>
    /// Agrega un Huesped a la tabla GuestsGroupsIntegrants Cuando se manda desde frmRegister
    /// </summary>
    /// <param name="id">Si solo se tiene el ID | 0 si si ya se tiene el guesped a guardar</param>
    /// <param name="guest">Guesped a Guardar en caso de que ya se tenga toda la informacion</param>
    async void AddGuestToGridGuestsGroupsIntegrants(int id, Guest guest = null)
    {
      Guest gu;
      if (id != 0)
      {
        if (_lstGuest == null)
          _lstGuestTemp = new List<Guest>();
        else
          _lstGuestTemp = _lstGuest;
        _lstGuestTemp.Add(await BRGuests.GetGuest(id));
      }
      else
      {
        gu = guest;
        _lstGuestTemp.Add(gu);
      }
      dtgGuestGroupIntegrants.ItemsSource = _lstGuestTemp;
      lblIntegrants.Content = "Integrants: " + dtgGuestGroupIntegrants.Items.Count;
    }

    #endregion

    #region CaseLoad

    /// <summary>
    /// Selecciona la accion a hacer segun EnumAction
    /// </summary>
    /// <HISTORY>[ECANUL] 38-03-2016 CREATED</HISTORY>
    async void CaseLoad()
    {
      switch (_enumAction)
      {
        case EnumAction.None:
          await LoadGrdGuestsGroups();
          dtgGuestGroupIntegrants.SelectedItem = 0;
          LoadGuestsGroupsInfo();
          break;
        case EnumAction.Search:
          await LoadGrdGuestsGroups();
          EnableControls(0);
          break;
        case EnumAction.Add:
          AddGuestToGridGuestsGroupsIntegrants(_guestID);
          break;
        case EnumAction.AddTo:
          //txtID.Text = groupID.ToString();
          if (_groupID != 0)
          {
            txtgxID.Text = _groupID.ToString();
            await LoadGrdGuestsGroups();
            LoadGuestsGroupsInfo();
            dtgGuestGroupIntegrants.IsEnabled = true;
          }
          break;
      }
    }

    #endregion

    #region EnableControls

    /// <summary>
    /// Habilita o Inhabilita controles segun sea el caso
    /// </summary>
    /// <param name="function">0 Search/Default | 1 Add | 2 Edit | 3 Vista(Cuando se selecciona un Huesped con grupo)</param>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// </history>
    void EnableControls(int function)
    {
      switch (function)
      {
        case 0: //Search/Default
          //Habilita
          grdCriteria.IsEnabled = true;
          grdGroup.IsEnabled = true;
          btnShow.IsEnabled = true;
          btnAdd.IsEnabled = true;
          btnEdit.IsEnabled = true;
          btnDelete.IsEnabled = true;
          //Inhabilita
          txtID.IsEnabled = false;
          txtDescription.IsEnabled = false;
          btnSave.IsEnabled = false;
          btnCancel.IsEnabled = false;
          dtgGuestGroupIntegrants.CanUserAddRows = false;
          dtgGuestGroupIntegrants.IsReadOnly = true;
          btnAddGuest.Visibility = Visibility.Collapsed;
          break;
        case 1: //ADD 
          //Inhabilita
          grdCriteria.IsEnabled = false;
          grdGroup.IsEnabled = false;
          btnShow.IsEnabled = false;
          btnAdd.IsEnabled = false;
          btnEdit.IsEnabled = false;
          btnDelete.IsEnabled = false;
          //Habilita
          txtDescription.IsEnabled = true;
         // dtgGuestGroupIntegrants.CanUserAddRows = true;
          dtgGuestGroupIntegrants.IsReadOnly = false;
          dtgGuestGroupIntegrants.Focus();
          _lstGuestsGroups = new List<GuestsGroup>();
          dtgGuestGroupIntegrants.ItemsSource = _lstGuestsGroups;
          grdListGuest.IsEnabled = true;
          btnSave.IsEnabled = true;
          btnCancel.IsEnabled = true;
          btnAddGuest.Visibility = Visibility.Visible;
          break;
        case 2: //Edit
          //Inhabilita
          grdCriteria.IsEnabled = false;
          grdGroup.IsEnabled = false;
          btnShow.IsEnabled = false;
          btnAdd.IsEnabled = false;
          btnEdit.IsEnabled = false;
          btnDelete.IsEnabled = false;
          //Habilita
          txtDescription.IsEnabled = true;
          //dtgGuestGroupIntegrants.CanUserAddRows = true;
          dtgGuestGroupIntegrants.IsReadOnly = false;
          dtgGuestGroupIntegrants.Focus();
          grdListGuest.IsEnabled = true;
          btnSave.IsEnabled = true;
          btnCancel.IsEnabled = true;
          btnAddGuest.Visibility = Visibility.Visible;
          break;
        case 3:
          //inhabilita
          grdCriteria.IsEnabled = false;
          btnSave.IsEnabled = false;
          btnCancel.IsEnabled = false;
          dtgGuestGroupIntegrants.CanUserAddRows = false;
          dtgGuestGroupIntegrants.IsReadOnly = true;
          btnAddGuest.Visibility = Visibility.Collapsed;
          //Habilita
          grdGroup.IsEnabled = true;
          btnShow.IsEnabled = true;
          btnEdit.IsEnabled = true;
          btnDelete.IsEnabled = true;
          break;
      }
    }

    #endregion

    #region AddGuests

    /// <summary>
    /// Abre el formulario frmSearchGuest y agrega los guespedes buscados al grid
    /// </summary>
    /// <history>
    /// [ECANUL] 01-04-2016 Created
    /// </history>
    void AddGuests()
    {
      frmSearchGuest frmSGuest = new frmSearchGuest(Context.User, EnumProgram.Inhouse, _date);
      frmSGuest.Owner = this;
      frmSGuest.ShowInTaskbar = false;
      frmSGuest.lstGuestAdd = new List<Guest>();

      if (_lstGuest == null) //Solo se abre desde el boton y se da new group
        _lstGuest = new List<Guest>();

      if (_lstGuestTemp == null) //Solo si es primer Guest en nuevo grupo pasa
        _lstGuestTemp = new List<Guest>();
      //Valida que se haya cerrado
      if (!frmSGuest.ShowDialog().Value)
      {

        //Valida que haya sido Ok y no cancel
        if (!frmSGuest.cancel)
        {
          List<Guest> tempList = new List<Guest>();
          if (_lstGuest.Count != 0)
            _lstGuestTemp = _lstGuest;
          tempList.AddRange(frmSGuest.lstGuestAdd);
          //Recorre cada Nuevo Guest a agregar
          foreach (Guest newGuest in tempList)
          {//Recorre los Guests Existentes
            bool exist = false;
            foreach (Guest gu in _lstGuestTemp)
            {
              if (newGuest.guID == gu.guID)
              {
                exist = true;
                break;
              }
            }
            if (exist)
              UIHelper.ShowMessage($"The Guest {newGuest.guID} already exists in the group", MessageBoxImage.Error, "Can´t Add Guest");
            else
              _lstGuestTemp.Add(newGuest);
          }
        }
        dtgGuestGroupIntegrants.ItemsSource = null;
        dtgGuestGroupIntegrants.ItemsSource = _lstGuestTemp;

        lblIntegrants.Content = "Integrants: " + _lstGuestTemp.Count;
      }
    }

    #endregion

    #region StaStart

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje a mostrar</param>
    /// <history>[ECANUL] 28-03-2016 Created </history>
    private void StaStart(String message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
    }

    #endregion

    #region StaEnd

    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    /// <history>[ECANUL] 28-03-2016 Created </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
    }

    #endregion

    #endregion

    #region Constructores y Destructores
    public frmGuestsGroups(int GroupID, int GuestID, int GuestIDToAdd, DateTime Date, EnumAction EnumAct)
    {
      InitializeComponent();
      _groupID = GroupID;
      _guestID = GuestID;
      _guestIDToAdd = GuestIDToAdd;
      _date = Date;
      _enumAction = EnumAct;
    }

    #region Window_Closing
    /// <summary>
    /// Valida Que no este en modo edicion de datos antes de cerrar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>[ECANUL]04-04-2016 Created</history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (txtDescription.IsEnabled == true)
      {
        MessageBoxResult msr = UIHelper.ShowMessage("This form is currently in edit mode. Please save or undo your changes before closing it.", MessageBoxImage.Question);
        if (msr == MessageBoxResult.No)
          e.Cancel = true;
      }
    }
    #endregion

    #endregion

    #region Metodos del Formulario

    #region dtgGuestsGroup_SelectionChanged
    /// <summary>
    /// Carga la informacion del grupo seleccionado
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// </history>
    private void dtgGuestsGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LoadGuestsGroupsInfo();
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Busca un grupo segun los parametros dados
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// [ecanul] 21/06/2016 Modified. Implementada asincronia
    /// </history>
    private async void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      if (DateHelper.ValidateValueDate(dtpGuestStart, dtpGuestEnd))
        await LoadGrdGuestsGroups();
    }
    #endregion

    #region btnAdd_click
    /// <summary>
    /// habilita los controles para agregar un nuevo grupo
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      ClearControls();
      EnableControls(1);
      _mode = false;
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cancela la edicion y restaura los datos 
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      //Limpiar Controles 
      EnableControls(0);
      if (_lstGuestTemp != null && _lstGuestTemp.Count != 0)
      {
        dtgGuestGroupIntegrants.ItemsSource = _lstGuest;
        _lstGuestTemp = null;
        txtDescription.Text = "";
      }

      if (_enumAction == EnumAction.Add)
        Close();
    } 
    #endregion

    #region btnEdit_Click
    /// <summary>
    /// Permite la edicion de un grupo
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      if (dtgGuestGroupIntegrants.Items.Count != 0)
      {
        LoadGuestsGroupsInfo();
        EnableControls(2);
        _lstGuestTemp = _lstGuest;
      }
      else if (ValidateGroupSelected())
      {
        LoadGuestsGroupsInfo();
        EnableControls(2);
      }
      else
        UIHelper.ShowMessage("Select a Guests Group", MessageBoxImage.Exclamation, "Can''t Edit");
      _mode = true;
    }
    #endregion

    #region btnAddGuest_Click
    /// <summary>
    /// Abre el formulario para agregar un nuevo guest 
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// </history>
    private void btnAddGuest_Click(object sender, RoutedEventArgs e)
    {
      AddGuests();
    }
    #endregion

    #region btnShow_Click
    /// <summary>
    /// Carga la informacion del Grupo seleccionado
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// </history>
    private void btnShow_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateGroupSelected())
        LoadGuestsGroupsInfo();
    } 
    #endregion

    #region btnSave_Clic
    /// <summary>
    /// Guarda un grupo y sus integrantes
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// [ecanul] 21/06/2016 Modified. Agregada Asincronia.
    /// [ecanul] 09/09/2016 Modified. Ahora se actualiza el formulario despues de guardar 
    /// </history>
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        List<Guest> lstGuests = (List<Guest>)dtgGuestGroupIntegrants.ItemsSource;
        _guestsGroup = new GuestsGroup();
        //Si no ha especificado un nombre de grupo
        if (string.IsNullOrEmpty(txtDescription.Text))
        {
          UIHelper.ShowMessage("Specify the Guest Group Name", MessageBoxImage.Error);
          return;
        }
        //Si tiene menos de 2 integrantes
        if (_lstGuestTemp == null || _lstGuestTemp.Count <= 1)
        {
          UIHelper.ShowMessage("Specify at least 2 integrants", MessageBoxImage.Error);
          return;
        }
        //Si se tiene un grupo
        if (!string.IsNullOrEmpty(txtID.Text))
        {//Obtiene el registro existente en base de datos
          _guestsGroup.gxID = Convert.ToInt32(txtID.Text.Trim());
          _guestsGroup = await BRGuestsGroups.GetGuestsGroup(_guestsGroup.gxID);
          _oldListGuests = _guestsGroup.Guests.ToList();
        }
        _guestsGroup.gxN = txtDescription.Text.Trim();
        //Guests a agregar
        List<Guest> lstAdd = lstGuests.Where(gu => !_oldListGuests.Any(gui => gui.guID == gu.guID)).ToList();
        //Guests a eliminar
        List<Guest> lstDel = _oldListGuests.Where(gu => !lstGuests.Any(gui => gui.guID == gu.guID)).ToList();
        //Guarda los datos
        int res = await BRGuestsGroups.SaveGuestGroup(_guestsGroup, _mode, lstAdd, lstDel);

        EnableControls(0);

        txtgxID.Text = res.ToString();
        await LoadGrdGuestsGroups();
        var gg = new GuestsGroup { gxID = res };
        LoadGridGuestsGroupsIntegrants(gg);
        txtgxID.Text = string.Empty;
        txtID.Text = res.ToString();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    } 
    #endregion

    #region DateTimePiker Changed
    /// <summary>
    /// Cambia los datos del datetime piker Start
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// </history>
    private void dtpGuestStart_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      DateHelper.ValidateValueDate((DateTimePicker)sender);
    }
    /// <summary>
    /// Cambia los datos del datetime piker End
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// </history>
    private void dtpGuestEnd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      DateHelper.ValidateValueDate((DateTimePicker)sender);
    } 
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga los datos iniciales del formulario 
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// [ecanul] 09/09/2016 Modified. Ahora no carga el listado al iniciar el formulario
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);

      #region Validacion de campos
      UIHelper.SetUpControls(new GuestsGroup(), grdGroupInfo, blnCharacters: true);
      UIHelper.SetUpControls(new Guest(), grdGuestInfo, blnCharacters: true);
      grdGroupInfo.DataContext = _guestsGroup;
      grdGuestInfo.DataContext = _guest;
      #endregion
      var loadList = (_enumAction == EnumAction.None);
      if (_enumAction == EnumAction.None)
      {
        txtgxID.Text = _groupID.ToString();
      }
      switch (_enumAction)
      {
        case EnumAction.Add: //Crea un nuevo grupo
          EnableControls(1);
          _mode = false;
          break;
        case EnumAction.None: //Midifica un grupo existente
          EnableControls(3);
          break;
        default:  //Habilita los controles de busqueda y botones sin guardado
          EnableControls(0);
          break;
      }

      //Muestra u oculta los botones Add y Edit
      if (_enumAction == EnumAction.Search)
        btnAdd.Visibility = Visibility.Visible;
      else
        btnAdd.Visibility = Visibility.Hidden;
      if (_enumAction != EnumAction.Add)
        btnEdit.Visibility = Visibility.Visible;
      else
        btnEdit.Visibility = Visibility.Hidden;
      //Fecha Inicial
      _guest.guCheckInD = _date.AddDays(-7);
      //Fecha Final
      _guest.guCheckOutD = _date;
      //se impide modificar datos si esta en modo solo lectura
      if (App.Current.Properties.IsReadOnly)
      {
        btnAdd.IsEnabled = false;
        btnEdit.IsEnabled = false;
        btnDelete.IsEnabled = false;
      }
      if (loadList)
        CaseLoad();
      txtgxID.Text = "";
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Revisa que tecla es precionada y no muestra en la barra de estado de ser necesario
    /// </summary>
    /// <history>
    /// [ecanul] 04/04/2016 Created;
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    } 
    #endregion

    #endregion

  }
}
