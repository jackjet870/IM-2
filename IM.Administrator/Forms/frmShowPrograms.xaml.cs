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
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmShowPrograms.xaml
  /// </summary>
  public partial class frmShowPrograms : Window
  {
    #region Variables
    private ShowProgram _showProgramFilter = new ShowProgram();//Filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmShowPrograms()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 25/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region KeyDown Form
    /// <summary>
    /// Verfica teclas precionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/04/2016
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

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/04/2016
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

    #region DoubleClick cell
    /// <summary>
    /// Muestra la ventana detalle en modo edit
    /// </summary>
    /// <history>
    /// [emoguel] 25/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      ShowProgram showProgram = (ShowProgram)dgrShowPrograms.SelectedItem;
      frmShowProgramDetail frmShowProgramDetail = new frmShowProgramDetail();
      frmShowProgramDetail.Owner = this;
      frmShowProgramDetail.oldShowProgram = showProgram;
      frmShowProgramDetail.enumMode = EnumMode.edit;
      if(frmShowProgramDetail.ShowDialog()==true)
      {
        List<ShowProgram> lstShowPrograms = (List<ShowProgram>)dgrShowPrograms.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmShowProgramDetail.showProgram))//Validar que cumpla con los filtros
        {
          ObjectHelper.CopyProperties(showProgram, frmShowProgramDetail.showProgram);//Actualizar los datos
          lstShowPrograms.Sort((x, y) => string.Compare(x.skN, y.skN));//Ordenar la lista
          nIndex = lstShowPrograms.IndexOf(showProgram);//Obtener la posición del registro
        }
        else
        {
          lstShowPrograms.Remove(showProgram);//Remover el registro
        }
        dgrShowPrograms.Items.Refresh();//Actualizar la vista
        GridHelper.SelectRow(dgrShowPrograms, nIndex);//Seleccionar el registro
        StatusBarReg.Content = lstShowPrograms.Count + " Show Programs.";//Actualizar el contador
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana detalle en modo Search
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmShowProgramDetail frmShowProgramDetail = new frmShowProgramDetail();
      frmShowProgramDetail.Owner = this;
      frmShowProgramDetail.enumMode = EnumMode.search;
      frmShowProgramDetail.oldShowProgram = _showProgramFilter;
      frmShowProgramDetail.nStatus = _nStatus;
      if(frmShowProgramDetail.ShowDialog()==true)
      {
        _nStatus = frmShowProgramDetail.nStatus;
        _showProgramFilter = frmShowProgramDetail.showProgram;
        LoadShowPrograms();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmShowProgramDetail frmShowProgramDetail = new frmShowProgramDetail();
      frmShowProgramDetail.Owner = this;
      frmShowProgramDetail.enumMode = EnumMode.add;
      if(frmShowProgramDetail.ShowDialog()==true)
      {
        ShowProgram showProgram = frmShowProgramDetail.showProgram;
        if(ValidateFilter(showProgram))//Validar si cumple con los filtros
        {
          List<ShowProgram> lstShowPrograms = (List<ShowProgram>)dgrShowPrograms.ItemsSource;
          lstShowPrograms.Add(showProgram);//Agregamos el registro
          lstShowPrograms.Sort((x, y) => string.Compare(x.skN, y.skN));//Ordenar la lista
          int nIndex = lstShowPrograms.IndexOf(showProgram);//Obtener la posición del registro
          dgrShowPrograms.Items.Refresh();//Actualizar la vista
          GridHelper.SelectRow(dgrShowPrograms, nIndex);//Seleccionar un registro
          StatusBarReg.Content = lstShowPrograms.Count + " Show Programs.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza el grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param><
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      ShowProgram showProgram = (ShowProgram)dgrShowPrograms.SelectedItem;
      LoadShowPrograms(showProgram);
    }
    #endregion

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadShowPrograms();
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadShowPrograms
    /// <summary>
    /// Llena el grid
    /// </summary>
    /// <param name="showProgram">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    private void LoadShowPrograms(ShowProgram showProgram = null)
    {
      int nIndex = 0;
      List<ShowProgram> lstShowPrograms = BRShowPrograms.GetShowPrograms(_nStatus, _showProgramFilter);
      dgrShowPrograms.ItemsSource = lstShowPrograms;
      if (lstShowPrograms.Count > 0 && showProgram != null)
      {
        showProgram = lstShowPrograms.Where(sk => sk.skID == showProgram.skID).FirstOrDefault();
        nIndex = lstShowPrograms.IndexOf(showProgram);
      }
      GridHelper.SelectRow(dgrShowPrograms, nIndex);
      StatusBarReg.Content = lstShowPrograms.Count + " Show Programs.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un show program cumpla con los filtros Actuales
    /// </summary>
    /// <param name="showProgram">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    private bool ValidateFilter(ShowProgram showProgram)
    {
      if (_nStatus != -1)//Filtro por Status
      {
        if (showProgram.skA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_showProgramFilter.skID))//Filtro por ID
      {
        if (_showProgramFilter.skID != showProgram.skID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_showProgramFilter.skN))//Filtro por descripción
      {
        if (!showProgram.skN.Contains(_showProgramFilter.skN))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_showProgramFilter.sksg))//Filtro por categoria
      {
        if (_showProgramFilter.sksg != showProgram.sksg)
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
