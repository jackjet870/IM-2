using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRChargeTos
  {
    #region GetChargeTos
    /// <summary>
    /// Devuelve la lista de chargeTo
    /// </summary>
    /// <param name="chargeTo">Entidad que contiene los filtros adicionales, puede ser null</param>
    /// <param name="nCxC">-1.Todos | 0. CxC=falso | 1. CxC=true</param>
    /// <returns>Lista de Charge To</returns>
    /// <history>
    /// [Emoguel] created 01/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto
    /// </history>
    public static List<ChargeTo> GetChargeTos(ChargeTo chargeTo=null, int nCxC=-1)
    {
      
      List<ChargeTo> lstCharge = new List<ChargeTo>();

      using (var dbContext=new IMEntities())
      {
        var query = from c in dbContext.ChargeTos
                    select c;

        if(nCxC!=-1)//Filtra por CxC
        {
          bool blnCxc = Convert.ToBoolean(nCxC);
          query = query.Where(c=>c.ctIsCxC==blnCxc);
        }

        if (chargeTo != null)//Si se recibió objeto
        {
          if (!string.IsNullOrWhiteSpace(chargeTo.ctID))//Filtra por ID
          {
            query = query.Where(c => c.ctID == chargeTo.ctID);
          }

          if (chargeTo.ctPrice > 0)//Filtra por Price
          {
            query = query.Where(c => c.ctPrice == chargeTo.ctPrice);
          }
        }


        lstCharge = query.OrderBy(c=>c.ctID).ToList();
      }
      return lstCharge;
      
    }
    #endregion
    #region SaveChargeTo
    /// <summary>
    /// Funcion que guarda|actualiza un registro en el catalogo ChargeTo
    /// </summary>
    /// <param name="chargeTo">Entidad a guardar en la BD</param>
    /// <param name="blnUpd">true. Actualizar | false. Insertar</param>
    /// <returns>0. No se guardó el registro | 1. El registro se guardó | 2.- Existe un registro con el mismo ID</returns>
    /// <history>
    /// [Emoguel] 01/03/2016
    /// </history>
    public static int SaveChargeTo(ChargeTo chargeTo,bool blnUpd)
    {
      
      int nRes = 0;
      using (var dbContext = new IMEntities())
      {
        if (blnUpd)//Actualizar
        {
          dbContext.Entry(chargeTo).State = System.Data.Entity.EntityState.Modified;
          nRes = dbContext.SaveChanges();
        }
        else//Insertar
        {
          ChargeTo chargeToVal = dbContext.ChargeTos.Where(c => c.ctID == chargeTo.ctID).FirstOrDefault();
          if(chargeToVal!=null)//Validamos que no exista un registro con el mismo ID
          {
            nRes = 2;
          }
          else
          {
            dbContext.ChargeTos.Add(chargeTo);
            nRes = dbContext.SaveChanges();
          }
        }
      }
      return nRes;
    }
    #endregion
  }
}
