using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IM.Model.Classes;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace IM.Base.Classes
{
  public class InvitationValidationRules
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
    /// [aalcocer] 11/08/2016 Modified.  Se hizo generico
    /// [erosado] 31/08/2016  Modfied. Ahora el Store te devuelve el mensaje de error.
    /// </history>
    public static bool ValidateFolio(Guest guest, EnumProgram enumProgram, Control txtFolio, FrameworkElement btnSearchReservation, TabItem tabItem, Guest cloneGuest = null)
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
            tabItem.IsSelected = true;
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
          //Si el folio de invitacion es diferente (Nuevo, o Modificado) entonces se valida, de lo contrario no se valida.
          if (guest.guOutInvitNum != cloneGuest?.guOutInvitNum)
          {
            //validamos que el folio exista en el catalogo de folios de invitacion y que no haya sido utilizado por otra invitacion
            var validationResult = BRFolios.ValidateFolioInvitationOutside(guest.guID, serie, numero);

            //Si trae un mensaje quiere decir que falló
            if (!string.IsNullOrWhiteSpace(validationResult))
            {
              UIHelper.ShowMessage(validationResult);
              isValid = false;
            }
          }

        }
        if (!isValid)
        {
          tabItem.IsSelected = true;
          txtFolio.Focus();
        }

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

    #region Grid Invitation Gift



    #endregion Grid Invitation Gift

    #region Save Validation

    #region ValidateGeneral

    /// <summary>
    /// Validamos informacion general del formulario
    /// </summary>
    /// <param name="form">Ref frmInvitation</param>
    /// <param name="guest">GuestObj</param>
    /// <param name="program">Program</param>
    /// <returns>True is valid | False No</returns>
    public static bool ValidateGeneral(frmInvitation form, GuestInvitation dbContext)
    {
      string _res = string.Empty;
      //Validamos el folio de reservacion (InHouse & OutHouse)
      if (!ValidateFolio(dbContext.Guest, dbContext.Program, form.txtguOutInvitNum, form.brdSearchButton, form.tabGeneral, dbContext.CloneGuest)) return false;
      //Si la fecha de un booking no este en fecha cerrada
      if (!ValidateBookingCloseDate(dbContext)) return false;
      //Validamos que el Numero de habitaciones este en el rango permitido
      if (!ValidateHelper.ValidateNumber(dbContext.Guest.guRoomsQty, 1, 5, "Rooms Quantity")) return false;
      //Validamos que se ingresen datos en los campos obligatorios
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(form, "Invitation", showMessage: true))) return false;
      //Validamos el Booking y el Reschedule
      if (!ValidateBookReschedule(ref form, dbContext)) return false;
      //Validamos Pax
      if (!ValidateHelper.ValidateNumber(dbContext.Guest.guPax, 0.1M, 1000, "Pax number")) return false;
      //Validamos el codigo contable
      //TODO: esta validacion no se hizo por que aun esta en desarrollo
      return true;
    }

    #endregion ValidateGeneral

    #region ValidateInformationGrids
    /// <summary>
    /// Valida la informacion de los grids antes de guardad
    /// </summary>
    /// <param name="form">frmInvitation</param>
    /// <param name="dbContext">GuestInvitation</param>
    /// <returns></returns>
    public static bool ValidateInformationGrids(frmInvitation form, GuestInvitation dbContext)
    {
      bool _isValid = true;

      if (dbContext.Program != EnumProgram.Outhouse)
      {
        //Validamos el Status del invitado
        if (string.IsNullOrWhiteSpace(dbContext.Guest.guGStatus))
        {
          UIHelper.ShowMessage("Specify the Guest Status");
          form.cmbGuestStatus.Focus();
          _isValid = false;
        }
      }

      //Validamos los regalos de la invitacion
      if (!InvitationGiftValidation(ref form, dbContext)) _isValid = false;
      else if (!ValidateBookingDeposits(form)) _isValid = false;
      //Validamos las Tarjetas de credito
      else if (!ValidateGuestCreditCard(dbContext.GuestCreditCardList.ToList())) _isValid = false;
      //Burned Hotel
      else if (string.IsNullOrEmpty(dbContext.Guest.guHotelB) && dbContext.Guest.guDepositTwisted > 0)
      {
        UIHelper.ShowMessage("Specify the Resorts");
        form.cmbResorts.Focus();
        _isValid = false;
      }

      return _isValid;
    }
    #endregion

    #region ValidateCloseDate

    /// <summary>
    //Valida que la fecha de Arrival se encuentre y que la fecha de Booking NO se encuentre en fecha cerrada
    /// </summary>
    /// <param name="dbContext">CommonCatObject</param>
    /// <returns>True is Valid | False No</returns>
    private static bool ValidateBookingCloseDate(GuestInvitation dbContext)
    {
      bool _isValid = true;
      //Si la fecha de Booking esta en fecha Cerrada
      if (dbContext.Guest.guBookD != null && dbContext.Guest.guBookD < dbContext.CloseDate)
      {
        UIHelper.ShowMessage("It's not allowed to make Invitations for a closed date.");
        _isValid = false;
      }
      return _isValid;
    }

    #endregion ValidateCloseDate

    #region ValidateBookReschedule

    /// <summary>
    /// Valida el booking y el reschedule
    /// </summary>
    /// <param name="form">ref frmInvitation</param>
    /// <param name="dbContext">CommonCatObject</param>
    /// <returns>True is valid | False No</returns>
    /// <history>
    /// [erosado] 06/08/2016  Created.
    /// </history>
    private static bool ValidateBookReschedule(ref frmInvitation form, GuestInvitation dbContext)
    {
      //Validamos BookingDateTime Si no pasa la validacion retorna false
      if (!ValidateBookingDateTime(ref form, dbContext)) return false;
      //Validamos RescheduleDateTime Si no pasa la validacion retorna false
      if (!ValidateRescheduleDateTime(ref form, dbContext)) return false;

      return true;
    }

    #endregion ValidateBookReschedule

    #region ValidateRescheduleDateTime

    /// <summary>
    /// Valida la fecha y hora de reschedule
    /// </summary>
    /// <param name="form">ref frmInvitation</param>
    /// <param name="dbContext">CommonCatObject</param>
    /// <returns>True is valid | False No</returns>
    /// <history>
    /// [erosado] 08/08/2016  Created.
    /// </history>
    private static bool ValidateRescheduleDateTime(ref frmInvitation form, GuestInvitation dbContext)
    {
      bool _isValid = true;
      //Si se permite reschedule
      if (dbContext.AllowReschedule)
      {
        //Si se puede modificar la fecha de Reschedule
        if (form.dtpRescheduleDate.IsEnabled)
        {
          //Validamos que tenga seleccionada una fecha y que no sea el valor default
          if (dbContext.Guest.guReschD == null || dbContext.Guest.guReschD == DateTime.MinValue)
          {
            UIHelper.ShowMessage("Specify a valid reschedule date.");
            form.dtpRescheduleDate.Focus();
            _isValid = false;
          }
          //validamos que la fecha de reschedule no sea despues de la fecha de salida
          else if (dbContext.Guest.guReschD > dbContext.Guest.guCheckOutD)
          {
            UIHelper.ShowMessage("Reschedule date can not be after check out date.");
            form.dtpRescheduleDate.Focus();
            _isValid = false;
          }
          //validamos que la fecha de reschedule no sea antes de la fecha de llegada
          else if (dbContext.Guest.guReschD < dbContext.Guest.guCheckInD)
          {
            UIHelper.ShowMessage("Reschedule date can not be before Check-in date.");
            form.dtpRescheduleDate.Focus();
            _isValid = false;
          }
          else if (dbContext.Guest.guReschD < dbContext.Guest.guBookD)
          {
            UIHelper.ShowMessage("Reschedule date can not be before booking date.");
            form.dtpRescheduleDate.Focus();
            _isValid = false;
          }
          else if (dbContext.Guest.guReschD < BRHelpers.GetServerDate())
          {
            UIHelper.ShowMessage("Reschedule date can not be before today.");
            form.dtpRescheduleDate.Focus();
            _isValid = false;
          }
          else if (!ValidateHelper.ValidateRequired(form.cmbReschT, "Reschedule Time"))
          {
            _isValid = false;
          }
          //Si la fecha del Reschedule es la misma que BookinD
          else if (dbContext.Guest.guReschD == dbContext.Guest.guBookD)
          {
            //validamos que la hora de reschedule no sea la misma o antes de la hora de booking
            if (dbContext.Guest.guReschT <= dbContext.Guest.guBookT)
            {
              UIHelper.ShowMessage("Reschedule time can not be before booking time.");
              form.cmbReschT.Focus();
              _isValid = false;
            }
          }
        }
      }
      return _isValid;
    }

    #endregion ValidateRescheduleDateTime

    #region ValidateBookingDateTime

    /// <summary>
    /// Valida la fecha y hora de booking
    /// </summary>
    /// <param name="form">ref frmInvitation</param>
    /// <param name="dbContext">CommonCatObject</param>
    /// <returns>True is Valid | False No</returns>
    /// <history>
    /// [erosado] 08/08/2016  Created.
    /// </history>
    private static bool ValidateBookingDateTime(ref frmInvitation form, GuestInvitation dbContext)
    {
      bool _isValid = true;
      EnumPermission _permission = EnumPermission.PRInvitations;

      //Si el control dtpBookDate esta disponible podemos editar la fecha si no retornamos True
      if (form.dtpBookDate.IsEnabled)
      {
        //Si tiene un valor y No es el valor default
        if (dbContext.Guest.guBookD != null && dbContext.Guest.guBookD != DateTime.MinValue)
        {
          //Si el tipo de invitacion viene de host se cambia el permiso a HostInvitation
          if (form._module == EnumModule.Host)
          {
            _permission = EnumPermission.HostInvitations;
          }

          #region Validaciones Comunes CON y SIN permisos

          //validamos que la fecha de booking no sea despues de la fecha de salida
          if (dbContext.Guest.guBookD > dbContext.Guest.guCheckOutD)
          {
            UIHelper.ShowMessage("Booking date can not be after Check Out date.");
            form.dtpBookDate.Focus();
            _isValid = false;
          }
          //validamos que la fecha de booking no sea antes de la fecha de llegada
          else if (dbContext.Guest.guBookD < dbContext.Guest.guCheckInD)
          {
            UIHelper.ShowMessage("Booking date can not be before Check In date.");
            form.dtpBookDate.Focus();
            _isValid = false;
          }

          #endregion Validaciones Comunes CON y SIN permisos

          /*Si si nivel de permiso es SuperSpecial
           * y su permiso es de PRInvitation o HostInvitation dependiendo del tipo
           * y La invitacion es Nueva (InvitationMode.modAdd)
           * */
          if (form._user.HasPermission(_permission, EnumPermisionLevel.SuperSpecial) && form._invitationType != EnumInvitationType.existing)
          {
            //Valida que el invitado no tenga un mes de haber salido
            if (BRHelpers.GetServerDate() >= dbContext.Guest.guCheckOutD.AddDays(30))
            {
              UIHelper.ShowMessage("Guest already made check out.");
              form.dtpOtherInfoDepartureD.Focus();
              _isValid = false;
            }
          }
          //Si NO tiene permiso
          else
          {
            //Validamos que la fecha de booking no sea antes de hoy
            if (dbContext.Guest.guBookD < BRHelpers.GetServerDate())
            {
              UIHelper.ShowMessage("Booking date can not be before today.");
              form.dtpOtherInfoDepartureD.Focus();
              _isValid = false;
            }
          }
        }
        else
        {
          UIHelper.ShowMessage("Specify a valid booking date.");
          form.dtpOtherInfoDepartureD.Focus();
          _isValid = false;
        }

        if (_isValid)
        {
          if (!ValidateHelper.ValidateRequired(form.cmbBookT, "Booking Time")) { _isValid = false; }
        }
      }

      if (!_isValid)
      {
        form.tabGeneral.IsSelected = true;
      }
      return _isValid;
    }

    #endregion ValidateBookingDateTime

    #region ValidateFolioOuthouseFormat

    /// <summary>
    /// Validamos que el formato del folio sea Letra(s)-Numeros
    /// </summary>
    /// <param name="form">frmInvitation</param>
    /// <param name="serie">Serie</param>
    /// <param name="numero">Numero</param>
    /// <returns>True is valid | False No</returns>
    /// <history>
    /// [erosado] 05/08/2016  Created.
    /// </history>
    private static bool ValidateFolioOuthouseFormat(ref frmInvitation form, ref string serie, ref int numero)
    {
      try
      {
        bool _isValid = true;
        //creamos un arreglo apartir del guión.
        var array = form.txtguOutInvitNum.Text.Split('-');
        if (array.Length != 2) //Revisamos que sea un arreglo de 2 exclusivamente
          return false;

        if (!Regex.IsMatch(array[0], @"^[a-zA-Z]+$"))//Revisamos que la serie solo contenga letras
          return false;

        serie = array[0]; //Asignamos la serie
        numero = int.Parse(array[1]); //Asignamos el número, en caso de crear una excepción se retornará false

        return _isValid;
      }
      catch (Exception)
      {
        return false;
      }
    }

    #endregion ValidateFolioOuthouseFormat

    #region DataGrid Invitation Gifts
    #region StartEdit

    /// <summary>
    /// Inicia las validaciones de los campos del Grid
    /// </summary>
    /// <param name="invitationGift">Objeto enlazado al Row que se esta modificando</param>
    /// <param name="currentCellInfo">Celda que se esta editando</param>
    /// <param name="dtg">El datagrid que se esta modificando</param>
    /// <param name="_hasError">True tiene error | False No tiene</param>
    public static void StartEdit(ref InvitationGift invitationGift, ref DataGridCellInfo currentCellInfo, DataGrid dtg, ref bool _hasError)
    {
      //Index del Row en edicion
      int rowIndex = dtg.SelectedIndex != -1 ? dtg.SelectedIndex : 0;

      if (invitationGift.iggr == null || invitationGift.iggr == 0)
      {
        switch (currentCellInfo.Column.SortMemberPath)
        {
          case "iggi":
            //Si no ha ingresado una cantidad
            if (invitationGift.igQty == 0)
            {
              UIHelper.ShowMessage("Enter the quantity first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
              _hasError = true;
              //Asignamos la cantidad minima
              invitationGift.igQty = 1;
              //Mandamos el foco a la columna igQty index[0].
              GridHelper.SelectRow(dtg, rowIndex, 0, true);
            }
            break;

          case "igAdults":
          case "igMinors":
          case "igExtraAdults":
            //Si no se ha seleccionado un regalo
            if (string.IsNullOrEmpty(invitationGift.iggi))
            {
              UIHelper.ShowMessage("Enter the gift first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
              _hasError = true;
              //Mandamos el foco a la columna iggi index[1].
              GridHelper.SelectRow(dtg, rowIndex, 1, true);
            }
            else
            {
              //Obtenemos el Gift Completo
              var gift = BRGifts.GetGiftId(invitationGift.iggi);
              // se permite modificar si el regalo maneja Pax
              currentCellInfo.Column.IsReadOnly = !gift.giWPax;
            }
            break;

          default:
            break;
        }
      }
      //Si el regalo ya fue entregado (No necesita validar las columnas)
      else
      {
        _hasError = false;
      }
    }

    #endregion StartEdit

    #region ValidateEdit

    /// <summary>
    /// Valida la informacion del Grid
    /// </summary>
    /// <param name="invitationGift">Objeto enlazada a la fila que se esta editando</param>
    /// <param name="currentCellInfo">Celda que se esta editando</param>
    /// <returns>
    /// True  si tiene error| False si NO tiene
    /// </returns>
    /// <history>
    /// [erosado] 27/07/2016  Created.
    /// </history>
    public static bool ValidateEdit(ref InvitationGift invitationGift, ref DataGridCellInfo currentCellInfo)
    {
      bool _hasError = false;
      switch (currentCellInfo.Column.SortMemberPath)
      {
        case "igQty":
          //Si tiene seleccionado un gift
          if (invitationGift.iggi != null)
          {
            //Buscamos el Gift
            var gift = BRGifts.GetGiftId(invitationGift.iggi);
            //Validacion cantidad máxima del regalo
            Gifts.ValidateMaxQuantityOnEntryQuantity(ref invitationGift, gift, false, 1, ref _hasError, nameof(invitationGift.igQty));
          }//Si no ha seleccionado el Gift
          else
          {
            //Validacion cantidad máxima del regalo
            Gifts.ValidateMaxQuantityOnEntryQuantity(ref invitationGift, null, false, 1, ref _hasError, nameof(invitationGift.igQty));
          }
          return _hasError;

        case "igAdults":
          //Validacion Numero de Adultos
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Adults, invitationGift, ref _hasError, nameof(invitationGift.igAdults), nameof(invitationGift.igMinors));
          return _hasError;

        case "igMinors":
          //Validacion Numero de menores
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Minors, invitationGift, ref _hasError, nameof(invitationGift.igAdults), nameof(invitationGift.igMinors));
          return _hasError;

        default:
          break;
      }
      return _hasError;
    }

    #endregion ValidateEdit

    #region AfterEdit

    /// <summary>
    /// Valida la informacion de la celda.
    /// </summary>
    /// <param name="dtg">DataGrid que estamos validando</param>
    /// <param name="invitationGift">Objeto enlazado a la fila que estamos validando</param>
    /// <param name="currentCell">Celda que estamos validando</param>
    /// <param name="txtTotalCost">Caja de texto donde se pondrá el resultado del calculo de Costos</param>
    /// <param name="txtTotalPrice">Caja de texto donde se pondrá el resultado del calculo de Precios</param>
    /// <param name="txtgrMaxAuthGifts">Caja de texto donde se pondrá el resultado del calculo de costos</param>
    public static void AfterEdit(int guestId, DataGrid dtg, ref InvitationGift invitationGift, DataGridCellInfo currentCell,
      ref TextBox txtTotalCost, ref TextBox txtTotalPrice, ref TextBox txtgrMaxAuthGifts, GuestStatusType guestStatusType, EnumProgram program)
    {
      //Obtenemos el Gift
      var gift = BRGifts.GetGiftId(invitationGift.iggi);

      switch (currentCell.Column.SortMemberPath)
      {
        case nameof(invitationGift.igQty):
          //Si son diferentes de NULL
          if (invitationGift?.igQty >= 0 && invitationGift?.igAdults >= 0 && invitationGift?.igMinors >= 0 &&
             invitationGift?.igExtraAdults >= 0)
          {
            //Calcula costos y precios
            Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult));
          }
          break;

        case nameof(invitationGift.iggi):
          //Si se selecciono el Gift
          if (!string.IsNullOrEmpty(invitationGift.iggi))
          {
            // Cargar a Marketing
            invitationGift.igct = "MARKETING";

            //Si el regalo no maneja Pax
            if (!gift.giWPax)
            {
              //Agrega valores default
              invitationGift.igAdults = 1;
              invitationGift.igMinors = 0;
              invitationGift.igExtraAdults = 0;
            }
            //Establecemos valores default de adultos y menores
            invitationGift.igAdults = 1;
            invitationGift.igMinors = 0;
            invitationGift.igExtraAdults = 0;
            invitationGift.igPriceA = 0;
            invitationGift.igPriceM = 0;
            invitationGift.igPriceAdult = 0;
            invitationGift.igPriceMinor = 0;
            invitationGift.igPriceExtraAdult = 0;

            //Validamos la cantidad maxima que podemos regalar de el regalo en específico.
            Gifts.ValidateMaxQuantity(gift, invitationGift.igQty, false, ref invitationGift, nameof(invitationGift.igQty));

            //Si es InHouse y ya selecciono guestStatusType
            if (program == EnumProgram.Inhouse && guestStatusType != null && guestId != 0)
            {
              //Obtenemos el GuestStatusInfo
              var guestStatusInfo = BRGuestStatus.GetGuestStatusInfo(guestId, 0);

              if (guestStatusInfo != null)
              {
                //Valida que no den mas de los tours permitidos && Validamos que no den mas de los descuentos de Tour
                Gifts.ValidateGiftsGuestStatus(dtg, guestStatusInfo, nameof(invitationGift.igQty), nameof(invitationGift.iggi));
              }
            }

            //Calculamos los costos y los precios
            Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
                nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
                nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
                nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
                nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false);
          }
          break;

        case nameof(invitationGift.igAdults):
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false, EnumPriceType.Adults);
          break;

        case nameof(invitationGift.igMinors):
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false, EnumPriceType.Minors);
          break;

        case nameof(invitationGift.igExtraAdults):
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false, EnumPriceType.ExtraAdults);
          break;

        default:
          break;
      }
      //Calculamos el monto total de los regalos
      Gifts.CalculateTotalGifts(dtg, EnumGiftsType.InvitsGifts, nameof(invitationGift.igQty), nameof(invitationGift.iggi),
        nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceAdult),
        nameof(invitationGift.igPriceA), nameof(invitationGift.igPriceExtraAdult), txtTotalCost, txtTotalPrice);

      //Refresca los datos en las celdas del Grid
      GridHelper.UpdateCellsFromARow(dtg);
    }

    #endregion AfterEdit

    #region InvitationGiftValidation

    /// <summary>
    /// Valida la informacion de los regalos de invitacion
    /// </summary>
    /// <param name="form">ref frmInvitation</param>
    /// <param name="dbContext">CommonCatObject</param>
    /// <returns>True is valid | False NO</returns>
    /// <history>
    /// [erosado] 09/08/2016  Created.
    /// </history>
    public static bool InvitationGiftValidation(ref frmInvitation form, GuestInvitation dbContext)
    {
      bool isValid = true;
      int columnIndex = 0;
      //Validar que ya se haya salido del modo edición del Grid de Gift
      DataGridRow row = GridHelper.GetRowEditing(form.dtgGifts);
      if (row != null)
      {
        //Revisamos la fila que esta en edicion buscando algun valor incorrecto.
        PropertyInfo[] properties = (row.Item as InvitationGift).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo pi in properties)
        {
          //Si encontro algun valor incorrecto buscamos en que columna fue y le asignamos al columnIndex.
          if (!ValidateEditBeforeSave(pi.Name,row.Item as InvitationGift,ref columnIndex))
          {
            var column = form.dtgGifts.Columns.Where(cl => cl.SortMemberPath == pi.Name).FirstOrDefault();
            columnIndex = (column != null) ? column.DisplayIndex : 0;
            isValid = false;
          }
        }
        //Si Los valores que estan en la fila en edicion son correctos entonces hacemos commit
        if (isValid)
        {
          form.dtgGifts.RowEditEnding -= form.dtgGifts_RowEditEnding;
          form.dtgGifts.CommitEdit();
          form.dtgGifts.RowEditEnding += form.dtgGifts_RowEditEnding;
        }
        //Si hay algun valor incorrecto
        else
        {
          isValid = false;
          GridHelper.SelectRow(form.dtgGifts, row.GetIndex(), columnIndex, true);
        }

      }
      //Validamos gift duplicados
      if (isValid)
      {
        if (ObjectHelper.AreAnyDuplicates(dbContext.InvitationGiftList.ToList(), nameof(InvitationGift.iggi)))
        {
          UIHelper.ShowMessage("Repeated gift");
          form.dtgGifts.Focus();
          isValid = false;
        }
      }
      //Validamos el GuestStatus
      if (isValid)
      {
        var guestStatusInfo = BRGuestStatus.GetGuestStatusInfo(dbContext.Guest.guID);

        //Si tiene informacion de GuestStatus y se desea aplicar la validacion de Tours
        if (guestStatusInfo != null && guestStatusInfo.gsMaxQtyTours > 0)
        {
          isValid = Gifts.ValidateGiftsGuestStatus(form.dtgGifts, guestStatusInfo, nameof(InvitationGift.igQty), nameof(InvitationGift.iggi));
        }
      }
      //Si pasó la validacion de GuestStatus  validamos  monto maximo de regalos
      if (isValid)
      {
        isValid = Gifts.ValidateMaxAuthGifts(form.txtGiftTotalCost.Text, form.txtGiftMaxAuth.Text);
      }
      return isValid;
    }

    #endregion

    #region ValidateEdit BeforeSave
    public static bool ValidateEditBeforeSave(string propertyName, InvitationGift invitationGift, ref int columnIndex)
    {
      bool hasError = false;
      switch (propertyName)
      {
        case "igQty":
          //Si tiene seleccionado un gift
          if (invitationGift.iggi != null)
          {
            //Buscamos el Gift
            var gift = BRGifts.GetGiftId(invitationGift.iggi);
            //Validacion cantidad máxima del regalo
            Gifts.ValidateMaxQuantityOnEntryQuantity(ref invitationGift, gift, false, 1, ref hasError, nameof(invitationGift.igQty));
          }//Si no ha seleccionado el Gift
          else
          {
            //Validacion cantidad máxima del regalo
            Gifts.ValidateMaxQuantityOnEntryQuantity(ref invitationGift, null, false, 1, ref hasError, nameof(invitationGift.igQty));
          }
          return hasError;

        case "igAdults":
          //Validacion Numero de Adultos
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Adults, invitationGift, ref hasError, nameof(invitationGift.igAdults), nameof(invitationGift.igMinors));
          return hasError;

        case "igMinors":
          //Validacion Numero de menores
          Gifts.ValidateAdultsMinors(EnumAdultsMinors.Minors, invitationGift, ref hasError, nameof(invitationGift.igAdults), nameof(invitationGift.igMinors));
          return hasError;

        default:
          break;
      }
      return hasError;
    }

    #endregion


    #endregion

    #region DataGrid Booking Deposits    
    #region StartEditBokingDeposits

    /// <summary>
    /// Valida la celda que se quiere editar
    /// </summary>
    /// <param name="dtgDeposits">Datagrid a validar</param>
    /// <param name="strColumn">Columna que se quiere editar</param>
    /// <param name="bookingDeposit">Objeto bindeado</param>
    /// <param name="blnModifyPaymentPlace">unicamente se debe enviar como false cuando sea desde el formulario show</param>
    /// <returns>True. no se puede editar | False. si se puede editar</returns>
    /// <history>
    /// [emoguel] created 10/08/2016
    /// </history>
    public static bool StartEditBookingDeposits(string strColumn, BookingDeposit bookingDeposit, bool blnModifyPaymentPlace = true)
    {
      switch (strColumn)
      {
        case "bdcc":
        case "bdCardNum":
        case "bdExpD":
        case "bdAuth":
          {
            if (bookingDeposit != null && (string.IsNullOrWhiteSpace(bookingDeposit.bdpt) || bookingDeposit.bdpt != "CC"))
            {
              return false;
            }
            break;
          }
        case "bdpc":
          {
            return blnModifyPaymentPlace;
          }
        case "bdFolioCXC":
          {
            if ((bookingDeposit.bdAmount - bookingDeposit.bdReceived) <= 0)
            {
              return false;
            }
            break;
          }

      }

      return true;
    }

    #endregion StartEditBokingDeposits

    #region validateEditBookingDeposit

    /// <summary>
    /// Valida que la columna cumpla con los datos necesarios
    /// </summary>
    /// <param name="strColumn"></param>
    /// <param name="bookingDeposit">Objeto que se está editando(e.row.Item)</param>
    /// <param name="dtgBookingDeposits">Datagrid de deposits que se está editando</param>
    /// <param name="editElement">control que se está editando</param>
    /// <history>
    /// [emoguel] 11/08/2016 created
    /// </history>
    /// <returns> false. no es valido | true. es valido</returns>
    public static bool validateEditBookingDeposit(string strColumn, BookingDeposit bookingDeposit, DataGrid dtgBookingDeposits, Control editElement, List<BookingDeposit> lstBookingDeposits, int guestID)
    {
      switch (strColumn)
      {
        #region Amount

        case "bdAmount":
          {
            return validateEditNumber(bookingDeposit.bdAmount.ToString(), "Deposit", 999999, 1);
          }

        #endregion Amount

        #region Received

        case "bdReceived":
          {
            if (validateEditNumber(bookingDeposit.bdReceived.ToString(), "Received", 999999, 0))
            {
              if (bookingDeposit.bdReceived > bookingDeposit.bdAmount)
              {
                UIHelper.ShowMessage("Received can not be greater than Deposit.");
                return false;
              }
              else if (bookingDeposit.bdReceived == bookingDeposit.bdAmount)
              {
                bookingDeposit.bdFolioCXC = null;
              }
            }
            else
            {
              return false;
            }

            break;
          }

        #endregion Received

        #region Card Number

        case "bdCardNum":
          {
            if (bookingDeposit.bdpt == "CC")//Sólo si es tipo Credit Card hace las validaciones
            {
              if (bookingDeposit.bdCardNum == null || bookingDeposit.bdCardNum.ToString().Length > 4 || bookingDeposit.bdCardNum.ToString().Length < 4)
              {
                UIHelper.ShowMessage("Specify the last four numbers of credit card.");
                return false;
              }
              //validamos que no este repetida la moneda, forma de pago, tipo de tarjeta de credito y numero de tarjeta
              if (isRepeatCreditCardInfo(dtgBookingDeposits, bookingDeposit))
              {
                UIHelper.ShowMessage("A currency and type of payment can not be specified twice.");
                return false;
              }
              return validateEditNumber(bookingDeposit.bdCardNum.ToString(), "Card Number", 999999, 0);
            }
            break;
          }

        #endregion Card Number

        #region Payment type

        case "bdpt":
          {
            //Validamos que se especifique el tipo de pago
            if (string.IsNullOrWhiteSpace(bookingDeposit.bdpt))
            {
              UIHelper.ShowMessage("Specify the Payment Type");
              return false;
            }

            //validamos que no este repetida la moneda, forma de pago, tipo de tarjeta de credito y numero de tarjeta
            if (isRepeatCreditCardInfo(dtgBookingDeposits, bookingDeposit))
            {
              UIHelper.ShowMessage("A currency and type of payment can not be specified twice.");
              return false;
            }

            if (bookingDeposit != null && !string.IsNullOrWhiteSpace(bookingDeposit.bdpt) && bookingDeposit.bdpt != "CC")
            {
              bookingDeposit.bdcc = null;
              bookingDeposit.bdCardNum = null;
              bookingDeposit.bdExpD = "";
              bookingDeposit.bdAuth = null;
              bookingDeposit.bdds = null;
              bookingDeposit.bdD = DateTime.Now;
              GridHelper.UpdateCellsFromARow(dtgBookingDeposits);
            }
            break;
          }

        #endregion Payment type

        #region Expiration date

        case "bdExpD":
          {
            if (bookingDeposit.bdpt == "CC")//Sólo se valida cuando es tipo Credit Card
            {
              bool blnIsValid = Regex.IsMatch(bookingDeposit.bdExpD, @"^(0[1-9]|1[0-2])\/?([0-9]{2})$");
              if (!blnIsValid)
              {
                UIHelper.ShowMessage("Expiration Date is not valid");
              }
              return blnIsValid;
            }
            break;
          }

        #endregion Expiration date

        #region Tarjeta de credito y tipo de moneda

        case "bdcu":
        case "bdcc":
          {
            //Si es tarjeta de credito verificar que tenga el tipo de tarjeta
            if (bookingDeposit.bdpt == "CC" && string.IsNullOrWhiteSpace(bookingDeposit.bdcc))
            {
              UIHelper.ShowMessage("Specify a credit card type");
              return false;
            }

            //Verificar que se especifique el currency
            if (string.IsNullOrWhiteSpace(bookingDeposit.bdcu))
            {
              UIHelper.ShowMessage("Specify the Currency.");
              return false;
            }
            //validamos que no este repetida la moneda, forma de pago, tipo de tarjeta de credito y numero de tarjeta
            if (isRepeatCreditCardInfo(dtgBookingDeposits, bookingDeposit))
            {
              UIHelper.ShowMessage("A currency and type of payment can not be specified twice.");
              return false;
            }

            break;
          }

        #endregion Tarjeta de credito y tipo de moneda

        #region Lugar de pago

        case "bdpc":
          {
            if (string.IsNullOrWhiteSpace(bookingDeposit.bdpc))
            {
              UIHelper.ShowMessage("Specify the Payment place.");
              return false;
            }
            break;
          }

        #endregion Lugar de pago

        #region Number Autorization

        case "bdAuth":
          {
            if (bookingDeposit.bdpt == "CC" && string.IsNullOrWhiteSpace(bookingDeposit.bdAuth))
            {
              UIHelper.ShowMessage("Specify a Authorization ID.");
              return false;
            }
            break;
          }

        #endregion Number Autorization

        #region CxC

        case "CxC":
          {
            if ((bookingDeposit.bdAmount - bookingDeposit.bdReceived) > 0 && bookingDeposit.bdpt != "CS")
            {
              UIHelper.ShowMessage("CxC is only for payment type 'Cash'.");
              return false;
            }
            break;
          }

        #endregion CxC

        #region Folio CxC

        case "bdFolioCXC":
          {
            //Si el CxC es mayor a cero y el Folio CxC es null o cero
            if ((bookingDeposit.bdAmount - bookingDeposit.bdReceived) > 0 && bookingDeposit.bdFolioCXC == null)
            {
              UIHelper.ShowMessage("Specify the CxC folio.");
              return false;
            }
            //Si el folio es diferente de null o cero y el CXC es 0
            else if ((bookingDeposit.bdAmount - bookingDeposit.bdReceived) == 0 && bookingDeposit.bdFolioCXC != null)
            {
              UIHelper.ShowMessage("CxC Folio is only for CxC distinct of zero.");
              return false;
            }
            //Si el folioCxC es diferente de null
            else if (bookingDeposit.bdFolioCXC != null)
            {
              //Verificar que el folio no esté en la lista y que no lo contenga un deposit en la BD
              List<BookingDeposit> lstDeposits = dtgBookingDeposits.ItemsSource.OfType<BookingDeposit>().ToList();
              if (lstDeposits.Where(bd => bd.bdFolioCXC == bookingDeposit.bdFolioCXC).Count() > 1)
              {
                UIHelper.ShowMessage("Please select other Folio");
                return false;
              }
              else if (bookingDeposit.bdID > 0)//Verificamos si el booking deposit se está actualizando
              {
                if (lstBookingDeposits != null && lstBookingDeposits.Where(bd => bd.bdID == bookingDeposit.bdID && bd.bdFolioCXC != bookingDeposit.bdFolioCXC).ToList().Count > 0)
                {
                  string folio = BRFoliosCXC.FolioValidateCXC(Convert.ToInt32(bookingDeposit.bdFolioCXC), guestID, true, 1);
                  if (folio != "VALIDO")
                  {
                    UIHelper.ShowMessage(folio);
                    return false;
                  }
                }
              }
              else
              {
                string folio = BRFoliosCXC.FolioValidateCXC(Convert.ToInt32(bookingDeposit.bdFolioCXC), guestID, true, 1);
                if (folio != "VALIDO")
                {
                  UIHelper.ShowMessage(folio);
                  return false;
                }
              }
            }
            else//Volver nulo el booking Deposits
            {
              bookingDeposit.bdFolioCXC = null;
            }
            break;
          }

          #endregion Folio CxC
      }
      return true;
    }

    #endregion validateEditBookingDeposit

    #region validateEditNumber

    /// <summary>
    ///
    /// </summary>
    /// <param name="number"></param>
    /// <param name="strTitle"></param>
    /// <param name="maxNumber"></param>
    /// <param name="minNumber"></param>
    /// <param name="blnValidateBounds"></param>
    /// <history>
    /// [emoguel] 11/08/2016 created
    /// </history>
    /// <returns> false. no es valido | true. es valido</returns>
    public static bool validateEditNumber(string number, string strTitle, int maxNumber, int minNumber, bool blnValidateBounds = true)
    {
      if (!string.IsNullOrWhiteSpace(number))
      {
        decimal numb = 0;
        decimal.TryParse(number, out numb);
        if (blnValidateBounds)
        {
          if (numb > maxNumber)
          {
            UIHelper.ShowMessage($"{strTitle} can not be greater than {maxNumber}");
            return false;
          }
          if (numb < minNumber)
          {
            UIHelper.ShowMessage($"{strTitle} can not be lower than {minNumber}");
            return false;
          }
        }
        return true;
      }
      else
      {
        UIHelper.ShowMessage($"{strTitle} is invalid");
        number = "0";
        return false;
      }
    }

    #endregion validateEditNumber

    #region EndingEditBookingDeposits
    /// <summary>
    ///
    /// </summary>
    /// <param name="bookingDeposits"></param>
    /// <returns>True. Son campos validos | False. Hay un campo invalido</returns>
    /// <history>
    /// [emoguel] 11/08/2016 created
    /// </history>
    public static bool EndingEditBookingDeposits(BookingDeposit bookingDeposit, DataGrid dtgBookingDeposits, List<BookingDeposit> lstBookingsDeposits, int guestID, ref int columnIndex)
    {
      PropertyInfo[] properties = bookingDeposit.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
      foreach (PropertyInfo pi in properties)
      {
        if (!validateEditBookingDeposit(pi.Name, bookingDeposit, dtgBookingDeposits, null, lstBookingsDeposits, guestID))
        {
          var column = dtgBookingDeposits.Columns.Where(cl => cl.SortMemberPath == pi.Name).FirstOrDefault();
          columnIndex = (column != null) ? column.DisplayIndex : 0;
          return false;
        }
      }
      return true;
    }
    #endregion

    #region isRepeatCreditCardInfo

    /// <summary>
    /// Validaciones del creditcard info
    /// </summary>
    /// <param name="dtgBookingDeposits">datagrid de deposits</param>
    /// <history>
    /// [emoguel] 12/08/2016 created
    /// </history>
    /// <returns>True. existe otro item con los mismos datos | False. no existe otro item con los mismo datos</returns>
    public static bool isRepeatCreditCardInfo(DataGrid dtgBookingDeposits, BookingDeposit bookingDeposits)
    {
      GridHelper.UpdateSourceFromARow(dtgBookingDeposits);//Actualizamos el source
      List<BookingDeposit> lstItemsGrid = dtgBookingDeposits.Items.OfType<BookingDeposit>().ToList();
      if (lstItemsGrid.Count > 1 && bookingDeposits != null)
      {
        var lstRepeatItems = lstItemsGrid.Where(bd => bd.bdcu == bookingDeposits.bdcu && bd.bdpt == bookingDeposits.bdpt && bd.bdcc == bookingDeposits.bdcc && bd.bdCardNum == bookingDeposits.bdCardNum).ToList();
        return lstRepeatItems.Count > 1;
      }
      return false;
    }
    #endregion

    #region ValidateBookingDeposits
    /// <summary>
    /// Valida el grid de deposits antes de guardar
    /// Verifica que no haya ni un registro en modo edición
    /// </summary>
    /// <param name="form">Formulario de invitación</param>
    /// <returns>True. Es valido | false. No es valido</returns>
    /// <history>
    /// [emoguel] 10/09/2016 created
    /// </history>
    private static bool ValidateBookingDeposits(frmInvitation form)
    {
      bool isValid = true;
      //Validar que ya se haya salido del modo edición del Grid de Booking Deposits
      DataGridRow row = GridHelper.GetRowEditing(form.dtgBookingDeposits);
      if (row != null)
      {
        int columnIndex = 0;
        bool gridvalid = EndingEditBookingDeposits(row.Item as BookingDeposit, form.dtgBookingDeposits, form.dbContext.CloneBookingDepositList, form.dbContext.Guest.guID, ref columnIndex);
        if (gridvalid)
        {
          form.dtgBookingDeposits.RowEditEnding -= form.dtgBookingDeposits_RowEditEnding;
          form.dtgBookingDeposits.CommitEdit();
          form.dtgBookingDeposits.RowEditEnding += form.dtgBookingDeposits_RowEditEnding;
        }
        else
        {
          isValid = false;
          GridHelper.SelectRow(form.dtgBookingDeposits, row.GetIndex(), columnIndex, true);
          form.tabStatusGiftsOthers.IsSelected = true;
        }
      }
      return isValid;
    }
    #endregion

    #endregion

    #region Validate Guest Additional

    #region Grid Guest Additional

    #region StartEdit

    /// <summary>
    /// Inicia las validaciones de los campos del Grid
    /// </summary>
    /// <param name="currentCellInfo">Celda que se esta editando</param>
    /// <param name="dtg">El datagrid que se esta modificando</param>
    /// <param name="_hasError">True tiene error | False No tiene</param>
    public static bool dtgGuestAdditional_StartEdit(ref DataGridCellInfo currentCellInfo, DataGrid dtg, ref bool _hasError)
    {
      switch (currentCellInfo.Column.SortMemberPath)
      {
        case "guFirstName1":
          _hasError = true;
          break;

        case "guLastName1":
          _hasError = true;
          break;
      }
      return _hasError;
    }

    #endregion StartEdit

    #region ValidateEdit

    /// <summary>
    /// Valida la informacion del Grid
    /// </summary>
    /// <param name="guestAdditional">Objeto enlazada a la fila que se esta editando</param>
    /// <param name="currentCellInfo">Celda que se esta editando</param>
    /// <returns>
    /// True  si tiene error| False si NO tiene
    /// </returns>
    /// <history>
    /// [edgrodriguez] 16/08/2016  Created.
    /// </history>
    public static async System.Threading.Tasks.Task<bool> dtgGuestAdditional_ValidateEdit(Guest guestParent, Guest guestAdditional, DataGridCellInfo currentCellInfo, EnumProgram program)
    {
      bool _hasError = false;
      switch (currentCellInfo.Column.SortMemberPath)
      {
        case "guID":
          _hasError = !(await ValidateAdditionalGuest(guestParent, guestAdditional, program, true)).Item1;
          break;
      }
      return _hasError;
    }

    #endregion ValidateEdit

    #endregion Grid Guest Additional

    /// <summary>
    /// Valida  que el guest adicional.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="additional"></param>
    /// <returns> True | False </returns>
    /// <history>
    /// [edgrodriguez] 16/08/2016  Created.
    /// </history>
    public static async System.Threading.Tasks.Task<Tuple<bool, string>> ValidateAdditionalGuest(Guest parent, Guest additional, EnumProgram program, bool showMsg = false)
    {
      bool IsValid = true;
      string msg = "";

      if (additional == null || (additional != null && additional.guID == 0))
      {
        msg = "Guest ID doesn't exists.";
        IsValid = false;
      }
      else if (parent.guID > 0 && additional.guID == parent.guID)
      {
        msg = "The main guest can't be an additional guest.";
        IsValid = false;
      }
      else if (!additional.guCheckIn && program == EnumProgram.Inhouse)
      {
        msg = $"The additional guest {((showMsg) ? additional.guID.ToString() : "")} has not made Check-In.";
        IsValid = false;
      }
      else if (!(parent.guID > 0 && parent.guRef != null))
      {
        // obtenemos un huesped principal que tenga como huesped adicional al huesped adicional en cuestion.
        var guestAux = await BRGuests.GetMainGuest(additional.guID, (parent.guID > 0) ? parent.guID : 0);

        //Si lo encontramos.
        if (guestAux != null && guestAux.guID > 0)
        {
          msg = $"The additional guest {((showMsg) ? additional.guID.ToString() : "")} already belongs to the invitation {guestAux.guID} - {guestAux.guLastName1} {guestAux.guFirstName1}.";
          IsValid = false;
        }
      }
      else if (additional.guInvit)
      {
        msg = $"The additional guest {((showMsg) ? additional.guID.ToString() : "")} already has an invitation.";
        IsValid = false;
      }            
        

      if (showMsg && !string.IsNullOrWhiteSpace(msg))
      {
        UIHelper.ShowMessage(msg, MessageBoxImage.Exclamation, "Intelligence Marketing");
      }

      return Tuple.Create(IsValid, msg);
    }

    #endregion Validate Guest Additional

    #region DataGrid Guest Credit Card

    #region ValidateGuestCreditCard
    /// <summary>
    /// Valida que no halla 
    /// </summary>
    /// <param name="gcc">Listado de los Guest Credit Card</param>
    /// <history>
    /// [jorcanche]  created 18/08/2016
    /// </history>
    public static bool ValidateGuestCreditCard(List<GuestCreditCard> gcc)
    {
      bool validate = true;
      gcc.ForEach(guestCreditCardList =>
      {
        if (string.IsNullOrEmpty(guestCreditCardList.gdcc) || guestCreditCardList.gdQuantity == 0)
        {
          validate = false;
        }
      });
      return validate;
    }
    #endregion

    #endregion
    #endregion

  }
}