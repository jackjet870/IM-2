using System.Collections.Generic;
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
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(club, oldClub);
      UIHelper.SetUpControls(club, this);
      DataContext = club;
      LoadAgencies(club.clID);
      txtclID.IsEnabled = (enumMode == EnumMode.add);
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
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
        List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
        if (enumMode != EnumMode.add && ObjectHelper.IsEquals(club, oldClub) && ObjectHelper.IsListEquals(_oldLstAgencies, lstAgencies))
        {
          blnClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          string strMsj = ValidateHelper.ValidateForm(this, "Club");
          if (club.clID == 0)
          {
            strMsj += (strMsj == "") ? "" : " \n " + "The Club ID can not be 0.";
          }
          if (strMsj == "")
          {
            List<Agency> lstAdd = lstAgencies.Where(ag => !_oldLstAgencies.Any(agg => agg.agID == ag.agID)).ToList();
            List<Agency> lstDel = _oldLstAgencies.Where(ag => !lstAgencies.Any(agg => agg.agID == ag.agID)).ToList();
            int nRes = await BRClubs.SaveClub(club, (enumMode == EnumMode.edit), lstAdd, lstDel);
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
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Clubs");
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
    /// [emoguel] created 03/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
      if (!ObjectHelper.IsEquals(club, oldClub) || !ObjectHelper.IsListEquals(lstAgencies, _oldLstAgencies))
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
    #endregion

    #region EndEdit
    /// <summary>
    /// Valida que el Sales Room no esté seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    private void dgrAgencies_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrAgencies);
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
    /// No repite registros vacios
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
        dgrAgencies.ItemsSource = lstAgencies;
        cmbAgencies.ItemsSource = lstAllAgencies;
        _oldLstAgencies = lstAgencies.ToList();
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Clubs");
      }
    }
    #endregion

    #endregion
  }
}
