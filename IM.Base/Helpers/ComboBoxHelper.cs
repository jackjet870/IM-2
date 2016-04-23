using System.Collections.Generic;
using System.Windows.Controls;

namespace IM.Base.Helpers
{
  public class ComboBoxHelper
  {
    /// <summary>
    /// Llena un combobox con opciones default
    /// </summary>
    /// <param name="comboBox">Combobox a llenar</param>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// </history>
    public static void LoadComboDefault(ComboBox comboBox)
    {
      List<object> lstOptions = new List<object>();
      lstOptions.Add(new { sName = "All", sValue = -1 });
      lstOptions.Add(new { sName = "Yes", sValue = 1 });
      lstOptions.Add(new { sName = "No", sValue = 0 });      
      comboBox.ItemsSource = lstOptions;
    }
  }
}
