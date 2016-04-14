using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml.Style;

namespace IM.Model.Classes
{
  public class ExcelFormatGroupHeaders
  {
    /// <summary>
    /// Se pondran los valores de cada nivel de agrupacion.
    /// </summary>
    public string BackGroundColor { get; set; }
    public bool FontBold { get; set; } = false;
    public ExcelHorizontalAlignment TextAligment { get; set; } = ExcelHorizontalAlignment.Left;
    public string FontColor { get; set; } = "#FFFFFF";
  }
}
