using IM.Base.Helpers;
using IM.Model.Classes;
using System;
using System.Windows;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for ErrorMessage.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 11/Feb/2016 Created
  /// </history>
  public partial class frmError : Window
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
    ///  [lchairez] 11/Feb/2016 Created
    ///  [wtorres]  11/Mar/2016 Modified. Ahora despliega el Stack Trace y la excepcion anidada
    ///  [wtorres]  13/Ago/2016 Modified. Ahora envia un correo electronico con la excepcion
    ///  [wtorres]  14/Sep/2016 Modified. Ahora enviar los datos del usuario
    /// </history>
    public frmError(Exception exception, UserData user)
    {
      InitializeComponent();

      _exception = exception;
      txtMessage.Text = exception.Message;
      txtStackTrace.Text = exception.StackTrace;
      if (exception.InnerException == null)
        btnInnerException.Visibility = Visibility.Hidden;

      // notificamos la excepcion por correo electronico
      NotificationHelper.SendException(exception, user);
    }

    #endregion

    #region Eventos del formulario

    #region btnYes_Click

    /// <summary>
    /// Evento del botón YES
    /// </summary>
    /// <history>
    /// [lchairez] 11/feb/2016 Created
    /// </history>
    private void btnYes_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }

    #endregion

    #region btnNo_Click

    /// <summary>
    /// Evento del botón NO
    /// </summary>
    /// <history>
    /// [lchairez] 11/feb/2016 Created
    /// </history>
    private void btnNo_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
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
