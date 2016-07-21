using System;

namespace IM.Model.Classes
{
  /// <summary>
  /// Clase que sirve para enlistar elementos en una lista
  /// </summary>
  /// <history>
  /// [erosado] 01/Jun/2016 Created
  /// [emoguel] 29/Jun/2016 Modified. Agregue la propiedad Category
  /// [wtorres] 15/Jul/2016 Modified. Agregue la propiedad ImageUrl
  /// </history>
  public class Item
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public string By { get; set; }
    public string Category { get; set; }
    public string ImageUrl { get; set; }
  }
}
