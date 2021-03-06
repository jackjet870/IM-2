﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmShowProgramDetail.xaml
  /// </summary>
  public partial class frmShowProgramDetail : Window
  {
    #region variables
    public ShowProgram showProgram = new ShowProgram();//Objeto a guardar
    public ShowProgram oldShowProgram = new ShowProgram();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    public int nStatus = -1;//Estatus para el modo search
    private bool _isClosing = false;
    #endregion
    public frmShowProgramDetail()
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
    /// [emoguel] created 25/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(showProgram, oldShowProgram);
      UIHelper.SetUpControls(showProgram, this);
      LoadCategories();
      if(enumMode==EnumMode.Search)
      {
        cmbStatus.Visibility = Visibility.Visible;
        lblStatus.Visibility = Visibility.Visible;
        chkstA.Visibility = Visibility.Collapsed;
        ComboBoxHelper.LoadComboDefault(cmbStatus);
        cmbStatus.SelectedValue = nStatus;
      }
      txtskID.IsEnabled = (enumMode != EnumMode.Edit);
      DataContext = showProgram;
    }
    #endregion

    #region Keydown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/06/2016
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
    /// Agrega|Actualiza un registro en el catalogp ShowPrograms
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/04/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.Search)
        {
          if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(showProgram, oldShowProgram))
          {
            _isClosing = true;
            Close();
          }
          else
          {
            string strMsj = ValidateHelper.ValidateForm(this, "Show Program");
            if (strMsj == "")
            {
              int nRes =await BREntities.OperationEntity<ShowProgram>(showProgram, enumMode);
              UIHelper.ShowMessageResult("Show Program", nRes);
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
        else
        {
          _isClosing = true;
          nStatus = Convert.ToInt32(cmbStatus.SelectedValue);
          DialogResult = true;
          Close();
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/04/2016
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
    /// [emoguel] created 28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        if (enumMode != EnumMode.Search)
        {
          if (!ObjectHelper.IsEquals(showProgram, oldShowProgram))
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
    #region LoadCategories
    /// <summary>
    /// Llena el combobox de categories
    /// </summary>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    private async void LoadCategories()
    {
      try
      {
        List<ShowProgramCategory> lstShowProCategories = await BRShowProgramsCategories.GetShowProgramsCategories();
        if (enumMode == EnumMode.Search)
        {
          lstShowProCategories.Insert(0, new ShowProgramCategory { sgID = "", sgN = "ALL" });
        }
        cmbsksg.ItemsSource = lstShowProCategories;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #endregion
    
  }
}
