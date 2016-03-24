﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Base.Helpers
{
  public class DateHelper
  {

    #region DateRange for Report
    /// <summary>
    /// Obtiene un rango de fecha formateado para utilizar en el Reporte
    /// </summary>
    /// <param name="dateFrom">Fecha inicio</param>
    /// <param name="dateTo">Fecha Fin</param>
    /// <returns>Rango de fecha formateada (string) </returns>
    /// <history>
    /// [erosado] 15/Mar/2016 Created
    /// </history>
    public static string DateRange(DateTime dateFrom, DateTime dateTo)
    {
      string dateRange = string.Empty;
      //Comparar si es la misma fecha
      if (dateFrom.Date == dateTo.Date)
      {
        dateRange = string.Format("{0:MMMM d, yyyy}", dateFrom);
      }
      // Si son diferentes años
      else if (dateFrom.Year != dateTo.Year)
      {
        dateRange = string.Format("{0:MMMM d, yyyy} - {1:MMMM d, yyyy}", dateFrom, dateTo);
      }
      //Si son diferentes Meses
      else if (dateFrom.Month != dateTo.Month)
      {
        dateRange = string.Format("{0:MMMM d} - {1:MMMM d, yyyy}", dateFrom, dateTo);
      }
      // Mismo mes y mismo año
      else
      {
        dateRange = string.Format("{0:MMMM d} - {1:d, yyyy}", dateFrom, dateTo);
      }

      return dateRange;
    }
    #endregion

    #region DateRange for Suggest FileName Report
    /// <summary>
    /// Obtiene un rango de fecha formateado para utilizar al sugerir Nombre del Reporte
    /// </summary>
    /// <param name="dateFrom">Fecha inicio</param>
    /// <param name="dateTo">Fecha fin</param>
    /// <returns>Rango de fecha formateada (string) </returns>
    /// <history>
    /// [erosado] 24/Mar/2016 Created
    /// </history>
    public static string DateRangeFileName(DateTime dateFrom, DateTime dateTo)
    {
      string dateRange = string.Empty;
      //Comparar si es la misma fecha
      if (dateFrom.Date == dateTo.Date)
      {
        dateRange = string.Format("{0:yyyy,MMMM, dd}", dateFrom);
      }
      // Si son diferentes años
      else if (dateFrom.Year != dateTo.Year)
      {
        //Años completos
        if (dateFrom.Day == 1 && dateFrom.Month == 1 && dateTo.Day==31 && dateTo.Month==12)
        {
          dateRange= string.Format("{0:yyyy} to {1:yyyy}", dateFrom, dateTo);
        }
        //Años incompletos meses completos
        else if (dateFrom.Day == 1 && dateTo.Day ==DateTime.DaysInMonth(dateTo.Year,dateTo.Month))
        {
          dateRange = string.Format("{0:yyyy-MM} to {1:yyyy-MM}", dateFrom, dateTo);
        }
        //Años incompletos, meses incompletos
        else
        {
          dateRange = string.Format("{0:yyyy-MM-dd} to {1:yyyy-MM-dd}", dateFrom, dateTo);
        }
       
      }
      //Si son diferentes Meses
      else if (dateFrom.Month != dateTo.Month)
      {
        //Año completo
        if (dateFrom.Day ==1 && dateFrom.Month==1 && dateTo.Day==31 && dateTo.Month==12)
        {
          dateRange = string.Format("{0:yyyy}", dateFrom);
        }
        //Meses Completo
        else if (dateFrom.Day==1 && dateTo.Day== DateTime.DaysInMonth(dateTo.Year,dateTo.Month))
        {
          dateRange = string.Format("{0:yyyy-MM} to {1:MM}", dateFrom, dateTo);
        }
        //Meses Incompletos
        else
        {
          dateRange = string.Format("{0:yyyy-MM-dd to} - {1:MM-dd}", dateFrom, dateTo);
        }
      }
      // Mismo mes y mismo año
      else
      {
        //Mes completo
        if (dateFrom.Day==1 && dateTo.Day== DateTime.DaysInMonth(dateTo.Year,dateTo.Month))
        {
          dateRange = string.Format("{0:yyyy-MM}", dateFrom);
        }
        //Mes Incompleto
        else
        {
          dateRange = string.Format("{0:yyyy-MM-dd to}{1:dd}", dateFrom,dateTo);
        }
      }
      return dateRange;
    }

    #endregion


  }
}
