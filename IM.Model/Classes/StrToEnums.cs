using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Classes
{
    public class StrToEnums
    {

        public Model.Enums.EnumLoginType TeamType { get; set; }
        
        /// <summary>
        /// Convierte un Enumerado tipo TeamType a String
        /// </summary>
        /// <param name="teamTipe">Enumerado TeamType</param>
        /// <returns>Valor del Enumerado en String</returns>
        /// <history>[ECANUL] 09-03-2016 Created</history>
        public string EnumTeamTypeToString(Model.Enums.TeamType teamTipe)
        {
            string _str;
            if (teamTipe == Model.Enums.TeamType.teamPRs)
                _str = "GS";
            else
                _str = "SA";
            return _str;
        }
    }
}
