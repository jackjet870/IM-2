using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRWhsMovs
  {
    /// <summary>
    /// Método para obtener una lista de movimientos del almacén en una fecha específica.
    /// </summary>
    /// <param name="wmwh">ID del almacen.</param>
    /// <param name="wmD">Fecha del movimiento.</param>
    /// <returns> List<GetWhsMovs> </returns>
    /// <history>
    /// [edgrodriguez] 19/Feb/2016 Created
    /// </history>
    public static List<GetWhsMovs> getWhsMovs(string wmwh, DateTime wmD)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetWhsMovs(wmwh, wmD).ToList();
      }
    }

    /// <summary>
    /// Método para guardar nuevos movimientos en un almacén.
    /// </summary>
    /// <param name="lstWhsMovs">Lista de movimientos para agregar.</param>
    /// <history>
    /// [edgrodriguez] 22/Feb/2016 Created
    /// </history>
    public static void saveWhsMovs(ref List<WhsMov> lstWhsMovs)
    {
      IEnumerable<WhsMov> lstResult;
      using (var dbContext = new IMEntities())
      {
        lstResult = dbContext.WhsMovs.AddRange(lstWhsMovs);
        dbContext.SaveChanges();
      }
      lstWhsMovs = lstResult.ToList();
    }
  }
}
