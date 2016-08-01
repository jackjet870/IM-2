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
  /// Interaction logic for frmScoreRuleDetail.xaml
  /// </summary>
  public partial class frmScoreRuleDetail : Window
  {
    #region Variables
    public ScoreRule scoreRule = new ScoreRule();//Objeto a guardar
    public ScoreRule oldScoreRule = new ScoreRule();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    private List<ScoreRuleDetail> _lstScoreRuleDetail = new List<ScoreRuleDetail>();//Lista de Score Rule
    private bool isCellCancel = false;
    private bool blnClosing = false;
    #endregion
    public frmScoreRuleDetail()
    {
      InitializeComponent();
    }

    #region MethodsForm
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
      skpStatus.Visibility = Visibility.Visible;
      ObjectHelper.CopyProperties(scoreRule, oldScoreRule);
      txtsuID.IsEnabled = (enumMode == EnumMode.add); 
      LoadSocreRuleDetail();
      LoadScoreRuleConcepts();
      UIHelper.SetUpControls(scoreRule, this);
      DataContext = scoreRule;
    }
    #endregion

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing)
      {
        btnCancel.Focus();
        if (!ObjectHelper.IsEquals(scoreRule, oldScoreRule) || hasChageScores())
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrScores.CancelEdit();
          }
        }
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda un Score Rule
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<ScoreRuleDetail> lstScoreRuleDetails = (List<ScoreRuleDetail>)dgrScores.ItemsSource;
      if (enumMode != EnumMode.add && ObjectHelper.IsEquals(scoreRule, oldScoreRule) && !hasChageScores())
      {
        blnClosing = true;
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Score Rule",blnDatagrids:true);
        if (strMsj == "")
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";

          List<ScoreRuleDetail> lstScoreDetail = (List<ScoreRuleDetail>)dgrScores.ItemsSource;
          #region Listas
          var lstAdd = lstScoreDetail.Where(su =>
                !_lstScoreRuleDetail.Any(suu =>
                su.sisp == suu.sisp && su.sisu == suu.sisu
                )).ToList();

          var lstDel = _lstScoreRuleDetail.Where(su =>
          !lstScoreDetail.Any(suu =>
          su.sisp == suu.sisp && su.sisu == suu.sisu
          )).ToList();

          var lstUpd= _lstScoreRuleDetail.Where(su =>
           lstScoreDetail.Any(suu =>
           su.sisp == suu.sisp && su.sisu == suu.sisu
           )).ToList();
          #endregion

          int nRes = await BRScoreRules.SaveScore(scoreRule,lstAdd,lstDel,lstUpd,(enumMode==EnumMode.edit));// await BRProducts.SaveProduct(product, (enumMode == EnumMode.edit), _productLegend, lstAdd, lstDel);
          skpStatus.Visibility = Visibility.Collapsed;
          UIHelper.ShowMessageResult("Score Rule", nRes);
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
      }
    }
    #endregion

    #region CellEdit
    /// <summary>
    /// Verifica que no se repita un concept
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    private void dgrScores_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if(e.EditAction==DataGridEditAction.Commit)
      {
        isCellCancel = false;
        if (e.EditingElement is Control)
        {          
          bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrScores, strPropGrid: "sisp");
          e.Cancel = isRepeat;
        }
      }
      else
      {
        isCellCancel = true;
      }
    }
    #endregion

    #region RowEdit
    /// <summary>
    /// Verifica que no se agreguen filas vacias
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/06/2016
    /// </history>
    private void dgrScores_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (isCellCancel)
        {
          dgrScores.RowEditEnding -= dgrScores_RowEditEnding;
          dgrScores.CancelEdit();
          if (e.Row.IsNewItem)
          {
            dgrScores.Items.RemoveAt(dgrScores.SelectedIndex);
            dgrScores.Items.Refresh();
          }
          dgrScores.RowEditEnding += dgrScores_RowEditEnding;
        }
        else
        {
          cmbScoreRuleConcept.Header = "Concept (" + (dgrScores.Items.Count - 1) + ")";
        }
      }
    }
    #endregion

    #region dgrScores_BeginningEdit
    /// <summary>
    /// Obliga a editar primero el rule concept
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private void dgrScores_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (!GridHelper.IsInEditMode(dgrScores))
      {
        if ((e.Column is DataGridComboBoxColumn) == false)
        {
          var item = e.Row.Item;
          if (Convert.ToInt32(item.GetType().GetProperty("sisp").GetValue(item)) < 1)
          {
            UIHelper.ShowMessage("Please select one concept");
            e.Cancel = true;
          }
        }
      }
      else
      {
        e.Cancel = true;
      }
    }
    #endregion

    #region PreviewtextInput
    /// <summary>
    /// Verifica que solo se acepten números y un punto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    private void txtScore_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (!(e.Text == "." && !txt.Text.Trim().Contains(".")))
      {
        e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
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
    /// [created] 25/05/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        var item = dgrScores.SelectedItem;
        if (item.GetType().Name == "ScoreRuleDetail")
        {
          cmbScoreRuleConcept.Header = "Concept (" + (dgrScores.Items.Count - 1) + ")";
        }
      }
    }

    #endregion
    #endregion

    #region Methods
    #region LoadScoreRuleConcepts
    /// <summary>
    /// Carga el combo de ScoreRuleConcept
    /// </summary>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    private async void LoadScoreRuleConcepts()
    {
      try
      {
        List<ScoreRuleConcept> lstScoreRuleConcepts = await BRScoreRulesConcepts.GetScoreRulesConcepts(1);
        cmbScoreRuleConcept.ItemsSource = lstScoreRuleConcepts;
        cmbScoreRuleConcept.Header = "Score Rule ("+lstScoreRuleConcepts.Count+")";
        skpStatus.Visibility = Visibility.Collapsed;
        btnAccept.Visibility = Visibility.Visible;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Score Rule");
      }
    }
    #endregion

    #region GetScoreRulesDetails
    /// <summary>
    /// Llena el grid de Score Rule Concept
    /// </summary>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private async void LoadSocreRuleDetail()
    {
      try
      {        
        List<ScoreRuleDetail> lstScoreRulesDetail = await BRScoreRulesDetails.GetScoreRulesDetails(scoreRule.suID);
        dgrScores.ItemsSource = lstScoreRulesDetail;        
        lstScoreRulesDetail.ForEach(su=>{
          ScoreRuleDetail scoreRuleDetail = new ScoreRuleDetail();
          ObjectHelper.CopyProperties(scoreRuleDetail, su);
          _lstScoreRuleDetail.Add(scoreRuleDetail);
        });
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Score Rule");
      }
    }
    #endregion

    #region ValidateChangeScores
    /// <summary>
    /// Verifica si ha cambiado algo en el score details
    /// </summary>
    /// <returns>True. hubo cambios | false. no hubo cambios</returns>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private bool hasChageScores()
    {
      List<ScoreRuleDetail> lstScoreDetail = (List<ScoreRuleDetail>)dgrScores.ItemsSource;
      var lstAdd = lstScoreDetail.Where(su => 
      !_lstScoreRuleDetail.Any(suu =>
      su.siScore==suu.siScore && su.sisp==suu.sisp && su.sisu==suu.sisu
      )).ToList();

      var lstDel = _lstScoreRuleDetail.Where(su =>
      !lstScoreDetail.Any(suu =>
      su.siScore == suu.siScore && su.sisp == suu.sisp && su.sisu == suu.sisu
      )).ToList();

      if(lstAdd.Count>0 || lstDel.Count>0)
      {
        return true;
      }
      return false;
    }
    #endregion

    #endregion
  }
}
