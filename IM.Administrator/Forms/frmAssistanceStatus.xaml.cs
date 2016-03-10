﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAssistanceStatus.xaml
  /// </summary>
  public partial class frmAssistanceStatus : Window
  {
    private AssistanceStatus _assistanceFilter = new AssistanceStatus();//Objeto a filtrar en el grid
    private int _nStatus = -1;//Estatus de los registros que se muestran en el grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permisos para editar|agregar
    public frmAssistanceStatus()
    {
      InitializeComponent();
    }

    #region eventos de los controles
    #region Loaded form
    /// <summary>
    /// carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Helpers.PermisionHelper.EditPermision("SALES");
      btnAdd.IsEnabled = _blnEdit;
      LoadAssitance();      
    }

    #endregion

    #region KeyBoardChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 09/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region KeyDown Form
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


    #region DoubleClick cell
    /// <summary>
    /// Muestra la ventana de AssistanceStatusDetail en modo preview
    /// </summary>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      DataGridRow row = sender as DataGridRow;
      AssistanceStatus Assistance = (AssistanceStatus)row.DataContext;
      frmAssistanceStatusDetail frmAssistanceDetail = new frmAssistanceStatusDetail();
      frmAssistanceDetail.assistance = Assistance;
      frmAssistanceDetail.Owner = this;
      frmAssistanceDetail.mode = ((_blnEdit == true) ? ModeOpen.edit : ModeOpen.preview);
      frmAssistanceDetail.ShowDialog();

    }

    #endregion

    #region Refresh grid
    /// <summary>
    /// Actualiza la lista de Status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created.
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadAssitance();
    }
    #endregion

    #region Add
    /// <summary>
    /// Muestra la ventana de AssistanceStatusDetail para agregar un registro nuevo
    /// </summary>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {

      frmAssistanceStatusDetail frmAssistanceDetail = new frmAssistanceStatusDetail();
      frmAssistanceDetail.assistance = new AssistanceStatus();
      frmAssistanceDetail.Owner = this;
      frmAssistanceDetail.mode = ModeOpen.add;//insertar
      if (frmAssistanceDetail.ShowDialog() == true)
      {
        LoadAssitance();
      }

    }

    #endregion
    
    #region Search
    /// <summary>
    /// Abre la ventana de busqueda cargando 
    /// los datos de busqueda que ya se hayan realizado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 3/Mar/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.nStatus = _nStatus;
      frmSearch.sID = _assistanceFilter.atID;
      frmSearch.sDesc = _assistanceFilter.atN;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _assistanceFilter.atID = frmSearch.sID;
        _assistanceFilter.atN = frmSearch.sDesc;

        LoadAssitance();
      }
    }
    #endregion
    #endregion

    #region metodos
    /// <summary>
    /// carga la lista de Assistance Status
    /// </summary>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    #region Load Assistance
    protected void LoadAssitance()
    {
      List<AssistanceStatus> lstAssistance = BRAssistancesStatus.GetAssitanceStatus(_assistanceFilter, _nStatus);
      dgrAssitance.ItemsSource = lstAssistance;
      if (lstAssistance.Count > 0)
      {        
        dgrAssitance.SelectedIndex = 0;
      }
      StatusBarReg.Content = lstAssistance.Count + " Assistance Status.";
    }
    #endregion

    #endregion
    
  }
}