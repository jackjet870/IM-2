﻿using System;
using System.Windows;
using System.Windows.Threading;

namespace IM.Base.Helpers
{
  /// <summary>
  /// Clase que sirve como asistente para la interfaz de usuario
  /// </summary>
  /// <history>
  ///   [wtorres]  01/03/2016 Created
  /// </history>
  public class UIHelper
  {
    #region Metodos

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
    /// </history>
    public static MessageBoxResult ShowMessage(string message, MessageBoxImage image = MessageBoxImage.Information, string title = "")
    {
      MessageBoxButton button = MessageBoxButton.OK;
      switch (image)
      {
        case MessageBoxImage.Error:
          if (String.IsNullOrEmpty(title))
            title = "Error";
          break;

        case MessageBoxImage.Warning:
          if (String.IsNullOrEmpty(title))
            title = "Warning";
          break;

        case MessageBoxImage.Question:
          if (String.IsNullOrEmpty(title))
            title = "Question";
          button = MessageBoxButton.YesNo;
          break;

        default:
          if (String.IsNullOrEmpty(title))
            title = "Information";
          break;
      }

      return MessageBox.Show(message, title, button, image);
    }

    #endregion ShowMessage

    #region ForceUIToUpdate

    /// <summary>
    /// Forza la actualizacion del UI mientra se cargan datos
    /// </summary>
    /// <history>
    /// [aalcocer] 08/Mar/16 Created
    /// </history>
    public static void ForceUIToUpdate()
    {
      DispatcherFrame frame = new DispatcherFrame();

      Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate (object parameter)
      {
        frame.Continue = false;
        return null;
      }), null);

      Dispatcher.PushFrame(frame);
    }

    #endregion ForceUIToUpdate

    #endregion Metodos
  }
}