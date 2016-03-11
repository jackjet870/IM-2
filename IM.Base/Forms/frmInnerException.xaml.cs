using System;
using System.Windows;

namespace IM.Base.Forms
{
  /// <summary>
  /// Formulario para desplegar una excepcion anidada
  /// </summary>
  /// <history>
  /// [wtorres] 11/Mar/2016 Created
  /// </history>
  public partial class frmInnerException : Window
  {
    #region Atributos

    Exception _exception;

    #endregion

    #region Constructores y destructores

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exception">Excepcion</param>
    /// <history>
    /// [wtorres]  11/Mar/2016 Created
    /// </history>
    public frmInnerException(Exception exception)
    {
      InitializeComponent();

      _exception = exception;
      txtMessage.Text = exception.Message;
      txtStackTrace.Text = exception.StackTrace;
      if (exception.InnerException == null)
        btnInnerException.Visibility = Visibility.Hidden;
    }

    #endregion

    #region Eventos del formulario

    #region btnYes_Click

    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <history>
    /// [wtorres]  11/Mar/2016 Created
    /// </history>
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }

    #endregion

    #region btnInnerException_Click

    /// <summary>
    /// Despliega la excepcion anidada de la excepcion actual
    /// </summary>
    /// <history>
    /// [wtorres]  11/Mar/2016 Created
    /// </history>
    private void btnInnerException_Click(object sender, RoutedEventArgs e)
    {
      var frm = new frmInnerException(_exception.InnerException);
      frm.ShowDialog();
    }

    #endregion

    #endregion
  }
}
