﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmMarketDetail.xaml
  /// </summary>
  public partial class frmMarketDetail : Window
  {
    #region Variables
    public Market market = new Market();//Objeto a guardar
    public Market oldMarket = new Market();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    private List<Agency> _oldLstAgencies = new List<Agency>();//Lista inicial de agencies
    bool blnClosing = false;
    bool isCellCancel = false;
    #endregion
    public frmMarketDetail()
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
    /// [emoguel] created 18/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(market, oldMarket);
      UIHelper.SetUpControls(market, this);
      LoadAgencies();
      if(enumMode!=EnumMode.preview)
      {
        txtmkID.IsEnabled = (enumMode == EnumMode.add);
        txtmkN.IsEnabled = true;
        chkmkA.IsEnabled = true;
        dgrAgencies.IsReadOnly = false;
        btnAccept.Visibility = Visibility.Visible;
      }
      DataContext = market;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/05/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region dgrAgencies_CellEditEnding
    /// <summary>
    /// Verifica que un agency no se repita
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/05/2016
    /// </history>
    private void dgrAgencies_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        isCellCancel = false;
        bool isRepeat=GridHelper.HasRepeatItem((Control)e.EditingElement, dgrAgencies);        
        e.Cancel = isRepeat;
      }
      else
      {
        isCellCancel = true;
      }
    }
    #endregion    

    #region dgrAgencies_RowEditEnding
    /// <summary>
    /// RownEdingEdit
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void dgrAgencies_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {     
      if (isCellCancel)
      {
        dgrAgencies.RowEditEnding -= dgrAgencies_RowEditEnding;
        dgrAgencies.CancelEdit();
        dgrAgencies.RowEditEnding += dgrAgencies_RowEditEnding;
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
    /// [emoguel] created 18/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if(enumMode!=EnumMode.preview)
      {
        List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
        if (!ObjectHelper.IsEquals(market, oldMarket) || !ObjectHelper.IsListEquals(lstAgencies, _oldLstAgencies))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {            
            if (!blnClosing) { blnClosing = true; Close(); }
          }
          else
          {
            blnClosing = false;
          }
        }
        else
        {
          if (!blnClosing) { blnClosing = true; Close(); }
        }
      }
      else
      {
        if (!blnClosing) { blnClosing = true; Close(); }
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza registros en el catalogo Markets
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/05/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
      if (enumMode != EnumMode.add && ObjectHelper.IsEquals(market, oldMarket) && ObjectHelper.IsListEquals(lstAgencies, _oldLstAgencies))
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Market");
        if (strMsj == "")
        {
          List<Agency> lstAdd = lstAgencies.Where(ag => !_oldLstAgencies.Any(agg => agg.agID == ag.agID)).ToList();          
          int nRes = BRMarkets.SaveMarket(market,lstAdd,(enumMode==EnumMode.edit));
          UIHelper.ShowMessageResult("Market", nRes);
          if (nRes > 0)
          {
            blnClosing = true;
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
    #endregion

    #region Row KeyDown
    /// <summary>
    /// Elimina registros nuevos con el boton suprimir
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [created] 07/05/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        var item = dgrAgencies.SelectedItem;
        if (item.GetType().Name == "Agency")
        {
          Agency agencySelec = (Agency)item;
          if (agencySelec.agmk == market.mkID)
          {
            e.Handled = true;
          }
        }
      }
    }

    #endregion

    #region Closing
    /// <summary>
    /// Se ejecuta cuando se está cerrando la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing)
      {
        blnClosing = true;
        btnCancel_Click(null, null);
        if (!blnClosing)
        {
          e.Cancel = true;
        }
      }
    }
    #endregion

    #endregion

    #region Methods
    #region LoadAgencies
    /// <summary>
    /// Llena el combobox y el grid de Agencies
    /// </summary>
    /// <history>
    /// [emoguel] created 18/05/2016
    /// </history>
    private async void  LoadAgencies()
    {
      List<Agency> lstAllAgencies = await BRAgencies.GetAgencies();
      cmbAgencies.ItemsSource = lstAllAgencies;
      List<Agency> lstAgencies = (!string.IsNullOrWhiteSpace(market.mkID)) ? lstAllAgencies.Where(ag => ag.agmk == market.mkID).ToList() : new List<Agency>();
      dgrAgencies.ItemsSource = lstAgencies;
      _oldLstAgencies = lstAgencies.ToList();
      cmbAgencies.Header=("Agency ("+lstAgencies.Count+")");
    }
    #endregion

    #endregion
  }
}