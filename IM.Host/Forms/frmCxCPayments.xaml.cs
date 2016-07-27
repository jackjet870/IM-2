using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
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

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmCxCPayments.xaml
  /// </summary>
  public partial class frmCxCPayments : Window
  {
    #region Atributos
    private ExchangeRateShort _lstExchangeRate;
    CollectionViewSource cxCPaymentShortViewSource;
    int giftReceiptID;
    string strUserID;
    string strUserName;
    public static DateTime _dtpServerDate;
    public static double dbExchange;
    double dbTextExchange;
    decimal dcUSD;
    decimal dcMXN;
    string previousText = String.Empty;
    #endregion

    #region Constructores y Destructores
    public frmCxCPayments(int iGiftReceiptID)
    {
      InitializeComponent();
      cxCPaymentShortViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cxCPaymentShortViewSource")));
      giftReceiptID = iGiftReceiptID;
      _dtpServerDate = BRHelpers.GetServerDate();
      _lstExchangeRate = BRExchangeRate.GetExchangeRatesByDate(DateTime.Now, "MEX").FirstOrDefault();
      dbExchange = (double)_lstExchangeRate.exExchRate;
      
    }
    #endregion

    #region Metodos de la ventana

    /// <summary>
    /// 
    /// </summary>
    ///<history>
    ///[michan] 16/Junio/2016 Created
    ///</history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      
      LoadPayments();
      LoadTexBox();
      strUserID = App.User.User.peID;
      strUserName = App.User.User.peN;
      tbxReceivedBy.Text = $"{strUserName}";
      lblReceiptID.Content = $"{giftReceiptID}";
      lblPaymentDt.Content = String.Format("{0:dd/MM/yyyy}", _dtpServerDate);
      lblExchangeRate.Content = String.Format("{0:C}", (1/dbExchange));// $"{dbExchange}"; 
      
    }

    #region tbx_KeyDown
    /// <summary>
    /// Evento para la tecla presionada
    /// </summary>
    ///<history>
    ///[michan] 16/Junio/2016 Created
    ///</history>
    private void tbx_KeyDown(object sender, KeyEventArgs e)
    {
      /*if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
        e.Handled = false;
      else if (e.Key == Key.Decimal)//Key.OemComma
        e.Handled = false;
      else
        e.Handled = true;*/
      //e.Handled = !ValidateHelper.OnlyDecimals(e., sender as TextBox);
      switch (((TextBox)sender).Name)
      {
        case "tbxUSD":
          tbxUSD.TextChanged += tbx_TextChanged;
          break;
        case "tbxMXN":
          tbxMXN.TextChanged += tbx_TextChanged;
          break;
        default:
          break;
      }
      
    }
    #endregion



    #region Decimal_PreviewTextInput
    /// <summary>
    /// Valida que el texto introducido sea decimal
    /// </summary>
    /// <history>
    /// [michan] 23/07/2016 Created
    /// </history>
    private void Decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyDecimals(e.Text, sender as TextBox);
      if(!e.Handled)
        switch (((TextBox)sender).Name)
        {
          case "tbxUSD":
            tbxUSD.TextChanged += tbx_TextChanged;
            break;
          case "tbxMXN":
            tbxMXN.TextChanged += tbx_TextChanged;
            break;
          default:
            break;
        }
    }
    #endregion



    #region tbx_TextChanged
    /// <summary>
    /// Valida en cabio en los texbox
    /// </summary>
    ///<history>
    ///[michan] 16/Junio/2016 Created
    ///</history>
    private void tbx_TextChanged(object sender, TextChangedEventArgs e)
    {
      var tbx = ((TextBox)sender);
      bool valid = Valid(sender);
      if (valid && tbx.Name == "tbxUSD")
      {
        ApplyRate("USD", dbTextExchange);
      }
      else if (valid && tbx.Name == "tbxMXN")
      {
        ApplyRate("MXN", dbTextExchange);
      }
      tbx.TextChanged -= tbx_TextChanged;
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    /// Guarda las cantidades ingresadas.
    /// </summary>
    ///<history>
    ///[michan] 16/Junio/2016 Created
    ///</history>
    private void btnSave_Click(object sender, MouseButtonEventArgs e)
    {
      Save();
    }
    #endregion
    
    #endregion

    #region LoadPayments
    /// <summary>
    /// Carga el registro de los Payments el el grid
    /// </summary>
    ///<history>
    ///[michan] 16/Junio/2016 Created
    ///</history>
    public async void LoadPayments()
    {
      cxCPaymentShortViewSource.Source = await BRCxC.GetCxCPayments(giftReceiptID);      
    }
    #endregion

    #region LoadTexBox
    /// <summary>
    /// Inicializa los TexBox
    /// </summary>
    ///<history>
    ///[michan] 16/Junio/2016 Created
    ///</history>
    public void LoadTexBox()
    {
      tbxMXN.Text = "0.00";
      tbxUSD.Text = "0.00";
    }
    #endregion

    #region Valid
    /// <summary>
    /// Valida el texto en el TexBox
    /// </summary>
    ///<history>
    ///[michan] 16/Junio/2016 Created
    ///</history>
    public bool Valid(object sender)
    {
      if (string.IsNullOrEmpty(((TextBox)sender).Text))//((TextBox)sender).Text
      {
        previousText = "";
      } 
      else
      {
        double db = 0;
        
        bool success = double.TryParse(((TextBox)sender).Text, out db);//((TextBox)sender).Text
        if (success & db >= 0)
        {
          ((TextBox)sender).Text.Trim();
          //strText.Trim();
          previousText = ((TextBox)sender).Text;//strText.Trim();
          dbTextExchange = db;
          
          return true;
        }
        else
        {
          ((TextBox)sender).Text = previousText;
          ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
          return false;
        }
      }
      
      return false;

    }
    #endregion

    #region ApplyRate
    /// <summary>
    /// Realiza el calculo del rate en base a cual fue modificado
    /// </summary>
    /// <param name="strModified">Tipo de exchange</param>
    /// <param name="dbAmount">Valor modificado</param>
    ///<history>
    ///[michan] 16/Junio/2016 Created
    ///</history>
    public void ApplyRate(string strModified, double dbAmount)
    {
      if (strModified == "USD")
      {
        dcUSD = Convert.ToDecimal(dbAmount);
        dcMXN = Convert.ToDecimal(dbAmount / dbExchange);
        tbxMXN.Text = String.Format("{0:C}", dcMXN);
      }
      else //if (strModified == "MXN")
      {
        dcMXN = Convert.ToDecimal(dbAmount);
        dcUSD = Convert.ToDecimal(dbAmount * dbExchange);
        tbxUSD.Text = String.Format("{0:C}", dcUSD);
      }
    }
    #endregion

    #region Save
    /// <summary>
    /// Guarda el rate que fue modificado
    /// </summary>
    ///<history>
    ///[michan] 16/Junio/2016 Created
    ///</history>
    public async void Save()
    {
      string message = string.Empty;
      bool error = false;
      if (String.IsNullOrEmpty(tbxUSD.Text) || String.IsNullOrEmpty(tbxMXN.Text))
      {
        message = "Inser a valid Amount.";
        error = !error;
      }
      else if (dcUSD <= 0 || dcMXN <= 0)
      {
        message = "The amount must be greater than 0.";
        error = !error;
      }
      else
      {
        var list = await BRGiftsReceiptsPayments.AddGiftReceiptPayment(giftReceiptID, strUserID, _dtpServerDate, dcUSD, (decimal)dbExchange, dcMXN);
        var er = list.FirstOrDefault();
        if (er.iRes < 0)
        {
          message = er.sRes;
          error = !error;
        }
        else
        {
          message = "Transaction Added.";
          LoadTexBox();
          LoadPayments();
        }

      }
      UIHelper.ShowMessage(message, (error) ? MessageBoxImage.Error : MessageBoxImage.Information, "CxC Payments");
    }
    #endregion

  }
}
