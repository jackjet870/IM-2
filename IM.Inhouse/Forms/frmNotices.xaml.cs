using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace IM.Inhouse.Forms
{
  /// <summary>
  /// Formulario que sirve para visualizar las noticias 
  /// </summary>
  /// <history>
  /// [jorcanche] 19/04/2016
  /// </history>
  public partial class frmNotices : Window
  {
    #region Atributos
    string RTFNotices = string.Empty;
    string RTFNotice = string.Empty;
    DispatcherTimer timer;
    double speed = 0; 
    #endregion

    #region Contructor

    /// <summary>
    /// Contructor
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// </history>
    public frmNotices()
    {
      InitializeComponent();
    } 
    #endregion

    #region GetNotices
    /// <summary>
    /// Trae las noticias y las carga en el rtb
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// </history>
    private async void GetNotices()
    {
      rtbViewerNotice.Document.Blocks.Clear();
      RTFNotices = string.Empty;
      RTFNotice = string.Empty;

      var notices = await BRNotices.GetNotices(Context.User.LeadSource.lsID, BRHelpers.GetServerDate());

      if (notices.Count > 0)
      {
        foreach (var notice in notices)
        {
          RTFNotices = RTFNotices + Title(notice.noTitle);
          RTFNotices = RTFNotices + Text(notice.noText);
        }
        UIRichTextBoxHelper.LoadRTF(ref rtbViewerNotice, RTFNotices);
        RTFNotice = UIRichTextBoxHelper.getRTFFromRichTextBox(ref rtbViewerNotice);
      }
    }
    #endregion

    #region Text
    /// <summary>
    /// Devuelve el text en el estilo RTF
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// </history>
    private string Text(string text)
    {
      var rtb = new RichTextBox();
      UIRichTextBoxHelper.LoadRTF(ref rtb, text);
      rtb.Document.Foreground = Brushes.Black;
      rtb.Document.FontSize = 10;
      //Agregamos dos saltos de pagina
      rtb.Document.Blocks.Add(new Paragraph(new Run("\u2028 \u2028")));
      return UIRichTextBoxHelper.getRTFFromRichTextBox(ref rtb);
    }
    #endregion

    #region Title
    /// <summary>
    /// Agrega titulo a las noticias con su formato correspondiente
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// </history>
    public new string Title(string titulo)
    {
      // Create a FlowDocument
      var flowDocument = new FlowDocument();
      // Create a paragraph with text
      var paragraph = new Paragraph();
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
      var rtb = new RichTextBox { Document = flowDocument };
      return UIRichTextBoxHelper.getRTFFromRichTextBox(ref rtb);
    }
    #endregion

    #region btnRefresh_Click
    /// <summary>
    /// Vuelve a cargar las noticias del día en el rtb
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// </history>
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
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga todas las notices del día en el rtb
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      GetNotices();
    }
    #endregion

    #region chkAutoscroll_Checked
    /// <summary>
    /// inicia el tiempo y manipula el scrollViewer.
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// </history>
    private void chkAutoscroll_Checked(object sender, RoutedEventArgs e)
    {
      speed = .0025;
      _scrollViewer.ScrollToHome();
      RiniciarTimer();
      AutoScroll();
      //Habilitamos botones de velocidad
      btnUpSpeed.Visibility = btnDownSpeed.Visibility = Visibility.Visible;
    }
    #endregion

    #region chkAutoscroll_Unchecked
    /// <summary>
    /// Termina el tiempo y manipula el scrollViewer.
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// </history>
    private void chkAutoscroll_Unchecked(object sender, RoutedEventArgs e)
    {
      RiniciarTimer();
      _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
      //deshabilitamos botones de velocidad
      btnUpSpeed.Visibility = btnDownSpeed.Visibility = Visibility.Hidden;
      GetNotices();
    }
    #endregion

    #region AutoScroll
    /// <summary>
    /// Inicia el tiempo y manipula el scrollViewer.
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// </history>
    public void AutoScroll()
    {
      double scroll = 0;
      _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
      timer = new DispatcherTimer();
      timer.Interval = new TimeSpan(0, 0, 3 / 4);
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
    #endregion

    #region ViewerNotice_MouseEnter
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
    #endregion

    #region ViewerNotice_MouseLeave
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
    #endregion

    #region RiniciarTimer
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
    #endregion

    #region UpSpeed_Click
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
    #endregion

    #region DownSpeed_Click
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
    #endregion

    #region btnUpSpeed_MouseDown
    /// <summary>
    /// Agrega velocidad al Scroll
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void btnUpSpeed_MouseDown(object sender, MouseButtonEventArgs e)
    {
      speed = speed + .0002;
    }

    #endregion

    #region btnDownSpeed_MouseDown
    /// <summary>
    /// Quita velocidad al Scroll
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void btnDownSpeed_MouseDown(object sender, MouseButtonEventArgs e)
    {
      speed = speed - .0002;
    }
    #endregion

    #region stbStatusBar_MouseLeftButtonDown
    /// <summary>
    /// Para porder mover la ventana con el status bar
    /// </summary>
    /// <history>
    /// [jorcanche] 19/04/2016
    /// <history>
    private void stbStatusBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      DragMove();
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Cierra la ventana al precionar la tecla Esc
    /// </summary>
    /// <history>
    /// [jorcanche] 10/08/2016
    /// <history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape) Close();
    } 
    #endregion
  }
}
