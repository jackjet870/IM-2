using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using System.Linq;

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
        btnCancel_Click(null, null);
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
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(enumMode!=EnumMode.preview)
      {
        if(enumMode==EnumMode.search)
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
          DialogResult = true;
          Close();
        }
        else
        {
          if(ObjectHelper.IsEquals(postLog,oldPostLog) && enumMode!=EnumMode.add)
          {
            Close();
          }
          else
          {
            #region Insertar|Agregar
            string strMsj = ValidateHelper.ValidateForm(this, "Post Log");
            if (strMsj == "")
            {
              int nRes = BRPostsLog.SavePostLog(postLog, (enumMode == EnumMode.edit));
              UIHelper.ShowMessageResult("PostLog", nRes);
              if (nRes == 1)
              {
                postLog = BRPostsLog.GetPostsLog(postLog).FirstOrDefault();
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
      else
      {
        Close();
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
      if(enumMode!=EnumMode.preview && enumMode!=EnumMode.search)
      {
        if (!ObjectHelper.IsEquals(postLog, oldPostLog))
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
    #region LoadPersonnel
    /// <summary>
    /// Carga el combo de Personel y ChangedBy
    /// </summary>
    /// <history>
    /// [emoguel] 12/04/2016
    /// </history>
    private void LoadPersonnel()
    {
      List<PersonnelShort> lstPersonnel = BRPersonnel.GetPersonnel();
      if(enumMode==EnumMode.search)
      {
        lstPersonnel.Insert(0,new PersonnelShort { peID = "", peN = "" });
      }
      cmbppChangedBy.ItemsSource = lstPersonnel;
      cmbpppe.ItemsSource = lstPersonnel;
    }
    #endregion
    #region LoadPosts
    /// <summary>
    /// Carga el comoboBox de Posts
    /// </summary>
    /// <history>
    /// [emoguel] created 12/04/2016
    /// </history>
    private void LoadPosts()
    {
      List<Post> lstPosts = BRPosts.GetPosts();
      cmbpppo.ItemsSource = lstPosts;
    }
    #endregion

    #endregion
    
    
  }
}
