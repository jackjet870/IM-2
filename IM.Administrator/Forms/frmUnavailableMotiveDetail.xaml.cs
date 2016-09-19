using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmUnavailableMotiveDetail.xaml
  /// </summary>
  public partial class frmUnavailableMotiveDetail : Window
  {
    #region Variables
    public UnavailableMotive unavailableMotive = new UnavailableMotive();//Objeto a guardar
    public UnavailableMotive oldUnavailableMotive = new UnavailableMotive();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    private List<Agency> _lstOldAgencies = new List<Agency>();//lista inicial de agencies
    private List<Country> _lstOldCountries = new List<Country>();//Lista inicial de countries
    private List<Contract> _lstOldContracts = new List<Contract>();//Lista inicial de contracts
    private bool _isCellCancel = false;
    private bool _isClosing = false;
    #endregion

    public frmUnavailableMotiveDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(unavailableMotive, oldUnavailableMotive);
      UIHelper.SetUpControls(unavailableMotive, this);
      if(enumMode!=EnumMode.ReadOnly)
      {
        txtumID.IsEnabled = (enumMode == EnumMode.Add);
        txtumN.IsEnabled = true;
        chkumA.IsEnabled = true;
        dgrAgencies.IsReadOnly = false;
        dgrContracts.IsReadOnly = false;
        dgrCountries.IsReadOnly = false;
        LoadAgencies();
        LoadContracs();
        LoadCountries();
        dgrAgencies.BeginningEdit += GridHelper.dgr_BeginningEdit;
        dgrContracts.BeginningEdit += GridHelper.dgr_BeginningEdit;
        dgrCountries.BeginningEdit += GridHelper.dgr_BeginningEdit;
      }
      DataContext = unavailableMotive;
    }
    #endregion

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing && enumMode != EnumMode.ReadOnly)
      {
        btnCancel.Focus();
        List<Agency> lstAgencies = new List<Agency>();
        List<Country> lstCountries = (List<Country>)dgrCountries.ItemsSource;
        List<Contract> lstContracts = (List<Contract>)dgrContracts.ItemsSource;
        if (!ObjectHelper.IsEquals(unavailableMotive, oldUnavailableMotive) || !ObjectHelper.IsListEquals(lstAgencies, _lstOldAgencies) || !ObjectHelper.IsListEquals(lstContracts, _lstOldContracts) || !ObjectHelper.IsListEquals(lstCountries, _lstOldCountries))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrAgencies.CancelEdit();
            dgrContracts.CancelEdit();
            dgrCountries.CancelEdit();
          }
        }
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda los cambios de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<Agency> lstAgenciesAdd = new List<Agency>();
        List<Country> lstCountriesAdd = new List<Country>();
        List<Contract> lstContractsAdd = new List<Contract>();
        getNewItems(ref lstAgenciesAdd, ref lstCountriesAdd, ref lstContractsAdd);
        if (ObjectHelper.IsEquals(unavailableMotive, oldUnavailableMotive) && lstAgenciesAdd.Count == 0 && lstContractsAdd.Count == 0 && lstCountriesAdd.Count == 0)
        {
          _isClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          btnAccept.Visibility = Visibility.Collapsed;
          string strMsj = ValidateHelper.ValidateForm(this, "Unavailable Motives", blnDatagrids: true);
          if (unavailableMotive.umID == 0)
          {
            strMsj += (strMsj == "") ? "" : " \n " + "ID can not be 0";
          }
          if (strMsj == "")
          {
            int nRes = await BRUnavailableMotives.SaveUnavailableMotives(unavailableMotive, lstAgenciesAdd, lstContractsAdd, lstCountriesAdd, (enumMode == EnumMode.Edit));
            UIHelper.ShowMessageResult("Unavailable Motives", nRes);
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
          skpStatus.Visibility = Visibility.Collapsed;
          btnAccept.Visibility = Visibility.Visible;
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region CellEditEnding
    /// <summary>
    /// No permite agregar registros repetidos
    /// </summary>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    private void dgr_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if(e.EditAction==DataGridEditAction.Commit)
      {
        _isCellCancel = false;
        bool blnIsRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, (DataGrid)sender, true);
        e.Cancel = blnIsRepeat;
      }
      else
      {
        _isCellCancel = true;
      }
    }
    #endregion

    #region RowEditEnding
    /// <summary>
    /// No permite agregar filas vacias
    /// </summary>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    private void dgr_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        DataGrid dgr = sender as DataGrid;
        if (_isCellCancel)
        {
          dgr.RowEditEnding -= dgr_RowEditEnding;
          dgr.CancelEdit();
          dgr.RowEditEnding += dgr_RowEditEnding;
        }
        else
        {
          switch (dgr.Name)
          {
            case "dgrCountries":
              {
                cmbCountries.Header = "Country (" + (dgr.Items.Count - 1) + ")";
                break;
              }
            case "dgrAgencies":
              {
                cmbAgencies.Header = "Agency (" + (dgr.Items.Count - 1) + ")";
                break;
              }
            case "dgrContracts":
              {
                cmbContracts.Header = "Contract (" + (dgr.Items.Count - 1) + ")";
                break;
              }
          }
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
    /// [created] 06/06/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        DataGridRow dr = sender as DataGridRow;
        var item = dr.Item;
        switch(item.GetType().Name)
        {
          case "Agency":
            {
              Agency agency = (Agency)item;
              if(agency.agum==unavailableMotive.umID)
              {
                e.Handled = true;
              }
              else
              {
                cmbAgencies.Header = "Agency (" + (dgrAgencies.Items.Count-2) + ")";
              }
              break;
            }
          case "Country":
            {
              Country country = (Country)item;
              if(country.coum==unavailableMotive.umID)
              {
                e.Handled = true;
              }
              else
              {
                cmbCountries.Header = "Country (" + (dgrCountries.Items.Count - 2) + ")";
              }
              break;
            }
          case "Contract":
            {
              Contract contract = (Contract)item;
              if(contract.cnum==unavailableMotive.umID)
              {
                e.Handled = true;
              }
              else
              {
                cmbContracts.Header = "Contract (" + (dgrContracts.Items.Count - 2) + ")";
              }
              break;
            }
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadAgencies
    /// <summary>
    /// Llena el grid y el combobox de agencies
    /// </summary>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    private async void LoadAgencies()
    {
      try
      {
        List<Agency> lstAllAgencies = await BRAgencies.GetAgencies();
        cmbAgencies.ItemsSource = lstAllAgencies;        
        List<Agency> lstAgencies = (unavailableMotive.umID > 0) ? lstAllAgencies.Where(ag => ag.agum ==unavailableMotive.umID).ToList() : new List<Agency>();
        dgrAgencies.ItemsSource = lstAgencies;
        lstAgencies.ForEach(ag => {
          Agency agency = new Agency();
          ObjectHelper.CopyProperties(agency, ag);
          _lstOldAgencies.Add(agency);
          });         
        cmbAgencies.Header = "Agencies (" + lstAgencies.Count + ")";
                
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadCountries
    /// <summary>
    /// Llena el combobox y el grid de countries
    /// </summary>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    private async void LoadCountries()
    {
      try
      {
        List<Country> lstAllCountries = await BRCountries.GetCountries(null);
        cmbCountries.ItemsSource = lstAllCountries;
        List<Country>lstCountries=(unavailableMotive.umID>0)? lstAllCountries.Where(co=>co.coum==unavailableMotive.umID).ToList():new List<Country>();
        dgrCountries.ItemsSource = lstCountries;
        _lstOldCountries = lstCountries.ToList();
        cmbCountries.Header = "Country (" + lstCountries.Count + ")";
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadContracts
    /// <summary>
    /// Llena el grid y el combobox de contracts
    /// </summary>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    private async void LoadContracs()
    {
      try
      {
        List<Contract> lstAllContracts = await BRContracts.getContracts();
        cmbContracts.ItemsSource = lstAllContracts;
        List<Contract> lstContracts = (unavailableMotive.umID > 0) ? lstAllContracts.Where(cn => cn.cnum == unavailableMotive.umID).ToList() : new List<Contract>();
        dgrContracts.ItemsSource = lstContracts;
        _lstOldContracts = lstContracts.ToList();
        cmbContracts.Header = "Contract (" + lstContracts.Count + ")";
        btnAccept.Visibility = Visibility.Visible;
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region getNewItems
    /// <summary>
    /// Devuelve los registros agregados a los grids nuevos
    /// </summary>
    /// <param name="lstAgencyAdd">Lista de agencias agregadas</param>
    /// <param name="lstCountryAdd">Lista de countries Agregados</param>
    /// <param name="lstContractsAdd">Lista de contratos Agregados</param>
    /// <history>
    /// [emoguel] created 06/06/2016
    /// </history>
    private void getNewItems(ref List<Agency> lstAgencyAdd, ref List<Country> lstCountryAdd, ref List<Contract> lstContractsAdd)
    {
      #region Agencies
      List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
      lstAgencyAdd = lstAgencies.Where(ag => !_lstOldAgencies.Any(agg => agg.agID == ag.agID)).ToList();
      #endregion

      #region Agencies
      List<Country> lstCountries = (List<Country>)dgrCountries.ItemsSource;
      lstCountryAdd = lstCountries.Where(co => !_lstOldCountries.Any(coo => coo.coID == co.coID)).ToList();
      #endregion

      #region Agencies
      List<Contract> lstContracts = (List<Contract>)dgrContracts.ItemsSource;
      lstContractsAdd = lstContracts.Where(cn => !_lstOldContracts.Any(cnn => cnn.cnID == cn.cnID)).ToList();
      #endregion
    } 
    #endregion
    #endregion
  }
}
