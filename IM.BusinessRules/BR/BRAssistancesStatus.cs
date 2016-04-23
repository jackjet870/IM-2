using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRAssistancesStatus
  {
    #region GetAssistanceStatus
    /// <summary>
    /// Devuelve el catalogo de AssistanceSatatus
    /// </summary>
    /// <param name="assistanceStatus">objeto con filtros extra, puede ser null</param>
    /// <param name="nStatus">-1. Sin filtro| 0. Inactivos| 1. Activos</param>
    /// <returns>Lista de AssitanceStatus</returns>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// </history>
    public static List<AssistanceStatus> GetAssitanceStatus(AssistanceStatus assistanceStatus=null, int nStatus=-1)
    {
      
      List<AssistanceStatus> lstAssitanceStatus = new List<AssistanceStatus>();

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from ast in dbContext.AssistancesStatus
                    select ast;
        if(nStatus!=-1)//Filtra por status
        {
          bool bStat = Convert.ToBoolean(nStatus);
          query = query.Where(ast => ast.atA == bStat);
        }

        if (assistanceStatus != null)//validacion del objeto
        {
          if (!string.IsNullOrWhiteSpace(assistanceStatus.atID))//Filtra por ID
          {
            query = query.Where(ast => ast.atID == assistanceStatus.atID);
          }

          if (!string.IsNullOrWhiteSpace(assistanceStatus.atN))//Filtra por Descripción
          {
            query = query.Where(ast => ast.atN.Contains(assistanceStatus.atN));
          }
        }

        lstAssitanceStatus = query.OrderBy(a=>a.atN).ToList();
      }

      return lstAssitanceStatus;
      
    }
    #endregion

    #region SaveAssistanceStatus
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bUpd">true para actualizar | false para guardar un nuevo registro</param>
    /// <param name="assistance"></param>
    /// <returns>0.No se guardó el registro | 1. El registro se guardo correctamente | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    public static int SaveAssitanceStatus(bool bUpd,AssistanceStatus assistance)
    {
      
      int nRes = 0;
        
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        if (bUpd == true)//Actualizar
        {
          dbContext.Entry(assistance).State = System.Data.Entity.EntityState.Modified;
          nRes = dbContext.SaveChanges();
        }
        else//insertar
        {
          AssistanceStatus assistanceVal = dbContext.AssistancesStatus.Where(c => c.atID == assistance.atID).FirstOrDefault();
          if (assistanceVal != null)//Validar si existe un registro con el mismo nombre
          {
            nRes = -1;
          }
          else//guardar el registro
          {
            dbContext.AssistancesStatus.Add(assistance);
            nRes = dbContext.SaveChanges();
          }

        }
      }

      return nRes;
      
    }
    #endregion
  }
}
