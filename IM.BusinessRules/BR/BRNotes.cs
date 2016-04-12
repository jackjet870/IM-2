using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// Clase que se utiliza para las notas de un Guest
  /// </summary>
  /// <history>
  /// [jorcanche] 11/04/2016
  /// </history>
  public class BRNotes
  {
    #region GetCountNoteGuest
    /// <summary>
    /// Indica si un huesped tiene notas o no
    /// </summary>
    /// <param name="guId">Id a Guest</param>
    /// <returns>bool</returns>
    /// <history>
    /// [jorcanche] created 22/03/2016
    /// </history>
    public static bool  GetCountNoteGuest(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {    
        return ((from gu in dbContext.PRNotes where gu.pngu == guestId select gu).Count() > 0) ? true : false;
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
    /// Guarda la Nota y modifica el Guest
    /// </summary>
    /// <param name="PRNote">Tipo nota</param>
    ///<param name="guest">Tipo de Guest</param>
    /// <history>
    /// [jorcanche] created 14/03/2016
    /// </history>
    public static int SaveNoteGuest(PRNote prNote, Guest guest)
    {
      int respuesta = 0;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
          try
          {
            ///Guardamos la informacion de la nota del guest 
            dbContext.Entry(prNote).State = System.Data.Entity.EntityState.Added;

            //Guardamos la modificación del guest si se debe modificar
            if (guest != null)
            {
              dbContext.Entry(guest).State = System.Data.Entity.EntityState.Modified;
            }

            respuesta = dbContext.SaveChanges();
            transaction.Commit();
            return respuesta;
          }
          catch
          {
            transaction.Rollback();
            return respuesta = 0;
          }
        }
      }
    }
    #endregion
  }
}


