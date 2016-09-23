using System.Windows;
using IM.Model.Classes;
using IM.Base.Helpers;
using IM.Model.Helpers;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmMargin.xaml
  /// </summary>
  public partial class frmMargin : Window
  {
    public frmMargin(Margin margin)
    {
      InitializeComponent();
      //Asignamos el datacontext
      DataContext = margin;
      //Agregamos el evento para que pueda escribir sólo decimales
      txtLeft.PreviewTextInput += TextBoxHelper.DecimalTextInput;
      txtRight.PreviewTextInput += TextBoxHelper.DecimalTextInput;
      txtTop.PreviewTextInput += TextBoxHelper.DecimalTextInput;
      txtBottom.PreviewTextInput += TextBoxHelper.DecimalTextInput;
    }

    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      Margin margin = DataContext as Margin;
      DialogResult = true;
      Close();
    }
  }
}
