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
using IM.Base.Helpers;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for InputBox.xaml
  /// </summary>
  public partial class InputBox : Window
  {
    System.Text.RegularExpressions.Regex regex;

    string strDefaultValue = "";
    static string strResult = "";
    string strMessage = "Message";
    string strTitle = "InputBox";
    string strError = "the field is empty";
    bool typeText = false;
    double imin = 1;
    double imax = 1;
    string strIsEqual = "";
    bool IsValidMin = false;
    bool IsValidMax = false;
    bool IsValidEquals = false;
    double number = 0;
    public InputBox(string title, string message, string error = "", string defaultvalue = "", bool? isString = true, double? minLength = 0, double? maxLength = 256, string isEqual = "")
    {
      strTitle = title;
      strMessage = message;
      typeText = isString.Value;
      imax = maxLength.Value;
      imin = minLength.Value;
      strIsEqual = isEqual;
      strDefaultValue = defaultvalue;
      strError = (!String.IsNullOrEmpty(error) && !String.IsNullOrWhiteSpace(error)) ? error : "the field is empty";
      InitializeComponent();
      LoadComponents();
    }

    public void LoadComponents()
    {
      Title = strTitle;
      tbkMessage.Text = strMessage;
      Input.Text = strDefaultValue;
      regex =  (typeText) ? new System.Text.RegularExpressions.Regex("^[A-Za-z]+$") : new System.Text.RegularExpressions.Regex("^[A-Za-z]+$");
    }

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

    private void btnAcept_Click(object sender, RoutedEventArgs e)
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

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }
  }
}
