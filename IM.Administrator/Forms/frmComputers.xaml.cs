using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using System.Linq;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmComputers.xaml
  /// </summary>
  public partial class frmComputers : Window
  {
    private Computer _computerFilter = new Computer();//Objeto con los filtros del grid    

    public frmComputers()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region WindowKeyDown
    /// <summary>
    /// Valida las teclas presionadas para cambiar la barra de estado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 16/03/2016
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

    #region Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadComputers();
    }
    #endregion

    #region KeyboardFocusChanged
    /// <summary>
    /// Verifica las teclas presionadas cuando se le cambia el foco a otra ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    #endregion

    #region Refresh
    /// <summary>
    /// Recarga el grid de Computers
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Computer computer = (Computer)dgrComputers.SelectedItem;
      LoadComputers(computer);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la centana computerDetail en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmComputerDetail frmComputerDetail = new frmComputerDetail();
      frmComputerDetail.Owner = this;
      frmComputerDetail.mode = EnumMode.add;
      if (frmComputerDetail.ShowDialog() == true)
      {
        if (ValidateFilters(frmComputerDetail.computer))//Validamos que cumpla con los filtros
        {
          List<Computer> lstComputers = (List<Computer>)dgrComputers.ItemsSource;
          lstComputers.Add(frmComputerDetail.computer);//Agregamos el registro nuevo
          lstComputers.Sort((x, y) => string.Compare(x.cpN, y.cpN));//ordenamos la lista        
          int nIndex = lstComputers.IndexOf(frmComputerDetail.computer);//obtenemos el index del registro nuevo
          dgrComputers.Items.Refresh();//refrescamos la lista        
          GridHelper.SelectRow(dgrComputers, nIndex);
          StatusBarReg.Content = lstComputers.Count + " Computers.";
        }
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de filtros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.enumWindow = EnumWindow.Computers;
      frmSearch.Owner = this;
      frmSearch.strID = _computerFilter.cpID;
      frmSearch.strDesc = _computerFilter.cpN;

      if(frmSearch.ShowDialog()==true)
      {
        _computerFilter.cpID = frmSearch.strID;
        _computerFilter.cpN = frmSearch.strDesc;
        LoadComputers();
      }
    }
    #endregion

    #region Cell Double Click
    /// <summary>
    /// Muestra la ventada computerDetail dependiendo de los permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 16/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Computer computer = (Computer)dgrComputers.SelectedItem;
      frmComputerDetail frmComDetail = new frmComputerDetail();
      frmComDetail.Owner = this;
      frmComDetail.mode = EnumMode.edit;
      frmComDetail.oldComputer = computer;
      frmComDetail.ShowDialog();
      if ( frmComDetail.DialogResult== true)
      {        
        List<Computer> lstComputers = (List<Computer>)dgrComputers.ItemsSource;
        int nIndex = 0;        
        if (!ValidateFilters(frmComDetail.computer))//Validamos que cumpla con los filtros
        {
          lstComputers.Remove(computer);//Quitamos el registro de la lista
        }
        else
        {
          ObjectHelper.CopyProperties(computer, frmComDetail.computer);
          lstComputers.Sort((x, y) => string.Compare(x.cpN, y.cpN));//ordenamos la lista     
          nIndex = lstComputers.IndexOf(computer);
        }                   
        dgrComputers.Items.Refresh();//refrescamos la lista        
        GridHelper.SelectRow(dgrComputers, nIndex);
        StatusBarReg.Content = lstComputers.Count + " Computers.";
      }

    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            Cell_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }

    #endregion
    #endregion

    #region metodos
    #region LoadComputers
    /// <summary>
    /// Llena el grid de computers
    /// </summary>
    /// <history>
    /// [emoguel] created 16/03/2016
    /// </history>
    private void LoadComputers(Computer computer=null)
    {
      int nIndex = 0;
      List<Computer> lstComputers = BRComputers.GetComputers(_computerFilter);
      dgrComputers.ItemsSource = lstComputers;      
      if(computer!=null && lstComputers.Count>0)
      {
        computer = lstComputers.Where(co => co.cpID == computer.cpID).FirstOrDefault();
        nIndex = lstComputers.IndexOf(computer);
      }
      GridHelper.SelectRow(dgrComputers, nIndex);
      StatusBarReg.Content = lstComputers.Count+" Computers.";
    }
    #endregion

    #region Validate Filters
    /// <summary>
    /// Valida si una entidad de tipo Computer coincide con los filtros
    /// </summary>
    /// <param name="newComputer">Objeto a validar</param>
    /// <returns>true. Si se muestra | false. Nose muestra</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private bool ValidateFilters(Computer newComputer)
    {

      if (!string.IsNullOrWhiteSpace(_computerFilter.cpID))
      {
        if (_computerFilter.cpID != newComputer.cpID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_computerFilter.cpN))
      {
        if (!newComputer.cpN.Contains(_computerFilter.cpN))
        {
          return false;
        }
      }
      return true;
    }
    #endregion
    #endregion
  }
}
