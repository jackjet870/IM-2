using IM.Base.Helpers;
using System.Windows.Controls;
using IM.Model.Enums;
using System;
using System.Windows;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Base.Forms;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using IM.Model.Classes;

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

    #region Grid Invitation Gift

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
    #endregion

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
    internal static bool ValidateEdit(ref InvitationGift invitationGift, ref DataGridCellInfo currentCellInfo)
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

    #endregion

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
    internal static void AfterEdit(DataGrid dtg, ref InvitationGift invitationGift, DataGridCellInfo currentCell,
      ref TextBox txtTotalCost, ref TextBox txtTotalPrice, ref TextBox txtgrMaxAuthGifts, GuestStatusType guestStatusType, string program = "")
    {
      //bool _passValidate = false;
      //Obtenemos el Gift
      var gift = BRGifts.GetGiftId(invitationGift.iggi);

      switch (currentCell.Column.SortMemberPath)
      {
        case "igQty":
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
        case "iggi":
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

            //Si es OutHouse y ya selecciono guestStatusType
            if (program == EnumToListHelper.GetEnumDescription(EnumProgram.Outhouse) && guestStatusType != null)
            {
              //Valida que no den mas de los tours permitidos && Validamos que no den mas de los descuentos de Tour 
              // TODO:  EDDER-> Cambiar el tipo de parametro guestStatusType por GuestStatusValidateData
              //_passValidate = Gifts.ValidateMaxQuantityGiftTour(dtg, guestStatusType, nameof(invitationGift.igQty), nameof(invitationGift.iggi));

            }

            //Calculamos los costos y los precios 
            Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
                nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
                nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
                nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
                nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false);
          }
          break;
        case "igAdults":
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false, EnumPriceType.Adults);
          break;
        case "igMinors":
          // calculamos los costos y precios
          Gifts.CalculateCostsPrices(ref invitationGift, gift, nameof(invitationGift.igQty),
              nameof(invitationGift.igAdults), nameof(invitationGift.igMinors),
              nameof(invitationGift.igExtraAdults), nameof(invitationGift.igPriceA),
              nameof(invitationGift.igPriceM), nameof(invitationGift.igPriceAdult),
              nameof(invitationGift.igPriceMinor), nameof(invitationGift.igPriceExtraAdult), false, EnumPriceType.Minors);
          break;
        case "igExtraAdults":
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

    #endregion

    #endregion

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
      if (!ValidateFolio(ref form, dbContext)) return false;
      //Si la fecha de Arrival esta en fecha Cerrada
      if (!ValidateCloseDate(dbContext)) return false;
      //Validamos que el Numero de habitaciones este en el rango permitido
      if (!ValidateHelper.ValidateNumber(dbContext.Guest.guRoomsQty, 1, 5, "Rooms Quantity")) return false;
      //Validamos que se ingresen datos en los campos obligatorios
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(form, "Invitation", blnDatagrids: true, showMessage: true))) return false;
      //Validamos el Booking y el Reschedule
      if (!ValidateBookReschedule(ref form, dbContext)) return false;

      return true;
    }

    #endregion

    public static bool ValidateInformationGrids(frmInvitation form, GuestInvitation dbContext)
    {
      bool _isValid = true;
      //Validamos el Status del invitado
      if (string.IsNullOrWhiteSpace(dbContext.Guest.guGStatus))
      {
        UIHelper.ShowMessage("Specify the Guest Status");
        form.cmbGuestStatus.Focus();
        _isValid = false;
      }
      //Validamos los regalos de la invitacion
      else if (!InvitationGiftValidation(ref form, dbContext)) _isValid = false;
      else if (!BookingDepositsValidation(ref form, dbContext)) _isValid = false;

      return _isValid;
    }

    private static bool BookingDepositsValidation(ref frmInvitation form, GuestInvitation dbContext)
    {
      bool _isValid = true;
      List<string> _fieldNameToValidate = new List<string>(){nameof(BookingDeposit.bdcu), nameof(BookingDeposit.bdpt), nameof(BookingDeposit.bdcc), nameof(BookingDeposit.bdCardNum)};
      //Validamos que no esten repetidos los campos Moneda, Forma de pago, Tipo de tarjeta de credito y numero de tarjeta de credito
      if (!string.IsNullOrWhiteSpace(ObjectHelper.AreAnyDuplicates(dbContext.BookingDepositList.ToList(),_fieldNameToValidate)))
      {
        UIHelper.ShowMessage("A currency and type of payment can not be specified twice.");
        form.dtgBookingDeposits.Focus();
        _isValid = false;
      }
      //Validamos campo por campo que se hayan ingresado
      else
      {

      }
      return _isValid;
    }

    #region ValidateFolio
    /// <summary>
    /// Valida el Folio dependiendo del tipo de programa que sea InHouse o OutHouse
    /// Valida que el folio de reservacion InHouse no haya sido utilizado anteriormente
    /// Valida que el folio de reservacion OutHouse ingresado en el control txtguOutInvitNum sea valido.
    /// </summary>
    /// <param name="form">ref frmInvitation</param>
    /// <param name="dbContext">CommonCatObject</param>
    /// <returns>True is valid | False No</returns>
    ///<history>
    ///[erosado]  08/08/2016  Created.
    /// </history>
    private static bool ValidateFolio(ref frmInvitation form, GuestInvitation dbContext)
    {
      bool _isValid = true;
      string _serie = "";
      int _numero = 0;

      if (dbContext.Program == EnumProgram.Inhouse)
      {
        //Si el boton de busqueda de Guest esta activo
        if (form.brdSearchButton.IsEnabled == true)
        {
          //Validamos que el folio no haya sido utilizado por otra invitacion
          _isValid = BRFolios.ValidateFolioReservation(dbContext.Guest.guls, dbContext.Guest.guHReservID, dbContext.Guest.guID);

          //Si el folio de reservacion NO es valido hay que escoger otro
          if (!_isValid)
          {
            UIHelper.ShowMessage("The Reservation Folio was already used for Guest ID");
            form.brdSearchButton.Focus();
          }
        }
      }
      else
      {
        //Validamos que se ingrese el folio
        if (!ValidateHelper.ValidateRequired(form.txtguOutInvitNum, "Outhouse Invitation Folio"))
        {
          _isValid = false;
        }
        else if (!ValidateFolioOuthouseFormat(ref form, ref _serie, ref _numero))
        {
          UIHelper.ShowMessage("Please specify a Outhouse invitation folio valid in the format 'AA-0000'");
          _isValid = false;
        }
        else
        {
          //Verificamos que sea valido.
          _isValid = BRFolios.ValidateFolioInvitationOutside(dbContext.Guest.guID, _serie, _numero);
        }

      }

      return _isValid;
    }
    #endregion

    #region ValidateCloseDate
    /// <summary>
    //Valida que la fecha de Arrival se encuentreno se encuentre en fecha cerrada
    /// </summary>
    /// <param name="dbContext">CommonCatObject</param>
    /// <returns>True is Valid | False No</returns>
    private static bool ValidateCloseDate(GuestInvitation dbContext)
    {
      bool _isValid = true;
      if (dbContext.Guest.guBookD == null)
      {
        UIHelper.ShowMessage("Please select an Arrival date");
        _isValid = false;
      }
      //Si la fecha de Arrival esta en fecha Cerrada
      if (dbContext.Guest.guBookD < dbContext.CloseDate)
      {
        UIHelper.ShowMessage("It's not allowed to make Invitations for a closed date.");
        _isValid = false;
      }
      return _isValid;
    }
    #endregion

    #region  ValidateBookReschedule
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
      if (ValidateRescheduleDateTime(ref form, dbContext)) return false;

      return true;
    }
    #endregion

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
      if (form._allowReschedule)
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

    #endregion

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
          else if (dbContext.Guest.guBookD < dbContext.Guest.guCheckOutD)
          {
            UIHelper.ShowMessage("Booking date can not be before Check In date.");
            form.dtpBookDate.Focus();
            _isValid = false;
          }
          #endregion

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
          if (ValidateHelper.ValidateRequired(form.cmbBookT, "Booking Time")) { _isValid = false; }
        }

      }
      return _isValid;
    }

    #endregion

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
    #endregion

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
      bool _isValid = true;
      //Validamos gift duplicados
      if (ObjectHelper.AreAnyDuplicates(dbContext.InvitationGiftList.ToList(), nameof(InvitationGift.iggi)))
      {
        UIHelper.ShowMessage("Repeated gift");
        form.dtgGifts.Focus();
        _isValid = false;
      }
      //Si pasó la validacion de Registros duplicados
      if (_isValid)
      {
        //Validamos el GuestStatus
        var guestStatusInfo = BRGuestStatus.GetGuestStatusInfo(dbContext.Guest.guID);

        //Si tiene informacion de GuestStatus y se desea aplicar la validacion de Tours
        if (guestStatusInfo != null && guestStatusInfo.gsMaxQtyTours > 0)
        {
          _isValid = Gifts.ValidateGiftsGuestStatus(form.dtgGifts, guestStatusInfo, nameof(InvitationGift.igQty), nameof(InvitationGift.iggi));
        }
      }
      //Si pasó la validacion de GuestStatus  validamos  monto maximo de regalos
      if (_isValid)
      {
        _isValid = Gifts.ValidateMaxAuthGifts(form.txtGiftTotalCost.Text, form.txtGiftMaxAuth.Text);
      }
      return _isValid;
    }
    #endregion

    #endregion

    #region DataGrid Booking Deposits
    #region IsReaOnlyBookingDeposits
    /// <summary>
    /// indica si el grid sólo va a ser lectura
    /// </summary>
    /// <param name="enumMode">Modo en que se encuentra la ventana</param>
    /// <returns>True. Va a ser modo lectura | False. Se puede editar</returns>
    /// <history>
    /// [emoguel] 12/08/2016 created 
    /// </history>
    public static bool IsReaOnlyBookingDeposits(EnumMode enumMode, Guest guest, bool blnNewInv, EnumPermisionLevel enumPermisionLevel)
    {
      //Validar si se está editando
      if (enumMode == EnumMode.Edit)
      {
        //Falta validar si no es el modulo outside

        // si la fecha de salida es hoy o despues y (es una invitacion nueva o la fecha de invitacion es hoy o
        // (tiene permiso especial de invitaciones y la fecha de booking original Mayor o igual a hoy))
        if (!(guest.guCheckOutD >= DateTime.Now && (blnNewInv || guest.guInvitD == DateTime.Now || (enumPermisionLevel >= EnumPermisionLevel.Special && guest.guBookD >= DateTime.Now))))
        {
          return true;
        }
        //Fin de la validacion
      }
      return false;
    }
    #endregion

    #region StartEditBokingDeposits
    /// <summary>
    /// Valida la celda que se quiere editar
    /// </summary>
    /// <param name="dtgDeposits">Datagrid a validar</param>
    /// <param name="strColumn">Columna que se quiere editar</param>
    /// <param name="bookingDeposit">Objeto bindeado</param>
    /// <param name="blnModifyPaymentPlace"></param>
    /// <returns>True. no se puede editar | False. si se puede editar</returns>
    /// <history>
    /// [emoguel] created 10/08/2016
    /// </history>
    public static bool StartEditBookingDeposits(string strColumn, BookingDeposit bookingDeposit,bool blnModifyPaymentPlace=true)
    {
      switch (strColumn)
      {
        case "bdcc":
        case "bdCardNum":
        case "bdExpD":
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
      }

      return true;
    }
    #endregion

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
    public static bool validateEditBookingDeposit(string strColumn, BookingDeposit bookingDeposit, DataGrid dtgBookingDeposits, Control editElement,List<BookingDeposit> lstBookingDeposits,int guestID)
    {
      switch (strColumn)
      {
        #region Amount
        case "bdAmount":
          {
            return validateEditNumber(bookingDeposit.bdAmount.ToString(), "Deposit", 999999, 1);
          }
        #endregion
        #region Received
        case "bdReceived":
          {
            if( validateEditNumber(bookingDeposit.bdReceived.ToString(), "Received", 999999, 0))
            {
              if(bookingDeposit.bdReceived>bookingDeposit.bdAmount)
              {
                UIHelper.ShowMessage("Received can not be greater than Deposit.");
                bookingDeposit.bdReceived = bookingDeposit.bdAmount;
                return false;
              }
            }
            else
            {
              return false;
            }

            break;
          }
        #endregion
        #region Card Number
        case "bdCardNum":
          {
            if (bookingDeposit.bdpt == "CC")//Sólo si es tipo Credit Card hace las validaciones
            {
              if (bookingDeposit.bdCardNum == null || bookingDeposit.ToString().Length > 4)
              {
                UIHelper.ShowMessage("Specify the last four numbers of credit card.");
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
        #endregion
        #region Payment type
        case "bdpt":
          {
            //Validamos que se especifique el tipo de pago
            if(string.IsNullOrWhiteSpace(bookingDeposit.bdpt))
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
        #endregion
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
        #endregion
        #region Tarjeta de credito y tipo de moneda
        case "bdcu":
        case "bdcc":
          {
            //Si es tarjeta de credito verificar que tenga el tipo de tarjeta
            if(bookingDeposit.bdpt=="CC" && string.IsNullOrWhiteSpace(bookingDeposit.bdcc))
            {
              UIHelper.ShowMessage("Specify a credit card type");
              return false;
            }

            //Verificar que se especifique el currency
            if(string.IsNullOrWhiteSpace(bookingDeposit.bdcu))
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
        #endregion
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
        #endregion
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
        #endregion
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
        #endregion
        #region Folio CxC
        case "bdFolioCxC":
          {
            if ((bookingDeposit.bdAmount - bookingDeposit.bdReceived) > 0 && bookingDeposit.bdFolioCXC == null)
            {
              UIHelper.ShowMessage("Specify the CxC folio.");
              return false;
            }
            else if ((bookingDeposit.bdAmount - bookingDeposit.bdReceived) == 0 && bookingDeposit.bdFolioCXC != null)
            {
              UIHelper.ShowMessage("CxC Folio is only for CxC distinct of zero.");
              return false;
            }
            else if(bookingDeposit.bdFolioCXC!=null)
            {
              //Verificar que el folio no esté en la lista y que no lo contenga un deposit en la BD
              List<BookingDeposit> lstDeposits = dtgBookingDeposits.ItemsSource.OfType<BookingDeposit>().ToList();
              if(lstDeposits.Any(bd=>bd.bdFolioCXC==bookingDeposit.bdFolioCXC))
              {
                UIHelper.ShowMessage("Please select other Folio");
                return false;
              }
              else if(bookingDeposit.bdID>0)//Verificamos si el booking deposit se está actualizando
              {
                if(lstBookingDeposits!=null && lstBookingDeposits.Where(bd=>bd.bdID==bookingDeposit.bdID && bd.bdFolioCXC==bookingDeposit.bdFolioCXC).ToList().Count>0)
                {
                  if (!BRFoliosCXC.FolioValidateCXC(Convert.ToInt32(bookingDeposit.bdFolioCXC), guestID, true, 1)) 
                  {
                    UIHelper.ShowMessage("Folio is in use.");
                    return false;
                  }
                }
              }
              else
              {
                if (!BRFoliosCXC.FolioValidateCXC(Convert.ToInt32(bookingDeposit.bdFolioCXC), guestID, true, 0))
                {
                  UIHelper.ShowMessage("Folio is in use.");
                  return false;
                }
              }
            }
            break;
          } 
          #endregion
      }
      return true;
    } 
    #endregion

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
        int numb = 0;
        int.TryParse(number, out numb);
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
    #endregion

    #region AfertEditBookingDeposits
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bookingDeposits"></param>    
    /// <returns>True. Son campos validos | False. Hay un campo invalido</returns>
    /// <history>
    /// [emoguel] 11/08/2016 created
    /// </history>
    public static bool AfertEditBookingDeposits(BookingDeposit bookingDeposit,DataGrid dtgBookingDeposits,List<BookingDeposit>lstBookingsDeposits,int guestID)
    {
      PropertyInfo [] properties = bookingDeposit.GetType().GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance);
      foreach(PropertyInfo pi in properties)
      {
        if(!validateEditBookingDeposit(pi.Name,bookingDeposit,dtgBookingDeposits,null,lstBookingsDeposits,guestID))
        {
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
    #endregion
  }
}
