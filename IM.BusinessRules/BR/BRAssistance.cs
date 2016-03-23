﻿using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRAssistance
  {
    #region GetAssistance
    /// <summary>
    /// Obtiene el de asistencia del personal de un sitio y que existe un registro previo en base de datos
    /// </summary>
    /// <param name="palaceType">LS, SR o WH</param>
    /// <param name="PalaceID"> String con el ID del Sitio </param>
    /// <param name="dateStart">Fecha inicio de la lista de asistencia</param>
    /// <param name="DateEnd">Fecha fin de la lista de asistencia</param>
    /// <returns>Lista de Asistencias entre las fechas seleccionadas</returns>
    /// <history>[ECANUL] 17-03-2016 Created</history>
    public static List<AssistanceData> GetAssistance(EnumPlaceType palaceType, string PalaceID, DateTime dateStart, DateTime DateEnd)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        string strPalaceType = StrToEnums.EnumPalaceTypeToSting(palaceType);
        return dbContext.USP_OR_GetAssistance(strPalaceType, PalaceID, dateStart, DateEnd).ToList();
      }
    }
    #endregion

    #region GetPersonnelAssistance
    /// <summary>
    /// Obtiene la lista de empleados En un periodo de fechas establecido y que NO ha sido registrado en la base de datos
    /// NO REGISTRA SOLO HACE UNA CONSULTA
    /// </summary>
    /// <param name="palaceType">LS, SR o WH</param>
    /// <param name="PalaceID"> String con el ID del Sitio </param>
    /// <param name="dateStart">Fecha inicio de la lista de asistencia</param>
    /// <param name="DateEnd">Fecha fin de la lista de asistencia</param>
    /// <returns>Lista de asistencia enrtre las fechas seleccionadas</returns>
    /// <history>[ECANUL] 19-03-2016 CREATED</history>
    public static List<PersonnelAssistance> GetPersonnelAssistance(EnumPlaceType palaceType, string PalaceID, DateTime dateStart, DateTime DateEnd)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        string strPalaceType = StrToEnums.EnumPalaceTypeToSting(palaceType);
        return dbContext.USP_OR_GetPersonnelAssistance(strPalaceType, PalaceID, dateStart, DateEnd).ToList();
      }
    }
    #endregion

    #region SaveAssistanceList
    /// <summary>
    /// Guarda La asistencia del personal del lugar
    /// </summary>
    /// <param name="assist">Empleado a Guardar</param>
    /// <returns>Entero con la confirmacion de la operacion</returns>
    /// <history>[ECANUL] 22-03-2016 CREATED</history>
    public static int SaveAssistanceList(Assistance assist)
    {
      int res;
      using (var dbcontext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        try
        {
          dbcontext.Entry(assist).State = System.Data.Entity.EntityState.Modified;
          res = dbcontext.SaveChanges();
        }
        catch
        {
          dbcontext.Assistances.Add(assist);
          res = dbcontext.SaveChanges();
        }
      }
      return res;
    } 
    #endregion
  }
}
