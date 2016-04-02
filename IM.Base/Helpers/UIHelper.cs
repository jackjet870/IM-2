using System;
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
    /// [emoguel]  28/Mar/2016 Modified. Ahora el boton se puede cambiar por otro
    /// </history>
    public static MessageBoxResult ShowMessage(string message, MessageBoxImage image = MessageBoxImage.Information, string title = "",MessageBoxButton button=MessageBoxButton.OK)
    {
      
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

    #region ShowMessageResult
    /// <summary>
    /// Valida el resultado de guardar|actualizar en algun catalogo
    /// </summary>
    /// <param name="strObject">Nombre del objeto que aparecerá en el mensaje</param>
    /// <param name="nResult">resultado de la operación</param>
    /// <param name="blnIsTransaccion">valida si la operacion es una transacción</param>
    /// <history>
    /// [emoguel] created 02/04/2016
    /// </history>
    public static void ShowMessageResult(string strObject,int nResult,bool blnIsTransaccion=false,bool blnIsRange=false)
    {
      #region respuesta
      switch (nResult)
      {
        case 0:
          {
            ShowMessage( strObject + " not saved.");
            break;
          }
        case 1:
          {
            ShowMessage(strObject + " successfully saved.");
            break;
          }
        case 2:
          {
            if (blnIsTransaccion)//SI es una transaccion
            {
              ShowMessage(strObject + " successfully saved.");
              break;
            }
            else if(blnIsRange)//si es una validacion de rango de numeros
            {
              ShowMessage(strObject + " Please check the ranges, impossible save it."); 
            }
            else
            {
              ShowMessage(strObject + " ID already exist please select another one.");
            }
            break;
          }
        default://mayor que 2 cuando se guardan mas de 1 objeto
          {
            ShowMessage(strObject + " successfully saved.");
            break;
          }
      }
      #endregion
    }
    #endregion
    #endregion Metodos
  }
}