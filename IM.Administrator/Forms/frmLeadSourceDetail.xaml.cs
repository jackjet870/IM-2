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
    bool blnClosing = false;
    bool isCellCancel = false; 
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
      LoadPrograms();
      LoadSalesRoom();
      LoadAreas();
      LoadRegions();
      LoadSegmentsByLeadSources();
      if (enumMode==EnumMode.search)
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
        ResizeMode = ResizeMode.NoResize;
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
        LoadBoss();
        LoadHotels();
      }
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
        Close();
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
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<Location> lstLocations = (List<Location>)dgrLocations.ItemsSource;
      List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
      #region Save
      if (enumMode != EnumMode.search)
      {
        if (enumMode != EnumMode.add && ObjectHelper.IsEquals(leadSource, oldLeadSource) && ObjectHelper.IsListEquals(_oldAgencies, lstAgencies) && ObjectHelper.IsListEquals(_oldLocations, lstLocations))
        {
          blnClosing = true;
          Close();
        }
        else
        {
          #region Save
          txtStatus.Text = "Saving Data...";
          skpStatus.Visibility = Visibility.Visible;
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

            int nRes =await BRLeadSources.SaveLeadSource(leadSource,lstLocAdd,lstLocDel,lstAgeAdd,lstAgeDel,(enumMode==EnumMode.edit));
            UIHelper.ShowMessageResult("Lead Source", nRes);
            if(nRes>0)
            {
              blnClosing = true;
              DialogResult = true;
              Close();
            }
          }
          else
          {
            UIHelper.ShowMessage(strMsj);
          }
          skpStatus.Visibility = Visibility.Collapsed;
          #endregion
        }
       }
      #endregion
      #region Search
      else
      {
        blnClosing = true;
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
      btnCancel.Focus();
      if(enumMode!=EnumMode.preview && enumMode!=EnumMode.search)
      {
        List<Agency> lstAgencies = (List<Agency>)dgrAgencies.ItemsSource;
        List<Location> lstLocations = (List<Location>)dgrLocations.ItemsSource;
        if (!ObjectHelper.IsEquals(leadSource, oldLeadSource) || !ObjectHelper.IsListEquals(lstLocations, _oldLocations) || !ObjectHelper.IsListEquals(lstAgencies,_oldAgencies))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!blnClosing) { blnClosing = true; Close(); }
          }
          else
          {
            blnClosing = false;
          }
        }
        else
        {
          if (!blnClosing) { blnClosing = true; Close(); }
        }
      }
      else
      {
        if (!blnClosing) { blnClosing = true; Close(); }
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
    private void dgr_CellEditEnding(object sender,DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        isCellCancel = false;
        DataGrid dg = sender as DataGrid;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dg);
        e.Cancel = isRepeat;
      }
      else
      {
        isCellCancel = true;
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica cambios al cerrar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing)
      {
        blnClosing = true;
        btnCancel_Click(null, null);
        if (!blnClosing)
        {
          e.Cancel = true;
        }
      }
    }
    #endregion

    #region dgr_RowEditEnding
    /// <summary>
    /// No permite que se agreguen filas vacias
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void dgr_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      DataGrid dgr = (DataGrid)sender;
      if (isCellCancel)
      {
        dgr.RowEditEnding -= dgr_RowEditEnding;
        dgr.CancelEdit();
        dgr.RowEditEnding += dgr_RowEditEnding;
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
      try
      {
        List<Hotel> lstHotels = await BRHotels.GetHotels(nStatus: 1);
        cmbHotels.ItemsSource = lstHotels;
        skpStatus.Visibility = Visibility.Hidden;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Lead Source");
      }
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
      try
      {
        List<Program> lstPrograms = await BRPrograms.GetPrograms();
        if (enumMode == EnumMode.search)
        {
          lstPrograms.Insert(0, new Program { pgID = "", pgN = "ALL" });
        }
        cmblspg.ItemsSource = lstPrograms;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Lead Source");
      }
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
      try
      {
        List<SalesRoomShort> lstSalesRooms = await BRSalesRooms.GetSalesRooms(1);
        if (enumMode == EnumMode.search)
        {
          lstSalesRooms.Insert(0, new SalesRoomShort { srID = "", srN = "ALL" });
        }
        cmblssr.ItemsSource = lstSalesRooms;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Lead Source");
      }
    }
    #endregion 

    #region LoadAreas
    /// <summary>
    /// Carga el combobox de Areas
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private async void LoadAreas()
    {
      try
      {
        List<Area> lstAreas = await BRAreas.GetAreas(nStatus: 1);
        if (enumMode == EnumMode.search)
        {
          lstAreas.Insert(0, new Area { arID = "", arN = "ALL" });
        }
        cmblsar.ItemsSource = lstAreas;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Lead Source");
      }
    }
    #endregion

    #region LoadRegions
    /// <summary>
    /// Llena el combobox de regiones
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private async void LoadRegions()
    {
      try
      {
        List<Region> lstRegions = await BRRegions.GetRegions(1);
        if (enumMode == EnumMode.search)
        {
          lstRegions.Insert(0, new Region { rgID = "", rgN = "ALL" });
        }
        cmblsrg.ItemsSource = lstRegions;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Lead Source");
      }
    }
    #endregion

    #region LoadSegmentsByLeadSources
    /// <summary>
    /// Llena el combobox de LeadSources
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private async void LoadSegmentsByLeadSources()
    {
      try
      {
        List<SegmentByLeadSource> lstSegments = await BRSegmentsByLeadSource.GetSegmentsByLeadSource();
        if (enumMode == EnumMode.search)
        {
          lstSegments.Insert(0, new SegmentByLeadSource { soID = "", soN = "ALL" });
          skpStatus.Visibility = Visibility.Collapsed;
        }
        cmblsso.ItemsSource = lstSegments;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Lead Sources");
      }
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
      try
      {
        List<PersonnelShort> lstBoss = await BRPersonnel.GetPersonnel(roles: EnumToListHelper.GetEnumDescription(EnumRole.Boss));
        cmblsBoss.ItemsSource = lstBoss;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Lead Source");
      }
    }
    #endregion

    #region LoadLocations
    /// <summary>
    /// Carga los locations
    /// </summary>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    private async void LoadLocations()
    {
      try
      {
        List<Location> lstAllLocations = await BRLocations.GetLocations();
        cmbLocations.ItemsSource = lstAllLocations;
        List<Location> lstLocations = lstAllLocations.Where(lo => lo.lols == leadSource.lsID).ToList();
        dgrLocations.ItemsSource = lstLocations;
        _oldLocations = lstLocations.ToList();
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Lead Source");
      }
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
      try
      {
        List<Agency> lstAgencies = await BRAgencies.GetAgencies();
        cmbAgencies.ItemsSource = lstAgencies;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Lead Source");
      }
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
