using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRHotels
  {
    #region GetHotels
    /// <summary>
    /// Obtiene la lista de hoteles
    /// </summary>
    /// <param name="status">-1. Todos los registros | 0. Registros Activos | 1. Registros inactivos</param>
    /// <param name="hotel">Objeto con filtros adicionales</param>
    /// <param name="blnInclude">llena el objeto Area y Hotelgroup</param>
    /// <returns>Lista Hotels</returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// [emoguel] modified 29/03/2016 se agregaron parametros de busqueda
    /// </history>
    public static List<Hotel> GetHotels(Hotel hotel = null, int nStatus = -1,bool blnInclude = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        
        var query = (from ho in dbContext.Hotels
                     select ho );
        
        if (blnInclude)//Incluimos el area y el grupo
        {
          query = query.Include("Area").Include("HotelGroup");
        }
        if (nStatus != -1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(ho => ho.hoA == blnEstatus);
        }

        if (hotel != null)//Verificamos que se tenga un objeto
        {
          if (!string.IsNullOrWhiteSpace(hotel.hoID))//filtro por ID
          {
            query = query.Where(ho => ho.hoID == hotel.hoID);
          }

          if (!string.IsNullOrWhiteSpace(hotel.hoGroup))//Filtro por Grupo
          {
            query = query.Where(ho => ho.hoGroup == hotel.hoGroup);
          }

          if (!string.IsNullOrWhiteSpace(hotel.hoar))//Filtro por Area
          {
            query = query.Where(ho => ho.hoar == hotel.hoar);
          }

        }

        return query.OrderBy(ho => ho.hoID).ToList();
        
      }
    }
    #endregion

    #region SaveHotel
    /// <summary>
    /// Actualiza|inserta un registro en el catalogo Hotels
    /// </summary>
    /// <param name="hotel">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0. No se guardó | 1.Se guardó correctamente | 2.Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    public static int SaveHotel(Hotel hotel, bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        
        #region Update
        if (blnUpdate)//Si es actualizar
        {
          hotel.Area = null;
          hotel.HotelGroup = null;
          dbContext.Entry(hotel).State= EntityState.Modified;          
        }
        #endregion
        #region Insert
        else//Si es insertar
        {
          Hotel hotelValid = dbContext.Hotels.Where(ho => ho.hoID == hotel.hoID).FirstOrDefault();

          if (hotelValid != null)//Si existe un registro con el mismo ID
          {
            return 2;
          }
          else//Si no existe un registro con el mismo ID
          {
            dbContext.Hotels.Add(hotel);
          }
        }
        #endregion
        
        int nRes= dbContext.SaveChanges();
        //devolvemos el objeto con sus relaciones
        hotel = dbContext.Hotels.Where(ho => ho.hoID == hotel.hoID).Include("Area").Include("HotelGroup").FirstOrDefault();

        return nRes;
      }
    }
      #endregion

    }
  }
