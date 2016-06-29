using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRDesks
  {
    #region GetDesks
    /// <summary>
    /// Obtiene registros del catalogo desk
    /// </summary>
    /// <param name="desk">objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1. registros activos</param>
    /// <returns>Lista de tipo desk</returns>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    public async static Task<List<Desk>> GetDesks(Desk desk = null, int nStatus = -1)
    {
      List<Desk> lstDesk = new List<Desk>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from dk in dbContext.Desks
                      select dk;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnEstatus = Convert.ToBoolean(nStatus);
            query = query.Where(dk => dk.dkA == blnEstatus);
          }

          if (desk != null)//validacion de si hay objeto
          {
            if (desk.dkID > 0)//Filtro por ID
            {
              query = query.Where(dk => dk.dkID == desk.dkID);
            }

            if (!string.IsNullOrWhiteSpace(desk.dkN))//Filtro por Nombre Descripcion
            {
              query = query.Where(dk => dk.dkN.Contains(desk.dkN));
            }
          }

          lstDesk = query.OrderBy(dk => dk.dkN).ToList();
        }
      });

      return lstDesk;
    }
    #endregion

    #region saveDesks
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo Desk
    /// </summary>
    /// <param name="desk">Objeto a guardar en la BD</param>
    /// <param name="blnUpdate">True. Actualiza un registro | False. Agrega un registro</param>
    /// <param name="lstIdsComputers">id's de computers a relacionar</param>
    /// <returns>0. No se puedo guardar | > 0 . Se guardó correctamente | -1. Existe un registro con el mismo ID (Sólo cuando es insertar)</returns>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// [emoguel] modified 09/06/2016---> se volvió async
    /// </history>
    public async static Task<int> SaveDesk(Desk desk, bool blnUpdate, List<string> lstIdsComputers)
    {
      int nRes = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region Actualizar
              if (blnUpdate)//Si es actualizar
              {
                dbContext.Entry(desk).State = System.Data.Entity.EntityState.Modified;
              }
              #endregion
              #region Agregar
              else//Si es guardar
              {
                dbContext.Desks.Add(desk);
              }
              #endregion

              #region Actualizar Computers            

              #region Desaignar
              var lstComputersDes = (from cmp in dbContext.Computers
                                     where !lstIdsComputers.Contains(cmp.cpID)
                                     && cmp.cpdk == desk.dkID
                                     select cmp).ToList();//Buscamos todas las computers asigndas que ya no estan en la lista

              lstComputersDes.ForEach(cmp => cmp.cpdk = null);//Des-relacionamos las computers que no esten en la nueva lista

              #endregion

              #region Asignar
              var lstComputersAsi = (from cmp in dbContext.Computers
                                     where lstIdsComputers.Contains(cmp.cpID)
                                     select cmp).ToList();//Buscamos todas las computers que se van a asignar

              lstComputersAsi.ForEach(cmp => cmp.cpdk = desk.dkID);//Des-relacionamos las computers que no esten en la nueva lista

              #endregion
              #endregion

              int nSave = dbContext.SaveChanges();
              transacction.Commit();
              return nSave;
            }
            catch
            {
              transacction.Rollback();
              return 0;
            }
          }
        }
      });
      return nRes;
    }
    #endregion
  }
}
