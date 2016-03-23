using System;
using System.Windows;
using System.Windows.Controls;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.BusinessRules.BR;

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
    private Window _frmHost = null;
    private UserData _userData;
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
    public frmCloseSalesRoom(Window pHost, UserData userData, DateTime serverDate)
    {
      _userData = userData;
      _serverDate = serverDate;

      InitializeComponent();
      _frmHost = pHost;
      validateUserPermissions(_userData);
    }

    #region EVENTOS DE TIPO WINDOW
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      var _getSalesRoom = BRSalesRooms.GetSalesRoom(_userData.SalesRoom.srID);

      //Se agrega Binding a los controles correspondientes
      dtpCloseShowsLast.SelectedDate = _getSalesRoom.srShowsCloseD;
      dtpCloseMealTicketsLast.SelectedDate = _getSalesRoom.srMealTicketsCloseD;
      dtpCloseSalesLast.SelectedDate = _getSalesRoom.srSalesCloseD;
      dtpCloseGiftsReceiptsLast.SelectedDate = _getSalesRoom.srGiftsRcptCloseD;
    }

    /// <summary>
    /// Funcion para quitar el efecto de la ventana Host al momento de cerrar!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 01/03/2016 Created
    /// </history>
    private void Window_Closed(object sender, EventArgs e)
    {
      _frmHost.Effect = null;
    }
    #endregion

    #region EVENTOS DE TIPO BOTON
    private void btnCloseShows_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumSalesRoomType.Shows, ref dtpCloseShows, dtpCloseSalesLast);
    }

    private void btnCloseMealTickets_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumSalesRoomType.MealTickets, ref dtpCloseMealTicket, dtpCloseSalesLast);
    }

    private void btnCloseSales_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumSalesRoomType.Sales, ref dtpCloseSales, dtpCloseSalesLast);
    }

    private void btnCloseGiftsReceipts_Click(object sender, RoutedEventArgs e)
    {
      CloseSalesRoom(EnumSalesRoomType.GiftsReceipts, ref dtpCloseGiftsReceipts, dtpCloseSalesLast);
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
      frmSalesRoomsLog mfrmSalesRoomsLog = new frmSalesRoomsLog(_userData);
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
      dtpCloseShows.SelectedDate = _serverDate.AddDays(-1);
      dtpCloseGiftsReceipts.SelectedDate = _serverDate.AddDays(-1);
      dtpCloseSales.SelectedDate = _serverDate.AddDays(-1);
      dtpCloseMealTicket.SelectedDate = _serverDate.AddDays(-1);
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
    private void CloseSalesRoom(EnumSalesRoomType _closeType, ref DatePicker _dateClose, DatePicker _dateCloseLast)
    {
      //verificacion de fechas no mayores a la actual
      if (_dateClose.SelectedDate.Value > _serverDate)
      {
        MessageBox.Show("Closing date con not be greater than today.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        return;
      }
      //Se pregunta si en verdad desea realizar el cierre
      if (MessageBox.Show("Are you sure you want to close all  " + _closeType.ToString() + " until " + _dateClose.Text + "? You wont't be able to modify that " + _closeType.ToString() + " anymore.",
                          "Warning",
                          MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.Cancel)
        return;

      //Realizamos el cierre
      BRSalesRooms.SetCloseSalesRoom(_closeType, _userData.SalesRoom.srID, _dateClose.SelectedDate);

      //Guardamos la accion en el historico de sala de ventas
      BRSalesRoomsLogs.SaveSalesRoomLog(_userData.SalesRoom.srID, Convert.ToInt16(_userData.SalesRoom.srHoursDif), _userData.User.peID);

      //Actualizamos datos de UI
      updateDate(_closeType, _dateClose.SelectedDate);
    }

    /// <summary>
    /// Función para actualizar los controles DataPicker del UI 
    /// </summary>
    /// <param name="salesRoomType"> Tipo de entidad Sales Room </param>
    /// <param name="dateUpdate"> Fecha de Cierre </param>
    /// <history>
    /// [vipacheco] 02/03/2016 Created
    /// </history>
    private void updateDate(EnumSalesRoomType salesRoomType, DateTime? dateUpdate)
    {
      switch (salesRoomType)
      {
        case EnumSalesRoomType.Shows:
          dtpCloseShows.SelectedDate = _serverDate.AddDays(-1);
          dtpCloseShowsLast.SelectedDate = dateUpdate;
          break;
        case EnumSalesRoomType.MealTickets:
          dtpCloseMealTicket.SelectedDate = _serverDate.AddDays(-1);
          dtpCloseMealTicketsLast.SelectedDate = dateUpdate;
          break;
        case EnumSalesRoomType.Sales:
          dtpCloseSales.SelectedDate = _serverDate.AddDays(-1);
          dtpCloseSalesLast.SelectedDate = dateUpdate;
          break;
        case EnumSalesRoomType.GiftsReceipts:
          dtpCloseGiftsReceipts.SelectedDate = _serverDate.AddDays(-1);
          dtpCloseGiftsReceiptsLast.SelectedDate = dateUpdate;
          break;
      }
      MessageBox.Show("Closing process completed.", salesRoomType.ToString() + " Process", MessageBoxButton.OK, MessageBoxImage.Information);
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
