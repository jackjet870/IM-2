using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRAreas
  {
    #region GetAreas
    /// <summary>
    /// Obtiene el catalogo de Areas
    /// </summary>
    /// <param name="nStatus">-1. Sin filtro| 0. Inactivos| 1. Activos</param>
    /// <param name="area">filtra dependiendo de los valores que contenga, puede ser null</param>
    /// <returns>Lista de Areas</returns>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// </history>
    public static List<Area> GetAreas(Area area=null,int nStatus=-1)
    { 
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from areas in dbContext.Areas                    
                    select areas ;

        if (nStatus != -1)//filtro por estatus
        {
          bool bStat = Convert.ToBoolean(nStatus);
          query = query.Where(a => a.arA == bStat); 
        }

        if (area != null)//Validacion del objeto
        {
          if (!string.IsNullOrWhiteSpace(area.arID))//Filtra por ID
          {
            query = query.Where(a => a.arID == area.arID);
          }
          if (!string.IsNullOrWhiteSpace(area.arN))//Filtra por nombre
          {
            query = query.Where(a => a.arN.Contains(area.arN));
          }
        }          
       return query.OrderBy(a=>a.arN).ToList();//pasar el resultado a la lista
          
      }

    }
    #endregion   
  }
}
