using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSalesAmountRangeDetail.xaml
  /// </summary>
  public partial class frmSalesAmountRangeDetail : Window
  {
    #region variables
    public SalesAmountRange salesAmountRange = new SalesAmountRange();//Objeto a guardar
    public SalesAmountRange oldSalAmoRan = new SalesAmountRange();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    public int nStatus = -1;//Estatus para el modo busqueda
    private bool _isClosing = false;
    #endregion
    public frmSalesAmountRangeDetail()
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
    /// [emoguel] created 20/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

      UIHelper.SetUpControls(salesAmountRange, this, enumMode);
      ObjectHelper.CopyProperties(salesAmountRange, oldSalAmoRan);
      if (enumMode == EnumMode.Search)
      {
        #region Configurar Controles
        lblStatus.Visibility = Visibility.Visible;
        cmbStatus.Visibility = Visibility;
        chksnA.Visibility = Visibility.Collapsed;
        txtsnID.IsEnabled = true;
        Title = "Search";
        ComboBoxHelper.LoadComboDefault(cmbStatus);
        #endregion
        #region Valor a los campos
        cmbStatus.SelectedValue = nStatus;
        dynamic salesAmoRange = new
        {
          snID= (salesAmountRange.snID > 0) ? salesAmountRange.snID.ToString() : "",
          snN = (salesAmountRange.snN != null) ? salesAmountRange.snN.ToString() : "",
          snFrom = (salesAmountRange.snFrom > 0) ? (decimal?)salesAmountRange.snFrom : null,
          snTo = (salesAmountRange.snTo > 0) ? (decimal?)salesAmountRange.snTo : null,
        };
        UIHelper.UiSetDatacontext(salesAmoRange, this, BindingMode.OneWay);
        DataContext = salesAmoRange;
        #endregion
      }
      else
      {        
        txtsnTo.LostFocus += LostFocus;
        txtsnFrom.LostFocus += LostFocus;        
        UIHelper.UiSetDatacontext(salesAmountRange, this);
        DataContext = salesAmountRange;
      }      
    }
    #endregion

    #region keyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/04/2016
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
    /// Guarda|Actualiza un registro en el catalogo salesAmountRanges
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/044/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.Search)
      {
        if (enumMode != EnumMode.Add && ObjectHelper.IsEquals(salesAmountRange, oldSalAmoRan))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          #region Save
          string strMsj = ValidateHelper.ValidateForm(this, "Sales Amount Range");
          #region Validar Rangos          

          if (salesAmountRange.snFrom > salesAmountRange.snTo)
          {
            strMsj += (strMsj != "") ? " \n " : "" + "Start Amount Can not be greater than End amount.";
          }

          #endregion

          if (strMsj == "")
          {
            int nRes = BRSaleAmountRanges.SaveSalesAmountRange(salesAmountRange, (enumMode == EnumMode.Edit));
            UIHelper.ShowMessageResult("Sales Amount Range", nRes);
            if (nRes == 1)
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
          #endregion
        }
      }
      else
      {
        #region Search
        bool blnDialogResult = true;
        salesAmountRange.snN = txtsnN.Text;
        #region Validacion Int
        if (!string.IsNullOrWhiteSpace(txtsnID.Text))
        {
          int nID = Convert.ToInt32(txtsnID.Text);
          if(nID>0)
          {
            salesAmountRange.snID = nID;
          }
          else
          {
            blnDialogResult = false;
            UIHelper.ShowMessage("ID can not be 0.");
          }
        }
        #endregion
        #region ValidateRanges        
        if (!string.IsNullOrWhiteSpace(txtsnFrom.Text) && string.IsNullOrWhiteSpace(txtsnTo.Text))
        {
          UIHelper.ShowMessage("Specify the Amount to.");
          blnDialogResult = false;
        }
        else if (!string.IsNullOrWhiteSpace(txtsnTo.Text) && string.IsNullOrWhiteSpace(txtsnFrom.Text))
        {
          UIHelper.ShowMessage("Specify the Amount from.");
          blnDialogResult = false;
        }
        else if (!string.IsNullOrWhiteSpace(txtsnFrom.Text) && !string.IsNullOrWhiteSpace(txtsnTo.Text))
        {
          salesAmountRange.snFrom = Convert.ToDecimal(txtsnFrom.Text);
          salesAmountRange.snTo = Convert.ToDecimal(txtsnTo.Text);
          if (salesAmountRange.snTo < salesAmountRange.snFrom)
          {
            UIHelper.ShowMessage("Amount From can not be greater than Amount To.");
            blnDialogResult = false;
          }

        }
        else
        {
          salesAmountRange.snFrom = 0;
          salesAmountRange.snTo = 0;
        } 
        #endregion

        if (blnDialogResult == true)
        {
          _isClosing = true;
          nStatus = Convert.ToInt32(cmbStatus.SelectedValue);
          DialogResult = true;
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
    /// [emoguel] created 20/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      Close();
    }
    #endregion
    

    #region LostFocus
    /// <summary>
    /// Cambia el texto de la descripción si cambia algun valor entre From y To
    /// </summary>
    /// <history>
    /// [emoguel] created 21/04/2016
    /// </history>
    private new void LostFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = sender as TextBox;
      var bindingExpresion = txt.GetBindingExpression(TextBox.TextProperty);
      if(bindingExpresion!=null)
      {
        bindingExpresion.UpdateSource();
      }
      txtsnN.Text = $"${salesAmountRange.snFrom} - ${salesAmountRange.snTo}";      
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Cierra la ventana verificando si hay cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 28/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(!_isClosing)
      {
        if (enumMode != EnumMode.Search)
        {
          if (!ObjectHelper.IsEquals(salesAmountRange, oldSalAmoRan))
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
  }
}
