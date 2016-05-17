using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Input;

namespace IM.Graph.Forms
{
  public partial class frmGraph : Window
  {
    #region Atributos

    private LeadSource _leadsource;

    #endregion Atributos

    #region Constructores y destructores

    /// <summary>
    /// Contructor Modificado
    /// </summary>
    /// <param name="leadSource"> Leadsource con que se inicia </param>
    /// <history>
    /// [aalcocer] 10/03/2015 Created
    /// </history>
    public frmGraph(LeadSource leadSource)
    {
      InitializeComponent();
      _leadsource = leadSource;
    }

    #endregion Constructores y destructores

    #region Métodos de la forma

    #region btnOK_Click

    /// <summary>
    /// Evento boton que realiza la gráfica
    /// </summary>
    /// <history>
    /// [aalcocer] 05/03/2016 Created
    /// </history>
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      DoGraph();
    }

    #endregion btnOK_Click

    #region frmGraph_Loaded

    /// <summary>
    /// Configura el formulario
    /// </summary>
    /// <history>
    /// [aalcocer] 05/03/2016 Created
    /// </history>
    private void frmGraph_Loaded(object sender, RoutedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    #endregion frmGraph_Loaded

    #region Window_ContentRendered

    /// <summary>
    /// Carga la informacion del formulario
    /// </summary>
    /// <history>
    /// [aalcocer] 05/03/2016 Created
    /// </history>
    private void Window_ContentRendered(object sender, EventArgs e)
    {
      DateTime _serverDate = BRHelpers.GetServerDate();

      // Fecha inicial
      dtpStartDate.SelectedDate = new DateTime(_serverDate.Year, _serverDate.Month, 1);

      //Fecha final
      dtpEndDate.SelectedDate = _serverDate;

      LoadFromFile();

      // Lead Source
      cmbLS.ItemsSource = BRLeadSources.GetLeadSources(1,Model.Enums.EnumProgram.All);
      cmbLS.SelectedValue = _leadsource.lsID;
      // Realiza la gráfica
      DoGraph();
    }

    #endregion Window_ContentRendered

    #region frmMailOuts_KeyDown

    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [aalcocer] 08/03/2016 Created
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

    #endregion frmMailOuts_KeyDown

    #endregion Métodos de la forma

    #region Métodos privados

    #region LoadFromFile

    /// <summary>
    /// Carga la informacion de las fechas desde el archivo de configuracion
    /// </summary>
    /// <history>
    /// [aalcocer] 05/03/2016 Created
    /// </history>
    private void LoadFromFile()
    {
      string strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (File.Exists(strArchivo))
      {
        IniFileHelper _iniFileHelper = new IniFileHelper(strArchivo);
        dtpStartDate.SelectedDate = _iniFileHelper.readDate("FilterDate", "DateStart", dtpStartDate.SelectedDate.Value);
        dtpEndDate.SelectedDate = _iniFileHelper.readDate("FilterDate", "DateEnd", dtpEndDate.SelectedDate.Value);
      }
    }

    #endregion LoadFromFile

    #region DoGraph

    /// <summary>
    /// Realiza la gráfica
    /// </summary>
    /// <history>
    /// [aalcocer] 05/03/2016 Created
    /// </history>
    private void DoGraph()
    {
      StaStart();
      lblTitleGraph.Content = cmbLS.Text;

      List<GraphProductionByPR> _lstGraphProductionByPRData = BRGraph.GetGraphProductionByPR(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value, cmbLS.SelectedValue.ToString());
      List<GraphProductionByPR> _lstgeneralGraphProductionByPRAux = new List<GraphProductionByPR>();
      _lstgeneralGraphProductionByPRAux.Add(new GraphProductionByPR
      {
        PR = "General Production",
        Books = _lstGraphProductionByPRData.Sum(lst => lst.Books),
        Shows = _lstGraphProductionByPRData.Sum(lst => lst.Shows),
        Sales = _lstGraphProductionByPRData.Sum(lst => lst.Sales)
      });
      DynamicGraphSize(_lstGraphProductionByPRData.Count);

      chartProduccion.Series.OfType<ColumnSeries>().ToList().ForEach(serie => serie.ItemsSource = _lstGraphProductionByPRData);
      chartGeneralProduccion.Series.OfType<ColumnSeries>().ToList().ForEach(serie => serie.ItemsSource = _lstgeneralGraphProductionByPRAux);

      if (_lstGraphProductionByPRData.Count == 0)
        lblTitleGraph.Content += " There is no data.";
      StaEnd();
    }

    #endregion DoGraph

    #region DynamicGraphSize

    /// <summary>
    ///Cambia el ancho de la grafica segun el tamaño de sus resultados
    /// </summary>
    /// <param name="count"></param>
    /// <history>
    /// [aalcocer] 05/03/2016 Created
    /// </history>
    private void DynamicGraphSize(int count)
    {
      if (count > 8)
        chartProduccion.Width = 100 * count;
      else
        chartProduccion.Width = Double.NaN;
    }

    #endregion DynamicGraphSize

    #region StaEnd

    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    /// <history>
    /// [aalcocer] 05/03/2016 Created
    /// </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = String.Empty;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
      UIHelper.ForceUIToUpdate();
    }

    #endregion StaEnd

    #region StaStart

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje</param>
    /// <history>
    /// [aalcocer] 05/03/2016 Created
    /// </history>
    private void StaStart(String message = "Loading Report...")
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
      UIHelper.ForceUIToUpdate();
    }

    #endregion StaStart

    #endregion Métodos privados
  }
}