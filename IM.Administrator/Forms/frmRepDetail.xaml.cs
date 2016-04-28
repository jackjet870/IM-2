﻿using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmRepDetail.xaml
  /// </summary>
  public partial class frmRepDetail : Window
  {
    #region Variables
    public Rep rep = new Rep();//Objeto a guardar
    public Rep oldRep = new Rep();//Objeto con los datos iniciales de la ventana
    public EnumMode enumMode;//Modo en que se abrira la ventana
    #endregion
    public frmRepDetail()
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
    /// [emoguel] created 18/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(rep, oldRep);
      txtrpID.IsEnabled = (enumMode == EnumMode.add);
      UIHelper.SetUpControls(rep, this);
      DataContext = rep;
    } 
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key==Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);
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
    /// [emoguel] created 18/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(rep, oldRep))
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

    #region Accept
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo Reps
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (enumMode != EnumMode.add && ObjectHelper.IsEquals(rep, oldRep))
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Rep");
        if (strMsj == "")
        {
          int nRes = BRReps.SaveReps(rep, (enumMode == EnumMode.edit));
          UIHelper.ShowMessageResult("Rep", nRes);
          if (nRes == 1)
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
  }
}