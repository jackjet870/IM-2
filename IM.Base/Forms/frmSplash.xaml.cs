using System.Windows;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmSplash.xaml
  /// </summary>
  public partial class frmSplash : Window
  {
    #region Constructores y destructores

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="title">Formulario principal del modulo</param>
    /// <history>
    /// [wtorres]  10/Mar/2016 Modified. Agregue el parametro title
    /// </history>
    public frmSplash(string title)
    {
      InitializeComponent();

      this.Title = title;
    }

    #endregion

    #region Metodos

    #region ShowLogin

    /// <summary>
    /// Función para ejecutar el frmLogin sobre el Splash
    /// </summary>
    /// <param name="pParent"> Instancia del frmLogin segun sea el tipo </param>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// </history>
    public void ShowLogin(ref frmLogin frmLogin)
    {
      frmLogin.WindowStyle = WindowStyle.None;
      frmLogin.ShowInTaskbar = false;
      frmLogin.Owner = this;

      frmLogin.Left = this.Left + 240;
      frmLogin.Top = this.Top + 83;
      frmLogin.ShowDialog();
    }

    #endregion

    #endregion
  }
}
