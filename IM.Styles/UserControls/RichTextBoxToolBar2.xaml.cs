using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IM.Styles.UserControls
{
  /// <summary>
  /// Interaction logic for RichTextBoxToolBar2.xaml
  /// </summary>
  public partial class RichTextBoxToolBar2 : UserControl
  {
    public event EventHandler eChangeFontSize;
    public event EventHandler eChangeFontFamily;

    public RichTextBoxToolBar2()
    {
      
      InitializeComponent();
      loadFontSizeAndFontFamilies();
    }
    /// <summary>
    /// Delega el evento cambiar tamaño de la fuente
    /// </summary>
    /// <history>
    /// [erosado] 07/05/2016  Created
    /// </history>
    private void cbxfontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      eChangeFontSize?.Invoke(this, e);
    }
    /// <summary>
    /// Delega el evento cambiar tipo de fuente
    /// </summary>
    /// <history>
    /// [erosado] 07/05/2016  Created
    /// </history>
    private void cbxfontFamilies_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      eChangeFontFamily?.Invoke(this, e);
    }
    /// <summary>
    /// Carga la información de Font size y font Familie en los combos
    /// </summary>
    /// <history>
    /// [erosado] 08/04/2016  Created
    /// </history>
    private void loadFontSizeAndFontFamilies()
    {
      //Load Font Families
      cbxfontFamilies.ItemsSource = System.Drawing.FontFamily.Families.Select(s => s.Name).ToArray();
      cbxfontFamilies.SelectedIndex = 0;

      //Load FontSize
      List<int> fontSize = new List<int>();
      for (int i = 4; i <= 72; i++)
      {
        fontSize.Add(i);
      }
      cbxfontSize.ItemsSource = fontSize;
      cbxfontSize.SelectedIndex = 0;
    }

  }
}
