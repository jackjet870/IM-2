using System;
using System.Linq;
using System.Collections.Generic;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;
using IM.BusinessRules.Properties;

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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
    /// 0 Todos
    /// 1 Activos
    /// 2 Inactivos 
    /// </param>
    /// <param name="permission">Permiso</param>
    /// <param name="relationalOperator">Operador logico</param>
    /// <param name="level">Nivel</param>
    /// <param name="dept">Departamento</param>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// [erosado] 04/08/2016 Modified. Se estandarizó el valor que retorna.
    /// </history>
    public async static Task<List<PersonnelShort>> GetPersonnel(string leadSources = "ALL", string salesRooms = "ALL",
      string roles = "ALL", int status = 1, string permission = "ALL",
      string relationalOperator = "=", EnumPermisionLevel level = EnumPermisionLevel.None, string dept = "ALL", string idPersonnel = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_GetPersonnel(leadSources, salesRooms, roles, ((byte)status), permission, relationalOperator, ((int)level), dept, idPersonnel).ToList();
        }
      });
    }

    #endregion

    #region GetPersonnelById

    /// <summary>
    /// Obtiene un personal dada su clave
    /// </summary>
    /// <param name="id">Clave</param>
    /// <param name="role">gb</param>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// [wtorres]    05/Jul/2016 Modified. Correccion de error. Estaba devolviendo null para personal que no tenia roles
    /// </history>
    public static Personnel GetPersonnelById(string id, string role = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.Personnels.FirstOrDefault(p => p.peID == id && (role == "ALL" || p.Roles.Any(r => r.roID == role)));
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
    /// [vku] 13/Jul/2016 Modified. Agregué Include Personnel_Liner
    /// </history>
    public async static Task<List<Personnel>> GetPersonnels(int nStatus = -1, Personnel personnel = null, bool blnLiner = false)
    {
      List<Personnel> lstPersonnel = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var query = from pe in dbContext.Personnels
                        select pe;

            if (nStatus != -1)//Filtro por estatus
            {
              bool blnStatus = Convert.ToBoolean(nStatus);
              query = query.Where(pe => pe.peA == blnStatus);
            }

            if (blnLiner)
            {
              query = query.Include(pe => pe.Personnel_Liner);
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
        });
      return lstPersonnel;
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
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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

          return query.ToList();
        }
      });
    }

    #endregion

    #region GetPersonnelByRole
    /// <summary>
    /// Devuelve registros del catalogo PR
    /// </summary>
    /// <param name="prRol">Rol</param>
    /// <returns>Lista tipo PersonnelSHort</returns>
    /// <history>
    /// [emoguel] modified 09/06/2016--> se volvió async
    /// </history>
    public async static Task<List<PersonnelShort>> GetPersonnelByRole(string prRol)
    {
      List<PersonnelShort> lstPersonnel = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var query = from p in dbContext.Personnels
                        where p.Roles.Where(r => r.roID == prRol).Count() > 0
                        select p;

            return query.ToList().Select(p => new PersonnelShort { peID = p.peID, peN = p.peN }).ToList();
          }
        });
      return lstPersonnel;
    }
    #endregion

    #region DeletePersonnels
    /// <summary>
    /// Elimina Personnels de la BD
    /// </summary>
    /// <param name="lstPersonnels"></param>
    /// <returns>0. No se pudo eliminar | >0. Se eliminarion correctamente</returns>
    /// <history>
    /// [emoguel] created 14/06/2016
    /// </history>
    public static async Task<int> DeletePersonnels(List<PersonnelShort> lstPersonnels)
    {
      int nRes = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
              try
              {
                dbContext.Database.CommandTimeout = Settings.Default.USP_OR_DeletePersonnel;
                lstPersonnels.ForEach(pe =>
                {
                  dbContext.USP_OR_DeletePersonnel(pe.peID);
                });

                int nSave = dbContext.SaveChanges();
                transacction.Commit();
                return 1;
              }
              catch
              {
                transacction.Rollback();
                throw;
              }
            }
          }
        });

      return nRes;
    }
    #endregion

    #region SavePersonnel
    /// <summary>
    /// Guarda un personnel y todos sus permisos
    /// </summary>
    /// <param name="idUser">id del usuario que está editando el personnel</param>
    /// <param name="personnel">personnel a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. inserta</param>
    /// <param name="lstPermissionAdd">Permisos a agregar</param>
    /// <param name="lstPermissionDel">Permisos a eliminar</param>
    /// <param name="lstPermissionUpd">Permisos a actualizar</param>
    /// <param name="lstLeadSourceDel">Leadsource a eliminar</param>
    /// <param name="lstLeadSourceAdd">LeadSource a agregar</param>
    /// <param name="lstWarehouseDel">Warehouses a eliminar</param>
    /// <param name="lstWarehousesAdd">Warehouses a agregar</param>
    /// <param name="lstSalesRoomDel">Salesroom a eliminar</param>
    /// <param name="lstSalesRoomAdd">SalesRoom a agregar</param>
    /// <param name="lstRolesDel">Roles a eliminar</param>
    /// <param name="lstRoleAdd">Roles a agregar</param>
    /// <param name="blnPostLog">Si se modifico el puesto</param>
    /// <param name="blnTeamsLog">Si se cambio de tem</param>
    /// <returns>-1. Existe un registro con el mismo ID | 0. No se guardó | >0. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 22/06/2016
    /// </history>
    public static async Task<int> SavePersonnel(string idUser, Personnel personnel, bool blnUpdate, List<PersonnelPermission> lstPermissionAdd, List<PersonnelPermission> lstPermissionDel, List<PersonnelPermission> lstPermissionUpd,
      List<PersonnelAccess> lstLeadSourceDel, List<PersonnelAccess> lstLeadSourceAdd, List<PersonnelAccess> lstWarehouseDel, List<PersonnelAccess> lstWarehousesAdd, List<PersonnelAccess> lstSalesRoomDel,
      List<PersonnelAccess> lstSalesRoomAdd, List<Role> lstRoleDel, List<Role> lstRoleAdd, bool blnPostLog, bool blnTeamsLog)
    {
      int nRes = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
              try
              {
                Personnel personnelSave = new Personnel();
                #region Personnel
                if (blnUpdate)//Actualizar
                {
                  personnelSave = dbContext.Personnels.Where(pe => pe.peID == personnel.peID).Include(pe => pe.Roles).FirstOrDefault();
                  //dbContext.Entry(personnel).State = EntityState.Modified;
                  ObjectHelper.CopyProperties(personnelSave, personnel);
                }
                else//Agregar
                {
                  if (dbContext.Personnels.Where(pe => pe.peID == personnel.peID).FirstOrDefault() != null)
                  {
                    return -1;
                  }
                  else
                  {
                    dbContext.Personnels.Add(personnel);
                    personnelSave = personnel;
                  }
                }
                #endregion

                #region PersonnelPermission
                //Eliminar
                lstPermissionDel.ForEach(pp =>
                {
                  dbContext.Entry(pp).State = EntityState.Deleted;
                });

                //Add
                lstPermissionAdd.ForEach(pp =>
                {
                  pp.pppe = personnelSave.peID;
                  dbContext.Entry(pp).State = EntityState.Added;
                });

                //update
                lstPermissionUpd.ForEach(pp =>
                {
                  pp.pppe = personnelSave.peID;
                  dbContext.Entry(pp).State = EntityState.Modified;
                });
                #endregion

                #region Roles
                //Del
                personnelSave.Roles.Where(ro => lstRoleDel.Any(roo => ro.roID == roo.roID)).ToList().ForEach(ro =>
                {
                  personnelSave.Roles.Remove(ro);
                });

                //Add
                lstRoleAdd = dbContext.Roles.AsEnumerable().Where(ro => lstRoleAdd.Any(roo => ro.roID == roo.roID)).ToList();
                lstRoleAdd.ForEach(ro =>
                {
                  personnelSave.Roles.Add(ro);
                });
                #endregion

                #region leadSource
                //Eliminar
                lstLeadSourceDel.ForEach(pl =>
                {
                  pl.plpe = personnelSave.peID;
                  pl.plLSSR = "LS";
                  dbContext.Entry(pl).State = EntityState.Deleted;
                });

                //Del
                lstLeadSourceAdd.ForEach(pl =>
                {
                  pl.plpe = personnelSave.peID;
                  pl.plLSSR = "LS";
                  dbContext.PersonnelAccessList.Add(pl);
                });
                #endregion

                #region SalesRoom
                //Del
                lstSalesRoomDel.ForEach(pl =>
                {
                  pl.plpe = personnelSave.peID;
                  pl.plLSSR = "SR";
                  dbContext.Entry(pl).State = EntityState.Deleted;
                });

                //Add
                lstSalesRoomAdd.ForEach(pl =>
                {
                  pl.plpe = personnelSave.peID;
                  pl.plLSSR = "SR";
                  dbContext.PersonnelAccessList.Add(pl);
                });
                #endregion

                #region Warehouses
                //Del
                lstWarehouseDel.ForEach(pl =>
                {
                  pl.plpe = personnelSave.peID;
                  pl.plLSSR = "WH";
                  dbContext.Entry(pl).State = EntityState.Deleted;
                });

                //Add
                lstWarehousesAdd.ForEach(pl =>
                {
                  pl.plpe = personnelSave.peID;
                  pl.plLSSR = "WH";
                  dbContext.PersonnelAccessList.Add(pl);
                });
                #endregion

                DateTime dtmServerDate = BRHelpers.GetServerDateTime();

                #region postLog
                if (blnPostLog)
                {
                  PostLog postLog = new PostLog();
                  postLog.ppChangedBy = idUser;
                  postLog.ppDT = dtmServerDate;
                  postLog.pppe = personnel.peID;
                  postLog.pppo = personnel.pepo;
                  dbContext.PostsLogs.Add(postLog);
                }
                #endregion

                #region TeamsLog
                if (blnTeamsLog)
                {
                  TeamLog teamLog = new TeamLog();
                  teamLog.tlDT = dtmServerDate;
                  teamLog.tlChangedBy = idUser;
                  teamLog.tlpe = personnel.peID;
                  teamLog.tlTeamType = (personnel.peTeamType == "") ? null : personnel.peTeamType;
                  teamLog.tlPlaceID = personnel.pePlaceID;
                  teamLog.tlTeam = personnel.peTeam;
                  dbContext.TeamsLogs.Add(teamLog);
                }
                #endregion

                int nSave = dbContext.SaveChanges();
                transacction.Commit();
                return nSave;
              }
              catch
              {
                transacction.Rollback();
                throw;
              }
            }
          }
        });
      return nRes;
    }

    #endregion

    #region PersonnelChageID
    /// <summary>
    /// Cambia el ID de un personnel
    /// </summary>
    /// <returns>0 No se realizó nada | 1. se realizó correctamente</returns>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    public static async Task<int> UpdatePersonnelId(string idOld, string idNew)
    {
      int nRes = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            dbContext.Database.CommandTimeout = Settings.Default.USP_OR_UpdatePersonnelId;
            return dbContext.USP_OR_UpdatePersonnelId(idOld, idNew);
          }
        });

      return nRes;
    }
    #endregion

    #region GetPersonnelStatistics
    /// <summary>
    /// Devuelve los statistics de un personnel
    /// </summary>
    /// <param name="idPersonnel">id del personnel a buscar sus statistics</param>
    /// <returns>PersonnelStatistics</returns>
    /// <history>
    /// [emoguel] created 24/06/2016
    /// </history>
    public static async Task<PersonnelStatistics> GetPersonnelStatistics(string idPersonnel)
    {
      PersonnelStatistics personnelStatistics = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            dbContext.Database.CommandTimeout = Settings.Default.USP_OR_GetPersonnelStatistics;
            return dbContext.USP_OR_GetPersonnelStatistics(idPersonnel).FirstOrDefault();
          }
        });
      return personnelStatistics;
    }
    #endregion


    #region IsFrontToMiddle
    /// <summary>
    /// Determina si un personal es Front To Middle
    /// </summary>
    /// <param name="personnelID">Clave del personal</param>
    /// <param name="pR1">Clave del PR 1 (Opcional)</param>
    /// <param name="pR2">Clave del PR 2 (Opcional)</param>
    /// <param name="pR3">Clave del PR 3 (Opcional)</param>
    /// <returns>bool</returns>
    /// <history>
    /// [aalcocer] created 13/08/2016
    /// </history>
    public static async Task<bool> IsFrontToMiddle(string personnelID, string pR1 = null, string pR2 = null, string pR3 = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var result = dbContext.USP_OR_EsFrontToMiddle(personnelID, pR1, pR2, pR3).FirstOrDefault();
          return Convert.ToBoolean(result);
        }
      });
    }
    #endregion


  }
}
