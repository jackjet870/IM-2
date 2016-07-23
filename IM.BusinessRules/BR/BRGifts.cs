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
    public async static Task<List<GiftShort>> GetGiftsShort(string location = "ALL", int Status = 0)
    {
      List<GiftShort> result = new List<GiftShort>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          result = dbContext.USP_OR_GetGifts(location, Convert.ToByte(Status)).ToList();
        }
      });
      return result;
    }

    #endregion GetGifts

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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.Gifts.Where(g => g.giID == giftId).SingleOrDefault();
      }
    }

    #endregion GetGiftId

    #region GetInventationGift

    public static InvitationGift GetInventationGift(int guestId, string gift)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
    /// <returns><list type="GiftShort"></list></returns>
    /// <history>
    /// [aalcocer] 23/03/2016 Created
    /// [aalcocer] 25/05/2016  Modified. Se agregó asincronía
    /// </history>
    public static async Task<List<GiftShort>> GetGiftsShortById(IEnumerable<string> giIDList)
    {
      var result = new List<GiftShort>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          result = dbContext.Gifts.Where(x => giIDList.Contains(x.giID)).
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
      });

      return result;
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
    /*public static List<Gift> GetGiftsInputList(List<GiftsReceiptDetailShort> _listGiftsID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.Gifts.Where(x => x.giA == true).Intersect(_listGiftsID.Select(s => s.gegi)) // && _listGiftsID.Select(s => s.gegi).Contains(x.giID)).ToList();
      }
    } */

    #endregion GetGiftsInputList

    #region GetGifts
    /// <summary>
    /// Obtiene registros del catalogo Gift
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="gift">Objeto con filtros adicionales</param>
    /// <returns>Lista tipo Gift</returns>
    /// <history>
    /// [emoguel] created 23/05/2016
    /// [emoguel] modified 30/06/2016 ----> Se agregaron más filtros
    /// </history>
    public async static Task<List<Gift>> GetGifts(int nStatus = -1, Gift gift = null, int giftPack=-1)
    {
      List<Gift> lstGift = new List<Gift>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from gi in dbContext.Gifts
                      select gi;
          
          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(gi => gi.giA == blnStatus);
          }

          if (gift != null)
          {
            if (!string.IsNullOrWhiteSpace(gift.giID))//Filtro por ID
            {
              query = query.Where(gi => gi.giID == gift.giID);
            }

            if (!string.IsNullOrWhiteSpace(gift.giN))//Filtro por descripción
            {
              query = query.Where(gi => gi.giN.Contains(gift.giN));
            }
            
            if(!string.IsNullOrWhiteSpace(gift.giShortN))//Filtro por Descripción corta
            {
              query = query.Where(gi => gi.giShortN == gift.giShortN);
            }

            if(!string.IsNullOrWhiteSpace(gift.gigc))//Filtro por categoria
            {             
              query = query.Where(gi => gi.gigc == gift.gigc);
            }

            if(giftPack!=-1)//Si es paquete
            {
              bool blnPack = Convert.ToBoolean(giftPack);
              query = query.Where(gi => gi.giPack == blnPack);
            }

            #region giftCard
            if (!string.IsNullOrEmpty(gift.giProductGiftsCard) && gift.giProductGiftsCard!="ALL")//Filtro por giftCard
            {
              switch (gift.giProductGiftsCard)
              {
                case "ANY":
                  {
                    query = query.Where(gi => gi.giProductGiftsCard != null);
                    break;
                  }
                case "NONE":
                  {
                    query = query.Where(gi => gi.giProductGiftsCard == null);
                    break;
                  }
                default:
                  {
                    query = query.Where(gi => gi.giProductGiftsCard == gift.giProductGiftsCard);
                    break;
                  }
              }              
            }
            #endregion

            #region Promotion
            if (!string.IsNullOrWhiteSpace(gift.giPVPPromotion) && gift.giPVPPromotion!="ALL")//Filtro por Promotion
            {
              switch (gift.giPVPPromotion)
              {
                case "ANY":
                  {
                    query = query.Where(gi => gi.giPVPPromotion != null);
                    break;
                  }
                case "NONE":
                  {
                    query = query.Where(gi => gi.giPVPPromotion == null);
                    break;
                  }
                default:
                  {
                    query = query.Where(gi => gi.giPVPPromotion == gift.giPVPPromotion);
                    break;
                  }
              }
            }
            #endregion

            #region Transacction
            if (!string.IsNullOrWhiteSpace(gift.giOperaTransactionType) && gift.giOperaTransactionType!="ALL")//Filtro por opera transacction
            {
              switch (gift.giOperaTransactionType)
              {
                case "ANY":
                  {
                    query = query.Where(gi => gi.giOperaTransactionType != null);
                    break;
                  }
                case "NONE":
                  {
                    query = query.Where(gi => gi.giOperaTransactionType == null);
                    break;
                  }
                default:
                  {
                    query = query.Where(gi => gi.giOperaTransactionType == gift.giOperaTransactionType);
                    break;
                  }
              }
            }
            #endregion

            #region Promotion Opera
            if (!string.IsNullOrWhiteSpace(gift.giPromotionOpera) && gift.giPromotionOpera!="ALL")//Filtro por promotion Opera
            {
              switch (gift.giPromotionOpera)
              {
                case "ANY":
                  {
                    query = query.Where(gi => gi.giPromotionOpera != null);
                    break;
                  }
                case "NONE":
                  {
                    query = query.Where(gi => gi.giPromotionOpera == null);
                    break;
                  }
                default:
                  {
                    query = query.Where(gi => gi.giPromotionOpera == gift.giPromotionOpera);
                    break;
                  }
              }
            } 
            #endregion
          }          
          lstGift= query.OrderBy(gi => gi.giN).ToList();
        }
      });
      return lstGift;

    }
    #endregion

    #region GetGiftsLog
    /// <summary>
    /// Devuelve el log de un Gift
    /// </summary>
    /// <param name="idGift"></param>
    /// <returns></returns>
    /// <history>
    /// [emoguel] created 05/07/2016
    /// </history>
    public static async Task<List<GiftLogData>> GetGiftsLog(string idGift)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {          
          return dbContext.USP_OR_GetGiftLog(idGift).ToList();
        }
      });
    } 
    #endregion

    public static IEnumerable<object> GetGiftsWithPackages()
    {
      IEnumerable<object> lstResult;

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        var query = from a in dbContext.Gifts
                    where a.giA == true
                    select new
                    {
                                giID = a.giID,
                                giN = a.giN,
                                Visibility = a.giPack ? true : false,
                                packs = dbContext.GiftsPackagesItems.Where(w => w.gpPack == a.giID).Select(s => dbContext.Gifts.Where(w => w.giID == s.gpgi).Select(ss => ss.giN).FirstOrDefault()).ToList()
                                };

        lstResult = query.ToList();
      }

      return lstResult;
    }

    #region SaveGift
    /// <summary>
    /// Guarda un Gift y Su Log
    /// </summary>
    /// <param name="gift">Objeto a guardar</param>
    /// <param name="blnUpdate">Truw. Actualiza | False. Inserta</param>
    /// <param name="lstAddLocations">Locaciones a agregar</param>
    /// <param name="lstDelLocations">Locaciones a eliminar</param>
    /// <param name="lstAddAgencies">Agencias a agregar</param>
    /// <param name="lstDelAgencies">Agencias a eliminar</param>
    /// <param name="lstAddGiftPack">Gifts a agregar al paquete</param>
    /// <param name="lstDelGiftPack">Gifts a elimina del paquete</param>
    /// <param name="lstUpdGiftPack">Gift a actualizar en el paquete</param>
    /// <param name="blnIsInventory">True. Guardamos el regalo en todos los almacenes existentes</param>
    /// <param name="userId">usuario que está guardando el gift</param>
    /// <history>
    /// [emoguel] created 20/07/2016
    /// </history>
    /// <returns>-1. Existe un Gift con el mismo nombre| 0. No se guardó. | >0. Se guardó correctamente</returns>    
    public async static Task<int> SaveGift(Gift gift, bool blnUpdate, List<Location> lstAddLocations, List<Location> lstDelLocations, List<Agency> lstAddAgencies, List<Agency> lstDelAgencies,
                                            List<GiftPackageItem> lstAddGiftPack, List<GiftPackageItem> lstDelGiftPack, List<GiftPackageItem> lstUpdGiftPack, bool blnIsInventory, string userId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transacction = dbContext.Database.BeginTransaction())
          {
            try
            {
              int nSave = 0;
              Gift giftSave = new Gift();
              #region Update
              if (blnUpdate)//Actualizar
              {
                giftSave = dbContext.Gifts.Where(gi => gi.giID == gift.giID).
                                            Include(gi => gi.Locations).
                                            Include(gi => gi.Agencies).FirstOrDefault();

                ObjectHelper.CopyProperties(giftSave, gift);
              }
              #endregion
              #region Add
              else//Add
              {
                if (dbContext.Gifts.Where(gi => gi.giID == gift.giID).FirstOrDefault() != null)
                {
                  return -1;
                }
                else
                {
                  int nOrder = dbContext.Gifts.Max(gi => gi.giO)+1;
                  gift.giO = nOrder;
                  dbContext.Gifts.Add(gift);
                  giftSave = gift;
                  nSave += dbContext.SaveChanges(); 
                }
              }
              #endregion

              #region Locations
              //Eliminar Locations
              giftSave.Locations.Where(lo => lstDelLocations.Any(loo => lo.loID == loo.loID)).ToList().ForEach(lo =>
              {
                giftSave.Locations.Remove(lo);
              });


              //Agregar Locations
              var addLocations = dbContext.Locations.AsEnumerable().Where(lo => lstAddLocations.Any(loo => loo.loID == lo.loID)).ToList();
              addLocations.ForEach(lo =>
              {
                giftSave.Locations.Add(lo);
              });
              #endregion

              #region Agencies
              //Eliminar
              giftSave.Agencies.Where(ag => lstDelAgencies.Any(agg => ag.agID == agg.agID)).ToList().ForEach(ag =>
              {
                giftSave.Agencies.Remove(ag);
              });

              //Agregar
              var addAgencies = dbContext.Agencies.AsEnumerable().Where(ag => lstAddAgencies.Any(agg => agg.agID == ag.agID)).ToList();

              addAgencies.ForEach(ag =>
              {
                giftSave.Agencies.Add(ag);
              });
              #endregion

              #region GiftPack
              if (gift.giPack)
              {
                //Eliminar en caso de que ya no se vaya a utilizar
                if (lstDelGiftPack.Count > 0)
                {
                  lstDelGiftPack.ForEach(gp =>
                  {
                    gp.GiftItem = null;
                    dbContext.Entry(gp).State = EntityState.Deleted;
                  });
                }

                //Actualizar en caso de que se haya modificado uno ya existeste
                if (lstUpdGiftPack.Count > 0)
                {
                  lstUpdGiftPack.ForEach(gp =>
                  {
                    gp.GiftItem = null;
                    dbContext.Entry(gp).State = EntityState.Modified;
                  });
                }

                //Agregar Gift Pack Item
                if (lstAddGiftPack.Count > 0)
                {
                  lstAddGiftPack.ForEach(gp =>
                  {
                    gp.GiftItem = null;
                    gp.GiftPackage = null;
                    gp.gpPack = gift.giID;
                    dbContext.Entry(gp).State = EntityState.Added;
                  });
                }
              }
              else if (blnUpdate)
              {
                //Eliminar items en caso de que ya no se paquete
                var lstDelPack = dbContext.GiftsPackagesItems.Where(gp => gp.gpPack == gift.giID).ToList();
                lstDelPack.ForEach(gp =>
                {
                  gp.GiftItem = null;
                  dbContext.Entry(gp).State = EntityState.Deleted;
                });

              }
              #endregion

              #region Inventory
              if (blnIsInventory)
              {
                var date = BRHelpers.GetServerDate();
                var lstWH = dbContext.Warehouses.Select(wh => wh.whID).ToList();
                lstWH.ForEach(whID =>
                {
                  GiftInventory giftInventory = new GiftInventory { gvgi = gift.giID, gvwh = whID, gvQty = 0, gvD = new DateTime(date.Year, date.Month, date.Day) };
                  dbContext.Entry(giftInventory).State = EntityState.Added;
                });
              }
              #endregion

              nSave += dbContext.SaveChanges();
              #region GiftLog
              dbContext.USP_OR_SaveGiftLog(gift.giID, 0, userId);
              #endregion

              nSave += dbContext.SaveChanges();
              transacction.Commit();
              return nSave;
            }
            catch(Exception ex)
            {

              throw;
            }
          }
        }
      });
    } 
    #endregion

  }
}