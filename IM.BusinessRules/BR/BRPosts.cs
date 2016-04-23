using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    public static List<Post> GetPosts(int nStatus = -1, Post post = null)
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
    }
    #endregion

    #region SavePost
    /// <summary>
    /// Agrega|Actualiza un reistro en el catalogo Posts
    /// </summary>
    /// <param name="post">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <returns>0. No se guardó | 1. se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    public static int SavePost(Post post,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)
        {
          dbContext.Entry(post).State = System.Data.Entity.EntityState.Modified;
        } 
        #endregion
        #region Add
        else
        {
          Post postVal = dbContext.Posts.Where(po => po.poID == post.poID).FirstOrDefault();
          if(postVal!=null)//verficamos si existe un registro con el mismo ID
          {
            return -1;
          }
          else
          {
            dbContext.Posts.Add(post);
          }
        } 
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
