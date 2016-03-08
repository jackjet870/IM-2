using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRAssistancesStatus
  {
    #region GetAssistanceStatus
    /// <summary>
    /// Devuelve el catalogo de AssistanceSatatus
    /// </summary>
    /// <param name="nStatus">-1. Sin filtro| 0. Inactivos| 1. Activos</param>
    /// <returns>Lista de AssitanceStatus</returns>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    public static List<AssistanceStatus> GetAssitanceStatus(AssistanceStatus assistanceStatus, int nStatus=-1)
    {
      
      List<AssistanceStatus> lstAssitanceStatus = new List<AssistanceStatus>();

      using (var dbContext = new IMEntities())
      {
        var query = from ast in dbContext.AssistancesStatus
                    select ast;
        if(nStatus!=-1)//Filtra por status
        {
          bool bStat = Convert.ToBoolean(nStatus);
          query = query.Where(ast => ast.atA == bStat);
        }

        if(!string.IsNullOrWhiteSpace(assistanceStatus.atID))//Filtra por ID
        {
          query = query.Where(ast=>ast.atID==assistanceStatus.atID);
        }

        if(!string.IsNullOrWhiteSpace(assistanceStatus.atN))//Filtra por Descripción
        {
          query = query.Where(ast=>ast.atN==assistanceStatus.atN);
        }
        lstAssitanceStatus = query.ToList();
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
    /// <returns>0.No se guardó el registro | 1. El registro se guardo correctamente | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    public static int SaveAssitanceStatus(bool bUpd,AssistanceStatus assistance)
    {
      
      int nRes = 0;
        
      using (var dbContext = new IMEntities())
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
            nRes = 2;
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
