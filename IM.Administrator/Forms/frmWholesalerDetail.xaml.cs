using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmWholesalerDetail.xaml
  /// </summary>
  public partial class frmWholesalerDetail : Window
  {
    #region Variables
    public WholesalerData wholesalerData = new WholesalerData();//Para cuando se abra la ventana como filtro
    public Wholesaler wholesaler = new Wholesaler();//Objeto a guardar
    public Wholesaler oldWholesaler = new Wholesaler();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo de la ventana
    private bool _isClosing = false;
    #endregion
    public frmWholesalerDetail()
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
    /// [emoguel] created 07/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (enumMode == EnumMode.search)
      {
        Title = "Search";
        txtName.IsEnabled = true;
        txtwsApplication.IsEnabled = true;
        txtwsCompany.IsEnabled = true;
        cmbClub.IsEnabled = true;
        txtName.Visibility = Visibility.Visible;
        lblName.Visibility = Visibility.Visible;        
        DataContext = wholesalerData;
        btnAccept.Visibility = Visibility.Visible;
        DataContext = wholesalerData;
        txtwsCompany.PreviewTextInput += TextBoxHelper.IntTextInput;        
      }
      else
      {
        ObjectHelper.CopyProperties(wholesaler, oldWholesaler);        
        if(enumMode==EnumMode.add)
        {
          txtwsApplication.IsEnabled = true;
          txtwsCompany.IsEnabled = true;
          cmbClub.IsEnabled = true;
          btnAccept.Visibility = Visibility.Visible;
          UIHelper.SetUpControls(wholesaler, this);
        }
        DataContext = wholesaler;
      }      
      LoadClubs();
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region isClosing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        _isClosing = true;
        btnCancel_Click(null, null);
        if (!_isClosing)
        {
          e.Cancel = true;
        }
        else
        {
          _isClosing = false;
        }
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda los datos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.search)
      {
        if(ObjectHelper.IsEquals(wholesaler,oldWholesaler))
        {
          _isClosing = true;
          Close();
        }
        else
        {
          txtStatus.Text = "Saving Data...";
          skpStatus.Visibility = Visibility.Visible;
          string strMsj = "";
          strMsj = ValidateHelper.ValidateForm(this, "Wholesaler");          
          if(strMsj=="")
          {
            int nRes = await BRWholesalers.SaveWholesaler(wholesaler);
            UIHelper.ShowMessageResult("Whosaler", nRes);
            if(nRes>0)
            {
              var wholesalersData = await BRWholesalers.GetWholesalers(new WholesalerData { wscl = wholesaler.wscl, wsApplication = wholesaler.wsApplication, wsCompany = wholesaler.wsCompany });
              wholesalerData = wholesalersData.FirstOrDefault();
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
        DialogResult = true;
        Close();
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
    /// [emoguel] created 07/06/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if (enumMode != EnumMode.search && enumMode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(wholesaler, oldWholesaler))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!_isClosing) { _isClosing = true; Close(); }
          }
          else
          {
            _isClosing = false;
          }
        }
        else
        {
          if (!_isClosing) { _isClosing = true; Close(); }
        }
      }
      else
      {
        if (!_isClosing) { _isClosing = true; Close(); }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadClubs
    /// <summary>
    /// Carga el grid de clubs
    /// </summary>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private async void LoadClubs()
    {
      try
      {
        List<Club> lstClubs = await BRClubs.GetClubs(null, 1);
        if (enumMode == EnumMode.search)
        {
          lstClubs.Insert(0, new Club { clID = 0, clN = "" });
        }
        cmbClub.ItemsSource = lstClubs;
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Clubs");
      }
    } 
    #endregion

    #endregion
  }
}
