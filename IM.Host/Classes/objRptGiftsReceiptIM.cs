using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using System.Reflection;

namespace IM.Host.Classes
{
  public class objRptGiftsReceiptIM : RptGiftsReceipt
  {    
    public objRptGiftsReceiptIM(RptGiftsReceipt parent)
    {
     foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }

  public class objRptGiftsReceipt_GiftsIM : RptGiftsReceipt_Gifts {
    public objRptGiftsReceipt_GiftsIM(RptGiftsReceipt_Gifts parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  }
  public class objRptGiftsReceipt_ProductLegendsIM : RptGiftsReceipt_ProductLegends {
    public objRptGiftsReceipt_ProductLegendsIM(RptGiftsReceipt_ProductLegends parent)
    {
      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }
  } 
} 