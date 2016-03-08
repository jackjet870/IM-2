using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Administrator.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Administrator.Helpers;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAgencies.xaml
  /// </summary>
  public partial class frmAgencies : Window
  {
    private Agency _agencyFilter=new Agency();//Objeto para filtrar en la BD
    private int _nStatus = -1;//Status a filtrar en la lista
    private bool _blnEdit = false;//boleano para saber si se tiene minimo permiso para editar|agregar 

    public frmAgencies()
    {
      InitializeComponent();
    }

    #region event controls
    #region Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = PermisionHelper.EditPermision("AGENCIES");
      LoadAgencies();
      btnAdd.IsEnabled = _blnEdit;
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region keyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            break;
          }
      }
    }
    #endregion

    #region refresh
    /// <summary>
    /// Recarga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadAgencies();
    }
    #endregion

    #region Boton agregar
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region Boton editar

    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region Boton Buscar

    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {

    } 
    #endregion
    #endregion
    #region metodos
    /// <summary>
    /// Llena el grid de Agencies dependiendo de los filtros seleccionados
    /// </summary>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    protected void LoadAgencies()
    {
      List<Agency> lstAgencies = BRAgencies.GetAgencies(_agencyFilter, _nStatus);
      dgrAgencies.ItemsSource = lstAgencies;
      if(lstAgencies.Count>0)
      {
        btnEdit.IsEnabled = _blnEdit;
        dgrAgencies.SelectedIndex = 0;
      }
      else
      {
        btnEdit.IsEnabled = false;
      }

    }
    #endregion

  }
}
