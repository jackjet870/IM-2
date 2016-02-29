using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Entities
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
    public userLogin User { get; set; }
    public List<rolesLogin> Roles { get; set; }
    public List<permissionsLogin> Permissions { get; set; }
    public salesRoomLogin SalesRoom { get; set; }
    public locationLogin Location { get; set; }
    public leadSourceLogin LeadSource { get; set; }
    public warehouseLogin Warehouse { get; set; }
  }
}
