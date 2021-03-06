﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using System;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSaleTypeDetail.xaml
  /// </summary>
  public partial class frmSaleTypeDetail : Window
  {
    #region Variables
    public SaleType saleType = new SaleType();//Objeto a guardar
    public SaleType oldSaleType = new SaleType();//Objeto con los datos iniciales 
    public EnumMode enumMode;//Modo de la ventana
    public int nStatus = -1;//Estatus para el modo search
    private bool _isClosing = false;
    #endregion
    public frmSaleTypeDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// CArga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(saleType, oldSaleType);
      UIHelper.SetUpControls(saleType, this,enumMode);
      txtstID.IsEnabled = !(enumMode == EnumMode.Edit);
      DataContext = saleType;
      if(enumMode==EnumMode.Search)
      {
        ComboBoxHelper.LoadComboDefault(cmbStatus);
        Title = "Search";
        cmbStatus.SelectedValue = nStatus;
        chkstA.Visibility = Visibility.Collapsed;
        chkstUpdate.Visibility = Visibility.Collapsed;
        cmbStatus.Visibility = Visibility.Visible;
        lblStatus.Visibility = Visibility.Visible;
      }
      LoadSaleTypeCategories();
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
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
    /// Guarda|Actualiza un registro en el catalogo Sale Type
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/04/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.Search)
        {
          if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(saleType, oldSaleType))
          {
            _isClosing = true;
            Close();
          }
          else
          {
            string strMsj = ValidateHelper.ValidateForm(this, "Sale Type");
            if (strMsj == "")
            {
              int nRes =await BREntities.OperationEntity(saleType, enumMode);
              UIHelper.ShowMessageResult("Sale Type", nRes);
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
        }
        else
        {
          _isClosing = true;
          nStatus = Convert.ToInt32(cmbStatus.SelectedValue);
          DialogResult = true;
          Close();
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
    /// Cierra la ventana verificando los cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();
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
        if (enumMode != EnumMode.Search)
        {
          if (!ObjectHelper.IsEquals(saleType, oldSaleType))
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
    #region LoadSaleTypeCategories
    /// <summary>
    /// Carga el combobox de saleTypeCategories
    /// </summary>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// </history>
    private async void LoadSaleTypeCategories()
    {
      try
      {
        List<SaleTypeCategory> lstSaleTypeCategories = await BRSaleTypesCategories.GetSaleCategories(1);
        if (enumMode == EnumMode.Search && lstSaleTypeCategories.Count > 0)
        {
          lstSaleTypeCategories.Insert(0, new SaleTypeCategory { stcID = "", stcN = "ALL" });
        }
        cmbststc.ItemsSource = lstSaleTypeCategories;
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
