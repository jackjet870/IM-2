using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRMaritalStatus
  {
    #region GetMaritalStratus
    /// <summary>
    /// Obtiene la lista de estados civiles
    /// </summary>
    /// <param name="status">-1 Todos los registros | 0. Registros inactivos | 1. Registros activos</param>
    /// <returns></returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// [emoguel] modified 01/04/2016--->Se agregaron filtros opcionales
    /// </history>
    public static List<MaritalStatus> GetMaritalStatus(int status= -1,MaritalStatus maritaStatus=null)
    {
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from ms in dbContext.MaritalStatusList
                    select ms;

        if (status != -1)//Filtro por Estatus
        {
          bool blnStatus = Convert.ToBoolean(status);
          query = query.Where(ms => ms.msA == blnStatus);
        }

        #region Filtros adicionales
        if(maritaStatus!=null)//Se verifica que se tenga un objeto
        {
          if(!string.IsNullOrWhiteSpace(maritaStatus.msID))//filtro por ID
          {
            query = query.Where(ms => ms.msID == maritaStatus.msID);
          }

          if(!string.IsNullOrWhiteSpace(maritaStatus.msN))//Filtro por Descripcion
          {
            query = query.Where(ms => ms.msN.Contains(maritaStatus.msN));
          }
        }
        #endregion
        return query.OrderBy(ms => ms.msN).ToList();
      }
    }
    #endregion

    #region SaveMaritalStatus
    /// <summary>
    /// Agregra|Actualiza un registro del catalogo MStatus
    /// </summary>
    /// <param name="maritalStatus">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0. No se guardó | 1. Se guardó correctamente | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    public static int SaveMaritalStatus(MaritalStatus maritalStatus,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Actualizar
        if (blnUpdate)//Actualizar
        {
          dbContext.Entry(maritalStatus).State = System.Data.Entity.EntityState.Modified;
        } 
        #endregion
        #region Insertar
        else//Insertar
        {
          MaritalStatus marStatusVal = dbContext.MaritalStatusList.Where(ms => ms.msID == maritalStatus.msID).FirstOrDefault();

          if (marStatusVal != null)//Validamos que no exista registro con el mismoID
          {
            return -1;
          }
          else
          {
            dbContext.MaritalStatusList.Add(maritalStatus);
          }
        } 
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
