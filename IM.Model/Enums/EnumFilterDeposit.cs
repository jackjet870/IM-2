using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de los diferentes filtros de depositos
  /// </summary>
  /// <history>
  ///   [vku] 06/Abr/2016 Created
  /// </history>
  public enum EnumFilterDeposit
  {
    [Description("Sin filtro")]
    fdAll,
    [Description("Con desposito (Deposits)")]
    fdDeposit,
    [Description("Sin desposito (Flyers)")]
    fdNoDeposit,
    [Description("Con deposito y shows sin deposito (Deposits & Flyers Show)")]
    fdDepositShowsNoDeposit,
  }
}
