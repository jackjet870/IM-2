using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmMembershipGroupDetail.xaml
  /// </summary>
  public partial class frmMembershipGroupDetail : Window
  {
    #region Variables
    public MembershipGroup membershipGroup = new MembershipGroup();//Objeto a guardar
    public MembershipGroup oldMembershipGroup = new MembershipGroup();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    private List<MembershipType> _oldLstmembershipTypes = new List<MembershipType>();//Lista inicial de MembershipTypes
    bool blnClosing = false;
    bool isCellCancel = false;
    #endregion
    public frmMembershipGroupDetail()
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
    /// [emoguel] created 19/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(membershipGroup, oldMembershipGroup);
      UIHelper.SetUpControls(membershipGroup, this);
      txtmgID.IsEnabled = (enumMode == EnumMode.add);      
      DataContext = membershipGroup;
      LoadMembershipTypes();
    }
    #endregion

    #region Keydown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
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

    #region Closing
    /// <summary>
    /// Se ejecuta cuando se está cerrando la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing)
      {
        List<MembershipType> lstMembershipTypes = (List<MembershipType>)dgrmembershipTypes.ItemsSource;
        if (!ObjectHelper.IsEquals(membershipGroup, oldMembershipGroup) || !ObjectHelper.IsListEquals(lstMembershipTypes, _oldLstmembershipTypes))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result != MessageBoxResult.Yes)
          {
            e.Cancel = true;
          }
        }
      }
    }
    #endregion

    #region dgrmembershipTypes_CellEditEnding
    /// <summary>
    /// Verifica que no se pueda seleccionar un MembershipType 2 veces
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private void dgrmembershipTypes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))
      {
        isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrmembershipTypes);
        e.Cancel = isRepeat;
      }
      else
      {
        isCellCancel = true;
      }
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();
    }
    #endregion

    #region Accept
    /// <summary>
    /// Agrega| Actualiza un MembershipGroup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<MembershipType> lstMembershipGroup = (List<MembershipType>)dgrmembershipTypes.ItemsSource;
        if (enumMode != EnumMode.add && ObjectHelper.IsEquals(membershipGroup, oldMembershipGroup) && ObjectHelper.IsListEquals(lstMembershipGroup, _oldLstmembershipTypes))
        {
          blnClosing = true;
          Close();
        }
        else
        {
          txtStatus.Text = "Saving Data...";
          skpStatus.Visibility = Visibility.Visible;
          string strMsj = ValidateHelper.ValidateForm(this, "Membership Group");
          if (strMsj == "")
          {
            List<MembershipType> lstAdd = lstMembershipGroup.Where(mt => !_oldLstmembershipTypes.Any(mtt => mtt.mtID == mt.mtID)).ToList();
            int nRes = await BRMembershipGroups.SaveMembershipGroup(membershipGroup, lstAdd, (enumMode == EnumMode.edit));
            UIHelper.ShowMessageResult("Membership Group", nRes);
            if (nRes > 0)
            {
              blnClosing = true;
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
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Membership Group");
      }
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// Elimina registros nuevos con el boton suprimir
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [created] 20/05/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        var item = dgrmembershipTypes.SelectedItem;
        if (item.GetType().Name == "MembershipType")
        {
          MembershipType Membership = (MembershipType)item;
          if (Membership.mtGroup == membershipGroup.mgID)
          {
            e.Handled = true;
          }
        }
      }
    }

    #endregion

    #region dgrmembershipTypes_RowEditEnding
    /// <summary>
    /// EndRowEdit
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void dgrmembershipTypes_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {      
      if (isCellCancel)
      {
        dgrmembershipTypes.RowEditEnding -= dgrmembershipTypes_RowEditEnding;
        dgrmembershipTypes.CancelEdit();
        dgrmembershipTypes.RowEditEnding += dgrmembershipTypes_RowEditEnding;        
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadMembershipTypes
    /// <summary>
    /// Llena el grid y el combobox de MembershipTypes
    /// </summary>
    /// <history>
    /// [emoguel] created 19/06/2016
    /// </history>
    private async void LoadMembershipTypes()
    {
      try
      {
        List<MembershipType> lstAllMembershipTypes = await BRMemberShipTypes.GetMemberShipTypes();
        cmbMembershipTypes.ItemsSource = lstAllMembershipTypes;
        List<MembershipType> lstMembershipTypes = lstAllMembershipTypes.ToList().Where(mt => mt.mtGroup == membershipGroup.mgID).ToList();
        dgrmembershipTypes.ItemsSource = lstMembershipTypes;
        lstMembershipTypes.ForEach(mt =>
        {
          MembershipType mtp = new MembershipType();
          ObjectHelper.CopyProperties(mtp, mt);
          _oldLstmembershipTypes.Add(mtp);
        });
        cmbMembershipTypes.Header = "Membership Type (" + lstMembershipTypes.Count + ")";
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error, "Membership Group");
      }
    }
    #endregion

    #endregion
    
  }
}
