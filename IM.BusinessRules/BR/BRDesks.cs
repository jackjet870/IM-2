using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    public static List<Desk> GetDesks(Desk desk = null, int nStatus = -1)
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

        return query.OrderBy(dk => dk.dkN).ToList();
      }
    }
    #endregion

    #region saveDesks
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo Desk
    /// </summary>
    /// <param name="desk">Objeto a guardar en la BD</param>
    /// <param name="blnUpdate">True. Actualiza un registro | False. Agrega un registro</param>
    /// <param name="lstIdsComputers">id's de computers a relacionar</param>
    /// <returns>0. No se puedo guardar | > 0 . Se guardó correctamente | 2. Existe un registro con el mismo ID (Sólo cuando es insertar)</returns>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    public static int SaveDesk(Desk desk, bool blnUpdate, List<string> lstIdsComputers)
    {
      int nRes = 0;
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
              Desk deskVal = dbContext.Desks.Where(dk => dk.dkID == desk.dkID).FirstOrDefault();
              if (deskVal == null)//No existe registro con el ID
              {
                dbContext.Desks.Add(desk);
              }
              else//Existe un registro con el mismo ID
              {
                nRes = 2;
              }
            }
            #endregion

            #region Actualizar Computers
            if (nRes != 2)
            {

              #region Desaignar
              var lstComputersDes = (from cmp in dbContext.Computers
                                     where !lstIdsComputers.Contains(cmp.cpID)
                                     && cmp.cpdk == desk.dkID
                                     select cmp).ToList();//Buscamos todas las computers asigndas que ya no estan en la lista

              lstComputersDes.ForEach(cmp => cmp.cpdk = null);//Des-relacionamos las computers que no esten en la nueva lista

              #endregion

              #region Desaignar
              var lstComputersAsi = (from cmp in dbContext.Computers
                                     where lstIdsComputers.Contains(cmp.cpID)
                                     select cmp).ToList();//Buscamos todas las computers que se van a asignar

              lstComputersAsi.ForEach(cmp => cmp.cpdk = desk.dkID);//Des-relacionamos las computers que no esten en la nueva lista

              #endregion

            }
            #endregion

            nRes = dbContext.SaveChanges();
            transacction.Commit();
          }
          catch
          {
            transacction.Rollback();
            nRes = 0;
          }
        }
        return nRes;
      }
    }
    #endregion
  }
}
