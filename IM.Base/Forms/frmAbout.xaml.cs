using System;
using System.Windows;


namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmAbout.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 09/Feb/2016 Created
  /// </history>
  public partial class frmAbout : Window
  {
    public frmAbout()
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      InitializeComponent();
    }

    /// <summary>
    /// Cierra la ventana About
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnCerrar_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
