using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

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

    #region Funciones

    #region UFN_OR_GetAccountingCode
    /// <summary>
    /// Funcion para recalcular los codigos de cecos y cebes
    /// </summary>
    /// <param name="guestId">ID del huesped</param>
    /// <param name="activity"> Actividad </param>
    /// <returns>String</returns>
    /// <history>
    /// [edgrodriguez]  14/Oct/2016 Created
    /// </history>
    [DbFunction("IMModel.Store", "UFN_OR_GetAccountingCode")]
    public string UFN_OR_GetAccountingCode(int guestId, string activity)
    {
      var objectContext = ((IObjectContextAdapter)this).ObjectContext;

      var parameters = new List<ObjectParameter>();
      parameters.Add(new ObjectParameter("GuestID", guestId));
      parameters.Add(new ObjectParameter("Activity", activity));

      return objectContext.CreateQuery<string>("IMModel.Store.UFN_OR_GetAccountingCode(@GuestID, @Activity)", parameters.ToArray())
           .Execute(MergeOption.NoTracking)
           .FirstOrDefault();
    }
    #endregion

    #region UFN_OR_GetMissingAccountInfo
    /// <summary>
    /// Funcion para obtener la razon de falta de codigo contable
    /// </summary>
    /// <param name="guestId">ID del huesped</param>
    /// <param name="activity"> Actividad </param>
    /// <returns>String</returns>
    /// <history>
    /// [edgrodriguez]  14/Oct/2016 Created
    /// </history>
    [DbFunction("IMModel.Store", "UFN_OR_GetMissingAccountInfo")]
    public string UFN_OR_GetMissingAccountInfo(int guestId, string activity)
    {
      var objectContext = ((IObjectContextAdapter)this).ObjectContext;

      var parameters = new List<ObjectParameter>();
      parameters.Add(new ObjectParameter("GuestID", guestId));
      parameters.Add(new ObjectParameter("Activity", activity));

      return objectContext.CreateQuery<string>("IMModel.Store.UFN_OR_GetMissingAccountInfo(@GuestID, @Activity)", parameters.ToArray())
           .Execute(MergeOption.NoTracking)
           .FirstOrDefault();
    }
    #endregion 

    #endregion
  }
}