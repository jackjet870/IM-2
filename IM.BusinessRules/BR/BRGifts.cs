using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
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
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async static Task<List<GiftShort>> GetGifts(string location = "ALL", int Status = 0)
    {
      List<GiftShort> result = new List<GiftShort>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_OR_GetGifts(location, Convert.ToByte(Status)).ToList();
        }
      });
      return result;
    }


    #endregion GetGifts

    #region GetGifts
    /// <summary>
    /// Obtiene todos los gifts activos
    /// </summary>
    /// <param name="status"></param>
    /// <returns> Lista de entidades GIFT</returns>
    /// <history>
    /// [vipacheco] 22/Abril/2016 Created
    /// </history>
    public static List<Gift> GetGifts(int status)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        bool _status = Convert.ToBoolean(status);
        return dbContext.Gifts.Where(x => x.giA == _status).ToList();
      }
    }
    #endregion

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
    public async static Task<List<GiftCategory>> GetGiftsCategories(int Status = 0)
    {
      List<GiftCategory> result = new List<GiftCategory>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var lstGiftsCateg = dbContext.GiftsCategories;
          switch (Status)
          {
            case 1:
              result = lstGiftsCateg.Where(c => c.gcA == true).ToList();
              break;
            case 2:
              result = lstGiftsCateg.Where(c => c.gcA == false).ToList();
              break;
            default:
              result = lstGiftsCateg.ToList();
              break;
          }
        }
      });
      return result;
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

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.Gifts.Where(x => giIDList.Contains(x.giID)).
          Select(x => new
          {
            x.giID,
            x.giN,
            x.gigc
          }).
          Select(x => new GiftShort
          {
            giID = x.giID,
            giN = x.giN,
            gigc = x.gigc
          }).ToList();
      }

    }

    #endregion GetGiftsShortById

    #region GetGiftsInputList
    /// <summary>
    /// Obtiene Gifts de una lista de gifts ingresada
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    public static List<Gift> GetGiftsInputList(List<GiftsReceiptDetailShort> _listGiftsID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.Gifts.Where(x => x.giA == true && _listGiftsID.Select(s => s.gegi).Contains(x.giID)).ToList();
      }
    }
    #endregion
  }
}