using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;


namespace IM.BusinessRules.BR
{
  public class BRDepts
  {
    #region GetDepts
    /// <summary>
    /// Obtiene registro del catalogo Depts
    /// </summary>
    /// <param name="nStatus">-1. todos | 0. Inactivos | 1. Activos</param>
    /// <param name="dept">objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Dept</returns>
    /// <history>
    /// [emoguel] created 03/05/2016
    /// </history>
    public static List<Dept> GetDepts(int nStatus = -1, Dept dept = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from de in dbContext.Depts
                    select de;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(de => de.deA == blnStatus);
        }

        if (dept != null)
        {
          if (!string.IsNullOrWhiteSpace(dept.deID))//filtro por ID
          {
            query = query.Where(de => de.deID == dept.deID);
          }

          if (!string.IsNullOrWhiteSpace(dept.deN))//Filtro por descripción
          {
            query = query.Where(de => de.deN.Contains(dept.deN));
          }
        }

        return query.OrderBy(de => de.deN).ToList();
      }
    }
    #endregion

    #region SaveDept
    /// <summary>
    /// Agrega|Actualiza registros del catalogo depts
    /// </summary>
    /// <param name="dept">Objeto a guardar</param>
    /// <param name="blnUpdate">Truw. Actualiza | False. Agrega</param>
    /// <param name="lstAdd">Personnels a asignar</param>
    /// <param name="lstDel">Personels a desasignar</param>
    /// <returns>0. No se guardó | 1. Se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 04/05/2016
    /// </history>
    public static int SaveDept(Dept dept,bool blnUpdate,List<Personnel> lstAdd, List<Personnel> lstDel)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
          try
          {
            #region Update
            if (blnUpdate)
            {
              dbContext.Entry(dept).State = EntityState.Modified;
            }
            #endregion

            #region Add
            else
            {
              Dept deptVal = dbContext.Depts.Where(de => de.deID == dept.deID).FirstOrDefault();
              if (deptVal != null)
              {
                return -1;
              }
              else
              {
                dbContext.Depts.Add(dept);
              }
            }
            #endregion

            #region add personnel
            if (lstAdd.Count > 0)
            {
              dbContext.Personnels.AsEnumerable().Where(pe => lstAdd.Any(pee => pee.peID == pe.peID)).ToList().ForEach(pe => pe.pede = dept.deID);
            }
            #endregion

            #region del Personnel
            if (lstDel.Count > 0)
            {
              dbContext.Personnels.AsEnumerable().Where(pe => lstDel.Any(pee => pee.peID == pe.peID)).ToList().ForEach(pe => pe.pede = null);
            }
            #endregion

            int nRes = dbContext.SaveChanges();
            transaction.Commit();
            return nRes;
          }
          catch
          {
            transaction.Rollback();
            return 0;
          }
        }
      }
    }
    #endregion
  }
}
