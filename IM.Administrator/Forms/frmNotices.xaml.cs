using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.Model.Helpers;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmNotices.xaml
  /// </summary>
  public partial class frmNotices : Window
  {
    #region Variables
    private int _nStatus = -1;
    private Notice _noticeFilter = new Notice();
    #endregion
    public frmNotices()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window_Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 23/07/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadNotices();
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
    /// <history>
    /// [emoguel] created 23/07/2016
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
    /// <history>
    /// [emoguel] created 23/07/2016
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
    /// <history>
    /// [emoguel] created 23/07/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      frmNoticeDetail frmNoticeDetail = new frmNoticeDetail();
      frmNoticeDetail.Owner = this;
      frmNoticeDetail.enumMode = EnumMode.Edit;
      Notice notice=dgrNotices.SelectedItem as Notice;
      frmNoticeDetail.oldNotice = notice;
      if(frmNoticeDetail.ShowDialog() == true)
      {
        List<Notice> lstNotices = dgrNotices.ItemsSource as List<Notice>;
        int nIndex = 0;
        if(ValidateFilters(frmNoticeDetail.notice))
        {
          ObjectHelper.CopyProperties(notice, frmNoticeDetail.notice);//Actualizamos los datos
          lstNotices.Sort((x, y) => string.Compare(x.noTitle, y.noTitle));//Ordenamos la lista
          nIndex = lstNotices.IndexOf(notice);//Buscamos la posicion del registro
        }
        else
        {
          lstNotices.Remove(notice);//Quitamos el registro de la lista
        }

        dgrNotices.Items.Refresh();//Actualizamos los datos
        GridHelper.SelectRow(dgrNotices, nIndex);//Seleccionamos el registro
        StatusBarReg.Content="Notices ("+ dgrNotices.Items.Count+") ";//Actualizamos el contador
      }
    }
    #endregion

    #region btnRef_Click
    /// <summary>
    /// Actualiza la lista del grid
    /// </summary>
    /// <history>
    /// [emoguel] created 23/07/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Notice notice=dgrNotices.SelectedItem as Notice;
      LoadNotices(notice);
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    /// Abre la ventana de detalle en modo Add
    /// </summary>
    /// <history>
    /// [emoguel] created 23/07/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmNoticeDetail frmNoticeDetail = new frmNoticeDetail();
      frmNoticeDetail.enumMode = EnumMode.Add;
      if(frmNoticeDetail.ShowDialog()==true)
      {
        if(ValidateFilters(frmNoticeDetail.notice))//Validamos que cumpla con los filtros
        {
          List<Notice> lstNotices = dgrNotices.ItemsSource as List<Notice>;
          lstNotices.Add(frmNoticeDetail.notice);//Agregamos el registro
          lstNotices.Sort((x, y) => string.Compare(x.noTitle, y.noTitle));//Ordenamos la lista
          int nIndex = lstNotices.IndexOf(frmNoticeDetail.notice);//Buscamos la posición del registros
          dgrNotices.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrNotices, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = "Notices (" + dgrNotices.Items.Count + ")";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <history>
    /// [emoguel] created 23/07/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.enumWindow = EnumWindow.DefaultInt;
      frmSearch.strID = (_noticeFilter.noID > 0) ? _noticeFilter.noID.ToString() : "";
      frmSearch.strDesc = _noticeFilter.noTitle;
      if(frmSearch.ShowDialog()==true)
      {
        _noticeFilter.noID = Convert.ToInt32(frmSearch.strID);
        _noticeFilter.noTitle = frmSearch.strDesc;
        LoadNotices();
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadNotices
    /// <summary>
    /// Carga la lista de notices
    /// </summary>
    /// <param name="notice">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 23/07/2016
    /// </history>
    private async void LoadNotices(Notice notice = null)
    {
      try
      {
        int nIndex = 0;
        List<Notice> lstNotices = await BRNotices.GetNotices(_nStatus, _noticeFilter);
        dgrNotices.ItemsSource = lstNotices;
        if (lstNotices.Count > 0 && notice != null)
        {
          notice = lstNotices.Where(no => no.noID == notice.noID).FirstOrDefault();
          nIndex = lstNotices.IndexOf(notice);
        }
        GridHelper.SelectRow(dgrNotices, nIndex);
        StatusBarReg.Content = lstNotices.Count + " Notices.";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }

    }
    #endregion

    #region ValidateFilters
    /// <summary>
    /// Valida que un "Notice" cumpla con los filtros actuales
    /// </summary>
    /// <param name="notice">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 23/07/2016
    /// </history>
    private bool ValidateFilters(Notice notice)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(notice.noA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(_noticeFilter.noID>0)//Filtro por ID
      {
        if(notice.noID!=_noticeFilter.noID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_noticeFilter.noTitle))//Filtro por descripción
      {
        if(notice.noTitle!=_noticeFilter.noTitle)
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
