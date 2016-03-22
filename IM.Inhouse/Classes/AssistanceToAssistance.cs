using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.Inhouse.Classes
{
  class AssistanceToAssistance
  {
    /// <summary>
    /// Convierte un AssistanceData a Assistance;
    /// </summary>
    /// <param name="assistData">Item AssistanceData</param>
    /// <returns>Clase Assistance con valores de AssistanceData</returns>
    /// <history>[ECANUL] 22-03-2016 CREATED</history>
    public static Assistance ConvertAssistanceDataToAssistance (AssistanceData assistData)
    {
      Assistance assistance = new Assistance();
      assistance.aspe = assistData.aspe;
      assistance.asMonday = assistData.asMonday;
      assistance.asTuesday = assistData.asTuesday;
      assistance.asWednesday = assistData.asWednesday;
      assistance.asThursday = assistData.asThursday;
      assistance.asFriday = assistData.asFriday;
      assistance.asSaturday = assistData.asSaturday;
      assistance.asSunday = assistData.asSunday;
      assistance.asPlaceID = assistData.asPlaceID;
      assistance.asPlaceType = assistData.asPlaceType;
      assistance.asStartD = Convert.ToDateTime(assistData.asStartD);
      assistance.asEndD = Convert.ToDateTime(assistData.asEndD);
      assistance.asNum = Convert.ToByte(assistData.asNum);
      //assistance.peN = assistData.peN; //Clase Assistance no tiene peN
      return assistance;
    }

    /// <summary>
    /// Convierte un tipo PersonnelAssistance(proviniente del USP_OR_GetPersonnelAssistance a AssistanceData
    /// </summary>
    /// <param name="personAssist">Item con la clase PersonnelAssistance</param>
    /// <returns>Clase AssistanceData con Datos</returns>
    /// <history>[ECANUL] 22-03-2016 CREATED</history>
    public static AssistanceData ConvertPersonnelAssistanceToAssistanceData(PersonnelAssistance personAssist)
    {
      AssistanceData assistance = new AssistanceData();
      assistance.aspe = personAssist.aspe;
      assistance.asMonday = personAssist.asMonday;
      assistance.asTuesday = personAssist.asTuesday;
      assistance.asWednesday = personAssist.asWednesday;
      assistance.asThursday = personAssist.asThursday;
      assistance.asFriday = personAssist.asFriday;
      assistance.asSaturday = personAssist.asSaturday;
      assistance.asSunday = personAssist.asSunday;
      assistance.asPlaceID = personAssist.asPlaceID;
      assistance.asPlaceType = personAssist.asPlaceType;
      assistance.asStartD = Convert.ToDateTime(personAssist.asStartD);
      assistance.asEndD = Convert.ToDateTime(personAssist.asEndD);
      assistance.asNum = Convert.ToByte(personAssist.asNum);
      assistance.peN = personAssist.peN; //Clase Assistance no tiene peN
      return assistance;
    }
  }
}
