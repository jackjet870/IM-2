﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmClubDetail.xaml
  /// </summary>
  public partial class frmClubDetail : Window
  {
    #region Variables
    public Club club = new Club();//Objeto a guardar
    public Club oldClub = new Club();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    private List<Agency> _oldLstAgencies = new List<Agency>();//Agencies iniciales
    private bool blnClosing = false;
    private bool isCellCancel = false;
    #endregion
    public frmClubDetail()
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
    /// [emoguel] created 03/05/2016
    /// [emoguel] modified 29/07/2016---->Se agrego la suscripcion al evento begin edit
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(club, oldClub);
      UIHelper.SetUpControls(club, this);
      DataContext = club;
      LoadAgencies(club.clID);
      txtclID.IsEnabled = (enumMode == EnumMode.Add);
      dtgAgencies.BeginningEdit += GridHelper.dgr_BeginningEdit;
      dtgAgencies.CurrentCellChanged += GridHelper.dtg_CurrentCellChanged;
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza un registro en los catalogos Banks y Agencies
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<Agency> lstAgencies = (List<Agency>)dtgAgencies.ItemsSource;
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(club, oldClub) && ObjectHelper.IsListEquals(_oldLstAgencies, lstAgencies))
        {
          blnClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          btnAccept.Visibility = Visibility.Collapsed;
          string strMsj = ValidateHelper.ValidateForm(this, "Club", blnDatagrids: true);
          if (club.clID == 0)
          {
            strMsj += (strMsj == "") ? "" : " \n " + "The Club ID can not be 0.";
          }
          if (strMsj == "")
          {
            List<Agency> lstAdd = lstAgencies.Where(ag => !_oldLstAgencies.Any(agg => agg.agID == ag.agID)).ToList();
            List<Agency> lstDel = _oldLstAgencies.Where(ag => !lstAgencies.Any(agg => agg.agID == ag.agID)).ToList();
            int nRes = await BRClubs.SaveClub(club, (enumMode == EnumMode.Edit), lstAdd, lstDel);
            UIHelper.ShowMessageResult("Club", nRes);
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

    #region EndEdit
    /// <summary>
    /// Valida que el Sales Room no esté seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// [emoguel] modified se agregó la validación de editAction
    /// </history>
    private void dtgAgencies_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction==DataGridEditAction.Commit)//Verificar si se está cancelando la edición
      {
        isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dtgAgencies);
        e.Cancel = isRepeat;
      }
      else
      {
        isCellCancel = true;
      }

    }
    #endregion

    #region dtgAgencies_RowEditEnding
    /// <summary>
    /// No repite registros vacios
    /// </summary>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// [emoguel] modified se agregó la validación de editAction
    /// </history>
    private void dtgAgencies_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if(e.EditAction==DataGridEditAction.Commit && isCellCancel)
      { 
        dtgAgencies.RowEditEnding -= dtgAgencies_RowEditEnding;
        dtgAgencies.CancelEdit();
        dtgAgencies.RowEditEnding += dtgAgencies_RowEditEnding;
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing)
      {
        btnCancel.Focus();
        List<Agency> lstAgencies = (List<Agency>)dtgAgencies.ItemsSource;
        if (!ObjectHelper.IsEquals(club, oldClub) || !ObjectHelper.IsListEquals(lstAgencies, _oldLstAgencies))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dtgAgencies.CancelEdit();
          }
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadAgencies
    /// <summary>
    /// Carga la agencies relacionadas al club
    /// </summary>
    /// <param name="clubId">id del club para buscar las agencies</param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    private async void LoadAgencies(int clubId)
    {
      try
      {
        List<Agency> lstAllAgencies = await BRAgencies.GetAgencies();
        List<Agency> lstAgencies = lstAllAgencies.Where(ag => ag.agcl == clubId).ToList();
        dtgAgencies.ItemsSource = lstAgencies;
        cmbAgencies.ItemsSource = lstAllAgencies;
        _oldLstAgencies = lstAgencies.ToList();        
        btnAccept.Visibility = Visibility.Visible;
        skpStatus.Visibility = Visibility.Hidden;
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
