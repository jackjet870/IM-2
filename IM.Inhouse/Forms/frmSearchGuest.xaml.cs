using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Inhouse.Classes;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IM.Inhouse.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchGuest.xaml
  /// </summary>
  public partial class frmSearchGuest : Window
  {
    #region Atributos

    EnumPrograms enumPrograms;
    Guest guest;
    LeadSource leadSource;
    public List<Guest> lstGuest;
    frmGuestsGroups parent;

    #endregion

    #region Propiedades

    public bool cancel { get; set; }
    public List<Guest> lstGuestAdd { get; set; }

    #endregion

    #region Metodos

    void LoadCombo()
    {
      cmbLeadSourse.ItemsSource = BRLeadSources.GetLeadSources(1);
    }

    /// <summary>
    /// Llena datos para crear la clausula Where.
    /// </summary>
    /// <param name="where">Clausula WHERE si ya se tiene predefinida (Experimental)</param>
    /// <history>[ECNUL] 31-03-2016 Created</history>
    void CreateWhere()
    {
      guest = new Guest();
      leadSource = new LeadSource();
      leadSource.lspg = StrToEnums.EnumProgramToString(enumPrograms);
      if (txtGUID.Text != "") //Si se puso id del Huesped
        guest.guID = Convert.ToInt32(txtGUID.Text.Trim());
      else
      {
        //Busca por nombre y apellido
        if (txtName.Text != "")
        {
          guest.guLastName1 = txtName.Text;
          guest.guFirstName1 = txtName.Text;
          guest.guLastname2 = txtName.Text;
          guest.guFirstName2 = txtName.Text;
        }
        else if (txtRoom.Text != "") //Numero de habitacion
          guest.guRoomNum = txtRoom.Text;
        else if (txtReservation.Text != "") //Folio de reservacion
          guest.guHReservID = txtReservation.Text;
        ///BUSQUEDAS WHERE OBLIGADAS
        //Between de Fecha de llegada Si no se tiene GUID Siempre se busca por fecha de llegada
        guest.guCheckInD = Convert.ToDateTime(dtpFrom.SelectedDate.Value.ToShortDateString());
        guest.guCheckOutD = Convert.ToDateTime(dtpTo.SelectedDate.Value.ToShortDateString()); //Deberia ser CheckInD tambien pero se usa este para hacer el between
        guest.guls = cmbLeadSourse.SelectedValue.ToString(); //Toma el LeadSource del Combo Obligado siempre se busca por LS
        //Se usa si se tiene un Where Antes
        //if (false)
        //  guest.guID = guest.guID;
      }


    }

    /// <summary>
    /// Carga El grid Con los paramntros de busqueda ya especificados
    /// </summary>
    void CargaGrid()
    {
      StaStart("Loading Guests...");
      CreateWhere();
      lstGuest = BRGuests.GetSearchGuestByLS(guest, leadSource);
      if (lstGuest.Count != 0)
        dtgGuests.ItemsSource = lstGuest;

      if (lstGuest.Count == 1)
        StatusBarReg.Content = lstGuest.Count + " Guest";
      else
        StatusBarReg.Content = lstGuest.Count + " Guests";

      StaEnd();
    }

    /// <summary>
    /// Carga la lista con los valores a retornar
    /// </summary>
    /// <history>[ECANUL] 01-04-2016 Created</history>
    void LoadReturnList()
    {
      StaStart("Loading Selected Guests...");
      lstGuest = dtgGuests.SelectedItems.OfType<Guest>().ToList();
      if (lstGuest.Count == 0)
        MessageBox.Show("No se han seleccionado Huespedes");
      //lstGuest = dtgGuests.SelectedItems
      StaEnd();
    }

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje a mostrar</param>
    /// <history>[ECANUL] 31-03-2016 Created </history>
    private void StaStart(String message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
    }

    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    /// <history>[ECANUL] 31-03-2016 Created </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
    }

    #endregion

    #region Contructores y Destructores
    public frmSearchGuest(EnumPrograms prog, frmGuestsGroups winParent)
    {
      InitializeComponent();
      enumPrograms = prog;
      parent = winParent;
    }
    #endregion
    
    #region Eventos del Formulario

    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      CargaGrid();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadCombo();
      DateTime dt = BRHelpers.GetServerDate();
      dtpTo.SelectedDate = dt;
      dtpFrom.SelectedDate = dt.AddDays(-7);
      cmbLeadSourse.SelectedValue = App.User.LeadSource.lsID;
      StatusBarReg.Content ="0 Guests";
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      LoadReturnList();
      lstGuestAdd = lstGuest;
      dtgGuests.ItemsSource = lstGuest;
      Close();
      cancel = false;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      cancel = true;
      Close();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }

    #endregion
    
  }
}
