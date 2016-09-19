using IM.Base.Classes;
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
    /// Notificamos una excepcion por correo electronico
    /// </summary>
    /// <param name="exception">Objeto con los datos de la excepcion</param>
    /// <history>
    /// [wtorres] 14/Sep/2016 Created
    /// [wtorres] 15/Sep/2016 Modified. Elimine el parametro user
    /// </history>
    public static void SendException(Exception exception)
    {
      // obtenemos los datos del usuario si estos estan disponibles
      AppUser appUser = null;
      if (Context.User != null && Context.User.User != null)
      {
        appUser = new AppUser()
        {
          Id = Context.User.User.peID,
          Name = Context.User.User.peN
        };
      }

      // notificamos la excepcion por correo electronico
      Notifier.AsyncSendException(exception, null, appUser);
    }
    #endregion
  }
}