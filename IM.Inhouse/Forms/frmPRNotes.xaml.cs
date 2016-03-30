using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
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
using IM.BusinessRules.Entities;

namespace IM.Inhouse.Forms
{
  /// <summary>
  /// Interaction logic for frmPRNotes.xaml
  /// </summary>
  public partial class frmPRNotes : Window
  {
    #region Atributos
    private int _guestID;
    private Guest _guest;
    //si se va a buscar por Textbox el nombre del PR
    private bool _searchPRbyTxt;
    private CollectionViewSource _pRNoteViewSource;
    //si se esta creando la nota 
    private bool _CreatingNote = false;
    public bool _saveNote = false;
    #endregion

    #region Contructores y destructores
    public frmPRNotes(int guestID)
    {
      InitializeComponent();
      _guestID = guestID;
      //Cargamos los datos del huesped
      _guest = BRGuests.GetGuest(_guestID);

    }
    #endregion   

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      EnabledControls(true, true, false);
      //Cargamos los PR
      cbopnPR.ItemsSource = BRPersonnel.GetPersonnel(App.User.Location.loID, "ALL", "PR");
      // desplegamos los datos del huesped
      txtguID.Text = _guest.guID.ToString();
      txtguLastName1.Text = _guest.guLastName1;
      txtguFirstName1.Text = _guest.guFirstName1;
      txtguCheckInD.Text = _guest.guCheckInD.ToString();
      txtguCheckOutD.Text = _guest.guCheckOutD.ToString();

      //Cargamos el datagrid
      _pRNoteViewSource = ((CollectionViewSource)(this.FindResource("pRNoteViewSource")));
      _pRNoteViewSource.Source = BRNotes.GetNoteGuest(_guestID);
      //Si no tiene ninguna nota se inhabilita el control
      if (dgNotes.Items.Count < 1)
      {
        btnShowInfo.IsEnabled = false;
      }
    }
    #endregion

    #region AfterValidate
    //Permite guardar los datos extras de un registro
    public bool AfterValidate()
    {
      //Validamos la fecha y hora
      if (!ValidateHelper.ValidateRequired(txtpnDT, "date and time"))
      {
        return false;
      }
      //validamos que la fecha no sea mayor a 7 días posteriores a su salida
      if (Convert.ToDateTime(txtpnDT.Text) > Convert.ToDateTime(txtguCheckOutD.Text).AddDays(7))
      {
        UIHelper.ShowMessage("No notes are permitted after seven days after the departure.");
        return false;
      }
      //Validamos quien hizo  el cambio y su contraseña
      if (!ValidateHelper.ValidateChangedBy(txtpnPR, txtPwd, "PR"))
      {
        return false;
      }
      //Validamos nota
      if (!ValidateHelper.ValidateRequired(txtpnText, "note"))
      {
        txtpnText.Focus();
        return false;
      }

       ValidationData Validate =  BRHelpers.ValidateChangedByExist(txtpnPR.Text, EncryptHelper.Encrypt(txtPwd.Password), App.User.LeadSource.lsID, "PR").Single();

      //Validamos que los datos de quien hizo el cambio y su contraseña existan
      if (Validate.Focus != string.Empty)
      {
        //Desplegamos el mensaje de error
        UIHelper.ShowMessage(Validate.Message);

        //Esteblecemos el foco en el control que tiene el error
        switch (Validate.Focus)
        {
          case "ID":
            txtpnPR.Focus();          
            break;
          case "ChangedBy":
            txtpnPR.Focus();
            break;
          case "Password":
            txtPwd.Focus();
            break;
          case "PR":
            txtpnPR.Focus();
            break;
        } 
        return false;
      }
      ////si es la primera nota que se agrega
      //if (BRNotes.GetCountNoteGuest(_guestID) < 1)
      //{

      //}
      return true;
    } 
    #endregion 

