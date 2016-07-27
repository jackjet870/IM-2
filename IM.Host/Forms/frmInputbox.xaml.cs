using IM.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmInputbox.xaml
  /// </summary>
  public partial class frmInputbox : Window
  {
    #region Atributos y Propiedades
    string strDefaultValue = ""; // string default que aparecera en el texbox
    static string strResult = "";
    string strMessage = "Message";// texto a mostrar para la captura en el texbox
    string strTitle = "InputBox";// titulo del inputbox
    string strError = "the field is empty";// mensaje de error
    bool typeText = false;// determina si es texto o numero que se captura en el texbox
    double imin = 1;// valor minimo ingresado en el texbox
    double imax = 1;// valor maximo ingresado en el texbox
    string strIsEqual = ""; //si el texbox debe ser igual a algun caracter
    bool IsValidMin = false;
    bool IsValidMax = false;
    bool IsValidEquals = false;
    double number = 0;
    #endregion

    #region frmInputbox
    public frmInputbox(string title, string message, string error = "", string defaultvalue = "", bool? isString = true, double? minLength = 0, double? maxLength = 256, string isEqual = "")
    {
      strTitle = title;
      strMessage = message;
      typeText = isString.Value;
      imax = maxLength.Value;
      imin = minLength.Value;
      strIsEqual = isEqual;
      strDefaultValue = defaultvalue;
      WindowStartupLocation = WindowStartupLocation.CenterScreen;
      strError = (!String.IsNullOrEmpty(error) && !String.IsNullOrWhiteSpace(error)) ? error : "the field is empty";
      InitializeComponent();
      LoadComponents();
    }
    #endregion

    #region LoadComponents
    /// <summary>
    /// Configura los componentes de la ventana
    /// </summary>
    /// <history>
    /// [michan] Created 30/Junio/2016
    /// </history>
    public void LoadComponents()
    {
      Title = strTitle;
      tbkMessage.Text = strMessage;
      Input.Text = strDefaultValue;
      if (!typeText) Input.PreviewTextInput += Decimal_PreviewTextInput;
      //regex =  (typeText) ? new System.Text.RegularExpressions.Regex("^[A-Za-z]+$") : new System.Text.RegularExpressions.Regex("^[A-Za-z]+$");
    }
    #endregion

    #region ValidateText
    /// <summary>
    /// Valida el contenido del Texbox de acuerdo a los parametros enviados
    /// </summary>
    /// <returns>Retorna true si cumple con las validaciones establecidas</returns>
    /// <history>
    /// [michan] Created 30/Junio/2016
    /// </history>
    public bool ValidateText()
    {
      if (typeText)
      {
        IsValidEquals = (strResult.Equals(strIsEqual)) ? true : false;
        IsValidMin = (strResult.Length >= imin) ? true : false;
        IsValidMax = (strResult.Length <= imax) ? true : false;
      }
      else
      {

        if (double.TryParse(strResult, out number))
        {
          IsValidEquals = (strResult.Equals(strIsEqual)) ? true : false;
          IsValidMin = (number >= imin) ? true : false;
          IsValidMax = (number <= imax) ? true : false;
        }
      }
      return (IsValidEquals || (IsValidMin && IsValidMax)) ? true : false;

    }
    #endregion

    #region Decimal_PreviewTextInput
    /// <summary>
    /// Valida que el texto introducido sea decimal
    /// </summary>
    /// <history>
    /// [michan] 23/07/2016 Created
    /// </history>
    private void Decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyDecimals(e.Text, sender as TextBox);
      if (e.Handled)
      {
        UIHelper.ShowMessage("Inser a valid number.", MessageBoxImage.Error, "Error");
      }
    }
    #endregion

    #region ShowError
    /// <summary>
    /// Envia el mensaje de error al usuario
    /// </summary>
    /// <history>
    /// [michan] Created 30/Junio/2016
    /// </history>
    public void ShowError()
    {
      if (!IsValidMin)
      {
        UIHelper.ShowMessage($"You can not insert less than {imin}", MessageBoxImage.Error, strTitle);
      }
      else if (!IsValidMax)
      {
        UIHelper.ShowMessage($"You can not insert more than {imax}", MessageBoxImage.Error, strTitle);
      }
      else
      {
        UIHelper.ShowMessage("The input value is not valid", MessageBoxImage.Error, strTitle);
      }
    }
    #endregion

    #region btnAccept_Click
    /// <summary>
    /// Retorna el valor establecido en el texbox
    /// </summary>
    /// <history>
    /// [michan] Created 30/Junio/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      if (!String.IsNullOrEmpty(Input.Text) && !String.IsNullOrWhiteSpace(Input.Text))
      {
        strResult = Input.Text;
        if (ValidateText())
        {
          DialogResult = true;
          Close();
        }
        else
        {
          ShowError();
        }
      }
      else
      {
        UIHelper.ShowMessage(strError, MessageBoxImage.Error, strTitle);
      }
    }
    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }
    #endregion
  }
}
