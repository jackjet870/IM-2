using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;

namespace IM.Host.Classes
{
  public class objRptGuestRegistrationIM : RptGuestRegistration {
    public DateTime ShowD => guShowD ?? new DateTime(1899, 12, 31);
    public DateTime TimeInT => guTimeInT ?? new DateTime(1899, 12, 31);
    public byte Reimpresion => guReimpresion ?? 0;
    public byte[] imageTitle => GetImageTitle(gula);
    public objRptGuestRegistrationIM(RptGuestRegistration parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }

    private byte[] GetImageTitle(string gula)
    {
      Uri oUri = new Uri($"pack://application:,,,/IM.Styles;component/Images/{((gula == "ES") ? "RegistroHuespedes.jpg" : "GuestRegistration.jpg")}", UriKind.Absolute);
      var imageC = new BitmapImage(oUri);
      MemoryStream memStream = new MemoryStream();
      JpegBitmapEncoder encoder = new JpegBitmapEncoder();
      encoder.Frames.Add(BitmapFrame.Create(imageC));
      encoder.Save(memStream);
      return memStream.ToArray();
    }
  }

  public class objRptGuestRegistrationGuestIM : RptGuestRegistration_Guests
  {
    public byte guAge => Age ?? 0;
    public objRptGuestRegistrationGuestIM(RptGuestRegistration_Guests parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }
  public class objRptGuestRegistrationDepositsIM : RptGuestRegistration_Deposits {
    public objRptGuestRegistrationDepositsIM(RptGuestRegistration_Deposits parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }

  public class objRptGuestRegistrationGiftsIM : RptGuestRegistration_Gifts
  {
    public int guesID => guID ?? 0;
    public objRptGuestRegistrationGiftsIM(RptGuestRegistration_Gifts parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }
  public class objRptGuestRegistrationSalesmenIM : RptGuestRegistration_Salesmen {
    public objRptGuestRegistrationSalesmenIM(RptGuestRegistration_Salesmen parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }
  public class objRptGuestRegistrationCommentsIM : RptGuestRegistration_Comments {
    public objRptGuestRegistrationCommentsIM(RptGuestRegistration_Comments parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }
  public class objRptGuestRegistrationCreditCardsIM : RptGuestRegistration_CreditCards {
    public objRptGuestRegistrationCreditCardsIM(RptGuestRegistration_CreditCards parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }
}
