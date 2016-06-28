using System.Windows;
using System.Windows.Data;
using IM.BusinessRules.BR;
using IM.Base.Forms;
using System.Threading.Tasks;

namespace IM.Base.Forms
{
  /// <summary>
  /// Formulario que visualiza el Guest Log
  /// </summary>
  /// <history>
  /// [jorcanche] 20/06/2016 created
  /// </history>
  public partial class frmGuestLog : Window
  {
    #region Atributos
    private int _idGuest;
    private string _leadSource;
    #endregion

    #region Constructores y destructores
    public frmGuestLog(int idGuest, string leadSource)
    {
      InitializeComponent();
      _idGuest = idGuest;
      _leadSource = leadSource;
    }
    #endregion

    #region Eventos del formulario

    #region Window_Loaded
    /// <summary>
    /// Inicializa el datagrid 
    /// </summary>
    /// <history>
    /// [jorcanche] 20/06/2016 created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadGuestLog();
      Title = $"IM guest Log - Guest ID {_idGuest} / Lead Source {_leadSource}";           
    }
    private async void LoadGuestLog()
    {
      dgGuestLog.DataContext = await BRGuestsLogs.GetGuestLog(_idGuest);
    }
    #endregion

    #region btnGuestMovents_Click
    /// <summary>
    /// Invoca al formulario de Guest Movents
    /// </summary>
    /// <history>
    /// [jorcanche] 20/06/2016 created
    /// </history>
    private void btnGuestMovents_Click(object sender, RoutedEventArgs e)
    {
      frmGuestsMovements guestMovements = new frmGuestsMovements(_idGuest);
      guestMovements.Owner = this;
      guestMovements.ShowDialog();
    }
    #endregion

    #endregion

  }
}
