using IM.Base.Helpers;
using IM.Host.Forms;
using IM.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace IM.Host.Classes
{

  internal class GiftsReceiptsValidationGrid : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      // Obtenemos el binding del row
      /*BindingGroup bindingGroup = value as BindingGroup;

      if (bindingGroup != null)
      {
        // Recorremos el grupo
        foreach (var bindingSource in bindingGroup.Items)
        {
          // Obtenemos el tipo de objeto
          GiftsReceiptDetailShort _giftRow = bindingSource as GiftsReceiptDetailShort;

          // Se realizan las validaciones necesarias!
          if (_giftRow.geInPVPPromo)
          {
            UIHelper.ShowMessage("Why do you think we called it 'High' and 'Low", System.Windows.MessageBoxImage.Information);
            return new ValidationResult(false, "Why do you think we called it 'High' and 'Low' ?");
          }
        }
      }

      // Everything OK.*/
      return ValidationResult.ValidResult;
    }
  }
}
