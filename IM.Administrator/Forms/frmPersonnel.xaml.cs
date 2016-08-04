using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPersonnel.xaml
  /// </summary>
  public partial class frmPersonnel : Window
  {
    #region Variables    
    private string _leadSource = "ALL";
    private string _salesRoom = "ALL";
    private string _roles = "ALL";
    private int _status = 0;
    private string _permission = "ALL";
    private string _relationalOperator = "=";
    private string _dept = "ALL";
    private EnumPermisionLevel _enumPermission = EnumPermisionLevel.None;
    private bool _blnDel = false;
    private bool _blnEdit = false;
    #endregion

    public frmPersonnel()
    {
      InitializeComponent();
    }

    #region Methods form
    #region Load
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnDel = App.User.HasPermission(EnumPermission.Personnel, EnumPermisionLevel.Special);
      _blnEdit = App.User.HasPermission(EnumPermission.Personnel, EnumPermisionLevel.Standard);
      btnDel.IsEnabled = _blnDel;
      btnPostLog.IsEnabled = App.User.HasPermission(EnumPermission.Teams, EnumPermisionLevel.ReadOnly);
      btnTeamLog.IsEnabled = App.User.HasPermission(EnumPermission.Teams, EnumPermisionLevel.ReadOnly);
      btnChangeID.IsEnabled = (App.User.HasRole(EnumRole.Administrator) && App.User.HasPermission(EnumPermission.Personnel,EnumPermisionLevel.SuperSpecial));
      LoadPersonnel();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window keyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
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

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 10/06/2016
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

    #region DobleClic Grid
    /// <summary>
    /// Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private async void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      PersonnelShort personnelShort = (PersonnelShort)dgrPersonnels.SelectedItem;
      Personnel personnel = BRPersonnel.GetPersonnelById(personnelShort.peID);
      frmPersonnelDetail frmPersonnelDetail=new frmPersonnelDetail();
      frmPersonnelDetail.Owner = this;
      frmPersonnelDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.preview;
      frmPersonnelDetail.oldPersonnel = personnel;
      if(frmPersonnelDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<PersonnelShort> lstPersonnel = (List<PersonnelShort>)dgrPersonnels.ItemsSource;
        var persons = await BRPersonnel.GetPersonnel(idPersonnel: frmPersonnelDetail.personnel.peID);
        
        if(persons.Count>0)
        {
          PersonnelShort person = persons.FirstOrDefault();
          ObjectHelper.CopyProperties(personnelShort, person);//Actualizamos los datos
          lstPersonnel.Sort((x, y) => string.Compare(x.peN, y.peN));//Ordenamos la lista
          nIndex = lstPersonnel.IndexOf(personnelShort);//Obtenemos la posición del registro
        }
        else
        {
          lstPersonnel.Remove(personnelShort);//Quitamos el registro
        }
        btnDel.IsEnabled = (lstPersonnel.Count > 0) ? _blnDel : false;
        dgrPersonnels.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrPersonnels, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstPersonnel.Count + " Personnels.";//Actualizamos el contador
      }
    }
    #endregion

    #region refresh
    /// <summary>
    /// Recarga los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      PersonnelShort personnel = (PersonnelShort)dgrPersonnels.SelectedItem;
      LoadPersonnel(personnel);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private async void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmPersonnelDetail frmPersonnelDetail = new frmPersonnelDetail();
      frmPersonnelDetail.Owner = this;
      frmPersonnelDetail.enumMode = EnumMode.add;
      if (frmPersonnelDetail.ShowDialog() == true)
      { 
        var persons = await BRPersonnel.GetPersonnel(idPersonnel: frmPersonnelDetail.personnel.peID);

        if (persons.Count > 0)
        {
          List<PersonnelShort> lstPersonnel = (List<PersonnelShort>)dgrPersonnels.ItemsSource;
          PersonnelShort person = persons.FirstOrDefault();
          lstPersonnel.Add(person);//Agregamos el registro
          lstPersonnel.Sort((x, y) => string.Compare(x.peN, y.peN));//Ordenamos la lista
          int nIndex = lstPersonnel.IndexOf(person);//Obtenemos la posición del registro
          btnDel.IsEnabled = (lstPersonnel.Count > 0) ? _blnDel : false;
          dgrPersonnels.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPersonnels, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstPersonnel.Count + " Personnels.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearchPersonnel frmSearchPersonnel = new frmSearchPersonnel();
      frmSearchPersonnel.Owner = this;
      frmSearchPersonnel.leadSource = _leadSource;
      frmSearchPersonnel.salesRoom = _salesRoom;
      frmSearchPersonnel.roles = _roles;
      frmSearchPersonnel.status = _status;
      frmSearchPersonnel.permission = _permission;
      frmSearchPersonnel.relationalOperator = _relationalOperator;
      frmSearchPersonnel.dept = _dept;
      frmSearchPersonnel.enumPermission = _enumPermission;
      if(frmSearchPersonnel.ShowDialog()==true)
      {
        _leadSource = frmSearchPersonnel.leadSource;
        _salesRoom = frmSearchPersonnel.salesRoom;
        _roles = frmSearchPersonnel.roles;
        _status = frmSearchPersonnel.status;
        _permission = frmSearchPersonnel.permission;
        _relationalOperator = frmSearchPersonnel.relationalOperator;
        _dept = frmSearchPersonnel.dept;
        _enumPermission = frmSearchPersonnel.enumPermission;
        LoadPersonnel();
      }
  }
    #endregion

    #region Delete
    /// <summary>
    /// Elimina los registros seleccionados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private async void btnDel_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Cursor = Cursors.Wait;
        if (dgrPersonnels.SelectedItems.Count>0)
        {          
          txtStatus.Text = "Deleting Data";
          status.Visibility = Visibility.Visible;
          List<PersonnelShort> lstPersonnelsDel=dgrPersonnels.SelectedItems.OfType<PersonnelShort>().ToList();

          MessageBoxResult msgResult = MessageBoxResult.No;
          #region MessageBox
          if (lstPersonnelsDel.Count == 1)
          {
            msgResult = UIHelper.ShowMessage("Are you sure you want to delete this Person?", MessageBoxImage.Question, "Delete");
          }
          else
          {
            msgResult = UIHelper.ShowMessage("Are you sure you want to delete these Persons", MessageBoxImage.Question, "Delete");
          }
          #endregion

          if (msgResult == MessageBoxResult.Yes)
          {
            int nRes =await BRPersonnel.DeletePersonnels(lstPersonnelsDel);

            if (nRes > 0)
            {
              if (nRes == 1)
              {
                UIHelper.ShowMessage("Person was Deleted.");
              }
              else
              {
                UIHelper.ShowMessage("Person Log were Deleted.");
              }

              List<PersonnelShort> lstPersonnel = (List<PersonnelShort>)dgrPersonnels.ItemsSource;
              lstPersonnel.RemoveAll(pe => lstPersonnelsDel.Contains(pe));
              dgrPersonnels.Items.Refresh();
            }
          }
          status.Visibility = Visibility.Collapsed;

        }
        else
        {
          UIHelper.ShowMessage("Please select a Person.");
        }
        Cursor = Cursors.Arrow;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
        Cursor = Cursors.Arrow;
      }
    }
    #endregion

    #region TeamLog
    /// <summary>
    /// Abre la ventana de TeamsLog
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private void btnTeamLo_Click(object sender, RoutedEventArgs e)
    {
      PersonnelShort personnel = (PersonnelShort)dgrPersonnels.SelectedItem;
      frmTeamsLog frmTeamsLog = new frmTeamsLog();
      frmTeamsLog.Owner = this;
      frmTeamsLog._teamLogFilter = new TeamLog { tlpe = personnel.peID };
      frmTeamsLog.btnSearch.Visibility = Visibility.Collapsed;
      frmTeamsLog.ShowDialog();
    }
    #endregion

    #region PostLog
    /// <summary>
    /// Abre la ventana de PostLog
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private void btnPostLog_Click(object sender, RoutedEventArgs e)
    {
      PersonnelShort personnel = (PersonnelShort)dgrPersonnels.SelectedItem;
      frmPostsLog frmPostLog = new frmPostsLog();
      frmPostLog.Owner = this;
      frmPostLog._postLogFilter = new PostLog { pppe = personnel.peID };
      frmPostLog.btnSearch.Visibility = Visibility.Collapsed;
      frmPostLog.ShowDialog();
    }
    #endregion

    #region ChangeID
    /// <summary>
    /// Abre la ventana de changeID
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private void btnChangeID_Click(object sender, RoutedEventArgs e)
    {
      frmPersonnelChangeID frmPersonnelChangeID = new frmPersonnelChangeID();
      frmPersonnelChangeID.Owner = this;
      if(dgrPersonnels.SelectedItem!=null)
      {
        PersonnelShort personnelShort = (PersonnelShort)dgrPersonnels.SelectedItem;
        frmPersonnelChangeID.idOldSelect = personnelShort.peID;
      }
      if(frmPersonnelChangeID.ShowDialog()==true)
      {
        List<PersonnelShort> lstPersonnel = (List<PersonnelShort>)dgrPersonnels.ItemsSource;
        PersonnelShort personnelShort=lstPersonnel.Where(pe => pe.peID == frmPersonnelChangeID.idOldSelect).FirstOrDefault();
        lstPersonnel.Remove(personnelShort);
        dgrPersonnels.Items.Refresh();
        GridHelper.SelectRow(dgrPersonnels, 0);
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadPersonnels
    /// <summary>
    /// Llena el grid de personnels
    /// </summary>
    /// <param name="personnel">objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 10/06/2016
    /// </history>
    private async void LoadPersonnel(PersonnelShort personnel = null)
    {
      try
      {        
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<PersonnelShort> lstPersonnels = await BRPersonnel.GetPersonnel(_leadSource,_salesRoom,_roles,_status,_permission
          ,_relationalOperator,_enumPermission,_dept);
        dgrPersonnels.ItemsSource = lstPersonnels;
        if (lstPersonnels.Count > 0)
        {
          if (personnel != null)
          {
            personnel = lstPersonnels.Where(pe => pe.peID == personnel.peID).FirstOrDefault();
            nIndex = lstPersonnels.IndexOf(personnel);
          }
          GridHelper.SelectRow(dgrPersonnels, nIndex);
        }
        else
        {
          btnDel.IsEnabled = false;
        }        
        StatusBarReg.Content = lstPersonnels.Count + " Personnels";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnels");
      }
    }
    #endregion

    #endregion
  }
}
