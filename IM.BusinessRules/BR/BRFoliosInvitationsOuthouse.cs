using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRFoliosInvitationsOuthouse
  {

    #region GetFoliosInvittionsOutside
    /// <summary>
    /// Obtiene registros del catalogo FolioInvitationOutside
    /// </summary>
    /// <param name="folioInvitationOutside">Objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1.Registros activos</param>
    /// <returns>Lista de tipo FolioInvitationOuthouse </returns>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    public static List<FolioInvitationOuthouse> GetFoliosInvittionsOutside(FolioInvitationOuthouse folioInvitationOutside = null, int nStatus = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from fi in dbContext.FoliosInvitationsOuthouse
                    select fi;

        if (nStatus != -1)//FIltro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(fi => fi.fiA == blnEstatus);
        }

        if (folioInvitationOutside != null)//Validamos si tiene objeto
        {
          if (folioInvitationOutside.fiID > 0)//Filtro por ID
          {
            query = query.Where(fi => fi.fiID == folioInvitationOutside.fiID);
          }

          if (!string.IsNullOrWhiteSpace(folioInvitationOutside.fiSerie))//Filtro por serie
          {
            query = query.Where(fi => fi.fiSerie == folioInvitationOutside.fiSerie);
          }

          if (folioInvitationOutside.fiFrom > 0 && folioInvitationOutside.fiTo > 0)//Filtro por rango
          {
            if (folioInvitationOutside.fiFrom == folioInvitationOutside.fiTo)
            {
              query = query.Where(fi => folioInvitationOutside.fiFrom>=fi.fiFrom && folioInvitationOutside.fiTo<= fi.fiTo );              
            }
            else
            {
              query = query.Where(fi => fi.fiFrom >= folioInvitationOutside.fiFrom  && fi.fiTo<= folioInvitationOutside.fiTo);
            }             
          }

        }

        return query.OrderBy(fi => fi.fiSerie).ThenBy(fi=>fi.fiID).ToList();
      }
    }
    #endregion

    #region SaveFolioInvitationOutside
    /// <summary>
    /// Actualiza|Agrega un registro al catalogo FolioInvitationOuthouse
    /// </summary>
    /// <param name="folioInvitationOutside">Entidad a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <returns>0. No se guardó | 1. Se guardó correctamente | -2. Verificar el rango</returns>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    public static int SaveFolioInvittionsOutside(FolioInvitationOuthouse folioInvitationOutside,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        bool blnIsValid = false;
        #region Update
        if (blnUpdate)//Si es actualizar
        {
          blnIsValid = (bool)dbContext.USP_OR_ValidateFolioInvitationsOutside(folioInvitationOutside.fiSerie,folioInvitationOutside.fiFrom,folioInvitationOutside.fiTo,folioInvitationOutside.fiA,1).FirstOrDefault();

          if (blnIsValid)//Validamos el rango
          {
            dbContext.Entry(folioInvitationOutside).State = System.Data.Entity.EntityState.Modified;
          }
          else
          {
            return -2;
          }
        }
        #endregion
        #region Add
        else//SI es insertar
        {
          blnIsValid = (bool)dbContext.USP_OR_ValidateFolioInvitationsOutside(folioInvitationOutside.fiSerie, folioInvitationOutside.fiFrom, folioInvitationOutside.fiTo, folioInvitationOutside.fiA, 0).FirstOrDefault();

          if (blnIsValid)//Validamos el rango
          {
            dbContext.FoliosInvitationsOuthouse.Add(folioInvitationOutside);
          }
          else
          {
            return -2;
          }
        }
        #endregion
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
