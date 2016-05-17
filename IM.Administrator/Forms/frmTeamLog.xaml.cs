using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmTeamsLog.xaml
  /// </summary>
  public partial class frmTeamsLog : Window
  {
    #region Variables
    private TeamLog _teamLogFilter = new TeamLog();//Contiene los filtros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar
    private bool _blnDel = false;//Permiso para saber si se tiene permiso para eliminar
    private bool _blnDate = false;//Boleano para saber si va a filtrar por ppDT  
    #endregion
    public frmTeamsLog()
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
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Personnel, EnumPermisionLevel.Special);
      _blnDel = App.User.HasPermission(EnumPermission.Personnel, EnumPermisionLevel.SuperSpecial);
      btnAdd.IsEnabled = _blnEdit;
      btnDel.IsEnabled = _blnDel;
      LoadTeamsLog();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 27/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region KeyDown Form
    /// <summary>
    /// Verfica teclas precionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
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
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
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
    /// Muestra la ventana detalle en modo edit
    /// </summary>
    /// <history>
    /// [emoguel] 27/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      object dTeamLog = dgrTeamsLog.SelectedItem;
      TeamLog teamLog = (TeamLog)dTeamLog.GetType().GetProperty("teamLog").GetValue(dTeamLog, null);
      frmTeamLogDetail frmTeamLogDetail = new frmTeamLogDetail();
      frmTeamLogDetail.Owner = this;
      frmTeamLogDetail.oldTeamLog = teamLog;
      frmTeamLogDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.preview;
      if(frmTeamLogDetail.ShowDialog()==true)
      {
        List<object> lstTeamsLog = (List<object>)dgrTeamsLog.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmTeamLogDetail.teamLog))
        {
          ObjectHelper.CopyProperties(teamLog, frmTeamLogDetail.teamLog);//Actualizamos los datos
          lstTeamsLog.Sort((x, y) => DateTime.Compare(Convert.ToDateTime(y.GetType().GetProperty("tlDT").GetValue(y, null)), Convert.ToDateTime(x.GetType().GetProperty("tlDT").GetValue(x, null))));//Reordenamos la lista
          nIndex = lstTeamsLog.IndexOf(dTeamLog);//Buscamos la posición del registro
        }
        else
        {
          lstTeamsLog.Remove(dTeamLog);//Quitamos el registro
        }
        dgrTeamsLog.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrTeamsLog, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstTeamsLog.Count + " Teams Log.";//Actualizamos el contador
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      object selectedItem = dgrTeamsLog.SelectedItem;
      TeamLog teamLog = (TeamLog)selectedItem.GetType().GetProperty("teamLog").GetValue(selectedItem,null);
      LoadTeamsLog(teamLog);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmTeamLogDetail frmTeamLogDetail = new frmTeamLogDetail();
      frmTeamLogDetail.Owner = this;
      frmTeamLogDetail.enumMode = EnumMode.add;
      if (frmTeamLogDetail.ShowDialog() == true)
      {
        TeamLog teamLog = frmTeamLogDetail.teamLog;
        if (ValidateFilter(teamLog))//Validamos que cumpla con los filtros actuales
        {
          object dTeamLog = BRTeamsLog.GetTeamsLog(teamLog).FirstOrDefault();//Obtenemos el registro nuevo
          List<object> lstTeamsLog = (List<object>)dgrTeamsLog.ItemsSource;
          lstTeamsLog.Add(dTeamLog);//Agregamos el registro
          lstTeamsLog.Sort((x, y) => DateTime.Compare(Convert.ToDateTime(y.GetType().GetProperty("tlDT").GetValue(y, null)), Convert.ToDateTime(x.GetType().GetProperty("tlDT").GetValue(x, null))));//Reordenamos la lista
          int nIndex = lstTeamsLog.IndexOf(dTeamLog);//Obtenemos la posición del registro
          dgrTeamsLog.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrTeamsLog, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstTeamsLog.Count + " Teams Log.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda en modo search
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmTeamLogDetail frmTeamLogDetail = new frmTeamLogDetail();
      frmTeamLogDetail.Owner = this;
      frmTeamLogDetail.enumMode = EnumMode.search;
      frmTeamLogDetail.oldTeamLog = _teamLogFilter;
      frmTeamLogDetail.blnDate = _blnDate;
      if(frmTeamLogDetail.ShowDialog()==true)
      {
        _teamLogFilter = frmTeamLogDetail.teamLog;
        _blnDate = frmTeamLogDetail.blnDate;
        LoadTeamsLog();
      }
    }
    #endregion

    #region Delete
    /// <summary>
    /// Elimina los registros seleccionados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    private void btnDel_Click(object sender, RoutedEventArgs e)
    {
      if (dgrTeamsLog.SelectedItems.Count > 0)
      {
        List<object> lstTeamsLogDel = dgrTeamsLog.SelectedItems.OfType<object>().ToList();
        List<TeamLog> lstTeamL = lstTeamsLogDel.Select(tt => (TeamLog)tt.GetType().GetProperty("teamLog").GetValue(tt, null)).ToList();
        MessageBoxResult msgResult = MessageBoxResult.No;
        #region MessageBox
        if (lstTeamsLogDel.Count == 1)
        {
          msgResult = UIHelper.ShowMessage("Are you sure you want to delete this Team Log?", MessageBoxImage.Question, "Delete");
        }
        else
        {
          msgResult = UIHelper.ShowMessage("Are you sure you want to delete these Teams Log?", MessageBoxImage.Question, "Delete");
        }
        #endregion

        if (msgResult == MessageBoxResult.Yes)
        {
          int nRes = BREntities.OperationEntities<TeamLog>(lstTeamL,EnumMode.deleted);

          if (nRes > 0)
          {
            if (nRes == 1)
            {
              UIHelper.ShowMessage("Team Log was Deleted.");
            }
            else
            {
              UIHelper.ShowMessage("Teams Log were Deleted.");
            }
            List<object> lstTeamstLog = (List<object>)dgrTeamsLog.ItemsSource;
            lstTeamstLog.RemoveAll(tl => lstTeamsLogDel.Contains(tl));
            dgrTeamsLog.Items.Refresh();
          }
        }
      }
      else
      {
        UIHelper.ShowMessage("Please select a Post Log.");
      }
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadTeamsLog
    /// <summary>
    /// Llena el grid de Teams Log
    /// </summary>
    /// <param name="teamLog">Objeto a selecionar</param>
    /// <history>
    /// [emoguel] created 26/04/2016
    /// </history>
    private void LoadTeamsLog(TeamLog teamLog = null)
    {
      int nIndex = 0;
      List<object> lstTeamsLog = BRTeamsLog.GetTeamsLog(_teamLogFilter,_blnDate).ToList();
      dgrTeamsLog.ItemsSource = lstTeamsLog;
      if (lstTeamsLog.Count() > 0 )
      {
        if (teamLog != null)
        {
          dynamic dTeamLog = lstTeamsLog.Where(item => Convert.ToUInt32(item.GetType().GetProperty("tlID").GetValue(item, null)) == teamLog.tlID).FirstOrDefault();
          nIndex = lstTeamsLog.IndexOf(dTeamLog);
        }
        GridHelper.SelectRow(dgrTeamsLog, nIndex);
        btnDel.IsEnabled = _blnDel;
      }
      else
      {
        btnDel.IsEnabled = false;
      }      
      StatusBarReg.Content = lstTeamsLog.Count() + " Teams Log.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un objeto TeamLog cumpla con los filtros actuales
    /// </summary>
    /// <param name="teamLog">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    private bool ValidateFilter(TeamLog teamLog)
    {
      if(_blnDate)//Filtro por fecha
      {
        if(teamLog.tlDT.Date!=_teamLogFilter.tlDT.Date)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_teamLogFilter.tlChangedBy))//Filtro por ChangedBY
      {
        if(teamLog.tlChangedBy!=_teamLogFilter.tlChangedBy)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_teamLogFilter.tlpe))//Filtro por Personel
      {
        if(teamLog.tlpe!=_teamLogFilter.tlpe)
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
