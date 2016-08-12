using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmBankDetail.xaml
  /// </summary>
  public partial class frmBankDetail : Window
  {
    #region Variables
    
    public Bank bank = new Bank();//Objeto a edita o agregar
    public Bank oldBank = new Bank();//Objeto con los datos iniciales
    public EnumMode enumMode;//modo en que se mostrará la ventana  
    private List<SalesRoom> _oldLstSalesRoom = new List<SalesRoom>();//Lista incial de computadoras 
    private bool isCellCancel = false;
    private bool blnClosing = false;
    #endregion
    public frmBankDetail()
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
    /// [emoguel] created 30/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _oldLstSalesRoom = oldBank.SalesRooms.ToList();
      List<SalesRoom> lstSalesRoom = new List<SalesRoom>();
      oldBank.SalesRooms.ToList().ForEach(sr=> {
        SalesRoom srm = new SalesRoom();
        ObjectHelper.CopyProperties(srm, sr);
        lstSalesRoom.Add(srm);
      });
      dgrSalesRoom.ItemsSource = lstSalesRoom;
      ObjectHelper.CopyProperties(bank, oldBank);            
      UIHelper.SetUpControls(bank, this);      
      txtbkID.IsEnabled = (enumMode == EnumMode.Add);
      LoadSalesRoom();
      DataContext = bank;
      dgrSalesRoom.BeginningEdit += GridHelper.dgr_BeginningEdit;
    }
    #endregion    

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing)
      {
        btnCancel.Focus();        
        List<SalesRoom> lstSalesRoom = (List<SalesRoom>)dgrSalesRoom.ItemsSource;
        if (!ObjectHelper.IsEquals(bank, oldBank) || !ObjectHelper.IsListEquals(lstSalesRoom, _oldLstSalesRoom))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrSalesRoom.CancelEdit();
          }
        }
      }
    }
    #endregion

    #region dgrSalesRoom_RowEditEnding
    /// <summary>
    /// No permite agregar registros vacios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void dgrSalesRoom_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (isCellCancel)
        {
          dgrSalesRoom.RowEditEnding -= dgrSalesRoom_RowEditEnding;
          dgrSalesRoom.CancelEdit();
          dgrSalesRoom.RowEditEnding += dgrSalesRoom_RowEditEnding;
        }
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo Banks
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/04/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<SalesRoom> lstSalesRoom = (List<SalesRoom>)dgrSalesRoom.ItemsSource;
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(bank, oldBank) && ObjectHelper.IsListEquals(_oldLstSalesRoom, lstSalesRoom))
        {
          blnClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          btnAccept.Visibility = Visibility.Collapsed;
          string strMsj = ValidateHelper.ValidateForm(this, "Bank",blnDatagrids:true);
          if (strMsj == "")
          {
            List<SalesRoom> lstAdd = lstSalesRoom.Where(sr => !_oldLstSalesRoom.Any(srr => srr.srID == sr.srID)).ToList();
            List<SalesRoom> lstDel = _oldLstSalesRoom.Where(sr => !lstSalesRoom.Any(srr => srr.srID == sr.srID)).ToList();
            var grid = dgrSalesRoom;
            int nRes = await BRBanks.SaveBank(bank, (enumMode == EnumMode.Edit), lstAdd, lstDel);
            var banks = await BRBanks.GetBanks(bank: bank);
            bank = banks.FirstOrDefault();
            UIHelper.ShowMessageResult("Bank", nRes);
            if (nRes > 0)
            {
              blnClosing = true;
              DialogResult = true;
              Close();
            }
          }
          else
          {
            UIHelper.ShowMessage(strMsj);
          }
          skpStatus.Visibility = Visibility.Collapsed;
          btnAccept.Visibility = Visibility.Visible;
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Banks");
      }
    }
    #endregion

    #region EndEdit
    /// <summary>
    /// Valida que el Sales Room no esté seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/04/2016
    /// </history>
    private void dgrSalesRoom_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction==DataGridEditAction.Commit)//Verificar si se está cancelando la edición
      {      
        isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrSalesRoom);
        e.Cancel = isRepeat;
      }
      else
      {
        
        isCellCancel = true;
      }
    }
    #endregion
    #endregion

    #region Methods   

    #region LoadSalesRoom
    /// <summary>
    /// Llena el combobox de salesRoom
    /// </summary>
    /// <history>
    /// [emoguel] 02/05/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void LoadSalesRoom()
    {
      try
      {
        List<SalesRoom> lstSalesRoom =await BRSalesRooms.GetSalesRooms(1, -1);
        cmbSalesRoom.ItemsSource = lstSalesRoom.ToList();
        skpStatus.Visibility = Visibility.Collapsed;
        btnAccept.Visibility = Visibility.Visible;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Banks");
      }
    }
    #endregion

    #endregion


  }
}
