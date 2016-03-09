using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Enums;
using System.Windows.Controls;

namespace IM.Administrator.Helpers
{
  public class PermisionHelper
  {
    #region permiso para editar
    /// <summary>
    /// Valida si tienes minimo permiso para editar|agregar
    /// </summary>
    /// <param name="sPermision">Nombre del permiso</param>
    /// <returns>true. si se cuenta con el permiso | false. si no se cuenta con el permiso</returns>
    /// <history>
    /// [Emoguel] created 04/03/2016
    /// </history>
    public static bool EditPermision(string sPermision)
    {
      bool blnHavePer = false;
      PermissionLogin permision = App._lstPermision.Where(p => p.pppm == sPermision).FirstOrDefault();
      if (permision != null && permision.pppl >= (int)EnumPermisionLevel.Standard)//Permite Editar y Modificar
      {
        blnHavePer = true;    
      }
      return blnHavePer;
    }

    #endregion
    #region permiso para eliminar
    /// <summary>
    /// Valida si tienes permiso para eliminar
    /// </summary>
    /// <param name="sPermision">Nombre del permiso</param>
    /// <returns>true. si se cuenta con el permiso | false. si no se cuenta con el permiso</returns>
    /// <history>
    /// [Emoguel] created 04/03/2016
    /// </history>
    public static bool DelPermision(string sPermision)
    {
      bool blnHavePer = false;
      PermissionLogin permision = App._lstPermision.Where(p => p.pppm == sPermision).FirstOrDefault();
      if (permision != null && permision.pppl >= (int)EnumPermisionLevel.Special)//Permite Eliminar
      {
        blnHavePer = true;
      }
      return blnHavePer;
    }
    #endregion
    #region permiso para cambiar ID
    /// <summary>
    /// Valida si tienes permiso para CambiarID
    /// </summary>
    /// <param name="sPermision">Nombre del permiso</param>
    /// <returns>true. si se cuenta con el permiso | false. si no se cuenta con el permiso</returns>
    /// <history>
    /// [Emoguel] created 04/03/2016
    /// </history>
    public static bool ChangeIdPermision(string sPermision)
    {
      bool blnHavePer = false;
      PermissionLogin permision = App._lstPermision.Where(p => p.pppm == sPermision).FirstOrDefault();
      if (permision != null && permision.pppl >= (int)EnumPermisionLevel.SuperSpecial)//Permite Cambiar ID's
      {
        blnHavePer = true;
      }
      return blnHavePer;
    }
    #endregion
  }
}
