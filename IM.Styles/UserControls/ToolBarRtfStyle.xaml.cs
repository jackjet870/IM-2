using IM.Styles.Interfaces;
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
  /// Interaction logic for RichTextBoxToolBar1.xaml
  /// </summary>
  public partial class ToolBarRtfStyle : UserControl
  {
    #region Eventos
    public event EventHandler eLoadRTF;
    public event EventHandler eExportRTF;
    public event EventHandler eTextBold;
    public event EventHandler eTextItalic;
    public event EventHandler eTextUnderLine;
    public event EventHandler eTextStrikeOut;
    public event EventHandler eTextLeft;
    public event EventHandler eTextCenter;
    public event EventHandler eTextRight;
    public event EventHandler eColorPick;

    public event EventHandler koko;
    #endregion

    private readonly SolidColorBrush _color = (SolidColorBrush) new BrushConverter().ConvertFrom("#147F79");

    public ToolBarRtfStyle()
    {
      InitializeComponent();
    }

    private void imooo(object sender, SelectedCellsChangedEventArgs e)
    {
      koko.Invoke(this, e);
    }
    #region Eventos Normales
    /// <summary>
    /// Delega el evento LoadRTF
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// </history>
    private void imgLoadRTF_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eLoadRTF?.Invoke(this,e);
    }
    /// <summary>
    /// Delega el evento ExportRTF
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// </history>
    private void imgExportRTF_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eExportRTF?.Invoke(this, e);
    }
    /// <summary>
    /// Delega el evento TextBold
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// [jorcanche] 20/08/2016 Modified ahora cada vez que se oprime cambia de color 
    /// </history>
    private void imgTextBold_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextBold?.Invoke(this, e);
      imgTextBold.Background = imgTextBold.Background.Equals(_color) ? Brushes.White : _color;
    }
    /// <summary>
    /// Delega el evento TextItalic
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// [jorcanche] 20/08/2016 Modified ahora cada vez que se oprime cambia de color 
    /// </history>
    private void imgTextItalic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextItalic?.Invoke(this, e);
      imgTextItalic.Background = imgTextItalic.Background.Equals(_color) ? Brushes.White : _color;
    }
    /// <summary>
    /// Delega el evento TextUnderLine
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// [jorcanche] 20/08/2016 Modified ahora cada vez que se oprime cambia de color 
    /// </history>
    private void imgTextUnderLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextUnderLine?.Invoke(this, e);
      imgTextUnderLine.Background = imgTextUnderLine.Background.Equals(_color) ? Brushes.White : _color;
      imgTextStrikeOut.Background =  Brushes.White ;
    }
    /// <summary>
    /// Delega el evento TextStrikeOut
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// [jorcanche] 20/08/2016 Modified ahora cada vez que se oprime cambia de color 
    /// </history>
    private void imgTextStrikeOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextStrikeOut?.Invoke(this, e);
      imgTextStrikeOut.Background = imgTextStrikeOut.Background.Equals(_color) ? Brushes.White : _color;
      imgTextUnderLine.Background = Brushes.White;
    }
    /// <summary>
    /// Delega el evento TextLeft
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// [jorcanche] 20/08/2016 Modified ahora cada vez que se oprime cambia de color 
    /// </history>
    private void imgTextLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextLeft?.Invoke(this, e);
      imgTextLeft.Background = _color;
      imgTextCenter.Background = Brushes.White;
      imgTextRight.Background = Brushes.White ;
    }
    /// <summary>
    /// Delega el evento TextCenter
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// [jorcanche] 20/08/2016 Modified ahora cada vez que se oprime cambia de color 
    /// </history>
    private void imgTextCenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextCenter?.Invoke(this, e);     
      if (imgTextCenter.Background.Equals(_color))
      {
        imgTextCenter.Background = Brushes.White;
        imgTextLeft.Background = _color;
      }
      else
      {
        imgTextCenter.Background = _color;
        imgTextLeft.Background = Brushes.White; 
      }     
      imgTextRight.Background = Brushes.White;
    }
    /// <summary>
    /// Delega el evento TextRight
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// [jorcanche] 20/08/2016 Modified ahora cada vez que se oprime cambia de color 
    /// </history>
    private void imgTextRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextRight?.Invoke(this, e);      
      if (imgTextRight.Background.Equals(_color))
      {
        imgTextRight.Background = Brushes.White;
        imgTextLeft.Background = _color;
      }
      else
      {
        imgTextRight.Background = _color;
        imgTextLeft.Background = Brushes.White;
      }      
      imgTextCenter.Background = Brushes.White;
    }
    /// <summary>
    /// Delega el evento ColorPick
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// [jorcanche] 20/08/2016 Modified ahora cada vez que se oprime cambia de color 
    /// </history>
    private void imgColorPick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eColorPick?.Invoke(this, e);
    }
    #endregion

  }
}
