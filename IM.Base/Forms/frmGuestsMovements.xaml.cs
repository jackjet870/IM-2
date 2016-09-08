using IM.BusinessRules.BR;
using System.Windows;
using System.Windows.Input;

namespace IM.Base.Forms
{
  /// <summary>
  /// Formulario que visualiza el Guests Movements
  /// </summary>
  /// <history>
  /// [jorcanche] 20/06/2016 created
  /// </history>
  public partial class frmGuestsMovements : Window
  {
    private int _guestID = 0;
    public frmGuestsMovements(int guestId)
    {
      InitializeComponent();
      _guestID = guestId;     
    }

    #region Window_Loaded
    /// <summary>
    /// Inicializa el datagrid 
    /// </summary>
    /// <history>
    /// [jorcanche] 20/06/2016 created
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Mouse.OverrideCursor = Cursors.Wait;
      Title = $"IM Guest Movements - Guest ID {_guestID}";
      var movement = await BRGuests.GetGuestMovement(_guestID);
      guestMovementsDataGrid.ItemsSource = movement;
      Mouse.OverrideCursor = null;
    } 
    #endregion
  }
}