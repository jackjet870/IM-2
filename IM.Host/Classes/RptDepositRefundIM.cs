using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.Host.Classes
{
  public class objRptDepositRefundIM : RptRefundLetter{}
  public class objRptDepositRefund_BookingDepositIM : RptRefundLetter_BookingDeposit { }

  public class objReportText {
    public string Header { get; set; }
    public string Footer { get; set; }
  }
}