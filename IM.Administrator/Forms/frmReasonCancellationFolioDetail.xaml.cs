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
  /// Interaction logic for frmReasonCancellationFolioDetail.xaml
  /// </summary>
  public partial class frmReasonCancellationFolioDetail : Window
  {
    #region Variables
    public ReasonCancellationFolio reasonCancellationFolio = new ReasonCancellationFolio();//Objeto a guardar
    public ReasonCancellationFolio oldReasonCanFol = new ReasonCancellationFolio();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    #endregion
    public frmReasonCancellationFolioDetail()
    {
      InitializeComponent();
    }

    #region WindowLoaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(reasonCancellationFolio, oldReasonCanFol);
      UIHelper.SetUpControls(reasonCancellationFolio, this);
      if(enumMode!=EnumMode.preview)
      {
        txtrcfID.IsEnabled = (enumMode == EnumMode.add);
        txtrcfN.IsEnabled = true;
        chkrcfA.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
      }
      DataContext = reasonCancellationFolio;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
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

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(reasonCancellationFolio, oldReasonCanFol))
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
      else
      {
        Close();
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza registros en el catalogo ReasonCancellationFolios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode == EnumMode.edit && ObjectHelper.IsEquals(reasonCancellationFolio, oldReasonCanFol))
        {
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Reason For Cancellation Of Folio");
          if (strMsj == "")
          {
            int nRes = await BREntities.OperationEntity(reasonCancellationFolio, enumMode);
            UIHelper.ShowMessageResult("Reason For Cancellation Of Folio", nRes);
            if (nRes > 0)
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
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Reason For Cancellation Of Folio");
      }
    } 
    #endregion
  }
}
