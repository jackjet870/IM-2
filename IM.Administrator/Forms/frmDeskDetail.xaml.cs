using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmDeskDetail.xaml
  /// </summary>
  public partial class frmDeskDetail : Window
  {
    #region variables
    public Desk desk = new Desk();//Objeto a edita o agregar
    public Desk oldDesk = new Desk();//Objeto con los datos iniciales
    public EnumMode enumMode;//modo en que se mostrará la ventana  
    private List<Computer> _oldLstComputers = new List<Computer>();//Lista incial de computadoras
    private bool _isCellCancel = false;
    private bool _isClosing = false; 
    #endregion
    public frmDeskDetail()
    {
      InitializeComponent();
    }

    #region eventos del formulario
    #region WindowLoaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/06/2016
    /// [emoguel] modified 29/07/2016--->Se agregó el método Beginnigedti
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(desk, oldDesk);
      DataContext = desk;
      txtdkID.Text = ((enumMode == EnumMode.Edit) ? desk.dkID.ToString() : "");
      UIHelper.SetUpControls(desk, this);
      LoadGridComputers();
      LoadCmbComputers();
      dgrComputers.BeginningEdit += GridHelper.dgr_BeginningEdit;
    } 
    #endregion

    #region Boton aceptar
    /// <summary>
    /// Agrega o actualiza un registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/03/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<Computer> lstComputers = (List<Computer>)dgrComputers.ItemsSource;
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(desk, oldDesk) && ObjectHelper.IsEquals(lstComputers, _oldLstComputers))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          btnAccept.Visibility = Visibility.Collapsed;
          string strMsj = ValidateHelper.ValidateForm(this, "Desk", blnDatagrids: true);
          int nRes = 0;

          if (strMsj == "")
          {
            List<string> lstIdsComputers = lstComputers.Select(cmp => cmp.cpID).ToList();
            nRes =await BRDesks.SaveDesk(desk, (enumMode == EnumMode.Edit), lstIdsComputers);
            UIHelper.ShowMessageResult("Desk", nRes);
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
          skpStatus.Visibility = Visibility.Collapsed;
          btnAccept.Visibility = Visibility.Visible;
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Desks");
      }
    }
    #endregion

    #region EndEdit
    /// <summary>
    /// Valida si la computadora no está seleccionada en otra fila
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 17/03/2016
    /// </history>
    private void dgrComputers_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction==DataGridEditAction.Commit)//Verificar si se está cancelando la edición
      {
        _isCellCancel = false;
        bool blnIsRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrComputers);
        e.Cancel = blnIsRepeat;
      }
      else
      {
        _isCellCancel = true;
      }

    }
    #endregion
      
    #region Window_Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing && enumMode != EnumMode.ReadOnly)
      {
        btnCancel.Focus();
        List<Computer> lstComputers = (List<Computer>)dgrComputers.ItemsSource;
        if (!ObjectHelper.IsEquals(desk, oldDesk) || !ObjectHelper.IsListEquals(lstComputers, _oldLstComputers))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrComputers.CancelEdit();
          }
        }
      }

    }
    #endregion v

    #region RowEditEnding
    /// <summary>
    /// Verifica que no se agreguen registros vacios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    private void dgrComputers_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCancel)
        {
          dgrComputers.RowEditEnding -= dgrComputers_RowEditEnding;
          dgrComputers.CancelEdit();
          dgrComputers.RowEditEnding += dgrComputers_RowEditEnding;
        }
        else
        {
          cmbComputers.Header = "Computer (" + (dgrComputers.Items.Count - 1) + ")";
        }
      }
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cambia el contador cuando se eliminan registros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        var item = dgrComputers.SelectedItem;
        if (item.GetType().Name == "Computer")
        {
          cmbComputers.Header = "Computer (" + (dgrComputers.Items.Count - 2) + ")";
        }
      }
    }
    #endregion
    #endregion

    #region Metodos
    #region LoadComputers
    /// <summary>
    /// Llena el grid de computadoras asignadas al desk
    /// </summary>
    /// <history>
    /// [emoguel] created 17/03/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void LoadGridComputers()
    {
      try
      {
        Computer computer = new Computer();
        List<Computer> lstComputers = new List<Computer>();
        if (enumMode == EnumMode.Edit)
        {
          computer.cpdk = desk.dkID;
          lstComputers = await BRComputers.GetComputers(computer);
          _oldLstComputers = lstComputers.ToList();
        }
        dgrComputers.ItemsSource = lstComputers;
        cmbComputers.Header = "Computer (" + lstComputers.Count + ")";
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Desks");
      }
    }
    #endregion

    #region LoadCmbComputers
    /// <summary>
    /// Llena el combo de computers del grid
    /// </summary>
    /// <history>
    /// [emoguel] created 17/03/2016        
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void LoadCmbComputers()
    {
      try
      {
        List<Computer> lstComputers = await BRComputers.GetComputers();
        cmbComputers.ItemsSource = lstComputers;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Deks");
      }
    }
    #endregion

    #endregion

    
  }
}
