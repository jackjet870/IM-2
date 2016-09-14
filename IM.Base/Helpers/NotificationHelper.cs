using IM.Model.Classes;
using PalaceResorts.Common.Notifications.WinForm;
using System;

namespace IM.Base.Helpers
{
  /// <summary>
  /// Clase que sirve como asistente para el envio de notificaciones por correo
  /// </summary>
  /// <history>
  ///   [wtorres]  14/Sep/2016 Created
  /// </history>
  public class NotificationHelper
  {
    #region ShowMessage

    /// <summary>
    /// Envia un mensaje al usuario
    /// </summary>
    /// <param name="message">Mensaje que se mostrará</param>
    /// <param name="image">Imagen que se mostrará en la ventana</param>
    /// <param name="title">Título de la ventana</param>
    /// <history>
    /// [lchairez] 24/Feb/2016 Created
    /// [wtorres]  01/Mar/2016 Modified. Ahora la imagen y el titulo son opcionales
    /// [emoguel]  28/Mar/2016 Modified. Ahora el boton se puede cambiar por otro
    /// </history>
    public static void SendException(Exception exception, UserData user)
    {
      // obtenemos los datos del usuario si estos estan disponibles
      AppUser appUser = null;
      if (user != null && user.User !=null)
      {
        appUser = new AppUser()
        {
          Id = user.User.peID,
          Name = user.User.peN
        };
      }

      // notificamos la excepcion por correo electronico
      Notifier.AsyncSendException(exception, null, appUser);
    }
    #endregion
  }
}