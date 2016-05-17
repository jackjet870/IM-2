using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRHotelGroups
  {

    #region GetHotelGroups
    /// <summary>
    /// Obtiene registros del catalogo HotelGroups
    /// </summary>
    /// <param name="hotelGroup">Objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1. Registros Activos</param>
    /// <returns>Lista de tipo HotelGroup</returns>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    public static List<HotelGroup> GetHotelGroups(HotelGroup hotelGroup = null, int nStatus = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from hg in dbContext.HotelsGroups
                    select hg;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(hg => hg.hgA == blnEstatus);
        }

        if (hotelGroup != null)//Verificamos si tenemos un objeto
        {
          if (!string.IsNullOrWhiteSpace(hotelGroup.hgID))//Filtro por ID
          {
            query = query.Where(hg => hg.hgID == hotelGroup.hgID);
          }

          if (!string.IsNullOrWhiteSpace(hotelGroup.hgN))//Filtro por descripcion
          {
            query = query.Where(hg => hg.hgN.Contains(hotelGroup.hgN));
          }
        }

        return query.OrderBy(hg => hg.hgN).ToList();
      }
    }
    #endregion

    #region SaveHotelGroup
    /// <summary>
    /// Agrega|Actualiza registros en el catalogo HotelGroup
    /// Asigan|Desasigna Hotels de HotelGroups
    /// </summary>
    /// <param name="hotelGrooup">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <param name="lstAdd">Lista a asignar al Hotel Group</param>
    /// <param name="lstDel">Lista a Eliminar del grupo</param>
    /// <returns>-1. Existe un registro con el mismo ID | 0. No se pudo guardar | 1. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 13/05/2016
    /// </history>
    public static int SaveHotelGroup(HotelGroup hotelGrooup, bool blnUpdate, List<Hotel> lstAdd, List<Hotel> lstDel)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
          try
          {
            #region Update
            if (blnUpdate)
            {
              dbContext.Entry(hotelGrooup).State = EntityState.Modified;
            }
            #endregion

            #region Add
            else
            {
              if (dbContext.HotelsGroups.Where(hoo => hoo.hgID == hotelGrooup.hgID).FirstOrDefault() != null)
              {
                return -1;
              }
              else
              {
                dbContext.HotelsGroups.Add(hotelGrooup);
              }
            }
            #endregion

            #region List Add
            dbContext.Hotels.AsEnumerable().Where(ho=>lstAdd.Any(hoo=>hoo.hoID==ho.hoID)).ToList().ForEach(ho =>
            {
              ho.hoGroup = hotelGrooup.hgID;
              dbContext.Entry(ho).State = EntityState.Modified;
            });
            #endregion

            #region ListDel
            dbContext.Hotels.AsEnumerable().Where(ho => lstDel.Any(hoo => hoo.hoID == ho.hoID)).ToList().ForEach(ho =>
            {
              ho.hoGroup = null;
              dbContext.Entry(ho).State = EntityState.Modified;
            });
            #endregion

            int nRes = dbContext.SaveChanges();
            transacction.Commit();
            return nRes;
          }
          catch(Exception e)
          {
            transacction.Rollback();
            return 0;
          }
        }
      }
    }
    #endregion
  }
}
