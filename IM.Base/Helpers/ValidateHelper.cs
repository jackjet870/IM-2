using System.Text.RegularExpressions;
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
    /// </history>
    public static string GetValue(Control control)
    {
      string value = string.Empty;

      if (control is TextBox)
      {
        value = ((TextBox)control).Text;
      }

      return value;
    }

    #endregion
  }
}
