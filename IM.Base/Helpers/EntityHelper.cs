﻿using IM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model.Helpers;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;

namespace IM.Base.Helpers
{
  public class EntityHelper
  {
    #region GetEntityTypeBase
    /// <summary>
    /// Obtiene las propiedades del Model Entity dependiendo de la entidad
    /// </summary>
    /// <param name="type">Typo de entidad</param>
    /// <returns>EntityTypeBase</returns>
    /// <history>
    /// [emoguel] created 15/04/2016
    /// </history>
    public static EntityTypeBase GetEntityTypeBase(Type type)
    {
      using (var dbContex = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var objectContext = ((IObjectContextAdapter)dbContex).ObjectContext;//obtenemos el contexto

        var elementType = objectContext
            .MetadataWorkspace
            .GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace)
            .BaseEntitySets
            .First(meta => meta.ElementType.Name == type.Name)
            .ElementType;//Obtenemos el objeto
        return elementType;
      }
    } 
    #endregion
  }
}
