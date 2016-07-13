using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model.Classes;

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
    }
    #endregion

    #region Save
    public async void Save()
    {
      EnabledControls(true, true, false);
      if (AfterValidate())
      {
        //Agregamos la nota 
        var note = new PRNote
        {
          pnDT = Convert.ToDateTime(txtpnDT.Text),
          pngu = _guestID,
          pnPR = txtpnPR.Text,
          pnText = txtpnText.Text
        };

        //Actualizamos el guest si no tiene ninguna nota
        Guest guest = null;
        if (! await BRNotes.GetCountNoteGuest(_guestID))
        {
          guest = await BRGuests.GetGuest(_guestID);
          guest.guPRNote = true;
          _saveNote = true;
        }
        else
        {
          _saveNote = false;
        }

        //Enviamos los parametros para que agregue la nueva nota y modificamos el guest.
        //Si hubo un erro al ejecutar el metodo SaveNoteGuest nos devolvera 0, indicando que ningun paso 
        //se realizo, es decir ni se agrego la nota y se modifico el guest, y siendo así ya no modificamos la variable
        //_saveNote que es el que indica que se guardo el Avail.
        if ( await BRNotes.SaveNoteGuest(note, guest) != 0)
        {
          //Actualizamos el datagrid 
          _pRNoteViewSource.Source = await BRNotes.GetNoteGuest(_guestID);
        }
        else
        {
          //De no ser así informamos que no se guardo la información por algun motivo
          UIHelper.ShowMessage("There was an error saving the information, consult your system administrator",
            MessageBoxImage.Error, "Information can not keep");
        }
        CleanControls();
        _CreatingNote = false;
      }
      else
      {
        EnabledControls(false, false, true);
      }
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga e incializa las variables
    /// </summary>
    /// <history>
    /// [jorcanche] 02/02/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Cargamos los datos del huesped
      _guest = await BRGuests.GetGuest(_guestID);
      EnabledControls(true, true, false);
      //Cargamos los PR
      cbopnPR.ItemsSource = await BRPersonnel.GetPersonnel(App.User.Location.loID, "ALL", "PR");
      // desplegamos los datos del huesped
      txtguID.Text = _guest.guID.ToString();
      txtguLastName1.Text = _guest.guLastName1;
      txtguFirstName1.Text = _guest.guFirstName1;
      txtguCheckInD.Text = _guest.guCheckInD.ToString("dd/MM/yyyy");
      txtguCheckOutD.Text = _guest.guCheckOutD.ToString("dd/MM/yyyy");

      //Cargamos el datagrid
      _pRNoteViewSource = (CollectionViewSource)FindResource("pRNoteViewSource");
      _pRNoteViewSource.Source = await BRNotes.GetNoteGuest(_guestID);
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

      ValidationData Validate = BRHelpers.ValidateChangedByExist(txtpnPR.Text, EncryptHelper.Encrypt(txtPwd.Password), App.User.LeadSource.lsID, "PR").Single();

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

      if (App.User.AutoSign)
      {
        txtpnPR.Text = App.User.User.peID;
        cbopnPR.SelectedValue = App.User.User.peID;
        txtPwd.Password = App.User.User.pePwd;
      }
      EnabledControls(false, false, true);
      //ingresamos la fecha en el campo 
      txtpnDT.Text = BRHelpers.GetServerDateTime().ToString();
      txtpnText.Focus();
      _CreatingNote = true;
    }

    #endregion

    #region btnSave_Click
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      Save();
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
            Pass();
          }
        }
        else
        {
          //txtguFollowD.Text = string.Empty;
        }
      }
    }
    #endregion

    #region Pass
    public void Pass()
    {
      txtPwd.Password = (App.User.AutoSign ? (App.User.User.peID != txtpnPR.Text ? string.Empty : App.User.User.pePwd) : string.Empty);
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
      //Si es el mismo usuario que esta actualmente logueado y si puso autosing entonces le agregamos la contraseña y la desencriptamos 
      //de no ser así lo dejamos vacio
      Pass();
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
        Pass();
        txtpnText.Text = item.Text;
        txtpnDT.Text = item.Date.ToString();
        EnabledControls(true, true, false);
      }
    }
    #endregion

    #region EnabledControls
    public void EnabledControls(bool show, bool ctrluno, bool ctrldos)
    {
      txtpnDT.IsReadOnly = true;
      txtpnPR.IsReadOnly = txtpnText.IsReadOnly = ctrluno;
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

    #region btnSave_KeyDown
    /// <summary>
    /// Si es precionado la tecla entre inicia las validiaciones
    /// </summary>
    /// <param name="e"></param>
    /// <history>
    /// [jorcanche] 01/04/2016
    /// </history>
    private void txtpnText_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter && !string.IsNullOrEmpty(txtpnDT.Text))
      {
        Save();
      }
    }
    #endregion
  }
}
