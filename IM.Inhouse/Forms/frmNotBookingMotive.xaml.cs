using System.Windows;
using System.Windows.Controls;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using System.Collections.Generic;

namespace IM.Inhouse.Forms
{
  /// <summary>
  ///Formulario que sirve para indicar por que no se invito el huesped y así invitarlo si no ha pasado mas de un mes 
  /// desde que hizo Check Out 
  /// </summary>
  /// <hostory>
  /// [JORCANCHE] 11/AGO/2016
  /// </hostory>
  public partial class frmNotBookingMotive : Window
  {

    #region Variables
    public bool Invit { get; set; }
    public bool Save { get; set; }

    private readonly int _guestId;

    private Guest _guest;

    private UserData _userData;
    #endregion

    #region Constructor
    public frmNotBookingMotive(int guesId)
    {
      InitializeComponent();
      _guestId = guesId;
      Title = $"Not Booking Motive - Guest ID: {guesId}";
    } 
    #endregion

    #region FrmNotBookingMotive_OnLoaded
    /// <summary>
    /// Carga e inicializa las variables
    /// </summary>
    /// <history>
    /// [jorcanche]  created 11/08/2016
    /// </history>
    private async void FrmNotBookingMotive_OnLoaded(object sender, RoutedEventArgs e)
    {
      Invit = false;
      Save = false;
      lblUserName.Text = App.User.User.peN;
      //Cargamos los Controles y el Guest
      _guest = await BRGuests.GetGuest(_guestId);
      cbmgunb.ItemsSource = await BRNotBookingMotives.GetNotBookingMotives(1);
      cbmguPRNoBook.ItemsSource = await BRPersonnel.GetPersonnel(App.User.Location.loID, "ALL", "PR");
      //Asignar valores
      if (_guest.guNoBookD != null) txtguNoBookD.Text = _guest.guNoBookD.Value.ToShortDateString();
      cbmgunb.SelectedValue = _guest.gunb;
      cbmguPRNoBook.SelectedValue = _guest.guPRNoBook;

      // establecemos el modo lectura
      SetMode(EnumMode.ReadOnly);
    }
    #endregion

    #region btnEdit_Click
    /// <summary>
    /// Prepara el formulario para modificar los datos
    /// </summary>
    /// <history>
    /// [jorcanche]  created 11/08/2016
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      var login = new frmLogin(switchLoginUserMode: true);
      if (App.User.AutoSign)
      {
        login.UserData = App.User;
      }
      login.ShowDialog();
      if (!login.IsAuthenticated) return;
      _userData = login.UserData;
      //Nombre del usuario
      lblUserName.Text = _userData.User.peN;
      //Establecemos el modo de edicion
      SetMode(EnumMode.Edit);
    }
    #endregion

    #region SetMode
    /// <summary>
    /// Habilita / deshabilita los controles del formulario segun el modo de datos
    /// </summary>
    /// <param name="enumMode">Enumerado que indica si esta en modo de edicion o visual</param>
    /// <history>
    /// [jorcanche]  created 11/08/2016
    /// </history>
    private void SetMode(EnumMode enumMode)
    {
      var enable = enumMode != EnumMode.ReadOnly;
      //Controles del detalle 
      txtguNoBookD.IsEnabled = cbmgunb.IsEnabled = cbmguPRNoBook.IsEnabled = enable;

      //Si se debe de habilitar
      if (enable)
      {

        //Fecha en que se definio el motivo de no booking
        if (string.IsNullOrEmpty(txtguNoBookD.Text))
        {
          txtguNoBookD.Text = BRHelpers.GetServerDate().ToShortDateString();
          _guest.guNoBookD = BRHelpers.GetServerDate();
        }

        //PR que modifico el motivo de no booking
        cbmguPRNoBook.SelectedValue = _userData.User.peID;
        _guest.guPRNoBook = _userData.User.peID;


        //No se permite modificar la fecha ni el PR de no Booking
        txtguNoBookD.IsEnabled = false;
        cbmguPRNoBook.IsEnabled = false;
      }

      //Botones
      btnEdit.IsEnabled = !enable;
      btnCancel.IsEnabled = btnSave.IsEnabled = enable;
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    /// Permite guardar los cambios 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 11/08/2016
    /// </history>
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      //Validamos el Motivo de no Booking
      var validate = ValidateHelper.ValidateForm(grdNotBookingMotive, "Booking");
      if (!string.IsNullOrEmpty(validate))
      {
        UIHelper.ShowMessage(validate);
        return;
      }
      _guest.gunb = (int)cbmgunb.SelectedValue;
      await BREntities.OperationEntity(_guest, EnumMode.Edit);
      Save = true;
      Close();
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <history>
    /// [jorcanche]  created 11/08/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
    #endregion

    #region btnInvit_Click
    /// <summary>
    /// Indica si abrira el formulario de Invit
    /// </summary>
    /// <history>
    /// [jorcanche]  created 11/08/2016
    /// </history>
    private void btnInvit_Click(object sender, RoutedEventArgs e)
    {
      Invit = true;
    } 
    #endregion

  }
}
