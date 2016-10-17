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
      lstOptions.Add(new { sName = "ALL", sValue = -1 });
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

    #region ConfigureDates
    /// <summary>
    /// Carga el combobox de Predefined Date dependiendo el tipo de periodo
    /// </summary>
    /// <param name="cmbDate">Combobox a llenar</param>
    /// <param name="enumPeriod">Tipo de periodo</param>
    /// <param name="selectedCmb">index a seleccionar en el combo</param>
    /// <history>
    /// [emoguel] created 11/05/2016
    /// </history>
    public static void ConfigureDates(ComboBox cmbDate, EnumPeriod enumPeriod, int selectedCmb = 0)
    {
      Dictionary<EnumPredefinedDate, string> dictionaryPredefinedDate = EnumToListHelper.GetList<EnumPredefinedDate>();
      cmbDate.Items.Clear();
      cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.DatesSpecified));

      switch (enumPeriod)
      {
        //Sin periodo
        case EnumPeriod.None:

          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.Today));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.Yesterday));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisWeek));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousWeek));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisHalf));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousHalf));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisMonth));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousMonth));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisYear));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousYear));
          break;

        //Semanal
        case EnumPeriod.Weekly:
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisWeek));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousWeek));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.TwoWeeksAgo));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThreeWeeksAgo));
          break;

        //Mensual
        case EnumPeriod.Monthly:
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisMonth));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousMonth));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.TwoMonthsAgo));
          cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThreeMonthsAgo));
          break;
      }
      cmbDate.SelectedIndex = selectedCmb;
    }
    #endregion
  }
}
