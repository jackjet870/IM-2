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
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IM.Base.Classes
{
  public class InvitationValidationRules
  {
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
    public static bool ValidateGeneral(frmInvitation form, CommonCatObject dbContext)
    {
      string _res = string.Empty;
      //Validamos el folio de reservacion (InHouse & OutHouse)
      if (!ValidateFolio(ref form, dbContext)) return false;
      //Si la fecha de Arrival esta en fecha Cerrada
      if (!ValidateCloseDate(dbContext)) return false;
      //Validamos que el Numero de habitaciones este en el rango permitido
      if (!ValidateHelper.ValidateNumber(dbContext.GuestObj.guRoomsQty, 1, 5, "Rooms Quantity")) return false;
      //Validamos que se ingresen datos en los campos obligatorios
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(form, "Invitation", blnDatagrids: true, showMessage: true))) return false;
      //Validamos el Booking y el Reschedule
      if (!ValidateBookReschedule(ref form, dbContext)) return false;

      return true;
    }

    #endregion

    public static bool ValidateInformationGrids(frmInvitation form, CommonCatObject dbContext)
    {
      bool _isValid = true;
      //Validamos el Status del invitado
      if (string.IsNullOrWhiteSpace(dbContext.GuestObj.guGStatus))
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

    private static bool BookingDepositsValidation(ref frmInvitation form, CommonCatObject dbContext)
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
    private static bool ValidateFolio(ref frmInvitation form, CommonCatObject dbContext)
    {
      bool _isValid = true;
      string _serie = "";
      int _numero = 0;

      if (dbContext.Program == EnumProgram.Inhouse)
      {
        //Si el boton de busqueda de Guest esta activo
        if (form.imgSearch.IsEnabled == true)
        {
          //Validamos que el folio no haya sido utilizado por otra invitacion
          _isValid = BRFolios.ValidateFolioReservation(dbContext.GuestObj.guls, dbContext.GuestObj.guHReservID, dbContext.GuestObj.guID);

          //Si el folio de reservacion NO es valido hay que escoger otro
          if (!_isValid)
          {
            UIHelper.ShowMessage("The Reservation Folio was already used for Guest ID");
            form.imgSearch.Focus();
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
          _isValid = BRFolios.ValidateFolioInvitationOutside(dbContext.GuestObj.guID, _serie, _numero);
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
    private static bool ValidateCloseDate(CommonCatObject dbContext)
    {
      bool _isValid = true;
      if (dbContext.GuestObj.guBookD == null)
      {
        UIHelper.ShowMessage("Please select an Arrival date");
        _isValid = false;
      }
      //Si la fecha de Arrival esta en fecha Cerrada
      if (dbContext.GuestObj.guBookD < dbContext.CloseDate)
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
    private static bool ValidateBookReschedule(ref frmInvitation form, CommonCatObject dbContext)
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
    private static bool ValidateRescheduleDateTime(ref frmInvitation form, CommonCatObject dbContext)
    {
      bool _isValid = true;
      //Si se permite reschedule
      if (form._allowReschedule)
      {
        //Si se puede modificar la fecha de Reschedule
        if (form.dtpRescheduleDate.IsEnabled)
        {
          //Validamos que tenga seleccionada una fecha y que no sea el valor default
          if (dbContext.GuestObj.guReschD == null || dbContext.GuestObj.guReschD == DateTime.MinValue)
          {
            UIHelper.ShowMessage("Specify a valid reschedule date.");
            form.dtpRescheduleDate.Focus();
            _isValid = false;
          }
          //validamos que la fecha de reschedule no sea despues de la fecha de salida
          else if (dbContext.GuestObj.guReschD > dbContext.GuestObj.guCheckOutD)
          {
            UIHelper.ShowMessage("Reschedule date can not be after check out date.");
            form.dtpRescheduleDate.Focus();
            _isValid = false;
          }
          //validamos que la fecha de reschedule no sea antes de la fecha de llegada
          else if (dbContext.GuestObj.guReschD < dbContext.GuestObj.guCheckInD)
          {
            UIHelper.ShowMessage("Reschedule date can not be before Check-in date.");
            form.dtpRescheduleDate.Focus();
            _isValid = false;
          }
          else if (dbContext.GuestObj.guReschD < dbContext.GuestObj.guBookD)
          {
            UIHelper.ShowMessage("Reschedule date can not be before booking date.");
            form.dtpRescheduleDate.Focus();
            _isValid = false;
          }
          else if (dbContext.GuestObj.guReschD < BRHelpers.GetServerDate())
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
          else if (dbContext.GuestObj.guReschD == dbContext.GuestObj.guBookD)
          {
            //validamos que la hora de reschedule no sea la misma o antes de la hora de booking
            if (dbContext.GuestObj.guReschT <= dbContext.GuestObj.guBookT)
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
    private static bool ValidateBookingDateTime(ref frmInvitation form, CommonCatObject dbContext)
    {
      bool _isValid = true;
      EnumPermission _permission = EnumPermission.PRInvitations;

      //Si el control dtpBookDate esta disponible podemos editar la fecha si no retornamos True
      if (form.dtpBookDate.IsEnabled)
      {
        //Si tiene un valor y No es el valor default
        if (dbContext.GuestObj.guBookD != null && dbContext.GuestObj.guBookD != DateTime.MinValue)
        {
          //Si el tipo de invitacion viene de host se cambia el permiso a HostInvitation
          if (form._invitationType == EnumInvitationType.Host)
          {
            _permission = EnumPermission.HostInvitations;
          }

          #region Validaciones Comunes CON y SIN permisos
          //validamos que la fecha de booking no sea despues de la fecha de salida
          if (dbContext.GuestObj.guBookD > dbContext.GuestObj.guCheckOutD)
          {
            UIHelper.ShowMessage("Booking date can not be after Check Out date.");
            form.dtpBookDate.Focus();
            _isValid = false;
          }
          //validamos que la fecha de booking no sea antes de la fecha de llegada
          else if (dbContext.GuestObj.guBookD < dbContext.GuestObj.guCheckOutD)
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
          if (form._user.HasPermission(_permission, EnumPermisionLevel.SuperSpecial) && form._invitationMode == EnumInvitationMode.modAdd)
          {
            //Valida que el invitado no tenga un mes de haber salido
            if (BRHelpers.GetServerDate() >= dbContext.GuestObj.guCheckOutD.AddDays(30))
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
            if (dbContext.GuestObj.guBookD < BRHelpers.GetServerDate())
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
    public static bool InvitationGiftValidation(ref frmInvitation form, CommonCatObject dbContext)
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
        var guestStatusInfo = BRGuestStatus.GetGuestStatusInfo(dbContext.GuestObj.guID);

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


  }
}
