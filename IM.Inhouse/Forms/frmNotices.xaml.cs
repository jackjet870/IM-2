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
    //string RTFNotices = string.Empty;
    //string RTFNotice = string.Empty;

    string RTFNotices = string.Empty;
    string RTFNotice = string.Empty;
    DispatcherTimer timer;
    double speed = 0;
    //bool speedChanged = false;

    public frmNotices()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Trae las noticias y las carga en el rtb
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void GetNotices()
    {     
      rtbViewerNotice.Document.Blocks.Clear();
      RTFNotices = string.Empty;
      RTFNotice = string.Empty;

      var Notices = BRNotices.GetNotices(App.User.LeadSource.lsID, BRHelpers.GetServerDate());

      if (Notices.Count > 0)
      {
        foreach (NoticeShort Notice in Notices)
        {                   
          RTFNotices = RTFNotices + Title(Notice.noTitle);
          RTFNotices = RTFNotices + Text(Notice.noText);
        }
        UIRichTextBoxHelper.LoadRTF(ref rtbViewerNotice, RTFNotices);
        RTFNotice = UIRichTextBoxHelper.getRTFFromRichTextBox(ref rtbViewerNotice);
        //rtbViewerNotice.Document.Blocks.Add(new Paragraph(new Run("\u2028 \u2028")));
      }
    }

    /// <summary>
    /// Devuelve el text en el estilo RTF
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private string Text(string Text)
    {
      var rtb = new RichTextBox();
      UIRichTextBoxHelper.LoadRTF(ref rtb, Text);
      rtb.Document.Foreground = Brushes.Black;
      rtb.Document.FontSize = 10;
      //Agregamos dos saltos de pagina
      rtb.Document.Blocks.Add(new Paragraph(new Run("\u2028 \u2028")));      
      return UIRichTextBoxHelper.getRTFFromRichTextBox(ref rtb);
    }

    /// <summary>
    /// Agrega titulo a las noticias con su formato correspondiente
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    public new string Title(string titulo)
    {
      // Create a FlowDocument
      FlowDocument flowDocument = new FlowDocument();
      // Create a paragraph with text
      Paragraph paragraph = new Paragraph();
      //Agregamos el titulo
      paragraph.Inlines.Add(new Underline(new Run(titulo)));
      paragraph.Inlines.Add(new Run("\u2028"));      
      //Lo agregamos el documento
      flowDocument.Blocks.Add(paragraph);
      //Aplicamos el estilo      
      flowDocument.Foreground = Brushes.Black;
      flowDocument.FontFamily = new FontFamily("Verdana");
      flowDocument.FontSize = 20;
      flowDocument.TextAlignment = TextAlignment.Center;
      flowDocument.FontWeight = FontWeights.Bold;      
      var rtb = new RichTextBox();
      rtb.Document = flowDocument;
      return UIRichTextBoxHelper.getRTFFromRichTextBox(ref rtb); 
    }

    /// <summary>
    /// Vuelve a cargar las noticias del día en el rtb
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
      //speedChanged = false;
      _scrollViewer.ScrollToHome();
      GetNotices();
      if (chkAutoscroll.IsChecked.Value)
      {
        speed = .0025;
        RiniciarTimer();
        AutoScroll();
      }
    }

    /// <summary>
    /// Carga todas las notices del día en el rtb
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      GetNotices();
    }

    /// <summary>
    /// inicia el tiempo y manipula el scrollViewer.
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
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
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void chkAutoscroll_Unchecked(object sender, RoutedEventArgs e)
    {
      RiniciarTimer();
      _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
      //deshabilitamos botones de velocidad
      btnUpSpeed.Visibility = btnDownSpeed.Visibility = Visibility.Hidden;
      //speedChanged = false;
      GetNotices();
    }

    /// <summary>
    /// Inicia el tiempo y manipula el scrollViewer.
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    public void AutoScroll()
    {
      //PrepareRTB();
      //_scrollViewer.MaxHeight = 10000;
      double scroll = 0;
      _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
      timer = new DispatcherTimer();
      timer.Interval = new TimeSpan(0, 0, 3/4);
      timer.Tick += ((sender, e) =>
      {     
        scroll = scroll + speed;
        _scrollViewer.ScrollToVerticalOffset(scroll);      

        if (_scrollViewer.VerticalOffset == _scrollViewer.ScrollableHeight)
        {
          RiniciarTimer();
          AutoScroll();
        }
      });
      timer.Start();
    }

    private void PrepareRTB()
    {
      string espace = agregarespacios();
      espace = espace + RTFNotice;
      UIRichTextBoxHelper.LoadRTF(ref rtbViewerNotice, espace);
    }

    private string agregarespacios()
    {
      string espacio = string.Empty;
      double tamaño = Height; //rtb 248
      //double resultado = (((tamaño-120) * 11)/230);
      double resultado = (((tamaño -100) * 12) / 248);
      //double resultado = 12;
      for (int i = 1; i<= resultado; i++)
      {
        espacio = espacio + string.Format(@"Linea {0} \par ",i );
      }
      return espacio;
    }
    /// <summary>
    /// Detiene el tiempo cuando entrea el Mouse al control
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void ViewerNotice_MouseEnter(object sender, MouseEventArgs e)
    {
      if (timer != null)
        timer.Stop();
    }

    /// <summary>
    /// Prosigue con el tiempo cuando sale el Mouse del control
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void ViewerNotice_MouseLeave(object sender, MouseEventArgs e)
    {
      if (timer != null)
        timer.Start();
    }

    /// <summary>
    /// Detiene el tiempo 
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
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
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void UpSpeed_Click(object sender, RoutedEventArgs e)
    {
      //speedChanged = true;
      //speed = speed + .0002;
      speed = speed + .0008;
    }

    /// <summary>
    /// Disminuye la velocidad del tiempo en .0002 segundos
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void DownSpeed_Click(object sender, RoutedEventArgs e)
    {
      //speedChanged = true;
      //if((speed-.0008) >= 0)
      speed = speed - .0008;
    }

    private void btnUpSpeed_MouseDown(object sender, MouseButtonEventArgs e)
    {
      speed = speed + .0002;
    }


    private void btnDownSpeed_MouseDown(object sender, MouseButtonEventArgs e)
    {
      speed = speed - .0002;
    }


    /// <summary>
    /// Para porder mover la ventana con el status bar
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void stbStatusBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.DragMove();
    }
  }
}
