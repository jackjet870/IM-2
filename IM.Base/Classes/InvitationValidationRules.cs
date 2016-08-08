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
      bool _passValidate = false;
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
              _passValidate = Gifts.ValidateMaxQuantityGiftTour(dtg, guestStatusType, nameof(invitationGift.igQty), nameof(invitationGift.iggi));

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
    /// <summary>
    /// Validamos informacion general del formulario
    /// </summary>
    /// <param name="form">Ref frmInvitation</param>
    /// <param name="guest">GuestObj</param>
    /// <param name="program">Program</param>
    /// <returns>True is valid | False No</returns>
    public static bool ValidateGeneral(frmInvitation form, CommonCatObject dbContext, string program)
    {
      bool _isValid = true;
      string _res = string.Empty;
      //Validamos folio invitacion OutHouse
      if (!validateFolioOutHouse(ref form, dbContext.GuestObj, program)) return false;
      //Validamos folio de reservacion inhouse
      if (!ValidateFolioInHouse(ref form, dbContext.GuestObj, program)) return false;
      //Si la fecha de Arrival esta en fecha Cerrada
      if (!ValidateCloseDate(dbContext)) return false;
      //Validamos que el Numero de habitaciones este en el rango permitido
      if (!ValidateHelper.ValidateNumber(dbContext.GuestObj.guRoomsQty, 1, 5, "Rooms Quantity")) return false;
      //Validamos que se ingresen datos en los campos obligatorios
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(form, "Invitation", blnDatagrids: true, showMessage: true))) return false;
      //Validamos el Booking y el Reschedule
      if (!ValidateBookReschedule(ref form,dbContext)) return false;

      return _isValid;
    }

    #region validateFolioOutHouse
    /// <summary>
    /// Valida que el folio ingresado en el control txtguOutInvitNum sea valido.
    /// </summary>
    /// <param name="form">ref frmInvitation</param>
    /// <param name="guest">Guest Obj</param>
    /// <returns>True is valid | False No</returns>
    /// <history>
    /// [erosado] 05/08/2016  Created.
    /// </history>
    private static bool validateFolioOutHouse(ref frmInvitation form, Guest guest, string program)
    {
      bool _valid = true;
      string _serie = "";
      int _numero = 0;

      if (program == EnumToListHelper.GetEnumDescription(EnumProgram.Outhouse))
      {
        //Validamos que se ingrese el folio
        if (!ValidateHelper.ValidateRequired(form.txtguOutInvitNum, "Outhouse Invitation Folio"))
        {
          _valid = false;
        }
        else if (!ValidateFolioOuthouseFormat(ref form, ref _serie, ref _numero))
        {
          UIHelper.ShowMessage("Please specify a Outhouse invitation folio valid in the format 'AA-0000'");
          _valid = false;
        }
        else
        {
          //Verificamos que sea valido.
          _valid = BRFolios.ValidateFolioInvitationOutside(guest.guID, _serie, _numero);
        }
      }
      return _valid;
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

    #region ValidateFolioInHouse
    /// <summary>
    /// Valida que el folio de reservacion inhouse no haya sido utilizado anteriormente
    /// </summary>
    /// <param name="form">ref frmInvitation</param>
    /// <param name="guest">Guest Obj</param>
    /// <param name="program">Program</param>
    /// <returns>True is valid | False NO</returns>
    private static bool ValidateFolioInHouse(ref frmInvitation form, Guest guest, string program)
    {
      bool _isValid = true;

      if (program == EnumToListHelper.GetEnumDescription(EnumProgram.Inhouse))
      {
        //Si el boton de busqueda de Guest esta activo
        if (form.imgSearch.IsEnabled == true)
        {
          //Validamos que el folio no haya sido utilizado por otra invitacion
          _isValid = BRFolios.ValidateFolioReservation(guest.guls, guest.guHReservID, guest.guID);

          //Si el folio de reservacion NO es valido hay que escoger otro
          if (!_isValid)
          {
            UIHelper.ShowMessage("The Reservation Folio was already used for Guest ID");
            form.imgSearch.Focus();
          }
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
      bool _isValid = true;
      //Validamos fecha y hora del booking
      _isValid = ValidateBookingDateTime(ref form);

      if (_isValid)
      {
        //ValidateRescheduleDateTime();
      }

      return _isValid;
      //Validamos la fecha y hora del Reschedule
    }

    /// <summary>
    /// Valida fecha y hora del Booking
    /// </summary>
    /// <param name="form">ref frmInvitation</param>
    /// <returns>True is valid | False No</returns>
    /// <history>
    /// [lchairez]  ? Created.
    /// [erosado] 06/08/2016  Modified, agregamos parametros y optimizamos el codigo.
    /// </history>
    private static bool ValidateBookingDateTime(ref frmInvitation form)
    {
      bool _IsValid = true;

      //Si se puede modificar la fecha del booking
      if (form.dtpBookDate.IsEnabled)
      {
        if (form.dtpBookDate.Value.HasValue)
        {
          //Si tiene permiso de PRInvitations y su nivel es Super Special
          if (form._user.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.SuperSpecial))
          {

          }
        }
        else
        {

        }
      }
      else
      {

      }

      if (!form.dtpBookDate.Value.HasValue)
      {
        UIHelper.ShowMessage("Specify a Booking Date");
        form.dtpBookDate.Focus();
        _IsValid = false;
      }
      // si tiene permiso super especial de invitaciones
      else if (form._user.Permissions.Where(p => p.plN == "SuperSpecial").Any())
      {
        #region Instrucciones
        /*
        Permiso super especial (4) cuando hace una nueva.
        1. No puede hacer invitacion antes de la fecha de llegada
        2. Si puede hacer invitaciones despues de la fecha de salida pero la fecha de booking debe estar dentro del periodo de estadia del huesped
        3. Si puede hacer invitaciones de dias anteriores
        4. Tiene un periodo de 30 dias despues de la fecha de salida para hacer la invitacion
        */
        #endregion
        //validamos que el invitado no tenga un mes de que haya salido
        if (DateTime.Today >= form.dtpBookDate.Value.Value.AddDays(30))
        {
          UIHelper.ShowMessage("Guest already made check out.");
          form.dtpBookDate.Focus();
          _IsValid = false;
        }
      }
      else
      {
        //validamos que la fecha de booking no sea antes de hoy
        if (form.dtpBookDate.Value.Value.Date < DateTime.Today.Date)
        {
          UIHelper.ShowMessage("Booking date can not be before today.");
          form.dtpBookDate.Focus();
          _IsValid = false;
        }
      }

      if (!_IsValid) return _IsValid;
      //Validamos que la fecha de entrada no sea mayor a la fecha de salida
      if ((DateTime.Compare(form.dtpBookDate.Value.Value, form.dtpOtherInfoDepartureD.Value.Value)) > 0)
      {
        UIHelper.ShowMessage("Booking date can not be after Check Out date");
        form.dtpBookDate.Focus();
        _IsValid = false;
      }
      if (form.dtpOtherInfoDepartureD.Value.HasValue && (form.dtpOtherInfoDepartureD.Value.Value.Date < form.dtpBookDate.Value.Value.Date))
      {

      }
      else if (form.dtpOtherInfoArrivalD.Value.HasValue && (form.dtpOtherInfoArrivalD.Value.Value.Date > form.dtpBookDate.Value.Value.Date))
      {
        UIHelper.ShowMessage("Booking date can not be before Check In date.");
        form.dtpBookDate.Focus();
        _IsValid = false;
      }
      if (form.cbmBookTime.SelectedValue == null || String.IsNullOrEmpty(form.cbmBookTime.SelectedValue.ToString()) || form.cbmBookTime.SelectedIndex == -1)
      {
        UIHelper.ShowMessage("Specify a Booking Time.");
        form.cbmBookTime.Focus();
        _IsValid = false;
      }

      return _IsValid;
    }

    #endregion


  }
}
