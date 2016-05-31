using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmScoreRulesbyLeadSources.xaml
  /// </summary>
  public partial class frmScoreRulesByLeadSource : Window
  {
    public frmScoreRulesByLeadSource()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window_Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      status.Visibility = Visibility.Visible;
      LoadScoreRulesByLS();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/05/2016
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
    /// [emoguel] created 27/05/2016
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
      ScoreRuleByLeadSource scoreRuleByLeadSource = (ScoreRuleByLeadSource)dgrScoreRulesByLS.SelectedItem;
      frmScoreRuleByLeadSourceDetail frmScoreRuleByLS = new frmScoreRuleByLeadSourceDetail();
      frmScoreRuleByLS.Owner = this;
      frmScoreRuleByLS.enumMode = EnumMode.edit;
      frmScoreRuleByLS.oldScoreRuleByLeadSource = scoreRuleByLeadSource;
      frmScoreRuleByLS.ShowDialog();
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmScoreRuleByLeadSourceDetail frmScoreRuleByLS = new frmScoreRuleByLeadSourceDetail();
      frmScoreRuleByLS.Owner = this;
      frmScoreRuleByLS.enumMode = EnumMode.add;      
      if(frmScoreRuleByLS.ShowDialog()==true)
      {
        List<ScoreRuleByLeadSource> lstScoreRuleByLeadSource = new List<ScoreRuleByLeadSource>();
        lstScoreRuleByLeadSource.Add(frmScoreRuleByLS.scoreRuleByLeadSource);
        lstScoreRuleByLeadSource.Sort((x, y) =>string.Compare(x.sbls,y.sbls));
        int nIndex = lstScoreRuleByLeadSource.IndexOf(frmScoreRuleByLS.scoreRuleByLeadSource);
        dgrScoreRulesByLS.Items.Refresh();
        GridHelper.SelectRow(dgrScoreRulesByLS, nIndex);
        StatusBarReg.Content = lstScoreRuleByLeadSource.Count+" Score Rules.";
      }
    }
    #endregion

    #region refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      status.Visibility = Visibility.Visible;
      ScoreRuleByLeadSource scoreRuleByLeadSource = (ScoreRuleByLeadSource)dgrScoreRulesByLS.SelectedItem;
      LoadScoreRulesByLS();
    }
    #endregion
    #endregion

    #region Methods
    #region LoadScoreRulesByLS
    /// <summary>
    /// Llena el grid de ScoreRuleByLeadSources
    /// </summary>
    /// <param name="scoreRuleByLS">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private async void LoadScoreRulesByLS(ScoreRuleByLeadSource scoreRuleByLS = null)
    {
      List<ScoreRuleByLeadSource> lstScoreRulesByLS = await BRScoreRulesByLeadSource.GetScoreRuleByLeadSource();
      int nIndex = 0;
      dgrScoreRulesByLS.ItemsSource = lstScoreRulesByLS;
      if(lstScoreRulesByLS.Count>0 && scoreRuleByLS!=null)
      {
        scoreRuleByLS = lstScoreRulesByLS.Where(sb => sb.sbls == scoreRuleByLS.sbls).FirstOrDefault();
        nIndex = lstScoreRulesByLS.IndexOf(scoreRuleByLS);
      }
      GridHelper.SelectRow(dgrScoreRulesByLS, nIndex);
      StatusBarReg.Content = lstScoreRulesByLS.Count + " Score Rules.";
      status.Visibility = Visibility.Collapsed;
    } 
    #endregion
    #endregion
  }
}
