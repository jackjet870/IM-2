using System;
using System.Windows.Forms;
using System.Windows;

namespace IM.Base.Helpers
{
  public class NotifyIconHelper
  {
    public static NotifyIcon notifyIcon;
    public static ContextMenu menu;
    public static Window _frm;

    #region Notify
    /// <summary>
    /// Metodo que crea el notifyIcon, se pasa el form sobre el que se va aplicar.
    /// </summary>
    /// <param name="form">Gormulario que se aplica al NotifyIcon</param>
    /// <returns>torna un objecto de tipo NotifyIcon</returns>
    /// <history>
    /// [michan]  25/04/2016  Created
    /// </history>
    public static NotifyIcon Notify(Window form)
    {
      
      /// Objeto del tipo NotifyIcon 
      notifyIcon = new NotifyIcon();
      _frm = form;
      ///icono que muestra la nube de notificación. será tipo info, pero existen warning, error, etc..
      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      ///la ruta del icono que se va a mostrar cuando la app esté minimizada.
      if(form.Icon != null)
      {
        ;
        Uri iconUri = new Uri(form.Icon.ToString(), UriKind.RelativeOrAbsolute);
        System.IO.Stream iconStream = System.Windows.Application.GetResourceStream(new Uri(form.Icon.ToString())).Stream;
        notifyIcon.Icon = new System.Drawing.Icon(iconStream);

      }
        
      //notifyIcon.Icon = new System.Drawing.Icon(iconStream);//@"M:\PalaceResorts\Client4.6\IntelligenceMarketing\IM.Base\Images\IM.ico");
      /// Mensaje que se muestra al minimizar al formulario
      notifyIcon.BalloonTipText = "Running Transfers...";
      notifyIcon.BalloonTipTitle = "Information";
      notifyIcon.Text = _frm.Title.ToString();
      notifyIcon.Visible = true;
      /// Evento clic para mostrar la ventana cuando se encuentre minimizada.
      notifyIcon.Click += new EventHandler(MiniIcon_Click);

      notifyIcon.MouseDoubleClick += new MouseEventHandler(NotifyIcon_MouseDoubleClick);
      /// Menu contextual que sera visible en el icono
      menu = new ContextMenu();
      MenuItem maximize = new MenuItem("Show");// opcion de abrir para cuando la ventana este minimizada
      maximize.Click += new EventHandler(Maximize); // opcion para mostrar la ventana cuando se encuentre minimizada
      MenuItem minimize = new MenuItem("Hide");
      minimize.Click += new EventHandler(Minimize);

      menu.MenuItems.Add(maximize);//Se agrega al menu la opcion para mostrar el formulario
      menu.MenuItems.Add(minimize);// Se agrega en el menu la opcion para ocultar el form
      notifyIcon.ContextMenu = menu; // agrgegamos el menu en el notyficon
      notifyIcon.ContextMenu.MenuItems[0].Visible = false;
      notifyIcon.ContextMenu.MenuItems[1].Visible = false;

      return notifyIcon;
    }
    #endregion

    #region MiniIcon_Click
    /// <summary>
    /// se verifica si se encuentra minimizada y se procede a mostrar la ventana.
    /// </summary>
    /// <history>
    /// [michan]  25/04/2016
    /// </history>
    public static void MiniIcon_Click(object sender, EventArgs e)
    {
      if (_frm.WindowState == WindowState.Minimized || _frm.WindowState == WindowState.Normal || _frm.WindowState == WindowState.Maximized)
      {
        _frm.WindowState = WindowState.Normal;
        _frm.Show();
        notifyIcon.ContextMenu.MenuItems[0].Visible = false;
        notifyIcon.ContextMenu.MenuItems[1].Visible = true;
      }
    }
    #endregion

    #region Maximize
    /// <summary>
    /// Metodo que responde a la accion de ver, muestra el formulario si esta minimizado
    /// </summary>
    /// <history>
    /// [michan]  25/04/2016  Created
    /// </history>
    public static void Maximize(object sender, EventArgs e)
    {
      if (_frm.WindowState == WindowState.Minimized || _frm.WindowState == WindowState.Normal || _frm.WindowState == WindowState.Maximized)
      {
        _frm.WindowState = WindowState.Normal;
        _frm.Show();
        //_frm.Activate();
        notifyIcon.ContextMenu.MenuItems[0].Visible = false;
        notifyIcon.ContextMenu.MenuItems[1].Visible = true;
        notifyIcon.Visible = false;
      }
    }
    #endregion

    #region Minimize
    /// <summary>
    /// Metodo que responde a la accion de ocultar formulario
    /// </summary>
    /// <history>
    /// [michan]  25/04/2016  Created
    /// </history>
    public static void Minimize(object sender, EventArgs e)
    {
      /// cada vez que cambie el status de la ventana independientemente si es por el notifyicon o del boton minimizar de la ventana
      /// se verificara el state y se procede a ocultar y a mostrar la nube del notifyicon.
      if (_frm.WindowState == WindowState.Maximized || _frm.WindowState == WindowState.Normal || _frm.WindowState == WindowState.Minimized)
      {
        _frm.Hide();
        _frm.WindowState = WindowState.Minimized;
        notifyIcon.Visible = true;
        notifyIcon.ContextMenu.MenuItems[0].Visible = true;
        notifyIcon.ContextMenu.MenuItems[1].Visible = false;
      }

    }
    #endregion

    #region Button_Click
    public static void Button_Click(object sender, RoutedEventArgs e)
    {
      if (notifyIcon != null)
      {
        /// cuando se pulse el boton mostrará informacion, cambiaremos los textos para que muestre que la app esta trabajando...
        notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
        notifyIcon.BalloonTipText = "The application is working...";
        notifyIcon.BalloonTipTitle = "Wait...";
        notifyIcon.ShowBalloonTip(500);
        notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
        notifyIcon.BalloonTipText = "Still Running...";
        notifyIcon.BalloonTipTitle = "Information";
      }
    }
    #endregion

    #region NotifyIcon_MouseDoubleClick
    /// <summary>
    /// Metodo que maximiza el formulario al hcer doble clik sobre el icono
    /// </summary>
    /// <history>
    /// [michan]  25/04/2016  Created
    public static void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (_frm.WindowState == WindowState.Minimized || _frm.WindowState == WindowState.Normal || _frm.WindowState == WindowState.Maximized)
      {
        notifyIcon.Visible = false;
        notifyIcon.ContextMenu.MenuItems[0].Visible = false;
        notifyIcon.ContextMenu.MenuItems[1].Visible = true;
        _frm.WindowState = WindowState.Normal;
        _frm.Show();
        //_frm.Activate();
      }
    }
    #endregion

  }
}
