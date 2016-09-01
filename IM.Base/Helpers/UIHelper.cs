using IM.Model.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using System.Data.Entity.Validation;
using IM.BusinessRules.BR;
using IM.Styles.Classes;
using IM.Styles.Enums;
using PalaceResorts.Common.Notifications.WinForm;

namespace IM.Base.Helpers
{
  /// <summary>
  /// Clase que sirve como asistente para la interfaz de usuario
  /// </summary>
  /// <history>
  ///   [wtorres]  01/03/2016 Created
  ///   [ecanul] 17/05/2016 Modified Agregado metodo GetErrorMessage
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

    /// <summary>
    /// Muestra un MessageBox con los errores encontrados en una Excepción, Implementa GetMessageError y EntityValidationException
    /// </summary>
    /// <param name="ex">Mensaje de la excepcion</param>
    ///<history>
    ///[erosado]  28/May/2016 Created.
    ///[erosado]  08/Jul/2016 Modified. Se agregó buscar excepcion de tipo entityValidationException.
    ///[wtorres]  13/Ago/2016 Modified. Ahora envia un correo electronico con la excepcion
    /// </history>
    public static MessageBoxResult ShowMessage(Exception ex)
    {
      //Declaramos variables
      string message = string.Empty;
      List<DbValidationError> errorList = null;
      var dbEntityValidationException = ex as DbEntityValidationException; // Casteamos la excepcion generica.

      // si es una excepcion de validacion de Entity Framework
      if (dbEntityValidationException != null && dbEntityValidationException.EntityValidationErrors.Any())
      {
        errorList = new List<DbValidationError>();
        dbEntityValidationException.EntityValidationErrors.OfType<DbEntityValidationResult>().ToList().ForEach(objVR =>
        {
          objVR.ValidationErrors.OfType<DbValidationError>().ToList().ForEach(objVE =>
          {
            errorList.Add(objVE);
          });
        });
        var errorListDistinct = errorList.Select(c => new { c.PropertyName, c.ErrorMessage }).Distinct().ToList();
        errorListDistinct.ForEach(x => { message += $"{x.PropertyName}: {x.ErrorMessage} \n"; }); // Preparamos el mensaje.
      }
      else // Si es Generica
      {
        message = GetMessageError(ex);
      }

      // notificamos la excepcion por correo electronico
      Notifier.AsyncSendException(ex);

      // desplegamos el mensaje de la excepcion
      return MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
    /// <history>
    /// [emoguel] created 02/04/2016
    /// [emoguel] modified 22/04/2016 se Elimino el parametro transaction
    /// </history>
    public static void ShowMessageResult(string strObject, int nResult)
    {
      #region respuesta
      switch (nResult)
      {
        case -2:
          {
            ShowMessage(strObject + " Please check the ranges, impossible save it.");
            break;
          }
        case -1:
          {
            ShowMessage(strObject + " ID already exist please select another one.");
            break;
          }
        case 0:
          {
            ShowMessage(strObject + " not saved.");
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

    #region SetUpControls
    /// <summary>
    /// Le asigna el maximo de escritura a los controles igual al que tienen en la BD
    /// </summary>
    /// <param name="obj">Objeto a validar las propiedades</param>
    /// <param name="ui">Contenedor donde va a buscar controles</param>
    /// <param name="enumMode">Modo en el que se abrió la ventana</param>
    /// <param name="blnCharacters">true. bloquea caracters especiales</param>
    /// <history>
    /// [emoguel] created 08/04/2016
    /// [emoguel] modified 11/07/2016
    /// [erosado] Modified. 12/08/2016. Se agrego para que acepte el MaxLenght de las cajas de texto o si no tuviera aceptaria las de la propiedad MaxLengthPropertyClass
    /// </history>
    public static void SetUpControls<T>(T obj, UIElement ui, EnumMode enumMode = EnumMode.ReadOnly, bool blnCharacters = false, EnumDatabase database = EnumDatabase.IntelligentMarketing) where T : class
    {
      List<Control> lstControls = GetChildParentCollection<Control>(ui);//Obtenemos la lista de controles del contenedor
      List<Model.Classes.ColumnDefinition> lstColumnsDefinitions = BRHelpers.GetFieldsByTable<T>(obj, database);

      Type type = obj.GetType();//Obtenemos el tipo de objeto
      if (lstControls.Count > 0)
      {

        #region DataGrid
        List<DataGrid> lstDataGrids = lstControls.Where(cl => cl is DataGrid && !((DataGrid)cl).IsReadOnly).OfType<DataGrid>().ToList();
        lstDataGrids.ForEach(dtg => dtg.Sorting += GridHelper.dtg_Sorting);
        #endregion

        #region Obtenemos el MaxLength
        foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => !pi.GetMethod.IsVirtual))//recorremos las propiedades
        {
          //buscamos si existe el control
          Control control = lstControls.FirstOrDefault(cl => cl.Name == "txt" + pi.Name);
          //Buscamos la descripción de la columna
          var columnDefinition = lstColumnsDefinitions.FirstOrDefault(cd => cd.column == pi.Name);
          #region tooltip
          //var controlTooltip = lstControls.FirstOrDefault(cl => cl.Name.EndsWith(pi.Name));
          //if(controlTooltip!=null && columnDefinition!=null && !string.IsNullOrWhiteSpace(columnDefinition.description))
          //{
          //  controlTooltip.ToolTip = columnDefinition.description;
          //}
          #endregion

          if (control != null && columnDefinition != null)//Verifcamos que tengamos un control
          {
            TextBox txt = control as TextBox;//Convertimos el control a texbox
            TypeCode typeCode = Type.GetTypeCode(Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType);
            int maxLengthProp = MaxLengthPropertyClass.GetMaxLength(txt) == 0 ? txt.MaxLength : MaxLengthPropertyClass.GetMaxLength(txt);
            EnumFormatInput formatInput = FormatInputPropertyClass.GetFormatInput(txt);//Formato del campo de texto
            switch (typeCode)
            {
              #region String
              case TypeCode.String:
              case TypeCode.Char:
                {
                  txt.MaxLength = (maxLengthProp > 0) ? maxLengthProp : columnDefinition.maxLength;//Asignamos el maxLength                  
                  if (formatInput == EnumFormatInput.NotSpecialCharacters)
                  {
                    txt.PreviewTextInput += TextBoxHelper.TextInputSpecialCharacters;
                  }
                  break;
                }
              #endregion

              #region Decimal
              case TypeCode.Decimal:
              case TypeCode.Double:
                {
                  //Si permite decimales
                  if (columnDefinition.scale > 0 || PrecisionPropertyClass.GetPrecision(txt) != "0,0")
                  {
                    if (PrecisionPropertyClass.GetPrecision(txt) == "0,0")
                    {
                      PrecisionPropertyClass.SetPrecision(txt, columnDefinition.precision - columnDefinition.scale + "," + columnDefinition.scale);
                    }
                    txt.PreviewTextInput += TextBoxHelper.DecimalTextInput;
                    txt.PreviewKeyDown += TextBoxHelper.Decimal_PreviewKeyDown;
                    txt.GotFocus += TextBoxHelper.DecimalGotFocus;
                    if (enumMode != EnumMode.Search)
                    {
                      txt.LostFocus += TextBoxHelper.LostFocus;
                    }
                  }
                  //Si sólo permite enteros
                  else
                  {
                    txt.PreviewTextInput += TextBoxHelper.IntTextInput;
                    txt.PreviewKeyDown += TextBoxHelper.ValidateSpace;
                    if (enumMode != EnumMode.Search)
                    {
                      txt.LostFocus += TextBoxHelper.LostFocus;
                      txt.GotFocus += TextBoxHelper.IntGotFocus;
                    }
                  }
                  txt.MaxLength = (maxLengthProp > 0) ? maxLengthProp : columnDefinition.maxLength;
                  break;
                }
              #endregion

              #region Byte
              case TypeCode.Byte:
                {
                  txt.MaxLength = (maxLengthProp > 0 && maxLengthProp <= 3) ? maxLengthProp : 3;
                  txt.PreviewTextInput += TextBoxHelper.ByteTextInput;
                  txt.PreviewKeyDown += TextBoxHelper.ValidateSpace;
                  if (enumMode != EnumMode.Search)
                  {
                    txt.LostFocus += TextBoxHelper.LostFocus;
                  }
                  break;
                }
              #endregion

              #region Int
              case TypeCode.Int16:
              case TypeCode.Int32:
              case TypeCode.Int64:
                {

                  switch (formatInput)
                  {
                    case EnumFormatInput.Number:
                      txt.PreviewTextInput += TextBoxHelper.IntTextInput;
                      txt.MaxLength = (maxLengthProp > 0) ? maxLengthProp : columnDefinition.maxLength;
                      break;
                    case EnumFormatInput.NumberNegative:
                      txt.PreviewTextInput += TextBoxHelper.IntWithNegativeTextInput;
                      txt.MaxLength = (maxLengthProp > 0) ? maxLengthProp + 1 : columnDefinition.maxLength + 1;
                      break;
                  }
                  txt.PreviewKeyDown += TextBoxHelper.ValidateSpace;
                  if (enumMode != EnumMode.Search)
                  {
                    txt.LostFocus += TextBoxHelper.LostFocus;
                  }
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
    /// <typeparam name="T">tipo de "child" a buscar </typeparam>
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
    /// <typeparam name="T">tipo de control</typeparam>
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

    #region GetParentCollection
    /// <summary>
    /// Obtiene los contenedores del control o sus "parents"
    /// </summary>
    /// <typeparam name="T">Tipo de contenedor a buscar</typeparam>
    /// <param name="child">control a buscar sus parents</param>
    /// <returns>Lista de contenedores</returns>
    /// <history>
    /// [emoguel] created 09/08/2016
    /// </history>
    public static List<T> GetParentCollection<T>(object child) where T : DependencyObject
    {
      List<T> logicalCollection = new List<T>();
      GetParentCollection(child as DependencyObject, logicalCollection);
      return logicalCollection;
    }
    #endregion

    #region GetParentCollection

    /// <summary>
    /// Obtiene todos los controles dentro de los contenedores
    /// </summary>
    /// <typeparam name="T">tipo de contenedor</typeparam>
    /// <param name="child">control</param>
    /// <param name="logicalCollection">lista de controles</param>
    /// <history>
    /// [emoguel] created 09/08/2016
    /// </history>
    public static void GetParentCollection<T>(DependencyObject child, List<T> logicalCollection) where T : DependencyObject
    {
      //Get parent parent
      var parent = LogicalTreeHelper.GetParent(child);

      if (parent is DependencyObject)
      {
        DependencyObject depParent = child as DependencyObject;
        if (parent is T)
        {
          logicalCollection.Add(parent as T);
        }
        GetParentCollection(parent, logicalCollection);
      }
    }
    #endregion

    #region UiSetDatacontext
    /// <summary>
    /// Asigna el datacontex a cada control del formulario dependiendo del ID y del tipo de dato
    /// </summary>
    /// <param name="obj">Objeto con las proiedades</param>
    /// <param name="ui">Formulario al que se le asignará los bindings</param>
    /// <param name="bindingMode">Modo de sincronizado del control y el objeto</param>
    /// <history>
    /// [emoguel] created 21/04/2016
    /// </history>
    public static void UiSetDatacontext(object obj, Window ui, BindingMode bindingMode = BindingMode.TwoWay)
    {
      List<Control> lstControls = GetChildParentCollection<Control>(ui);//Obtenemos la lista de controles del contenedor      

      Type type = obj.GetType();//Obtenemos el tipo de objeto
      if (lstControls.Count > 0)
      {
        #region Obtenemos el MaxLength

        foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => !pi.GetMethod.IsVirtual))//recorremos las propiedades
        {
          TypeCode typeCode = Type.GetTypeCode(Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType);
          switch (typeCode)
          {
            #region Texbox,ComboBox
            case TypeCode.Int32:
            case TypeCode.Int16:
            case TypeCode.Int64:
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.String:
              {
                Control control = lstControls.Where(cl => cl.Name.EndsWith(pi.Name)).FirstOrDefault();//buscamos si existe el control                

                if (control != null)
                {
                  Binding binding = new Binding();
                  binding.Mode = bindingMode;
                  binding.Path = new PropertyPath(pi.Name);
                  if (control is TextBox)
                  {
                    control.SetBinding(TextBox.TextProperty, binding);
                  }
                  else if (control is ComboBox)
                  {
                    control.SetBinding(ComboBox.SelectedValueProperty, binding);
                  }
                }
                break;
              }
            #endregion

            #region TextBox Currency
            case TypeCode.Double:
            case TypeCode.Decimal:
              {
                Control control = lstControls.Where(cl => cl.Name.EndsWith(pi.Name)).FirstOrDefault();//buscamos si existe el control                

                if (control != null)
                {
                  Binding binding = new Binding();
                  binding.Mode = bindingMode;
                  binding.Path = new PropertyPath(pi.Name);
                  if (bindingMode != BindingMode.OneWay)
                  {
                    binding.StringFormat = "{0:C}";
                  }
                  control.SetBinding(TextBox.TextProperty, binding);
                }
                break;
              }
            #endregion

            #region CheckBox
            case TypeCode.Boolean:
              {
                Control control = lstControls.Where(cl => cl.Name == "chk" + pi.Name).FirstOrDefault();//buscamos si existe el control                
                if (control != null)
                {
                  Binding binding = new Binding();
                  binding.Mode = bindingMode;
                  binding.Path = new PropertyPath(pi.Name);
                  binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                  control.SetBinding(CheckBox.IsCheckedProperty, binding);
                }
                break;
              }
            #endregion

            #region DateTime
            case TypeCode.DateTime:
              {
                Control control = lstControls.Where(cl => cl.Name.EndsWith(pi.Name)).FirstOrDefault();//buscamos si existe el control                

                if (control != null)
                {
                  Binding binding = new Binding();
                  binding.Mode = bindingMode;
                  binding.Path = new PropertyPath(pi.Name);
                  control.SetBinding(DatePicker.SelectedDateProperty, binding);
                }
                break;
              }
              #endregion

          }

        }
        #endregion
      }

    }
    #endregion

    #region GetMessageError
    /// <summary>
    /// Obtiene el Mensaje Original o mas bajo (InnerException) de un error
    /// </summary>
    /// <param name="exception"></param>
    /// [ecanul] 17/05/2016 Created
    public static string GetMessageError(Exception exception)
    {
      string message = exception.Message;
      if (exception.InnerException == null) return message;
      message = GetMessageError(exception.InnerException);
      return message;
    }
    #endregion

    #region UpdateTarget
    /// <summary>
    /// Refresca los controles para que se actualicen la informacion que tiene el objeto
    /// </summary>
    /// <param name="control">Control</param>
    /// <history>
    /// [erosado] 18/08/2016  Created.
    /// </history>
    public static void UpdateTarget(Control control)
    {
      List<Control> lstControls = GetChildParentCollection<Control>(control);//Obtenemos la lista de controles del contenedor
      lstControls.ForEach(cl =>
      {
        switch (cl.GetType().Name)
        {
          case "TextBox":
            {
              var bindingProperty = cl.GetBindingExpression(TextBox.TextProperty);
              if (bindingProperty != null)
              {
                bindingProperty.UpdateTarget();
              }
              break;
            }
          case "ComboBox":
            {
              var bindingProperty = cl.GetBindingExpression(ComboBox.SelectedValueProperty);
              if (bindingProperty != null)
              {
                bindingProperty.UpdateTarget();
              }
              break;
            }
          case "CheckBox":
            {
              var bindingProperty = cl.GetBindingExpression(CheckBox.IsCheckedProperty);
              if (bindingProperty != null)
              {
                bindingProperty.UpdateTarget();
              }
              break;
            }

        }
      });
    }
    #endregion

    #endregion Metodos
  }
}