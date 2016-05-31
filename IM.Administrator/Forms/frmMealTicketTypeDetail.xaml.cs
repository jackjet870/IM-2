using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;
using System;

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
      UIHelper.SetUpControls(mealTicketType, this);
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
    /// [emoguel] modified 30/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(mealTicketType, oldMealTicketType) && enumMode != EnumMode.add)
        {
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Meal Ticket Detail");
          if (strMsj == "")
          {
            int nRes = await BREntities.OperationEntity(mealTicketType, enumMode);
            UIHelper.ShowMessageResult("Meal Ticket Detail", nRes);
            if (nRes > 0)
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
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Meal Ticket");
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
  }
}
