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
  /// Interaction logic for frmScoreRulesConcepts.xaml
  /// </summary>
  public partial class frmScoreRulesConcepts : Window
  {
    #region Variables
    private ScoreRuleConcept _scoreRuleConceptFilter = new ScoreRuleConcept();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmScoreRulesConcepts()
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
    /// [emoguel] created 23/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadScoreRulesConcepts();
    }
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 22/04/2016
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
    /// [emoguel] created 22/04/2016
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
    /// [emoguel] created 22/04/2016
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
    /// [emoguel] 22/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      ScoreRuleConcept scoreRuleConcept = (ScoreRuleConcept)dgrScoreRulesConcepts.SelectedItem;
      frmScoreRuleConceptDetail frmScoRulConDetail = new frmScoreRuleConceptDetail();
      frmScoRulConDetail.Owner = this;
      frmScoRulConDetail.enumMode = EnumMode.edit;
      frmScoRulConDetail.oldScoreRuleConcept = scoreRuleConcept;
      if(frmScoRulConDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<ScoreRuleConcept> lstScoreRulesConcepts = (List<ScoreRuleConcept>)dgrScoreRulesConcepts.ItemsSource;
        if(ValidateFilter(frmScoRulConDetail.scoreRuleConcept))//Validamos si cumple con los filtros actuales
        {
          ObjectHelper.CopyProperties(scoreRuleConcept, frmScoRulConDetail.scoreRuleConcept);//Actualizamos los datos
          lstScoreRulesConcepts.Sort((x, y) => string.Compare(x.spN, y.spN));//Ordenamos la lista
          nIndex = lstScoreRulesConcepts.IndexOf(scoreRuleConcept);//Buscamos la posición del registro
        }
        else
        {
          lstScoreRulesConcepts.Remove(scoreRuleConcept);//Quitamos el registro
        }
        dgrScoreRulesConcepts.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrScoreRulesConcepts, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstScoreRulesConcepts.Count + " Score Rules Concepts.";//Actualizamos el contador
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
    /// [emoguel] created 22/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.enumWindow = EnumWindow.DefaultByte;
      frmSearch.nStatus = _nStatus;
      frmSearch.strID = (_scoreRuleConceptFilter.spID > 0) ? _scoreRuleConceptFilter.spID.ToString() : "";
      frmSearch.strDesc = _scoreRuleConceptFilter.spN;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _scoreRuleConceptFilter.spID = Convert.ToByte(frmSearch.strID);
        _scoreRuleConceptFilter.spN = frmSearch.strDesc;
        LoadScoreRulesConcepts();
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
    /// [emoguel] created 23/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmScoreRuleConceptDetail frmScoRulConDetail = new frmScoreRuleConceptDetail();
      frmScoRulConDetail.Owner = this;
      frmScoRulConDetail.enumMode = EnumMode.add;
      if(frmScoRulConDetail.ShowDialog()==true)
      {
        ScoreRuleConcept scoreRuleConcept = frmScoRulConDetail.scoreRuleConcept;
        if(ValidateFilter(scoreRuleConcept))//Validamos si cumple con los filtros actuales
        {
          List<ScoreRuleConcept> lstScoreRulesConcepts = (List<ScoreRuleConcept>)dgrScoreRulesConcepts.ItemsSource;
          lstScoreRulesConcepts.Add(scoreRuleConcept);//Agregamos el registro
          lstScoreRulesConcepts.Sort((x, y) => string.Compare(x.spN, y.spN));//Ordenamos la lista
          int nIndex = lstScoreRulesConcepts.IndexOf(scoreRuleConcept);//Buscamos la posición del registro
          dgrScoreRulesConcepts.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrScoreRulesConcepts, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstScoreRulesConcepts.Count + " Score Rules Concepts.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga el grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param><
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      ScoreRuleConcept scoreRuleConcept = (ScoreRuleConcept)dgrScoreRulesConcepts.SelectedItem;
      LoadScoreRulesConcepts(scoreRuleConcept);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadScoreRulesConcepts
    /// <summary>
    /// Llena el grid de score rules concepts
    /// </summary>
    /// <param name="scoreRuleConcept">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    private async void LoadScoreRulesConcepts(ScoreRuleConcept scoreRuleConcept = null)
    {
      try
      {
        int nIndex = 0;
        List<ScoreRuleConcept> lstScoreRulesConcepts = await BRScoreRulesConcepts.GetScoreRulesConcepts(_nStatus, _scoreRuleConceptFilter);
        dgrScoreRulesConcepts.ItemsSource = lstScoreRulesConcepts;
        if (lstScoreRulesConcepts.Count > 0 && scoreRuleConcept != null)
        {
          scoreRuleConcept = lstScoreRulesConcepts.Where(sp => sp.spID == scoreRuleConcept.spID).FirstOrDefault();
          nIndex = lstScoreRulesConcepts.IndexOf(scoreRuleConcept);
        }

        GridHelper.SelectRow(dgrScoreRulesConcepts, nIndex);
        StatusBarReg.Content = lstScoreRulesConcepts.Count + " Score Rules Concepts.";
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Score Rule Concept");
      }
    }
    #endregion

    #region validateFilter
    /// <summary>
    /// Valida que un objeto cumpla con los filtros actuales
    /// </summary>
    /// <param name="scoreRuleConcept">Objeto a validar</param>
    /// <returns>Treu. si cumple | false. No cumple</returns>
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    private bool ValidateFilter(ScoreRuleConcept scoreRuleConcept)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(scoreRuleConcept.spA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }
      if(_scoreRuleConceptFilter.spID>0)//Filtro por ID
      {
        if(scoreRuleConcept.spID!=_scoreRuleConceptFilter.spID)
        {
          return false;
        }
      }
      if(!string.IsNullOrWhiteSpace(_scoreRuleConceptFilter.spN))//Filtro por descripción
      {
        if(!scoreRuleConcept.spN.Contains(_scoreRuleConceptFilter.spN))
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
