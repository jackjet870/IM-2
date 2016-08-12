using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Linq;

namespace IM.MailOutsConfig.Forms
{
  /// <summary>
  /// Interaction logic for frmMailOutsConfig.xaml
  /// </summary>
  public partial class frmMailOutsConfig : Window
  {
    #region Propiedades y Atributos
    public ExecuteCommandHelper RefreshDataSource { get; set; }
    MailOut _selectedMailOut;
    LeadSourceByUser _lsbyUser;

    #endregion
    public frmMailOutsConfig()
    {
      InitializeComponent();
      RefreshDataSource = new ExecuteCommandHelper(x => LoadDataSource());
    }
    #region Eventos de la ventana
    /// <summary>
    /// Evento que se lanza al iniciar la aplicacion
    /// </summary>
    /// <history>
    /// [erosado] 06/Mar/2016 Created
    /// </history>   
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadDataSource();

      //Configuramos los controles
      UIHelper.SetUpControls(new MailOut(), this);
    }
    /// <summary>
    /// Guarda los cambios de un MailOut
    /// </summary>
    /// <history>
    /// [erosado] 09/04/2016  Created
    /// </history>
    private void imgSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      DoUpdateMailOut(_selectedMailOut);
    }
    /// <summary>
    /// Activa el RichTextBox para que pueda ser editado
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    private void imgEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      EditModeOn();
    }
    /// <summary>
    /// Cancela el modo edicion
    /// </summary>
    /// <history>
    /// [erosadp] 07/04/2016  Created
    /// </history>
    private void imgCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      EditModeOff();
    }
    /// <summary>
    /// Cierra la aplicación
    /// </summary>
    ///<history>
    ///[erosado]  08/04/2016  Created
    /// </history>
    private void imgExit_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
    {
      if (imgEdit.IsEnabled)
      {
        Close();
      }
      else
      {
        MessageBoxResult result = UIHelper.ShowMessage("Do you want to save changes before closing?", MessageBoxImage.Question, "Mail Outs Configuration");
        if (result == MessageBoxResult.No)
        {
          Close();
        }
      }
    }
    /// <summary>
    /// Abre la interfaz para agregar nuevo MailOut
    /// </summary>
    /// <history>
    /// [erosado] 20/04/2016  Created
    /// </history>
    private void imgAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      pupNewMot.IsOpen = true;
      txtNewMotName.Focus();
    }
    /// <summary>
    /// Evento que elimina un MailOut
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [erosado] 20/04/2016  Created
    /// </history>
    private void imgDelete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      MessageBoxResult result = UIHelper.ShowMessage("Do you want to delete the mail out selected?", MessageBoxImage.Question, "Mail Outs Configuration");
      if (result == MessageBoxResult.Yes)
      {
        StaStart("Deleting MailOut...");
        DoDeleteMailOut(_selectedMailOut);
      }
    }
    /// <summary>
    /// Obtiene los MailOutText
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    private void cbxLeadSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _lsbyUser = cbxLeadSource.SelectedItem as LeadSourceByUser;

      DoGetMailOuts(_lsbyUser?.lsID);
      StaStart("Searching MailOut List...");
    }

    /// <summary>
    /// Evento que se ejecuta se selecciona un MailOut
    /// </summary>
    /// <history>
    /// [erosado] 19/04/2016  Created
    /// </history>
    private void lsbxMailOuts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (lsbxMailOuts.SelectedValue != null)
      {
        _selectedMailOut = lsbxMailOuts.SelectedItem as MailOut;
        layout.DataContext = _selectedMailOut;
      }
    }
    #endregion

    #region Async Methods
    /// <summary>
    /// Obtiene los LeadSources por User, Programs All  y Region All
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// [edgrodriguez] 21/05/2016 Modified. El metodo GetLeadSourcesByUser se volvió asincrónico.
    /// </history>
    /// <param name="user">loginUserID</param>
    public async void DoGetLeadSources(string user)
    {
      try
      {
        List<LeadSourceByUser> data = await BRLeadSources.GetLeadSourcesByUser(user, EnumProgram.Inhouse);
        if (data.Count > 0)
        {
          cbxLeadSource.ItemsSource = data;
          cbxLeadSource.SelectedIndex = 0;
        }
        else
        {
          cbxLeadSource.Text = "No data found - Press Ctrl+F5 to load Data";
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
    }

    /// <summary>
    /// Obtiene la lista de MailOutText por LeadSourceID y LanguageID
    /// </summary>
    /// <param name="ls">LeadSourceID</param>
    /// <history>
    /// [erosado] 13/04/2016  Created
    /// [erosado] 12/08/2016  Modified. si el envias un moCod lo busca en la lista y lo selecciona.
    /// </history>
    public async void DoGetMailOuts(string ls, string moCod = "")
    {
      try
      {
        var data = await BRMailOuts.GetMailOuts(ls);
        if (data.Count > 0)
        {
          lsbxMailOuts.ItemsSource = data;
          txtbMailOutsNumber.Text = data.Count.ToString();

          //Buscamos en la lista el moCod
          var index = data.FindIndex(x => x.moCode == moCod);
          lsbxMailOuts.SelectedIndex = index != -1 ? index : 0;
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }

    }

    /// <summary>
    /// Obtiene el catalogo de countries
    /// </summary>
    /// <history>
    /// [erosado] 20/04/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async void DoGetCountries()
    {
      try
      {
        List<CountryShort> data = await BRCountries.GetCountries(1);
        if (data.Count > 0)
        {
          data.Insert(0, new CountryShort { coN = "ANY ONE", coID = "ANY ONE" });
          cbxCountry.ItemsSource = data;
          cbxCountry.SelectedIndex = 0;
        }
        else
        {
          cbxCountry.Text = "No data found - Press Ctrl+F5 to load Data";
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
    }

    /// <summary>
    /// Obtiene el catalogo de Agencies
    /// </summary>
    /// <history>
    /// [erosado] 20/04/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async void DoGetAgencies()
    {
      try
      {
        List<AgencyShort> data = await BRAgencies.GetAgencies(1);
        if (data.Count > 0)
        {
          data.Insert(0, new AgencyShort { agN = "ANY ONE", agID = "ANY ONE" });
          cbxAgency.ItemsSource = data;
          cbxAgency.SelectedIndex = 0;
        }
        else
        {
          cbxAgency.Text = "No data found - Press Ctrl+F5 to load Data";
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
    }
    /// <summary>
    /// Obtiene el catalogo de Markets
    /// </summary>
    /// <history>
    /// [erosado] 20/04/2016 Created
    /// </history>
    public async void DoGetMarkets()
    {
      try
      {
        List<MarketShort> data = await BRMarkets.GetMarkets(1);
        if (data.Any())
        {
          data.Insert(0, new MarketShort { mkN = "ANY ONE", mkID = "ANY ONE" });
          cbxMarket.ItemsSource = data;
          cbxMarket.SelectedIndex = 0;
        }
        else
        {
          cbxMarket.Text = "No data found - Press Ctrl+F5 to load Data";
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
    }
    /// <summary>
    /// Actualiza la información de un MailOut
    /// </summary>
    /// <param name="mo">MailOut</param>
    /// <history>
    /// [erosado] 20/04/2016  Created.
    /// [erosado] 12/08/2016  Modified. Cuando se guarda la informacion del MailOut se mantiene seleccionado en la lista lsbxMailOuts.
    /// </history>
    public async void DoUpdateMailOut(MailOut mo)
    {
      try
      {
        var data = await BRMailOuts.UpdateMailOut(mo);
        if (data > 0)
        {
          UIHelper.ShowMessage("Data saved successfully", MessageBoxImage.Information, "Mail Outs Configuration");
          EditModeOff();
          DoGetMailOuts(_lsbyUser.lsID, mo.moCode);
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
    }
    /// <summary>
    /// Elimina los registros del MailOut Seleccionado
    /// </summary>
    /// <param name="mo">MailOut</param>
    /// <history>
    /// [erosado] 20/04/2016 Created
    /// </history>
    public async void DoDeleteMailOut(MailOut mo)
    {
      try
      {
        var data = await BRMailOuts.DeleteMailOut(mo);
        if (data > 0)
        {
          UIHelper.ShowMessage("Delete MailOut successfully", MessageBoxImage.Information, "Mail Outs Configuration");
          EditModeOff();
          cbxLeadSource_SelectionChanged(this, null);
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
    }

    /// <summary>
    /// Inserta un MailOut Nuevo
    /// </summary>
    /// <param name="mols">MailOut</param>
    /// <param name="moCode"></param>
    /// <history>
    /// [erosado] 20/04/2016 Created
    /// </history>
    public async void DoInsertMailOut(string mols, string moCode)
    {
      try
      {
        var data = await BRMailOuts.InsertMailOut(mols, moCode);

        if (data != 0)
        {
          UIHelper.ShowMessage("Insert MailOut successfully", MessageBoxImage.Information, "Mail Outs Configuration");
          DoGetMailOuts(_lsbyUser.lsID, moCode);
        }
        else
        {
          UIHelper.ShowMessage("Mail Out name already exists.", MessageBoxImage.Warning, "Mail Outs Configuration");
        }

        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }

    }
    #endregion

    #region StatusBar
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }

    /// <summary>
    /// Verifica que la tecla se encuentre activa/inactiva, para cambiar el estilo de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void CkeckKeysPress(StatusBarItem statusBar, Key key)
    {
      var keyPess = Keyboard.GetKeyStates(key).ToString();

      if (keyPess.Contains("Toggled")) //si está activo el Bloq Mayús
      {
        statusBar.FontWeight = FontWeights.Bold;
        statusBar.Foreground = Brushes.Black;
      }
      else
      {
        KeyDefault(statusBar);
      }
    }

    /// <summary>
    /// Configuracion inicial de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void KeyDefault(StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    private void StaStart(string message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      Cursor = Cursors.Wait;

    }
    /// <summary>
    /// Indica en la barra de estado que se termina un proceso
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      Cursor = null;
    }

    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    #endregion

    #region  Metodos
    /// <summary>
    /// Desactiva el Modo edicion
    /// </summary>
    /// <history>
    /// [erosado] 19/04/2016  Created.
    /// </history>
    private void EditModeOn()
    {
      brdUno.IsEnabled =
      brdDos.IsEnabled =
      brdContacted.IsEnabled =
      brdInvited.IsEnabled =
      brdBookCancelled.IsEnabled =
      brdLsAgCo.IsEnabled =
      brdShSa.IsEnabled =
      brdRoomNumber.IsEnabled =
      brdOnGroup.IsEnabled =
      imgSave.IsEnabled =
      imgCancel.IsEnabled = true;
      txtEditMode.Visibility = Visibility.Visible;
      imgEdit.IsEnabled =
      imgExit.IsEnabled =
      imgDelete.IsEnabled =
      imgAdd.IsEnabled = false;
    }

    /// <summary>
    /// Activa el Modo edicion
    /// </summary>
    /// <history>
    /// [erosado] 19/04/2016  Created.
    /// </history>
    private void EditModeOff()
    {
      brdUno.IsEnabled =
      brdDos.IsEnabled =
      brdContacted.IsEnabled =
      brdInvited.IsEnabled =
      brdBookCancelled.IsEnabled =
      brdLsAgCo.IsEnabled =
      brdShSa.IsEnabled =
      brdRoomNumber.IsEnabled =
      brdOnGroup.IsEnabled =
      imgSave.IsEnabled =
      imgCancel.IsEnabled = false;
      txtEditMode.Visibility = Visibility.Collapsed;
      imgEdit.IsEnabled =
      imgExit.IsEnabled =
      imgDelete.IsEnabled =
      imgAdd.IsEnabled = true;
    }

    /// <summary>
    /// Carga los catalogos LeadSources, Markets, Agencies, Countries 
    /// </summary>
    /// <history>
    /// [erosado] 19/04/2016  Created.
    /// </history>
    public void LoadDataSource()
    {
      StaStart("Loading LeadSources, Markets, Agencies, Countries ...");
      //Cargamos LeadSource
      DoGetLeadSources(App.User.User.peID);
      //Cargamos Markets
      DoGetMarkets();
      //Cargamos Agencies
      DoGetAgencies();
      //Cargamos Countries
      DoGetCountries();
    }
    #endregion

    #region Eventos PopUp
    /// <summary>
    /// Cierra la ventana del PopUp New MailOut
    /// </summary>
    /// <history>
    /// [erosado] 19/04/2016  Created
    /// </history>
    private void imgCerrar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      pupNewMot.IsOpen = false;
      txtNewMotName.Clear();

    }
    /// <summary>
    /// Agrega un Nuevo MailOut al presionar Enter
    /// </summary>
    /// <history>
    /// [erosado] 19/04/2016  Created
    /// </history>
    private void txtNewMotName_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        if (!string.IsNullOrEmpty(txtNewMotName.Text))
        {
          DoInsertMailOut(cbxLeadSource.SelectedValue.ToString(), txtNewMotName.Text);
          txtNewMotName.Clear();
          pupNewMot.IsOpen = false;
          StaStart("Creating new MailOut...");
        }
        else
        {
          UIHelper.ShowMessage("Specify a mail out name", MessageBoxImage.Information, "Mail Outs Configuration");
        }
      }

    }
    /// <summary>
    /// Agrega un Nuevo MailOut al presionar el boton Save
    /// </summary>
    /// <history>
    /// [erosado] 19/04/2016  Created
    /// </history>
    private void imgSaveNewMot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!string.IsNullOrEmpty(txtNewMotName.Text))
      {
        DoInsertMailOut(cbxLeadSource.SelectedValue.ToString(), txtNewMotName.Text);
        txtNewMotName.Clear();
        pupNewMot.IsOpen = false;
        StaStart("Creating new MailOut...");
      }
      else
      {
        UIHelper.ShowMessage("Specify a mail out name", MessageBoxImage.Information, "Mail Outs Configuration");
      }
    }

    #endregion

  }
}
