using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;
using System;
using System.Windows.Controls;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAgencyDetail.xaml
  /// </summary>
  public partial class frmAgencyDetail : Window
  {
    #region Variables
    public Agency oldAgency = new Agency();//Objeto con los valores iniciales
    public Agency agency = new Agency();//Objeto para llenar el formulario 
    public EnumMode enumMode;
    private bool _isClosing = false;
    #endregion

    public frmAgencyDetail()
    {
      InitializeComponent();      
    }
    #region eventos del formulario
    
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
      
      ObjectHelper.CopyProperties(agency, oldAgency);      
      LoadUnavailableMotives();
      LoadMarkets();
      LoadReps();
      LoadSegments();
      LoadClubs();
      LoadCountries();
      #region Bloquear botones
      if (enumMode != EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        txtagID.IsEnabled = (enumMode == EnumMode.Add);
        txtagN.IsEnabled = true;
        txtagSalePay.IsEnabled = true;
        txtagShowPay.IsEnabled = true;
        cmbagcl.IsEnabled = true;
        cmbagco.IsEnabled = true;
        cmbagmk.IsEnabled = true;
        cmbagrp.IsEnabled = true;
        cmbagse.IsEnabled = true;
        cmbagum.IsEnabled = true;
        chkagA.IsEnabled = true;
        chkagIncludeTour.IsEnabled = true;
        chkagList.IsEnabled = true;
        UIHelper.SetUpControls(agency, this);
      }
      #endregion      
      DataContext = agency;
      skpStatus.Visibility = Visibility.Collapsed;
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
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();//Para actualizar el datacontext
      if (cmbagum.SelectedValue!=null && ObjectHelper.IsEquals(agency, oldAgency) && enumMode!=EnumMode.Add )
      {
        _isClosing = true;
        Close();
      }
      else
      {
        string sMsj = ValidateHelper.ValidateForm(this, "Agency");

        if (sMsj == "")
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          agency.agcl = ((agency.agcl == -1) ? null : agency.agcl);
          agency.agse = ((agency.agse == "-1") ? null : agency.agse);
          int nRes = 0;
          #region Operacion
          switch (enumMode)
          {
            case EnumMode.Add:
              {
                nRes = await BRAgencies.SaveAgency(agency, false);
                break;
              }
            case EnumMode.Edit:
              {
                bool blnMarkets = ((agency.agmk.ToString() != oldAgency.agmk.ToString()) ? true : false);
                bool blnUnMot = ((agency.agum.ToString() != oldAgency.agum.ToString()) ? true : false);
                nRes = await BRAgencies.SaveAgency(agency, true, blnUnMot, blnMarkets);
                break;
              }
          }
          #endregion
          UIHelper.ShowMessageResult("Agency", nRes);
          skpStatus.Visibility = Visibility.Collapsed;
          if(nRes>0 )
          {
            _isClosing = true;
            DialogResult = true;
            Close();
          }      
        }
        else
        {
          UIHelper.ShowMessage(sMsj);
        }
      }

    }
    #endregion    
    
    #region KeyDown
    /// <summary>
    /// cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        Close();
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
    /// [emoguel] modified 30/05/2016 sel volvió async
    /// </history>
    protected async void LoadUnavailableMotives()
    {
      try
      {
        List<UnavailableMotive> lstUnavailableMotive = await BRUnavailableMotives.GetUnavailableMotives();
        cmbagum.ItemsSource = lstUnavailableMotive;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Agencies");
      }
    }
    #endregion

    #region LoadMarkets
    /// <summary>
    /// Llena el combobox de markets
    /// </summary>
    /// <history>
    /// [emoguel] 11/03/2016
    /// </history>
    protected async void LoadMarkets()
    {
      try
      {
        List<MarketShort> lstMarkestShort = await BRMarkets.GetMarkets(1);
        cmbagmk.ItemsSource = lstMarkestShort;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Agencies");
      }
    }
    #endregion

    #region LoadReps
    /// <summary>
    /// Llena el combo de Reps
    /// </summary>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// [emoguel] modified 30/05/2016 Se volvió async el metodo
    /// </history>
    protected async void LoadReps()
    {
      try
      {
        List<Rep> lstReps = await BRReps.GetReps(new Rep(), 1);
        cmbagrp.ItemsSource = lstReps;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Agencies");
      }
    }
    #endregion

    #region LoadSegemnt
    /// <summary>
    /// Llena el combobox de segments By agency
    /// </summary>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    protected async void LoadSegments()
    {
      try
      {
        List<SegmentByAgency> lstSegmentsByAgencies =await BRSegmentsByAgency.GetSegMentsByAgency(new SegmentByAgency());
        cmbagse.ItemsSource = lstSegmentsByAgencies;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Agencies");
      }
    }

    #endregion

    #region LoadClubs
    /// <summary>
    /// Llena el combobox de Clubs
    /// </summary>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// </history>
    protected async void LoadClubs()
    {
      try
      {
        List<Club> lstClubs = await BRClubs.GetClubs(new Club());        
        cmbagcl.ItemsSource = lstClubs;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Agencies");
      }
    }
    #endregion

    #region LoadCountries
    /// <summary>
    /// 
    /// </summary>
    /// <history>
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    protected async void LoadCountries()
    {
      try
      {
        List<CountryShort> lstCountries = await BRCountries.GetCountries(1);
        cmbagco.ItemsSource = lstCountries;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Agencies");
      }
    }

    #endregion

    #endregion

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <history>
    /// [emoguel] 30/05/2016 created
    /// [emoguel] 31/08/2016 modified--->Se agregaron las validaciones del boton cancel
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        btnCancel.Focus();
        if (enumMode != EnumMode.ReadOnly)
        {
          if (!ObjectHelper.IsEquals(agency, oldAgency))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result == MessageBoxResult.No)
            {
              e.Cancel = true;
            }
          }
        }
      }
    }
    #endregion
  }
}