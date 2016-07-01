using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Enums;

namespace IM.BusinessRules.BR
{
  public class BRPersonnelAcces
  {
    #region getPersonnelAcces
    /// <summary>
    /// obtiene registros del catalogo PersLSSR
    /// </summary>
    /// <param name="idUser">id del Usuario</param>
    /// <param name="typeAcces">SR. Sales Room | LS. Lead Source | WH. Wharehouse</param>
    /// <returns></returns>
    public static async Task<List<PersonnelAccess>> getPersonnelAcces(string idUser, EnumPlaceType enumPlaceType)
    {
      List<PersonnelAccess> lstPersonnelAcces = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            string placeType = EnumToListHelper.GetEnumDescription(enumPlaceType);
            return dbContext.PersonnelAccessList.Where(pl => pl.plpe == idUser && pl.plLSSR == placeType).ToList();
          }
        });
      return lstPersonnelAcces;
    }
    #endregion
  }
}
