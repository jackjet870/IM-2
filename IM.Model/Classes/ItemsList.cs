using IM.Model.Enums;
using IM.Model.Helpers;
using System.Collections.Generic;

namespace IM.Model.Classes
{
  /// <summary>
  /// Lista de elementos
  /// </summary>
  /// <history>
  /// [wtorres]  15/Jul/2016 Created
  /// </history>
  public class ItemsList : List<Item>
  {
    #region Add
    /// <summary>
    /// Agrega un elemento a la lista actual
    /// </summary>
    /// <param name="id">Clave</param>
    /// <param name="name">Descripcion</param>
    /// <param name="imageFile">Nombre del archivo de una imagen</param>
    /// <history>
    /// [wtorres]  15/Jul/2016 Created
    /// </history>
    public void Add(string id, string name, string imageFile)
    {
      Add(new Item()
      {
        Id = id,
        Name = name,
        ImageUrl = $"pack://application:,,,/IM.Styles;component/Images/{imageFile}"
      });
    }
    #endregion

    #region Add
    /// <summary>
    /// Agrega un elemento EnumMenu a la lista actual
    /// </summary>
    /// <param name="item">Enumerado del elemento</param>
    /// <history>
    /// [wtorres]  16/Jul/2016 Created
    /// </history>
    public void Add(EnumMenu item)
    {
      Add(new Item()
      {
        Id = item.ToString(),
        Name = EnumToListHelper.GetEnumDescription(item),
        ImageUrl = $"pack://application:,,,/IM.Styles;component/Images/{item.ToString()}.png"
      });
    }
    #endregion

  }
}
