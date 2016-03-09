using System;
using System.Linq;
using System.Collections.Generic;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;


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
      using (var dbContext = new IMEntities())
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

    public static bool ChangePassword(string user, string newPassword, DateTime serverDate)
    {
      int result;
      using (var dbContext = new IMEntities())
      {
        Personnel _personnel = dbContext.Personnels.Where(c => c.peID == user).FirstOrDefault();
        _personnel.pePwd = newPassword;
        _personnel.pePwdD = serverDate.Date;

        result = dbContext.SaveChanges();
      }
      return Convert.ToBoolean(result);
    }

    #endregion

    #region GetPersonnel

    public static List<PersonnelShort> GetPersonnel(string leadSource)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetPersonnel(leadSource, "ALL", "PR", 1, "ALL", "=", 0, "ALL").ToList();
      }
    }

    #endregion
  }
}
