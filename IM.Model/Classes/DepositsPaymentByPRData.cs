using System.Collections.Generic;

namespace IM.Model.Classes
{
  /// <summary>
  ///   Entidad que trae la informacion para el reporte de pago de depositos a los PRs (Comisiones de PRs de Outhouse)
  /// </summary>
  /// <history>
  ///   [vku] 26/Abr/2016 Created
  /// </history>
  public class DepositsPaymentByPRData
  {
    public List<RptDepositsPaymentByPR> DepositsPaymentByPR { get; set; }
    public List<RptDepositsPaymentByPR_Deposit> DepositsPaymentByPR_Deposit { get; set; }
    public List<CurrencyShort> Currencies { get; set; }
    public List<PaymentTypeShort> PaymentTypes { get; set; }
  }
}
