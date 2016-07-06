using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Inhouse.Classes
{
  /// <summary>
  /// Banderas para saber que reportes mostrar y que no
  /// </summary>
  /// <history>
  /// [ecanul] 14/04/2016 Created
  /// [ecanul] 06/07/2016 Modified. Agregue la bandera Has MembershipsPrevious
  /// </history>
  public class RptEquityHeader
  {
    public bool HasMembershipsPrevious { get; set; }

    public bool HasGolfFields { get; set; }

    public bool HasSAIRF { get; set; }

    public bool HasSCOMP { get; set; }

    public bool HasSCRG { get; set; }

    public bool HasSIGR { get; set; }

    public bool HasSNORM { get; set; }

    public bool HasSRCI { get; set; }

    public bool HasSREF { get; set; }

    public bool HasSVEC { get; set; }

    public bool IsElite { get; set; }
  }
}
