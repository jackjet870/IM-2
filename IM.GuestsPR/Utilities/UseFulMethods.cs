using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;

namespace IM.GuestsPR.Utilities
{
  public class UseFulMethods
  {
    /// <summary>
    /// Genera las columnas que necesito en el reporte RPTStatistics
    /// </summary>
    /// <returns>List<ColumnFormat></returns>
    /// <history>
    /// [erosado] 14/Mar/2016  Created
    /// </history>
    public static ColumnFormatList getExcelFormatTable()
    {
      ColumnFormatList formatColumns = new ColumnFormatList();
      formatColumns.Add("GUID", "guID");
      formatColumns.Add("Last Name", "guLastName1");
      formatColumns.Add("First Name", "guFirstName1");
      formatColumns.Add("LS", "guls");
      formatColumns.Add("SR", "gusr");
      formatColumns.Add("Agency ID", "guag");
      formatColumns.Add("Agency", "agN");
      formatColumns.Add("Market ID", "gumk");
      formatColumns.Add("Market", "mkN");
      formatColumns.Add("Ext", "External", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("Rbk", "Rebook", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("Check-In D", "guCheckInD", format:  EnumFormatTypeExcel.Date); // date
      formatColumns.Add("PR A", "guPRAssign");
      formatColumns.Add("Avl", "guAvail", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("O.Avl", "guOriginAvail", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("Info D", "guInfo", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("guInfoD", "guInfoD", format:  EnumFormatTypeExcel.Date); //date
      formatColumns.Add("PR Info", "guPRInfo");
      formatColumns.Add("PR Info Name", "PRInfoN");
      formatColumns.Add("FU", "guFollow", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("Follow D", "guFollowD", format:  EnumFormatTypeExcel.Date);//date
      formatColumns.Add("PR Follow", "guPRFollow");
      formatColumns.Add("PR Follow Name", "PRFollowN");
      formatColumns.Add("Invit", "guInvit", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("Book D", "guBookD", format:  EnumFormatTypeExcel.Date);//date
      formatColumns.Add("PR", "guPRInvit1");
      formatColumns.Add("PR Name", "PR1N");
      formatColumns.Add("PR 2", "guPRInvit2");
      formatColumns.Add("PR 2 Name", "PR2N");
      formatColumns.Add("PR 3", "guPRInvit3");
      formatColumns.Add("PR 3 Name", "PR3N");
      formatColumns.Add("Qui", "guQuinella", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("Books", "guRoomsQty", format:  EnumFormatTypeExcel.Number);
      formatColumns.Add("SH", "guShow", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("Show D", "guShowD", format:  EnumFormatTypeExcel.Date);//date
      formatColumns.Add("Tour", "guTour", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("IO", "guInOut", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("WO", "guWalkOut", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("QS", "guQuinellaSplit", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("Shows", "guShowsQty", format:  EnumFormatTypeExcel.Number);
      formatColumns.Add("Sale", "guSale", format:  EnumFormatTypeExcel.Boolean);
      formatColumns.Add("Sales", "Sales", format:  EnumFormatTypeExcel.Number);
      formatColumns.Add("Amount", "SalesAmount", format:  EnumFormatTypeExcel.Currency);
      return formatColumns;
    }

  }
}
