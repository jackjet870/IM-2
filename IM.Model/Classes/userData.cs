using System.Collections.Generic;

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
    public UserLogin User { get; set; }
    public List<RoleLogin> Roles { get; set; }
    public List<PermissionLogin> Permissions { get; set; }
    public SalesRoomLogin SalesRoom { get; set; }
    public LocationLogin Location { get; set; }
    public LeadSourceLogin LeadSource { get; set; }
    public WarehouseLogin Warehouse { get; set; }
  }
}
