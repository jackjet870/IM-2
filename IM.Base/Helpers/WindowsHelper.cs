using System;
using System.Windows.Forms;
using System.Windows;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Input;

namespace IM.Base.Helpers
{
  public class WindowsHelper
  {
    #region Notify
    /// <summary>
    /// Metodo que crea el notifyIcon, se pasa el form sobre el que se va aplicar.
    /// </summary>
    /// <param name="form">Formulario que se aplica al NotifyIcon</param>
    /// <returns>torna un objecto de tipo NotifyIcon</returns>
    /// <history>
    /// [michan]  25/04/2016  Created
    /// </history>
    public static NotifyIcon Notify(System.Windows.Application app = null ,System.Windows.Window form = null, string title = null)
    {
      /// Objeto del tipo NotifyIcon 
      System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
      //_frm = form;
      ///icono que muestra la nube de notificación. será tipo info, pero existen warning, error, etc..
      notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
      string strTitle = "";
      ///la ruta del icono que se va a mostrar cuando la app esté minimizada.
      if (form != null)
      {
        if (form.Icon != null)
        {
          Uri iconUri = new Uri(form.Icon.ToString(), UriKind.RelativeOrAbsolute);
          System.IO.Stream iconStream = System.Windows.Application.GetResourceStream(new Uri(form.Icon.ToString())).Stream;
          notifyIcon.Icon = new System.Drawing.Icon(iconStream);
        }
        strTitle = form.Title.ToString();
      }
      // AppContext.BaseDirectory
      else if(app != null)
      {
        notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
      }
      

      if (title != null)
      {
        strTitle = title;
      }
      
      //notifyIcon.Icon = new System.Drawing.Icon(iconStream);//@"M:\PalaceResorts\Client4.6\IntelligenceMarketing\IM.Base\Images\IM.ico");
      /// Mensaje que se muestra al minimizar al formulario
      notifyIcon.BalloonTipTitle = "Information";
      
      notifyIcon.Text = (!String.IsNullOrEmpty(strTitle) && !String.IsNullOrWhiteSpace(strTitle)) ? strTitle : "The application";
      notifyIcon.BalloonTipText = "Running " + strTitle;
      notifyIcon.Visible = true;

      /// Evento clic para mostrar la ventana cuando se encuentre minimizada.
      notifyIcon.Click += new EventHandler(
        (s, e) =>
                  {
                    if (notifyIcon != null)
                    {
                      /// cuando se pulse el boton mostrará informacion, cambiaremos los textos para que muestre que la app esta trabajando...
                      notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
                      notifyIcon.BalloonTipText = strTitle + " is working...";
                      notifyIcon.BalloonTipTitle = "Wait...";
                      notifyIcon.ShowBalloonTip(400);
                      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                      notifyIcon.BalloonTipText = strTitle + " Running...";
                      notifyIcon.BalloonTipTitle = "Information";
                    }
                  }
      );

      notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(
        (s, e) =>
        {
          if (notifyIcon != null)
          {
            if (form != null)
            {
              form.Show();
              form.WindowState = WindowState.Normal;
              form.Activate();
            }
            else if (app != null)
            {
              app.MainWindow.Show();
              app.MainWindow.WindowState = WindowState.Normal;
              app.MainWindow.Activate();
            }
            
            notifyIcon.Visible = true;
            notifyIcon.ContextMenu.MenuItems[0].Visible = false;
            notifyIcon.ContextMenu.MenuItems[1].Visible = true;
          }
        }
        );
      


      // agrgegamos el menu en el notyficon
      notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(
              /// Menu contextual que sera visible en el icono
              new System.Windows.Forms.MenuItem[]
            {
                new System.Windows.Forms.MenuItem(
                  "Show",// opcion de abrir para cuando la ventana este minimizada
                  (s, e) =>
                  {
                    if (form != null)
                    {
                      //Se agrega al menu la opcion para mostrar el formulario
                      form.Show();
                      form.WindowState = WindowState.Normal;
                      form.Activate();
                    }
                    else if (app != null)
                    {
                      app.MainWindow.Show();
                      app.MainWindow.WindowState = WindowState.Normal;
                      app.MainWindow.Activate();
                    }
                    


                    notifyIcon.Visible = true;
                    notifyIcon.ContextMenu.MenuItems[0].Visible = false;
                    notifyIcon.ContextMenu.MenuItems[1].Visible = true; 
                  }
                ),
                new System.Windows.Forms.MenuItem(
                  "Hide",// opcion para mostrar la ventana cuando se encuentre maximizada
                  (s, e) =>
                  {
                    if (form != null)
                    {
                      // Se agrega en el menu la opcion para ocultar el form
                      form.Hide();
                      form.WindowState = WindowState.Minimized;
                    }
                    else if (app != null)
                    {
                      app.MainWindow.Hide();
                      app.MainWindow.WindowState = WindowState.Minimized;
                    }
                    
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(400);
                    notifyIcon.ContextMenu.MenuItems[0].Visible = true;
                    notifyIcon.ContextMenu.MenuItems[1].Visible = false;
                  }
                  ),
                new System.Windows.Forms.MenuItem("-"),
                new System.Windows.Forms.MenuItem("Close", 
                (s, e) => {
                  if(form != null)
                  {
                    form.Close();
                  }
                  else if(app != null)
                  {
                    app.Shutdown();
                  }
                  
                  }
                )
            }
            );

      notifyIcon.ContextMenu.MenuItems[0].Visible = false;
      notifyIcon.ContextMenu.MenuItems[1].Visible = true;

      return notifyIcon;
    }
    #endregion

