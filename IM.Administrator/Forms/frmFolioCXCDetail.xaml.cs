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
using IM.BusinessRules.BR;
using IM.Model;
using IM.Administrator.Enums;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmFolioCXCDetail.xaml
  /// </summary>
  public partial class frmFolioCXCDetail : Window
  {
    public FolioCXC folioCXC = new FolioCXC();//objeto a guardar o actualizar
    public EnumMode enumMode;//Modo en el que se mostrará la ventana

    public frmFolioCXCDetail()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        Close();
      }
    }
    #endregion

    #region Window Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      OpenMode();
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda|Actualiza un registro del catalogo de FoliosCXC
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      string strMsj = "";
      int nRes = 0;
      #region Validar El rango del folio      
      if (folioCXC.fiFrom == 0)
      {
        txtFrom.Text = "0";
        strMsj += "Start number can not be 0.";
      }
      else
      {
        if (folioCXC.fiTo < folioCXC.fiFrom)
        {
          if(string.IsNullOrWhiteSpace(txtTo.Text))
          {
            txtTo.Text = txtFrom.Text;
          }
          strMsj += "Start number can not be greater than End Number.";
        }
      }
      #endregion

      if (strMsj=="")
      {
        nRes = BRFoliosCXC.SaveFolioCXC(folioCXC, (enumMode == EnumMode.edit));

        #region respuesta
        switch (nRes)//Se valida la respuesta de la operacion
        {
          case 0:
            {
              UIHelper.ShowMessage("Folio CXC Type not saved.");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Folio CXC successfully saved.");
              DialogResult = true;
              Close();
              break;
            }
          case 2:
            {
              UIHelper.ShowMessage("Please check the ranges, impossible save it.");
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
    #region Preview text Input
    /// <summary>
    /// Valida que únicamente se acepten números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtFrom_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }
    #endregion

    #region txtLostFocus
    /// <summary>
    /// Cambia el valor del texbox cuando pierde el foco
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void txt_LostFocus(object sender, RoutedEventArgs e)
    {
      TextBox txtText = (TextBox)sender;
      if (string.IsNullOrWhiteSpace(txtText.Text))
      {
        txtText.Text = "0";
      }
    }
    #endregion
    #endregion
    #region Métodos
    #region OpenMode
    /// <summary>
    /// Cambia el estado de los controles dependiendo del modo en que se abrio
    /// </summary>
    /// <history>
    /// [emoguel] created 22/03/2016
    /// </history>
    private void OpenMode()
    {
      DataContext = folioCXC;
      if (enumMode != EnumMode.preview)
      {
        btnAccept.Visibility = Visibility.Visible;
        txtFrom.IsEnabled = true;
        txtTo.IsEnabled = true;
        chkA.IsEnabled = true;
      }
    }
    #endregion

    #endregion

    
  }
}
