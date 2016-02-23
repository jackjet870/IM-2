using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IM.BusinessRules.BR
{
    public class BRCountries
    {
        /// <summary>
        /// Obtiene el catalogo de countries 
        /// </summary>
        /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
        /// <returns>List<Model.GetCountries></returns>
        public static List<Model.GetCountries> GetCountries(int status)
        {
            try
            {
                using (var g = new IM.Model.IMEntities())
                {
                    return g.USP_OR_GetCountries(Convert.ToByte(status)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

    }
}
