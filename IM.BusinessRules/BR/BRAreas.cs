using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRAreas
  {
    #region GetAreas
    /// <summary>
    /// Obtiene el catalogo de Areas
    /// </summary>
    /// <param name="nStatus">-1. Sin filtro| 0. Inactivos| 1. Activos</param>
    /// <param name="area">filtra dependiendo de los valores que contenga</param>
    /// <returns>Lista de Areas</returns>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    public static List<Area> GetAreas(Area area,int nStatus=-1)
    {      
      List<Area> lstAreas = new List<Area>();
      using (var dbContext = new IMEntities())
      {
        var query = from areas in dbContext.Areas
                    select areas ;

        if (nStatus != -1)//filtro por estatus
        {
          bool bStat = Convert.ToBoolean(nStatus);
          query = query.Where(a => a.arA == bStat); 
        }

        if (!string.IsNullOrWhiteSpace(area.arID))//Filtra por ID
        {
          query = query.Where(a=>a.arID==area.arID);
        }
        if (!string.IsNullOrWhiteSpace(area.arN))//Filtra por nombre
        {
          query = query.Where(a=>a.arN!=area.arN);
        }
                    
        lstAreas = query.ToList();//pasar el resultado a la lista
          
      }
        
      return lstAreas;

    }
    #endregion
    #region SaveArea
    /// <summary>
    /// Actualiza|Agrega un registro al catalogo de Areas
    /// </summary>
    /// <param name="bUpd">true para Actualizar| false para agregar registro</param>
    /// <param name="Area">Entidad para guardar</param>
    /// <returns>0.No se guardó el registro | 1. El registro se guardo correctamente | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    public static int SaveArea(bool bUpd,Area area)
    {
      
      int nRes = 0;       
          using (var dbContext = new IMEntities())
          {
            if (!bUpd)//insertar
            {
              Area areaValue = dbContext.Areas.Where(c => c.arID == area.arID).FirstOrDefault();
              if (areaValue != null)//Verificar que no exista un registro con el mismo ID
              {
                nRes = 2;
              }
              else
              {
                dbContext.Areas.Add(area);
                nRes = dbContext.SaveChanges();
              }
            }
            else//Actualizar
            {
              dbContext.Entry(area).State = System.Data.Entity.EntityState.Modified;
              nRes = dbContext.SaveChanges();
            }

          }
      return nRes;     

    }
    #endregion
  }
}
