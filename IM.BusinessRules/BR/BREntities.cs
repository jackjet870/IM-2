using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BREntities
  {
    #region OperationEntity
    /// <summary>
    /// Generic save|update
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="entitySave">objeto a guardar</param>
    /// <returns>0. No se guardó | 1. Se guardó | -1 Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    public async static Task<int> OperationEntity<T>(T entitySave, EnumMode enumMode) where T : class
    {
      int nRes = 0;
     nRes= await Task.Run(() => 
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          switch (enumMode)
          {
            #region Delete
            case EnumMode.Delete:
              {
                dbContext.Entry(entitySave).State = EntityState.Deleted;
                break;
              }
            #endregion
            #region Edit
            case EnumMode.Edit:
              {
                dbContext.Entry(entitySave).State = EntityState.Modified;
                break;
              }
            #endregion
            #region Add
            case EnumMode.Add:
              {
                var objContext = ((IObjectContextAdapter)dbContext).ObjectContext;
                var objSet = objContext.CreateObjectSet<T>();
                #region Buscar si tiene Llave identity
                bool blIdentity = false;
                foreach (EdmProperty edm in objSet.EntitySet.ElementType.KeyMembers)
                {
                  var a = edm.MetadataProperties.Where(item => item.Name == "http://schemas.microsoft.com/ado/2009/02/edm/annotation:StoreGeneratedPattern").FirstOrDefault();
                  if (a != null)
                  {
                    if (a.Value.ToString() == "Identity")
                    {
                      blIdentity = true;
                      break;
                    }
                  }
                }
                #endregion

                if (blIdentity == true)//Si es campo autoincremental
                {
                  dbContext.Entry(entitySave).State = EntityState.Added;
                }
                else
                {
                  var keyNames = objSet.EntitySet.ElementType.KeyMembers.Select(edmMember => edmMember.Name);
                  var keyValues = keyNames.Select(name => entitySave.GetType().GetProperty(name).GetValue(entitySave, null)).ToArray();
                  var exists = dbContext.Set<T>().Find(keyValues);
                  if (exists != null)//Validamos si existe un registro con el mismo ID
                  {
                    return -1;
                  }
                  else//Agrega
                  {
                    dbContext.Entry(entitySave).State = EntityState.Added;
                  }
                }
                break;
              }
              #endregion
          }
          return dbContext.SaveChanges();
        }
      });

      return nRes;
    }
    #endregion

    #region OperationEntities
    /// <summary>
    /// Agrega|Actualiza|Elimina registros de la BD
    /// dependiendo del tipo de lista que se le TengaEnviado
    /// El enum agregar unicamente sirve para entidades con id autoincrementales
    /// </summary>
    /// <typeparam name="T">Tipo de lista</typeparam>
    /// <param name="lstEntities">Lista</param>
    /// <param name="state">Modified. Modifica | Added. Agrega | Deleted. Elimina</param>
    /// <returns>0. No se hizo nada | >0. Numero de registros afectados</returns>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// [jorcanche] se agrego asincronia
    /// [edgrodriguez]  28/07/2016 Modified. Ahora se realiza un throw a la excepcion. Para utilizar el metodo de ShowMessage(Exception ex).
    /// </history>
    public async static Task<int> OperationEntities<T>(List<T> lstEntities, EnumMode enumMode) where T : class
    {
      int nRes = 0;
      try
      {
        await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
              #region Transacción
              try
              {
                lstEntities.ForEach(item =>
                {
                  dbContext.Entry(item).State = (enumMode == EnumMode.Add) ? EntityState.Added : (enumMode == EnumMode.Edit) ? EntityState.Modified : EntityState.Deleted;
                });
                nRes = dbContext.SaveChanges();
                transaction.Commit();
              }
              catch (Exception)
              {
                /// nRes = 0;
                transaction.Rollback();
                throw;
              }
              #endregion
            }
          }
        });
        return nRes;
      }
      catch (Exception)
      {
        throw;
      }
    } 
    #endregion

  }
}
