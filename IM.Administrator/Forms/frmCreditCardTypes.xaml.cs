﻿using System;
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
using IM.Administrator.Helpers;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCreditCardTypes.xaml
  /// </summary>
  public partial class frmCreditCardTypes : Window
  {
    private CreditCardType _creditCardTypeFilter=new CreditCardType();//Objeto a filtrar en la lista
    private int _nStatus = -1;//Status del grid a filtrar
    private bool _blnEdit = false;//para saber sis se cuenta con permiso paraa editar y agregar
    public frmCreditCardTypes()
    {
      InitializeComponent();
    }


    #region Métodos
    #region Load CreditCardType
    /// <summary>
    /// llena el datagrid de credit card types
    /// </summary>
    /// <history>
    /// [Emoguel] created 07/003/2016
    /// </history>
    protected void LoadCreditCardTypes()
    {
      List<CreditCardType> lstCreditCardTypes = BRCreditCardTypes.GetCreditCardTypes(_creditCardTypeFilter, _nStatus);
      if (lstCreditCardTypes.Count > 0)
      {
        dgrCreditCard.SelectedIndex = 0;
      }
      dgrCreditCard.ItemsSource = lstCreditCardTypes;
      StatusBarReg.Content = lstCreditCardTypes.Count() + "  Credit Card Types.";
    }
    #endregion
    #endregion

    #region eventos del formlario
    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 07/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.nStatus = _nStatus;
      frmSearch.sID = _creditCardTypeFilter.ccID;
      frmSearch.sDesc = _creditCardTypeFilter.ccN;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _creditCardTypeFilter.ccID = frmSearch.sID;
        _creditCardTypeFilter.ccN = frmSearch.sDesc;
        LoadCreditCardTypes();
      }
    }

    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana de detalle en modo agregar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 07/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmCreditCardTypesDetail frmCreditCard = new frmCreditCardTypesDetail();
      frmCreditCard.Owner = this;
      frmCreditCard.mode = ModeOpen.add;
      if (frmCreditCard.ShowDialog() == true)
      {
        LoadCreditCardTypes();
      }
    } 
    #endregion

    #region refresh
    /// <summary>
    /// Evento del boton refresh---recarga los datos de la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadCreditCardTypes();
    }
    #endregion

    #region Loaded Form
    /// <summary>
    /// llena los datos del formulario 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = PermisionHelper.EditPermision("SALES");
      btnAdd.IsEnabled = _blnEdit;
      LoadCreditCardTypes();
    }
    #endregion
    #region KeyboardFocusChange
    /// <summary>
    /// verfica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/03/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {

      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion
    #region KeyDownForm
    /// <summary>
    /// Valida las teclas presionadas para cambiar la barra de estado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 07/03/2016
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
    #region DoubleClick
    /// <summary>
    /// Muestra la ventada detalle en modo preview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 03/03/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      DataGridRow row = sender as DataGridRow;
      CreditCardType creditCardType = (CreditCardType)row.DataContext;
      frmCreditCardTypesDetail frmCrediCard = new frmCreditCardTypesDetail();
      frmCrediCard.Owner = this;
      frmCrediCard.mode = ((_blnEdit == true) ? ModeOpen.edit : ModeOpen.preview);
      frmCrediCard.creditCardType = creditCardType;
      frmCrediCard.ShowDialog();
    }
    #endregion

    #endregion

    
  }
}