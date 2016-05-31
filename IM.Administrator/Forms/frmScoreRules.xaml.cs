using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmScoreRules.xaml
  /// </summary>
  public partial class frmScoreRules : Window
  {
    #region Variables
    private ScoreRule _scoreRuleFilter = new ScoreRule();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmScoreRules()
    {
      InitializeComponent();
    }

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      status.Visibility = Visibility.Visible;
      LoadScoreRules();
    } 
    #endregion

    #region Methods Form
    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/05/2016
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
    /// [emoguel] created 26/05/2016
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
    /// <history>
    /// [emoguel] created 26/05/2016
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

    #region DobleClic Grid
    /// <summary>
    /// Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      ScoreRule scoreRule = (ScoreRule)dgrScoreRules.SelectedItem;
      frmScoreRuleDetail frmScoreRuleDetail = new frmScoreRuleDetail();
      frmScoreRuleDetail.Owner = this;
      frmScoreRuleDetail.enumMode = EnumMode.edit;
      frmScoreRuleDetail.oldScoreRule = scoreRule;
      if(frmScoreRuleDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<ScoreRule> lstScoreRules = (List<ScoreRule>)dgrScoreRules.ItemsSource;
        if(ValidateFilter(frmScoreRuleDetail.scoreRule))//Verificamos si cumple con los filtros
        {
          ObjectHelper.CopyProperties(scoreRule, frmScoreRuleDetail.scoreRule);//Actualizamos el registro
          lstScoreRules.Sort((x, y) => string.Compare(x.suN, y.suN));//Reordenamos la lista
          nIndex = lstScoreRules.IndexOf(scoreRule);//BUscamos la posición del index
        }
        else
        {
          lstScoreRules.Remove(scoreRule);//Quitamos el registro
        }
        dgrScoreRules.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrScoreRules, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstScoreRules.Count + " Score Rules.";//Actualizamos el contador
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
    /// [emoguel] created 26/05/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = (_scoreRuleFilter.suID > 0) ? _scoreRuleFilter.suID.ToString() : "";
      frmSearch.strDesc = _scoreRuleFilter.suN;
      frmSearch.nStatus = _nStatus;
      frmSearch.enumWindow = EnumWindow.DefaultByte;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _scoreRuleFilter.suID = Convert.ToByte(frmSearch.strID);
        _scoreRuleFilter.suN = frmSearch.strDesc;
        status.Visibility = Visibility.Visible;
        LoadScoreRules();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmScoreRuleDetail frmScoreRuleDetail = new frmScoreRuleDetail();
      frmScoreRuleDetail.Owner = this;
      frmScoreRuleDetail.enumMode = EnumMode.add;
      if(frmScoreRuleDetail.ShowDialog()==true)
      {
        if (ValidateFilter(frmScoreRuleDetail.scoreRule))//Verificamos que cumpla con los filtros
        {
          List<ScoreRule> lstScoreRules = (List<ScoreRule>)dgrScoreRules.ItemsSource;
          lstScoreRules.Add(frmScoreRuleDetail.scoreRule);//Agregamos el registro
          lstScoreRules.Sort((x, y) => string.Compare(x.suN, y.suN));//Ordenamos la lista
          int nIndex = lstScoreRules.IndexOf(frmScoreRuleDetail.scoreRule);//Buscamos la posición del registro
          dgrScoreRules.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrScoreRules, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstScoreRules.Count + " Score Rules.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Racarga los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      status.Visibility = Visibility.Visible;
      ScoreRule scoreRule = (ScoreRule)dgrScoreRules.SelectedItem;
      LoadScoreRules();
    }
    #endregion
    #endregion

    #region Methods
    #region LoadScoreRules
    /// <summary>
    /// Llena el grid de SocreRules
    /// </summary>
    /// <param name="scoreRule">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    private async void LoadScoreRules(ScoreRule scoreRule = null)
    {
      int nIndex = 0;
      List<ScoreRule> lstScoreRules = await BRScoreRules.GetScoreRules(_nStatus, _scoreRuleFilter);
      dgrScoreRules.ItemsSource = lstScoreRules;
      if (lstScoreRules.Count > 0 && scoreRule != null)
      {
        scoreRule = lstScoreRules.Where(su => su.suID == scoreRule.suID).FirstOrDefault();
        nIndex = lstScoreRules.IndexOf(scoreRule);
      }
      GridHelper.SelectRow(dgrScoreRules, nIndex);
      StatusBarReg.Content = lstScoreRules.Count + " Score Rules.";
      status.Visibility = Visibility.Collapsed;
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Verifica que un score rule cumpla con los filtros actuales
    /// </summary>
    /// <param name="scoreRule"></param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private bool ValidateFilter(ScoreRule scoreRule)
    {
      if(_nStatus!=-1)//Filtro por Estatus
      {
        if(scoreRule.suA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(_scoreRuleFilter.suID>0)//Filtro por ID
      {
        if(scoreRule.suID!=_scoreRuleFilter.suID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_scoreRuleFilter.suN))//Filtro por descripción
      {
        if(!scoreRule.suN.Contains(_scoreRuleFilter.suN,StringComparison.OrdinalIgnoreCase))
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
