namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de modos de datos
  /// </summary>
  /// <history>
  /// [emoguel] 14/Abr/2016 Created
  /// [emoguel] 29/Abr/2016 Modified. Agregue el modo Delete
  /// [wtorres] 11/Ago/2016 Modified. Fusione este enumerado con:
  ///                       1. enumMode de Host que tenia la opcion EditPartial [jorcanche] y [vipacheco]
  ///                       2. EnumModeOpen de Host que tiene la opcion PreviewEdit [vipacheco]
  /// </history>
  public enum EnumMode
  {
    ReadOnly,
    Add,
    Edit,
    EditPartial,

    //TODO: TONY -> Por favor elimina esta opcion. La utilizas en Gifts Receipts y Meal Tickets
    PreviewEdit,
    Search,
    Delete
  }
}
