using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.Model;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPersonnelChangeID.xaml
  /// </summary>
  public partial class frmPersonnelChangeID : Window
  {
    #region variables
    public string idOldSelect, idNewSelect = "";
    private string msjOld, msjNew = "";
    #endregion
    public frmPersonnelChangeID()
    {
      InitializeComponent();
    }

    #region Methods form
    #region Window_Loaded
    /// <summary>
    /// Carga los datos iniciales de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPersonnel();            
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/06/2016
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

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region btnChangeID_Click
    /// <summary>
    /// Cambia el ID del personnel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    private async void btnChangeID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        lockButtons(false);

        if(cmbPersonnelNew.SelectedValue!=null && cmbPersonnelOld.SelectedValue!=null)
        {
          if(cmbPersonnelNew.SelectedValue!=cmbPersonnelOld.SelectedValue)
          {
            #region Tasks            
            lockButtons(false);
            txtStatus.Text = "Loading...";
            if (cmbPersonnelOld.SelectedValue.ToString() != idOldSelect)
            {
              idOldSelect = cmbPersonnelOld.SelectedValue.ToString();              
              PersonnelShort personnelShort = (PersonnelShort)cmbPersonnelOld.SelectedItem;
              PersonnelStatistics personnelStatistics = await BRPersonnel.GetPersonnelStatistics(personnelShort.peID);              
              LoadStatistics(tvwOld, personnelStatistics, personnelShort);
            }
            if (cmbPersonnelNew.SelectedValue.ToString() != idNewSelect)
            {
              idNewSelect = cmbPersonnelNew.SelectedValue.ToString();
              PersonnelShort personnelShort = (PersonnelShort)cmbPersonnelNew.SelectedItem;
              PersonnelStatistics personnelStatistics = await BRPersonnel.GetPersonnelStatistics(personnelShort.peID);
              LoadStatistics(tvwNew, personnelStatistics, personnelShort);
            }

            #endregion

            MessageBoxResult msgResult = UIHelper.ShowMessage("Are you sure you want to change the User's ID? \n "+msjOld+" \n "+msjNew, MessageBoxImage.Question, "Personnel Change ID");            
            if (msgResult == MessageBoxResult.Yes)
            {
              txtStatus.Text = "Changing ID...";
              int nRes = await BRPersonnel.UpdatePersonnelId(cmbPersonnelOld.SelectedValue.ToString(), cmbPersonnelNew.SelectedValue.ToString());
              UIHelper.ShowMessageResult("ID", nRes);
              if(nRes>0)
              {
                DialogResult = true;
                Close();
              }
            }
            lockButtons(true);
          }
        }
        else
        {
          UIHelper.ShowMessage("Please select old ID and new ID");
        }


        lockButtons(true);
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel Change ID");
      }
      
    }
    #endregion

    #region btnGetStaticsOld_Click
    /// <summary>
    /// Obtiene los statistics del antiguo personnel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    private async void btnGetStaticsOld_Click(object sender, RoutedEventArgs e)
    {
      if (cmbPersonnelOld.SelectedValue != null)
      {
        idOldSelect = cmbPersonnelOld.SelectedValue.ToString();
        lockButtons(false);
        txtStatus.Text = "Loading...";
        PersonnelShort personnelShort = (PersonnelShort)cmbPersonnelOld.SelectedItem;
        PersonnelStatistics personnelStatistics = await BRPersonnel.GetPersonnelStatistics(personnelShort.peID);
        LoadStatistics(tvwOld, personnelStatistics, personnelShort);
        lockButtons(true);
      }
      else
      {
        UIHelper.ShowMessage("Please select a Personnel");
      }
    }
    #endregion

    #region btnGetStaticsNew_Click
    /// <summary>
    /// Obtiene los statistics del nuevo personnel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    private async void btnGetStaticsNew_Click(object sender, RoutedEventArgs e)
    {
      if (cmbPersonnelNew.SelectedValue != null)
      {       
        idNewSelect = cmbPersonnelNew.SelectedValue.ToString();
        lockButtons(false);
        status.Visibility = Visibility.Visible;
        txtStatus.Text = "Loading...";
        PersonnelShort personnelShort = (PersonnelShort)cmbPersonnelNew.SelectedItem;
        PersonnelStatistics personnelStatistics = await BRPersonnel.GetPersonnelStatistics(personnelShort.peID);
        LoadStatistics(tvwNew, personnelStatistics, personnelShort);  
        status.Visibility = Visibility.Collapsed;
        lockButtons(true);
      }
      else
      {
        UIHelper.ShowMessage("Please select a Personnel");
      }
    }
    #endregion
    #endregion


    #region Methods
    #region LoadStatistics
    /// <summary>
    /// Agrega los statistics a un treeview
    /// </summary>
    /// <param name="trv">treeview a modificar</param>
    /// <param name="idPersonnel">id del personnel a buscar sus statistics</param>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    private void LoadStatistics(TreeView trv, PersonnelStatistics personnelStatistics,PersonnelShort personnelShort)
    {
      if (personnelStatistics != null)
      {
          #region Operations
          //Total de Guest
          int? valueGuest = personnelStatistics.Guests_PR + personnelStatistics.Guests_Liner + personnelStatistics.Guests_Closer + personnelStatistics.Guests_Exit + personnelStatistics.Guests_Podium +
              personnelStatistics.Guests_VLO + personnelStatistics.Guests_Host + personnelStatistics.Guests_Captain + personnelStatistics.Shows_Salesmen + personnelStatistics.Guests_Log + personnelStatistics.Guests_Movements;
          //Total de sales
          int? valueSales = personnelStatistics.Sales_PR + personnelStatistics.Sales_Liner + personnelStatistics.Sales_Closer + personnelStatistics.Sales_Exit + personnelStatistics.Sales_Podium + personnelStatistics.Sales_VLO +
            personnelStatistics.Sales_Captain + personnelStatistics.Sales_Salesmen + personnelStatistics.Sales_Log;
          //Total de GiftsReceipts
          int? valueGiftsReceipts = personnelStatistics.Gifts_Receipts_Authorized + personnelStatistics.Gifts_Receipts_Host + personnelStatistics.Gifts_Receipts_PR + personnelStatistics.Gifts_Receipts_Payments +
            personnelStatistics.Gifts_Receipts_Log;
          //Total operations
          int? valueOperations = valueGuest + valueSales + valueGiftsReceipts + personnelStatistics.Meal_Tickets + personnelStatistics.Exchange_Rate_Log + personnelStatistics.Warehouse_Movements + personnelStatistics.PR_Notes +
            personnelStatistics.Logins_Log + personnelStatistics.Assistance + personnelStatistics.Days_Off + personnelStatistics.Efficiency_Salesmen;

          //Add operation          
          TreeViewItem tviOperations = addTreeViewItem("Operations", valueOperations);
          trv.Items.Add(tviOperations);
          //Huespedes
          TreeViewItem tviGuest = addTreeViewItem("Guests", valueGuest);
          tviOperations.Items.Add(tviGuest);
          tviGuest.Items.Add(addTreeViewItem("Guests PR", personnelStatistics.Guests_PR));
          tviGuest.Items.Add(addTreeViewItem("Guests Liner", personnelStatistics.Guests_Liner));
          tviGuest.Items.Add(addTreeViewItem("Guests Closer", personnelStatistics.Guests_Closer));
          tviGuest.Items.Add(addTreeViewItem("Guests Exit", personnelStatistics.Guests_Exit));
          tviGuest.Items.Add(addTreeViewItem("Guests Podium", personnelStatistics.Guests_Podium));
          tviGuest.Items.Add(addTreeViewItem("Guests VLO", personnelStatistics.Guests_VLO));
          tviGuest.Items.Add(addTreeViewItem("Guests Host", personnelStatistics.Guests_Host));
          tviGuest.Items.Add(addTreeViewItem("Guests Captain", personnelStatistics.Guests_Captain));
          tviGuest.Items.Add(addTreeViewItem("Shows Salesmen", personnelStatistics.Shows_Salesmen));
          tviGuest.Items.Add(addTreeViewItem("Guests Log", personnelStatistics.Guests_Log));
          tviGuest.Items.Add(addTreeViewItem("Guests Movements", personnelStatistics.Guests_Movements));

          //Ventas
          TreeViewItem tviSales = addTreeViewItem("Sales", valueSales);
          tviOperations.Items.Add(tviSales);
          tviSales.Items.Add(addTreeViewItem("Sales PR", personnelStatistics.Sales_PR));
          tviSales.Items.Add(addTreeViewItem("Sales Liner", personnelStatistics.Sales_Liner));
          tviSales.Items.Add(addTreeViewItem("Sales Closer", personnelStatistics.Sales_Closer));
          tviSales.Items.Add(addTreeViewItem("Sales Exit", personnelStatistics.Sales_Podium));
          tviSales.Items.Add(addTreeViewItem("Sales VLO", personnelStatistics.Sales_VLO));
          tviSales.Items.Add(addTreeViewItem("Sales Captain", personnelStatistics.Sales_Captain));
          tviSales.Items.Add(addTreeViewItem("Sales Salesmen", personnelStatistics.Sales_Salesmen));
          tviSales.Items.Add(addTreeViewItem("Sales Log", personnelStatistics.Sales_Log));

          //Recibos de regalo
          TreeViewItem tviGiftReceip = addTreeViewItem("Gifts Receipts", valueGiftsReceipts);
          tviOperations.Items.Add(tviGiftReceip);
          tviGiftReceip.Items.Add(addTreeViewItem("Gifts Receipts Authorized", personnelStatistics.Gifts_Receipts_Authorized));
          tviGiftReceip.Items.Add(addTreeViewItem("Gifts Receipts Host", personnelStatistics.Gifts_Receipts_Host));
          tviGiftReceip.Items.Add(addTreeViewItem("Gifts Receipts PR", personnelStatistics.Gifts_Receipts_PR));
          tviGiftReceip.Items.Add(addTreeViewItem("Gifts Receipts Payments", personnelStatistics.Gifts_Receipts_Payments));
          tviGiftReceip.Items.Add(addTreeViewItem("Gifts Receipts Log", personnelStatistics.Gifts_Receipts_Log));

          //cupones de comida
          tviOperations.Items.Add(addTreeViewItem("Meal Tickets", personnelStatistics.Meal_Tickets));

          //Tipos de cambio
          tviOperations.Items.Add(addTreeViewItem("Exchange Rate Log", personnelStatistics.Exchange_Rate_Log));

          //Movimientos al inventario
          tviOperations.Items.Add(addTreeViewItem("Warehouse Movements", personnelStatistics.Warehouse_Movements));

          //Notas de PR
          tviOperations.Items.Add(addTreeViewItem("PR Notes", personnelStatistics.PR_Notes));

          //Sesiones
          tviOperations.Items.Add(addTreeViewItem("Logins Log", personnelStatistics.Logins_Log));

          //Asistencia
          tviOperations.Items.Add(addTreeViewItem("Assistance", personnelStatistics.Assistance));
          tviOperations.Items.Add(addTreeViewItem("Days Off", personnelStatistics.Days_Off));

          //Eficiencia
          tviOperations.Items.Add(addTreeViewItem("Efficiency Salesmen", personnelStatistics.Efficiency_Salesmen));

          #endregion

          #region Catalogs
          //Total de personnel
          int? valuePersonnel = personnelStatistics.Places_Access + personnelStatistics.Roles + personnelStatistics.Permissions + personnelStatistics.Personnel_Liner + personnelStatistics.Posts_Log +
            personnelStatistics.Teams_Log + personnelStatistics.Teams_Guest_Services + personnelStatistics.Teams_Salesmen;
          //Total de sales Room
          int? valueSalesRoom = personnelStatistics.Sales_Rooms + personnelStatistics.Sales_Rooms_Log;

          //Total de catalogs
          int? valueCatalogs = valuePersonnel + valueSalesRoom + personnelStatistics.Gifts_Log + personnelStatistics.Lead_Sources + personnelStatistics.Configurations;

          //Add operation           
          TreeViewItem tviCatalogs = addTreeViewItem("Catalogs", valueCatalogs);
          trv.Items.Add(tviCatalogs);

          //Personnel
          TreeViewItem tviPersonnel = addTreeViewItem("Personnel", valuePersonnel);
          tviCatalogs.Items.Add(tviPersonnel);
          tviPersonnel.Items.Add(addTreeViewItem("Places Acces", personnelStatistics.Places_Access));
          tviPersonnel.Items.Add(addTreeViewItem("Roles", personnelStatistics.Roles));
          tviPersonnel.Items.Add(addTreeViewItem("Permissions", personnelStatistics.Permissions));
          tviPersonnel.Items.Add(addTreeViewItem("Personnel Liner", personnelStatistics.Personnel_Liner));
          tviPersonnel.Items.Add(addTreeViewItem("Post Log", personnelStatistics.Posts_Log));
          tviPersonnel.Items.Add(addTreeViewItem("Teams log", personnelStatistics.Teams_Log));
          tviPersonnel.Items.Add(addTreeViewItem("Teams Guest Services", personnelStatistics.Teams_Guest_Services));
          tviPersonnel.Items.Add(addTreeViewItem("Teams Salesmen", personnelStatistics.Teams_Salesmen));

          //Gifts
          TreeViewItem tviGift = addTreeViewItem("Gifts", personnelStatistics.Gifts_Log);
          tviCatalogs.Items.Add(tviGift);
          tviGift.Items.Add(addTreeViewItem("Gifts Log", personnelStatistics.Gifts_Log));

          //Lead Sources
          tviCatalogs.Items.Add(addTreeViewItem("Lead Sources", personnelStatistics.Lead_Sources));

          //SalesRooom
          TreeViewItem tviSalesRoom = addTreeViewItem("Sales Room", valueSalesRoom);
          tviCatalogs.Items.Add(tviSalesRoom);
          tviSalesRoom.Items.Add(addTreeViewItem("Sales Room Boss", personnelStatistics.Sales_Rooms));
          tviSalesRoom.Items.Add(addTreeViewItem("Sales Rooms Log", personnelStatistics.Sales_Rooms_Log));

          //Configurations
          tviCatalogs.Items.Add(addTreeViewItem("Configurations", personnelStatistics.Configurations));
          #endregion

          if (trv.Name == "tvwOld")
          {
            msjOld = "\n Old ID=" + personnelShort.peID + ", " + personnelShort.peN + " \n Records " + valueOperations + valueCatalogs + " " + valueOperations + " Operations + " + valueCatalogs + " Catalogs";
          }
          else
          {
            msjNew = "\n New ID=" + personnelShort.peID + ", " + personnelShort.peN + " \n Records " + valueOperations + valueCatalogs + " " + valueOperations + " Operations + " + valueCatalogs + " Catalogs";
          }
        }
        else
        {
          trv.Items.Add(addTreeViewItem("No Data", null));
        }
    }
    #endregion

    #region addTreeViewItem
    /// <summary>
    /// Devuelve un treeviewItem
    /// </summary>
    /// <param name="title">titulo del treeview</param>
    /// <param name="value">valor del item</param>
    /// <returns>devuelve un treeViewItem</returns>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    private TreeViewItem addTreeViewItem(string title, int? value)
    {
      TreeViewItem treeViewItem = new TreeViewItem();
      treeViewItem.Header = title +((value!=null)? " = " + value:"");
      return treeViewItem;
    }
    #endregion

    #region LoadPersonnel
    /// <summary>
    /// Carga los combobox de personnel
    /// </summary>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    private async void LoadPersonnel()
    {
      try
      {        
        List<PersonnelShort> lstPersonnel = await BRPersonnel.GetPersonnel(status: 0);
        cmbPersonnelNew.ItemsSource = lstPersonnel;
        cmbPersonnelOld.ItemsSource = lstPersonnel;
        cmbPersonnelOld.SelectedValue = idOldSelect;
        if (!string.IsNullOrWhiteSpace(idOldSelect))
        {
          btnGetStaticsOld_Click(null, null);
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel Change ID");
      }
    }


    #endregion

    #region lockButtons
    /// <summary>
    /// Bloquea y desbloquea botons
    /// </summary>
    /// <param name="blnLock">False. Bloqueaa | True. Desbloquea</param>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    private void lockButtons(bool blnLock)
    {
      status.Visibility = (blnLock) ? Visibility.Collapsed : Visibility.Visible;
      btnChangeID.IsEnabled = blnLock;
      btnGetStaticsNew.IsEnabled = blnLock;
      btnGetStaticsOld.IsEnabled = blnLock;
    }
    #endregion

    #endregion


  }
}
