using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;

namespace IM.BusinessRules.BR
{
  public class BRPersonnelDayOff
    {
        /// <summary>
        /// Obtiene el listado de dias libres del personal 
        /// </summary>
        /// <param name="placeID">ID del lugar de trabajo</param>
        /// <param name="teamType"></param>
        /// <returns></returns>
        /// <history>[ECANUL] 09-08-2016 Created</history>
        public static List<PersonnelDayOff> GetPersonnelDaysOff(string placeID, EnumTeamType teamType )
        {
            using (var dbContext = new IMEntities())
            {
                string _teamType = StrToEnums.EnumTeamTypeToString(teamType);
                return dbContext.USP_OR_GetPersonnelDaysOff( _teamType, placeID).ToList();
            }
        }

        /// <summary>
        /// Guarda/Actualiza los Dias libres de un Personnel
        /// </summary>
        /// <param name="action">true para actualizar| false para agregar</param>
        /// <returns>0- Registro no guardado | 1- Registro Guardado 
        /// <history>[ECANUL] 10-03-2016 Created</history>
        public static int SavePersonnelDayOff( Model.DayOff personnelDaysOf)
        {
            int nRes = 0;
            using (var dbContext = new IMEntities())
            {
                ///Uso en try Catch porque al momento de revisar si el usuario existe o no
                ///al coinicidir un ususario existente el manejador me dice que el "Registro esta en uso y no me permite modificarlo"
                try
                {//Intenta Actualizar
                    dbContext.Entry(personnelDaysOf).State = System.Data.Entity.EntityState.Modified;
                    nRes = dbContext.SaveChanges();
                }
                catch
                {///Si no pudo actualizar entonces guarda
                    dbContext.DaysOffs.Add(personnelDaysOf);
                    nRes = dbContext.SaveChanges();
                }
                /*  //Revisa si el Personnel ya esiste en la tabla DaysOff
                  DayOff persoDayOff = dbContext.DaysOffs.Where(c => c.dope == personnelDaysOf.dope).FirstOrDefault();
                  if(persoDayOff != null)
                  {//Si es diferente de nulo modifica
                      //persoDayOff = personnelDaysOf;
                      //dbContext.Entry(personnelDaysOf).State = System.Data.Entity.EntityState.Modified;
                     dbContext.Entry(personnelDaysOf).State = System.Data.Entity.EntityState.Modified;
                      nRes = dbContext.SaveChanges();
                  }
                  else
                  {
                      //Si es nulo Agrega
                      dbContext.DaysOffs.Add(personnelDaysOf);
                      nRes = dbContext.SaveChanges();
                  }
                  */
            }
            return nRes;
        }
    }
}
