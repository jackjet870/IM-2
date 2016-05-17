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
  /// Interaction logic for frmScoreRulesTypes.xaml
  /// </summary>
  public partial class frmScoreRulesTypes : Window
  {
    #region Variables
    private ScoreRuleType _scoreRuleTypeFilter = new ScoreRuleType();//Objeto con los filtros adicionales
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmScoreRulesTypes()
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
    /// [emoguel] created 25/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadScoreRulesTypes();
    } 
    #endregion

    #region MyRegion
    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 23/04/2016
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
    /// [emoguel] created 23/04/2016
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
    /// [emoguel] created 23/04/2016
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
    /// [emoguel] 23/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      ScoreRuleType scoreRuleType = (ScoreRuleType)dgrScoreRulesTypes.SelectedItem;
      frmScoreRuleTypeDetail frmScoreRuleTypeDetail = new frmScoreRuleTypeDetail();
      frmScoreRuleTypeDetail.Owner = this;
      frmScoreRuleTypeDetail.enumMode = EnumMode.edit;
      frmScoreRuleTypeDetail.oldScoreRuleType = scoreRuleType;
      if(frmScoreRuleTypeDetail.ShowDialog()==true)
      {
        List<ScoreRuleType> lstScoreRulesTypes = (List<ScoreRuleType>)dgrScoreRulesTypes.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmScoreRuleTypeDetail.scoreRuleType))//Validamos si cumple con los filtros actuales
        {
          ObjectHelper.CopyProperties(scoreRuleType, frmScoreRuleTypeDetail.scoreRuleType);//Actualizamos los datos del registro
          lstScoreRulesTypes.Sort((x, y) => string.Compare(x.syN, y.syN));//Ordenamos la lista
          nIndex = lstScoreRulesTypes.IndexOf(scoreRuleType);//Obtenemos la posicion del registro
        }
        else
        {
          lstScoreRulesTypes.Remove(scoreRuleType);//Quitamos el registro del grid
        }
        dgrScoreRulesTypes.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrScoreRulesTypes, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstScoreRulesTypes.Count + " Score Rules Types.";//Actualizamos el contador
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
    /// [emoguel] created 23/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.nStatus = _nStatus;
      frmSearch.strID = _scoreRuleTypeFilter.syID;
      frmSearch.strDesc = _scoreRuleTypeFilter.syN;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _scoreRuleTypeFilter.syID = frmSearch.strID;
        _scoreRuleTypeFilter.syN = frmSearch.strDesc;
        LoadScoreRulesTypes();
      }

    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en Modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmScoreRuleTypeDetail frmScoreRuleTypeDetail = new frmScoreRuleTypeDetail();
      frmScoreRuleTypeDetail.Owner = this;
      frmScoreRuleTypeDetail.enumMode = EnumMode.add;
      if(frmScoreRuleTypeDetail.ShowDialog()==true)
      {
        ScoreRuleType scoreRuleType = frmScoreRuleTypeDetail.scoreRuleType;
        if(ValidateFilter(scoreRuleType))//Verificamos que cumpla con el filtro
        {
          List<ScoreRuleType> lstScoreRulesTypes = (List<ScoreRuleType>)dgrScoreRulesTypes.ItemsSource;
          lstScoreRulesTypes.Add(scoreRuleType);//Agregamos el registro nuevo
          lstScoreRulesTypes.Sort((x, y) => string.Compare(x.syN, y.syN));//Ordenamos la lista
          int nIndex = lstScoreRulesTypes.IndexOf(scoreRuleType);//Obtenemos la posición del registro
          dgrScoreRulesTypes.Items.Refresh();//Actualizmaos la vista
          GridHelper.SelectRow(dgrScoreRulesTypes, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstScoreRulesTypes.Count + " Score Rules Types.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga el grid 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      ScoreRuleType scoreRuleType = (ScoreRuleType)dgrScoreRulesTypes.SelectedItem;
      LoadScoreRulesTypes(scoreRuleType);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadScoreRulesTypes
    /// <summary>
    /// llena el grid
    /// </summary>
    /// <param name="scoreRuleType">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    private void LoadScoreRulesTypes(ScoreRuleType scoreRuleType = null)
    {
      int nIndex = 0;
      List<ScoreRuleType> lstScoreRulesTypes = BRScoreRulesTypes.GetScoreRulesTypes(_nStatus, _scoreRuleTypeFilter);
      dgrScoreRulesTypes.ItemsSource = lstScoreRulesTypes;
      if (lstScoreRulesTypes.Count > 0 && scoreRuleType != null)
      {
        scoreRuleType = lstScoreRulesTypes.Where(sy => sy.syID == scoreRuleType.syID).FirstOrDefault();
        nIndex = lstScoreRulesTypes.IndexOf(scoreRuleType);
      }
      GridHelper.SelectRow(dgrScoreRulesTypes, nIndex);
      StatusBarReg.Content = lstScoreRulesTypes.Count + "Score Rules Types.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida si un scoreRuleType cumple con los filtros actuales
    /// </summary>
    /// <param name="scoreRuleType">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    private bool ValidateFilter(ScoreRuleType scoreRuleType)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(scoreRuleType.syA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_scoreRuleTypeFilter.syID))//Filtro por ID
      {
        if(_scoreRuleTypeFilter.syID!=scoreRuleType.syID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_scoreRuleTypeFilter.syN))//Filtro por descripcion
      {
        if(!scoreRuleType.syN.Contains(_scoreRuleTypeFilter.syN))
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
