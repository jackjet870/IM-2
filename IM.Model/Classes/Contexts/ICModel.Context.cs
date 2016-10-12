﻿using System.Data.Entity;

namespace IM.Model
{
  public partial class ICEntities : DbContext
  {
    #region Constructores y destructores

    /// <summary>
    /// Constructor que permite utilizar una cadena de conexion
    /// </summary>
    /// <param name="connectionString">Cadena de conexion</param>
    /// <history>
    /// [erosado]  20/06/2016 Created
    /// </history>
    public ICEntities(string connectionString)
      : base(connectionString)
    {
      Configuration.ProxyCreationEnabled = false;
    }

    #endregion
  }
}