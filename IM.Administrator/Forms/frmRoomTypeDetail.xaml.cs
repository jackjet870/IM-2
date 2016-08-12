using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmRoomTypeDetail.xaml
  /// </summary>
  public partial class frmRoomTypeDetail : Window
  {
    #region variables
    public RoomType roomType = new RoomType();//objeto a guardar
    public RoomType oldRoomType = new RoomType();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    private bool _isClosing = false;
    #endregion

    public frmRoomTypeDetail()
    {
      InitializeComponent();
    }

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(roomType, oldRoomType);
      txtrtID.IsEnabled = (enumMode == EnumMode.Add);
      UIHelper.SetUpControls(roomType, this);
      DataContext = roomType;
    } 
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventan con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key==Key.Escape)
      {
        btnCancel.Focus();
        Close();
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo RoomTypes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// [emoguel] modified 30/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(roomType, oldRoomType))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Room Type");
          if (strMsj == "")
          {
            int nRes =await BREntities.OperationEntity(roomType, enumMode);
            UIHelper.ShowMessageResult("Room Type", nRes);
            if (nRes > 0)
            {
              _isClosing = true;
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
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Room Type");
      }
    } 
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando los cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();      
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/06/2016if
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(!_isClosing)
      {
        if (!ObjectHelper.IsEquals(roomType, oldRoomType))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result != MessageBoxResult.Yes)
          {
            e.Cancel = true;
          }
        }
      }
    } 
    #endregion
  }
}
