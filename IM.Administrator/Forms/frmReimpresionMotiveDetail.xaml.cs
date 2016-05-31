using System.Windows;
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
  /// Interaction logic for frmReimpresionMotiveDetail.xaml
  /// </summary>
  public partial class frmReimpresionMotiveDetail : Window
  {
    #region Variables
    public ReimpresionMotive reimpresionMotive = new ReimpresionMotive();//Objeto a guardar
    public ReimpresionMotive oldReimpresionMotive = new ReimpresionMotive();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    #endregion
    public frmReimpresionMotiveDetail()
    {
      InitializeComponent();
    }

    #region Window KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(reimpresionMotive, oldReimpresionMotive);
      UIHelper.SetUpControls(reimpresionMotive, this);
      txtrmID.IsEnabled = true;
      DataContext = reimpresionMotive;
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo ReimpresionMotives
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// [emoguel] modified 30/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.add && ObjectHelper.IsEquals(reimpresionMotive, oldReimpresionMotive))
        {
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Reimpresion Motive");
          if (reimpresionMotive.rmID == 0)
          {
            strMsj += (strMsj == "") ? "" : " \n " + "Reimpresion Motive ID can not be 0.";
          }
          if (strMsj == "")
          {
            int nRes = await BREntities.OperationEntity(reimpresionMotive, enumMode);
            UIHelper.ShowMessageResult("Reimpresion Motive", nRes);
            if (nRes > 0)
            {
              DialogResult = true;
              Close();
            }
          }
          else
          {
            Close();
          }
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Reimpresion Motive");
      }
    } 
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra verficando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (!ObjectHelper.IsEquals(reimpresionMotive, oldReimpresionMotive))
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
