using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPostsLog.xaml
  /// </summary>
  public partial class frmPostsLog : Window
  {
    #region Variables
    private PostLog _postLogFilter = new PostLog();//Objeto con los filtros de la ven
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar
    private bool _blnDel = false;//Permiso para saber si se tiene permiso para eliminar
    private bool _blnDate = false;//Boleano para saber si va a filtrar por ppDT    
    #endregion
    public frmPostsLog()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] creted 12/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Personnel, EnumPermisionLevel.Special);
      _blnDel = App.User.HasPermission(EnumPermission.Personnel, EnumPermisionLevel.SuperSpecial);
      btnAdd.IsEnabled = _blnEdit;
      btnDel.IsEnabled = _blnDel;
      LoadPostLogs();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 11/04/2016
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
    /// [emoguel] created 11/04/2016
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
    /// Muestra la ventana detalle
    /// </summary>
    /// <history>
    /// [emoguel] 11/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      PostLog postLog = (PostLog)dgrPostsLog.SelectedItem;
      frmPostLogDetail frmPostLogDetail = new frmPostLogDetail();
      frmPostLogDetail.Owner = this;
      frmPostLogDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.preview;
      frmPostLogDetail.oldPostLog = postLog;
      if(frmPostLogDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<PostLog> lstPostsLog = (List<PostLog>)dgrPostsLog.ItemsSource;
        if(ValidateFilter(frmPostLogDetail.postLog))
        {
          ObjectHelper.CopyProperties(postLog, frmPostLogDetail.postLog, true);//Actualizamos los datos del objeto
          lstPostsLog.Sort((x, y) => y.ppDT.CompareTo(x.ppDT));//Ordenamos la lista
          nIndex = lstPostsLog.IndexOf(postLog);//Obtenemos la posición
        }
        else
        {
          lstPostsLog.Remove(postLog);//Quitamos el registro
        }
        dgrPostsLog.Items.Refresh();
        GridHelper.SelectRow(dgrPostsLog, nIndex);
        StatusBarReg.Content = lstPostsLog.Count + " Posts Log.";
      }
    }

    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana detalle en modo busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmPostLogDetail frmPostLogDetail = new frmPostLogDetail();
      frmPostLogDetail.Owner = this;
      frmPostLogDetail.oldPostLog = _postLogFilter;
      frmPostLogDetail.enumMode = EnumMode.search;
      frmPostLogDetail.blnDate = _blnDate;
      if (frmPostLogDetail.ShowDialog() == true)
      {        
        _postLogFilter= frmPostLogDetail.postLog;
        _blnDate = frmPostLogDetail.blnDate;
        LoadPostLogs(); 
      }      
    } 
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmPostLogDetail frmPostLogDetail = new frmPostLogDetail();
      frmPostLogDetail.Owner = this;
      frmPostLogDetail.enumMode = EnumMode.add;
      if(frmPostLogDetail.ShowDialog()==true)
      {
        PostLog postLog = frmPostLogDetail.postLog;
        if(ValidateFilter(postLog))
        {
          List<PostLog> lstPostsLog = (List<PostLog>)dgrPostsLog.ItemsSource;
          lstPostsLog.Add(postLog);//Agregamos el registro
          lstPostsLog.Sort((x, y) => y.ppDT.CompareTo(x.ppDT));//ordenamos la lista
          int nIndex = lstPostsLog.IndexOf(postLog);//obtenemos la posición del registro
          dgrPostsLog.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPostsLog, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstPostsLog.Count + " Posts Log.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      PostLog posLog = (PostLog)dgrPostsLog.SelectedItem;
      LoadPostLogs(posLog);
    }
    #endregion

    #region Delete
    /// <summary>
    /// Elimina el registro seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/04/2016
    /// [jorcanche]  se agrego asincronia 28062016
    /// </history>
    private async void btnDel_Click(object sender, RoutedEventArgs e)
    {
      if(dgrPostsLog.SelectedItems.Count>0)
      {       
        List<PostLog> lstPostsLogDel = dgrPostsLog.SelectedItems.OfType<PostLog>().ToList();
        MessageBoxResult msgResult=MessageBoxResult.No;
        #region MessageBox
        if (lstPostsLogDel.Count == 1)
        {
          msgResult = UIHelper.ShowMessage("Are you sure you want to delete this post Log?", MessageBoxImage.Question, "Delete");
        }
        else
        {
          msgResult=UIHelper.ShowMessage("Are you sure you want to delete these posts Log?", MessageBoxImage.Question, "Delete");
        }
        #endregion

        if (msgResult == MessageBoxResult.Yes)
        {
          int nRes = await BREntities.OperationEntities(lstPostsLogDel, EnumMode.deleted);

          if (nRes > 0)
          {
            if (nRes == 1)
            {
              UIHelper.ShowMessage("Posts Log was Deleted.");
            }
            else
            {
              UIHelper.ShowMessage("Posts Log were Deleted.");
            }
            List<PostLog> lstPostLog = (List<PostLog>)dgrPostsLog.ItemsSource;
            lstPostLog.RemoveAll(pp => lstPostsLogDel.Contains(pp));
            dgrPostsLog.Items.Refresh();              
          }
        }
      }
      else
      {
        UIHelper.ShowMessage("Please select a Post Log.");
      }
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadPostLog
    /// <summary>
    /// Carga la lista del grid
    /// </summary>
    /// <param name="postLog">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private void LoadPostLogs(PostLog postLog = null)
    {
      int nIndex = 0;
      List<PostLog> lstPostsLog = BRPostsLog.GetPostsLog(_postLogFilter,_blnDate);
      dgrPostsLog.ItemsSource = lstPostsLog;
      if (lstPostsLog.Count > 0 )
      {
        if (postLog != null)
        {
          postLog = lstPostsLog.Where(pp => pp.ppID == postLog.ppID).FirstOrDefault();
          nIndex = lstPostsLog.IndexOf(postLog);
        }
        
        GridHelper.SelectRow(dgrPostsLog, nIndex);
        btnDel.IsEnabled = _blnDel;
      } 
      else
      {
        btnDel.IsEnabled = false;
      }    
      
      StatusBarReg.Content = lstPostsLog.Count + " Posts Log.";
    }
    #endregion

    #region Validate Filter
    /// <summary>
    /// Valida que un registro cumpla con los filtros actuales
    /// </summary>
    /// <param name="postLog">objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 12/04/2016
    /// </history>
    private bool ValidateFilter(PostLog postLog)
    {
     if(_blnDate)//Filtro por Fecha
      {
        if(postLog.ppDT.Date!=_postLogFilter.ppDT.Date)
        {
          return false;
        }
      } 

     if(!string.IsNullOrWhiteSpace(_postLogFilter.ppChangedBy))//Filtro por Chaged By
      {
        if(_postLogFilter.ppChangedBy!=postLog.ppChangedBy)
        {
          return false;
        }
      }

     if(!string.IsNullOrWhiteSpace(_postLogFilter.pppe))//Filtro por personel
      {
        if(postLog.pppe!=_postLogFilter.pppe)
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
