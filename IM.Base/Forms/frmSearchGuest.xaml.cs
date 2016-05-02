using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Classes;
using IM.Model.Classes;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchGuest.xaml
  /// </summary>
  public partial class frmSearchGuest : Window
  {
    #region Atributos

    EnumProgram _program;
    Guest _guest;
    LeadSource _leadSource;
    private List<Guest> _lstGuests;
    private UserData user; 

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
      _guest = new Guest();
      _leadSource = new LeadSource();
      _leadSource.lspg = EnumToListHelper.GetEnumDescription(_program);
      if (txtGUID.Text != "") //Si se puso id del Huesped
        _guest.guID = Convert.ToInt32(txtGUID.Text.Trim());
      else
      {
        //Busca por nombre y apellido
        if (txtName.Text != "")
        {
          _guest.guLastName1 = txtName.Text;
          _guest.guFirstName1 = txtName.Text;
          _guest.guLastname2 = txtName.Text;
          _guest.guFirstName2 = txtName.Text;
        }
        else if (txtRoom.Text != "") //Numero de habitacion
          _guest.guRoomNum = txtRoom.Text;
        else if (txtReservation.Text != "") //Folio de reservacion
          _guest.guHReservID = txtReservation.Text;
        ///BUSQUEDAS WHERE OBLIGADAS
        //Between de Fecha de llegada Si no se tiene GUID Siempre se busca por fecha de llegada
        _guest.guCheckInD = Convert.ToDateTime(dtpFrom.SelectedDate.Value.ToShortDateString());
        _guest.guCheckOutD = Convert.ToDateTime(dtpTo.SelectedDate.Value.ToShortDateString()); //Deberia ser CheckInD tambien pero se usa este para hacer el between
        _guest.guls = cmbLeadSourse.SelectedValue.ToString(); //Toma el LeadSource del Combo Obligado siempre se busca por LS
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
      _lstGuests = BRGuests.GetSearchGuestByLS(_guest, _leadSource);
      if (_lstGuests.Count != 0)
        dtgGuests.ItemsSource = _lstGuests;

      if (_lstGuests.Count == 1)
        StatusBarReg.Content = _lstGuests.Count + " Guest";
      else
        StatusBarReg.Content = _lstGuests.Count + " Guests";

      StaEnd();
    }

    /// <summary>
    /// Carga la lista con los valores a retornar
    /// </summary>
    /// <history>[ECANUL] 01-04-2016 Created</history>
    void LoadReturnList()
    {
      StaStart("Loading Selected Guests...");
      _lstGuests = dtgGuests.SelectedItems.OfType<Guest>().ToList();
      if (_lstGuests.Count == 0)
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
    public frmSearchGuest(UserData userdata, EnumProgram program = EnumProgram.All)
    {
      InitializeComponent();
      _program = program;
      user = userdata;
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
      cmbLeadSourse.SelectedValue = user.LeadSource.lsID;
      StatusBarReg.Content ="0 Guests";
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      LoadReturnList();
      lstGuestAdd = _lstGuests;
      dtgGuests.ItemsSource = _lstGuests;
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
