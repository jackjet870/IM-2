using System;
using System.Linq;
using System.Collections.Generic;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRPersonnel
  {
    #region Login

    /// <summary>
    /// Método para obtener los datos del usuario
    /// como son datos principales, permisos, roles
    /// e informacion específica segun el tipo de login.
    /// </summary>
    /// <param name="logintype"> Tipo de Login 0. Normal 
    /// 1. Sala de Ventas 2. Locación 3. Almacen</param>
    /// <param name="user"> Nombre de Usuario</param>
    /// <param name="place"> Nombre del lugar</param>
    /// <returns></returns>
    public static UserData Login(EnumLoginType logintype, string user = "", string place = "")
    {
      UserData userData = new UserData();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var resUser = dbContext.USP_OR_Login(Convert.ToByte(logintype), user, place);
        userData.User = resUser.FirstOrDefault();
        var resRoles = resUser.GetNextResult<RoleLogin>();
        userData.Roles = resRoles.ToList();
        var resPermissions = resRoles.GetNextResult<PermissionLogin>();
        userData.Permissions = resPermissions.ToList();
        var resSalesroom = resPermissions.GetNextResult<SalesRoomLogin>();
        userData.SalesRoom = resSalesroom.FirstOrDefault();
        var resLocation = resSalesroom.GetNextResult<LocationLogin>();
        userData.Location = resLocation.FirstOrDefault();
        var resLeadSource = resLocation.GetNextResult<LeadSourceLogin>();
        userData.LeadSource = resLeadSource.FirstOrDefault();
        var resWarehouse = resLeadSource.GetNextResult<WarehouseLogin>();
        userData.Warehouse = resWarehouse.FirstOrDefault();
      }
      return userData;
    }

    #endregion

    #region ChangePassword
    /// <summary>
    /// Cambia el password del usuario.
    /// </summary>
    /// <param name="user">UserName</param>
    /// <param name="newPassword">New Password</param>
    /// <param name="serverDate">Server Date</param>
    /// <returns>bool</returns>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// [edgrodriguez] 22/Mar/2016 Modified. Se agregó el dbContext.Entry. Ya que el usuario
    ///                            lo obtiene desde otro método.
    /// </history>
    public static bool ChangePassword(string user, string newPassword, DateTime serverDate)
    {
      int result;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        Personnel personnel = GetPersonnelById(user);
        personnel.pePwd = newPassword;
        personnel.pePwdD = serverDate.Date;
        dbContext.Entry(personnel).State = System.Data.Entity.EntityState.Modified;
        result = dbContext.SaveChanges();
      }
      return Convert.ToBoolean(result);
    }

    #endregion

    #region GetPersonnel

    /// <summary>
    /// Obtiene un listado de personal
    /// </summary>
    /// <param name="leadSources">Lead Sources</param>
    /// <param name="salesRooms">Salas de ventas</param>
    /// <param name="roles">Roles</param>
    /// <param name="status">Estatus.
    /// -1  Todos
    /// 0 Inactivos
    /// 1 Activos
    /// </param>
    /// <param name="permission">Permiso</param>
    /// <param name="relationalOperator">Operador logico</param>
    /// <param name="level">Nivel</param>
    /// <param name="dept">Departamento</param>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// </history>
    public static List<PersonnelShort> GetPersonnel(string leadSources = "ALL", string salesRooms = "ALL",
      string roles = "ALL", int status = 1, string permission = "ALL",
      string relationalOperator = "=", EnumPermisionLevel level = EnumPermisionLevel.None, string dept = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetPersonnel(leadSources, salesRooms, roles, ((byte)status), permission, relationalOperator, ((int)level), dept).ToList();
      }
    }
    #endregion

    #region GetPersonnelById

    /// <summary>
    /// Obtiene un personal dada su clave
    /// </summary>
    /// <param name="id">Clave</param>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// </history>
    public static Personnel GetPersonnelById(string id)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.Personnels.Where(p => p.peID == id).FirstOrDefault();
      }
    }

    #endregion
  }
}
