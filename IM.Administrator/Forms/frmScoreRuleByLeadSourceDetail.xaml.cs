﻿using System;
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
  /// Interaction logic for frmScoreRuleByLeadSourceDetail.xaml
  /// </summary>
  public partial class frmScoreRuleByLeadSourceDetail : Window
  {
    #region Variables
    public EnumMode enumMode;//Modo de la ventana
    private bool _isCellCancel = false;
    private bool _isClosing = false;
    public ScoreRuleByLeadSource scoreRuleByLeadSource = new ScoreRuleByLeadSource();
    public ScoreRuleByLeadSource oldScoreRuleByLeadSource = new ScoreRuleByLeadSource();
    private List<ScoreRuleByLeadSourceDetail> _lstScoreRulesDet = new List<ScoreRuleByLeadSourceDetail>();
    #endregion
    public frmScoreRuleByLeadSourceDetail()
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
    /// [emoguel] created 27/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(scoreRuleByLeadSource, oldScoreRuleByLeadSource);            
      cmbsbID.IsEnabled = (enumMode == EnumMode.add);
      LoadLeadSources();
      LoadScoreRuleDetail();
      LoadScoreConcepts();
      DataContext = scoreRuleByLeadSource;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {

      if (!_isClosing)
      {
        _isClosing = true;
        btnCancel_Click(null, null);
        if (!_isClosing)
        {
          e.Cancel = true;
        }
        else
        {
          _isClosing = false;
        }
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
    /// [emoguel] created 27/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if (!ObjectHelper.IsEquals(scoreRuleByLeadSource, oldScoreRuleByLeadSource) || hasChageScores())
      {
        MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
        if (result == MessageBoxResult.Yes)
        {
          if (!_isClosing) { _isClosing = true; Close(); }
        }
        else
        {
          _isClosing = false;
        }
      }
      else
      {
        if (!_isClosing) { _isClosing = true; Close(); }
      }
    }
    #endregion

    #region btnAccept_Click
    /// <summary>
    /// Agrega|Actualiza un ScoreRuleByLeadSource
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<ScoreRuleByLeadSourceDetail> lstScoreRuleDetails = (List<ScoreRuleByLeadSourceDetail>)dgrScores.ItemsSource;
      if (enumMode != EnumMode.add && ObjectHelper.IsEquals(scoreRuleByLeadSource, oldScoreRuleByLeadSource) && !hasChageScores())
      {
        _isClosing = true;
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Score Rule");
        if (strMsj == "")
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";

          List<ScoreRuleByLeadSourceDetail> lstScoreDetail = (List<ScoreRuleByLeadSourceDetail>)dgrScores.ItemsSource;
          #region Listas
          var lstAdd = lstScoreDetail.Where(sj =>
                !_lstScoreRulesDet.Any(sjj =>
                sj.sjsp == sjj.sjsp && sj.sjls == sjj.sjls
                )).ToList();

          var lstDel = _lstScoreRulesDet.Where(sj =>
          !lstScoreDetail.Any(sjj =>
          sj.sjsp == sjj.sjsp && sj.sjls == sjj.sjls
          )).ToList();

          var lstUpd = _lstScoreRulesDet.Where(sj =>
            lstScoreDetail.Any(sjj =>
            sj.sjsp == sjj.sjsp && sj.sjls == sjj.sjls
            )).ToList();
          #endregion

          int nRes = await BRScoreRulesByLeadSource.SaveScoreRuleByLeadSource(scoreRuleByLeadSource, lstAdd, lstDel, lstUpd, (enumMode == EnumMode.edit));// await BRProducts.SaveProduct(product, (enumMode == EnumMode.edit), _productLegend, lstAdd, lstDel);
          skpStatus.Visibility = Visibility.Collapsed;
          UIHelper.ShowMessageResult("Score Rule By Lead Source", nRes);
          if (nRes > 0)
          {
            if(enumMode==EnumMode.add)
            {
              var lstNewItem = await BRScoreRulesByLeadSource.GetScoreRuleByLeadSource(scoreRuleByLeadSource.sbls);
              scoreRuleByLeadSource = lstNewItem.FirstOrDefault();  
            }
            _isClosing = true;
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

    #region dgrScores_BeginningEdit
    /// <summary>
    /// Verifica que primero se seleccione un concept
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/05/2016
    /// </history>
    private void dgrScores_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if ((e.Column is DataGridComboBoxColumn) == false)
      {
        var item = e.Row.Item;
        if (Convert.ToInt32(item.GetType().GetProperty("sjsp").GetValue(item)) < 1)
        {
          UIHelper.ShowMessage("Please select one concept");
          e.Cancel = true;
        }
      }
    }
    #endregion

    #region dgrScores_CellEditEnding
    /// <summary>
    /// Verifica que no se repita un registro en el grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private void dgrScores_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))
      {
        _isCellCancel = false;
        if (e.EditingElement is Control)
        {
          bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrScores, strPropGrid: "sjsp");
          e.Cancel = isRepeat;
        }
      }
      else
      {
        _isCellCancel = true;        
      }
    }
    #endregion

    #region dgrScores_RowEditEnding
    /// <summary>
    /// Verifica que no se agregue una fila vacia
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private void dgrScores_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (_isCellCancel)
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
        if (item.GetType().Name == "ScoreRuleByLeadSourceDetail")
        {
          cmbScoreRuleConcept.Header = "Concept (" + (dgrScores.Items.Count - 1) + ")";
        }
      }
    }

    #endregion

    #region txtScore
    /// <summary>
    /// Permite decimal y un punto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/05/2016
    /// </history>
    private void txtScore_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (!(e.Text == "." && !txt.Text.Trim().Contains(".")))
      {
        e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
      };
    }
    #endregion
    #endregion

    #region methods
    #region LoadLeadSources
    /// <summary>
    /// Llena el combobox de LeadSource
    /// </summary>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    private async void LoadLeadSources()
    {
      try
      {
        List<LeadSource> lstLeadSources = await BRLeadSources.GetLeadSources(1, -1,-1);
        cmbsbID.ItemsSource = lstLeadSources;
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error,"Score Rule By Lead Source");
      }
    }
    #endregion

    #region LoadScoreRuleDetail
    /// <summary>
    /// Llena el grid de ScoreRulesDetails
    /// </summary>
    /// <history>
    /// [emoguel] created 28/05/2016
    /// </history>
    private async void LoadScoreRuleDetail()
    {
      try
      {
        List<ScoreRuleByLeadSourceDetail> lstScoreRuleDetail = await BRScoreRulesByLeadSourceDetail.GetScoreRulesByLeadSourceDetail(scoreRuleByLeadSource.sbls);
        dgrScores.ItemsSource = lstScoreRuleDetail;
        lstScoreRuleDetail.ForEach(sj => {
          ScoreRuleByLeadSourceDetail scoreRuleByLSDet = new ScoreRuleByLeadSourceDetail();
          ObjectHelper.CopyProperties(scoreRuleByLSDet, sj);
          _lstScoreRulesDet.Add(scoreRuleByLSDet);
        });
        cmbScoreRuleConcept.Header = "Score Rule (" + lstScoreRuleDetail.Count + ")";
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Score Rule By Lead Source");
      }
    }
    #endregion

    #region LoadScoreConcepts
    /// <summary>
    /// Llena el combobox de Concepts
    /// </summary>
    /// <history>
    /// [emoguel] created 28/05/2016
    /// </history>
    private async void LoadScoreConcepts()
    {
      try
      {
        List<ScoreRuleConcept> lstScoreRuleConcepts = await BRScoreRulesConcepts.GetScoreRulesConcepts(1);
        cmbScoreRuleConcept.ItemsSource = lstScoreRuleConcepts;        
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Score Rule By Lead Source");
      }
    } 
    #endregion

    #region ValidateChangeScores
    /// <summary>
    /// Verifica si ha cambiado algo en el score details
    /// </summary>
    /// <returns>True. hubo cambios | false. no hubo cambios</returns>
    /// <history>
    /// [emoguel] created 28/05/2016
    /// </history>
    private bool hasChageScores()
    {
      List<ScoreRuleByLeadSourceDetail> lstScoreDetail = (List<ScoreRuleByLeadSourceDetail>)dgrScores.ItemsSource;
      var lstAdd = lstScoreDetail.Where(sj =>
      !_lstScoreRulesDet.Any(sjj =>
      sj.sjScore == sjj.sjScore && sj.sjsp == sjj.sjsp && sj.sjls == sjj.sjls
      )).ToList();

      var lstDel = _lstScoreRulesDet.Where(sj =>
      !lstScoreDetail.Any(sjj =>
      sj.sjScore == sjj.sjScore && sj.sjsp == sjj.sjsp && sj.sjls == sjj.sjls
      )).ToList();

      if (lstAdd.Count > 0 || lstDel.Count > 0)
      {
        return true;
      }
      return false;
    }
    #endregion
    #endregion
  }
}