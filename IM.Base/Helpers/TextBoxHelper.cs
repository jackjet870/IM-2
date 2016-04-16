using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.Base.Helpers
{
  public class TextBoxHelper
  {
    public static void Properties(TextBox textbox,TypeCode typeCode)
    {
      switch(typeCode)
      {
        #region Byte
        case TypeCode.Byte:
          {
            textbox.LostFocus += txt_LostFocus;
            //textbox.PreviewTextInput+=
            break;
          } 
          #endregion
      }      
    }

    #region LostFocus
    /// <summary>
    /// Asigna valor por defecto 0
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/04/2016
    /// </history>
    private static void txt_LostFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (string.IsNullOrWhiteSpace(txt.Text))
      {
        txt.Text = "0";
      }
    }
    #endregion

    #region DecimalTextInput
    /// <summary>
    /// verifica que unicamente acepte numero y 1 punto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/04/2016
    /// </history>
    private void DecimalTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (e.Text == "." && !txt.Text.Trim().Contains("."))
      {
        e.Handled = false;
      }
      else
      {
        e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
      }
    }
    #endregion

    #region ByteTextInput
    /// <summary>
    /// Valida que solo se puedan escribir Números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/04/2016
    /// </history>
    private void ByteTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (txt.Text.Trim().Count() == 2 && ValidateHelper.OnlyNumbers(e.Text))
      {
        int nLevel = Convert.ToInt32(txt.Text.Trim() + e.Text);
        if (nLevel > 255)
        {
          txt.Text = "255";
        }
      }
      else
      {
        e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
      }
    }
    #endregion

    #region Textbox OnlyNumber
    /// <summary>
    /// Valida que un texbox acepte solo números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/04/2016
    /// </history>
    private void PreviewTextInputNumber(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }
    #endregion
  }
}
