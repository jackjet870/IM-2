using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmShowsSalesmen.xaml
  /// </summary>
  public partial class frmShowsSalesmen : Window
  {
    #region Atributos

    private readonly ObservableCollection<ShowSalesman> _showSalesmanList;
    private List<ShowSalesman> _oldShowSalesmanList;
    private readonly int _guestCurrent;

    #endregion Atributos

    #region Constructores y destructores

    public frmShowsSalesmen(int guestID, List<ShowSalesman> showSalesmanList)
    {
      _guestCurrent = guestID;
      InitializeComponent();
      _showSalesmanList = new ObservableCollection<ShowSalesman>(showSalesmanList);
      DataContext = _showSalesmanList;
    }

    #endregion Constructores y destructores

    #region Metodos

    #region SetMode

    /// <summary>
    /// Habilita / deshabilita los controles del formulario segun el modo de datos
    /// </summary>
    /// <param name="enumMode"></param>
    /// <history>
    /// [aalcocer] 10/08/2016 created
    /// </history>
    private void SetMode(EnumMode enumMode)
    {
      var blnEnable = enumMode != EnumMode.ReadOnly;
      btnCancel.IsEnabled = btnSave.IsEnabled = blnEnable;
      btnEdit.IsEnabled = !blnEnable;
      dtgShowSalesman.IsReadOnly = !blnEnable;
    }

    #endregion SetMode

    #region LoadShowSalesman

    /// <summary>
    /// Carga los shows de vendedores
    /// </summary>
    /// <history>
    /// [aalcocer] 10/08/2016 created
    /// </history>
    private async void LoadShowSalesman()
    {
      var showsSalesmen = await BRShowSalesman.GetShowsSalesmen(_guestCurrent);
      _showSalesmanList.ToList().ForEach(ss =>
      {
        var showSalesmen = showsSalesmen.SingleOrDefault(s => s.shpe == ss.shpe);
        if (showSalesmen != null)
          ss.shUp = showSalesmen.shUp;
      });

      dtgShowSalesman.Items.Refresh();
    }

    #endregion LoadShowSalesman

    #region SaveShowSalesman

    /// <summary>
    /// Guarda los shows de vendedores
    /// </summary>
    /// <history>
    /// [aalcocer] 10/08/2016 created
    /// </history>
    private async void SaveShowSalesman()
    {
      //Guardamos los ShowSalesman
      try
      {
        var res = await BRShowSalesman.SaveShowsSalesmen(_guestCurrent, _showSalesmanList.Where(ss => !ss.shUp).ToList());

        //Si no ocurrio un problema al momento de guardar, mostramos el mensaje
        //de los contrario se ira al catch y alli nos mostrara el mensaje de error en especifico
        UIHelper.ShowMessageResult(Title, res);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion SaveShowSalesman

    #endregion Metodos

    #region Eventos del formulario

    #region Window_Loaded

    /// <summary>
    /// Carga e inicializa el formulario
    /// </summary>
    /// <history>
    /// [aalcocer] 70/08/2016 created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //cargamos Show salesmen
      LoadShowSalesman();

      SetMode(EnumMode.ReadOnly);
    }

    #endregion Window_Loaded

    #region btnEdit_Click

    /// <summary>
    /// Modo edicion
    /// </summary>
    /// <history>
    /// [aalcocer] 10/08/2016 created
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      SetMode(EnumMode.Edit);

      _oldShowSalesmanList = new List<ShowSalesman>();
      //Se hace un Foreach a ShowSalesman  y llenamos a _oldShowSalesmanList, Se hace de esta forma para que no tengan la misma referencia
      // y asi se puede modificar uno sin q el otro cambie igual
      _showSalesmanList.ToList().ForEach(item =>
      {
        var ss = ObjectHelper.CopyProperties(item, true);
        _oldShowSalesmanList.Add(ss);
      });
    }

    #endregion btnEdit_Click

    #region btnSave_Click

    /// <summary>
    /// Modo para guardar
    /// </summary>
    /// <history>
    /// [aalcocer] 10/08/2016 created
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      SaveShowSalesman();
      SetMode(EnumMode.ReadOnly);
    }

    #endregion btnSave_Click

    #region btnCancel_Click

    /// <summary>
    ///  Modo cancel
    /// </summary>
    /// <history>
    /// [aalcocer] 10/08/2016 created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      _showSalesmanList.Clear();
      _oldShowSalesmanList.ForEach(x => _showSalesmanList.Add(x));
      SetMode(EnumMode.ReadOnly);
    }

    #endregion btnCancel_Click

    #endregion Eventos del formulario
  }
}