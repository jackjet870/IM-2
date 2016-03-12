using System.ComponentModel;

namespace IM.Base.Helpers
{
  public class ObjectHelper
  {
    /// <summary>
    /// Copia el valor de los atributos de un objeto a uno nuevo
    /// para no modificar directamente sobre el datacontext
    /// </summary>
    /// <param name="objNew">Objeto para llenar con los valores</param>
    /// <param name="ObjOld">Objeto a copiar los valores</param>
    /// <history>
    /// [emoguel] created 12/03/2016
    /// </history>
    public static void CopyProperties(object objNew, object ObjOld)
    {
      foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(ObjOld))
      {
        item.SetValue(objNew, item.GetValue(ObjOld));
      }
    }

  }
}
