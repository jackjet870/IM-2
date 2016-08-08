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
    //private UserData _userData;
    private DateTime _serverDate;
    #endregion

    /// <summary>
    /// Constructor de la ventana
    /// </summary>
    /// <param name="pHost"></param>
    /// <param name="userData"></param>
    /// <param name="serverDate"></param>
    /// <history>
    /// [vipacheco] 27/02/2106 Created
    /// </history>
    public frmCloseSalesRoom(DateTime serverDate)
    {
      //_userData = UserData;
      _serverDate = serverDate;

      InitializeComponent();      validateUserPermissions(App.User);
    }

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
    private void btnCloseShows_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumEntities.Shows, ref dtpCloseShows, dtpCloseSalesLast);
    }

    private void btnCloseMealTickets_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumEntities.MealTickets, ref dtpCloseMealTicket, dtpCloseSalesLast);
    }

    private void btnCloseSales_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumEntities.Sales, ref dtpCloseSales, dtpCloseSalesLast);
    }

    private void btnCloseGiftsReceipts_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumEntities.GiftsReceipts, ref dtpCloseGiftsReceipts, dtpCloseSalesLast);
    }

    /// <summary>
    /// Funcion del evento btnLog que se encarga de desplegar el frmSalesRoomsLog
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 27/02/2016 Created
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      frmSalesRoomsLog mfrmSalesRoomsLog = new frmSalesRoomsLog();
      mfrmSalesRoomsLog.ShowInTaskbar = false;
      mfrmSalesRoomsLog.Owner = this;
      mfrmSalesRoomsLog.ShowDialog();
    }

    #endregion

    #region EVENTO DE TIPO DATAPICKER
    /// <summary>
    /// Funcion que carga los calendarios close con un dia anterior
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Función para actualizar los controles DataPicker del UI 
    /// </summary>
    /// <param name="salesRoomType"> Tipo de entidad Sales Room </param>
    /// <param name="dateUpdate"> Fecha de Cierre </param>
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

    /// <summary>
    /// Función que valida las opciones a mostrar segun el tipo de permisos que tenga el usuario.
    /// </summary>
    /// <param name="userDate"></param>
    /// <history>
    /// [vipacheco] 07/03/2016 Created
    /// </history>
    private void validateUserPermissions(UserData userDate)
    {
      if (userDate.Permissions.Exists(c => c.pppm == "CLOSESR" && c.pppl == 1))
      {
        // Ocultamos los label's
        lblCloseShows.Visibility = Visibility.Collapsed;
        lblCloseMealTickets.Visibility = Visibility.Collapsed;
        lblCloseSales.Visibility = Visibility.Collapsed;
        lblCloseGiftsReceipts.Visibility = Visibility.Collapsed;

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

  }
}
