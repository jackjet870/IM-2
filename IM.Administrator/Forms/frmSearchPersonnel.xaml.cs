using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchPersonnel.xaml
  /// </summary>
  public partial class frmSearchPersonnel : Window
  {
    #region Variables
    public string leadSource = "ALL";
    public string salesRoom = "ALL";
    public string roles = "ALL";
    public int status = 0;
    public string permission = "ALL";
    public string relationalOperator = "=";
    public string dept = "ALL";
    public EnumPermisionLevel enumPermission = EnumPermisionLevel.None;
    #endregion
    public frmSearchPersonnel()
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
    /// [emoguel] created 11/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadDeps();
      LoadStatus();
      LoadRoles();
      LoadPermission();
      LoadSalesRoom();
      LoadLeadSources();
      LoadRelationalOperator();
      LoadAcces();
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/06/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        Close();
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/06/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    #endregion

    #region Accept
    /// <summary>
    /// guarda los filtros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/06/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      dept = cmbDepts.SelectedValue.ToString();
      status = Convert.ToInt32(cmbStatus.SelectedValue);
      roles = cmbRoles.SelectedValue.ToString();
      permission = cmbPermission.SelectedValue.ToString();
      relationalOperator = cmbOperator.SelectedValue.ToString();
      enumPermission = (EnumPermisionLevel)cmbAcces.SelectedValue;
      leadSource = (dgrLeadSources.SelectedItems.Count > 0) ? string.Join(",", dgrLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(ls => ls.lsID)) : "ALL";
      salesRoom = (dgrSalesRoom.SelectedItems.Count > 0) ? string.Join(",", dgrSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(sr => sr.srID)) :"ALL" ;
      DialogResult = true;
     }
    #endregion
    #endregion

    #region Methods
    #region LoadAcces
    /// <summary>
    /// Llena el combobox de Acces
    /// </summary>
    /// <history>
    /// [emoguel] created 11/06/2016
    /// </history>
    private void LoadAcces()
    {
      cmbAcces.ItemsSource = EnumToListHelper.GetList<EnumPermisionLevel>();
      cmbAcces.SelectedValue = enumPermission;      
    }
    #endregion

    #region Operator
    /// <summary>
    /// Llena el Combobox de operator
    /// </summary>
    /// <history>
    /// [emoguel] created 11/06/2016
    /// </history>
    private void LoadRelationalOperator()
    {
      List<object> lstOperator =new List<object> { new { Value = "=", Name="Equal" },
      new { Value = "<>", Name="Distinct"},new { Value = ">", Name="Greater" },new { Value = ">=", Name="Greater or Equal" },
      new { Value = "<", Name="Less" },new { Value = "<=", Name="Less or equal" }};

      cmbOperator.ItemsSource = lstOperator;
      cmbOperator.SelectedValue = relationalOperator;
    }
    #endregion

    #region LoadLeadSources
    /// <summary>
    /// Llena el grid con los LeadSources a los que tiene acceso el usuario actual
    /// </summary>
    /// <history>
    /// [emoguel] created 11/06/2016
    /// </history>
    private async void LoadLeadSources()
    {
      try
      {
        List<LeadSourceByUser> lstLeadSources = await BRLeadSources.GetLeadSourcesByUser(App.User.User.peID);
        dgrLeadSources.ItemsSource = lstLeadSources;

        List<string> lstSales = leadSource.Split(',').ToList();
        lstSales.ForEach(lead => {
          LeadSourceByUser leadSource = lstLeadSources.Where(ls => ls.lsID == lead).FirstOrDefault();
          if (leadSource != null)
          {
            dgrLeadSources.SelectedItems.Add(leadSource);
          }
        });

        skpStatus.Visibility = Visibility.Collapsed;
        btnAccept.Visibility = Visibility.Visible;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region LoadSalesRoom
    /// <summary>
    /// Llena el grid de salesRoom
    /// </summary>
    /// <history>
    /// [emoguel] created 11/06/2016
    /// </history>
    private async void LoadSalesRoom()
    {
      try
      {
        List<SalesRoomByUser> lstSalesRoom =await BRSalesRooms.GetSalesRoomsByUser(App.User.User.peID);
        dgrSalesRoom.ItemsSource = lstSalesRoom;
        List<string> lstSales = leadSource.Split(',').ToList();
        lstSales.ForEach(sale => {
          SalesRoomByUser saleRoom = lstSalesRoom.Where(sr => sr.srID == sale).FirstOrDefault();
          if(saleRoom != null)
          {
            dgrSalesRoom.SelectedItems.Add(saleRoom);
          }
        });
                
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error , "Personnel");
      }
    }
    #endregion

    #region LoadPermision
    /// <summary>
    /// Carga el combobox de permision
    /// </summary>
    /// <history>
    /// [emoguel] created 11/06/2016
    /// </history>
    private async void LoadPermission()
    {
      try
      {
        List<Permission> lstPermission = await BRPermissions.GetPermissions(1);
        lstPermission.Insert(0, new Permission { pmID = "ALL", pmN = "ALL" });
        cmbPermission.ItemsSource = lstPermission;
        cmbPermission.SelectedValue = permission;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Permission");
      }
    }
    #endregion

    #region LoadRoles
    /// <summary>
    /// Llena el combobox de ROles
    /// </summary>
    /// <history>
    /// [emoguel] created 11/06/2016
    /// </history>
    private async void LoadRoles()
    {
      try
      {
        List<Role> lstRoles = await BRRoles.GetRoles(1);
        lstRoles.Insert(0, new Role { roID = "ALL", roN = "ALL" });
        cmbRoles.ItemsSource = lstRoles;
        cmbRoles.SelectedValue = roles;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region LoadStatus
    /// <summary>
    /// Llena el combobox de status
    /// </summary>
    /// <history>
    /// [emoguel] created 11/06/2016
    /// </history>
    private void LoadStatus()
    {
      List<object> lstOptions = new List<object>();
      lstOptions.Add(new { sName = "All", sValue = 0 });
      lstOptions.Add(new { sName = "ACTIVE", sValue = 1 });
      lstOptions.Add(new { sName = "INACTIVE", sValue = 2 });
      cmbStatus.ItemsSource = lstOptions;
      cmbStatus.SelectedValue = status;
    }
    #endregion

    #region LoadDeps
    /// <summary>
    /// LLena el grid de depts
    /// </summary>
    /// <history>
    /// [emoguel] created 11/06/2016
    /// </history>
    private async void LoadDeps()
    {
      try
      {
        List<Dept> lstDepst = await BRDepts.GetDepts(1);
        lstDepst.Insert(0, new Dept { deID = "ALL", deN = "ALL" });
        cmbDepts.ItemsSource = lstDepst;
        cmbDepts.SelectedValue = dept;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #endregion
  }
}
