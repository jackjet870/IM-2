using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
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
    /// <history>
    /// [edgrodriguez] 30/Abr/2016 Modified. Se implemento nuevo proceso para obtener los multiples selects
    /// que devuelve el StoreProcedure
    /// </history>
    public static UserData Login(EnumLoginType logintype, string user = "", string place = "")
    {
      var userData = new UserData();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var resUser = dbContext.USP_OR_Login(Convert.ToByte(logintype), user, place)
          .MultipleResults()
         .With<UserLogin>()
         .With<RoleLogin>()
         .With<PermissionLogin>()
         .With<SalesRoomLogin>()
         .With<LocationLogin>()
         .With<LeadSourceLogin>()
         .With<WarehouseLogin>()
         .GetValues();

        userData.User = (resUser[0] as List<UserLogin>).FirstOrDefault();
        userData.Roles = (resUser[1] as List<RoleLogin>);
        userData.Permissions = (resUser[2] as List<PermissionLogin>);
        userData.SalesRoom = (resUser[3] as List<SalesRoomLogin>).FirstOrDefault();
        userData.Location = (resUser[4] as List<LocationLogin>).FirstOrDefault();
        userData.LeadSource = (resUser[5] as List<LeadSourceLogin>).FirstOrDefault();
        userData.Warehouse = (resUser[6] as List<WarehouseLogin>).FirstOrDefault();
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
        return
          dbContext.USP_OR_GetPersonnel(leadSources, salesRooms, roles, ((byte) status), permission, relationalOperator,
            ((int) level), dept).ToList();
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
        return dbContext.Personnels.FirstOrDefault(p => p.peID == id);
      }
    }

    #endregion

    #region GetPersonnelAccess

    /// <summary>
    /// Obtiene una lista de accesos de personal
    /// </summary>
    /// <history>
    /// [edgrodriguez]  27/Abr/2016 Created
    /// </history>
    public static List<Personnel> GetPersonnelAccess()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from pe in dbContext.Personnels
          join pa in (from pa in dbContext.PersonnelAccessList
            join ls in dbContext.LeadSources on pa.plLSSRID equals ls.lsID
            where pa.plLSSR == "LS" && ls.lspg == "IH" && ls.lsA
            select pa)
            on pe.peID equals pa.plpe
          where pe.peA
          select pe
          ).Distinct().OrderBy(c=>c.peN).ToList();
      }
    }

    #endregion

    /// <summary>
    /// Método para obtener los datos del usuario
    /// como son datos principales, permisos, roles
    /// e informacion específica segun el tipo de login.
    /// </summary>
    /// <param name="loginType">EnumLoginType</param>
    /// <param name="user">Usuario</param>
    /// <param name="psw">Contraseña</param>
    /// <param name="place">Lugar</param>
    /// <returns>UserData</returns>
    /// <history>
    /// [erosado] 26/04/2016  Created
    /// </history>
    public static UserData login2(EnumLoginType loginType, string user, string psw, string place = "")
    {
      //using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      //{
      //  var personnel = dbContext.USP_IM_Login(Convert.ToByte(loginType), user, psw, place);
      //  var userData = new UserData();
      //  userData.User = personnel.FirstOrDefault();

      //  if (userData.User == null) return null;
      //  var resRoles = personnel.GetNextResult<RoleLogin>();
      //  userData.Roles = resRoles.ToList();
      //  var resPermissions = resRoles.GetNextResult<PermissionLogin>();
      //  userData.Permissions = resPermissions.ToList();
      //  var resSalesroom = resPermissions.GetNextResult<SalesRoomLogin>();
      //  userData.SalesRoom = resSalesroom.FirstOrDefault();
      //  var resLocation = resSalesroom.GetNextResult<LocationLogin>();
      //  userData.Location = resLocation.FirstOrDefault();
      //  var resLeadSource = resLocation.GetNextResult<LeadSourceLogin>();
      //  userData.LeadSource = resLeadSource.FirstOrDefault();
      //  var resWarehouse = resLeadSource.GetNextResult<WarehouseLogin>();
      //  userData.Warehouse = resWarehouse.FirstOrDefault();
      //  return userData;
      //}
      UserData userData = new UserData();
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var resUser = dbContext.USP_OR_Login(Convert.ToByte(loginType), user, place);
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
  }
}
