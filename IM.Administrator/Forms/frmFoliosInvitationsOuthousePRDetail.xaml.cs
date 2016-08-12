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
using System;

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
    private bool blnClosing = false;
    #endregion

    public frmFoliosInvitationsOuthousePRDetail()
    {
      InitializeComponent();
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
      if (enumMode != EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        if (enumMode == EnumMode.Search)
        {
          cmbPersonnel.IsEnabled = true;
          SizeToContent = SizeToContent.WidthAndHeight;
          grdContent.Visibility = Visibility.Collapsed;
          cmbPersonnel.Width = 250;
        }
        else
        {
          if (enumMode == EnumMode.Add)
          {
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
      dgrCancelled.BeginningEdit += GridHelper.dgr_BeginningEdit;
      dgrAssigned.BeginningEdit += GridHelper.dgr_BeginningEdit;
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
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        #region Save
        if (enumMode != EnumMode.Search)
        {
          #region ValidateChanges
          List<FolioInvitationOuthousePR> lstFoliosPR = (List<FolioInvitationOuthousePR>)dgrAssigned.ItemsSource;
          List<FolioInvitationOuthousePRCancellation> lstFoliosCan = (List<FolioInvitationOuthousePRCancellation>)dgrCancelled.ItemsSource;
          bool blnHasChanged = ValidateChanges(lstFoliosPR, lstFoliosCan);

          #endregion
          if (enumMode != EnumMode.Add && !blnHasChanged)
          {
            blnClosing = true;
            Close();
          }
          else
          {
            txtStatus.Text = "Saving Data";
            skpStatus.Visibility = Visibility.Visible;
            PersonnelShort personnelSave = (PersonnelShort)cmbPersonnel.SelectedItem;
            if (personnelSave != null)
            {
              string strMsj = "";

              #region ListFolios
              if (enumMode == EnumMode.Add)
              {
                var folio = await BRFoliosInvitationsOuthousePR.GetPRbyFolioOuthouse(personnelSave);
                if (folio.FirstOrDefault() != null)
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
              if (strMsj != "")
              {
                UIHelper.ShowMessage(strMsj);
                return;
              }

              int nRes =await BRFoliosInvitationsOuthousePR.SaveFoliosOuthousePR(personnelSave.peID, lstFoliosPR, lstFoliosCan);
              UIHelper.ShowMessageResult("Folios", nRes);
              if (nRes > 0)
              {
                blnClosing = true;
                DialogResult = true;
                Close();
              }

              #endregion
            }
            skpStatus.Visibility = Visibility.Collapsed;
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
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Folio Invitation Outhouse PR");
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

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing)
      {

        List<FolioInvitationOuthousePR> lstFoliosPR = (List<FolioInvitationOuthousePR>)dgrAssigned.ItemsSource;
        List<FolioInvitationOuthousePRCancellation> lstFoliosCancel = (List<FolioInvitationOuthousePRCancellation>)dgrCancelled.ItemsSource;
        bool blnHasChanged = ValidateChanges(lstFoliosPR, lstFoliosCancel);
        if ((!string.IsNullOrWhiteSpace(personnel.peID) && enumMode != EnumMode.Edit) || blnHasChanged == true)
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            dgrAssigned.CancelEdit();
            dgrCancelled.CancelEdit();
          }
        }
      }
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
    private async void LoadPrs()
    {
      try
      {
        List<PersonnelShort> lstPrs = await BRPersonnel.GetPersonnelByRole("PR");
        cmbPersonnel.ItemsSource = lstPrs;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Folios Invitation Houthouse By PR");
      }
    }
    #endregion

    #region loadReasons
    /// <summary>
    /// Llena el grid de reasons
    /// </summary>
    /// <history>
    /// [emoguel] created 07/05/2016
    /// [emoguel] se volvió async
    /// </history>
    private async void loadReasons()
    {
      try
      {
        List<ReasonCancellationFolio> lstReasons =await BRReasonCancellationFolios.GetReasonCancellationFolios(1);
        cmbReason.ItemsSource = lstReasons;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Folio Invitations Outhouse");
      }
    }
    #endregion

    #region LoadSeries
    /// <summary>
    /// Llena los combobox de series
    /// </summary>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    private async void loadSeries()
    {
      try
      {
        List<string> lstSeries =await BRFoliosInvitationsOuthousePR.GetSeriesFolioOuthouse();
        cmbSerieA.ItemsSource = lstSeries;
        cmbSerieB.ItemsSource = lstSeries;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Folio Invitations Outhouse");
      }
    }
    #endregion

    #region LoadFoliosByPr
    /// <summary>
    /// Carga el grid de asignados
    /// </summary>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    private async void LoadFoliosByPr()
    {
      try
      {
        List<FolioInvitationOuthousePR> lstFolios =await BRFoliosInvitationsOuthousePR.GetFoliosByPr(personnel.peID);
        _lstFolios =await BRFoliosInvitationsOuthousePR.GetFoliosByPr(personnel.peID);
        dgrAssigned.ItemsSource = lstFolios;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Folio Invitations Outhouse");
      }
    }
    #endregion

    #region LoadFoliosPrCancellation
    /// <summary>
    /// Carga el grid de cancelados
    /// </summary>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    private async void LoadFoliosPrCancellation()
    {
      try
      {
        List<FolioInvitationOuthousePRCancellation> lstCancellations = await BRFolioInvitationsOuthousePRCancellation.GetFoliosCancellation(personnel.peID);
        _lstCancellation =await BRFolioInvitationsOuthousePRCancellation.GetFoliosCancellation(personnel.peID);
        dgrCancelled.ItemsSource = lstCancellations;
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Folio Invitations Outhouse");
      }
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
