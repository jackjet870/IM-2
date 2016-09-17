using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmClubs.xaml
  /// </summary>
  public partial class frmClubs : Window
  {
    #region Variables
    private Club _clubFilter = new Club();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros de grid
    #endregion
    public frmClubs()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadClubs();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/05/2016
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
    /// [emoguel] created 02/05/2016
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
    /// [emoguel] created 02/05/2016
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
    /// [emoguel] created 02/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Club club = (Club)dgrClubs.SelectedItem;
      frmClubDetail frmClubDetail = new frmClubDetail();
      frmClubDetail.Owner = this;
      frmClubDetail.enumMode = EnumMode.Edit;
      frmClubDetail.oldClub = club;
      if(frmClubDetail.ShowDialog()==true)
      {
        List<Club> lstClubs = (List<Club>)dgrClubs.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmClubDetail.club))//Validamos que cumpla con los filtros
        {
          ObjectHelper.CopyProperties(club, frmClubDetail.club);//Actualizamos los datos 
          lstClubs.Sort((x, y) => string.Compare(x.clN, y.clN));//Ordenamos la lista
          nIndex = lstClubs.IndexOf(club);//Buscamos la posición del registro
        }
        else
        {
          lstClubs.Remove(club);//Quitamos el registro
        }
        dgrClubs.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrClubs, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstClubs.Count + " Clubs.";//Actualizamos el contador
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
    /// [emoguel] created 02/05/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = (_clubFilter.clID>0)?_clubFilter.clID.ToString():"";
      frmSearch.strDesc = (_clubFilter.clN!=null)?_clubFilter.clN:"";
      frmSearch.nStatus = _nStatus;
      frmSearch.enumWindow = EnumWindow.DefaultInt;
      if(frmSearch.ShowDialog()==true)
      {
        _clubFilter.clID = Convert.ToInt32(frmSearch.strID);
        _clubFilter.clN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadClubs();
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
    /// [emoguel] created 02/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmClubDetail frmClubDetail = new frmClubDetail();
      frmClubDetail.Owner = this;
      frmClubDetail.enumMode = EnumMode.Add;
      if (frmClubDetail.ShowDialog() == true)
      {
        if(ValidateFilter(frmClubDetail.club))//Verificar que cumpla con los filtros
        {
          List<Club> lstClubs = (List<Club>)dgrClubs.ItemsSource;
          lstClubs.Add(frmClubDetail.club);//Agregamos el registro
          lstClubs.Sort((x, y) => string.Compare(x.clN, y.clN));//Ordenamos la lista
          int nIndex = lstClubs.IndexOf(frmClubDetail.club);//Ordenamos lal ista
          dgrClubs.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrClubs, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstClubs.Count + " Clubs.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 02/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Club club = (Club)dgrClubs.SelectedItem;
      LoadClubs(club);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadClubs
    /// <summary>
    /// Llena el grid de clubs
    /// </summary>
    /// <param name="club">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 02/05/2016
    /// </history>
    private async void LoadClubs(Club club = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Club> lstClubs = await BRClubs.GetClubs(_clubFilter, _nStatus);
        dgrClubs.ItemsSource = lstClubs;
        if (lstClubs.Count > 0 && club != null)
        {
          club = lstClubs.FirstOrDefault(cl => cl.clID == club.clID);
          nIndex = lstClubs.IndexOf(club);
        }
        GridHelper.SelectRow(dgrClubs, nIndex);
        StatusBarReg.Content = lstClubs.Count + " Clubs.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un club cumpla con los filtros actuales
    /// </summary>
    /// <param name="club">Objeto a validar</param>
    /// <returns>True. Si cumple | False. no cumple</returns>
    /// <history>
    /// [emoguel] created 02/05/2016
    /// </history>
    private bool ValidateFilter(Club club)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(club.clA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(_clubFilter.clID>0)//FIltro por ID
      {
        if(_clubFilter.clID!=club.clID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_clubFilter.clN))//Filtro por descripción
      {
        if(!club.clN.Contains(_clubFilter.clN,StringComparison.OrdinalIgnoreCase))
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
