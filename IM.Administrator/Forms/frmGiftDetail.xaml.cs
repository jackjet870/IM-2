using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
  /// Interaction logic for frmGiftDetail.xaml
  /// </summary>
  public partial class frmGiftDetail : Window
  {
    #region Variables
    public Gift gift = new Gift();
    public Gift _oldGift = new Gift();
    public EnumMode enumMode;
    private bool _isClosing = false;
    private bool _isCellCancel = false;
    private List<Agency> _lstOldAgencies = new List<Agency>();
    private List<Location> _lstOldLocations = new List<Location>();
    private List<GiftPackageItem> _lstOldPacks = new List<GiftPackageItem>();
    CollectionViewSource collectionLocations = new CollectionViewSource();
    CollectionViewSource collectionPackages = new CollectionViewSource();
    #endregion
    public frmGiftDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window_Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(gift, _oldGift);
      GiftInPack();
      LoadLocations();
      LoadGiftPack();
      LoadPrograms();
      LoadRegions();
      LoadAgencies();
      LoadGiftCategories();
      LoadProductElectronicPurse();
      LoadTransacctionTypes();
      LoadPromotionSistur();
      chkgiPack.IsEnabled = !gift.giPack;
      dgrGiftInPack.IsReadOnly = gift.giPack;
      if (enumMode != EnumMode.ReadOnly)
      {
        UIHelper.SetUpControls(gift, this);
        grdCost.IsEnabled = true;
        grdGeneral.IsEnabled = true;
        grdLocations.IsEnabled = true;
        txtgiID.IsEnabled = (enumMode == EnumMode.Add);
      }
      DataContext = gift;
      dgrLocations.BeginningEdit += GridHelper.dgr_BeginningEdit;
      dgrAgencies.BeginningEdit += GridHelper.dgr_BeginningEdit;
      GridHelper.SetUpGrid(dgrGiftInPack, new GiftPackageItem());
    }
    #endregion    

    #region Window_Closing
    /// <summary>
    /// Verifica cambios antes de cerrar la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      btnCancel.Focus();
      if (!_isClosing)
      {
        List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
        List<Location> lstLocations = (List<Location>)dgrLocations.ItemsSource;
        List<GiftPackageItem> lstPackItems = (List<GiftPackageItem>)dgrGiftInPack.ItemsSource;
        if (enumMode != EnumMode.ReadOnly && (!ObjectHelper.IsEquals(gift, _oldGift) || ChangedGifts(lstPackItems) || !ObjectHelper.IsListEquals(lstAgencies, _lstOldAgencies)
          || !ObjectHelper.IsListEquals(lstLocations, _lstOldLocations)))
        {
          dgrGiftInPack.CancelEdit();
          dgrAgencies.CancelEdit();
          dgrLocations.CancelEdit();
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result != MessageBoxResult.Yes)
          {
            e.Cancel = true;
          }
        }
      }
    }
    #endregion

    #region btnAccept_Click
    /// <summary>
    /// Guarda los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
        List<Location> lstLocations = (List<Location>)dgrLocations.ItemsSource;
        List<GiftPackageItem> lstPackItems = (List<GiftPackageItem>)dgrGiftInPack.ItemsSource;
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(gift, _oldGift) && ObjectHelper.IsListEquals(lstAgencies, _lstOldAgencies) && ObjectHelper.IsListEquals(lstLocations, _lstOldLocations) && !ChangedGifts(lstPackItems))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          btnAccept.Visibility = Visibility.Collapsed;
          string strMsj = ValidateHelper.ValidateForm(this, "Gift", true);
          string strGeneral = ValidateGeneral();

          if (strGeneral != "") { strMsj += (strMsj != "") ? " \n " + strGeneral : strGeneral; }

          if (strMsj == "")
          {
            strMsj = "";
            #region Short Name
            var lstShortN = await BRGifts.GetGifts(-1, new Gift { giShortN = gift.giShortN });
            if (lstShortN.Any(gi => gi.giID != gift.giID && gi.giShortN == gift.giShortN))
            {
              strMsj += "Short Name must not be repeated. \n";
              txtgiShortN.Focus();
            }
            #endregion

            #region Promtion Sistur
            if (!string.IsNullOrWhiteSpace(gift.giPVPPromotion))
            {
              var lstPvpProms = await BRGifts.GetGifts(-1, new Gift { giPVPPromotion = gift.giPVPPromotion });
              if (lstPvpProms.Any(gi => gi.giPVPPromotion == gift.giPVPPromotion && gi.giID != gift.giID))
              {
                strMsj += "Promotion Sistur must not be repeated.";
                cmbgiPVPPromotion.Focus();
              }
            }
            #endregion

            if (strMsj == "")
            {
              CalculatePricesPackage(lstPackItems);

              #region GetList
              //Locaciones
              var lstAddLocations = lstLocations.Where(lo => !_lstOldLocations.Any(loo => loo.loID == lo.loID)).ToList();
              var lstDelLocations = _lstOldLocations.Where(lo => !lstLocations.Any(loo => lo.loID == loo.loID)).ToList();
              //Agencias
              var lstAddAgencies = lstAgencies.Where(ag => !_lstOldAgencies.Any(agg => agg.agID == ag.agID)).ToList();
              var lstDelAgencies = _lstOldAgencies.Where(ag => !lstAgencies.Any(agg => agg.agID == ag.agID)).ToList();

              //GiftItemPack
              var lstAddGiftPack = lstPackItems.Where(gp => !_lstOldPacks.Any(gpp => gp.gpgi == gpp.gpgi)).ToList();
              var lstDelGiftPack = _lstOldPacks.Where(gp => !lstPackItems.Any(gpp => gpp.gpgi == gp.gpgi)).ToList();
              var lstUpdGiftPack = lstPackItems.Where(gp => _lstOldPacks.Any(gpp => gp.gpgi == gpp.gpgi && gp.gpQty != gpp.gpQty)).ToList();
              #endregion

              int nRes = await BRGifts.SaveGift(gift, (enumMode == EnumMode.Edit), lstAddLocations, lstDelLocations, lstAddAgencies, lstDelAgencies, lstAddGiftPack, lstDelGiftPack, lstUpdGiftPack,
                (enumMode == EnumMode.Add && gift.giInven), App.User.User.peID);
              UIHelper.ShowMessageResult("Gift", nRes);
              if (nRes > 0)
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
          }
          else
          {
            UIHelper.ShowMessage(strMsj);
          }
          skpStatus.Visibility = Visibility.Collapsed;
          btnAccept.Visibility = Visibility.Visible;
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }

    }
    #endregion

    #region chk_Checked
    /// <summary>
    /// Ejecuta una funcion al seleccionar el checkbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private void chk_Checked(object sender, RoutedEventArgs e)
    {
      var checkbox = sender as CheckBox;
      switch (checkbox.Name)
      {
        case "chkgiPack":
          {
            dgrGiftInPack.Visibility = Visibility.Visible;
            break;
          }
        case "chkgiMonetary":
          {
            txtgiAmount.IsEnabled = true;
            break;
          }
      }
    }
    #endregion

    #region chk_Unchecked
    /// <summary>
    /// Ejecuta una funcion al deseleccionar el checkbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private void chk_Unchecked(object sender, RoutedEventArgs e)
    {
      var checkbox = sender as CheckBox;
      switch (checkbox.Name)
      {
        case "chkgiPack":
          {
            dgrGiftInPack.Visibility = Visibility.Collapsed;
            break;
          }
        case "chkgiMonetary":
          {
            txtgiAmount.IsEnabled = false;
            gift.giAmount = 0;
            txtgiAmount.Text = "0";
            break;
          }
      }
    }
    #endregion

    #region dgr_CellEditEnding
    /// <summary>
    /// Verifica que no se repitam registros en la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/07/2016
    /// </history>
    private void dgr_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        DataGrid dgr = sender as DataGrid;
        switch (e.Column.SortMemberPath)
        {
          case "loN":
            {
              _isCellCancel = false;
              var cp = (ContentPresenter)e.EditingElement;
              var combo = (ComboBox)cp.ContentTemplate.FindName("cmbLocations", cp);
              bool blnIsRepeat = GridHelper.HasRepeatItem(combo, dgr, true);
              if (!blnIsRepeat)
              {
                dtcLocations.Header = "Locations (" + (dgr.Items.Count - 1) + ")";
              }
              e.Cancel = blnIsRepeat;
              break;
            }
          case "agID":
            {
              _isCellCancel = false;
              bool blnIsRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgr);
              if (!blnIsRepeat)
              {
                cmbAgencies.Header = "Agencies (" + (dgr.Items.Count - 1) + ")";
              }
              e.Cancel = blnIsRepeat;
              break;
            }
          case "GiftItem.giN":
            {
              dgr.RowEditEnding -= dgr_RowEditEnding;
              var cp = (ContentPresenter)e.EditingElement;
              var combo = (ComboBox)cp.ContentTemplate.FindName("cmbGiftPack", cp);
              bool blnIsRepeat = GridHelper.HasRepeatItem(combo, dgrGiftInPack, false, "gpgi");
              if (!blnIsRepeat)
              {
                GiftPackageItem giftPackage = (GiftPackageItem)e.Row.Item;
                giftPackage.GiftItem = new Gift();
                Gift giftSelected = (Gift)combo.SelectedItem;
                ObjectHelper.CopyProperties(giftPackage.GiftItem, giftSelected);
                giftPackage.gpgi = giftSelected.giID;
                dtcGiftPack.Header = "Gift In Pack (" + (dgr.Items.Count - 1) + ")";
              }
              else
              {
                dgr.Focus();
                combo.Focus();
              }
              e.Cancel = blnIsRepeat;
              dgr.RowEditEnding += dgr_RowEditEnding;
              break;
            }
          case "gpQty":
            {
              TextBox txt = (TextBox)e.EditingElement;
              if (string.IsNullOrWhiteSpace(txt.Text) || Convert.ToInt32(txt.Text) < 1)
              {
                if (!Keyboard.IsKeyDown(Key.Tab))
                {
                  UIHelper.ShowMessage("Quantity can no be lower than 1.");
                  e.Cancel = true;
                }
              }
              else
              {
                GiftPackageItem giftPackageItem = (GiftPackageItem)e.Row.Item;
                giftPackageItem.gpQty = Convert.ToInt32(txt.Text);
              }
              break;
            }
        }
      }
      else
      {
        _isCellCancel = true;
      }

    }
    #endregion

    #region dgr_RowEditEnding
    /// <summary>
    /// Cancela o finalizá la edicion de un registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/07/2016
    /// </history>
    private void dgr_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      DataGrid dgr = sender as DataGrid;
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (dgr.CurrentColumn != null)
        {
          switch (dgr.CurrentColumn.SortMemberPath)
          {
            case "gpQty":
              {
                GiftPackageItem giftInPack = (GiftPackageItem)e.Row.Item;
                if (giftInPack.gpQty < 1)
                {
                  e.Cancel = true;
                  UIHelper.ShowMessage("Quantity can no be lower than 1.");
                }
                else if (string.IsNullOrWhiteSpace(giftInPack.gpgi))
                {
                  e.Cancel = true;
                  UIHelper.ShowMessage("Please Select a Gift.");
                }
                break;
              }
            case "GiftItem.giN":
              {
                GiftPackageItem giftPackage = (GiftPackageItem)e.Row.Item;
                if (giftPackage.GiftItem == null)
                {
                  UIHelper.ShowMessage("Please select a Gift");
                  e.Cancel = true;
                }
                break;
              }
            default:
              {
                dgr.RowEditEnding -= dgr_RowEditEnding;
                if (_isCellCancel)
                {
                  dgr.CancelEdit();
                }
                else
                {
                  dgr.CommitEdit();
                  dgr.Items.Refresh();
                  GridHelper.SelectRow(dgr, dgr.SelectedIndex);
                }
                dgr.RowEditEnding += dgr_RowEditEnding;
                _isCellCancel = false;
                break;
              }
          }
        }
        else if (dgr.Name == "dgrGiftInPack")
        {
          GiftPackageItem giftPackage = (GiftPackageItem)e.Row.Item;
          if (giftPackage.GiftItem == null)
          {
            e.Cancel = true;
          }
        }
      }
    }
    #endregion

    #region dgr_BeginningEdit
    /// <summary>
    /// Valida antes de iniciar la edición
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/07/2016
    /// </history>
    private void dgr_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      var grid = (DataGrid)sender;
      if (!GridHelper.IsInEditMode(sender as DataGrid))
      {
        switch (e.Column.SortMemberPath)
        {
          case "GiftItem.giN":
            {
              GiftPackageItem giftPackageItem = (GiftPackageItem)e.Row.Item;
              if (giftPackageItem.gpQty < 1)
              {
                e.Cancel = true;
                UIHelper.ShowMessage("Quantity can not be lower than 1.");
                grid.CurrentCell = new DataGridCellInfo(grid.SelectedItem, grid.Columns[0]);
                grid.BeginningEdit -= dgr_BeginningEdit;
                grid.BeginEdit();
                grid.BeginningEdit += dgr_BeginningEdit;
              }
              break;
            }
        }
      }
      else
      {
        e.Cancel = true;
      }
    }
    #endregion

    #region dgrGiftInPack_PreparingCellForEdit
    /// <summary>
    /// Manda el foco al combobox de edición
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/07/2016
    /// </history>
    private void dgrGiftInPack_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
      if (e.Column.SortMemberPath == "GiftItem.giN")
      {
        var cp = (ContentPresenter)e.EditingElement;
        var combo = (ComboBox)cp.ContentTemplate.FindName("cmbGiftPack", cp);
        combo.Focus();
      }
      else
      {
        var txt = e.EditingElement as TextBox;
        txt.Focus();
      }
    }
    #endregion

    #region btnAssignLocations_Click
    /// <summary>
    /// Asigna Locations
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/07/2016
    /// </history>
    private async void btnAssignLocations_Click(object sender, RoutedEventArgs e)
    {
      string strMsj = "";
      if (dgrRegions.SelectedItems.Count < 1)
      {
        strMsj += "Specify at least one Region. \n";
      }
      if (dgrPrograms.SelectedItems.Count < 1)
      {
        strMsj += "Specify at least one Program.";
      }

      if (strMsj == "")
      {
        string programs = string.Join(",", dgrPrograms.SelectedItems.OfType<Program>().Select(pg => pg.pgID).ToList());
        string regions = string.Join(",", dgrRegions.SelectedItems.OfType<Region>().Select(rg => rg.rgID).ToList());
        var lstAsiggn = await BRLocations.GetLocationsByRegionProgram(programs, 1, regions);

        List<Location> lstLocations = lstAsiggn.Select(lcs => new Location { loA = true, loID = lcs.loID, loN = lcs.loN }).ToList();
        dgrLocations.ItemsSource = lstLocations;
        dtcLocations.Header = "Locations (" + lstLocations.Count + ")";
      }
      else
      {
        UIHelper.ShowMessage(strMsj);
      }
    }
    #endregion

    #region KeyDown
    // <summary>
    /// Cambia el contador de los registros
    /// </summary>
    /// <history>
    /// [emoguel] created 16/07/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      DataGridRow dgrRow = sender as DataGridRow;
      var name = dgrRow.Item.GetType().Name;
      if (e.Key == Key.Delete)
      {
        switch (name)
        {
          case "GiftPackageItem":
            {
              if (chkgiPack.IsEnabled == true)
              {
                dtcGiftPack.Header = "Gift In Pack (" + (dgrGiftInPack.Items.Count - 2) + ")";
              }
              break;
            }
          case "Location":
            {
              dtcLocations.Header = "Locations (" + (dgrLocations.Items.Count - 2) + ")";
              break;
            }
          case "Agency":
            {
              cmbAgencies.Header = "Agencies (" + (dgrAgencies.Items.Count - 2) + ")";
              break;
            }
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadLocations
    /// <summary>
    /// Llena el combobox de locations
    /// </summary>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private async void LoadLocations()
    {
      try
      {
        collectionLocations = (CollectionViewSource)FindResource("cvsLocations");
        collectionLocations.Source = await BRLocations.GetLocations(1);
        List<Location> lstLocations = new List<Location>();
        if (enumMode != EnumMode.Add)
        {
          var locationByUser = await BRLocations.GetLocationsByGift(gift.giID);
          lstLocations = locationByUser.ToList();
          _lstOldLocations = locationByUser.ToList();
        }
        dgrLocations.ItemsSource = lstLocations;
        dtcLocations.Header = "Locations (" + lstLocations.Count + ")";
        GridHelper.SelectRow(dgrLocations, 0);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gift");
      }
    }
    #endregion

    #region LoadGiftPack
    /// <summary>
    /// Llena el combobox de Gift package con los gifts que no son paquetes
    /// </summary>
    /// <history>
    /// [emoguel] cretaed 08/07/2016
    /// </history>
    private async void LoadGiftPack()
    {
      try
      {
        collectionPackages = (CollectionViewSource)FindResource("cvsGiftsPackage");
        var lstGift = await BRGifts.GetGifts(1, giftPack: (enumMode == EnumMode.Add) ? 1 : 0);
        List<GiftPackageItem> lstGiftPack = new List<GiftPackageItem>();
        collectionPackages.Source = lstGift;
        if (enumMode != EnumMode.Add)
        {
          lstGiftPack = await BRGiftsPacks.GetGiftsPacks(new GiftPackageItem { gpPack = gift.giID }, true);
          _lstOldPacks = lstGiftPack.Select(gp => new GiftPackageItem { gpgi = gp.gpgi, gpQty = gp.gpQty, gpPack = gp.gpPack }).ToList();
        }
        dgrGiftInPack.ItemsSource = lstGiftPack.ToList();
        dtcGiftPack.Header = "Gifts In Pack (" + lstGiftPack.Count + ")";
        GridHelper.SelectRow(dgrGiftInPack, 0);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gift");
      }
    }
    #endregion

    #region LoadAgencies
    /// <summary>
    /// Llena el combobox de Agencias
    /// </summary>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private async void LoadAgencies()
    {
      try
      {
        cmbAgencies.ItemsSource = await BRAgencies.GetAgencies(null, 1);
        List<Agency> lstAgencies = new List<Agency>();
        if (enumMode != EnumMode.Add)
        {
          var agenciesByGift = await BRAgencies.GetAgenciesByGift(gift.giID);
          lstAgencies = agenciesByGift.ToList();
          _lstOldAgencies = agenciesByGift.ToList();
        }
        dgrAgencies.ItemsSource = lstAgencies;
        cmbAgencies.Header = "Agencies (" + lstAgencies.Count + ")";
        GridHelper.SelectRow(dgrAgencies, 0);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gift");
      }
    }
    #endregion

    #region LoadPrograms
    /// <summary>
    /// Carga el grid de Programs
    /// </summary>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private async void LoadPrograms()
    {
      try
      {
        dgrPrograms.ItemsSource = await BRPrograms.GetPrograms();
        GridHelper.SelectRow(dgrPrograms, 0);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gift");
      }
    }
    #endregion

    #region LoadRegion
    /// <summary>
    /// Llena el grid de Regions
    /// </summary>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private async void LoadRegions()
    {
      try
      {
        dgrRegions.ItemsSource = await BRRegions.GetRegions(1);
        GridHelper.SelectRow(dgrRegions, 0);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gift");
      }
    }
    #endregion

    #region LoadGiftCategories
    /// <summary>
    /// Llena el combobox de categorias
    /// </summary>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private async void LoadGiftCategories()
    {
      try
      {
        cmbgigc.ItemsSource = await BRGiftsCategories.GetGiftsCategories(nStatus: 1);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gifts");
      }
    }
    #endregion

    #region LoadProductElectronicPurse
    /// <summary>
    /// Llena el combo de Productos del Monedero electronico
    /// </summary>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private async void LoadProductElectronicPurse()
    {
      try
      {
        cmbgiProductGiftCard.ItemsSource = await ClubesHelper.GetProductsBySystem();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gifts");
      }
    }
    #endregion

    #region LoadPromotionSistur
    /// <summary>
    /// Llena el combobox de promotionsistur
    /// </summary>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private async void LoadPromotionSistur()
    {
      try
      {
        cmbgiPVPPromotion.ItemsSource = await SisturHelper.getPromotionsType("REGALO", "ACTIV");
        skpStatus.Visibility = Visibility.Collapsed;
        btnAccept.Visibility = Visibility.Visible;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gift");
      }
    }
    #endregion

    #region LoadTransacctionTypes
    /// <summary>
    /// Carga el combobox dee Transacction types
    /// </summary>
    /// <history>
    /// [emoguel] created 08/07/2016
    /// </history>
    private async void LoadTransacctionTypes()
    {
      try
      {
        cmbgiOperaTransactionType.ItemsSource = await WirePRHelper.GetTransactionTypes();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gifts");
      }
    }
    #endregion

    #region GiftInPack
    /// <summary>
    /// si ya se entrego por lo menos un paquete en regalos (Para evitar convertir en paquete un regalo ya otorgado)
    /// </summary>
    /// <history>
    /// [emoguel] created 11/07/2016
    /// </history>
    private async void GiftInPack()
    {
      if (enumMode == EnumMode.Edit)
      {
        var lstGiftsDetail = await BRGiftsReceiptDetail.GetGiftsReceiptDetail(new GiftsReceiptDetail { gegi = gift.giID });
        chkgiPack.IsEnabled = (lstGiftsDetail.Count < 1);
        dgrGiftInPack.IsReadOnly = (lstGiftsDetail.Count > 0);
      }
    }
    #endregion

    #region ValidateGeneral
    /// <summary>
    /// valida los datos adicionales antes de guardar
    /// </summary>
    /// <history>
    /// [emoguel] created 11/07/2016
    /// </history>
    private string ValidateGeneral()
    {
      string strMsj = "";

      if (!ValidateHelper.validateCharacters((gift.giShortN != null) ? gift.giShortN : ""))//Validar los caracteres especiales en giftShort
      {
        strMsj += "Can not use &, %, '' or blank space in short name. \n";
        tbiGeneral.IsSelected = true;
        tbiGeneral.UpdateLayout();
        txtgiShortN.Focus();
      }

      if (gift.giQty == 0)//Validar la cantidad
      {
        strMsj += "Please specify the quantity. \n";
        tbiGeneral.IsSelected = true;
        tbiGeneral.UpdateLayout();
        txtgiQty.Focus();
      }

      if (chkgiMonetary.IsChecked == true && gift.giAmount == 0)//Validar el Amount
      {
        strMsj += "Monetary gifts should specify the amount. \n";
        tbiGeneral.IsSelected = true;
        tbiGeneral.UpdateLayout();
        txtgiAmount.Focus();
      }

      if (dgrLocations.Items.Count - 1 == 0)//Que tenga mínimo una locacion
      {
        strMsj += "Specify at least one Location. \n";
        tbiItems.IsSelected = true;
        tbiItems.UpdateLayout();
        dgrLocations.Focus();
      }

      if (gift.giPack == true && chkgiPack.IsEnabled == true)//Que tenga minimo un gift
      {
        if (dgrGiftInPack.Items.Count - 1 == 0)
        {
          strMsj += "Specify at least one Gift. \n";
          tbiItems.IsSelected = true;
          tbiItems.UpdateLayout();
          dgrGiftInPack.Focus();
        }
      }
      return strMsj.TrimEnd('\n');
    }

    #endregion

    #region CalculatePricesPackage
    /// <summary>
    /// Calcula los precios
    /// </summary>
    /// <param name="lstGiftsPack">Lista para calcular los precios</param>
    /// <history>
    /// [emoguel] created 18/07/2016
    /// </history>
    private void CalculatePricesPackage(List<GiftPackageItem> lstGiftsPack)
    {
      if (gift.giPack && ChangedGifts(lstGiftsPack))
      {
        MessageBoxResult msgResult = UIHelper.ShowMessage("The gifts of package have changed. \n Would you like to calculate the prices of package?", MessageBoxImage.Question, "Gift");

        if (msgResult == MessageBoxResult.Yes)
        {
          gift.giPrice1 = 0;
          gift.giPrice2 = 0;
          gift.giPrice3 = 0;
          gift.giPrice4 = 0;
          gift.giPublicPrice = 0;
          gift.giPriceExtraAdult = 0;
          gift.giPriceMinor = 0;
          lstGiftsPack.ForEach(gp =>
          {
            gift.giPrice1 += (gp.GiftItem.giPrice1 * gp.gpQty);
            gift.giPrice2 += (gp.GiftItem.giPrice2 * gp.gpQty);
            gift.giPrice3 += (gp.GiftItem.giPrice2 * gp.gpQty);
            gift.giPrice4 += (gp.GiftItem.giPrice2 * gp.gpQty);
            gift.giPublicPrice += (gp.GiftItem.giPublicPrice * gp.gpQty);
            gift.giPriceExtraAdult += (gp.GiftItem.giPriceExtraAdult * gp.gpQty);
            gift.giPriceMinor += (gp.GiftItem.giPriceMinor * gp.gpQty);
          });
        }

        #region Actualizar Campos
        List<TextBox> lstPrices = UIHelper.GetChildParentCollection<TextBox>(grdCost);
        lstPrices.ForEach(txt =>
        {
          var bindingExpresion = txt.GetBindingExpression(TextBox.TextProperty);
          bindingExpresion.UpdateTarget();
        });
        #endregion
      }
    }
    #endregion

    #region ChangedGifts
    /// <summary>
    /// Valida si hubo algun cambio en la lista de giftsItems
    /// </summary>
    /// <param name="lstGiftsPack">Lista nueva a comparar con la lista anterior</param>
    /// <returns>True. Si hubo cambios | False. No hubo cambios</returns>
    /// <history>
    /// [emoguel] created 20/07/2016
    /// </history>
    private bool ChangedGifts(List<GiftPackageItem> lstGiftsPack)
    {
      var lstGiftChanges = (lstGiftsPack != null) ? lstGiftsPack.Where(gp => !_lstOldPacks.Any(gpp => gpp.gpgi == gp.gpgi && gpp.gpQty == gp.gpQty)).ToList() : new List<GiftPackageItem>();
      bool blnChanges = false;
      if (lstGiftsPack != null)
      {
        blnChanges = !(_lstOldPacks.Count == lstGiftsPack.Count);
      }
      return blnChanges || lstGiftChanges.Count > 0;
    }
    #endregion

    #endregion

  }
}
