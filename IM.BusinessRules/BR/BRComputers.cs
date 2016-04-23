using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRComputers
  {
    #region GetComputers
    /// <summary>
    /// Obtiene registros del catalogo Computer
    /// </summary>
    /// <param name="computer">objeto con filtros adicionales</param>
    /// <returns>Devuelve una lista de tipo computer</returns>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// </history>
    public static List<Computer> GetComputers(Computer computer=null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from cp in dbContext.Computers
                    select cp;

        if (computer != null)//Si se tiene objeto
        {
          if (!string.IsNullOrWhiteSpace(computer.cpID))//Filtro por ID
          {
            query = query.Where(cp => cp.cpID == computer.cpID);
          }

          if (!string.IsNullOrWhiteSpace(computer.cpN))//Filtro por descripcion (Nombre)
          {
            query = query.Where(cp => cp.cpN.Contains(computer.cpN));
          }

          if(computer.cpdk!=null)
          {
            query = query.Where(cp => cp.cpdk == computer.cpdk);
          }
        }

        return query.OrderBy(cp => cp.cpN).ToList();
      }
    }
    #endregion
    
    #region SaveComputer
    /// <summary>
    /// Agrega|Actualiza un registro al catalogo Computers
    /// </summary>
    /// <param name="computer">Objeto a guardar</param>
    /// <returns>0. No se guardo | 1. Guardado correctamente | -1. Existe un registro con el mismo ID</returns>
    public static int SaveComputer(Computer computer,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        if (blnUpdate)//Si es actualizar
        {
          dbContext.Entry(computer).State = System.Data.Entity.EntityState.Modified;
        }
        else//Si es agregar
        {
          Computer computerVal = dbContext.Computers.Where(cp => cp.cpID == computer.cpID).FirstOrDefault();
          if (computerVal == null)
          {//Guardar
            dbContext.Computers.Add(computer);
          }
          else//Existe un registro con el mismo ID
          {
            return -1;
          }
        }
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
