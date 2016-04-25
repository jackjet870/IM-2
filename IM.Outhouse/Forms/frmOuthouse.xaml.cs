using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.Model.Classes;
using System.Windows.Input;
using System.Linq;
using IM.Base.Forms;
using IM.Model;

namespace IM.Outhouse.Forms
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class frmOuthouse
  {
    #region Atributos

    private CollectionViewSource _outPremanifestViewSource;
    private DateTime _serverDate;
    private bool _bookInvit = true;

    #endregion

    public frmOuthouse()
    {
      InitializeComponent();
    }

    #region ValidateContact
    /// <summary>
    /// Valida los datos para desplegar el formulario de contactacion
    /// </summary>
    /// <param name="CheckIn">Si ya hizo CheckIn</param>
    /// <param name="Contact"> Si ya esta contactado</param>
    /// <param name="CheckOutD">Fecha de contactación</param>
    /// <returns></returns>
    ///<history>[jorcanche] 13/03/2016</history>
    private bool ValidateContact(bool CheckIn, bool Contact, DateTime CheckOutD)
    {
      //validamos que el huesped haya hecho Check In
      if (!CheckIn)
      {
        UIHelper.ShowMessage("Guest has not made Check-in.", MessageBoxImage.Asterisk);
        return false;
      }
      // no se permite contactar si ya hizo Check Out o si ya esta contactado el Guest
      if (!Contact && CheckOutD < BRHelpers.GetServerDate().Date)
      {
        UIHelper.ShowMessage("Guest already made Check-out.", MessageBoxImage.Asterisk);
        return false;
      }
      //validamos que el usuario tenga permiso de lectura
      if (!App.User.HasPermission(EnumPermission.Register, EnumPermisionLevel.ReadOnly))
      {
        UIHelper.ShowMessage("Access denied.", MessageBoxImage.Asterisk);
        return false;
      }
      return true;
    }
    #endregion

    private void dtpDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      //Obtener el valor actual del que tiene dtpDate
      var picker = sender as Xceed.Wpf.Toolkit.DateTimePicker;
      if (!picker.Value.HasValue)
      {
        //cuando el ususario ingresa un afecha invalida
        MessageBox.Show("Specify  the Date", "Date invalidates", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //Y le asignamos la fecha del servidor (la hora actual)
        dtpDate.Value = BRHelpers.GetServerDate();
      }
      else
      {
        //le asignamos el valor del dtpDate a la variable global para que otro control tenga acceso al valor actual
        _serverDate = picker.Value.Value;
        //Cargamos el grid 
        LoadGrid();
      }
    }
    #region LoadGrid
    private void LoadGrid()
    {
      if (_outPremanifestViewSource != null)
        _outPremanifestViewSource.Source = BRGuestOutPremanifest.GetGuestOutPremanifest(_bookInvit, _serverDate, App.User.Location.loID);
    }
    #endregion

    private void rbt_Checked(object sender, RoutedEventArgs e)
    {
      var rb = sender as RadioButton;
      _bookInvit = Convert.ToBoolean(rb.TabIndex);
      LoadGrid();
    }

    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
      LoadGrid();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Cargamos las variables del usuario
      txtUser.Text = App.User.User.peN;
      txtLocation.Text = App.User.Location.loN;

      //Cargamos la fecha actual del servidor
      dtpDate.Value = BRHelpers.GetServerDate().Date;

      //Inicializamos la variable del datagrid
      _outPremanifestViewSource = ((CollectionViewSource)(this.FindResource("outPremanifestViewSource")));

      //Cargamos el DataGrid  
      LoadGrid();
    }
    private void Info_Click(object sender, RoutedEventArgs e)
    {
      var chkguInfo = sender as CheckBox;
      var OutPre = dgGuestPremanifest.Items[dgGuestPremanifest.Items.CurrentPosition] as OutPremanifest;
      //Invertimos el valor del Check para que no se refleje si no que hasta que se halla terminado la solicitud 
      chkguInfo.IsChecked = !chkguInfo.IsChecked.Value;
      if (ValidateContact(OutPre.guCheckIn, OutPre.guInfo, OutPre.guCheckOutD))
      {
        StaStart("Loading Contact´s Info...");
        frmContact frmCont = new frmContact(OutPre.guID, App.User);
        frmCont.Owner = this;
        frmCont.ShowInTaskbar = false;
        StaEnd();
        if (!frmCont.ShowDialog().Value)
        {
          if (frmCont._wasSave)
          {
            StaStart("Save Contact's Info...");
             dgGuestPremanifest.SelectedItems.OfType<OutPremanifest>().ToList().ForEach(item =>
            { item.guPRInfo = frmCont.PRInfo; item.guInfoD = frmCont.InfoD; item.guCheckIn = true; item.guInfo = true; });
            dgGuestPremanifest.Items.Refresh();
            StaEnd();
          }
        }
        StaEnd();
      }
      StaEnd();
    }

    #region StaStart
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje a mostrar</param>
    /// <history>[jorcanche] 05/04/2016 Created </history>
    private void StaStart(string message)
    {
      lblStatusBarMessage.Text = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
    }
    #endregion

    #region StaEnd
    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    /// <history>[jorcanche] 05/04/2016 Created</history>
    private void StaEnd()
    {
      lblStatusBarMessage.Text = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
    }
    #endregion

    private void guCommentsColumnArrival_LostFocus(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;      
      var Row = dgGuestPremanifest.SelectedItem as OutPremanifest;
      Guest pre = BRGuests.GetGuest(Row.guID);
      pre.guComments = txt.Text;
      BRGuests.SaveGuest(pre);
    }

    private void guCommentsColumnArrival_Loaded(object sender, RoutedEventArgs e)
    {
      var txt = sender as TextBox;
      txt.Focus();
    }
  }
}