    #region EventsKeys
    /// <summary>
    /// Metodo para controlar las acciones de las combinaciones de teclas para los formularios.
    /// </summary>
    /// <param name="window">Ventana que se desea implementar</param>
    /// <param name="blnNotifycon">True si desea implementar notifycon</param>
    /// <history>
    /// [michan]  17/05/2016  Created
    /// </history>
    public static void EventsKeys(System.Windows.Window window, bool? blnNotifycon = false)
    {
      #region Manejo de la ventana
      //parametros para el manejo de la ventana
      // Si la ventana al minimizar ocultar aplicación en el menu de iconos de la barra de tarea.
      System.Windows.Forms.NotifyIcon _notifyIconFormTransfers = (blnNotifycon.Value) ? Notify(form:window) : null;
      // bandera si se pulso la tecla control
      bool pushCtrl = false;
      bool pushCtrlE = false;
      #endregion
      window.KeyDown += new System.Windows.Input.KeyEventHandler(
        (s, e) => {
          if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl || e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
          {
            pushCtrl = true;
          }

          if ((e.Key == Key.F4) && (e.Key == Key.LeftAlt || e.Key == Key.RightAlt))
          {
            // Se minimiza el formulario al presionar la combinación de teclas:
            // Alt + F4
            window.WindowState = WindowState.Minimized;
          }

          if (e.Key == Key.E && pushCtrl)
          {
            // Se cierra la aplicacion se se pulsaron las teclas:
            // Cntrl + E
            pushCtrlE = true;
            window.Close();
          }
          else if (e.Key == Key.P && pushCtrl)
          {
            // Abre información del usuario si se pulso la tecla:
            // Cntrl + P
            ShowInfoUser();
          }
          else if (e.Key == Key.S && pushCtrl)
          {
            // Se abre la configuracion de setup si se pulsaron las teclas:
            // Cntrl + S
            UIHelper.ShowMessage("Setup");
          }
        }
        );

      window.Closing += new System.ComponentModel.CancelEventHandler(
        (s, e) =>
        {
          // minimiza el formulario si se pulsa el boton de cerrar.
          e.Cancel = !pushCtrlE;
          window.WindowState = WindowState.Minimized;
        }
      );

      // evento para controlar las acciones del formulario
      // es utilizado para el notifycon
      window.StateChanged += new EventHandler(
        (s, e) => {
          // Si la ventana fue minimizada
          if (window.WindowState == WindowState.Minimized)
          {
            // Se oculta el formulario en el menu de icionos
            window.Hide();
            window.WindowState = WindowState.Minimized;
            // si tiene notificon muestra el mensaje de que ha sido ocultado el formulario
            if (_notifyIconFormTransfers != null)
            {
              _notifyIconFormTransfers.ContextMenu.MenuItems[0].Visible = true;
              _notifyIconFormTransfers.ShowBalloonTip(400);
            }
          }
        });
    }
    #endregion

    #region ShowInfoUser
    /// <summary>
    /// Muestra información sobre el usuario
    /// </summary>
    /// <history>
    /// [michan] 25/04/2016 Created
    /// </history>
    private static void ShowInfoUser()
    {
      var host = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).Select(y => y.ToString()).SingleOrDefault();
      string ipAddress = (!String.IsNullOrEmpty(host) && !String.IsNullOrWhiteSpace(host)) ? host : "Local IP Address Not Found!";
      string message = $"User Name: {Environment.UserName}\nComputer Name Local: {Environment.MachineName}\nComputer IP Address Local: {ipAddress}\nComputer Name Remote: {String.Empty}\nComputer IP Address Remote: {String.Empty}";
      UIHelper.ShowMessage(message);
    }
    #endregion
  }
}
