using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado con las diferentes opciones del menu
  /// </summary>
  /// <history>
  /// [wtorres]  11/04/2016 Modified. Movido desde IM.Launcher
  /// </history>
  public enum EnumMenu
  {
    [Description("Inhouse")]
    Inhouse,

    [Description("Assignment")]
    Assignment,

    [Description("Mail Outs")]
    MailOuts,

    [Description("Outhouse")]
    Outhouse,

    [Description("Host")]
    Host,

    [Description("Inventory Movs")]
    InventoryMovs,

    [Description("Processor INH")]
    ProcessorINH,

    [Description("Processor OUT")]
    ProcessorOUT,

    [Description("Processor GRAL")]
    ProcessorGRAL,

    [Description("Processor Sales")]
    ProcessorSales,

    [Description("PR Statistics")]
    PRStatistics,

    [Description("Graph")]
    Graph,

    [Description("Guests by PR")]
    GuestsByPR,

    [Description("Sales by PR")]
    SalesByPR,

    [Description("Sales by Liner")]
    SalesByLiner,

    [Description("Sales by Closer")]
    SalesByCloser,

    [Description("Administrator")]
    Administrator,

    [Description("Mail Outs Config")]
    MailOutsConfig,

    [Description("Invitations Config")]
    InvitationsConfig,

    [Description("Printer Config")]
    PrinterConfig
  }
}
