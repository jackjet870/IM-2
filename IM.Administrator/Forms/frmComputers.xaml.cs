using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

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
      LoadComputers();
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
      frmComputerDetail.mode = Enums.ModeOpen.add;
      if (frmComputerDetail.ShowDialog() == true)
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
      frmSearch.sForm = "Computer";
      frmSearch.Owner = this;
      frmSearch.sID = _computerFilter.cpID;
      frmSearch.sDesc = _computerFilter.cpN;

      if(frmSearch.ShowDialog()==true)
      {
        _computerFilter.cpID = frmSearch.sID;
        _computerFilter.cpN = frmSearch.sDesc;
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
      frmComputerDetail frmComputerDetail = new frmComputerDetail();
      frmComputerDetail.Owner = this;
      frmComputerDetail.mode = Enums.ModeOpen.edit;
      ObjectHelper.CopyProperties(frmComputerDetail.computer, computer);
      if(frmComputerDetail.ShowDialog()==true)
      {
        ObjectHelper.CopyProperties(computer, frmComputerDetail.computer);
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
    private void LoadComputers()
    {
      List<Computer> lstComputers = BRComputers.GetComputers(_computerFilter);
      dgrComputers.ItemsSource = lstComputers;
      if(lstComputers.Count>0)
      {
        GridHelper.SelectRow(dgrComputers, 0);
      }
      StatusBarReg.Content = lstComputers.Count+" Computers.";
    }
    #endregion
    #endregion
  }
}
