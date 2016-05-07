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
  public partial class RichTextBoxToolBar1 : UserControl
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
    #endregion

    public RichTextBoxToolBar1()
    {
      InitializeComponent();
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
    /// </history>
    private void imgTextBold_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextBold?.Invoke(this, e);
    }
    /// <summary>
    /// Delega el evento TextItalic
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// </history>
    private void imgTextItalic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextItalic?.Invoke(this, e);
    }
    /// <summary>
    /// Delega el evento TextUnderLine
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// </history>
    private void imgTextUnderLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextUnderLine?.Invoke(this, e);
    }
    /// <summary>
    /// Delega el evento TextStrikeOut
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// </history>
    private void imgTextStrikeOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextStrikeOut?.Invoke(this, e);
    }
    /// <summary>
    /// Delega el evento TextLeft
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// </history>
    private void imgTextLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextLeft?.Invoke(this, e);
    }
    /// <summary>
    /// Delega el evento TextCenter
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// </history>
    private void imgTextCenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextCenter?.Invoke(this, e);
    }
    /// <summary>
    /// Delega el evento TextRight
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// </history>
    private void imgTextRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eTextRight?.Invoke(this, e);
    }
    /// <summary>
    /// Delega el evento ColorPick
    /// </summary>
    /// <history>
    /// [erosado] 06/05/2016  Created
    /// </history>
    private void imgColorPick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      eColorPick?.Invoke(this, e);
    }
    #endregion

  }
}
