using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Entities;
using IM.Model.Enums;

namespace IM.BusinessRules.BR
{
  public class BRPersonnel
  {
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
    public static UserData login(LoginType logintype, string user = "", string place = "")
    {
      UserData usrD = new UserData();
      using (var dbContext = new IMEntities())
      {
        var _usr = dbContext.USP_OR_Login(Convert.ToByte(logintype), user, place);
        usrD.User = _usr.FirstOrDefault();
        var _roles = _usr.GetNextResult<rolesLogin>();
        usrD.Roles = _roles.ToList();
        var _permissions = _roles.GetNextResult<permissionsLogin>();
        usrD.Permissions = _permissions.ToList();
        var _salesroom = _permissions.GetNextResult<salesRoomLogin>();
        usrD.SalesRoom = _salesroom.FirstOrDefault();
        var _location = _salesroom.GetNextResult<locationLogin>();
        usrD.Location = _location.FirstOrDefault();
        var _leadsource = _location.GetNextResult<leadSourceLogin>();
        usrD.LeadSource = _leadsource.FirstOrDefault();
        var _warehouse = _leadsource.GetNextResult<warehouseLogin>();
        usrD.Warehouse = _warehouse.FirstOrDefault();
      }
      return usrD;
    }

    public static IEnumerable<USP_OR_GetPersonnel_Result> PersonnelByLS(string leadSource)
    {
      UserData usrD = new UserData();
      using (var dbContext = new IMEntities())
      {
        var personnel = dbContext.USP_OR_GetPersonnel(leadSource, String.Empty, "PR", 1, String.Empty, String.Empty, null, String.Empty);
      }
      return null;
    }

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
  }
}
