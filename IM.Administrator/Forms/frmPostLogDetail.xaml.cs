﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using System.Linq;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPostLogDetail.xaml
  /// </summary>
  public partial class frmPostLogDetail : Window
  {
    #region variables
    public PostLog postLog = new PostLog();//Objeto a guardar/Filtrar
    public PostLog oldPostLog = new PostLog();//objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    public bool blnDate = false;
    private bool _isClosing = false;
    #endregion
    public frmPostLogDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(postLog, oldPostLog);
      LoadPersonnel();
      LoadPosts();      
      if(enumMode!=EnumMode.preview)
      {        
        cmbpppe.IsEnabled = true;        
        btnAccept.Visibility = Visibility.Visible;
        txtppDT.IsEnabled = true;
        cmbpppo.IsEnabled = true;
        UIHelper.SetUpControls(postLog, this);
        if(enumMode==EnumMode.search)
        {
          txtppID.Visibility = Visibility.Collapsed;
          lblppID.Visibility = Visibility.Collapsed;
          cmbpppo.Visibility = Visibility.Collapsed;          
          lblpppo.Visibility = Visibility.Collapsed;
          txtppDT.Visibility = Visibility.Collapsed;
          dpppDT.Visibility = Visibility.Visible;
          cmbppChangedBy.IsEnabled = true;        
          if(blnDate)
          {
            dpppDT.SelectedDate = postLog.ppDT;
          }  
        }
        else
        {          
          postLog.ppChangedBy= App.User.User.peID;
          if (enumMode == EnumMode.add)
          {
            postLog.ppDT = DateTime.Now;
          }
        }
      }
      DataContext = postLog;
    } 
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/04/2016
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
    /// Guarda|actualiza un registro en la Bd
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 13/04/2016
    /// [emoguel] modified 30/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode == EnumMode.search)
        {
          if (dpppDT.SelectedDate != null)
          {
            blnDate = true;
            postLog.ppDT = Convert.ToDateTime(dpppDT.SelectedDate);
          }
          else
          {
            blnDate = false;
          }
          _isClosing = true;
          DialogResult = true;
          Close();
        }
        else
        {
          if (ObjectHelper.IsEquals(postLog, oldPostLog) && enumMode != EnumMode.add)
          {
            _isClosing = true;
            Close();
          }
          else
          {
            #region Insertar|Agregar
            string strMsj = ValidateHelper.ValidateForm(this, "Post Log");
            if (strMsj == "")
            {
              int nRes = await BREntities.OperationEntity(postLog, enumMode);
              UIHelper.ShowMessageResult("PostLog", nRes);
              if (nRes > 0)
              {
                _isClosing = true;
                var postsLog= await BRPostsLog.GetPostsLog(postLog);
                postLog = postsLog.FirstOrDefault();
                DialogResult = true;
                Close();
              }

            }
            else
            {
              UIHelper.ShowMessage(strMsj);
            }
            #endregion
          }
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Post Log");
      }
    } 
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verficando cambios pendiente
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica cambios antes de cerrar la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created  28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(!_isClosing)
      {
        if (enumMode != EnumMode.preview && enumMode != EnumMode.search)
        {
          if (!ObjectHelper.IsEquals(postLog, oldPostLog))
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
    #region LoadPersonnel
    /// <summary>
    /// Carga el combo de Personel y ChangedBy
    /// </summary>
    /// <history>
    /// [emoguel] 12/04/2016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadPersonnel()
    {
      try
      {
        List<PersonnelShort> lstPersonnel = await BRPersonnel.GetPersonnel();
        if (enumMode == EnumMode.search)
        {
          lstPersonnel.Insert(0, new PersonnelShort { peID = "", peN = "ALL" });
        }
        cmbppChangedBy.ItemsSource = lstPersonnel;
        cmbpppe.ItemsSource = lstPersonnel;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Post Log");
      }
    }
    #endregion

    #region LoadPosts
    /// <summary>
    /// Carga el comoboBox de Posts
    /// </summary>
    /// <history>
    /// [emoguel] created 12/04/2016
    /// </history>
    private async void LoadPosts()
    {
      try
      {
        List<Post> lstPosts = await BRPosts.GetPosts();
        cmbpppo.ItemsSource = lstPosts;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Post Log");
      }
    }

    #endregion

    #endregion
    
  }
}
