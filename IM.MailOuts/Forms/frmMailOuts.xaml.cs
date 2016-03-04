using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.MailOuts.Classes;
using IM.MailOuts.Forms;
using IM.MailOuts.Reports;
using IM.Model;
using IM.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace IM.MailOuts
{
  /// <summary>
  /// Foumulario que muestra y asigna los Mail Outs
  /// </summary>
  /// <history>
  /// [aalcocer] 24/02/2016 Created
  /// </history>
  public partial class frmMailOuts : Window
  {
    #region Variables

    private DateTime _dtmServerdate = new DateTime();
    private List<GuestMailOutsText> _guestMailOutsText = new List<GuestMailOutsText>();
    private leadSourceLogin _leadSourceLogin = new leadSourceLogin();
    private List<GuestsMailOuts> _ltsGuestsMailOuts = new List<GuestsMailOuts>();
    private List<GetLanguages> _ltsLanguages = new List<GetLanguages>();
    private List<GetMailOutTextsByLeadSource> _ltsMailOutTextsByLeadSource = new List<GetMailOutTextsByLeadSource>();
    private rptMailOuts _rptMailOuts = new rptMailOuts();
    private userLogin _userLogin = new userLogin();
    #endregion Variables

    public frmMailOuts(UserData userData)
    {
      InitializeComponent();
      _userLogin = userData.User;
      _leadSourceLogin = userData.LeadSource;
    }

    #region Métodos de la forma


    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [aalcocer] 01/03/2016 Created
    /// </history>
    private void frmMailOuts_KeyDown(object sender, KeyEventArgs e)
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
    /// Despliega el formulario de acerca de
    /// </summary>
    /// <history>
    /// [aalcocer] 01/03/2016 Created
    /// </history>
    private void btnAbout_Click(object sender, RoutedEventArgs e)
    {
      frmAbout frmAbout = new frmAbout();
      frmAbout.ShowDialog();
    }

    /// <summary>
    /// Asigna el correo seleccionado a los huespedes seleccionados
    /// </summary>
    /// <history>
    /// [aalcocer] 26/02/2016 Created
    /// </history>
    private void btnAssign_Click(object sender, RoutedEventArgs e)
    {
      dtgDatos.SelectedItems.OfType<GuestsMailOuts>().ToList().ForEach(item =>
      {
        item.gula = ((GetLanguages)ltsbLanguages.SelectedItem).laID;
        item.gumo = ((GetMailOutTextsByLeadSource)ltsbMailOuts.SelectedItem).mtmoCode;
      });

      dtgDatos.Items.Refresh();
    }

    /// <summary>
    /// Quita el correo asignado a los huespedes seleccionados
    /// </summary>
    /// <history>
    /// [aalcocer] 26/02/2016 Created
    /// </history>
    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
      dtgDatos.SelectedItems.OfType<GuestsMailOuts>().ToList().ForEach(item => item.gumo = null);

      dtgDatos.Items.Refresh();
    }

    /// <summary>
    /// Quita el correo asignado a todos los huespedes del grid
    /// </summary>
    /// <history>
    /// [aalcocer] 26/02/2016 Created
    /// </history>
    private void btnClearAll_Click(object sender, RoutedEventArgs e)
    {
      dtgDatos.Items.OfType<GuestsMailOuts>().ToList().ForEach(item => item.gumo = null);

      dtgDatos.Items.Refresh();
    }

    /// <summary>
    /// Despliega el reporte
    /// </summary>
    /// <history>
    /// [aalcocer] 01/03/2016 Created
    /// </history>
    private void btnPreview_Click(object sender, RoutedEventArgs e)
    {

      if (!dtgDatos.Items.IsEmpty)
      {
        StaStart("Loading preview...");
        fillMailReport();
        var _frmViewer = new frmViewer(_rptMailOuts);
        _frmViewer.ShowDialog();
        StaEnd();
      }
      else
        UIHelper.ShowMessage("There is no data.");
    }

    /// <summary>
    /// Imprime el reporte
    /// </summary>
    /// <history>
    /// [aalcocer] 01/03/2016 Created
    /// </history>
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      if (!dtgDatos.Items.IsEmpty)
      {
        StaStart("Printing mail outs...");
        fillMailReport();
        _rptMailOuts.PrintToPrinter(1, false, 0, 0);
        StaEnd();
      }
      else
        UIHelper.ShowMessage("There is no data.");
    }

    /// <summary>
    /// Refresca el grid de huespedes
    /// </summary>
    /// <history>
    /// [aalcocer] 01/03/2016 Created
    /// </history>
    private void btnReload_Click(object sender, RoutedEventArgs e)
    {
      LoadGrid();
    }

    /// <summary>
    /// Configura el formulario
    /// </summary>
    /// <history>
    /// [aalcocer] 24/02/2016 Created
    /// </history>
    private void frmMailOuts_Loaded(object sender, RoutedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
      txtUser.Text = _userLogin.peN;
      txtLocation.Text = _leadSourceLogin.lsN;

      //armamos la lista del combo de idiomas
      StaStart("Loading languages...");
      _ltsLanguages = BRLanguages.GetLanguages(0);
      CollectionViewSource getLanguagesViewSource = ((CollectionViewSource)(this.FindResource("getLanguagesViewSource")));
      getLanguagesViewSource.Source = _ltsLanguages;

      //cargamos los mail outs
      StaStart("Loading mail outs...");
      _ltsMailOutTextsByLeadSource = BRMailOutTexts.GetMailOutTextsByLeadSource(_leadSourceLogin.lsID, true);
      CollectionViewSource getMailOutTextsByLeadSourceViewSource = ((CollectionViewSource)(this.FindResource("getMailOutTextsByLeadSourceViewSource")));
      getMailOutTextsByLeadSourceViewSource.Source = _ltsMailOutTextsByLeadSource.Where(x => x.mtla == "EN");

      // procesamos los huespedes para asignarle automaticamente un mail out en base a los criterios definidos
      BRMailOuts.ProcessMailOuts(_leadSourceLogin.lsID);

      //cargamos los huespedes
      LoadGrid();
      StaEnd();
    }

    /// <summary>
    /// Método que actualiza la StatusBarReg con el registro seleccionado
    /// </summary>
    /// <history>
    /// [aalcocer] 03/03/2016 Created
    /// </history>
    private void dtgDatos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      StatusBarReg.Content = string.Format("{0}/{1}", dtgDatos.Items.IndexOf(dtgDatos.CurrentItem) + 1, dtgDatos.Items.Count);
    }

    #endregion Métodos de la forma

    #region Métodos privados

    /// <summary>
    /// Verifica que la tecla se encuentre activa/inactiva, para cambiar el estilo de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [aalcocer] 18/Feb/2016 Created
    /// </history>
    private void CkeckKeysPress(System.Windows.Controls.Primitives.StatusBarItem statusBar, Key key)
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
    /// Agrega los campos del reporte
    /// </summary>
    /// <history>
    /// [aalcocer] 02/03/2016 Created
    /// </history>
    private void fillMailReport()
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");

      string strPrintedBy = _userLogin.peID + " " + String.Format("{0:ddd dd MMM yy }", DateTime.Now) + " " + DateTime.Now.ToShortTimeString();

      _guestMailOutsText = (from gmo in dtgDatos.Items.OfType<GuestsMailOuts>()
                            join mot in _ltsMailOutTextsByLeadSource on new { a = gmo.gumo, b = gmo.gula } equals new { a = mot.mtmoCode, b = mot.mtla }
                            where gmo.gumo != null && gmo.gula != null && gmo.gumoA
                            select new GuestMailOutsText
                            {
                              mtRTF = mot.mtRTF.Replace("<LastName1>", gmo.guLastName1).Replace("<FirstName1>", gmo.guFirstName1).
                              Replace("<BookTime>",  gmo.guBookT.HasValue ? gmo.guBookT.Value.ToShortTimeString(): "<BookTime>").
                              Replace("<PickUp>",gmo.guPickUpT.HasValue ? gmo.guPickUpT.Value.ToShortTimeString(): "<PickUp>").
                              Replace("<Agency>", gmo.guag).Replace("<RoomNum>", gmo.guRoomNum).Replace("<PRCode>", gmo.guPRInvit1).Replace("<PRName>", gmo.peN).
                              Replace("<PrintedBy>", strPrintedBy),
                              MrMrs = mot.laMrMrs,
                              LastName = gmo.guLastName1,
                              Room = mot.laRoom,
                              RoomNum = gmo.guRoomNum
                            }).ToList();
      _rptMailOuts.Load();
      _rptMailOuts.SetDataSource(_guestMailOutsText);
    }

    /// <summary>
    /// Configuracion inicial de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [aalcocer] 18/Feb/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }

    /// <summary>
    /// Carga los huespedes
    /// </summary>
    /// </summary>
    /// <history>
    /// [aalcocer] 24/02/2016 Created
    /// </history>
    private void LoadGrid()
    {
      StaStart("Loading guests...");
      _dtmServerdate = BRHelpers.GetServerDate();
      _ltsGuestsMailOuts = BRGuest.GetGuestsMailOuts(_leadSourceLogin.lsID, _dtmServerdate, _dtmServerdate.AddDays(1), _dtmServerdate.AddDays(2));

      List<ObjGuestsMailOuts> _ltsObjGuestsMailOuts = _ltsGuestsMailOuts.Select(parent => new ObjGuestsMailOuts(parent)).ToList();

      CollectionViewSource _ObjGuestsMailOutsSourceViewSource = ((CollectionViewSource)(this.FindResource("objGuestsMailOutsSourceViewSource")));

      _ObjGuestsMailOutsSourceViewSource.Source = _ltsObjGuestsMailOuts;
      dtgDatos.SelectedItem = null;
      StaEnd();
    }

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje</param>
    private void StaStart(String message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
    }

    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
    }
    #endregion Métodos privados


  }
}