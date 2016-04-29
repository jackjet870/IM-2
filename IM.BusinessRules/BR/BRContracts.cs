using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRContracts
  {
    #region GetContracts
    /// <summary>
    /// Devuelve la lista de contract
    /// </summary>
    /// <param name="contract">Entidad con los filtros adicionales</param>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <returns>Lista de contratc</returns>
    /// <history>
    /// [Emoguel] created 01/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// </history>
    public static List<Contract> getContracts(Contract contract=null, int nStatus=-1)
    {
      
      List<Contract> lstContracts = new List<Contract>();

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from c in dbContext.Contracts
                    select c;

        if(nStatus!=-1)//Filtra por status
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(c=>c.cnA==blnStatus);
        }

        if (contract != null)//valida si se tiene objeto
        {
          if (!string.IsNullOrWhiteSpace(contract.cnID))//Filtra por ID
          {
            query=query.Where(c => c.cnID == contract.cnID);
          }

          if (!string.IsNullOrWhiteSpace(contract.cnN))//Filtra por Descripción
          {
            query = query.Where(c => c.cnN.Contains( contract.cnN));
          }
        }

        lstContracts = query.OrderBy(c=>c.cnN).ToList();
      }
      return lstContracts;
    }
    #endregion
  }
}
