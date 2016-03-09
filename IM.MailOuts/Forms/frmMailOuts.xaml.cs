﻿using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.MailOuts.Classes;
using IM.MailOuts.Reports;
using IM.Model;
using IM.Model.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace IM.MailOuts.Forms
{
  /// <summary>
  /// Foumulario que muestra y asigna los Mail Outs
  /// </summary>
  /// <history>
  /// [aalcocer] 24/02/2016 Created
  /// </history>
  public partial class frmMailOuts : Window
  {
    #region Atributos

    private DateTime _dtmServerdate = new DateTime();
    private List<GuestMailOutsText> _guestMailOutsText = new List<GuestMailOutsText>();
    private LeadSourceLogin _leadSourceLogin = new LeadSourceLogin();
    private List<GuestMailOut> _ltsGuestsMailOuts = new List<GuestMailOut>();
    private List<LanguageShort> _ltsLanguages = new List<LanguageShort>();
    private List<MailOutTextByLeadSource> _ltsMailOutTextsByLeadSource = new List<MailOutTextByLeadSource>();
    private rptMailOuts _rptMailOuts = new rptMailOuts();
    private UserLogin _userLogin = new UserLogin();
    private CollectionViewSource _languageShortViewSource;
    private CollectionViewSource _mailOutTextByLeadSourceViewSource;
    private CollectionViewSource _objGuestsMailOutsViewSource;

    #endregion Atributos

    #region Constructores y destructores

    public frmMailOuts(UserData userData)
    {
      InitializeComponent();
      _userLogin = userData.User;
      _leadSourceLogin = userData.LeadSource;
    }

    #endregion Constructores y destructores

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
      dtgDatos.SelectedItems.OfType<GuestMailOut>().ToList().ForEach(item =>
      {
        item.gula = ((LanguageShort)ltsbLanguages.SelectedItem).laID;
        item.gumo = ((MailOutTextByLeadSource)ltsbMailOuts.SelectedItem).mtmoCode;
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
      dtgDatos.SelectedItems.OfType<GuestMailOut>().ToList().ForEach(item => item.gumo = null);

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
      dtgDatos.Items.OfType<GuestMailOut>().ToList().ForEach(item => item.gumo = null);

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
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
      txtUser.Text = _userLogin.peN;
      txtLocation.Text = _leadSourceLogin.lsN;
      _objGuestsMailOutsViewSource = ((CollectionViewSource)(this.FindResource("objGuestsMailOutsViewSource")));
      _languageShortViewSource = ((CollectionViewSource)(this.FindResource("languageShortViewSource")));
      _mailOutTextByLeadSourceViewSource = ((CollectionViewSource)(this.FindResource("mailOutTextByLeadSourceViewSource")));
    }

    /// <summary>
    /// Carga el formulario
    /// </summary>
    /// <history>
    /// [aalcocer] 09/03/2016 Created
    /// </history>
    private void frmMailOuts_ContentRendered(object sender, EventArgs e)
    {
      //armamos la lista del combo de idiomas
      StaStart("Loading languages...");
      _ltsLanguages = BRLanguages.GetLanguages(0);
      _languageShortViewSource.Source = _ltsLanguages;

      //cargamos los mail outs
      StaStart("Loading mail outs...");
      _ltsMailOutTextsByLeadSource = BRMailOutTexts.GetMailOutTextsByLeadSource(_leadSourceLogin.lsID, true);

      _mailOutTextByLeadSourceViewSource.Source = _ltsMailOutTextsByLeadSource.Where(x => x.mtla == "EN");

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

    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [aalcocer] 09/03/2016 Created
    /// </history>
    private void frmMailOuts_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    #endregion Métodos de la forma

    #region Métodos privados

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

      _guestMailOutsText = (from gmo in dtgDatos.Items.OfType<GuestMailOut>()
                            join mot in _ltsMailOutTextsByLeadSource on new { a = gmo.gumo, b = gmo.gula } equals new { a = mot.mtmoCode, b = mot.mtla }
                            where gmo.gumo != null && gmo.gula != null && gmo.gumoA
                            select new GuestMailOutsText
                            {
                              mtRTF = mot.mtRTF.Replace("<LastName1>", gmo.guLastName1).Replace("<FirstName1>", gmo.guFirstName1).
                              Replace("<BookTime>", gmo.guBookT.HasValue ? gmo.guBookT.Value.ToShortTimeString() : "<BookTime>").
                              Replace("<PickUp>", gmo.guPickUpT.HasValue ? gmo.guPickUpT.Value.ToShortTimeString() : "<PickUp>").
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
      _ltsGuestsMailOuts = BRGuests.GetGuestsMailOuts(_leadSourceLogin.lsID, _dtmServerdate, _dtmServerdate.AddDays(1), _dtmServerdate.AddDays(2));

      List<ObjGuestsMailOuts> _ltsObjGuestsMailOuts = _ltsGuestsMailOuts.Select(parent => new ObjGuestsMailOuts(parent)).ToList();

      _objGuestsMailOutsViewSource.Source = _ltsObjGuestsMailOuts;
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
      UIHelper.ForceUIToUpdate();
    }

    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
      UIHelper.ForceUIToUpdate();
    }

    #endregion Métodos privados
  }
}