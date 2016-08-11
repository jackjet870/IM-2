using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;

namespace IM.Base.Classes
{
  /// <summary>
  /// Clase para el manejo de invitaciones
  /// </summary>
  /// <history>
  /// [aaloccer] 11/08/2016 Created
  /// </history>
  public class Invitations
  {
    #region ValidateFolio

    /// <summary>
    /// Valida el Folio dependiendo del tipo de programa que sea InHouse o OutHouse
    /// Valida que el folio de reservacion InHouse no haya sido utilizado anteriormente
    /// Valida que el folio de reservacion OutHouse ingresado en el control txtguOutInvitNum sea valido.
    /// </summary>
    /// <param name="guest"></param>
    /// <param name="enumProgram"></param>
    /// <param name="txtFolio"></param>
    /// <param name="btnSearchReservation"></param>
    /// <returns>True is valid | False No</returns>
    /// <history>
    /// [erosado]  08/08/2016  Created.
    /// [aalcocer] 11/08/2016 Modified.  Se modifica los parametros y es movido a la clase Invitation
    /// </history>
    public static bool ValidateFolio(Guest guest, EnumProgram enumProgram, Control txtFolio, FrameworkElement btnSearchReservation)
    {
      bool isValid = true;
      string serie = string.Empty;
      int numero = 0;

      if (enumProgram == EnumProgram.Inhouse)
      {
        //Si el boton de busqueda de Guest esta activo
        if (btnSearchReservation.IsEnabled)
        {
          //Validamos que el folio no haya sido utilizado por otra invitacion
          isValid = BRFolios.ValidateFolioReservation(guest.guls, guest.guHReservID, guest.guID);

          //Si el folio de reservacion NO es valido hay que escoger otro
          if (!isValid)
          {
            UIHelper.ShowMessage("The Reservation Folio was already used for Guest ID");
            btnSearchReservation.Focus();
          }
        }
      }
      else
      {
        //Validamos que se ingrese el folio
        if (!ValidateHelper.ValidateRequired(txtFolio, "Outhouse Invitation Folio"))
        {
          isValid = false;
        }
        // validamos el formato del folio.
        else if (!ValidateFolioOuthouseFormat(guest.guOutInvitNum, ref serie, ref numero))
        {
          UIHelper.ShowMessage("Please specify a Outhouse invitation folio valid in the format 'AA-0000'");
          isValid = false;
        }
        else
        {
          //validamos que el folio exista en el catalogo de folios de invitacion y que no haya sido utilizado por otra invitacion
          isValid = BRFolios.ValidateFolioInvitationOutside(guest.guID, serie, numero);
        }
        if (!isValid)
          txtFolio.Focus();
      }

      return isValid;
    }

    #endregion ValidateFolio

    #region ValidateFolioOuthouseFormat

    /// <summary>
    /// Validamos que el formato del folio sea Letra(s)-Numeros
    /// </summary>
    /// <param name="folio">folio</param>
    /// <param name="serie">Serie</param>
    /// <param name="numero">Numero</param>
    /// <returns>True is valid | False No</returns>
    /// <history>
    /// [erosado] 05/08/2016  Created.
    /// [aalcocer] 11/08/2016 Modified. Se modifica los parametros y es movido a la clase Invitation
    /// </history>
    private static bool ValidateFolioOuthouseFormat(string folio, ref string serie, ref int numero)
    {
      try
      {
        //creamos un arreglo apartir del guión.
        var array = folio.Split('-');
        if (array.Length != 2) //Revisamos que sea un arreglo de 2 exclusivamente
          return false;

        if (!Regex.IsMatch(array[0], @"^[a-zA-Z]+$"))//Revisamos que la serie solo contenga letras
          return false;

        serie = array[0]; //Asignamos la serie
        numero = int.Parse(array[1]); //Asignamos el número, en caso de crear una excepción se retornará false

        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    #endregion ValidateFolioOuthouseFormat
  }
}