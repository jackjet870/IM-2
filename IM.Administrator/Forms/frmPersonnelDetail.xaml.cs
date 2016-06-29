﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;
using IM.Services.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPersonnelDetail.xaml
  /// </summary>
  public partial class frmPersonnelDetail : Window
  {
    #region Variables
    private bool _isCellCancel = false;
    private bool _isClosing = false;
    public Personnel personnel = new Personnel();//Objeto a guardar
    public Personnel oldPersonnel = new Personnel();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana    
    private List<Role> _lstOldRoles = new List<Role>();//Lista de roles iniciales
    private List<PersonnelAccess> _lstOldAccesSalesRoom = new List<PersonnelAccess>();//PersonnelSales
    private List<PersonnelAccess> _lstOldAccesWH = new List<PersonnelAccess>();//PersonnelWH
    private List<PersonnelAccess> _lstOldAccesLeadSource = new List<PersonnelAccess>();//PersonnelLS
    private List<PersonnelPermission> _lstOldPersonnelPermission = new List<PersonnelPermission>();//PersonnelPermission
    #endregion
    public frmPersonnelDetail()
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
    /// [emoguel] created 14/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {      
      ObjectHelper.CopyProperties(personnel, oldPersonnel);
      LoadSalesMen();
      LoadRoles();
      LoadDeps();
      LoadLeadSources();
      LoadLiners();
      LoadPermission();
      LoadPrograms();
      LoadRegions();
      LoadSalesRoom();
      LoadStatus();
      LoadwareHouses();
      LoadLevelPermission();
      LoadPost();
      psbpePwd.Password = personnel.pePwd;
      if (enumMode != EnumMode.preview)
      {
        grdGeneral.IsEnabled = true;
        grdPermission.IsEnabled = true;
        grdPlaces.IsEnabled = true;
        txtpeID.IsEnabled = (enumMode == EnumMode.add);
        UIHelper.SetUpControls(personnel, grdGeneral);        
      }
      if (enumMode != EnumMode.add)
      {
        Title += " (" + personnel.peID + "," + personnel.peN + ")";
      }
      else
      {
        personnel.pePwdDays = 30;
        personnel.peA = true;
        personnel.pePwdD = DateTime.Today;
        personnel.peps = "ACTIVE";
      }
      DataContext = personnel;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] emoguel created 14/06/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key==Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        _isClosing = true;
        btnCancel_Click(null, null);
        if (!_isClosing)
        {
          e.Cancel = true;
        }
        else
        {
          _isClosing = false;
        }
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda los cambios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/06/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<PersonnelAccess> lstWarehousesAcces = (List<PersonnelAccess>)dgrWarehouses.ItemsSource;
      List<PersonnelAccess> lstSalesRoomAcces = (List<PersonnelAccess>)dgrSalesRoom.ItemsSource;
      List<PersonnelAccess> lstLeadSourcesAcces = (List<PersonnelAccess>)dgrLeadSources.ItemsSource;
      List<PersonnelPermission> lstPersonnelPermision = (List<PersonnelPermission>)dgrPermission.ItemsSource;
      List<Role> lstRoles = (List<Role>)dgrRoles.ItemsSource;

      #region PersonnelPermision changes
      var lstPersonnelPermissionAdd = lstPersonnelPermision.Where(pp => !_lstOldPersonnelPermission.Any(ppp => pp.pppe == ppp.pppe && pp.pppm == ppp.pppm)).ToList();
      var lstPersonnelPermissionDel = _lstOldPersonnelPermission.Where(pp => !lstPersonnelPermision.Any(ppp=> pp.pppe == ppp.pppe && pp.pppm == ppp.pppm)).ToList();
      var lstPersonnelPermissionUpd = _lstOldPersonnelPermission.Where(pp => lstPersonnelPermision.Any(ppp => pp.pppe == ppp.pppe && pp.pppm == ppp.pppm && pp.pppl!=ppp.pppl)).ToList();
      #endregion

      if(enumMode!=EnumMode.add && !HasChanged(lstWarehousesAcces,lstSalesRoomAcces,lstLeadSourcesAcces,lstRoles) && lstPersonnelPermissionAdd.Count==0 
        && lstPersonnelPermissionDel.Count==0 && lstPersonnelPermissionUpd.Count==0)
      {
        _isClosing = true;
        Close();
      }
      else
      {
        txtStatus.Text = "Saving Data...";
        skpStatus.Visibility = Visibility.Visible;
        btnAccept.Visibility = Visibility.Hidden;
        string strMsj = ValidateHelper.ValidateForm(tbiGeneral, "Personnel");
        string strValidate = ValidateGeneral();
        if (strValidate!="")
        {
          strMsj += (strMsj != "") ? "\n" : "" + strValidate;
        }

        if(strMsj=="")
        {
          //ROles
          var lstRolesAdd = lstRoles.Where(ro => !_lstOldRoles.Any(roo => roo.roID == ro.roID)).ToList();
          var lstRolesDel = _lstOldRoles.Where(ro => !lstRoles.Any(roo => roo.roID == ro.roID)).ToList();
          //LeadSources
          var lstLeadSourceAdd = lstLeadSourcesAcces.Where(pa => !_lstOldAccesLeadSource.Any(paa=>pa.plLSSRID==paa.plLSSRID)).ToList();
          var lstLeadSourceDel = _lstOldAccesLeadSource.Where(pa => !lstLeadSourcesAcces.Any(paa => pa.plLSSRID == paa.plLSSRID)).ToList();
          //SalesRoom
          var lstSalesRoomAdd = lstSalesRoomAcces.Where(pa => !_lstOldAccesLeadSource.Any(paa => pa.plLSSRID == paa.plLSSRID)).ToList();
          var lstSalesRoomDel = _lstOldAccesLeadSource.Where(pa => !lstSalesRoomAcces.Any(paa => pa.plLSSRID == paa.plLSSRID)).ToList();

          //Warehouses
          var lstWarehousesAdd = lstWarehousesAcces.Where(pa => !_lstOldAccesWH.Any(paa => pa.plLSSRID == paa.plLSSRID)).ToList();
          var lsWarehousesDel = _lstOldAccesWH.Where(pa => !lstWarehousesAcces.Any(paa => pa.plLSSRID == paa.plLSSRID)).ToList();

          //Guardar los registros
          int nRes = await BRPersonnel.SavePersonnel(App.User.User.peID, personnel, (enumMode == EnumMode.edit), lstPersonnelPermissionAdd, lstPersonnelPermissionDel, lstPersonnelPermissionUpd,
            lstLeadSourceDel, lstLeadSourceAdd, lsWarehousesDel, lstWarehousesAdd, lstSalesRoomDel, lstSalesRoomAdd, lstRolesDel, lstRolesAdd, (personnel.pepo != oldPersonnel.pepo), 
            (personnel.peTeamType!=oldPersonnel.peTeamType || personnel.pePlaceID!=oldPersonnel.pePlaceID || personnel.peTeam!=oldPersonnel.peTeam));
          UIHelper.ShowMessageResult("Personnel", nRes);
          if(nRes>0)
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
        skpStatus.Visibility = Visibility.Collapsed;
        btnAccept.Visibility = Visibility.Visible;
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
    /// [emoguel] created 14/06/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();      

      #region PersonnelPermision changes
      List<PersonnelAccess> lstWarehousesAcces = (List<PersonnelAccess>)dgrWarehouses.ItemsSource;
      List<PersonnelAccess> lstSalesRoomAcces = (List<PersonnelAccess>)dgrSalesRoom.ItemsSource;
      List<PersonnelAccess> lstLeadSourcesAcces = (List<PersonnelAccess>)dgrLeadSources.ItemsSource;
      List<PersonnelPermission> lstPersonnelPermision = (List<PersonnelPermission>)dgrPermission.ItemsSource;
      List<Role> lstRoles = (List<Role>)dgrRoles.ItemsSource;
      var lstPersonnelPermissionAdd = (lstPersonnelPermision != null)? lstPersonnelPermision.Where(pp => !_lstOldPersonnelPermission.Any(ppp => pp.pppe == ppp.pppe && pp.pppm == ppp.pppm)).ToList() : new List<PersonnelPermission>();
      var lstPersonnelPermissionDel = _lstOldPersonnelPermission.Where(pp => !lstPersonnelPermision.Any(ppp => pp.pppe == ppp.pppe && pp.pppm == ppp.pppm)).ToList();
      var lstPersonnelPermissionUpd = _lstOldPersonnelPermission.Where(pp => lstPersonnelPermision.Any(ppp => pp.pppe == ppp.pppe && pp.pppm == ppp.pppm && pp.pppl != ppp.pppl)).ToList();
      #endregion
      if (enumMode!= EnumMode.preview && ( !ObjectHelper.IsEquals(personnel,oldPersonnel) || HasChanged(lstWarehousesAcces, lstSalesRoomAcces, lstLeadSourcesAcces, lstRoles) || lstPersonnelPermissionAdd.Count > 0
        || lstPersonnelPermissionDel.Count > 0 || lstPersonnelPermissionUpd.Count > 0))
      {
        MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
        if (result == MessageBoxResult.Yes)
        {
          if (!_isClosing) { _isClosing = true; Close(); }
        }
        else
        {
          _isClosing = false;
        }
      }
      else
      {
        if (!_isClosing) { _isClosing = true; Close(); }
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda de IDColaborador
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/06/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearchCollaborator frmSearchCollaborator = new frmSearchCollaborator();
      frmSearchCollaborator.Owner = this;
      if (frmSearchCollaborator.ShowDialog() == true)
      {
        personnel.peCollaboratorID = frmSearchCollaborator.idCollaborator;
      }
    }
    #endregion

    #region Permission_Click
    /// <summary>
    /// Asigna permisos predefinidos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/06/2016
    /// </history>
    private void btnPermission_Click(object sender, RoutedEventArgs e)
    {
      List<PersonnelPermission> lstNewPersonnelPermision = new List<PersonnelPermission>();

      if (hasRoles("PR,PRMEMBER"))
      {
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Register), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Available), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.PRInvitations), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Equity), (int)EnumPermisionLevel.ReadOnly);
      }

      if (hasRoles("PRSUPER,PRCAPT"))
      {
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Register), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Available), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.PRInvitations), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Equity), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Assignment), (int)EnumPermisionLevel.Standard);

      }

      if (hasRoles("LINER,LINERCAPT,CLOSER,CLOSERCAPT,EXIT,PODIUM,VLO"))
      {
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Host), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Show), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Sales), (int)EnumPermisionLevel.ReadOnly);
      }

      if (hasRoles("HOSTENTRY,HOSTGIFTS,HOSTEXIT"))
      {
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Register), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.PRInvitations), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Host), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.HostInvitations), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Show), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.MealTicket), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.GiftsReceipts), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Sales), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.TaxiIn), (int)EnumPermisionLevel.Standard);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.CloseSalesRoom), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.CxCAuthorization), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.ExchangeRates), (int)EnumPermisionLevel.ReadOnly);
      }

      if (hasRoles("MANAGER"))
      {
        //Marketing
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Register), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.PRInvitations), (int)EnumPermisionLevel.ReadOnly);
        //host
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Host), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Show), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.MealTicket), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.GiftsReceipts), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Sales), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.CloseSalesRoom), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.CxCAuthorization), (int)EnumPermisionLevel.ReadOnly);
      }

      if (hasRoles("ADMIN"))
      {
        //Marketing
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Register), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Available), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.PRInvitations), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Equity), (int)EnumPermisionLevel.ReadOnly);

        //Asignacion
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Assignment), (int)EnumPermisionLevel.ReadOnly);

        //Mail outs
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.MailOutsTexts), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.MailOutsConfig), (int)EnumPermisionLevel.ReadOnly);

        //Host
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Host), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.HostInvitations), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Show), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.MealTicket), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.GiftsReceipts), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Sales), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.TaxiIn), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.CloseSalesRoom), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.CxCAuthorization), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.ExchangeRates), (int)EnumPermisionLevel.ReadOnly);

        //Catalogos
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Agencies), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Contracts), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Currencies), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.FolioInvitationsOuthouse), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.FolioCXC), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Gifts), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Languages), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Locations), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.MaritalStatus), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Motives), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Personnel), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Teams), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.TourTimes), (int)EnumPermisionLevel.Special);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Warehouses), (int)EnumPermisionLevel.Special);
      }

      if (hasRoles("SECRETARY"))
      {
        //Marketing
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Register), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Available), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.PRInvitations), (int)EnumPermisionLevel.ReadOnly);

        //Host
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Host), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Show), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.MealTicket), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.GiftsReceipts), (int)EnumPermisionLevel.ReadOnly);
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Sales), (int)EnumPermisionLevel.ReadOnly);

        //Catalogos
        AddPermision(lstNewPersonnelPermision, EnumToListHelper.GetEnumDescription(EnumPermission.Teams), (int)EnumPermisionLevel.Standard);
      }

      dgrPermission.ItemsSource = lstNewPersonnelPermision;
    }
    #endregion

    #region EndCellEdit
    /// <summary>
    /// Verifica que no se repita un registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/06/2016
    /// </history>
    private void CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))
      {
        _isCellCancel = false;
        DataGrid dgrEdit = sender as DataGrid;
        bool blnIsRepeat = false;
        switch (e.Column.SortMemberPath)
        {
          case "pppm":
            {
              blnIsRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrEdit, false, "pppm");
              PersonnelPermission personnelPermission = (PersonnelPermission)dgrPermission.SelectedItem;
              personnelPermission.pppl = 1;
              break;
            }
          case "plLSSRID":
            {
              blnIsRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrEdit,strPropGrid: "plLSSRID",typeName:e.Column.Header.ToString());
              break;
            }
          default:
            {
              if (e.Column.SortMemberPath != "pppl")
              {
                blnIsRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrEdit);
              }
              break;
            }
        }
        e.Cancel = blnIsRepeat;
      }
      else
      {
        _isCellCancel = true;
      }
    }
    #endregion

    #region EndRowEdit
    /// <summary>
    /// Verifica que no se agreguen filas vacias
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/06/2016
    /// </history>
    private void RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      DataGrid dgr = sender as DataGrid;
      dgr.RowEditEnding -= RowEditEnding;
      if (_isCellCancel)
      {
        dgr.CancelEdit();
      }
      else
      {        
        if (dgr.Name== "dgrPermission" && dgr.CurrentColumn.SortMemberPath == "pppm")
        {
          dgr.CommitEdit();
          dgr.Items.Refresh();
        }
      }
      dgr.RowEditEnding += RowEditEnding;
    }
    #endregion

    #region Begin Edit
    /// <summary>
    /// Permite editar unicamente cuando un el permiso ha sido asignado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/06/2016
    /// </history>
    private void dgrPermission_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (e.Column.SortMemberPath == "pppl")
      {
        PersonnelPermission personelPermission = (PersonnelPermission)e.Row.Item;
        e.Cancel = (string.IsNullOrWhiteSpace(personelPermission.pppm));
      }
    }
    #endregion

    #region ChekedStatus
    /// <summary>
    /// cheked Combobox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/06/2016
    /// </history>
    private void chkpeA_CheckedChange(object sender, RoutedEventArgs e)
    {
      if (chkpeA.IsChecked == true)
      {
        cmbpeps.SelectedValue = "ACTIVE";
      }
      else
      {
        cmbpeps.SelectedValue = "INACTIVE";
      }
    }
    #endregion

    #region cmbpePlaceID_SelectionChanged
    /// <summary>
    /// Carga el combobox de temas
    /// dependiendo del place seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/06/2016
    /// </history>
    private void cmbpePlaceID_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      switch (personnel.pepo)
      {
        case "GS":
        case "OPC":
          {
            List<TeamGuestServices> lstTeamguestService = BRTeamsGuestServices.GetTeamsGuestServices(1, new TeamGuestServices { tglo = personnel.pePlaceID });
            cmbpeTeamType.ItemsSource = lstTeamguestService;
            cmbpeTeamType.SelectedValuePath = "tgID";
            cmbpeTeamType.DisplayMemberPath = "tgN";
            break;
          }
        case "FTM":
        case "LINER":
        case "CLOSER":
        case "FTB":
        case "EXIT":
        case "REGEN":
        case "VLO":
        case "ASM":
          {
            List<TeamSalesmen> lstTeamSalesMen = BRTeamsSalesMen.GetTeamsSalesMen(1, new TeamSalesmen { tssr = personnel.pePlaceID });
            cmbpeTeamType.ItemsSource = lstTeamSalesMen;
            cmbpeTeamType.SelectedValuePath = "tsID";
            cmbpeTeamType.DisplayMemberPath = "tsN";
            break;
          }
        default:
          {
            if (cmbpeTeamType.Items.Count > 0)
            {
              cmbpeTeamType.Items.Clear();
            }
            break;
          }
      }
      
    }
    #endregion

    #region cmbpepo_SelectionChanged
    /// <summary>
    /// Carga las locaciones dependiendo del post seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/06/2016
    /// </history>
    private async void cmbpepo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      Post post = (e.RemovedItems.Count > 0) ? (Post)e.RemovedItems[0] :new Post();
      switch (personnel.pepo)
      {
        case "GS":
        case "OPC":
          {
            if (!string.IsNullOrWhiteSpace(post.poID) || (post.poID != "GS" && post.poID != "OPC"))
            {
              List<object> lstLocations = await BRLocations.GetLocationByTeamGuestService();
              cmbpePlaceID.ItemsSource = lstLocations;
              cmbpePlaceID.SelectedValuePath = "loID";
              cmbpePlaceID.DisplayMemberPath = "loN";
            }
            txtLocSal.Text = "Location";
            cmbpeLinerID.IsEnabled = true;
            break;
          }
        case "FTM":
        case "LINER":
        case "CLOSER":
        case "FTB":
        case "EXIT":
        case "REGEN":
        case "VLO":
        case "ASM":
          {
            if (!string.IsNullOrWhiteSpace(post.poID)  || (post.poID != "FTM" && post.poID != "LINER" && post.poID != "CLOSER" && post.poID != "FTB"
              && post.poID != "EXIT" && post.poID  != "REGEN" && post.poID != "VLO" && post.poID != "ASM"))
            {
              List<object> lstSalesRoom = await BRSalesRooms.GetSalesRoombyTeamSalesMen();              
              cmbpePlaceID.ItemsSource = lstSalesRoom;
              cmbpePlaceID.SelectedValuePath = "srID";
              cmbpePlaceID.DisplayMemberPath = "srN";
            }
            cmbpeLinerID.IsEnabled = false;
            txtLocSal.Text = "Sales Room";
            break;
          }
        default:
          {
            txtLocSal.Text = "Place ID";
            if (cmbpePlaceID.Items.Count > 0)
            {
              cmbpePlaceID.ItemsSource = null;
              personnel.peTeamType = "";
            }
            cmbpeLinerID.IsEnabled = false;
            break;
          }
      }
      SetRoles(personnel.pepo);
    }
    #endregion

    #region LeadSources_Click
    /// <summary>
    /// Asigna los permisos para leadSources
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/06/2016
    /// </history>
    private void btnLeadSources_Click(object sender, RoutedEventArgs e)
    {
      string strMsj = "";

      if (dgrRegion.SelectedItems.Count == 0)
      {
        strMsj += "Specify at least one Region. \n";
      }
      if (dgrPrograms.SelectedItems.Count == 0)
      {
        strMsj += "Specify at least one Program.";
      }

      if (strMsj == "")
      {
        List<LeadSourceByUser> lstLeadSourceByUser = (List<LeadSourceByUser>)cmbLeadSource.ItemsSource;
        List<string> lstPrograms = dgrPrograms.SelectedItems.OfType<Program>().Select(pg => pg.pgID).ToList();
        List<string> lstRegions = dgrRegion.SelectedItems.OfType<Region>().Select(rg => rg.rgID).ToList();
        var LeadSourceAsign = lstLeadSourceByUser.Where(lsb => lstPrograms.Contains(lsb.lspg) && lstRegions.Contains(lsb.lsrg)).ToList();        
        List<PersonnelAccess> lstLeadSources = LeadSourceAsign.Select(ls => new PersonnelAccess { plLSSRID = ls.lsID }).ToList();
        dgrLeadSources.ItemsSource = lstLeadSources;
      }
      else
      {
        UIHelper.ShowMessage(strMsj.TrimEnd('\n'));
      }
    }
    #endregion

    #region SalesRoom clic
    /// <summary>
    /// Asigna los permisos para SalesRoom
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/06/2016
    /// </history>
    private void btnSalesRoom_Click(object sender, RoutedEventArgs e)
    {

      if (dgrRegion.Items.Count == 0)
      {
        UIHelper.ShowMessage("Specify at least one Region");
      }
      else
      {
        List<string> lstRegions = dgrRegion.SelectedItems.OfType<Region>().Select(rg => rg.rgID).ToList();
        List<SalesRoomByUser> lstSalesRoomByUser = (List<SalesRoomByUser>)cmbSalesRoom.ItemsSource;
        var lstSalesRoomAssign = lstSalesRoomByUser.Where(sr => lstRegions.Contains(sr.arrg)).ToList();

        List<PersonnelAccess> lstSalesRoom = lstSalesRoomAssign.Select(sr => new PersonnelAccess { plLSSRID=sr.srID}).ToList();
        
        dgrSalesRoom.ItemsSource = lstSalesRoom;
      }
    }
    #endregion

    #region Warehouses
    /// <summary>
    /// Asigna los leadSources
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/06/2016
    /// </history>
    private void btnWarehouses_Click(object sender, RoutedEventArgs e)
    {
      if (dgrRegion.Items.Count == 0)
      {
        UIHelper.ShowMessage("Specify at least one Region");
      }
      else
      {
        List<string> lstRegions = dgrRegion.SelectedItems.OfType<Region>().Select(rg => rg.rgID).ToList();
        List<WarehouseByUser> lstWarehousesByUser = (List<WarehouseByUser>)cmbWarehouses.ItemsSource;
        var lstWarehousesAsign = lstWarehousesByUser.Where(wh => lstRegions.Contains(wh.arrg)).ToList();
        List<PersonnelAccess> lstWarehouses = lstWarehousesAsign.Select(wh => new PersonnelAccess { plLSSRID = wh.whID }).ToList();
        dgrWarehouses.ItemsSource = lstWarehouses;
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadStatus
    /// <summary>
    /// Llena el combobox de status
    /// </summary>
    /// <history>
    /// [emoguel] created 14/06/2016
    /// </history>
    private async void LoadStatus()
    {
      try
      {
        cmbpeps.ItemsSource = await BRPersonnelStatus.getPersonnelStatus();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region LoadDeps
    /// <summary>
    /// LLena el grid de depts
    /// </summary>
    /// <history>
    /// [emoguel] created 14/06/2016
    /// </history>
    private async void LoadDeps()
    {
      try
      {
        cmbpede.ItemsSource = await BRDepts.GetDepts(1);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region LoadPosts
    /// <summary>
    /// Llena el combobox el post
    /// </summary>
    /// <history>
    /// [emoguel] created 15/06/2016
    /// </history>
    private async void LoadPost()
    {
      try
      {
        cmbpepo.ItemsSource = await BRPosts.GetPosts(1);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region LoadLiners
    /// <summary>
    /// Llena el combobox de liners
    /// </summary>
    /// <history>
    /// [emoguel] created 15/06/2016
    /// </history>
    private async void LoadLiners()
    {
      try
      {
        cmbpeLinerID.ItemsSource = await BRPersonnel.GetPersonnel(roles: EnumToListHelper.GetEnumDescription(EnumRole.Liner));
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region LoadSalesMen
    /// <summary>
    /// Llena el combo de vendedores
    /// </summary>
    private async void LoadSalesMen()
    {
      try
      {
        var salesMen = await ClubesHelper.GetSalesMen();
        var lstSalessalesMen = salesMen.ToList();
        cmbpeSalesManID.ItemsSource = lstSalessalesMen;
        if(enumMode!=EnumMode.preview)
        {
          btnAccept.Visibility = Visibility.Visible;
        }
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }     

    }
    #endregion

    #region LoadLeadSources
    /// <summary>
    /// Llena el grid de LeadSources del usuario autentificado
    /// </summary>
    /// <history>
    /// [emoguel] created 15/06/2016
    /// </history>
    private async void LoadLeadSources()
    {
      try
      {
        List<LeadSourceByUser> lstLeadSource = await BRLeadSources.GetLeadSourcesByUser(App.User.User.peID.ToString());
        cmbLeadSource.ItemsSource = lstLeadSource;

        if (enumMode != EnumMode.add)
        {
          _lstOldAccesLeadSource = await BRPersonnelAcces.getPersonnelAcces(personnel.peID, EnumPlaceType.LeadSource);          
        }

        dgrLeadSources.ItemsSource = _lstOldAccesLeadSource.ToList();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region SalesRoom
    /// <summary>
    /// Llena el combobox de salesRoom
    /// </summary>
    /// <history>
    /// [emoguel] created 15/06/2016
    /// </history>
    private async void LoadSalesRoom()
    {
      try
      {
        List<SalesRoomByUser> lstSalesRoom = await BRSalesRooms.GetSalesRoomsByUser(App.User.User.peID);
        cmbSalesRoom.ItemsSource = lstSalesRoom;

        if (enumMode != EnumMode.add)
        {
          _lstOldAccesSalesRoom = await BRPersonnelAcces.getPersonnelAcces(personnel.peID, EnumPlaceType.SalesRoom);                    
        }
        dgrSalesRoom.ItemsSource = _lstOldAccesSalesRoom.ToList();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region WareHouses
    /// <summary>
    /// Llena el combobox de warehouses
    /// </summary>
    /// <history>
    /// [emoguel] created 15/06/2016
    /// </history>
    private async void LoadwareHouses()
    {
      try
      {
        List<WarehouseByUser> lstWarehouse = await BRWarehouses.GetWarehousesByUser(App.User.User.peID.ToString());
        cmbWarehouses.ItemsSource = lstWarehouse;

        if (enumMode != EnumMode.add)
        {
          _lstOldAccesWH = await BRPersonnelAcces.getPersonnelAcces(personnel.peID, EnumPlaceType.Warehouse);                    
        }
        dgrWarehouses.ItemsSource = _lstOldAccesWH.ToList();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region Roles
    /// <summary>
    /// Llena el grid de Roles
    /// </summary>
    /// <hitory>
    /// [emoguel] created 15/06/2016
    /// </hitory>
    private async void LoadRoles()
    {
      try
      {
        List<Role> lstRoles = await BRRoles.GetRoles();
        cmbRoles.ItemsSource = lstRoles;

        if (enumMode != EnumMode.add)
        {
          _lstOldRoles = await BRRoles.GetRolesByUser(personnel.peID);          
        }
        dgrRoles.ItemsSource = _lstOldRoles.ToList();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region Permission
    /// <summary>
    /// Llena el combobox de permission
    /// </summary>
    /// <history>
    /// [emoguel] created 15/06/2016
    /// </history>
    private async void LoadPermission()
    {
      try
      {
        cmbPermission.ItemsSource = await BRPermissions.GetPermissions(1);

        if (enumMode != EnumMode.add)
        {
          _lstOldPersonnelPermission = await BRPermissions.GetPersonnelPermission(personnel.peID);          
        }
        dgrPermission.ItemsSource = _lstOldPersonnelPermission.ToList();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region LoadLevelPermission
    /// <summary>
    /// Carga los levelPermission
    /// </summary>
    /// <history>
    /// [emoguel] created 16/06/2016
    /// </history>
    private async void LoadLevelPermission()
    {
      try
      {
        cmbLevelPermission.ItemsSource = await BRPermissionsLevels.GetPermissionsLevels(1);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region Programs
    /// <summary>
    /// Llena el grid de programs
    /// </summary>
    /// <history>
    /// [emoguel] created 16/06/2016
    /// </history>
    private async void LoadPrograms()
    {
      try
      {
        dgrPrograms.ItemsSource = await BRPrograms.GetPrograms();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region LoadRegions
    /// <summary>
    /// Llena el grid de Regions
    /// </summary>
    /// <history>
    /// [emoguel] created 16/06/2016
    /// </history>
    private async void LoadRegions()
    {
      try
      {
        dgrRegion.ItemsSource = await BRRegions.GetRegions(1);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion    

    #region hasRoles
    /// <summary>
    /// Determina si se tiene asignado alguno de los roles
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    private bool hasRoles(string roles)
    {
      List<string> lstPredRoles = roles.Split(',').ToList();
      List<Role> lstRoles = (List<Role>)dgrRoles.ItemsSource;

      List<Role> hasRoles = lstRoles.Where(ro => lstPredRoles.Contains(ro.roID)).ToList();
      return (hasRoles.Count > 0);
    }
    #endregion

    #region ListAddPermission
    /// <summary>
    /// Agrega un permision dependiendo
    /// </summary>
    /// <param name="lstPersmision">Lista a agregar Permiso</param>
    /// <param name="permision">permiso a  agregar</param>
    /// <param name="level">nivel del permiso</param>
    /// <history>
    /// [emoguel] created 17/06/2016
    /// </history>
    private void AddPermision(List<PersonnelPermission> lstPersmision, string permision, int level)
    {
      PersonnelPermission personnelPermision = lstPersmision.Where(pp => pp.pppm == permision).FirstOrDefault();
      if (personnelPermision == null)
      {
        lstPersmision.Add(new PersonnelPermission { pppm = permision, pppl = level });
      }
      else
      {
        personnelPermision.pppl = level;
      }
    }
    #endregion

    #region ValidateGeneral
    /// <summary>
    /// Valida campos dependiendo de las opciones seleccionadas
    /// </summary>
    /// <returns> =="". campos completos | !="". Campos incompletos </returns>
    /// <history>
    /// [emoguel] created 17/06/2016
    /// </history>
    private string ValidateGeneral()
    {
      string strMsj = "";

      //Validamos el puesto
      //Si el departamento es GuestServices, OPC's Outhouse, Vendedores, Verificadores legales o encargados de Socios morosos
      List<string> lstdepts = new List<string> { "GS", "OPC", "SALES", "VLO", "ASM" };
      if (cmbpede.SelectedValue != null && lstdepts.Contains(cmbpede.SelectedValue.ToString()))
      {
        if (cmbpepo.SelectedValue == null)
        {
          strMsj += "specify the post Personnel. \n";
        }
      }

      //Validamos el lugar
      //si es del departamento de Guest Services, OPC's Outhouse, vendedores, verificadores legales o Encargados de socios morosos
      List<string> lstPost = new List<string> { "GS", "OPC", "FTM", "LINER", "CLOSER", "FTB", "EXIT", "REGEN", "VLO", "ASM" };
      if (cmbpepo.SelectedValue != null && lstPost.Contains(cmbpepo.SelectedValue.ToString()))
      {
        if (cmbpePlaceID.SelectedValue == null)
        {
          strMsj += "Specify the  place Personnel. \n";
        }
      }

      //Validamos que se tenga seleccionado un equipo si se tiene seleccionado un lugar
      if (cmbpePlaceID.SelectedValue != null)
      {
        if (cmbpeTeamType.SelectedValue == null)
        {
          strMsj += "Specify the team Personnel. \n";
        }
      }

      //Validamos la clave del vendedor en intelligense contract
      //si tiene puesto de Guest Service, OPC Outhouse, vendedor o verificador legal
      List<string> lstPostLiner = new List<string> { "GS", "OPC", "FTM", "LINER", "CLOSER", "FTB", "EXIT", "REGEN", "VLO" };
      if (cmbpepo.SelectedValue != null && lstPostLiner.Contains(cmbpepo.SelectedValue.ToString()))
      {
        if (cmbpeSalesManID.SelectedValue != null)
        {
          strMsj += "Specify the team Personnel. \n";
        }
      }

      //Si es un usuario activo y no se capturo ningunu LeadSource o Sala
      if (chkpeA.IsChecked == true)
      {
        if (dgrSalesRoom.Items.Count == 0 && dgrLeadSources.Items.Count == 0)
        {
          strMsj += "Specify at least one Lead Source or Sales Room.";
        }
      }

      return strMsj.TrimEnd('\n');
    }
    #endregion

    #region SetRoles
    /// <summary>
    /// Carga los roles predeterminados
    /// </summary>
    /// <history>
    /// [emoguel] created 20/06/2016
    /// </history>
    private void SetRoles(string post)
    {
      List<Role> lstRoles = (List<Role>)dgrRoles.ItemsSource;
      List<Role> lstAllRole = (List<Role>)cmbRoles.ItemsSource;
      switch (post)
      {
        #region GuestService
        case "GS":
        case "OPC":
          {
            string role = EnumToListHelper.GetEnumDescription(EnumRole.PR);
            if (lstRoles.Where(ro => ro.roID == role).FirstOrDefault() == null)
            {
              Role rolePR = lstAllRole.Where(ro => ro.roID == role).FirstOrDefault();
              if (rolePR != null)
              {
                lstRoles.Add(rolePR);
              }
            }
            break;
          }
        #endregion
        #region SalesMen
        case "FTM":
        case "LINER":
        case "CLOSER":
        case "FTB":
        case "EXIT":
        case "REGEN":
          {
            #region Liner
            string roleL = EnumToListHelper.GetEnumDescription(EnumRole.Liner);
            if (lstRoles.Where(ro => ro.roID == roleL).FirstOrDefault() == null)
            {
              Role roleLiner = lstAllRole.Where(ro => ro.roID == roleL).FirstOrDefault();
              if (roleLiner != null)
              {
                lstRoles.Add(roleLiner);
              }
            }
            #endregion
            #region Closer
            string roleC = EnumToListHelper.GetEnumDescription(EnumRole.Closer);
            if (lstRoles.Where(ro => ro.roID == roleC).FirstOrDefault() == null)
            {
              Role roleCLoser = lstAllRole.Where(ro => ro.roID == roleC).FirstOrDefault();
              if (roleCLoser != null)
              {
                lstRoles.Add(roleCLoser);
              }
            }
            #endregion
            #region ExitCloser
            string roleEC = EnumToListHelper.GetEnumDescription(EnumRole.ExitCloser);
            if (lstRoles.Where(ro => ro.roID == roleEC).FirstOrDefault() == null)
            {
              Role roleExitCloser = lstAllRole.Where(ro => ro.roID == roleEC).FirstOrDefault();
              if (roleExitCloser != null)
              {
                lstRoles.Add(roleExitCloser);
              }
            }
            #endregion
            break;
          }
        #endregion
        #region VLO
        case "VLO":
          {
            string role = EnumToListHelper.GetEnumDescription(EnumRole.VLO);
            if (lstRoles.Where(ro => ro.roID == role).FirstOrDefault() == null)
            {
              Role roleVLO = lstAllRole.Where(ro => ro.roID == role).FirstOrDefault();
              if (roleVLO != null)
              {
                lstRoles.Add(roleVLO);
              }
            }
            break;
          }
          #endregion
      }
    }

    #endregion

    #region HasChanges
    /// <summary>
    /// Valida si hay cambios pendientes en la ventana
    /// </summary>
    ///<param name="lstWarehousesAcces">lista de warehouses nuevos</param>
    ///<param name="lstSalesRoomAcces">Lista de salesroom nuevos</param>
    ///<param name="lstLeadSourcesAcces">Lista de LeadSources nuevos</param>
    ///<param name="lstRoles"> Lista de roles nuevos</param>
    /// <returns>True. Hay cambios pendientes | False. No tiene cambios pendientes</returns>
    /// <history>
    /// [emoguel] created 23/06/2016
    /// </history>
    private bool HasChanged(List<PersonnelAccess> lstWarehousesAcces, List<PersonnelAccess> lstSalesRoomAcces, List<PersonnelAccess> lstLeadSourcesAcces, List<Role> lstRoles)
    {
      if(!ObjectHelper.IsEquals(personnel,oldPersonnel))
      {
        return true;
      }

      if(!ObjectHelper.IsListEquals(lstWarehousesAcces,_lstOldAccesWH))
      {
        return true;
      }

      if(!ObjectHelper.IsListEquals(lstSalesRoomAcces,_lstOldAccesSalesRoom))
      {
        return true;
      }

      if(!ObjectHelper.IsListEquals(lstLeadSourcesAcces,_lstOldAccesLeadSource))
      {
        return true;
      }

      if(!ObjectHelper.IsListEquals(lstRoles,_lstOldRoles))
      {
        return true;
      }

      return false;
    } 
    #endregion
    #endregion

  }
}
