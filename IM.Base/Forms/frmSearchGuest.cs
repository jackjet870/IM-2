using System;
using System.Windows;
using IM.BusinessRules.BR;
using IM.Model.Classes;
using IM.Model;
using System.Collections.Generic;
using IM.Model.Enums;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchGuest.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 09/Feb/2016 Created
  /// </history>
  public partial class frmSearchGuest : Window
  {
    #region Atributos
    UserData _user;

    #endregion

    #region Atributo Público
    public Guest guestAdditional = null;
    #endregion

    #region Constructores y destructores
    public frmSearchGuest(UserData user)
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      _user = user;
      InitializeComponent();
    }
    #endregion

    #region Métodos de la forma

    /// <summary>
    /// Configura los elementos de la forma
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void frmSearchGuest_Loaded(object sender, RoutedEventArgs e)
    {
      ConfiguraFechas();
      CargaLeadSources();

    }

    /// <summary>
    /// Aplica los filtros para la búsqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      if (Validate())
      {
        LoadGrid();
      }
    }

    /// <summary>
    /// Revisa que los textbox númericos solo resivan números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private new void PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
      if (e.Text == ".")
        e.Handled = false;
      else if (!char.IsDigit(e.Text, e.Text.Length - 1))
        e.Handled = true;

    }

    private void dtgGuests_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      guestAdditional = (Guest)dtgGuests.CurrentItem;
      this.DialogResult = true;
      this.Close();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      guestAdditional = (Guest)dtgGuests.SelectedItem;
      this.DialogResult = true;
      this.Close();
    }

    /// <summary>
    /// Cierra la forma
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    #endregion
    
    #region Métodos Privados
    private void ConfiguraFechas()
    {
      dtpArrivalFrom.Text = DateTime.Today.AddDays(-7).ToShortDateString();
      dtpArrivalTo.Text = DateTime.Today.ToShortDateString();
    }

    private void CargaLeadSources()
    {
      var ls = BRLeadSources.GetLeadSourcesByUser(_user.User.peID, EnumProgram.Inhouse);
      cmbLeadSources.DisplayMemberPath = "lsN";
      cmbLeadSources.SelectedValuePath = "lsID";
      cmbLeadSources.ItemsSource = ls;
    }

    private void LoadGrid()
    {
      var lstGuest = new List<Guest>();
      if(!String.IsNullOrEmpty(txtGuid.Text))
      {
        var guest = BusinessRules.BR.BRGuests.GetGuestById(int.Parse(txtGuid.Text));
        lstGuest.Add(guest);
      }
      else
      {
        var guests = BusinessRules.BR.BRGuests.GetSearchGuest(cmbLeadSources.SelectedValue.ToString()
                                                              , txtName.Text.ToUpper()
                                                              , txtRoom.Text
                                                              , txtReservation.Text
                                                              , dtpArrivalFrom.SelectedDate.Value
                                                              , dtpArrivalTo.SelectedDate.Value
                                                              , "IH");
        lstGuest.AddRange(guests);
      }

      dtgGuests.ItemsSource = lstGuest;
    }

    private bool Validate()
    {
      bool res = true;

      if (cmbLeadSources.SelectedIndex == -1 && String.IsNullOrEmpty(txtName.Text) && String.IsNullOrEmpty(txtRoom.Text) && String.IsNullOrEmpty(txtReservation.Text) && String.IsNullOrEmpty(txtGuid.Text))
      {
        Helpers.UIHelper.ShowMessage("Please specify a search criteria.");
        res = false;
        cmbLeadSources.Focus();
      }
      else if (!ValidateDates())
      {
        res = false;
      }

      return res;
    }

    private bool ValidateDates()
    {
      bool res = true;
      
      if(!dtpArrivalFrom.SelectedDate.HasValue) // validamos la fecha inicial
      {
        Helpers.UIHelper.ShowMessage("Specify the Start Date.");
        res = false;
        dtpArrivalFrom.Focus();
      }
      else if(!dtpArrivalTo.SelectedDate.HasValue)// validamos la fecha final
      {
        Helpers.UIHelper.ShowMessage("Specify the End Date.");
        res = false;
        dtpArrivalTo.Focus();
      }
      else if(dtpArrivalFrom.SelectedDate.Value > dtpArrivalTo.SelectedDate.Value)//validamos que no se traslapen las fechas
      {
        Helpers.UIHelper.ShowMessage("Start Date can not be greater than End Date.");
        res = false;
        dtpArrivalFrom.Focus();
      }
      return res;
    }

    #endregion
  }
}
