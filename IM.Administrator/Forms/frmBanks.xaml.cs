using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmBanks.xaml
  /// </summary>
  public partial class frmBanks : Window
  {

    #region Variables
    private Bank _bankFilter = new Bank();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion

    public frmBanks()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadBanks();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/04/2016
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
    /// [emoguel] created 30/04/2016
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
    /// [emoguel] created 30/04/2016
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
    /// [emoguel] created 30/04/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Bank bank = (Bank)dgrBanks.SelectedItem;
      frmBankDetail frmBankDetail = new frmBankDetail();
      frmBankDetail.Owner = this;
      frmBankDetail.enumMode = EnumMode.edit;
      frmBankDetail.oldBank = bank;
      if(frmBankDetail.ShowDialog()==true)
      {
        List<Bank> lstBanks = (List<Bank>)dgrBanks.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmBankDetail.bank))//Verificamos que cumpla con los filtros
        {
          ObjectHelper.CopyProperties(bank, frmBankDetail.bank, true);//Actualizamos los datos del registro
          lstBanks.Sort((x, y) => string.Compare(x.bkN, y.bkN));//Reordenamos la lista
          nIndex = lstBanks.IndexOf(bank);//Buscamos la posición del registro
        }
        else
        {
          lstBanks.Remove(bank);//Quitamos de la lista
        }
        dgrBanks.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrBanks, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstBanks.Count + " Banks.";//Actualizamos el contador
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
    /// [emoguel] created 30/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Bank bank = (Bank)dgrBanks.SelectedItem;
      LoadBanks(bank);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmBankDetail frmBankDetail = new frmBankDetail();
      frmBankDetail.Owner = this;
      frmBankDetail.enumMode = EnumMode.add;
      if(frmBankDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmBankDetail.bank))//Validamos que cumpla con los filtros actuales
        {
          List<Bank> lstBanks = (List<Bank>)dgrBanks.ItemsSource;
          lstBanks.Add(frmBankDetail.bank);//Agregamos el registro a la lista
          lstBanks.Sort((x, y) => string.Compare(x.bkN, y.bkN));//Ordenamos la lista
          int nIndex = lstBanks.IndexOf(frmBankDetail.bank);//BUscamos la posicion del registro
          dgrBanks.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrBanks, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstBanks.Count + " Banks.";//Actualizamos el contador
        }
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
    /// [emoguel] created 30/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _bankFilter.bkID;
      frmSearch.strDesc = _bankFilter.bkN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _bankFilter.bkID = frmSearch.strID;
        _bankFilter.bkN = frmSearch.strDesc;
        LoadBanks();
      }
    }
    #endregion
    #endregion

    #region Methods

    #region LoadBanks
    /// <summary>
    /// Llena el grid de banks
    /// </summary>
    /// <param name="bank">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 30/04/2016
    /// </history>
    private void LoadBanks(Bank bank = null)
    {
      int nIndex = 0;
      List<Bank> lstBanks = BRBanks.GetBanks(_nStatus, _bankFilter,true);
      dgrBanks.ItemsSource = lstBanks;
      if (lstBanks.Count > 0 && bank != null)
      {
        bank = lstBanks.Where(bk => bk.bkID == bank.bkID).FirstOrDefault();
        nIndex = lstBanks.IndexOf(bank);
      }
      GridHelper.SelectRow(dgrBanks, nIndex);
      StatusBarReg.Content = lstBanks.Count + " Banks.";
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un Bank cumpla con los filtros actuales
    /// </summary>
    /// <param name="bank">Objeto a validar</param>
    /// <returns>True. Si cumple | False . No cumple</returns>
    /// <history>
    /// [emoguel] created 30/04/2016
    /// </history>
    private bool ValidateFilter(Bank bank)
    {
      if(_nStatus!=-1)//Filtro por  estatus
      {
        if(bank.bkA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_bankFilter.bkID))//Filtro por ID
      {
        if(bank.bkID!=_bankFilter.bkID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_bankFilter.bkN))//Filtro por Descripción
      {
        if(!bank.bkN.Contains(_bankFilter.bkN))
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
