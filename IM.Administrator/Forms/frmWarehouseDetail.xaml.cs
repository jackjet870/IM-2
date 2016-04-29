using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

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
      if(enumMode!=EnumMode.preview)
      {
        txtwhID.IsEnabled = (enumMode != EnumMode.edit);
        txtwhN.IsEnabled = true;
        cmbwhar.IsEnabled = true;
        chkwhA.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        if(enumMode==EnumMode.search)
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
        btnCancel_Click(null, null);
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
      if(enumMode!=EnumMode.search)
      {
        if(enumMode!=EnumMode.add && ObjectHelper.IsEquals(warehouse,oldWarehouse))
        {
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Warehouse");
          if(strMsj=="")
          {
            int nRes = BRWarehouses.SaveWarehouse(warehouse,(enumMode==EnumMode.edit));
            UIHelper.ShowMessageResult("Warehouse", nRes);
            if(nRes>0)
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
      else
      {
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
      if(enumMode!=EnumMode.search && enumMode!=EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(warehouse, oldWarehouse))
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
      else
      {
        Close();
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
    private void LoadAreas()
    {
      List<Area> lstArea = BRAreas.GetAreas();
      if (enumMode == EnumMode.search)
      {
        lstArea.Insert(0, new Area { arID = "", arN = "" });
      }
      cmbwhar.ItemsSource = lstArea;
    } 
    #endregion
    #endregion
  }
}
