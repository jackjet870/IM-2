using System.Collections.Generic;
using System.Windows.Controls;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Linq;

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

    #region PopulateDates

    /// <summary>
    /// Llena una lista con las fechas predefinidas para cada periodo
    /// </summary>
    /// /// <param name="cboDate">Combobox a llenar</param>
    /// <param name="period">EnumPeriod para el llenado de fechas</param>
    /// <history>
    /// [ecanul] 26/04/2016 Created
    /// </history>
    public static void PopulateDates(ComboBox cboDate, EnumPeriod period = EnumPeriod.None)
    {
      Dictionary<EnumPredefinedDate, string> dictionaryPredefinedDate = EnumToListHelper.GetList<EnumPredefinedDate>();

      cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.DatesSpecified));

      switch (period)
      {
        //Sin periodo
        case EnumPeriod.None:

          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.Today));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.Yesterday));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisWeek));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousWeek));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisHalf));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousHalf));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisMonth));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousMonth));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisYear));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousYear));
          break;

        //Semanal
        case EnumPeriod.Weekly:
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisWeek));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousWeek));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.TwoWeeksAgo));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThreeWeeksAgo));
          break;

        //Mensual
        case EnumPeriod.Monthly:
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisMonth));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousMonth));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.TwoMonthsAgo));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThreeMonthsAgo));
          break;
      }
      cboDate.SelectedIndex = 0;
     // return cboDate;
    }

    #endregion

  }
}
