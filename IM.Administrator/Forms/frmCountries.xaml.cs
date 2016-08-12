using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using System.Linq;
using System;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCountries.xaml
  /// </summary>
  public partial class frmCountries : Window
  {
    private Country _countryFilter = new Country();//Objeto con los filtros del grid
    private int _nStatus = -1;//variable con el estatus de los registros del grid
    private bool _blnEdit = false;//Variable para saber si se tiene permiso para editar y 
    public frmCountries()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region WindowLoaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Agencies, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      loadCountries();
    }
    #endregion
    #region KeyboardFocusChanged
    /// <summary>
    /// Verifica las teclas INS|LockNum|Mayus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [created] 14/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Verifica las teclas que fueron presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
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
    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strDesc = _countryFilter.coN;
      frmSearch.strID = _countryFilter.coID;
      frmSearch.nStatus = _nStatus;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _countryFilter.coID = frmSearch.strID;
        _countryFilter.coN = frmSearch.strDesc;
        loadCountries();

      }
    }

    #endregion
    #region Add
    /// <summary>
    /// Abre la ventana de detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {

      frmCountryDetail frmCountryDetail = new frmCountryDetail();
      frmCountryDetail.mode = EnumMode.Add;
      frmCountryDetail.Owner = this;
      if (frmCountryDetail.ShowDialog() == true)
      {
        if (ValidateFilters(frmCountryDetail.country))//Validamos que cumpla con los filtros actuales
        {
          List<Country> lstCountry = (List<Country>)dgrCountries.ItemsSource;
          lstCountry.Add(frmCountryDetail.country);//Agregamos el nuevo registro
          lstCountry.Sort((x, y) => string.Compare(x.coN, y.coN));//Ordenamos la lista
          int nIndex = lstCountry.IndexOf(frmCountryDetail.country);//Buscamos el index del nuevo registro
          dgrCountries.Items.Refresh();//refrescamos la lista
          GridHelper.SelectRow(dgrCountries, nIndex);//Seleccionamos el registro en la lista    
          StatusBarReg.Content = lstCountry.Count + " Countries";
        }
      }
    }
    #endregion
    #region Refresh
    /// <summary>
    /// Recarga la lista de countries
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Country country = (Country)dgrCountries.SelectedItem;
      loadCountries( country);
    }
    #endregion
    #region Double Click

    /// <summary>
    /// Abre la ventana de detalle en modo edicion o detalle dependiendo de los permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Country country = (Country)dgrCountries.SelectedItem;
      frmCountryDetail frmCountryDetail = new frmCountryDetail();
      frmCountryDetail.Owner = this;
      frmCountryDetail.oldCountry = country;
      frmCountryDetail.mode = ((_blnEdit == true) ? EnumMode.Edit : EnumMode.ReadOnly);
      if (frmCountryDetail.ShowDialog() == true)
      {
        List<Country> lstCountry = (List<Country>)dgrCountries.ItemsSource;
        int nIndex = 0;
        if (!ValidateFilters(frmCountryDetail.country))
        {
          lstCountry.Remove(country);//Quitamos el registro de la lista
        }
        else
        {
          ObjectHelper.CopyProperties(country, frmCountryDetail.country);
          lstCountry.Sort((x, y) => string.Compare(x.coN, y.coN));//Ordenamos la lista
          nIndex = lstCountry.IndexOf(country);
        }
        dgrCountries.Items.Refresh();//refrescamos la lista
        GridHelper.SelectRow(dgrCountries, nIndex);//Seleccionamos el registro en la lista 
        StatusBarReg.Content = lstCountry.Count + " Countries";//Actualizamos el contador
      }
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    #endregion

    #region Métodos

    #region LoadCountries
    /// <summary>
    /// Llena el grid de countries
    /// </summary>
    /// <history>
    /// [Emoguel] created 14/03/2016
    /// </history>
    private async void loadCountries(Country country=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Country> lstCountries = await BRCountries.GetCountries(_countryFilter, _nStatus);
        dgrCountries.ItemsSource = lstCountries;
        if (country != null && lstCountries.Count > 0)
        {
          country = lstCountries.Where(cu => cu.coID == country.coID).FirstOrDefault();
          nIndex = lstCountries.IndexOf(country);
        }
        GridHelper.SelectRow(dgrCountries, nIndex);
        StatusBarReg.Content = lstCountries.Count + " Countries.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Countries");
      }
    }
    #endregion

    #region Validate Filters
    /// <summary>
    /// Valida si una entidad de tipo Country coincide con los filtros
    /// </summary>
    /// <param name="newCountry">Objeto a validar</param>
    /// <returns>true. Si se muestra | false. Nose muestra</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private bool ValidateFilters(Country newCountry)
    {
      if (_nStatus != -1)
      {
        if (newCountry.coA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_countryFilter.coID))
      {
        if (_countryFilter.coID != newCountry.coID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_countryFilter.coN))
      {
        if (!newCountry.coN.Contains(_countryFilter.coN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }

      return true;
    }
    #endregion
    #endregion
  }
}
