using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.BusinessRules.BR;
using System.Windows.Threading;
using IM.Model;
using IM.Base.Helpers;

namespace IM.Inhouse.Forms
{
  /// <summary>
  /// Interaction logic for frmNotices.xaml
  /// </summary>
  public partial class frmNotices : Window
  {
    string RTFNotices = string.Empty;
    DispatcherTimer timer;
    double speed = 0;
    //bool speedChanged = false;

    public frmNotices()
    {
      InitializeComponent();
    }


    /// <summary>
    /// Para porder mover la ventana con el status bar
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    private void stbStatusBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.DragMove();
    }

    /// <summary>
    /// Trae las noticias y las carga en el rtb
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    private void GetNotices()
    {
      ViewerNotice.AppendText(string.Empty);
      RTFNotices = string.Empty;
      var Notices = BRNotices.GetNotices(App.User.LeadSource.lsID, BRHelpers.GetServerDate().Date);

      if (Notices.Count > 0)
      {
        foreach (NoticeShort Notice in Notices)
        {
          //RTFNotices = RTFNotices + Notice.noTitle + @" \par  ";
          RTFNotices = RTFNotices + Title(Notice.noTitle)+ @" \par";
          RTFNotices = RTFNotices + Notice.noText + @" \par \par ";
        }
        UIRichTextBoxHelper.LoadRTF(ref ViewerNotice, RTFNotices);     
      }
    }

    /// <summary>
    /// Agrega titulo a las noticias con su formato correspondiente
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    public new string Title(string titulo)
    {
      RichTextBox rtb = new RichTextBox();
      //rtb.Document = mcFlowDoc;

      rtb.AppendText(titulo);
      //rtb.Background = Brushes.AntiqueWhite;
      rtb.Foreground = Brushes.Black;
      rtb.FontFamily = new FontFamily("Verdana");
      rtb.FontSize = 20;
      //rtb.HorizontalAlignment = HorizontalAlignment.Center;
      //rtb.FontStretch = FontStretches.UltraExpanded;      
      rtb.FontWeight = FontWeights.Bold;
      // rtb.HorizontalContentAlignment = HorizontalAlignment.Center;
      rtb.SelectAll();
      rtb.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
      rtb.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, BaselineAlignment.Center);
      // rtb.TextAlignment = "Center"
      return UIRichTextBoxHelper.getRTFFromRichTextBox(ref rtb);

    }

    /// <summary>
    /// Vuelve a cargar las noticias del día en el rtb
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
      //speedChanged = false;
      _scrollViewer.ScrollToHome();
      GetNotices();
      if (chkAutoscroll.IsChecked.Value)
      {
        RiniciarTimer();
        AutoScroll();
      }
    }

    /// <summary>
    /// Carga todas las notices del día en el rtb
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      GetNotices();
    }

    /// <summary>
    /// inicia el tiempo y manipula el scrollViewer.
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    private void chkAutoscroll_Checked(object sender, RoutedEventArgs e)
    {
      speed = .0025;
      _scrollViewer.ScrollToHome();
      RiniciarTimer();
      AutoScroll();
      //Habilitamos botones de velocidad
      btnUpSpeed.Visibility = btnDownSpeed.Visibility =  Visibility.Visible;
      //speedChanged = false;
    }

    /// <summary>
    /// Termina el tiempo y manipula el scrollViewer.
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    private void chkAutoscroll_Unchecked(object sender, RoutedEventArgs e)
    {
      RiniciarTimer();
      _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
      //deshabilitamos botones de velocidad
      btnUpSpeed.Visibility = btnDownSpeed.Visibility = Visibility.Hidden;
      //speedChanged = false;
    }

    /// <summary>
    /// Inicia el tiempo y manipula el scrollViewer.
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    public void AutoScroll()
    {
      double scroll = 0;
      _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
      timer = new DispatcherTimer();
      timer.Interval = new TimeSpan(0, 0, 3/4);
      timer.Tick += ((sender, e) =>
      {
        //_scrollViewer.LineDown();
        scroll = scroll + speed; //.095;
        _scrollViewer.ScrollToVerticalOffset(scroll);      

        if (_scrollViewer.VerticalOffset == _scrollViewer.ScrollableHeight)
        {
          RiniciarTimer();
          AutoScroll();
        }
      });
      timer.Start();
    }

    /// <summary>
    /// Detiene el tiempo cuando entrea el Mouse al control
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    private void ViewerNotice_MouseEnter(object sender, MouseEventArgs e)
    {
      if (timer != null)
        timer.Stop();
    }

    /// <summary>
    /// Prosigue con el tiempo cuando sale el Mouse del control
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    private void ViewerNotice_MouseLeave(object sender, MouseEventArgs e)
    {
      if (timer != null)
        timer.Start();
    }

    /// <summary>
    /// Detiene el tiempo 
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    public void RiniciarTimer()
    {
      if (timer != null)
      {
        timer.Stop();
        timer = null;
        _scrollViewer.ScrollToHome();
        if (speed == 0)
        {
          speed = .0025;
        }
      }
    }

    /// <summary>
    /// Aumenta la velocidad del tiempo en .0002 segundos
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    private void UpSpeed_Click(object sender, RoutedEventArgs e)
    {
      //speedChanged = true;
      speed = speed + .0002;
    }

    /// <summary>
    /// Disminuye la velocidad del tiempo en .0002 segundos
    /// </summary>
    /// <jorcanche> [jorcanche] 19/04/2016
    private void DownSpeed_Click(object sender, RoutedEventArgs e)
    {
      //speedChanged = true;
      speed = speed - .0002;
    }

    //// Create a FlowDocument
    //FlowDocument mcFlowDoc = new FlowDocument();

    //// Create a paragraph with text
    //Paragraph para = new Paragraph();
    //para.Inlines.Add(new Underline(new Bold(new Run(titulo))));
    ////para.Inlines.Add(new Bold(new Run("Go ahead.")));

    //// Add the paragraph to blocks of paragraph
    //mcFlowDoc.Blocks.Add(para);

  }
}
