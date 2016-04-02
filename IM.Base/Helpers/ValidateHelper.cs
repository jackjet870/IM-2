﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace IM.Base.Helpers
{
  public class ValidateHelper
  {
    #region OnlyNumbers
    /// <summary>
    /// Valida que el texto sea sólo números un número
    /// </summary>
    /// <param name="text"></param>
    /// <returns>true es un númerico | false No es númerico </returns>
    /// <history>
    /// [Emoguel] created 02/03/2015
    /// </history>
    public static bool OnlyNumbers(string text)
    {
      Regex regex = new Regex("^[0-9]+$");
      return regex.IsMatch(text);
    }
    #endregion

    #region ValidateRequired

    /// <summary>
    /// Valida que un control no este vacio
    /// </summary>
    /// <param name="control">Control</param>
    /// <param name="description">Descripcion del campo</param>
    /// <param name="message">Mensaje especial de que el campo no tiene informacion</param>
    /// <param name="condition">Condicion que se debe cumplir para validar que el campo no este vacio</param>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// </history>
    public static bool ValidateRequired(Control control, string description = "", string message = "", bool condition = true)
    {
      // si la condicion es valida
      if (!condition) return true;

      // obtenemos el valor ingresado
      string value = GetValue(control);

      // si no se ingreso un valor
      if (string.IsNullOrEmpty(value))
      {
        // si se envio la descripcion del campo
        if (!string.IsNullOrEmpty(description))
        {
          message = string.Format("Specify the {0}.", description);
        }
        else
        {
          // si no se envio el mensaje
          if (!string.IsNullOrEmpty(message))
          {
            message = "Specify a value";
          }
        }
        UIHelper.ShowMessage(message);
        control.Focus();
        return false;
      }

      return true;
    }

    #endregion

    #region GetValue

    /// <summary>
    /// Obtiene el valor de un control
    /// </summary>
    /// <param name="control">Control</param>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// [jorcanche]  24/03/2016 Modificado
    /// </history>
    public static string GetValue(Control control)
    {
      string value = string.Empty;

      if (control is TextBox)
      {
        value = ((TextBox)control).Text;
      }
      else
      {
        if (control is PasswordBox)
        {
          value = ((PasswordBox)control).Password;
        }
      }
      return value;
    }
    #endregion

    #region ValidateChangedBy
    /// <summary>
    /// Valida que se ingrese quien hizo el cambio y su contraseña
    /// </summary>
    /// <param name="ptxtChangedBy">Control de tipo TextBox en donde se ingresa el usuario</param>
    /// <param name="ptxtPwd">Control de tipo PaswordBox en donde se ingresa el password del usuario</param>
    /// <param name="pstrUserType">Cadena en donde se ingresa el tipo de usuario por ejemplo "PR"</param>
    /// <hitory> [jorcanche] 24/03/2016 </hitory>
    /// <returns></returns>

    public static bool ValidateChangedBy(TextBox ptxtChangedBy, PasswordBox ptxtPwd, string pstrUserType = "")
    {
      string strDescription = string.Empty;
      string strMessage = string.Empty;
      //establecemos el mensaje de error de quin hizo el cambio
      if (pstrUserType == "")
      {
        UIHelper.ShowMessage("Specify who is making the change.");
        return false;
      }
      else
      {
        strDescription = pstrUserType;
      }
      //validamos quien hizo el cambio
      if (!ValidateRequired(ptxtChangedBy, strDescription, strMessage))
      {
        ptxtChangedBy.Focus();
        return false;
      }
      //Validamos la contraseña de quin hizo el cambio
      if (!ValidateRequired(ptxtPwd, "Specify your password."))
      {
        ptxtPwd.Focus();
        return false;    
      }
      return true;
    }
    #endregion

    /// <summary>
    /// Valida que los datos de quien hizo el cambio y su contraseña existan
    /// </summary>
    /// <param name="ptxtChangedBy"></param>
    /// <param name="ptxtPwd"></param>
    /// <param name="pstrLeadSource"></param>
    /// <param name="pstrUserType"></param>
    /// <param name="ptxtPR"></param>
    /// <returns></returns>   
    public bool ValidateChangedByExist(TextBox ptxtChangedBy, PasswordBox ptxtPwd, string pstrLeadSource,
                                        string pstrUserType = "Changed By", TextBox ptxtPR = null)
    {
      //si se desea validar el PR
      if (ptxtPR != null)
      {


      }
      return true;
    }

    #region ValidateForm
    /// <summary>
    /// Valida que los texbox o combobox de un grid(contenedor) tengan que tener algun valor
    /// </summary>
    /// <param name="grid">Contenedor</param>
    /// <param name="strForm">Nombre del formulario</param>
    /// <returns>cadena de texto con el mensaje de los campos vacios</returns>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    public static string ValidateForm(UIElement container, string strForm)
    {
      string strMsj = "";      
      List<Control> lstControls = GetChildParentCollection<Control>(container);//buscamos todos los controles de la ventana
      lstControls = lstControls.Where(co => co.Tag != null).ToList();
      foreach (Control control in lstControls)
      {
        #region TextBox
        if (control is TextBox)//Si es Textbox
        {
          TextBox txt = (TextBox)control;
          if (string.IsNullOrWhiteSpace(txt.Text))
          {
            strMsj += "specify the " + strForm +" "+ txt.Tag.ToString() + ". \n";
          }
        }
        #endregion
        #region Combobox
        else if (control is ComboBox)
        {
          ComboBox cmb = (ComboBox)control;
          if (cmb.SelectedIndex < 0)
          {
            strMsj += "specify the " + strForm +" "+cmb.Tag.ToString() + ". \n";
          }
        } 
        #endregion
      }

      return strMsj.TrimEnd('\n');
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
    private static List<T> GetChildParentCollection<T>(object parent) where T : DependencyObject
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

  }
}
