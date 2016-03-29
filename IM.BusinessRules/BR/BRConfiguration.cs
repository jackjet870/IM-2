﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRConfiguration
  {
    #region GetCloseDate

    /// <summary>
    /// Obtiene la fecha de cierre
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// </history>
    public static DateTime? GetCloseDate()
    {
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.Configurations.Single().ocInvitationsCloseD;
      }
    }
    #endregion

    #region GetServerDateTime
    /// <summary>
    /// Obtiene la fecha y hora del servidor
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// </history>
    public static DateTime GetServerDateTime()
    {
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var dateTime =  dbContext.Database.SqlQuery<DateTime>("SELECT GETDATE()");
        return dateTime.AsEnumerable().First();
      }
    }
    #endregion
  }
}
