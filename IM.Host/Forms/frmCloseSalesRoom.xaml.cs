using System;
using System.Windows;
using System.Windows.Controls;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using Xceed.Wpf.Toolkit;
using IM.Base.Helpers;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmCloseSalesRoom.xaml
  /// </summary>
  /// <history>
  /// [vipacheco] 27/02/2016 Created
  /// </history>
  public partial class frmCloseSalesRoom : Window
  {
    #region VARIABLES
    private DateTime _serverDate;
    #endregion

    #region Contructor
    /// <summary>
    /// Constructor de la ventana
    /// </summary>
    /// <param name="serverDate"></param>
    /// <history>
    /// [vipacheco] 27/02/2106 Created
    /// </history>
    public frmCloseSalesRoom(DateTime serverDate)
    {
      //_userData = UserData;
      _serverDate = serverDate;

      InitializeComponent();
      // Validamos si tiene los permisos para cerrar salas
      validateUserPermissions();
    } 
    #endregion

    #region EVENTOS DE TIPO WINDOW
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      var _getSalesRoom = BRSalesRooms.GetSalesRoom(App.User.SalesRoom.srID);

      //Se agrega Binding a los controles correspondientes
      dtpCloseShowsLast.Value = _getSalesRoom.srShowsCloseD;
      dtpCloseMealTicketsLast.Value = _getSalesRoom.srMealTicketsCloseD;
      dtpCloseSalesLast.Value = _getSalesRoom.srSalesCloseD;
      dtpCloseGiftsReceiptsLast.Value = _getSalesRoom.srGiftsRcptCloseD;
    }
    #endregion

    #region EVENTOS DE TIPO BOTON

    #region btnCloseShows_Click
    /// <summary>
    /// Cierrra los shows
    /// </summary>
    /// <history>
    /// [vipacheco] 15/Marzo/2016 Created
    /// </history>
    private void btnCloseShows_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumEntities.Shows, ref dtpCloseShows, dtpCloseSalesLast);
    }
    #endregion

    #region btnCloseMealTickets_Click
    /// <summary>
    /// Cierra los Meal Tickets
    /// </summary>
    /// <history>
    /// [vipacheco] 15/Marzo/2016 Created
    /// </history>
    private void btnCloseMealTickets_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumEntities.MealTickets, ref dtpCloseMealTicket, dtpCloseSalesLast);
    } 
    #endregion

    #region btnCloseSales_Click
    /// <summary>
    /// Cierra los Sales
    /// </summary>
    /// <history>
    /// [vipacheco] 15/Marzo/2016 Created
    /// </history>
    private void btnCloseSales_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumEntities.Sales, ref dtpCloseSales, dtpCloseSalesLast);
    } 
    #endregion

    #region btnCloseGiftsReceipts_Click
    /// <summary>
    /// Cierra los Gifts Receipts
    /// </summary>
    /// <history>
    /// [vipacheco] 15/Marzo/2016 Created
    /// </history>
    private void btnCloseGiftsReceipts_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumEntities.GiftsReceipts, ref dtpCloseGiftsReceipts, dtpCloseSalesLast);
    }
    #endregion

    #region btnLog_Click
    /// <summary>
    /// Funcion del evento btnLog que se encarga de desplegar el frmSalesRoomsLog
    /// </summary>
    /// <history>
    /// [vipacheco] 27/02/2016 Created
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      frmSalesRoomsLog mfrmSalesRoomsLog = new frmSalesRoomsLog() { Owner = this };
      mfrmSalesRoomsLog.ShowDialog();
    } 
    #endregion

    #endregion

    #region EVENTO DE TIPO DATAPICKER
    /// <summary>
    /// Funcion que carga los calendarios close con un dia anterior
    /// </summary>
    /// <history>
    /// [vipacheco] 01/03/2016 Created
    /// </history>
    private void dtpClose_Loaded()
    {
      dtpCloseShows.Value = _serverDate.AddDays(-1);
      dtpCloseGiftsReceipts.Value = _serverDate.AddDays(-1);
      dtpCloseSales.Value = _serverDate.AddDays(-1);
      dtpCloseMealTicket.Value = _serverDate.AddDays(-1);
    }

    #endregion

    #region METODOS DE LA CLASE

    #region CloseSalesRoom
    /// <summary>
    /// Funcion para cerrar una sala de ventas segun el tipo requerido.
    /// </summary>
    /// <param name="_closeType"> Tipo de Cierre </param>
    /// <param name="_dateClose"> Date de cierre </param>
    /// <param name="_dateCloseLast">Date del ultimo cierre registrado </param>
    /// <history>
    /// [vipacheco] 01/03/2016 Created
    /// </history>
    private void CloseSalesRoom(EnumEntities _closeType, ref DateTimePicker _dateClose, DateTimePicker _dateCloseLast)
    {
      //verificacion de fechas no mayores a la actual
      if (_dateClose.Value.Value > _serverDate)
      {
        UIHelper.ShowMessage("Closing date con not be greater than today.", MessageBoxImage.Exclamation);
        return;
      }
      //Se pregunta si en verdad desea realizar el cierre
      if (UIHelper.ShowMessage("Are you sure you want to close all  " + _closeType.ToString() + " until " + _dateClose.Text + "? You wont't be able to modify that " + _closeType.ToString() + " anymore.",
                              MessageBoxImage.Question) == MessageBoxResult.Cancel)
        return;

      //Realizamos el cierre
      BRSalesRooms.SetCloseSalesRoom(_closeType, App.User.SalesRoom.srID, _dateClose.Value);

      //Guardamos la accion en el historico de sala de ventas
      BRSalesRoomsLogs.SaveSalesRoomLog(App.User.SalesRoom.srID, Convert.ToInt16(App.User.SalesRoom.srHoursDif), App.User.User.peID);

      //Actualizamos datos de UI
      updateDate(_closeType, _dateClose.Value);
    }
    #endregion

    #region updateDate
    /// <summary>
    /// Función para actualizar los controles DataPicker del UI 
    /// </summary>
    /// <history>
    /// [vipacheco] 02/03/2016 Created
    /// </history>
    private void updateDate(EnumEntities salesRoomType, DateTime? dateUpdate)
    {
      switch (salesRoomType)
      {
        case EnumEntities.Shows:
          dtpCloseShows.Value = _serverDate.AddDays(-1);
          dtpCloseShowsLast.Value = dateUpdate;
          break;
        case EnumEntities.MealTickets:
          dtpCloseMealTicket.Value = _serverDate.AddDays(-1);
          dtpCloseMealTicketsLast.Value = dateUpdate;
          break;
        case EnumEntities.Sales:
          dtpCloseSales.Value = _serverDate.AddDays(-1);
          dtpCloseSalesLast.Value = dateUpdate;
          break;
        case EnumEntities.GiftsReceipts:
          dtpCloseGiftsReceipts.Value = _serverDate.AddDays(-1);
          dtpCloseGiftsReceiptsLast.Value = dateUpdate;
          break;
      }
      UIHelper.ShowMessage("Closing process completed.", MessageBoxImage.Information, salesRoomType.ToString() + " Process");
    }
    #endregion

    #region validateUserPermissions
    /// <summary>
    /// Función que valida las opciones a mostrar segun el tipo de permisos que tenga el usuario.
    /// </summary>
    /// <param name="userDate"></param>
    /// <history>
    /// [vipacheco] 07/03/2016 Created
    /// </history>
    private void validateUserPermissions()
    {
      if (!App.User.HasPermission(EnumPermission.CloseSalesRoom, EnumPermisionLevel.Standard))
      {
        // Ocultamos los label's
        txbCloseShows.Visibility = Visibility.Collapsed;
        txbCloseMealTickets.Visibility = Visibility.Collapsed;
        txbCloseSales.Visibility = Visibility.Collapsed;
        txbCloseGiftsReceipts.Visibility = Visibility.Collapsed;

        // Ocultamos los datapicker's
        dtpCloseShows.Visibility = Visibility.Collapsed;
        dtpCloseGiftsReceipts.Visibility = Visibility.Collapsed;
        dtpCloseSales.Visibility = Visibility.Collapsed;
        dtpCloseMealTicket.Visibility = Visibility.Collapsed;

        //Ocultamos los botones
        btnCloseGiftsReceipts.Visibility = Visibility.Collapsed;
        btnCloseMealTickets.Visibility = Visibility.Collapsed;
        btnCloseSales.Visibility = Visibility.Collapsed;
        btnCloseShows.Visibility = Visibility.Collapsed;
        return;
      }

      dtpClose_Loaded();
    } 
    #endregion

    #endregion

  }
}
