using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftCategoryDetail.xaml
  /// </summary>
  public partial class frmGiftCategoryDetail : Window
  {
    #region Variables
    public GiftCategory giftCategory = new GiftCategory();//Objeto a agregar|Modificar
    public GiftCategory oldGiftCategory = new GiftCategory();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abre la ventana
    #endregion
    public frmGiftCategoryDetail()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region Window KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    { 
      if(e.Key==Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);

      }
    }
    #endregion

    #region Window Loaded
    /// <summary>
    /// Llena el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(giftCategory, oldGiftCategory);
      DataContext = giftCategory;
      if(enumMode!= EnumMode.preview)
      {
        txtgcID.IsEnabled = (enumMode == EnumMode.add);
        txtgcN.IsEnabled = true;
        chkA.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetMaxLength(giftCategory, this);
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cierra la ventana pero antes verifica que no se tengan cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if(enumMode!=EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(giftCategory, oldGiftCategory))
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

    #region Accept
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo GiftCategory
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(giftCategory,oldGiftCategory) && enumMode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Gift Category");
        int nRes = 0;

        if (strMsj == "")
        {
          nRes = BRGiftsCategories.SaveGiftCategory(giftCategory, (enumMode == EnumMode.edit));
          UIHelper.ShowMessageResult("Gift Category", nRes);
        }
        else
        {
          UIHelper.ShowMessage(strMsj);
        }
      }
    }
    #endregion

    #endregion
              

  }
}
