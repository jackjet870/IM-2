using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

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
      if (enumMode!=EnumMode.ReadOnly)
      {        
        cmbpppe.IsEnabled = true;        
        btnAccept.Visibility = Visibility.Visible;              
        cmbpppo.IsEnabled = true;
        UIHelper.SetUpControls(postLog, this);
        dpppDT.IsReadOnly = false;
        if (enumMode==EnumMode.Search)
        {
          DataContext = postLog;
          txtppID.Visibility = Visibility.Collapsed;
          lblppID.Visibility = Visibility.Collapsed;
          cmbpppo.Visibility = Visibility.Collapsed;          
          lblpppo.Visibility = Visibility.Collapsed;                    
          cmbppChangedBy.IsEnabled = true;
          BindingOperations.ClearBinding(dpppDT, DateTimePicker.ValueProperty);
          dpppDT.KeyDown += dpppDT_KeyDown;
          dpppDT.AllowTextInput = true;
          dpppDT.FormatString = "ddd d MMM yyyy";
          dpppDT.TimePickerVisibility = Visibility.Collapsed;
          if (blnDate)
          {
            dpppDT.Value = postLog.ppDT;
          }
        }
        else
        {          
          postLog.ppChangedBy= Context.User.User.peID;
          if (enumMode == EnumMode.Add)
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
        if (enumMode == EnumMode.Search)
        {
          if (dpppDT.Value != null)
          {
            blnDate = true;
            postLog.ppDT = Convert.ToDateTime(dpppDT.Value);
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
          if (ObjectHelper.IsEquals(postLog, oldPostLog) && enumMode != EnumMode.Add)
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
        UIHelper.ShowMessage(ex);
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
        if (enumMode != EnumMode.ReadOnly && enumMode != EnumMode.Search)
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

    #region dpppDT_KeyDown
    /// <summary>
    /// Deja vacio el valor del dateTimePicker
    /// </summary>
    /// <history>
    /// [emoguel] 29/08/2016 created
    /// </history>
    private void dpppDT_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        e.Handled = true;
        dpppDT.Value = null;
      }
      else if(!(e.Key==Key.Escape))
      {
        e.Handled = true;
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
        if (enumMode == EnumMode.Search)
        {
          lstPersonnel.Insert(0, new PersonnelShort { peID = "", peN = "ALL" });
        }
        cmbppChangedBy.ItemsSource = lstPersonnel;
        cmbpppe.ItemsSource = lstPersonnel;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion

    #endregion
  }
}
