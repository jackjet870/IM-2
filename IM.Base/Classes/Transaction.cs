using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Base.Classes
{
  ///<summary>Clase para creacion de archivos de log</summary>
  ///<history>
  ///[michan] 14/04/2016 Created
  ///</history>
  public partial class Transaction
  {
    public DateTime Date { get; set; }
    public string LogLevel { get; set; }
    public string Message { get; set; }
  }
}
