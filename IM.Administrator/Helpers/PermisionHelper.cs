using IM.Model.Enums;

namespace IM.Administrator.Helpers
{
  public class PermisionHelper
  {
    #region EditPermision
    /// <summary>
    /// Valida si tienes minimo permiso para editar|agregar
    /// </summary>
    /// <param name="sPermision">Nombre del permiso</param>
    /// <returns>true. si se cuenta con el permiso | false. si no se cuenta con el permiso</returns>
    /// <history>
    /// [emoguel] 04/03/2016 Created
    /// [wtorres] 10/03/2016 Modified. Simplifique el metodo. Ahora utiliza el metodo HasPermission
    /// </history>
    public static bool EditPermision(string sPermision)
    {
      return App.User.HasPermission(sPermision, EnumPermisionLevel.Standard);
    }

    #endregion

    #region DeletePermision

    /// <summary>
    /// Valida si tienes permiso para eliminar
    /// </summary>
    /// <param name="sPermision">Nombre del permiso</param>
    /// <returns>true. si se cuenta con el permiso | false. si no se cuenta con el permiso</returns>
    /// <history>
    /// [emoguel] 04/03/2016 Created
    /// [wtorres] 10/03/2016 Modified. Simplifique el metodo. Ahora utiliza el metodo HasPermission
    /// </history>
    public static bool DeletePermision(string sPermision)
    {
      return App.User.HasPermission(sPermision, EnumPermisionLevel.Special);
    }

    #endregion

    #region ChangeIdPermision

    /// <summary>
    /// Valida si tienes permiso para Cambiar ID
    /// </summary>
    /// <param name="sPermision">Nombre del permiso</param>
    /// <returns>true. si se cuenta con el permiso | false. si no se cuenta con el permiso</returns>
    /// <history>
    /// [emoguel] 04/03/2016 Created
    /// [wtorres] 10/03/2016 Modified. Simplifique el metodo. Ahora utiliza el metodo HasPermission
    /// </history>
    public static bool ChangeIdPermision(string sPermision)
    {
      return App.User.HasPermission(sPermision, EnumPermisionLevel.SuperSpecial);
    }

    #endregion
  }
}
