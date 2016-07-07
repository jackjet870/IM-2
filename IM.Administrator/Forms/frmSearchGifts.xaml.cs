using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Services.Helpers;
using IM.Base.Helpers;
using IM.Model;
using IM.BusinessRules.BR;
using System.Windows.Controls;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftsSearch.xaml
  /// </summary>
  public partial class frmSearchGifts : Window
  {

    #region variables
    public int nStatus = -1;
    public int nPackage = -1;
    public Gift gift = new Gift();
    #endregion
    public frmSearchGifts()
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
    /// [emoguel] created 01/07/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadProductElectronicPurse();      
      LoadTransacctionTypes();
      LoadPromotionSistur();
      LoadGiftCategories();
      ComboBoxHelper.LoadComboDefault(cmbStatus);
      ComboBoxHelper.LoadComboDefault(cmbPackage);
      cmbStatus.SelectedValue = nStatus;
      cmbPackage.SelectedValue = nPackage;
      #region Select promotion Opera
      switch(gift.giPromotionOpera)
      {
        case "ALL":
          {
            cmbPromotionOpera.SelectedIndex = 0;
            break;
          }
        case "ANY":
          {
            cmbPromotionOpera.SelectedIndex = 1;
            break;
          }
        case "NONE":
          {
            cmbPromotionOpera.SelectedIndex = 2;
            break;
          }
        default:
          {
            cmbPromotionOpera.SelectedIndex = 3;
            ((TextBox)cmbPromotionOpera.SelectedItem).Text = gift.giPromotionOpera;
            break;
          }
      }
      #endregion
      DataContext = gift;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/07/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        Close();
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda los filtros seleccionados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] modified 04/07/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      nStatus = Convert.ToInt32(cmbStatus.SelectedValue);
      nPackage = Convert.ToInt32(cmbPackage.SelectedValue);
      gift.giPromotionOpera = cmbPromotionOpera.Text;
      DialogResult = true;
      Close();
    }
    #endregion
    #endregion

    #region Methods
    #region LoadProductElectronicPurse
    /// <summary>
    /// Llena el combo de Productos del Monedero electronico
    /// </summary>
    /// <history>
    /// [emoguel] modified 04/07/2016
    /// </history>
    private async void LoadProductElectronicPurse()
    {
      try
      {
        var Products = await ClubesHelper.GetProductsBySystem();
        Products.Insert(0, new Services.ClubesService.Product { DESCRIPCION = "ALL", IDPRODUCTO = "ALL" });
        Products.Insert(1, new Services.ClubesService.Product { DESCRIPCION = "ANY", IDPRODUCTO = "ANY" });
        Products.Insert(2, new Services.ClubesService.Product { DESCRIPCION = "NONE", IDPRODUCTO = "NONE" });
        cmbProdElecPurse.ItemsSource = Products;
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
    /// [emoguel] created 04/07/2016
    /// </history>
    private async void LoadPromotionSistur()
    {
      try
      {
        var promotions = await SisturHelper.getPromotionsType("REGALO", "ACTIV");
        promotions.Insert(0, new Services.SisturService.PromocionesTipoResponse { nombre = "ALL", clave = "ALL" });
        promotions.Insert(1, new Services.SisturService.PromocionesTipoResponse { nombre = "ANY", clave = "ANY" });
        promotions.Insert(2, new Services.SisturService.PromocionesTipoResponse { nombre = "NONE", clave = "NONE" });
        cmbPromSistur.ItemsSource = promotions;
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
    /// [emoguel] modified 04/07/2016
    /// </history>
    private async void LoadTransacctionTypes()
    {
      try
      {
        var transacctionTypes = await WirePRHelper.GetTransactionTypes();
        transacctionTypes.Insert(0,new Services.WirePRService.TransactionTypes { Description = "ALL", Trx_code = "ALL" });
        transacctionTypes.Insert(1,new Services.WirePRService.TransactionTypes { Description = "ANY", Trx_code = "ANY" });
        transacctionTypes.Insert(2,new Services.WirePRService.TransactionTypes { Description = "NONE", Trx_code = "NONE" });
        cmbTranTypes.ItemsSource = transacctionTypes;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Gifts");
      }
    }
    #endregion

    #region LoadGiftCategories
    /// <summary>
    /// Llena el combo de categories
    /// </summary>
    /// <history>
    /// [emoguel] created 05/07/2016
    /// </history>
    private async void LoadGiftCategories()
    {
      try
      {
        List<GiftCategory> lstCategories= await BRGiftsCategories.GetGiftsCategories(nStatus: 1);
        lstCategories.Insert(0,new GiftCategory { gcID = "", gcN = "ALL" });
        cmbGiftsCategory.ItemsSource = lstCategories;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "LoadGiftCategories");
      }
    } 
    #endregion
    #endregion
  }
}
