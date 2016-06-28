using System;
using System.Linq;
using System.Collections.Generic;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;

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
    /// 0 Inactivos
    /// 1 Activos
    /// 2 Todos
    /// </param>
    /// <param name="permission">Permiso</param>
    /// <param name="relationalOperator">Operador logico</param>
    /// <param name="level">Nivel</param>
    /// <param name="dept">Departamento</param>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async static Task<List<PersonnelShort>> GetPersonnel(string leadSources = "ALL", string salesRooms = "ALL",
      string roles = "ALL", int status = 1, string permission = "ALL",
      string relationalOperator = "=", EnumPermisionLevel level = EnumPermisionLevel.None, string dept = "ALL" , string idPersonnel = "ALL")
    {
      List<PersonnelShort> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_OR_GetPersonnel(leadSources, salesRooms, roles, ((byte)status), permission, relationalOperator,
              ((int)level), dept, idPersonnel).ToList();
        }
      });
      return result;
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
    public static Personnel GetPersonnelById(string id, string role = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.Personnels.
          FirstOrDefault(p => p.peID == id && p.Roles.Where(r => r.roID == (role.Equals("ALL")?r.roID:role)).Count() > 0);
      }
    }

    #endregion

    #region GetPersonnels
    /// <summary>
    /// Obtiene registros del catalogo Perssonels
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Inactivos</param>
    /// <param name="personnel">Objeto con los filtros adicionales</param>
    /// <returns>Lista de tipo Personnel</returns>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    public static List<Personnel> GetPersonnels(int nStatus = -1, Personnel personnel = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from pe in dbContext.Personnels
                    select pe;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(pe => pe.peA == blnStatus);
        }

        if (personnel != null)
        {
          if (!string.IsNullOrWhiteSpace(personnel.peID))//Filtro por ID
          {
            query = query.Where(pe => pe.peID == personnel.peID);
          }

          if (!string.IsNullOrWhiteSpace(personnel.pede))//Filtro por dept
          {
            query = query.Where(pe => pe.pede == personnel.pede);
          }
        }

        return query.OrderBy(pe => pe.peN).ToList();
      }
    } 
    #endregion

    #region GetPersonnelAccess

    /// <summary>
    /// Obtiene una lista de accesos de personal
    /// </summary>
    /// <history>
    /// [edgrodriguez]  27/Abr/2016 Created
    /// [edgrodriguez] 23/May/2016 Modified Se agrego asincronia.
    /// </history>
    public async static Task<List<Personnel>> GetPersonnelAccess()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = (from pe in dbContext.Personnels
                     join pa in (from pa in dbContext.PersonnelAccessList
                                 join ls in dbContext.LeadSources on pa.plLSSRID equals ls.lsID
                                 where pa.plLSSR == "LS" && ls.lspg == "IH" && ls.lsA
                                 select pa)
                       on pe.peID equals pa.plpe
                     where pe.peA
                     select pe
          ).Distinct().OrderBy(c => c.peN);

        return await query.ToListAsync();
      }
    }

    #endregion

    #region GetPersonnelByRole
    public static List<PersonnelShort> GetPersonnelByRole(string prRol)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from p in dbContext.Personnels
                    where p.Roles.Where(r => r.roID == prRol).Count() > 0
                    select p;

        return query.ToList().Select(p => new PersonnelShort { peID = p.peID, peN = p.peN }).ToList();
      }
    }
    #endregion
  }
}
