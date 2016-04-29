using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

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
      txtmtID.IsEnabled = (enumMode != EnumMode.edit);
      #region Mode Search
      if (enumMode == EnumMode.search)
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
        btnCancel_Click(null, null);
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
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (enumMode != EnumMode.search)
      {
        if (ObjectHelper.IsEquals(membershipType, oldMembershipType) && !Validation.GetHasError(txtmtLevel) && enumMode!=EnumMode.add)
        {
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Membership Type");
          #region ValidateLevel
          if(!string.IsNullOrWhiteSpace(txtmtLevel.Text.Trim()))
          {
            int nRes = Convert.ToInt32(txtmtLevel.Text.Trim());
            if(nRes>255 || nRes<1)
            {
              strMsj +=((strMsj=="")?"":" \n")+ "Level is out of range. Allowed values are 1 to 255.";
            }
          }
          #endregion
          if (strMsj == "")
          {
            int nRes = BREntities.OperationEntity(membershipType, enumMode);
            UIHelper.ShowMessageResult("Membership Type", nRes);
            if (nRes >0)
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
        nStatus = Convert.ToInt32(cmbSta.SelectedValue);
        DialogResult = true;
        Close();
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
      if (enumMode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(membershipType, oldMembershipType) && enumMode!=EnumMode.search)
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
    #region LoadMemberGroups
    /// <summary>
    /// Llena el combobox de Groups
    /// </summary>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void LoadMemberGroups()
    {
      List<MembershipGroup> lstMembershipGroup = BRMembershipGroups.GetMembershipGroup();
      if (enumMode == EnumMode.search)
      {
        lstMembershipGroup.Insert(0, new MembershipGroup { mgID = "", mgN = "" });
      }
      cmbmtGroup.ItemsSource = lstMembershipGroup;
    }
    #endregion    
    #endregion
  }
}
