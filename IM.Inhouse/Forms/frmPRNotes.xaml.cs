using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace IM.Inhouse.Forms
{
  /// <summary>
  /// Formulario que sirve para gestionar la notas por cada Guest
  /// </summary>
  /// <history>
  /// [jorcanche] created 07/04/2016
  /// </history>
  public partial class frmPRNotes : Window
  {
    #region Atributos

    private readonly int _guestId;
    private Guest _guest;
    //si se va a buscar por Textbox el nombre del PR
    private bool _searchPRbyTxt;
    private CollectionViewSource _pRNoteViewSource;
    //si se esta creando la nota 
    private bool _creatingNote;
    public bool SaveNote;
 
    #endregion

    #region Contructores y destructores

    public frmPRNotes(int guestId)
    {
      InitializeComponent();
      _guestId = guestId;
    }

    #endregion

    #region Save

    /// <summary>
    /// Guarda la nota 
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    public async void Save()
    {
      try
      {
        EnabledControls(true, true, false);
        if (AfterValidate())
        {
          //Agregamos la nota 
          var note = new PRNote
          {
            pnDT = Convert.ToDateTime(txtpnDT.Text),
            pngu = _guestId,
            pnPR = txtpnPR.Text,
            pnText = txtpnText.Text
          };

          //Actualizamos el guest si no tiene ninguna nota
          Guest guest = null;
          if (!await BRNotes.GetCountNoteGuest(_guestId))
          {
            guest = await BRGuests.GetGuest(_guestId);
            guest.guPRNote = true;
            SaveNote = true;
          }
          else
          {
            SaveNote = false;
          }
          //Enviamos los parametros para que agregue la nueva nota y modificamos el guest.
          //Si hubo un erro al ejecutar el metodo SaveNoteGuest nos devolvera 0, indicando que ningun paso 
          //se realizo, es decir ni se agrego la nota y se modifico el guest, y siendo así ya no modificamos la variable
          //_saveNote que es el que indica que se guardo el Avail.
          if (await BRNotes.SaveNoteGuest(note, guest) != 0)
          {
            //Actualizamos el datagrid 
            _pRNoteViewSource.Source = await BRNotes.GetNoteGuest(_guestId);
            DgNotes_OnSelectedCellsChanged(dgNotes, null);
          }
          else
          {
            //De no ser así informamos que no se guardo la información por algun motivo
            UIHelper.ShowMessage("There was an error saving the information, consult your system administrator",
              MessageBoxImage.Error, "Information can not keep");
          }
          CleanControls();
          _creatingNote = false;
        }
        else
        {
          EnabledControls(false, false, true);
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
      //Indicamos al statusbar que me muestre cierta informacion cuando oprimimos cierto teclado
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
      //Cargamos los datos del huesped
      _guest = await BRGuests.GetGuest(_guestId);
      EnabledControls(true, true, false);
      //Cargamos los PR
      cbopnPR.ItemsSource = await BRPersonnel.GetPersonnel(Context.User.Location.loID, "ALL", "PR");
      // desplegamos los datos del huesped
      txtguID.Text = _guest.guID.ToString();
      txtguLastName1.Text = _guest.guLastName1;
      txtguFirstName1.Text = _guest.guFirstName1;
      txtguCheckInD.Text = _guest.guCheckInD.ToString("dd/MM/yyyy");
      txtguCheckOutD.Text = _guest.guCheckOutD.ToString("dd/MM/yyyy");

      //Cargamos el datagrid
      _pRNoteViewSource = (CollectionViewSource) FindResource("pRNoteViewSource");
      _pRNoteViewSource.Source = await BRNotes.GetNoteGuest(_guestId);
      //Si no tiene ninguna nota se inhabilita el control
      if (dgNotes.Items.Count < 1)
      {
        btnShowInfo.IsEnabled = false;
      }
    }

    #endregion

    #region AfterValidate

    /// <summary>
    /// Permite guardar los datos extras de un registro
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    public bool AfterValidate()
    {
      //Validamos la fecha y hora
      if (!ValidateHelper.ValidateRequired(txtpnDT, "date and time"))
      {
        return false;
      }
      //validamos que la fecha no sea mayor a 7 días posteriores a su salida
      if (Convert.ToDateTime(txtpnDT.Text).Date > Convert.ToDateTime(txtguCheckOutD.Text).AddDays(7))
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

      var validate =
        BRHelpers.ValidateChangedByExist(txtpnPR.Text, EncryptHelper.Encrypt(txtPwd.Password), Context.User.LeadSource.lsID,
          "PR").Single();

      //Validamos que los datos de quien hizo el cambio y su contraseña existan
      if (validate.Focus != string.Empty)
      {
        //Desplegamos el mensaje de error
        UIHelper.ShowMessage(validate.Message);

        //Esteblecemos el foco en el control que tiene el error
        switch (validate.Focus)
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
      return true;
    }

    #endregion

    #region btnAdd_Click

    /// <summary>
    /// Agrega una nueva Nota
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      CleanControls();
      if (Context.User.AutoSign)
      {
        txtpnPR.Text = Context.User.User.peID;
        cbopnPR.SelectedValue = Context.User.User.peID;
        txtPwd.Password = Context.User.User.pePwd;
      }
      EnabledControls(false, false, true);
      //ingresamos la fecha en el campo 
      txtpnDT.Text = BRHelpers.GetServerDateTime().ToString("dd/MM/yyyy hh:mm:ss tt");
      txtpnText.Focus();
      _creatingNote = true;
    }

    #endregion

    #region btnSave_Click

    /// <summary>
    /// Guarda una nota
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      Save();
    }

    #endregion

    #region btnCancel_Click

    /// <summary>
    /// Cancela una Nota
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      CleanControls();
      EnabledControls(true, true, false);
      _creatingNote = false;
    }

    #endregion

    #region CleanControls

    /// <summary>
    /// Limpia los controles
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    private void CleanControls()
    {
      txtpnDT.Text = txtpnPR.Text = txtPwd.Password = txtpnText.Text = string.Empty;
      cbopnPR.SelectedIndex = -1;
    }

    #endregion

    #region cbopnPR_SelectionChanged

    /// <summary>
    /// Contrala el combobox y su seleccion
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    private void cbopnPR_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (cbopnPR.SelectedIndex != -1 || txtpnPR.Text == string.Empty)
      {
        if (cbopnPR.SelectedValue != null)
        {
          if (!_searchPRbyTxt)
          {
            txtpnPR.Text = ((PersonnelShort) cbopnPR.SelectedItem).peID;
            Pass();
          }
        }   
      }
    }

    #endregion

    #region Pass

    /// <summary>
    /// Configura el password
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    public void Pass()
    {
      txtPwd.Password = (Context.User.AutoSign
        ? (Context.User.User.peID != txtpnPR.Text ? string.Empty : Context.User.User.pePwd)
        : string.Empty);
    }

    #endregion

    #region txtpnPR_LostFocus

    /// <summary>
    ///Valida el TextBox Cuando Pierde el Focus
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    private void txtpnPR_LostFocus(object sender, RoutedEventArgs e)
    {
      _searchPRbyTxt = true;
      if (txtpnPR.Text != string.Empty)
      {
        // validamos que el motivo de indisponibilidad exista en los activos
        var pr = BRPersonnel.GetPersonnelById(txtpnPR.Text);
        if (pr == null)
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

    /// <summary>
    /// Muestra la información en los Textbox yCombox segun el registro que se seleccione en el DataGrid cuando 
    /// se presiona el boton de Show
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    private void btnShowInfo_Click(object sender, RoutedEventArgs e)
    {
      LoadControls();
    }

    #endregion

    #region dgNotes_MouseDoubleClick

    /// <summary>
    /// Muestra la información en los Textbox yCombox segun el registro que se seleccione en el DataGrid cuando 
    /// se hace doble clic en el DataGrid
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    private void dgNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      LoadControls();
    }

    #endregion

    #region loadControls

    /// <summary>
    /// Carga los controles en donde se invoquen
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    public void LoadControls()
    {
      if (dgNotes.SelectedItems.Count != 1) return;
      var item = dgNotes.SelectedItem as NoteGuest;
      //var item = dgNotes.Items.GetItemAt(dgNotes.Items.IndexOf(dgNotes.CurrentItem)) as Note;
      if (item != null)
      {
        cbopnPR.SelectedValue = item.PR;
        txtpnPR.Text = item.PR;
        //txtPwd.Password = BRPersonnel.GetPersonnelById(item.PR).pePwd;
        Pass();
        txtpnText.Text = item.Text;
        txtpnDT.Text = item.Date.ToString(CultureInfo.InvariantCulture);
      }
      EnabledControls(true, true, false);
    }

    #endregion

    #region EnabledControls

    /// <summary>
    /// Habilita o Deshabilita los controles segun como se configure 
    /// </summary>
    /// <param name="show"> Boton Show</param>
    /// <param name="ctrluno">Texbox y combobox</param>
    /// <param name="ctrldos">Pasword y boton add</param>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    public void EnabledControls(bool show, bool ctrluno, bool ctrldos)
    {
      txtpnDT.IsReadOnly = true;
      txtpnPR.IsReadOnly = txtpnText.IsReadOnly = ctrluno;
      txtPwd.IsEnabled = cbopnPR.IsEnabled = btnSave.IsEnabled = btnCancel.IsEnabled = ctrldos;
      btnShowInfo.IsEnabled = btnAdd.IsEnabled = show;
    }

    #endregion

    #region Window_Closing

    /// <summary>
    /// Valida se no hay ninguna nota agregandose antes de cerrarse
    /// </summary>
    /// <history>
    /// [jorcanche] created 07/04/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_creatingNote) return;
      UIHelper.ShowMessage("This form is currently in edit mode. Please save or undo your changes before closing it.",
        MessageBoxImage.Asterisk, "Can´t Close");
      e.Cancel = true;
    }

    #endregion

    #region btnSave_KeyDown

    /// <summary>
    /// Si es precionado la tecla enter inicia las validiaciones
    /// </summary>    
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

    #region FrmPRNotes_OnKeyDown

    /// <summary>
    /// Actualiza el status bar
    /// </summary>
    ///<hystory>
    /// [jorcanche] created 14/07/2016
    /// </hystory>
    private void FrmPRNotes_OnKeyDown(object sender, KeyEventArgs e)
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


    #region DgNotes_OnSelectedCellsChanged

    /// <summary>
    /// Actualiza el conteo de los registros del Data Grid
    /// </summary>
    /// <history>
    /// [jorcanche] created 14/07/2016
    /// </history>
    private void DgNotes_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
      var dg = sender as DataGrid;
      if (dg != null) StatusBarReg.Content = $"{dg.Items.CurrentPosition + 1}/{dg.Items.Count}";
    }

    #endregion
  }
}
