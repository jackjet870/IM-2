﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmFolioCXCDetail.xaml
  /// </summary>
  public partial class frmFolioCXCDetail : Window
  {
    public FolioCXC folioCXC = new FolioCXC();//objeto a guardar o actualizar
    public FolioCXC oldFolioCxc = new FolioCXC();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en el que se mostrará la ventana
    private bool _isClosing = false;
    public frmFolioCXCDetail()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Window Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(folioCXC, oldFolioCxc);
      OpenMode();
      UIHelper.SetUpControls(folioCXC,this);
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda|Actualiza un registro del catalogo de FoliosCXC
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
     if(ObjectHelper.IsEquals(folioCXC,oldFolioCxc) && enumMode!=EnumMode.Add)
      {
        _isClosing = true;
        Close();
      }
     else
      {
        string strMsj = "";
        int nRes = 0;
        #region Validar El rango del folio      
        if (folioCXC.fiFrom == 0)
        {
          txtfiFrom.Text = "0";
          strMsj += "Start number can not be 0.";
        }
        else
        {
          if (folioCXC.fiTo < folioCXC.fiFrom)
          {
            if (string.IsNullOrWhiteSpace(txtfiTo.Text))
            {
              txtfiTo.Text = txtfiFrom.Text;
            }
            strMsj += "Start number can not be greater than End Number.";
          }
        }
        #endregion

        if (strMsj == "")
        {
          nRes = BRFoliosCXC.SaveFolioCXC(folioCXC, (enumMode == EnumMode.Edit));
          UIHelper.ShowMessageResult("Folio CxC", nRes);
          if(nRes==1)
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
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana pero antes verifica que no se tengan cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if(enumMode!=EnumMode.ReadOnly)
      {
        if (!ObjectHelper.IsEquals(folioCXC, oldFolioCxc))
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

    #region Closing
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
      if (!_isClosing)
      {
        _isClosing = true;
        btnCancel_Click(null, null);
        if (!_isClosing)
        {
          e.Cancel = true;
        }
        else
        {
          _isClosing = false;
        }
      }
    }
    #endregion
    #endregion

    #region Métodos
    #region OpenMode
    /// <summary>
    /// Cambia el estado de los controles dependiendo del modo en que se abrio
    /// </summary>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void OpenMode()
    {
      DataContext = folioCXC;
      if (enumMode != EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        txtfiFrom.IsEnabled = true;
        txtfiTo.IsEnabled = true;
        chkA.IsEnabled = true;
      }
    }

    #endregion

    #endregion
  }
}
