
namespace IM.BusinessRules.Login
{
  using IM.BusinessRules.Entities;
  using System;
  using System.Linq;

  public class Login
  {
    /// <summary>
    /// Obtiene el usuario que está intentando autentificarse
    /// </summary>
    /// <param name="user">usuario ID</param>
    /// <param name="password">Passeord del usuario</param>
    /// <returns>UserNormalLogin</returns>
    /// <history>
    /// [lchairez] 24/Feb/2016 Created
    /// </history>
    public UserNormalLogin GetUserLogin(string user, string password)
    {
      try
      {
        using (var dbContext = new IM.Model.IMEntities())
        {
          string query = String.Format("EXEC USP_OR_Login {0}, '{1}', '{2}'", 0, user, String.Empty);

          var personnel = dbContext.Database.SqlQuery<UserNormalLogin>(query).SingleOrDefault();
                  
          return personnel;
        }
      }
      catch (Exception ex)
      {
        return null;
      }
    }
  }
}
