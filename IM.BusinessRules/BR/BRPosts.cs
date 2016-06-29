using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRPosts
  {
    #region GetPosts
    /// <summary>
    /// Devuelve registros del catalogo Post
    /// </summary>
    /// <param name="nStatus">-1 Todos | 0. Inactivos | 1.Activos</param>
    /// <param name="post">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Post</returns>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    public async static Task<List<Post>> GetPosts(int nStatus = -1, Post post = null)
    {
      List<Post> lstPost = await Task.Run(() =>
        {

          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
          {
            var query = from po in dbContext.Posts
                        select po;

            if (nStatus != -1)
            {
              bool blnStatus = Convert.ToBoolean(nStatus);
              query = query.Where(po => po.poA == blnStatus);
            }

            if (post != null)//Verificamos si cumple con los filtros
          {
              if (!string.IsNullOrWhiteSpace(post.poID))//Filtro por ID
            {
                query = query.Where(po => po.poID == post.poID);
              }

              if (!string.IsNullOrWhiteSpace(post.poN))//Filtro pos descripción
            {
                query = query.Where(po => po.poN.Contains(post.poN));
              }
            }

            return query.OrderBy(po => po.poN).ToList();
          }
        });

      return lstPost;
    }
    #endregion
  }
}
