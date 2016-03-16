﻿using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRDesks
  {
    #region GetDesks
    /// <summary>
    /// Obtiene registros del catalogo desk
    /// </summary>
    /// <param name="desk">objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1. registros activos</param>
    /// <returns>Lista de tipo desk</returns>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    public static List<Desk> GetDesks(Desk desk,int nStatus=-1)
    {
      using (var dbContext = new IMEntities())
      {
        var query = from dk in dbContext.Desks
                    select dk;

        if(nStatus!=-1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(dk=>dk.dkA==desk.dkA);
        }

        if(desk.dkID>0)//Filtro por ID
        {
          query = query.Where(dk=>dk.dkID==desk.dkID);
        }

        if(!string.IsNullOrWhiteSpace(desk.dkN))//Filtro por Nombre Descripcion
        {
          query = query.Where(dk=>dk.dkN==desk.dkN);
        }

        return query.OrderBy(dk => dk.dkN).ToList();
      }
    }
    #endregion

    #region saveDesks
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo Desk
    /// </summary>
    /// <param name="desk">Objeto a guardar en la BD</param>
    /// <param name="blnUpdate">True. Actualiza un registro | False. Agrega un registro</param>
    /// <returns>0. No se puedo guardar | 1. Se guardó correctamente | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    public static int SaveDesk(Desk desk,bool blnUpdate)
    {
      using (var dbContext = new IMEntities())
      {
        if(blnUpdate)//Si es actualizar
        {
          dbContext.Entry(desk).State = System.Data.Entity.EntityState.Modified;
        }
        else//Si es guardar
        {
          Desk deskVal = dbContext.Desks.Where(dk => dk.dkID == desk.dkID).FirstOrDefault();
          if(deskVal==null)//No existe registro con el ID
          {
            dbContext.Desks.Add(desk);
          }
          else//Existe un registro con el mismo ID
          {
            return 2;
          }
        }
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
