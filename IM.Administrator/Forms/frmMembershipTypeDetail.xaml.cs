using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmMembershipTypeDetail.xaml
  /// </summary>
  public partial class frmMembershipTypeDetail : Window
  {
    #region variables
    public MembershipType membershipType = new MembershipType();//Objeto a guardar
    public MembershipType oldMembershipType = new MembershipType();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    public int nStatus = -1;//Estatus para el modo Search
    private bool _isClosing = false;
    #endregion
    public frmMembershipTypeDetail()
    {
      InitializeComponent();
    }

    #region Methods Forms
    #region Window loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(membershipType, oldMembershipType);
      DataContext = membershipType;
      LoadMemberGroups();
      txtmtID.IsEnabled = (enumMode != EnumMode.Edit);
      #region Mode Search
      if (enumMode == EnumMode.Search)
      {
        chkmtA.Visibility = Visibility.Collapsed;
        cmbSta.Visibility = Visibility.Visible;
        lblFrom.Visibility = Visibility.Collapsed;
        lblLevel.Visibility = Visibility.Collapsed;
        lblTo.Visibility = Visibility.Collapsed;
        txtmtLevel.Visibility = Visibility.Collapsed;
        txtmtTo.Visibility = Visibility.Collapsed;
        txtmtFrom.Visibility = Visibility.Collapsed;
        lblStatus.Visibility = Visibility.Visible;
        ComboBoxHelper.LoadComboDefault(cmbSta);
        cmbSta.SelectedValue = nStatus;
        Title = "Search";
      }
      #endregion
      UIHelper.SetUpControls(membershipType, this);
    }
    #endregion

    #region Window KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// Guarda un objeto en el catalogo membershipType
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// [emoguel] modified 30/05/2016 se volvió async
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.Search)
        {
          if (ObjectHelper.IsEquals(membershipType, oldMembershipType) && !Validation.GetHasError(txtmtLevel) && enumMode != EnumMode.Add)
          {
            _isClosing = true;
            Close();
          }
          else
          {
            txtStatus.Text = "Saving Data...";
            skpStatus.Visibility = Visibility.Visible;
            string strMsj = ValidateHelper.ValidateForm(this, "Membership Type");
            #region ValidateLevel
            if (!string.IsNullOrWhiteSpace(txtmtLevel.Text.Trim()))
            {
              int nRes = Convert.ToInt32(txtmtLevel.Text.Trim());
              if (nRes > 255 || nRes < 1)
              {
                strMsj += ((strMsj == "") ? "" : " \n") + "Level is out of range. Allowed values are 1 to 255.";
              }
            }
            #endregion
            if (strMsj == "")
            {
              int nRes = await BREntities.OperationEntity(membershipType, enumMode);
              UIHelper.ShowMessageResult("Membership Type", nRes);
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
            skpStatus.Visibility = Visibility.Collapsed;
          }
        }
        else
        {
          _isClosing = true;
          nStatus = Convert.ToInt32(cmbSta.SelectedValue);
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
    /// cierra la ventana verificando que no haya cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica cambios antes de cerrar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(!_isClosing)
      {
        if (enumMode != EnumMode.ReadOnly)
        {
          if (!ObjectHelper.IsEquals(membershipType, oldMembershipType) && enumMode != EnumMode.Search)
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
    #region LoadMemberGroups
    /// <summary>
    /// Llena el combobox de Groups
    /// </summary>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private async void LoadMemberGroups()
    {
      try
      {
        List<MembershipGroup> lstMembershipGroup = await BRMembershipGroups.GetMembershipGroups();
        if (enumMode == EnumMode.Search)
        {
          lstMembershipGroup.Insert(0, new MembershipGroup { mgID = "", mgN = "ALL" });
        }
        cmbmtGroup.ItemsSource = lstMembershipGroup;
        skpStatus.Visibility = Visibility.Collapsed;
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
