using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.Base.Helpers
{
  public class TextBoxHelper
  {

    #region LostFocus
    /// <summary>
    /// Asigna valor por defecto 0
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/04/2016
    /// </history>
    public static void LostFocus(object sender, RoutedEventArgs e)
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
    public static void DecimalTextInput(object sender, TextCompositionEventArgs e)
    {      
      TextBox txt = (TextBox)sender;
      if (!(e.Text == "." && !txt.Text.Trim().Contains(".")))
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
    public static void ByteTextInput(object sender, TextCompositionEventArgs e)
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
    public static void IntTextInput(object sender, TextCompositionEventArgs e)
    {
      var txt = (TextBox) sender;
      var input = txt.Text.Insert(txt.SelectionStart, e.Text);
      if (txt.MaxLength > 0)
        e.Handled = !(Regex.IsMatch(input, @"^[0-9]*$") && input.Count(char.IsDigit) <= txt.MaxLength);
      else
        e.Handled = !Regex.IsMatch(input, @"^[0-9]*$");
    }

    #endregion

    #region Decimal_GotFocus
    /// <summary>
    /// Cambia de formato currency a standar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 18/04/2016
    /// </history>
    public static void DecimalGotFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      txt.Text = ConvertHelper.DoubleCurrencyToStandar(txt.Text.Trim());
    }
    #endregion

    #region Int_GotFocus
    /// <summary>
    /// Cambia de formato currency a standar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 18/04/2016
    /// </history>
    public static void IntGotFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      txt.Text = ConvertHelper.IntCurrencyToStandar(txt.Text.Trim());
    }
    #endregion

    #region PreviewTextLetter
    /// <summary>
    /// Verifica que solo se escriban Letras
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// </history>
    public static void LetterTextInput(object sender, TextCompositionEventArgs e)
    {
      if (!char.IsLetter(Convert.ToChar(e.Text)))
      {
        e.Handled = true;
      }
    }
    #endregion

    #region Textbox without special Character
    /// <summary>
    /// Valida que un texbox acepte solo números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/07/2016
    /// </history>
    public static void TextInputSpecialCharacters(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.validateCharacters(e.Text);
    }
    #endregion

    #region IntWithNegativeTextInput
    /// <summary>
    /// Valida que un texbox acepte solo números positivos o negativos.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [edgordriguez] 30/07/2016 Created.
    /// </history>
    public static void IntWithNegativeTextInput(object sender, TextCompositionEventArgs e)
    {
      var txt = (TextBox)sender;
      var input = txt.Text.Insert(txt.SelectionStart, e.Text);
      if (txt.MaxLength > 0)
          e.Handled = !(Regex.IsMatch(input, @"^[-]?[0-9]*$") && input.Count(char.IsDigit) <= txt.MaxLength - 1);
      else
        e.Handled = !Regex.IsMatch(input, @"^[-]?[0-9]*$");
    }
    #endregion

    #region validateSpace
    /// <summary>
    /// Valida la tecla espacio.
    /// </summary>
    /// <history>
    /// [edgordriguez] 30/07/2016 Created.
    /// </history>
    public static void ValidateSpace(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Space)
        e.Handled = true;
    }   
    #endregion
  }
}
