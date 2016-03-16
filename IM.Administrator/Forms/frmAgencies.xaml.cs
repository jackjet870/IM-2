﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Administrator.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAgencies.xaml
  /// </summary>
  public partial class frmAgencies : Window
  {
    private Agency _agencyFilter=new Agency();//Objeto para filtrar en la BD
    private int _nStatus = -1;//Status a filtrar en la lista
    private bool _blnEdit = false;//boleano para saber si se tiene minimo permiso para editar|agregar 

    public frmAgencies()
    {
      InitializeComponent();
    }

    #region event controls
    #region Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Agencies, EnumPermisionLevel.Standard);
      LoadAgencies();
      btnAdd.IsEnabled = _blnEdit;
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/03/2016
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
    /// [emoguel] created 08/03/2016
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
    #region refresh
    /// <summary>
    /// Recarga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadAgencies();
    }
    #endregion

    #region Boton agregar
    /// <summary>
    /// Abre la ventana detalle en modo agregar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmAgencyDetail frmAgencyDetail = new frmAgencyDetail();
      frmAgencyDetail.mode = ModeOpen.add;//Insertar
      frmAgencyDetail.Owner = this;

      if(frmAgencyDetail.ShowDialog()==true)
      {
        List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;//cateamos el itemsource
        lstAgencies.Add(frmAgencyDetail.agency);//Agregamos el registro nuevo
        lstAgencies.Sort((x, y) => string.Compare(x.agN, y.agN));//ordenamos la lista
        int nIndex = lstAgencies.IndexOf(frmAgencyDetail.agency);//obtenemos el index del registro nuevo        
        dgrAgencies.Items.Refresh();//Refrescamos la lista
        GridHelper.SelectRow(dgrAgencies, nIndex);
        StatusBarReg.Content = lstAgencies.Count+" Agencies.";//Actualizamos el contador
      }     
    }
    #endregion
    
    #region Boton Buscar

    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.sID = _agencyFilter.agID;
      frmSearch.sDesc = _agencyFilter.agN;
      frmSearch.nStatus = _nStatus;
      frmSearch.sSegment = _agencyFilter.agse;
      frmSearch.sForm = "Agency";
      frmSearch.Owner = this;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _agencyFilter.agID = frmSearch.sID;
        _agencyFilter.agN = frmSearch.sDesc;
        _agencyFilter.agse = frmSearch.sSegment;
        LoadAgencies();
      }
    }
    #endregion
    #region DobleClic Grid
    /// <summary>
    /// Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Agency agency = (Agency)dgrAgencies.SelectedItem;      
      frmAgencyDetail frmAgencyDetail = new frmAgencyDetail();
      ObjectHelper.CopyProperties(frmAgencyDetail.agency,agency); 
      frmAgencyDetail.Owner = this;
      frmAgencyDetail.mode = ((_blnEdit == true) ? ModeOpen.edit : ModeOpen.preview);
      if(frmAgencyDetail.ShowDialog()==true)
      {
        ObjectHelper.CopyProperties(agency, frmAgencyDetail.agency);        
      }
    }
    #endregion
    #endregion
    #region métodos
    #region Load Agencies
    /// <summary>
    /// Llena el grid de Agencies dependiendo de los filtros seleccionados
    /// </summary>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    protected void LoadAgencies()
    {
      List<Agency> lstAgencies = BRAgencies.GetAgencies(_agencyFilter, _nStatus);
      dgrAgencies.ItemsSource = lstAgencies;
      if (lstAgencies.Count > 0)
      {        
        dgrAgencies.Focus();
        GridHelper.SelectRow(dgrAgencies,0);                
      }
      StatusBarReg.Content = lstAgencies.Count + " Agencies.";
    }
    #endregion

    #endregion
    
  }
}
