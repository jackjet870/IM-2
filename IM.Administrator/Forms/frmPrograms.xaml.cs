using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPrograms.xaml
  /// </summary>
  public partial class frmPrograms : Window
  {
    #region Variables
    private bool _blnEdit = false;
    #endregion
    public frmPrograms()
    {
      InitializeComponent();
    }

    #region Method Forms
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      status.Visibility = Visibility.Visible;
      _blnEdit = Context.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadPrograms();
    } 
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/05/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window keyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
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
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
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

    #region DobleClic Grid
    /// <summary>
    /// Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Program program = (Program)dgrPrograms.SelectedItem;
      frmProgramDetail frmProgramDetail = new frmProgramDetail();
      frmProgramDetail.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      frmProgramDetail.oldProgram = program;
      if(frmProgramDetail.ShowDialog()==true)
      {
        List<Program> lstPrograms = new List<Program>();
        ObjectHelper.CopyProperties(program, frmProgramDetail.program);//Actualizamos los datos del registro
        lstPrograms.Sort((x, y) => string.Compare(x.pgN, y.pgN));//Ordenamos la lista
        int nIndex = lstPrograms.IndexOf(program);//Obtenemos la posición del registro
        dgrPrograms.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrPrograms, nIndex);//Seleccionamos el registro
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
    /// [emoguel] created 25/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmProgramDetail frmProgramDetail = new frmProgramDetail();
      frmProgramDetail.Owner = this;
      frmProgramDetail.enumMode = EnumMode.Add;
      if(frmProgramDetail.ShowDialog()==true)
      {
        List<Program> lstPrograms = (List<Program>)dgrPrograms.ItemsSource;
        lstPrograms.Add(frmProgramDetail.program);//Agregamos el registro
        lstPrograms.Sort((x, y) => string.Compare(x.pgN, y.pgN));//Ordenamos la lista
        int nIndex = lstPrograms.IndexOf(frmProgramDetail.program);//Buscamos la posicion del registro
        dgrPrograms.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrPrograms, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstPrograms.Count + "Programs";//Actualizamos el contador
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      status.Visibility = Visibility.Visible;
      Program program = (Program)dgrPrograms.SelectedItem;
      LoadPrograms(program);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadPrograms
    /// <summary>
    /// Carga el grid de Programs
    /// </summary>
    /// <param name="program">OBjeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private async void LoadPrograms(Program program = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Program> lstPrograms = await BRPrograms.GetPrograms();
        dgrPrograms.ItemsSource = lstPrograms;
        if (lstPrograms.Count > 0 && program != null)
        {
          program = lstPrograms.Where(pg => pg.pgID == program.pgID).FirstOrDefault();
          nIndex = lstPrograms.IndexOf(program);
        }
        GridHelper.SelectRow(dgrPrograms, nIndex);
        StatusBarReg.Content = lstPrograms.Count + " Programs.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion 
    #endregion
  }
}