    #region btnAdd_Click
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      CleanControls();
      txtpnPR.Text = App.User.User.peID;
      cbopnPR.SelectedValue = App.User.User.peID;
      txtPwd.Password = EncryptHelper.Encrypt(App.User.User.pePwd);
      EnabledControls(false, false, true);
      //ingresamos la fecha en el campo 
      txtpnDT.Text = BRHelpers.GetServerDate().ToString();
      _CreatingNote = true;
    }

    #endregion

    #region btnSave_Click
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      EnabledControls(true, true, false);
      if (AfterValidate())
      {
        //Agregamos la nota 
        PRNote note = new PRNote();
        note.pnDT = Convert.ToDateTime(txtpnDT.Text);
        note.pngu = _guestID;
        note.pnPR = txtpnPR.Text;
        note.pnText = txtpnText.Text;         
        BRNotes.SaveNoteGuest(note);
        //Actualizamos el guest 
        Guest guest = BRGuests.GetGuest(_guestID);
        guest.guPRNote = true;
        BRGuests.SaveGuest(guest);
        //Actualizamos el datagrid 
        _pRNoteViewSource.Source = BRNotes.GetNoteGuest(_guestID);
        CleanControls();
        _CreatingNote = false;
        _saveNote = true;
      }
      else
      {
        EnabledControls(false, false, true);
      }
    } 
    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      CleanControls();
      EnabledControls(true, true, false);
      _CreatingNote = false;
    } 
    #endregion

    #region CleanControls
    private void CleanControls()
    {
      txtpnDT.Text = txtpnPR.Text = txtPwd.Password = txtpnText.Text = string.Empty;
      cbopnPR.SelectedIndex = -1;
    } 
    #endregion

    #region cbopnPR_SelectionChanged
    private void cbopnPR_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (cbopnPR.SelectedIndex != -1 || txtpnPR.Text == string.Empty)
      {
        if (cbopnPR.SelectedValue != null)
        {
          if (!_searchPRbyTxt)
          {
            txtpnPR.Text = ((PersonnelShort)cbopnPR.SelectedItem).peID;
            txtPwd.Password = (App.User.User.peID != txtpnPR.Text ? string.Empty : EncryptHelper.Encrypt(App.User.User.pePwd));
          }
        }
        else
        {
          //txtguFollowD.Text = string.Empty;
        }
      }
    } 
    #endregion

    #region txtpnPR_LostFocus
    private void txtpnPR_LostFocus(object sender, RoutedEventArgs e)
    {
      _searchPRbyTxt = true;
      if (txtpnPR.Text != string.Empty)
      {
        // validamos que el motivo de indisponibilidad exista en los activos
        Personnel PR = BRPersonnel.GetPersonnelById(txtpnPR.Text);
        if (PR == null)
        {
          UIHelper.ShowMessage("The PR not exist");
          txtpnPR.Text = string.Empty;
          txtpnPR.Focus();
        }
        else
        {
          cbopnPR.SelectedValue = txtpnPR.Text;
        }
      }
      else
      {
        cbopnPR.SelectedIndex = -1;
      }
      //Si es el mismo usuario que esta actualmente logueado entonces le agregamos la contraseña y la desencriptamos 
      //de no ser así lo dejamos vacio
      txtPwd.Password = (App.User.User.peID != txtpnPR.Text ? string.Empty : EncryptHelper.Encrypt(App.User.User.pePwd));
      _searchPRbyTxt = false;
    }
    #endregion

    #region btnShowInfo_Click
    private void btnShowInfo_Click(object sender, RoutedEventArgs e)
    {
      loadControls();
    }
    #endregion

    #region dgNotes_MouseDoubleClick
    private void dgNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      loadControls();
    }
    #endregion

    #region loadControls
    public void loadControls()
    {
      if (dgNotes.SelectedItems.Count == 1)
      {
        var item = dgNotes.SelectedItem as NoteGuest;
        //var item = dgNotes.Items.GetItemAt(dgNotes.Items.IndexOf(dgNotes.CurrentItem)) as Note;
        cbopnPR.SelectedValue = item.PR;
        txtpnPR.Text = item.PR;
        //txtPwd.Password = BRPersonnel.GetPersonnelById(item.PR).pePwd;
        txtPwd.Password = (App.User.User.peID != txtpnPR.Text ? string.Empty : EncryptHelper.Encrypt(App.User.User.pePwd));
        txtpnText.Text = item.Text;
        txtpnDT.Text = item.Date.ToString();
        EnabledControls(true,true,false);
      }
    }
    #endregion

    #region EnabledControls
    public void EnabledControls(bool show, bool ctrluno, bool ctrldos)
    {
      txtpnDT.IsReadOnly = txtpnPR.IsReadOnly = txtpnText.IsReadOnly = ctrluno;
      txtPwd.IsEnabled = cbopnPR.IsEnabled = btnSave.IsEnabled = btnCancel.IsEnabled = ctrldos;
      btnShowInfo.IsEnabled = btnAdd.IsEnabled = show;
    }
    #endregion

    #region Window_Closing
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (_CreatingNote)
      {
        UIHelper.ShowMessage("This form is currently in edit mode. Please save or undo your changes before closing it.", MessageBoxImage.Asterisk, "Can´t Close");
        e.Cancel = true;
      }
    }
    #endregion

    //valida los datos extras de un registro

    //Valida que la nota no se repita
    public void BeforeValidateID()
    {
      //establecemos la fecha y hora
      txtpnDT.Text = BRHelpers.GetServerDate().ToLongDateString();

      //Establecemos los clieterios de busqueda

    }
    public void SetSearchCriteria(string pstrWhere, DateTime pstrDateTime)
    {
      if (BRNotes.GetCountNoteGuest(_guestID) == 0)
      {

      }
      
    }
  }

  
}
