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
      dgrSalesRoom.ItemsSource = oldBank.SalesRooms.ToList();
      ObjectHelper.CopyProperties(bank, oldBank);            
      UIHelper.SetUpControls(bank, this);      
      txtbkID.IsEnabled = (enumMode == EnumMode.add);
      LoadSalesRoom();
      DataContext = bank;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana Verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {      
      List<SalesRoom> lstSalesRoom = (List<SalesRoom>)dgrSalesRoom.ItemsSource;
      if (!ObjectHelper.IsEquals(bank, oldBank) || !ObjectHelper.IsListEquals(lstSalesRoom, _oldLstSalesRoom))
      {
        MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
        if (result == MessageBoxResult.Yes)
        {
          Close();
        }
      }
      else
      {
        Close();
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
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<SalesRoom> lstSalesRoom = (List<SalesRoom>)dgrSalesRoom.ItemsSource;
      if(enumMode!=EnumMode.add && ObjectHelper.IsEquals(bank,oldBank) && ObjectHelper.IsListEquals(_oldLstSalesRoom,lstSalesRoom))
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Bank");
        if (strMsj == "")
        {          
          List<SalesRoom> lstAdd = lstSalesRoom.Where(sr => !_oldLstSalesRoom.Any(srr => srr.srID == sr.srID)).ToList();
          List<SalesRoom> lstDel = _oldLstSalesRoom.Where(sr => !lstSalesRoom.Any(srr => srr.srID == sr.srID)).ToList();
          int nRes = BRBanks.SaveBank(ref bank, (enumMode == EnumMode.edit), lstAdd, lstDel);
          UIHelper.ShowMessageResult("Bank", nRes);
          if (nRes > 0)
          {
            DialogResult = true;
            Close();
          }
        }
        else
        {
          UIHelper.ShowMessage(strMsj);
        }
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
      if (!Keyboard.IsKeyDown(Key.Escape))//Verificar si se está cancelando la edición
      {
        List<SalesRoom> lstSalesRoom = (List<SalesRoom>)dgrSalesRoom.ItemsSource;//Los items del grid                   
        SalesRoom salesRoom = (SalesRoom)dgrSalesRoom.SelectedItem;//Valor que se está editando

        var Combobox = (ComboBox)e.EditingElement;
        SalesRoom salesCombo = (SalesRoom)Combobox.SelectedItem;//Valor seleccionado del combo

        if (salesCombo != null)//Se valida que no esté seleccionado en otra fila
        {
          if (salesCombo != salesRoom)//Validar que se esté cambiando el valor
          {
            SalesRoom salesRoomVal = lstSalesRoom.Where(sr => sr.srID != salesRoom.srID && sr.srID == salesCombo.srID).FirstOrDefault();
            if (salesRoomVal != null)
            {
              UIHelper.ShowMessage("Sales Room must not be repeated");
              e.Cancel = true;
            }
          }
        }
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
    /// </history>
    private void LoadSalesRoom()
    {
      List<SalesRoom> lstSalesRoom = BRSalesRooms.GetSalesRooms(1, -1);
      cmbSalesRoom.ItemsSource = lstSalesRoom;
    }
    #endregion

    #endregion
  }
}
