using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmMealTicketTypeDetail.xaml
  /// </summary>
  public partial class frmMealTicketTypeDetail : Window
  {
    #region Variables
    public MealTicketType mealTicketType = new MealTicketType();//Objeto a guardar
    public MealTicketType oldMealTicketType = new MealTicketType();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    #endregion
    public frmMealTicketTypeDetail()
    {
      InitializeComponent();
    }
    #region Window Loaded
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
      ObjectHelper.CopyProperties(mealTicketType, oldMealTicketType);
      DataContext = mealTicketType;
      txtmyID.IsEnabled = (enumMode == EnumMode.add);
      UIHelper.SetMaxLength(mealTicketType, this);
    }
    #endregion

    #region Window Key Down
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
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
    /// Guarda|Actualiza registros en el catalogo MealTicketTypes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (ObjectHelper.IsEquals(mealTicketType, oldMealTicketType) && enumMode != EnumMode.add)
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this,"Meal Ticket Detail");
        if(strMsj=="")
        {
          int nRes = BRMealTicketTypes.SaveMealTicketTypes(mealTicketType,(enumMode==EnumMode.edit));
          UIHelper.ShowMessageResult("Meal Ticket Detail", nRes);
          if(nRes==1)
          {
            DialogResult = true;
            Close();
          }
        }
        else
        {
          MessageBox.Show(strMsj);
        }

      }
    }

    #endregion
    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando los cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {      
      if (!ObjectHelper.IsEquals(mealTicketType, oldMealTicketType))
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
    #endregion

    #region txt_NumberTextInput
    /// <summary>
    /// Solamente se pueden escribir números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void txt_NumberTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if(e.Text=="." && !txt.Text.Trim().Contains("."))
      {
        e.Handled = false;
      }
      else
      {
        e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
      }
      
    }
    #endregion

    #region LostFocus
    /// <summary>
    /// Convierte el formato de numero currency a estandar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void txt_LostFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (string.IsNullOrWhiteSpace(txt.Text.Trim()))
      {
        txt.Text = "0";
      }
    }
    #endregion

    #region GotFocus
    /// <summary>
    /// Convierte el formato de numero estandar a currency
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void txt_GotFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      txt.Text = ConvertHelper.DoubleCurrencyToStandar(txt.Text);      
    } 
    #endregion
  }
}
