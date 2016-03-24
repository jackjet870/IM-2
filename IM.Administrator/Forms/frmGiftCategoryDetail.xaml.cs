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
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftCategoryDetail.xaml
  /// </summary>
  public partial class frmGiftCategoryDetail : Window
  {
    #region Variables
    public GiftCategory giftCategory = new GiftCategory();//Objeto a agregar|Modificar
    public Enums.EnumMode enumMode;//Modo en que se abre la ventana
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
        Close();

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
      DataContext = giftCategory;
      switch(enumMode)//Verificar el modo en que fue abierta la ventana
      {
        case Enums.EnumMode.add:
          {
            txtID.IsEnabled = true;
            txtN.IsEnabled = true;
            chkA.IsEnabled = true;
            break;
          }
        case Enums.EnumMode.edit:
          {
            txtN.IsEnabled = true;
            chkA.IsEnabled = true;
            break;
          }
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
      string strMsj = "";
      int nRes = 0;
      #region validar campos
      if (string.IsNullOrWhiteSpace(txtID.Text))
      {
        strMsj += "Specify the Gift Category ID. \n";
      }
      if (string.IsNullOrWhiteSpace(txtN.Text))
      {
        strMsj += "Specify the Gift Category Description.";
      }
      #endregion

      if(strMsj=="")
      {
        nRes = BRGiftsCategories.SaveGiftCategory(giftCategory, (enumMode == Enums.EnumMode.edit));

        #region respuesta
        switch (nRes)
        {
          case 0:
            {
              UIHelper.ShowMessage("Gift Category not saved.");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Assistance Status successfully saved.");
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              UIHelper.ShowMessage("Gift Category ID already exist please select another one.");
              break;
            }
        }
        #endregion
      }
      else
      {
        UIHelper.ShowMessage(strMsj.TrimEnd('\n'));
      }
    }
    #endregion

    #endregion
              

  }
}
