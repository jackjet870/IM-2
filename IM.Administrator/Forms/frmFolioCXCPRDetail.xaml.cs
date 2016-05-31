using System.Collections.Generic;
using System.Linq;
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
  /// Interaction logic for frmFolioCXCPRDetail.xaml
  /// </summary>
  public partial class frmFolioCXCPRDetail : Window
  {
    #region Variables
    public PersonnelShort personnel = new PersonnelShort();//objeto para guardar
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    private List<FolioCxCPR> _lstFolios = new List<FolioCxCPR>();//Lista con los folios iniciales
    private List<FolioCxCCancellation> _lstCancellation=new List<FolioCxCCancellation>();//Lista de folios cancelados
    private bool blnClosing = false;
    public ExecuteCommandHelper KeyEnter { get; set; }
    #endregion
    public frmFolioCXCPRDetail()
    {
      InitializeComponent();
      KeyEnter = new ExecuteCommandHelper(x => txtpeID_KeyDown());
    }

    #region Methods Form
    #region Accept
    /// <summary>
    /// Agrega|Actualiza un folioCxcPR
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (enumMode != EnumMode.search)
      {
        List<FolioCxCPR> lstFoliosPR = (List<FolioCxCPR>)dgrAssigned.ItemsSource;
        List<FolioCxCCancellation> lstFoliosCan = (List<FolioCxCCancellation>)dgrCancelled.ItemsSource;
        bool blnHasChanged = ValidateChanged(lstFoliosPR, lstFoliosCan);
        if (enumMode != EnumMode.add && !blnHasChanged)
        {
          blnClosing = true;
          Close();
        }
        else
        {
          PersonnelShort personnelSave = (PersonnelShort)cmbPersonnel.SelectedItem;
          if (personnelSave != null)
          {
            string strMsj = "";

            if(enumMode==EnumMode.add)
            {
              if(BRFoliosCXCPR.GetPRByFoliosCXC(personnelSave).FirstOrDefault()!=null)
              {
                UIHelper.ShowMessage("The current PR already has folios, edit the correspoding PR.");
                return;
              }
              if(lstFoliosPR.Count==0)
              {
                UIHelper.ShowMessage("Cannot save an empty record, please add folios..");
                return;
              }
            }
            #region FoliosPR
            strMsj = ValidateFoliosPR(lstFoliosPR);
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

            int nRes = BRFoliosCXCPR.SaveFoliosCxCByPR(personnelSave, lstFoliosPR, lstFoliosCan);
            UIHelper.ShowMessageResult("Folios", nRes);
            if (nRes > 0)
            {
              blnClosing = true;
              DialogResult = true;
              Close();
            }

            #endregion
          }
          else
          {
            UIHelper.ShowMessage("Select a PR.");
          }
        }
      }
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
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if(enumMode!=EnumMode.preview && enumMode!=EnumMode.search)
      {
        List<FolioCxCPR> lstFoliosPR = (List<FolioCxCPR>)dgrAssigned.ItemsSource;
        List<FolioCxCCancellation> lstFolioCancel = (List<FolioCxCCancellation>)dgrCancelled.ItemsSource;
        bool blnHasChanged = ValidateChanged(lstFoliosPR, lstFolioCancel);
        if((!string.IsNullOrWhiteSpace(personnel.peID) && enumMode!=EnumMode.edit) || blnHasChanged)
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!blnClosing) { blnClosing = true; Close(); }
          }
          else
          {
            blnClosing = false;
          }
        }
        else
        {
          if (!blnClosing) { blnClosing = true; Close(); }
        }
      }
      else
      {
        if (!blnClosing) { blnClosing = true; Close(); }
      }
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.preview)
      {
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
        btnAccept.Visibility = Visibility.Visible;        
      }
      LoadPrs();
      UIHelper.SetUpControls(new Personnel(), this);
      DataContext = personnel;
      loadReasons();
      LoadFoliosAssigned(personnel.peID);
      LoadCancelled(personnel.peID);
    }

    #endregion

    #region cmbPersonnel_SelectionChanged
    /// <summary>
    /// Asigna el ID del Personnel seleccionado al textbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/05/2016
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

    #region txtpeID_KeyDown
    /// <summary>
    /// Selecciona en el combobox el id insertado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    protected void txtpeID_KeyDown()//(object sender, KeyEventArgs e)
    {      
      cmbPersonnel.SelectedValue = txtpeID.Text;
    }
    #endregion

    #region int_PreviewTextInput
    /// <summary>
    /// Valida que solo se puedan usar números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 06/05/2016
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
    /// [created] 06/05/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        var item = dgrAssigned.SelectedItem;
        var item1 = dgrCancelled.SelectedItem;
        #region Assigned
        if (item !=null && item.GetType().Name == "FolioCxCPR")
        {
          FolioCxCPR folioCxCPR = (FolioCxCPR)item;
          if (folioCxCPR.fcpID == 0)
          {
            dgrAssigned.CancelEdit();
            List<FolioCxCPR> lstFolioCxCPR = (List<FolioCxCPR>)dgrAssigned.ItemsSource;
            lstFolioCxCPR.Remove(folioCxCPR);
            dgrAssigned.Items.Refresh();
          }
        }
        #endregion
        #region Cancelled
        else if(item1 !=null && item1.GetType().Name== "FolioCxCCancellation")
        {
          FolioCxCCancellation folioCancell = (FolioCxCCancellation)item1;
          if (folioCancell.fccID == 0)
          {
            dgrCancelled.CancelEdit();
            List<FolioCxCCancellation> lstFolioCancell = (List<FolioCxCCancellation>)dgrCancelled.ItemsSource;
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
        blnClosing = true;
        btnCancel_Click(null, null);
        if (!blnClosing)
        {
          e.Cancel = true;
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
    /// [emoguel] created 05/05/2016
    /// </history>
    private void LoadPrs()
    {
      List<PersonnelShort> lstPrs = BRPersonnel.GetPersonnelByRole("PR");//BRPersonnel.GetPersonnel( status:2,roles: "PR");
      cmbPersonnel.ItemsSource = lstPrs;
    }
    #endregion

    #region loadReasons
    /// <summary>
    /// Llena el grid de reasons
    /// </summary>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    private void loadReasons()
    {
      List<ReasonCancellationFolio> lstReasons = BRReasonCancellationFolios.GetReasonCancellationFolios(1);
      cmbReason.ItemsSource = lstReasons;
    }
    #endregion

    #region LoadFoliosAssigned
    /// <summary>
    /// Llena el combo de assigment
    /// </summary>
    /// <param name="prID">Id del PR</param>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    private void LoadFoliosAssigned(string prID)
    {
      List<FolioCxCPR> lstFoliosCXCPR = BRFoliosCXCPR.GetFoliosCXCPR(prID);
      _lstFolios = BRFoliosCXCPR.GetFoliosCXCPR(prID);      
      dgrAssigned.ItemsSource = lstFoliosCXCPR;
    }
    #endregion

    #region LoadCancelled
    /// <summary>
    /// Llena el combo de cancelled
    /// </summary>
    /// <param name="prID">Pr Asignado</param>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    private void LoadCancelled(string prID)
    {
      List<FolioCxCCancellation> lstCancelled = BRFoliosCxCCancellation.GetFoliossCxCCancellation(prID);
      _lstCancellation = BRFoliosCxCCancellation.GetFoliossCxCCancellation(prID);
      dgrCancelled.ItemsSource = lstCancelled;
    }
    #endregion

    #region ValidateFoliosPR
    /// <summary>
    /// Valida que todos los FoliosByPR cumplan con los requisitos
    /// </summary>
    /// <param name="lstFolios">Lista a validar</param>
    /// <returns>Cadena de texto</returns>
    /// <history>
    /// [emoguel] created 06/05/2016
    /// </history>
    private string ValidateFoliosPR(List<FolioCxCPR> lstFolios)
    {
      string strMsj = "";
      int nIndex = 0;
      ValidationFolioData validate;
      foreach (FolioCxCPR folio in lstFolios)
      {
        nIndex = lstFolios.IndexOf(folio);
        if (folio.fcpFrom == 0 || folio.fcpTo == 0)
        {
          GridHelper.SelectRow(dgrAssigned, nIndex,0, true);
          strMsj = "Values must be greatter than 0.";
          break;
        }
        else if (folio.fcpTo <= folio.fcpFrom)
        {
          GridHelper.SelectRow(dgrAssigned, nIndex,1, true);
          strMsj = "From value must be greatter or equal than To value.";
          break;
        }
        else if (!BRFoliosCXCPR.ValidateFolioRange(folio.fcpFrom, folio.fcpTo))
        {
          GridHelper.SelectRow(dgrAssigned, nIndex,0, true);
          strMsj = "The assigned range does not exists in the Folios CxC catalog.";
          break;
        }
        validate = BRFoliosCXCPR.ValidateFolio(personnel.peID, folio.fcpFrom, folio.fcpTo, false);
        if (validate.Result==1)
        {
          GridHelper.SelectRow(dgrAssigned, nIndex, 0,true);
          strMsj = "There is a PR with same Folio , check PR: " + validate.PR.ToString();
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
    /// <returns>Cadena de texto</returns>
    /// <history>
    /// [emoguel] created 06/05/2016
    /// </history>
    private string ValidateCancelled(List<FolioCxCCancellation> lstFoliosCancell,List<FolioCxCPR>lstFoliosPR)
    {
      string strMsj = "";
      int nIndex = 0;
      ValidationFolioData validate;
      foreach(FolioCxCCancellation folio in lstFoliosCancell)
      {
        nIndex = lstFoliosCancell.IndexOf(folio);        
        if(folio.fccrcf==null)
        {
          GridHelper.SelectRow(dgrCancelled, nIndex,2,true);
          strMsj = "Reason is a required value, cannot be empty.";
          break;
        }
        else if(folio.fccFrom==0 || folio.fccTo==0)
        {
          GridHelper.SelectRow(dgrCancelled, nIndex,0,true);
          strMsj = "Values must be greatter than 0";
          break;
        }
        else if(folio.fccTo<=folio.fccFrom)
        {
          GridHelper.SelectRow(dgrCancelled, nIndex, 1, true);
          strMsj = "From value must be greatter or equal than To value.";
          break;
        }
        else if(!ValidateFoliosRange(lstFoliosPR,folio))
        {
          GridHelper.SelectRow(dgrCancelled, nIndex, 0, true);
          strMsj = "The range of the canceled folios must be in the range of the asigned folios.";
          break;
        }

        validate = BRFoliosCXCPR.ValidateFolio(personnel.peID, folio.fccFrom, folio.fccTo, true);
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
    /// [emoguel] created 06/05/2016
    /// </history>
    private bool ValidateFoliosRange(List<FolioCxCPR> lstFolios,FolioCxCCancellation folioCancell)
    {
      var folio = lstFolios.Where(fcp => fcp.fcpFrom >= folioCancell.fccFrom && fcp.fcpTo <= folioCancell.fccTo).FirstOrDefault();

      return (folio!=null);
    }
    #endregion

    #region ValidateChanged
    /// <summary>
    /// Verifica si se realizo algun cambio en la ventana
    /// </summary>
    /// <param name="lstAssigned">Folios Asignados</param>
    /// <param name="lstCancel">Folios Cancelados</param>
    /// <returns>True. Si hubo cambios | False. No hubo cambios</returns>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private bool ValidateChanged(List<FolioCxCPR> lstAssigned, List<FolioCxCCancellation> lstCancel)
    {
      bool blnHasChanged = (_lstCancellation.Count != lstCancel.Count || lstAssigned.Count != _lstFolios.Count);
      #region Assigend
      if (blnHasChanged == false)
      {
        _lstFolios.ForEach(fi =>
        {
          FolioCxCPR folioVal = lstAssigned.Where(fi2 => fi2.fcpID == fi.fcpID).FirstOrDefault();
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
          FolioCxCCancellation folioVal = lstCancel.Where(fi2 => fi2.fccID == fi.fccID).FirstOrDefault();
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
