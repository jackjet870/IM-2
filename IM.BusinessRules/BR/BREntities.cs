using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;

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
    public static int OperationEntity<T>(T entitySave, EnumMode enumMode) where T : class
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        switch (enumMode)
        {
          #region Delete
          case EnumMode.deleted:
            {
              dbContext.Entry(entitySave).State = EntityState.Deleted;
              break;
            } 
          #endregion
          #region Edit
          case EnumMode.edit:
            {
              dbContext.Entry(entitySave).State = EntityState.Modified;
              break;
            } 
          #endregion
          #region Add
          case EnumMode.add:
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
    }
    #endregion

    #region OperationEntities
    /// <summary>
    /// Agrega|Actualiza|Elimina registros de la BD
    /// dependiendo del tipo de lista que se le TengaEnviado
    /// </summary>
    /// <typeparam name="T">Tipo de lista</typeparam>
    /// <param name="lstEntities">Lista</param>
    /// <param name="state">Modified. Modifica | Added. Agrega | Deleted. Elimina</param>
    /// <returns>0. No se hizo nada | >0. Numero de registros afectados</returns>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    public static int OperationEntities<T>(List<T> lstEntities, EntityState state) where T : class
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        int nRes = 0;
        using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
          try
          {
            lstEntities.ForEach(item =>
            {
              dbContext.Entry(item).State = state;
            });
            nRes = dbContext.SaveChanges();
            transaction.Commit();
          }
          catch
          {
            nRes = 0;
            transaction.Rollback();
          }
        }
        return nRes;
      }
    } 
    #endregion

  }
}
