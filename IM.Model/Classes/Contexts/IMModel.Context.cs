using System.Data.Entity;

namespace IM.Model
{
  public partial class IMEntities : DbContext
  {
    #region Constructores y destructores

    /// <summary>
    /// Constructor que permite utilizar una cadena de conexion
    /// </summary>
    /// <param name="connectionString">Cadena de conexion</param>
    /// <history>
    /// [wtorres]  23/Mar/2016 Created
    /// </history>
    public IMEntities(string connectionString)
      : base(connectionString)
    {
      Configuration.ProxyCreationEnabled = false;
    }

    #endregion
  }
}