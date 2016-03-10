using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;


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
        public static List<PersonnelDayOff> GetPersonnelDaysOff(string placeID, Model.Enums.TeamType teamType )
        {
            using (var dbContext = new IMEntities())
            {
                Model.Classes.StrToEnums strTeamType = new Model.Classes.StrToEnums();
                string _teamType = strTeamType.EnumTeamTypeToString(teamType);
                return dbContext.USP_OR_GetPersonnelDaysOff( _teamType, placeID).ToList();
            }
        }

        /// <summary>
        /// Guarda/Actualiza los Dias libres de un Personnel
        /// </summary>
        /// <param name="action">true para actualizar| false para agregar</param>
        /// <param name="personnelDaysOf">Clase q que contiene los datos a guardar</param>
        /// <returns>0- Registro no guardado | 1- Registro Guardado | 2- Existe un registro con el mismo ID</returns>
        /// <history>[ECANUL] 10-03-2016 Created</history>
        public static int SavePersonnelDayOff(bool action, Model.DayOff personnelDaysOf)
        {
            int nRes = 0;
            using (var dbContext = new IMEntities())
            {
                if(!action)//Inserta
                {
                    DayOff persoDayOff = dbContext.DaysOffs.Where(c => c.dope == personnelDaysOf.dope).FirstOrDefault();
                    if (persoDayOff != null)//Verifica que no esista un registro con mismo dope
                        nRes = 2;
                    else
                    {
                        dbContext.DaysOffs.Add(personnelDaysOf);
                        nRes = dbContext.SaveChanges();
                    }
                }
                else
                {
                    dbContext.Entry(personnelDaysOf).State = System.Data.Entity.EntityState.Modified;
                    nRes = dbContext.SaveChanges();
                }
            }
            return nRes;
        }
    }
}
