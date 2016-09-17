using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Extensions;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmLanguages.xaml
  /// </summary>
  public partial class frmLanguages : Window
  {
    private Language _languageFilter = new Language();//Objeto con los filtros de la ventana
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//boleano para saber si se tiene permiso para editar|Agregar
    public frmLanguages()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window KeyDown
    /// <summary>
    /// Verifica botones presionados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/03/2016
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

    #region Window loaded
    /// <summary>
    /// Carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.ReadOnly);
      btnAdd.IsEnabled = _blnEdit;
      LoadLanguages();
    }
    #endregion

    #region IsKeyboardFocuChanged
    /// <summary>
    /// Verifica que botones fueron presionados con la ventana inactiva
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana de detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmLanguageDetail frmlangDetail = new frmLanguageDetail();
      frmlangDetail.Owner = this;
      frmlangDetail.enumMode = EnumMode.Add;
      if(frmlangDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmlangDetail.language))//verificamos si cumple con los filtros
        {
          int nIndex = 0;
          List<Language> lstLanguages = (List<Language>)dgrLanguages.ItemsSource;
          lstLanguages.Add(frmlangDetail.language);//Agregamos el registro a la lista
          lstLanguages.Sort((x, y) => string.Compare(x.laID, y.laID));//ordenamos la lista
          nIndex = lstLanguages.IndexOf(frmlangDetail.language);//Buscamos el index del nuevo registro
          dgrLanguages.Items.Refresh();//Actualizamos la vista del grid
          GridHelper.SelectRow(dgrLanguages, nIndex);//Seleccionamos el registro nuevo
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
    /// [emoguel] created 31/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch=new frmSearch();
      frmSearch.Owner = this;
      frmSearch.nStatus = _nStatus;
      frmSearch.strID = _languageFilter.laID;
      frmSearch.strDesc = _languageFilter.laN;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _languageFilter.laID = frmSearch.strID;
        _languageFilter.laN = frmSearch.strDesc;
        LoadLanguages();
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 31/03/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Language language = (Language)dgrLanguages.SelectedItem;
      LoadLanguages(language);
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

    #region DoubleClick cell
    /// <summary>
    /// Muestra la ventana detalle en modo ReadOnly|edicion
    /// </summary>
    /// <history>
    /// [emoguel] 30/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Language language = (Language)dgrLanguages.SelectedItem;
      frmLanguageDetail frmlangDetail = new frmLanguageDetail();
      frmlangDetail.Owner = this;
      frmlangDetail.oldLanguage = language;
      frmlangDetail.enumMode = ((_blnEdit==true)?EnumMode.Edit:EnumMode.ReadOnly);
      if(frmlangDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Language> lstLanguages = (List<Language>)dgrLanguages.ItemsSource;
        if(!ValidateFilter(frmlangDetail.language))//Verificamos si cumple con los filtros actuales
        {
          lstLanguages.Remove(language);//Lo quitamos de la lista
        }
        else
        {
          ObjectHelper.CopyProperties(language, frmlangDetail.language);//Actualizamos sus propiedades
          lstLanguages.Sort((x, y) => string.Compare(x.laN, y.laN));//Ordenamos la lista
          nIndex = lstLanguages.IndexOf(language);//obtenemos el index
        }
        dgrLanguages.Items.Refresh();//Actualizamos la vista del grid
        GridHelper.SelectRow(dgrLanguages, nIndex);
      }
    }

    #endregion
    #endregion

    #region methods
    #region LoadLanguages
    /// <summary>
    /// llena el grid de Languages
    /// </summary>
    /// <param name="nIndex"></param>
    /// <param name="language"></param>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    private async void LoadLanguages( Language language = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Language> lstLanguages = await BRLanguages.GetLanguages(_languageFilter, _nStatus);
        dgrLanguages.ItemsSource = lstLanguages;
        if (language != null && lstLanguages.Count > 0)
        {
          language = lstLanguages.Where(la => la.laID == language.laID).FirstOrDefault();
          nIndex = lstLanguages.IndexOf(language);
        }
        GridHelper.SelectRow(dgrLanguages, nIndex);
        StatusBarReg.Content = lstLanguages.Count + " Languages.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error);
      }
    }
    #endregion

    #region ValidateFilters
    /// <summary>
    /// COmpara un objeto languaje cumpla con los filtros establecidos
    /// </summary>
    /// <param name="language">Objeto a verificar</param>
    /// <returns>true. Si cumple | false No cumple</returns>
    /// <history>
    /// [emoguel] created 31/03/2016
    /// </history>
    private bool ValidateFilter(Language language)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(Convert.ToBoolean(_nStatus)!=language.laA)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_languageFilter.laID))//Filtro por ID
      {
        if(language.laID!=_languageFilter.laID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_languageFilter.laN))//Filtro por Descripcion
      {
        if(!language.laN.Contains(_languageFilter.laN,StringComparison.OrdinalIgnoreCase))
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
