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
  /// Interaction logic for frmScoreRuleConceptDetail.xaml
  /// </summary>
  public partial class frmScoreRuleConceptDetail : Window
  {
    #region Variables
    public ScoreRuleConcept scoreRuleConcept = new ScoreRuleConcept();//Objeto a guardar
    public ScoreRuleConcept oldScoreRuleConcept = new ScoreRuleConcept();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    #endregion
    public frmScoreRuleConceptDetail()
    {
      InitializeComponent();
    }
    #region Loaded
    /// <summary>
    /// Carga los datos iniciales de  la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(scoreRuleConcept, oldScoreRuleConcept);
      UIHelper.SetUpControls(scoreRuleConcept, this);
      txtspID.IsEnabled = (enumMode == EnumMode.add);
      DataContext = scoreRuleConcept;
    }
    #endregion

    #region Window KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key==Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);
      }
    }
    #endregion
    #region Add
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(enumMode!=EnumMode.add && ObjectHelper.IsEquals(scoreRuleConcept,oldScoreRuleConcept))
      {
        Close();
      }
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Score Rule Concept");
        if (strMsj == "")
        {
          int nRes = BREntities.OperationEntity(scoreRuleConcept, enumMode);
          UIHelper.ShowMessageResult("Score Rule Concept", nRes);
          if(nRes>0)
          {
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

    #region Cancel
    /// <summary>
    /// Cierra la ventana Verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (!ObjectHelper.IsEquals(scoreRuleConcept, oldScoreRuleConcept))
      {
        MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
        if (result == MessageBoxResult.Yes)
        {
          Close();
        }
      }
      else
      {
        Close();
      }
    } 
    #endregion
  }
}
