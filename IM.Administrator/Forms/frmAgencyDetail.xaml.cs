﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Administrator.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using System.Globalization;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAgencyDetail.xaml
  /// </summary>
  public partial class frmAgencyDetail : Window
  {
    public Agency agency = new Agency();//Objeto para llenar el formulario
    private string _unavailableMotive;
    private string _Market;
    public ModeOpen mode;
    public frmAgencyDetail()
    {
      InitializeComponent();      
    }
    #region eventos del formulario
    #region WindowKeyDown
    /// <summary>
    /// cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {  
      if (e.Key == Key.Escape )
      {
        DialogResult = false;
        Close();                
      }      
    }
    #endregion
    
    #region WindowLoaded
    /// <summary>
    /// Carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {      
      LoadUnavailableMotives();
      LoadMarkets();
      LoadReps();
      LoadSegments();
      LoadClubs();
      LoadCountries();
      OpenMode();
    }
    #endregion

    #region Accept
    /// <summary>
    /// guarda o actualiza el registro dependiendo del modo en que se abrió
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      string sMsj = "";

      #region Validar campos obligatorios
      if(string.IsNullOrWhiteSpace(txtID.Text))
      {
        sMsj += "Specify the Agency ID.\n";
      }
      if(string.IsNullOrWhiteSpace(txtN.Text))
      {
        sMsj += "Specify the Agency Description.\n";
      }
      if(cmbUnavMot.SelectedIndex<0)
      {
        sMsj += "Specify the Unavailable Motive.\n";
      }
      if(cmbMarket.SelectedIndex<0)
      {
        sMsj += "Specify the Market.\n";
      }
      if(string.IsNullOrWhiteSpace(txtShowPay.Text))
      {
        sMsj += "Specify the Show Pay.\n";
      }
      if(string.IsNullOrWhiteSpace(txtSalePay.Text))
      {
        sMsj += "Specify the Sale Pay. \n";
      }

      #endregion

      if(sMsj=="")
      {
        agency.agcl = ((agency.agcl == -1) ? null : agency.agcl);
        agency.agse = ((agency.agse == "-1") ? null : agency.agse);
        int nRes = 0;
        #region Operacion
        switch (mode)
        {
          case ModeOpen.add:
            {
              nRes = BRAgencies.SaveAgency(agency, false);
              break;
            }
          case ModeOpen.edit:
            {                    
              bool blnMarkets = ((agency.agmk.ToString() != _Market) ? true : false);
              bool blnUnMot = ((agency.agum.ToString() != _unavailableMotive) ? true : false);
              nRes = BRAgencies.SaveAgency(agency, true,blnUnMot,blnMarkets);
              break;
            }
        } 
        #endregion

        #region Respuesta
        switch (nRes)//Se valida la repuesta
        {
          case 0:
            {
              UIHelper.ShowMessage("Agency not saved");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Agency successfully saved");
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              UIHelper.ShowMessage("Agency ID already exist please select another one");
              break;
            }
        }
        #endregion
      }
      else
      {
        UIHelper.ShowMessage(sMsj.TrimEnd('\n'));
      }
    }
    #endregion
    

    #region Texbox Sólo Numeros
    /// <summary>
    /// TextBox Sólo acepta número
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }
    #endregion

    #region Got Focus
    /// <summary>
    /// Cambia el string format del campo de textos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    private void txt_GotFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      int nRes;
      txt.Text = Int32.TryParse(txt.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out nRes) ? nRes.ToString("N0") : "0";
    }
    #endregion

    #region LostFocus
    /// <summary>
    /// Le pone un valor default a cun campo de texto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    private void txt_LostFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (string.IsNullOrWhiteSpace(txt.Text))
      {
        txt.Text = "0";
      }
    }
    #endregion

    #endregion

    #region metodos
    #region LoadUnavailMot
    /// <summary>
    /// Llena el combobox de Unavailable Motive
    /// </summary>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    protected void LoadUnavailableMotives()
    {
      List<UnavailableMotive> lstUnavailableMotive = BRUnavailableMotives.GetUnavailableMotives();
      cmbUnavMot.ItemsSource = lstUnavailableMotive;
    }
    #endregion

    #region LoadMarkets
    /// <summary>
    /// Llena el combobox de markets
    /// </summary>
    /// <history>
    /// [emoguel] 11/03/2016
    /// </history>
    protected void LoadMarkets()
    {
      List<MarketShort> lstMarkestShort = BRMarkets.GetMarkets(1);
      cmbMarket.ItemsSource = lstMarkestShort;
    }
    #endregion

    #region LoadReps
    /// <summary>
    /// Llena el combo de Reps
    /// </summary>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    protected void LoadReps()
    {
      List<Rep> lstReps = BRReps.GetReps(new Rep(), 1);      
      cmbRep.ItemsSource = lstReps;
    }
    #endregion

    #region LoadSegemnt
    /// <summary>
    /// Llena el combobox de segments By agency
    /// </summary>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    protected void LoadSegments()
    {
      List<SegmentByAgency> lstSegmentsByAgencies = BRSegmentsByAgency.GetSegMentsByAgency(new SegmentByAgency());    
      if(lstSegmentsByAgencies.Count>0)
      {
        lstSegmentsByAgencies.Insert(0, new SegmentByAgency { seID = "-1", seN = "" });
      }
      cmbSegment.ItemsSource = lstSegmentsByAgencies;
    }

    #endregion

    #region LoadClubs
    /// <summary>
    /// Llena el combobox de Clubs
    /// </summary>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    protected void LoadClubs()
    {
      List<Club> lstClubs = BRClubs.GetClubs(new Club());
      if (lstClubs.Count > 0)
      {
        lstClubs.Insert(0, new Club { clID = -1, clN = "" });
      }
      cmbClub.ItemsSource = lstClubs;
    }
    #endregion

    #region LoadCountries

    protected void LoadCountries()
    {
      List<CountryShort> lstCountries = BRCountries.GetCountries(1);
      cmbCountri.ItemsSource = lstCountries;
    }
    #endregion

    #region modo de la ventana
    /// <summary>
    /// Abre la ventana dependiendo del modo que elija el usuario
    /// Preview|Edit|Add
    /// </summary>
    /// <history>
    /// [emoguel] 11/03/2016 Created
    /// </history>
    protected void OpenMode()
    {
      this.DataContext = agency;
      switch (mode)
      {        
        case ModeOpen.preview://show
          {
            btnAccept.Visibility = Visibility.Hidden;            
            break;
          }
        case ModeOpen.add://add
          {
            txtID.IsEnabled = true;
            LockControls(true);
            break;
          }
        case ModeOpen.edit://Edit
          {
            txtID.IsEnabled = false;
            LockControls(true);
            _unavailableMotive = agency.agum.ToString();//Llenamos la variable con el valor original
            _Market = agency.agmk.ToString();//Llenamos la variable con el valor original        
            break;
          }
      }

    }
    #endregion
    #region Bloquear|Desbloquear controles
    /// <summary>
    /// Bloquea|Desbloquea los botones dependiendo del modo en que se habra
    /// </summary>
    /// <param name="bValue">true para desbloquear| false para bloquear</param>
    /// <history>
    /// [emoguel] 11/03/2016 Created
    /// </history>
    protected void LockControls(bool blnValue)
    {
      txtN.IsEnabled = blnValue;
      txtSalePay.IsEnabled = blnValue;
      txtShowPay.IsEnabled = blnValue;
      cmbCountri.IsEnabled = blnValue;
      cmbMarket.IsEnabled = blnValue;
      cmbRep.IsEnabled = blnValue;
      cmbUnavMot.IsEnabled = blnValue;
      cmbClub.IsEnabled = blnValue;
      cmbSegment.IsEnabled = blnValue;
      chkA.IsEnabled = blnValue;
      chkIncTour.IsEnabled = blnValue;
      chkShowInLst.IsEnabled = blnValue;
    }
    #endregion

    #endregion
  }
}