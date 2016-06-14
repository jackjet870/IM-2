using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Host.Classes
{
  /// <summary>
  /// clase con metodos en comun
  /// </summary>
  /// <history>
  /// [vipacheco] 26/Mayo/2016 Created
  /// </history>
  public class Common
  {

    #region IsExternalProduct
    /// <summary>
    /// Determina si un regalo es manejado por un sistema externo
    /// </summary>
    /// <param name="ExternalProduct"></param>
    /// <param name="_Gift"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static bool IsExternalProduct(EnumExternalProduct ExternalProduct, string _Gift, ref Gift _GiftResult)
    {
      string FieldName = "";
      bool blnIs = false;


      // si es el monedero electronico
      if (ExternalProduct == EnumExternalProduct.expElectronicPurse)
        FieldName = "giProductGiftsCard";
      // si son las promociones de PVP
      else
        FieldName = "giPVPPromotion";

      // localizamos el regalo
      _GiftResult = frmHost._lstGifts.Where(x => x.giID == _Gift).First();
      Type type = _GiftResult.GetType();

      blnIs = type.GetProperty(FieldName).GetValue(_GiftResult, null) == null ? false : true;


      return blnIs;
    } 
    #endregion

  }
}
