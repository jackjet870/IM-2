using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAreaDetalle.xaml
  /// </summary>
  public partial class frmAreaDetalle : Window
  {

    #region Variables
    public Area area = new Area();//Variable que se va a guardar o actualizar
    public Area oldArea = new Area();//Variable con los atributos iniciales
    public EnumMode mode;//Modo en el que se abrirá la ventana  
    private bool _isClosing = false;
    #endregion
    public frmAreaDetalle()
    {
      InitializeComponent();
    }

    #region metodos
    #region LoadRegion
    /// <summary>
    /// Cargala lista de regiones
    /// </summary>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// [emoguel] 30/05/2016 se volvió async
    /// </history>
    protected async void LoadRegions()
    {
      try
      {
        List<Region> lstRegions = await BRRegions.GetRegions(1);
        cmbarrg.ItemsSource = lstRegions;
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion
    #endregion
    #region evento botones

    #region Loaded
    /// <summary>
    /// Carga los controles al abrir la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      skpStatus.Visibility = Visibility.Visible;
      ObjectHelper.CopyProperties(area, oldArea);
      LoadRegions();
      DataContext = area;
      if(mode!=EnumMode.ReadOnly)
      {
        txtarN.IsEnabled = true;
        chkarA.IsEnabled = true;
        cmbarrg.IsEnabled = true;
        txtarID.IsEnabled = (mode == EnumMode.Add);
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetUpControls(area, this);
      }      
    }
    #endregion
    
    #region Boton Aceptar
    /// <summary>
    /// Agrega o actualiza los registros del catalogo Areas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(area,oldArea)&& mode!=EnumMode.Add)
      {
        _isClosing = true;
        Close();
      }
      else
      {
        skpStatus.Visibility = Visibility.Visible;
        txtStatus.Text = "Saving Data...";
        string sMsj = ValidateHelper.ValidateForm(this, "Area");
        int nRes = 0;
        
        if (sMsj == "")
        {
          nRes = await BREntities.OperationEntity(area, mode);

          UIHelper.ShowMessageResult("Area", nRes);
          if(nRes==1)
          {
            _isClosing = true;
            DialogResult = true;
            Close();
          }
        }
        else
        {
          UIHelper.ShowMessage(sMsj);
        }
        skpStatus.Visibility = Visibility.Collapsed;
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana pero antes verifica que no se tengan cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 29/03/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if(mode!=EnumMode.ReadOnly)
      {
        if (!ObjectHelper.IsEquals(area, oldArea))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!_isClosing) { _isClosing = true; Close(); }
          }
          else
          {
            _isClosing = false;
          }
        }
        else
        {
          if (!_isClosing) { _isClosing = true; Close(); }
        }
      }
      else
      {
        if (!_isClosing) { _isClosing = true; Close(); }
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        btnCancel.Focus();
        if (mode != EnumMode.ReadOnly)
        {
          if (!ObjectHelper.IsEquals(area, oldArea))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result == MessageBoxResult.No)
            {
              e.Cancel=true;
              _isClosing = false;
            }
          }
        }
      }
    }
    #endregion

    #endregion
  }
}
