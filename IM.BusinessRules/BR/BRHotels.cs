using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

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
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async static Task<List<Hotel>> GetHotels(Hotel hotel = null, int nStatus = -1,bool blnInclude = false)
    {
      //List<Hotel> result = null;
     //return await Task.Run(() =>
     // {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
       return await Task.Run(() =>
        {
          var query = (from ho in dbContext.Hotels
                       select ho);
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
        });
        }

      //}) ;
      //return result;
    }
    #endregion

    }
  }
