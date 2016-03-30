using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.BusinessRules.Entities;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRNotes
  {
    #region GetCountNoteGuest
    /// <summary>
    /// Devuelve el numero total de notas de un Guest
    /// </summary>
    /// <param name="guId">Id a Guest</param>
    /// <returns>bool</returns>
    /// <history>
    /// [jorcanche] created 22/03/2016
    /// </history>
    public static int GetCountNoteGuest(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from gu in dbContext.PRNotes where gu.pngu == guestId select gu).Count();
      }
    }
    #endregion

    #region GetNoteGuest
    /// <summary>
    /// Devuelve una Nota o varias dependiendo de cuantas tanga el Guest
    /// </summary>
    /// <param name="guId">Id a Guest</param>
    /// <returns>Un listado de PRNotes</returns>
    /// <history>
    /// [jorcanche] created 22/03/2016
    /// </history>
    public static List<NoteGuest> GetNoteGuest(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //--select pnDT,pnPR,p.peN,pnText from PRNotes n left join Personnel p on p.peID = n.pnPR where n.pngu = 7747521
        return (from N in dbContext.PRNotes
                   join P in dbContext.Personnels
                   on N.pnPR equals P.peID
                   where  N.pngu == guestId
                   select new NoteGuest
                   {                    
                     Date = N.pnDT,
                     PR = N.pnPR,
                     Name = P.peN,
                     Text = N.pnText
                   }).ToList();   
      }
    }
    #endregion

    #region SaveNoteGuest
    /// <summary>
    /// Guarda la Nota del Guest
    /// </summary>
    /// <param name="PRNote">Tipo nota</param>
    /// <history>
    /// [jorcanche] created 14/03/2016
    /// </history>
    public static int SaveNoteGuest(PRNote PRNote)
    {
      int nRes = 0;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Entry(PRNote).State = System.Data.Entity.EntityState.Added;
        return nRes = dbContext.SaveChanges();

      }
    }
    #endregion
  }
}


