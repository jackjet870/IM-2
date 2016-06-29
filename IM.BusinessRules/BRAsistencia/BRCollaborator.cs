using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BRAsistencia
{
  public class BRCollaborator
  {
    #region GetCollaborator
    /// <summary>
    /// Devuelve la lista de collaborators
    /// </summary>
    /// <param name="collaborator">objeto con los parametros para el SP</param>
    /// <returns>Lista de tipo Collaborator</returns>
    /// <history>
    /// [emoguel] created 21/06/2016
    /// </history>
    public static async Task<List<Collaborator>> GetCollaborators(Collaborator collaborator)
    {
      List<Collaborator> lstCollaborator = await Task.Run(() =>
      {
        using (var dbContext = new AsistenciaEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_ObtenerColaboradoresPorParametro(
            (!string.IsNullOrWhiteSpace(collaborator.EmpID))? collaborator.EmpID :"ALL", 
            (!string.IsNullOrWhiteSpace(collaborator.NombreCompleto))? collaborator.NombreCompleto:"ALL",
            (!string.IsNullOrWhiteSpace(collaborator.Puesto))? collaborator.Puesto:"ALL", 
            (!string.IsNullOrWhiteSpace(collaborator.Locacion))? collaborator.Locacion:"ALL", 
            (!string.IsNullOrWhiteSpace(collaborator.Hotel))? collaborator.Locacion:"ALL").ToList();
        }
      });

      return lstCollaborator;
    } 
    #endregion
  }
}
