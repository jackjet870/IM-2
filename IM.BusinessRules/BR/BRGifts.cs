using IM.BusinessRules.Classes;
using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRGifts
  {
    #region GetGifts

    /// <summary>
    /// Método para obtener una lista de Regalos por lugar y/o estatus.
    /// </summary>
    /// <param name="location">Lugar o Almacén</param>
    /// <param name="Status">0. Sin filtro.
    /// 1. Activos.
    /// 2. Inactivos.</param>
    /// <history>
    /// [edgrodriguez] 24/Feb/2016 Created
    /// </history>
    public static List<GiftShort> GetGifts(string location = "ALL", int Status = 0)
    {
      List<GiftShort> lstgetGifs = new List<GiftShort>();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //dbContext.Database.CommandTimeout = 180;
        var dx = dbContext.USP_OR_GetGifts(location, Convert.ToByte(Status));
        lstgetGifs = dx.ToList();
      }
      return lstgetGifs;
    }

    #endregion GetGifts

    #region GetGiftsCategories

    /// <summary>
    /// Método para obtener una lista de categorias de regalo.
    /// </summary>
    /// <param name="Status">0. Sin filtro.
    /// 1. Activos.
    /// 2. Inactivos.</param>
    /// <returns> List<GiftsCateg> </returns>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    public static List<GiftCategory> GetGiftsCategories(int Status = 0)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstGiftsCateg = dbContext.GiftsCategories;
        switch (Status)
        {
          case 1:
            return lstGiftsCateg.Where(c => c.gcA == true).ToList();

          case 2:
            return lstGiftsCateg.Where(c => c.gcA == false).ToList();

          default:
            return lstGiftsCateg.ToList();
        }
      }
    }

    #endregion GetGiftsCategories

    #region GetGiftsByGuest

    /// <summary>
    /// Método para obtener una lista de Regalos por invitado
    /// </summary>
    /// <param name="guestID">Indetificador del invitado</param>
    /// <history>
    /// [lchairez] 10/Mar/2016 Created
    /// </history>
    public static List<InvitationGift> GetGiftsByGuest(int guestID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.InvitationsGifts.Where(i => i.iggu == guestID).ToList();
      }
    }

    #endregion GetGiftsByGuest

    #region GetGiftsInvitation

    /// <summary>
    /// Obtiene una lista especifica para cargar el grid de regalos de una invitación.
    /// </summary>
    /// <param name="guestId">Identificador del cliente</param>
    /// <returns>Lista de la clase GiftInvitation</returns>
    /// <history>
    /// [lchairez] 23/Mar/2016 Created
    /// </history>
    public static List<GiftInvitation> GetGiftsInvitation(int guestId)
    {
      var gifts = GetGiftsByGuest(guestId);

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var giftsInvit = (from gi in gifts
                          join g in dbContext.Gifts on gi.iggi equals g.giID
                          select new GiftInvitation
                          {
                            iggu = gi.iggu,
                            igQty = gi.igQty,
                            iggi = gi.iggi,
                            Gift = g.giN,
                            igAdults = gi.igAdults,
                            igMinors = gi.igMinors,
                            igExtraAdults = gi.igExtraAdults,
                            igPriceA = gi.igPriceA,
                            igPriceAdult = gi.igPriceAdult,
                            igPriceExtraAdult = gi.igPriceExtraAdult,
                            igPriceM = gi.igPriceM,
                            igPriceMinor = gi.igPriceMinor,
                            igct = gi.igct
                          }).ToList();
        return giftsInvit;
      }
    }

    #endregion GetGiftsInvitation

    #region GetGiftId

    /// <summary>
    /// Obtiene un regalo específico por si ID
    /// </summary>
    /// <param name="giftId">Identificador del regalo</param>
    /// <returns>Gifts</returns>
    /// <history>
    /// [lchairez] 23/03/2016 Created.
    /// </history>
    public static Gift GetGiftId(string giftId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.Gifts.Where(g => g.giID == giftId).SingleOrDefault();
      }
    }

    #endregion GetGiftId

    #region GetInventationGift

    public static InvitationGift GetInventationGift(int guestId, string gift)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.InvitationsGifts.SingleOrDefault(g => g.iggu == guestId && g.iggi == gift);
      }
    }

    #endregion GetInventationGift

    #region GetGiftsShortById

    /// <summary>
    ///Método para obtener una lista de Regalos por id.
    /// </summary>
    /// <param name="giIDList">Lista de id de Gits</param>
    /// <returns>List<GiftShort></returns>
    /// <history>
    /// [aalcocer] 23/03/2016 Created
    /// </history>
    public static List<GiftShort> GetGiftsShortById(IEnumerable<string> giIDList)
    {
      List<GiftShort> lstgetGifs;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        lstgetGifs = dbContext.Gifts.Where(x => giIDList.Contains(x.giID)).
          Select(x => new
          {
            x.giID,
            x.giN,
            x.gigc
          }).AsEnumerable().
          Select(x => new GiftShort
          {
            giID = x.giID,
            giN = x.giN,
            gigc = x.gigc
          }).ToList();
      }
      return lstgetGifs;
    }

    #endregion GetGiftsShortById
  }
}