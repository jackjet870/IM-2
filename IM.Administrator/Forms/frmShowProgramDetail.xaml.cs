using System;
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
      if(enumMode==EnumMode.search)
      {
        cmbStatus.Visibility = Visibility.Visible;
        lblStatus.Visibility = Visibility.Visible;
        chkstA.Visibility = Visibility.Collapsed;
        ComboBoxHelper.LoadComboDefault(cmbStatus);
        cmbStatus.SelectedValue = nStatus;
      }
      txtskID.IsEnabled = (enumMode != EnumMode.edit);
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
        btnCancel_Click(null, null);
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
        if (enumMode != EnumMode.search)
        {
          if (enumMode != EnumMode.add && ObjectHelper.IsEquals(showProgram, oldShowProgram))
          {
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
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Show Program");
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
      if(enumMode!=EnumMode.search)
      {
        if(!ObjectHelper.IsEquals(showProgram,oldShowProgram))
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
    #region LoadCategories
    /// <summary>
    /// Llena el combobox de categories
    /// </summary>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    private void LoadCategories()
    {
      List<ShowProgramCategory> lstShowProCategories = BRShowProgramsCategories.GetShowProgramsCategories();
      if (enumMode == EnumMode.search)
      {
        lstShowProCategories.Insert(0, new ShowProgramCategory { sgID = "", sgN = "" });
      }
      cmbsksg.ItemsSource = lstShowProCategories;
    } 
    #endregion
    #endregion
  }
}
