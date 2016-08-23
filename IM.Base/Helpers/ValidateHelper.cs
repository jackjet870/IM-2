using System;
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
    public static bool OnlyDecimals(string text, TextBox control)
    {
      Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
      return regex.IsMatch((control).Text.Insert((control).SelectionStart, text));
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
          if (string.IsNullOrEmpty(message))
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
    /// [aalcocer] 12/08/2016 Modified. Ahora soporta un DateTimePicker
    /// </history>
    public static string GetValue(Control control)
    {
      string value = string.Empty;

      if (control is TextBox)
      {
        value = ((TextBox)control).Text;
      }
      else if (control is PasswordBox)
      {
        value = ((PasswordBox)control).Password;
      }
      else if (control is ComboBox)
      {
        value = ((ComboBox)control).Text;
      }
      else if (control is DateTimePicker)
      {
        value = ((DateTimePicker)control).Text;
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
    /// <history> 
    /// [jorcanche] 24/03/2016 Created.
    /// [aalcocer] 12/08/2016 Modified. Se corrige la validación
    /// </history>
    /// <returns></returns>

    public static bool ValidateChangedBy(TextBox ptxtChangedBy, PasswordBox ptxtPwd, string pstrUserType = "")
    {
      string strDescription = string.Empty;
      string strMessage = string.Empty;
      //establecemos el mensaje de error de quin hizo el cambio
      if (pstrUserType == "")
      {
        strMessage = "Specify who is making the change.";
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
    /// <param name="container">Contenedor</param>   
    /// <param name="strForm">Nombre del formulario</param>
    /// <param name="blnValidateVisibility">valida la visibilidad de los controles (TRUE - Para validar solo los controles visibles | FALSE - Para validar todos los controles) </param>
    /// <param name="blnDatagrids">Valida que los datagrids ya eno estén en modo edición</param>
    /// <returns>cadena de texto con el mensaje de los campos vacios</returns>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// [erosado] 02/05/2016  Modified. Se agrego la validacion de controles visibles y si es del tipo PasswordBox
    /// [vipacheco] Modified 14/Julio/2016 Se agrego parametro de validacion de visibilidad
    /// [jorcanche] Modified 19/07/2016 Se simplifico el metodo, se documento el parametro validateVisible
    /// [emoguel] modified Se cambió los IsNullorWhiteSpace por IsNullOrEmpty
    /// [emoguel] modified 01/08/2016-->Se le quitan los espacios a los que sean tipo Texto
    /// [erosado] 05/08/2016  Modified. Se corrigió la validacion del DateTimePicker, y se agrego una bandera para que el metodo se encargue de mandar el ShowMessage
    /// [erosado] 23/08/2016  Modified. Se optimizo el codigo, se agregó switch case en lugar de if-else, se corrigio que mande el focus a el primer elmento que falle.
    /// </history>
    public static string ValidateForm(UIElement container, string strForm, bool blnDatagrids = false, bool showMessage = false)
    {
      var strMsj = "";
      var lstControls = UIHelper.GetChildParentCollection<Control>(container);
      var listInvalid = new List<Control>();

      if (blnDatagrids)
      {
        var lstGrids = lstControls.OfType<DataGrid>().ToList();

        foreach (DataGrid dgr in lstGrids)
        {
          if (GridHelper.IsInEditMode(dgr, false))
          {
            var parents = UIHelper.GetParentCollection<TabItem>(dgr);
            parents.ForEach(tb => tb.IsSelected = true);
            container.UpdateLayout();
            dgr.Focus();
            return "Please finish editing the list. \n";
          }
        }
      }
      lstControls = lstControls.Where(co => co.Tag != null).ToList();

      foreach (var control in lstControls)
      {
        switch (control.GetType().Name)
        {
          case "TextBox":
            var txt = (TextBox)control;
            #region Remover espacios
            txt.Text = txt.Text.Trim();
            var binding = txt.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
            #endregion
            if (!string.IsNullOrWhiteSpace(txt.Text)) continue;
            if (txt.Visibility == Visibility.Visible)
            {
              strMsj += $"Specify the {strForm}  { txt.Tag }. \n";
              listInvalid.Add(control);
            }
            break;

          case "ComboBox":
            var cmb = (ComboBox)control;
            if (cmb.SelectedIndex > -1) continue;
            if (cmb.Visibility == Visibility.Visible)
            {
              strMsj += $"Specify the {strForm } {cmb.Tag}. \n";
              listInvalid.Add(control);
            }
            break;

          case "PasswordBox":
            var pwd = (PasswordBox)control;
            #region Remover espacios
            pwd.Password = pwd.Password.Trim();
            #endregion
            if (!string.IsNullOrWhiteSpace(pwd.Password.Trim())) continue;
            if (pwd.Visibility == Visibility.Visible)
            {
              strMsj += $"Specify the {strForm } {pwd.Tag}. \n";
              listInvalid.Add(control);
            }
            break;

          case "DateTimePicker":
            var dtp = (DateTimePicker)control;

            if (dtp.Value.HasValue && !dtp.Value.Equals(DateTime.MinValue)) continue;
            if (dtp.Visibility == Visibility.Visible)
            {
              strMsj += $"Specify the {strForm} {dtp.Tag}. \n";
              listInvalid.Add(control);
            }
            break;
        }
      }

      if (strMsj != "")
      {
        var control = listInvalid.FirstOrDefault();
        var parents = UIHelper.GetParentCollection<TabItem>(control);
        parents.ForEach(tb => tb.IsSelected = true);
        container.UpdateLayout();
        control?.Focus();
        //Si la showMessage viene activa muestra el mensaje
        if (showMessage)
        {
          UIHelper.ShowMessage(strMsj.TrimEnd('\n'), title: "Intelligence Marketing");
        }
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

    #region validateCharacters
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
      List<string> lstCharacters = new List<string> { "", "%", "'", "-", "_" };
      text.ToCharArray().ToList().ForEach(c =>
      {
        if (lstCharacters.Contains(c.ToString()))
        {
          _blnValid = false;
          return;
        }
      });
      return _blnValid;
    }
    #endregion

    #region  ValidateNumber
    /// <summary>
    /// Valida que un numero este en un determinado rango
    /// </summary>
    /// <param name="number">Numero</param>
    /// <param name="lowerBound">Limite inferior</param>
    /// <param name="upperBound">Limite superior</param>
    /// <param name="description">Nombre del campo</param>
    /// <returns>True is Valid | False No</returns>
    /// <history>
    /// [erosado] 06/08/2016  Created.
    /// [aalcocer] 11/08/2016 Modified. Se cambian los parametros a decimales
    /// </history>
    public static bool ValidateNumber(decimal number, decimal lowerBound, decimal upperBound, string description)
    {
      bool _isValid = true;
      if (!(lowerBound <= number && number <= upperBound))
      {
        UIHelper.ShowMessage($"{description} is out of range. Allowed values are {lowerBound} to {upperBound}.");
        _isValid = false;
      }
      return _isValid;
    }
    #endregion
  }
}
