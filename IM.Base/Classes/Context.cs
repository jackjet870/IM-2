using IM.Model.Classes;
using PalaceResorts.Common.PalaceTools;

namespace IM.Base.Classes
{
  /// <summary>
  /// Clase que contendra la informacion del contexto en un modulo
  /// </summary>
  /// <history>
  /// [wtorres]   15/Sep/2016 Created
  /// </history>
  public static class Context
  {
    #region Atributos

    private static string _environment;
    private static string _module;

    #endregion

    #region Propiedades

    // Informacion del usuario autentificado en un modulo
    public static UserData User;

    #region Environment

    /// <summary>
    /// Ambiente
    /// </summary>
    /// <history>
    /// [wtorres]  19/Sep/2016 Created
    /// </history>
    public static string Environment
    {
      get
      {
        if (string.IsNullOrEmpty(_environment))
        {
          _environment = ConfigHelper.GetString("Environment");
        }
        return _environment;
      }
    }

    #endregion

    #region Environment

    /// <summary>
    /// Nombre del modulo
    /// </summary>
    /// <history>
    /// [wtorres]  19/Sep/2016 Created
    /// </history>
    public static string Module
    {
      get
      {
        if (string.IsNullOrEmpty(_module))
        {
          _module = ConfigHelper.GetString("Module");
        }
        return _module;
      }
    }

    #endregion

    #endregion
  }
}