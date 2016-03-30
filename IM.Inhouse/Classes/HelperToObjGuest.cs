﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.BusinessRules.BR;
using System.Drawing;

namespace IM.Inhouse.Classes
{
  class HelperToObjGuest
  {

    #region Status
    /// <summary>
    /// Determina el estatus que tendra el Guest
    /// </summary>
    /// <param name="guCheckIn"></param>
    /// <param name="guCheckOutD"></param>
    /// <param name="guAvail"></param>
    /// <param name="guInvit"></param>
    /// <param name="guBookCanc"></param>
    /// <param name="guBookD"></param>
    /// <param name="guShow"></param>
    /// <param name="guInfo"></param>
    /// <history>[jorcanche] 29/03/2016</history>
    public static int Status(bool guCheckIn, DateTime guCheckOutD, bool guAvail, bool guInvit, bool guBookCanc, DateTime? guBookD, bool guShow, bool guInfo)
    {
      //Sin Check-In
      if (!guCheckIn)
      {
        return 8;
      }
      else
      {
        //Check Out
        if (guCheckOutD < BRHelpers.GetServerDate().Date)
        {
          return 7;
        }
        else
        {
          //Invitacion
          if (!guAvail)
          {
            return 6;
          }
          else
          {
            //Inv. Cancelada
            if (guInvit && guBookCanc)
            {
              return 5;
            }
            else
            {
              //Inv. No Show
              if (guInvit && guBookD < BRHelpers.GetServerDate().Date && !guShow)
              {
                return 4;
              }
              else
              {
                //Inv. Show
                if (guInvit && guShow)
                {
                  return 3;
                }
                else
                {
                  //Invitado en stand-by
                  if (guInvit)
                  {
                    return 2;
                  }
                  else
                  {
                    //Info
                    if (guInfo)
                    {
                      return 1;
                    }
                    else
                    {
                      //Disponible
                      if (guAvail)
                      {
                        return 0;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      //Sin Check-In
      return 8;
    }
    #endregion

    #region ColorStatus
    /// <summary>
    /// Retorna el color segun el estatus en el que se encuentra el Guest
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    /// 
    public static string ColorStatus(int status)
    {
      Color _color = new Color();
      switch (status)
      {
        case 0:
          _color = Color.FromArgb(0, 255, 128);// Disponible sin Info;
          break;

        case 1:
          _color = Color.FromArgb(0, 200, 200); // Con Info, sin Invitacion
          break;

        case 2:
          _color = Color.FromArgb(255, 255, 128); // Con Invitacion, stand-by
          break;

        case 3:
          _color = Color.FromArgb(255, 184, 0); // Con Invitacion, show
          break;

        case 4:
          _color = Color.FromArgb(255, 128, 0); // Con Invitacion, No show
          break;

        case 5:
          _color = Color.FromArgb(220, 0, 0); // Con Invitacion, Cancelada
          break;

        case 6:
          _color = Color.FromArgb(160, 0, 255); // No Disponible
          break;

        case 7:
          _color = Color.FromArgb(0, 0, 160); // Check-Out
          break;

        case 8:
          _color = Color.FromArgb(128, 128, 128); // Sin Check-In
          break;
      }
      return "#" + _color.R.ToString("X2") + _color.G.ToString("X2") + _color.B.ToString("X2");
    } 
    #endregion
  }
}

