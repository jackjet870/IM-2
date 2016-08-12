using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmWarehouseDetail.xaml
  /// </summary>
  public partial class frmWarehouseDetail : Window
  {
    #region Variables
    public Warehouse warehouse = new Warehouse();//Objeto a guardar
    public Warehouse oldWarehouse = new Warehouse();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    public int nStatus = -1;//sirve para el modo busqueda
    private bool _isClosing = false;
    #endregion
    public frmWarehouseDetail()
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
      ObjectHelper.CopyProperties(warehouse, oldWarehouse);
      UIHelper.SetUpControls(warehouse, this);
      if(enumMode!=EnumMode.ReadOnly)
      {
        txtwhID.IsEnabled = (enumMode != EnumMode.Edit);
        txtwhN.IsEnabled = true;
        cmbwhar.IsEnabled = true;
        chkwhA.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        if(enumMode==EnumMode.Search)
        {
          Title = "Search";
          chkwhA.Visibility = Visibility.Hidden;
          cmbStatus.Visibility = Visibility.Visible;
          lblStatus.Visibility = Visibility.Visible;
          ComboBoxHelper.LoadComboDefault(cmbStatus);
          cmbStatus.SelectedValue = nStatus;
        }
      }
      LoadAreas();
      DataContext = warehouse;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        Close();
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega|Actualiza un registro en warehouse
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(enumMode!=EnumMode.Search)
      {
        if(enumMode!=EnumMode.Add && ObjectHelper.IsEquals(warehouse,oldWarehouse))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Warehouse");
          if(strMsj=="")
          {
            int nRes = BRWarehouses.SaveWarehouse(warehouse,(enumMode==EnumMode.Edit));
            UIHelper.ShowMessageResult("Warehouse", nRes);
            if(nRes>0)
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
      else
      {
        _isClosing = true;
        nStatus = Convert.ToInt32(cmbStatus.SelectedValue);
        DialogResult = true;
        Close();
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios Pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
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
    /// [emoguel] crated 28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        if (enumMode != EnumMode.Search && enumMode != EnumMode.ReadOnly)
        {
          if (!ObjectHelper.IsEquals(warehouse, oldWarehouse))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result != MessageBoxResult.Yes)
            {
              e.Cancel = true;
            }
          }
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadAreas
    /// <summary>
    /// Llena el combobox de Areas
    /// </summary>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private async void LoadAreas()
    {
      try
      {
        List<Area> lstArea = await BRAreas.GetAreas();
        if (enumMode == EnumMode.Search)
        {
          lstArea.Insert(0, new Area { arID = "", arN = "ALL" });
        }
        cmbwhar.ItemsSource = lstArea;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Warehouse");
      }
    }
    #endregion

    #endregion
    
  }
}
