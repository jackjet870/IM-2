using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRFoliosCXC
  {

    #region GetFoliosCXC
    /// <summary>
    /// Obtiene registros del catalogo FoliosCXC
    /// </summary>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1.Registros Activos</param>
    /// <returns>Lista de tipo FolioCXC</returns>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// [emoguel] modified se volvió async
    /// </history>
    public async static Task<List<FolioCXC>> GetFoliosCXC(FolioCXC folioCXC=null,int nStatus=-1)
    {
      List<FolioCXC> lstFolios = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var query = from fi in dbContext.FoliosCXC
                        select fi;

            if (nStatus != -1)//Filtro por estatus
          {
              bool blnEstatus = Convert.ToBoolean(nStatus);
              query = query.Where(fi => fi.fiA == blnEstatus);
            }

            if (folioCXC != null)//Validamos si se tiene un objeto
          {
              if (folioCXC.fiFrom > 0 && folioCXC.fiTo > 0)//Validamos que se tenga un rango mayor a 0
            {
                query = query.Where(fi => folioCXC.fiFrom >= fi.fiFrom && folioCXC.fiTo <= fi.fiTo);//Filtramos por folio
            }
            }

            return query.OrderBy(fi => fi.fiID).ToList();//Ordenamos por ID
        }
        });
      return lstFolios;
    }
    #endregion

    #region SaveFolioCXC
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo de FoliosCXC
    /// </summary>
    /// <param name="folioCXC">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <returns>0. No se guardó | 1. Guardado correctamente | -2. Verificar el rango</returns>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    public static int SaveFolioCXC(FolioCXC folioCXC,bool blnUpdate)
    {
      bool blnIsValid = false;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        #region Actualizar
        if (blnUpdate)//Actualizar
        {
          blnIsValid =(bool)(dbContext.USP_OR_ValidateFolioCXC(folioCXC.fiFrom, folioCXC.fiTo, folioCXC.fiA, 1).FirstOrDefault());//Valida si se puede guardar
          

          if (blnIsValid)//Si es valido
          {
            dbContext.Entry(folioCXC).State = System.Data.Entity.EntityState.Modified;
          }
          else//No es valido
          {
            return -2;
          }
        }
        #endregion
        #region Insertar
        else//Insertar
        {
          blnIsValid = (bool)(dbContext.USP_OR_ValidateFolioCXC(folioCXC.fiFrom, folioCXC.fiTo, folioCXC.fiA, 0).FirstOrDefault());//Valida si se puede guardar

          if (blnIsValid)//Si es valido
          {
            dbContext.FoliosCXC.Add(folioCXC);
          }
          else//No es valido
          {
            return -2;
          }
        }
        #endregion
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
