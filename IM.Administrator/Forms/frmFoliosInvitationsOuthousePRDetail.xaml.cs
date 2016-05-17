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
  /// Interaction logic for frmFoliosInvitationsOuthousePRDetail.xaml
  /// </summary>
  public partial class frmFoliosInvitationsOuthousePRDetail : Window
  {
    #region Variables
    public PersonnelShort personnel = new PersonnelShort();//objeto para guardar
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    private List<FolioInvitationOuthousePR> _lstFolios = new List<FolioInvitationOuthousePR>();//Lista con los folios iniciales
    private List<FolioInvitationOuthousePRCancellation> _lstCancellation=new List<FolioInvitationOuthousePRCancellation>();//Lista de folios cancelados
    public ExecuteCommandHelper KeyEnter { get; set; }
    #endregion

    public frmFoliosInvitationsOuthousePRDetail()
    {
      InitializeComponent();
      KeyEnter = new ExecuteCommandHelper(x => txtpeID_KeyDown());
    }

    #region Methods Form
    #region Window_Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.preview)
      {
        btnAccept.Visibility = Visibility.Visible;
        if (enumMode == EnumMode.search)
        {
          txtpeID.IsEnabled = true;
          cmbPersonnel.IsEnabled = true;
          SizeToContent = SizeToContent.WidthAndHeight;
          grdContent.Visibility = Visibility.Collapsed;
          cmbPersonnel.Width = 250;
          txtpeID.Width = 250;
        }
        else
        {
          if (enumMode == EnumMode.add)
          {
            txtpeID.IsEnabled = true;
            cmbPersonnel.IsEnabled = true;
          }
          dgrAssigned.IsReadOnly = false;
          dgrCancelled.IsReadOnly = false;          
        }        
      }
      loadReasons();
      LoadFoliosByPr();
      LoadFoliosPrCancellation();
      LoadPrs();
      loadSeries();
      UIHelper.SetUpControls(new Personnel(), this);
      DataContext = personnel;
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/05/2016
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
    /// Agrega|Actualiza Folios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      #region Save
      if (enumMode != EnumMode.search)
      {
        #region ValidateChanges
        List<FolioInvitationOuthousePR> lstFoliosPR = (List<FolioInvitationOuthousePR>)dgrAssigned.ItemsSource;
        List<FolioInvitationOuthousePRCancellation> lstFoliosCan = (List<FolioInvitationOuthousePRCancellation>)dgrCancelled.ItemsSource;
        bool blnHasChanged = ValidateChanges(lstFoliosPR,lstFoliosCan);        
        
        #endregion
        if (enumMode != EnumMode.add && !blnHasChanged)
        {
          Close();
        }
        else
        {
          PersonnelShort personnelSave = (PersonnelShort)cmbPersonnel.SelectedItem;
          if (personnelSave != null)
          {
            string strMsj = "";

            #region ListFolios
            if (enumMode == EnumMode.add)
            {
              if (BRFoliosInvitationsOuthousePR.GetPRbyFolioOuthouse(personnelSave).FirstOrDefault()!= null)
              {
                UIHelper.ShowMessage("The current PR already has folios, edit the correspoding PR.");
                return;
              }
              if (lstFoliosPR.Count == 0)
              {
                UIHelper.ShowMessage("Cannot save an empty record, please add folios..");
                return;
              }
            }
            #endregion
            #region FoliosPR
            strMsj = ValidateFolioOuthouse(lstFoliosPR);
            if (strMsj != "")
            {
              UIHelper.ShowMessage(strMsj);
              return;
            }
            strMsj = ValidateCancelled(lstFoliosCan, lstFoliosPR);
            if(strMsj != "")
            {
              UIHelper.ShowMessage(strMsj);
              return;
            }

            int nRes = BRFoliosInvitationsOuthousePR.SaveFoliosOuthousePR(personnelSave.peID, lstFoliosPR, lstFoliosCan);
            UIHelper.ShowMessageResult("Folios", nRes);
            if (nRes > 0)
            {
              DialogResult = true;
              Close();
            }

            #endregion
          }
        }
      }
      #endregion
      #region Search
      else
      {
        #region Search
        if (cmbPersonnel.SelectedItem != null)
        {
          personnel = (PersonnelShort)cmbPersonnel.SelectedItem;
          DialogResult = true;
          Close();
        }
        else if (!string.IsNullOrWhiteSpace(personnel.peID))
        {
          cmbPersonnel.Focus();
          UIHelper.ShowMessage("Please select a PR.");
        }
        else
        {
          Close();
        }
        #endregion
      }
      #endregion
    }
    #endregion

    #region cmbPersonnel_SelectionChanged
    /// <summary>
    /// Asigna el ID del Personnel seleccionado al textbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// </history>
    private void cmbPersonnel_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      PersonnelShort prCombo = (PersonnelShort)cmbPersonnel.SelectedItem;
      if (prCombo != null)
      {
        txtpeID.Text = prCombo.peID;
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
    /// [emoguel] created 07/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.search && enumMode != EnumMode.preview)
      {
        List<FolioInvitationOuthousePR> lstFoliosPR = (List<FolioInvitationOuthousePR>)dgrAssigned.ItemsSource;
        List<FolioInvitationOuthousePRCancellation> lstFoliosCancel = (List<FolioInvitationOuthousePRCancellation>)dgrCancelled.ItemsSource;
        bool blnHasChanged = ValidateChanges(lstFoliosPR, lstFoliosCancel);
        if ((!string.IsNullOrWhiteSpace(personnel.peID) && enumMode!=EnumMode.edit) || blnHasChanged==true)
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

    #region int_PreviewTextInput
    /// <summary>
    /// Valida que solo se puedan usar números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// </history>
    private void int_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// Elimina registros nuevos con el boton suprimir
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [created] 07/05/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        var item = dgrAssigned.SelectedItem;
        var item1 = dgrCancelled.SelectedItem;
        #region Assigned
        if (item!=null && item.GetType().Name == "FolioInvitationOuthousePR")
        {
          FolioInvitationOuthousePR folio = (FolioInvitationOuthousePR)item;
          if (folio.fipID == 0)
          {
            dgrAssigned.CancelEdit();
            List<FolioInvitationOuthousePR> lstFolios = (List<FolioInvitationOuthousePR>)dgrAssigned.ItemsSource;
            lstFolios.Remove(folio);
            dgrAssigned.Items.Refresh();
          }
        }
        #endregion
        #region Cancelled
        else if(item1!=null && item1.GetType().Name== "FolioInvitationOuthousePRCancellation")
        {
          FolioInvitationOuthousePRCancellation folioCancell = (FolioInvitationOuthousePRCancellation)item1;
          if (folioCancell.ficID == 0)
          {
            dgrCancelled.CancelEdit();
            List<FolioInvitationOuthousePRCancellation> lstFolioCancell = (List<FolioInvitationOuthousePRCancellation>)dgrCancelled.ItemsSource;
            lstFolioCancell.Remove(folioCancell);
            dgrCancelled.Items.Refresh();
          }
        }
        #endregion
      }
    }

    #endregion

    #region txtpeID_KeyDown
    /// <summary>
    /// Selecciona en el combobox el id insertado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// </history>
    protected void txtpeID_KeyDown()//(object sender, KeyEventArgs e)
    {
      cmbPersonnel.SelectedValue = txtpeID.Text;
    }
    #endregion

    #endregion

    #region Methods
    #region LoadPrs
    /// <summary>
    /// Carga el combobox de Prs
    /// </summary>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// </history>
    private void LoadPrs()
    {
      List<PersonnelShort> lstPrs = BRPersonnel.GetPersonnelByRole("PR");
      cmbPersonnel.ItemsSource = lstPrs;
    }
    #endregion

    #region loadReasons
    /// <summary>
    /// Llena el grid de reasons
    /// </summary>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// </history>
    private void loadReasons()
    {
      List<ReasonCancellationFolio> lstReasons = BRReasonCancellationFolios.GetReasonCancellationFolios(1);
      cmbReason.ItemsSource = lstReasons;
    }
    #endregion

    #region LoadSeries
    /// <summary>
    /// Llena los combobox de series
    /// </summary>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    private void loadSeries()
    {
      List<string> lstSeries = BRFoliosInvitationsOuthousePR.GetSeriesFolioOuthouse();
      cmbSerieA.ItemsSource = lstSeries;
      cmbSerieB.ItemsSource = lstSeries;
    }
    #endregion

    #region LoadFoliosByPr
    /// <summary>
    /// Carga el grid de asignados
    /// </summary>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    private void LoadFoliosByPr()
    {
      List<FolioInvitationOuthousePR> lstFolios = BRFoliosInvitationsOuthousePR.GetFoliosByPr(personnel.peID);
      _lstFolios = BRFoliosInvitationsOuthousePR.GetFoliosByPr(personnel.peID);
      dgrAssigned.ItemsSource = lstFolios;
    }
    #endregion

    #region LoadFoliosPrCancellation
    /// <summary>
    /// Carga el grid de cancelados
    /// </summary>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    private void LoadFoliosPrCancellation()
    {
      List<FolioInvitationOuthousePRCancellation> lstCancellations = BRFolioInvitationsOuthousePRCancellation.GetFoliosCancellation(personnel.peID);
      _lstCancellation = BRFolioInvitationsOuthousePRCancellation.GetFoliosCancellation(personnel.peID);
      dgrCancelled.ItemsSource = lstCancellations;
    }
    #endregion

    #region ValidatePrOuthouse
    /// <summary>
    /// valida que los en rango de registros exista en FOlios Invitation outhouse
    /// </summary>
    /// <param name="lstFolios">Lista de folios a validar</param>
    /// <returns>cadena de texto</returns>
    /// <history>
    /// [emoguel] created
    /// </history>
    private string ValidateFolioOuthouse(List<FolioInvitationOuthousePR> lstFolios)
    {
      string strMsj = "";
      int nIndex = 0;
      ValidationFolioData validate;
      foreach (FolioInvitationOuthousePR folio in lstFolios)
      {
        nIndex = lstFolios.IndexOf(folio);
        if(string.IsNullOrWhiteSpace(folio.fipSerie))
        {
          GridHelper.SelectRow(dgrAssigned, nIndex, 0, true);
          strMsj = "Serie is a required value, cannot be empty.";
        }
        else if (folio.fipFrom == 0 || folio.fipTo == 0)
        {
          GridHelper.SelectRow(dgrAssigned, nIndex, 1, true);
          strMsj = "Values must be greatter than 0.";
          break;
        }
        else if (folio.fipTo <= folio.fipFrom)
        {
          GridHelper.SelectRow(dgrAssigned, nIndex, 2, true);
          strMsj = "From value must be greatter or equal than To value.";
          break;
        }
        else if (!BRFoliosInvitationsOuthousePR.ValidateFolioRange(folio.fipSerie,folio.fipFrom, folio.fipTo))
        {
          GridHelper.SelectRow(dgrAssigned, nIndex, 0, true);
          strMsj = "The assigned range does not exists in the Folios Invitations Outhouse catalog.";
          break;
        }
        validate = BRFoliosInvitationsOuthousePR.ValidateFolio(personnel.peID, folio.fipSerie, folio.fipFrom, folio.fipTo,false);
        if (validate.Result == 1)
        {
          GridHelper.SelectRow(dgrAssigned, nIndex, 0, true);
          strMsj = "There is a PR with same Folio and Serie, check PR: " + validate.PR.ToString();
          break;
        }
      }
      return strMsj;
    }
    #endregion

    #region ValidationCancelled
    /// <summary>
    /// Valida los datos de los folios cancelados
    /// </summary>
    /// <param name="lstFoliosCancell">Lista a validar</param>
    /// <param name="lstFoliosPR">Lista de FOlios a asignar</param>
    /// <returns>Cadena de texto</returns>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    private string ValidateCancelled(List<FolioInvitationOuthousePRCancellation> lstFoliosCancell, List<FolioInvitationOuthousePR> lstFoliosPR)
    {
      string strMsj = "";
      int nIndex = 0;
      ValidationFolioData validate;
      foreach (FolioInvitationOuthousePRCancellation folio in lstFoliosCancell)
      {
        nIndex = lstFoliosCancell.IndexOf(folio);
        if(string.IsNullOrWhiteSpace(folio.ficSerie))
        {
          GridHelper.SelectRow(dgrCancelled, nIndex, 0, true);
          strMsj = "Serie is a required value, cannot be empty.";
        }
        else if (folio.ficrcf == null)
        {
          GridHelper.SelectRow(dgrCancelled, nIndex, 3, true);
          strMsj = "Reason is a required value, cannot be empty.";
          break;
        }
        else if (folio.ficFrom == 0 || folio.ficTo == 0)
        {
          GridHelper.SelectRow(dgrCancelled, nIndex, 0, true);
          strMsj = "Values must be greatter than 0";
          break;
        }
        else if (folio.ficTo <= folio.ficFrom)
        {
          GridHelper.SelectRow(dgrCancelled, nIndex, 1, true);
          strMsj = "From value must be greatter or equal than To value.";
          break;
        }
        else if (!ValidateFoliosRange(lstFoliosPR, folio))
        {
          GridHelper.SelectRow(dgrCancelled, nIndex, 0, true);
          strMsj = "The range of the canceled folios must be in the range of the asigned folios.";
          break;
        }

        validate = BRFoliosInvitationsOuthousePR.ValidateFolio(personnel.peID,folio.ficSerie, folio.ficFrom, folio.ficTo, true);
        if (validate.Result == 1)
        {
          GridHelper.SelectRow(dgrAssigned, nIndex, 0, true);
          strMsj = "There is a PR with same Folio, check PR: " + validate.PR.ToString();
          break;
        }

      }
      return strMsj;
    }
    #endregion

    #region ValidateFoliosRange
    /// <summary>
    /// Valida que el rango exista en la lista de asignados
    /// </summary>
    /// <param name="lstFolios">Lista de folios asignados</param>
    /// <param name="folioCancell">Folio a validar</param>
    /// <returns>True. si existe el rango | False. No existe el rango</returns>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    private bool ValidateFoliosRange(List<FolioInvitationOuthousePR> lstFolios, FolioInvitationOuthousePRCancellation folioCancell)
    {
      var folio = lstFolios.Where(fcp => fcp.fipTo >= folioCancell.ficFrom && fcp.fipTo <= folioCancell.ficTo).FirstOrDefault();

      return (folio != null);
    }
    #endregion

    #region ValidateChanged
    /// <summary>
    /// Verifica si se realizo algun cambio en la ventana
    /// </summary>
    /// <param name="lstFoliosPR">Folios Asignados</param>
    /// <param name="lstFoliosCan">Folios Cancelados</param>
    /// <returns>True. Si hubo cambios | False. No hubo cambios</returns>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private bool ValidateChanges(List<FolioInvitationOuthousePR> lstFoliosPR,List<FolioInvitationOuthousePRCancellation> lstFoliosCan)
    {
      
      bool blnHasChanged = (_lstCancellation.Count != lstFoliosCan.Count || lstFoliosPR.Count != _lstFolios.Count);
      #region Assigend
      if (blnHasChanged == false)
      {
        _lstFolios.ForEach(fi =>
        {
          FolioInvitationOuthousePR folioVal = lstFoliosPR.Where(fi2 => fi2.fipID == fi.fipID).FirstOrDefault();
          if (folioVal != null)
          {
            bool blnEquals = ObjectHelper.IsEquals(fi, folioVal);
            if (!blnEquals)
            {
              blnHasChanged = true;
              return;
            }
          }
        });
      }
      #endregion
      #region Canceled
      if (blnHasChanged == false)
      {
        _lstCancellation.ForEach(fi =>
        {
          FolioInvitationOuthousePRCancellation folioVal = lstFoliosCan.Where(fi2 => fi2.ficID == fi.ficID).FirstOrDefault();
          if (folioVal != null)
          {
            bool blnEquals = ObjectHelper.IsEquals(fi, folioVal);
            if (!blnEquals)
            {
              blnHasChanged = true;
              return;
            }
          }
        });
      }
      #endregion

      return blnHasChanged;
    }
    #endregion
    #endregion
  }
}
