﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

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

    #region OnlyDecimals
    /// <summary>
    /// Valida que el texto sea sólo números de tipo decimal
    /// </summary>
    /// <param name="text"></param>
    /// <returns>true es un númerico | false No es númerico </returns>
    /// <history>
    /// [vipacheco] created 30/Junio/2016 Created
    /// </history>
    public static bool OnlyDecimals(string text, TextBox sender)
    {
      Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
      return regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, text));
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
    /// [vku] 04/Jul/2016 Modified. Ahora soporta un combobox
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
        else
        {
          if (control is ComboBox)
          {
            value = ((ComboBox)control).Text;
          }
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
      else if (!ValidateRequired(ptxtPwd, "Specify your password."))
      {
        ptxtPwd.Focus();
        return false;
      }
      return true;
    }
    #endregion  

    #region ValidateForm
    /// <summary>
    /// Valida que los texbox o combobox de un grid(contenedor) tengan que tener algun valor
    /// </summary>
    /// <param name="grid">Contenedor</param>
    /// <param name="strForm">Nombre del formulario</param>
    /// <returns>cadena de texto con el mensaje de los campos vacios</returns>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// [erosado] Modified  02/05/2016  Se agrego la validacion de controles visibles y si es del tipo PasswordBox
    /// [vipacheco] Modified 14/Julio/2016 Se agrego parametro de validacion de visibilidad
    /// </history>
    public static string ValidateForm(UIElement container, string strForm, bool validateVisible = true)
    {
      string strMsj = "";
      List<Control> lstControls = UIHelper.GetChildParentCollection<Control>(container);//buscamos todos los controles de la ventana
      lstControls = lstControls.Where(co => co.Tag != null).ToList();
      foreach (Control control in lstControls)
      {
        #region TextBox
        if (control is TextBox)//Si es Textbox
        {
          TextBox txt = (TextBox)control;

          if (validateVisible)
          {
            if (txt.IsVisible && string.IsNullOrWhiteSpace(txt.Text))
              strMsj += "Specify the " + strForm + " " + txt.Tag.ToString() + ". \n";
          }
          else
          {
            if (!validateVisible && string.IsNullOrWhiteSpace(txt.Text))
              strMsj += "Specify the " + strForm + " " + txt.Tag.ToString() + ". \n";
          }
        }
        #endregion

        #region Combobox
        else if (control is ComboBox)
        {
          ComboBox cmb = (ComboBox)control;
          if (validateVisible)
          {
            if (cmb.IsVisible && cmb.SelectedIndex < 0)
              strMsj += "Specify the " + strForm + " " + cmb.Tag.ToString() + ". \n";
          }
          else
          {
            if (cmb.SelectedIndex < 0)
              strMsj += "Specify the " + strForm + " " + cmb.Tag.ToString() + ". \n";
          }
        }
        #endregion

        #region PasswordBox
        else if (control is PasswordBox)
        {
          PasswordBox pwd = (PasswordBox)control;
          if (validateVisible)
          {
            if (pwd.IsVisible && string.IsNullOrWhiteSpace(pwd.Password))
              strMsj += "Specify the " + strForm + " " + pwd.Tag.ToString() + ". \n";
          }
          else 
          {
            if (string.IsNullOrWhiteSpace(pwd.Password))
              strMsj += "Specify the " + strForm + " " + pwd.Tag.ToString() + ". \n";
          }
        }
        #endregion

        else if (control is DateTimePicker)
        {
          DateTimePicker dtp = (DateTimePicker)control;
          if (validateVisible)
          {
            if (dtp.IsVisible && string.IsNullOrWhiteSpace(dtp.Text))
              strMsj += "Specify the " + strForm + " " + dtp.Tag.ToString() + ". \n";
          }
          else
          {
            if (string.IsNullOrWhiteSpace(dtp.Text))
              strMsj += "Specify the " + strForm + " " + dtp.Tag.ToString() + ". \n";
          }
        }
      }
      if (strMsj != "")//Mandamos el foco al primer campo
      {
        lstControls.FirstOrDefault().Focus();
      }
      return strMsj.TrimEnd('\n');
    }

    #endregion

    #region IsValidEmail
    /// <summary>
    /// Valida la dirección de correo electronico que sea correcta
    /// </summary>
    /// <param name="strEmail">Correo electronico a validar</param>
    /// <returns>Retorna falso si no es correcto en su caso verdadero si es correcto</returns>
    public static bool IsValidEmail(string strEmail)
    {
      string emailFormat = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

      if (Regex.IsMatch(strEmail, emailFormat))
      {
        if (Regex.Replace(strEmail, strEmail, string.Empty).Length == 0)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      else
      {
        return false;
      }
    }
    #endregion

    #region IsValidTimeFormat
    /// <summary>
    ///   Valida formato de hora 12hr or 24hr
    /// </summary>
    /// <param name="text"></param>
    /// <history>
    ///   [vku] 27/Jun/2016 Created
    /// </history>
    public static bool IsValidTimeFormat(string text)
    {
      Regex regex = new Regex(@"^(?:(?:0?[0-9]|1[0-2]):[0-5][0-9] [ap]m|(?:[01][0-9]|2[0-3]):[0-5][0-9])$", RegexOptions.IgnoreCase);
      return regex.IsMatch(text);
    }
    #endregion

    #region IsDate
    /// <summary>
    ///   Valida si es una fecha u hora
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// <history>
    ///   [vku] 08/Jul/2016 Created
    /// </history>
    public static bool IsDate(Object obj)
    {
      string strDate = obj.ToString();
      try
      {
        DateTime dt = DateTime.Parse(strDate);
        if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
          return true;
        return false;
      }
      catch
      {
        return false;
      }
    }
    #endregion

    /// <summary>
    /// Valida que el texto no contenga los caracteres de una lista
    /// </summary>
    /// <param name="text">texto a validar</param>
    /// <history>
    /// [emoguel] created 11/07/2016
    /// </history>
    /// <returns>True. No contiene los caracteres | false. Si contiene los caracteres</returns>
    public static bool validateCharacters(string text)
    {
      bool _blnValid = true;
      List<string> lstCharacters = new List<string> { "", "%", "'","-","_" };
      text.ToCharArray().ToList().ForEach(c => {
        if (lstCharacters.Contains(c.ToString()))
        {
          _blnValid = false;
          return;
        }
      });
      return _blnValid;
    }
  }
}
