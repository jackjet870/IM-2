using IM.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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

    #region SetMaxLength
    /// <summary>
    /// Le asigna el maximo de escritura a los controles igual al que tienen en la BD
    /// </summary>
    /// <param name="obj">Objeto a validar las propiedades</param>
    /// <param name="ui">Contenedor donde va a buscar controles</param>
    /// <history>
    /// [emoguel] created 08/04/2016
    /// </history>
    public static void SetMaxLength(object obj, Window ui)
    {
      List<Control> lstControls = GetChildParentCollection<Control>(ui);//Obtenemos la lista de controles del contenedor      

      Type type = obj.GetType();//Obtenemos el tipo de objeto
      if (lstControls.Count > 0)
      {
        #region Obtenemos el MaxLength
        EntityTypeBase entityTypeBase = EntityHelper.GetEntityTypeBase(type);
        foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => !pi.GetMethod.IsVirtual))//recorremos las propiedades
        {            
          EdmMember edm = entityTypeBase.Members.Where(em => em.Name == pi.Name).FirstOrDefault();//Obtenemos el edmMember            

          Facet facet;
          Control control = lstControls.Where(cl => cl.Name == "txt" + pi.Name).FirstOrDefault();//buscamos si existe el control

          if (control != null)//Verifcamos que tengamos un control
          {
            TextBox txt = control as TextBox;//Convertimos el control a texbox
            TypeCode typeCode = Type.GetTypeCode(pi.PropertyType);//Obtenemos el tipo de dato              
            switch (typeCode)
            {
              #region String
              case TypeCode.String:
              case TypeCode.Char:
                {
                  facet= edm.TypeUsage.Facets.Where(fc => fc.Name == "MaxLength").FirstOrDefault();//Obtenemos el length
                  txt.MaxLength = Convert.ToInt32(facet.Value);//Asignamos el MaxLength
                  break;
                }
              #endregion

              #region Decimal
              case TypeCode.Decimal:
              case TypeCode.Double:                
                {
                  int Precision = Convert.ToInt32(edm.TypeUsage.Facets.Where(fc => fc.Name == "Precision").FirstOrDefault().Value);
                  txt.MaxLength = Precision;
                  break;
                }
              #endregion

              #region Byte
              case TypeCode.Byte:
                {
                  txt.MaxLength = 3;
                  break;
                }
              #endregion

              #region Int
              case TypeCode.Int16:
              case TypeCode.Int32:
              case TypeCode.Int64:
                {
                  txt.MaxLength = 10;
                  break;
                }
              #endregion

              #region DateTime
              case TypeCode.DateTime:
                {
                  if (txt.Name.EndsWith("DT"))
                  {
                    txt.MaxLength = 24;
                  }
                  else if (txt.Name.EndsWith("T"))
                  {
                    txt.MaxLength = 5;
                  }
                  else
                  {
                    txt.MaxLength = 10;
                  }
                  break;
                } 
                #endregion

            }
          }           
            
        }
        #endregion
      }

    }
    #endregion

    #region GetChildParenCollection
    /// <summary>
    /// busca todos los controles contenedores para recorrerlos
    /// </summary>
    /// <typeparam name="T">tipo de contenedor</typeparam>
    /// <param name="parent">contenedor</param>
    /// <returns>devuelve una lista de controles</returns>
    /// <history>
    /// [emoguel] created 02/04/2016
    /// </history>
    public static List<T> GetChildParentCollection<T>(object parent) where T : DependencyObject
    {
      List<T> logicalCollection = new List<T>();
      GetChildCollection(parent as DependencyObject, logicalCollection);
      return logicalCollection;
    }
    #endregion

    #region GetChildCollection
    /// <summary>
    /// Obtiene todos los controles dentro de los contenedores
    /// </summary>
    /// <typeparam name="T">tipo de contenedor</typeparam>
    /// <param name="parent">Contenedor</param>
    /// <param name="logicalCollection">lista de controles</param>
    /// <history>
    /// [emoguel] created 02/04/2016
    /// </history>
    private static void GetChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
    {
      IEnumerable children = LogicalTreeHelper.GetChildren(parent);
      foreach (object child in children)
      {
        if (child is DependencyObject)
        {
          DependencyObject depChild = child as DependencyObject;
          if (child is T)
          {
            logicalCollection.Add(child as T);
          }
          GetChildCollection(depChild, logicalCollection);
        }
      }
    }
    #endregion
    #endregion Metodos
  }
}