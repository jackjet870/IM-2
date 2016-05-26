using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using System;
using System.Windows.Controls;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmLeadSourcesDetail.xaml
  /// </summary>
  public partial class frmLeadSourceDetail : Window
  {
    #region Variables
    public LeadSource leadSource = new LeadSource();//Objeto a guardar
    public LeadSource oldLeadSource = new LeadSource();//Objeto con los datos iniciales
    public int nStatus = -1;//Estatus para el modo busqueda
    public int nRegen = -1;//Is regen para el modo busqueda
    public int nAnimation = -1;//Is animation para el modo busqueda 
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    private List<Location> _oldLocations = new List<Location>();//Locaciones iniciales de la ventana
    private List<Agency> _oldAgencies = new List<Agency>();//Agencias iniciales de la ventana     
    #endregion
    public frmLeadSourceDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(leadSource, oldLeadSource,true);
      UIHelper.SetUpControls(leadSource, this);
      if(enumMode==EnumMode.search)
      {
        txtlsID.IsEnabled = true;
        dgrAgencies.Visibility = Visibility.Collapsed;
        dgrLocations.Visibility = Visibility.Collapsed;
        #region Ocultar Labels
        lblHotel.Visibility = Visibility.Collapsed;
        lblBoss.Visibility = Visibility.Collapsed;
        lblRooms.Visibility = Visibility.Collapsed; 
        #endregion

        #region Ocultar TextBox y Combobox
        cmbHotels.Visibility = Visibility.Collapsed;
        cmblsBoss.Visibility = Visibility.Collapsed;
        grdOpera.Visibility = Visibility.Collapsed;
        grd1.Visibility = Visibility.Collapsed;
        grd2.Visibility = Visibility.Collapsed;
        grd3.Visibility = Visibility.Collapsed;
        grd4.Visibility = Visibility.Collapsed;
        grd5.Visibility = Visibility.Collapsed;
        txtlsRooms.Visibility = Visibility.Collapsed;
        chklsA.Visibility = Visibility.Collapsed;
        chklsAnimation.Visibility = Visibility.Collapsed;
        chklsAUseOpera.Visibility = Visibility.Collapsed;
        chklsPayInOut.Visibility = Visibility.Collapsed;
        chklsPayWalkOut.Visibility = Visibility.Collapsed;
        chklsRegen.Visibility = Visibility.Collapsed;
        chklsUseSistur.Visibility = Visibility.Collapsed;
        #endregion

        #region Mostrar Combobox
        cmbStatus.Visibility = Visibility.Visible;
        cmbRegen.Visibility = Visibility.Visible;
        cmbAnimation.Visibility = Visibility.Visible;
        #endregion

        #region Mostrar Labels
        lblStatus.Visibility = Visibility.Visible;
        lblRegen.Visibility = Visibility.Visible;
        lblAnimation.Visibility = Visibility.Visible;
        #endregion
        SizeToContent = SizeToContent.WidthAndHeight;
        LoadSearch();
      }
      else
      {
        _oldAgencies = oldLeadSource.Agencies.ToList().Where(ag => ag.agA == true).ToList();
        dgrAgencies.ItemsSource = oldLeadSource.Agencies.ToList().Where(ag => ag.agA == true).ToList();
        LoadAgencies();
        LoadLocations();
        txtlsID.IsEnabled = (enumMode == EnumMode.add);
        if (enumMode == EnumMode.preview)
        {
          dgrAgencies.IsReadOnly = true;
          dgrLocations.IsReadOnly = true;
          btnAccept.Visibility = Visibility.Hidden;
        }
        LoadHotels();
        LoadBoss();
      }
      LoadPrograms();
      LoadSalesRoom();
      LoadAreas();
      LoadRegions();
      LoadSegmentsByLeadSources();

      DataContext = leadSource;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key==Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza registros en el catalogo LeadSources
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<Location> lstLocations = (List<Location>)dgrLocations.ItemsSource;
      List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
      #region Save
      if (enumMode != EnumMode.search)
      {
        if (enumMode != EnumMode.add && ObjectHelper.IsEquals(leadSource, oldLeadSource) && ObjectHelper.IsListEquals(_oldAgencies, lstAgencies) && ObjectHelper.IsListEquals(_oldLocations, lstLocations))
        {
          Close();
        }
        else
        {
          #region Save
          string strMsj = ValidateHelper.ValidateForm(this, "Lead Source");
          if (strMsj=="")
          {
            #region Locations
            List<Location> lstLocAdd = lstLocations.Where(lc => !_oldLocations.Any(lcc => lcc.loID == lc.loID)).ToList();
            List<Location> lstLocDel = _oldLocations.Where(lc => !lstLocations.Any(lcc => lcc.loID == lc.loID)).ToList();
            #endregion
            #region Agencies
            List<Agency> lstAgeAdd = lstAgencies.Where(ag => !_oldAgencies.Any(agg => agg.agID == ag.agID)).ToList();
            List<Agency> lstAgeDel = _oldAgencies.Where(ag => !lstAgencies.Any(agg => agg.agID == ag.agID)).ToList();
            #endregion

            int nRes = BRLeadSources.SaveLeadSource(leadSource,lstLocAdd,lstLocDel,lstAgeAdd,lstAgeDel,(enumMode==EnumMode.edit));
            UIHelper.ShowMessageResult("Lead Source", nRes);
            if(nRes>0)
            {
              DialogResult = true;
              Close();
            }
          }
          else
          {
            UIHelper.ShowMessage(strMsj);
          }

          #endregion
        }
       }
      #endregion
      #region Search
      else
      {
        DialogResult = true;
        nStatus = Convert.ToInt32(cmbStatus.SelectedValue);
        nRegen = Convert.ToInt32(cmbRegen.SelectedValue);
        nAnimation = Convert.ToInt32(cmbAnimation.SelectedValue);
        Close();
      } 
      #endregion
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambions pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.preview && enumMode!=EnumMode.search)
      {
        List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
        List<Location> lstLocations = (List<Location>)dgrLocations.ItemsSource;
        if (!ObjectHelper.IsEquals(leadSource, oldLeadSource) || !ObjectHelper.IsListEquals(lstLocations, _oldLocations) || !ObjectHelper.IsListEquals(lstAgencies,_oldAgencies))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            Close();
          }
        }
        else
        {
          Close();
        }
      }
      else
      {
        Close();
      }
    }
    #endregion

    #region dgrLocations_CellEditEnding
    /// <summary>
    /// Verifica que una locacion no se pueda repetir mas de una vez
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private void dgrLocations_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        List<Location> lstLocations = (List<Location>)dgrLocations.ItemsSource;//Los items del grid                   
        Location location = (Location)dgrLocations.SelectedItem;//Valor que se está editando

        var Combobox = (ComboBox)e.EditingElement;
        Location locationCombo = (Location)Combobox.SelectedItem;//Valor seleccionado del combo

        if (locationCombo != null)//Se valida que no esté seleccionado en otra fila
        {
          if (locationCombo != location)//Validar que se esté cambiando el valor
          {
            Location locationVal = lstLocations.Where(lo => lo.loID != location.loID && lo.loID == locationCombo.loID).FirstOrDefault();
            if (locationVal != null)
            {
              UIHelper.ShowMessage("Location must not be repeated");
              e.Cancel = true;
            }
          }
        }
      }
    }
    #endregion

    #region dgrAgencies_CellEditEnding
    /// <summary>
    /// Verifica que las agencias no se puedan repetir
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/05/2016
    /// </history>
    private void dgrAgencies_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {

      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;//Los items del grid                   
        Agency agency = (Agency)dgrAgencies.SelectedItem;//Valor que se está editando

        var Combobox = (ComboBox)e.EditingElement;
        Agency agencyCombo = (Agency)Combobox.SelectedItem;//Valor seleccionado del combo

        if (agencyCombo != null)//Se valida que no esté seleccionado en otra fila
        {
          if (agencyCombo != agency)//Validar que se esté cambiando el valor
          {
            Agency agencyVal = lstAgencies.Where(ag => ag.agID != agency.agID && ag.agID == agencyCombo.agID).FirstOrDefault();
            if (agencyVal != null)
            {
              UIHelper.ShowMessage("Agency must not be repeated");
              e.Cancel = true;
            }
          }
        }
      }
    }

    #endregion
    #endregion

    #region Methods

    #region LoadHotels
    /// <summary>
    /// Carga el combobox de hotels
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadHotels()
    {
      List<Hotel> lstHotels =await BRHotels.GetHotels(nStatus: 1);
      cmbHotels.ItemsSource = lstHotels;
    }
    #endregion

    #region LoadPrograms
    /// <summary>
    /// Llena el combobox de Programs
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// [edgrodriguez] 21/05/2016 Modified. El metodo GetPrograms se volvió asincrónico.
    /// </history>
    private async void LoadPrograms()
    {
      List<Program> lstPrograms = await BRPrograms.GetPrograms();
      if(enumMode==EnumMode.search)
      {
        lstPrograms.Insert(0, new Program { pgID = "", pgN = "" });
      }
      cmblspg.ItemsSource = lstPrograms;
    }
    #endregion

    #region LoadSalesRoom
    /// <summary>
    /// Carga el combobox de SalesRoom
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// [erosado] 24/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadSalesRoom()
    {
      List<SalesRoomShort> lstSalesRooms =await  BRSalesRooms.GetSalesRooms(1);
      if(enumMode==EnumMode.search)
      {
        lstSalesRooms.Insert(0, new SalesRoomShort { srID = "", srN = "" });
      }
      cmblssr.ItemsSource = lstSalesRooms;
    }
    #endregion 

    #region LoadAreas
    /// <summary>
    /// Carga el combobox de Areas
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private void LoadAreas()
    {
      List<Area> lstAreas = BRAreas.GetAreas(nStatus:1);
      if(enumMode==EnumMode.search)
      {
        lstAreas.Insert(0, new Area { arID = "", arN = "" });
      }
      cmblsar.ItemsSource = lstAreas;
    }
    #endregion

    #region LoadRegions
    /// <summary>
    /// Llena el combobox de regiones
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private void LoadRegions()
    {
      List<Region> lstRegions = BRRegions.GetRegions(1);
      if(enumMode==EnumMode.search)
      {
        lstRegions.Insert(0, new Region { rgID = "", rgN = "" });
      }
      cmblsrg.ItemsSource = lstRegions;
    }
    #endregion

    #region LoadSegmentsByLeadSources
    /// <summary>
    /// Llena el combobox de LeadSources
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private void LoadSegmentsByLeadSources()
    {
      List<SegmentByLeadSource> lstSegments = BRSegmentsByLeadSource.GetSegmentsByLeadSource();
      if(enumMode==EnumMode.search)
      {
        lstSegments.Insert(0, new SegmentByLeadSource { soID = "", soN = "" });
      }
      cmblsso.ItemsSource = lstSegments;
    }
    #endregion

    #region LoadBoss
    /// <summary>
    /// Carga el combobox de patrones
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadBoss()
    {
      List<PersonnelShort> lstBoss = await BRPersonnel.GetPersonnel(roles: EnumToListHelper.GetEnumDescription(EnumRole.Boss));
      cmblsBoss.ItemsSource = lstBoss;
    }
    #endregion

    #region LoadLocations
    /// <summary>
    /// Carga los locations
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private void LoadLocations()
    {
      List<Location> lstAllLocations = BRLocations.GetLocations();
      cmbLocations.ItemsSource = lstAllLocations;
      List<Location> lstLocations = lstAllLocations.Where(lo => lo.lols == leadSource.lsID).ToList();
      dgrLocations.ItemsSource = lstLocations;
      _oldLocations = lstLocations.ToList();
    }
    #endregion

    #region LoadAgencies
    /// <summary>
    /// Llena el combobox de Agencies
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private async void LoadAgencies()
    {
      List<Agency> lstAgencies =await BRAgencies.GetAgencies();
      cmbAgencies.ItemsSource = lstAgencies;
    }
    #endregion

    #region LoadSearch
    /// <summary>
    /// Carga combobox de Status, Regen, Animation
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private void LoadSearch()
    {
      ComboBoxHelper.LoadComboDefault(cmbStatus);
      ComboBoxHelper.LoadComboDefault(cmbRegen);
      ComboBoxHelper.LoadComboDefault(cmbAnimation);
      cmbStatus.SelectedValue = nStatus;
      cmbRegen.SelectedValue = nRegen;
      cmbAnimation.SelectedValue = nAnimation;
    }
    #endregion

    #endregion
    
  }
}
