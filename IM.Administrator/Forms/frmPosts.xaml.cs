using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPosts.xaml
  /// </summary>
  public partial class frmPosts : Window
  {
    #region variables
    private Post _postFilter = new Post();//objeto con los filtros del grid
    private int _nStatus = -1;//Estatus con los registros del grid
    private bool _blnEdit = false;//boleano para saber si se tiene permiso para editar
    #endregion
    public frmPosts()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window Loaded
    /// <summary>
    /// Carga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Teams, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadPosts();
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
      Post post = (Post)dgrPosts.SelectedItem;
      frmPostDetail frmPostDetail = new frmPostDetail();
      frmPostDetail.Owner = this;
      frmPostDetail.oldPost = post;
      frmPostDetail.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      if(frmPostDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Post> lstPosts = (List<Post>)dgrPosts.ItemsSource;
        if (ValidateFilter(frmPostDetail.post))//Verificamos si cumple con los filtros actuales
        {
          ObjectHelper.CopyProperties(post, frmPostDetail.post);//Actualizamos los datos del registro
          lstPosts.Sort((x, y) => string.Compare(x.poN, y.poN));//Ordenamos la lista
          nIndex = lstPosts.IndexOf(post);//buscamos la posición del registro nuevo
        }
        else
        {
          lstPosts.Remove(post);//Quitamos el registro de la lista
        }
        dgrPosts.Items.Refresh();
        GridHelper.SelectRow(dgrPosts, nIndex);
        StatusBarReg.Content = lstPosts.Count + " Posts.";
      }
    }

    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _postFilter.poID;
      frmSearch.strDesc = _postFilter.poN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _postFilter.poID = frmSearch.strID;
        _postFilter.poN = frmSearch.strDesc;
        LoadPosts();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmPostDetail frmPostDetail = new frmPostDetail();
      frmPostDetail.Owner = this;
      frmPostDetail.enumMode = EnumMode.Add;
      if(frmPostDetail.ShowDialog()==true)
      {
        Post post = frmPostDetail.post;
        if(ValidateFilter(post))//verificamos si cumple con los filtros actuales
        {
          List<Post> lstFiltros = (List<Post>)dgrPosts.ItemsSource;
          lstFiltros.Add(post);//Agreamos un registro nuevo
          lstFiltros.Sort((x, y) => string.Compare(x.poN, y.poN));//Ordenamos la lista 
          int nIndex = lstFiltros.IndexOf(post);//Obtenemos la posición del registro
          dgrPosts.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPosts, nIndex);//Seleccionamos el registro nuevo
          StatusBarReg.Content = lstFiltros.Count + " Post.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Post post = (Post)dgrPosts.SelectedItem;
      LoadPosts(post);
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadPosts
    /// <summary>
    /// Llena el grid de Posts
    /// </summary>
    /// <param name="post">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private  async void LoadPosts(Post post = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Post> lstPost = await BRPosts.GetPosts(_nStatus, _postFilter);
        dgrPosts.ItemsSource = lstPost;
        if (lstPost.Count > 0 && post != null)
        {
          post = lstPost.Where(po => po.poID == post.poID).FirstOrDefault();
          nIndex = lstPost.IndexOf(post);
        }
        GridHelper.SelectRow(dgrPosts, nIndex);
        StatusBarReg.Content = lstPost.Count + " Posts.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Posts");
      }
    }
    #endregion
    #region ValidateFilter
    /// <summary>
    /// Valida que un objeto cumpla con los filtros actuales
    /// </summary>
    /// <param name="post">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private bool ValidateFilter(Post post)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(post.poA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_postFilter.poID))//Filtro por ID
      {
        if(_postFilter.poID!=post.poID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_postFilter.poN))
      {
        if(!post.poN.Contains(_postFilter.poN,StringComparison.OrdinalIgnoreCase))
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
