﻿using System.Collections.Generic;
using IM.Model.Enums;

namespace IM.Model.Classes
{
  /// <summary>
  /// Entidad de Usuario. Contiene todos los permisos, roles e información específica.
  /// Segun el tipo de login.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 25/Feb/2016 Created
  /// </history>
  public class UserData
  {

    #region Propiedades

    public UserLogin User { get; set; }
    public List<RoleLogin> Roles { get; set; }
    public List<PermissionLogin> Permissions { get; set; }
    public SalesRoomLogin SalesRoom { get; set; }
    public LocationLogin Location { get; set; }
    public LeadSourceLogin LeadSource { get; set; }
    public WarehouseLogin Warehouse { get; set; }

    #endregion

    #region Metodos

    #region HasPermission

    /// <summary>
    /// Determina si el usuario tiene cierto nivel de un permiso
    /// </summary>
    /// <param name="permission">Permiso</param>
    /// <param name="level">Nivel</param>
    /// <history>
    /// [jorcanche]  08/Mar/2016 Created
    /// </history>
    public bool HasPermission(string permission, EnumPermisionLevel level)
    {
      return this.Permissions.Exists(p => p.pppm == permission && p.pppl >= (int)level);
    }

    #endregion

    #region HasRole

    /// <summary>
    /// Determina si el usuario tiene cierto rol
    /// </summary>
    /// <param name="role">Rol</param>
    /// <history>
    /// [jorcanche]  08/Mar/2016 Created
    /// </history>
    public bool HasRole(string role)
    {
      return this.Roles.Exists(p => p.prro == role);
    }
    #endregion

    #endregion
  }
}