using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Classes;
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
    /// <param name="guestId">Id del guest</param>
    /// <returns>bool</returns>
    /// <history>
    /// [jorcanche] created 22/03/2016
    /// </history>
    public static async Task<bool>  GetCountNoteGuest(int guestId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return (from gu in dbContext.PRNotes where gu.pngu == guestId select gu).Any();
        }
      });
    }
    #endregion

    #region GetNoteGuest

    /// <summary>
    /// Devuelve una Nota o varias dependiendo de cuantas tanga el Guest
    /// </summary>
    /// <param name="guestId">Id a Guest</param>
    /// <returns>Un listado de PRNotes</returns>
    /// <history>
    /// [jorcanche] created 22/03/2016
    /// </history>
    public static async Task<List<NoteGuest>> GetNoteGuest(int guestId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          //--select pnDT,pnPR,p.peN,pnText from PRNotes n left join Personnel p on p.peID = n.pnPR where n.pngu = 7747521
          return (from n in dbContext.PRNotes
            join p in dbContext.Personnels
              on n.pnPR equals p.peID
            where n.pngu == guestId
            select new NoteGuest
            {
              Date = n.pnDT,
              PR = n.pnPR,
              Name = p.peN,
              Text = n.pnText
            }).ToList();
        }
      });
    }

    #endregion

    #region SaveNoteGuest
    /// <summary>
    /// Guarda la Nota y modifica el Guest
    /// </summary>
    /// <param name="prNote">Tipo nota</param>
    ///<param name="guest">Tipo de Guest</param>
    /// <history>
    /// [jorcanche] created 14/03/2016
    /// </history>
    public static async Task<int> SaveNoteGuest(PRNote prNote, Guest guest)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              //Guardamos la informacion de la nota del guest 
              dbContext.Entry(prNote).State = System.Data.Entity.EntityState.Added;

              //Guardamos la modificación del guest si se debe modificar
              if (guest != null)
              {
                dbContext.Entry(guest).State = System.Data.Entity.EntityState.Modified;
              }

              var respuesta = dbContext.SaveChanges();
              transaction.Commit();
              return respuesta;
            }
            catch
            {
              transaction.Rollback();
              return 0;
            }
          }
        }
      });
    }
    #endregion
  }
}


