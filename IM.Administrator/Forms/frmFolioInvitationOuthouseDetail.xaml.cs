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
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Administrator.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmFoliosInvitationOutsideDetail.xaml
  /// </summary>
  public partial class frmFolioInvitationOuthouseDetail : Window
  {
    #region Variables
    public FolioInvitationOuthouse folioInvOut = new FolioInvitationOuthouse();//Objeto a editar o agregar
    public EnumMode enumMode;//Modo de la ventana
    #endregion
    public frmFolioInvitationOuthouseDetail()
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
      if (e.Key == Key.Escape)
      {
        Close();
      }
    }
    #endregion

    #region WindowLoaded
    /// <summary>
    /// carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      DataContext = folioInvOut;
      if (enumMode != EnumMode.preview)
      {
        txtSerie.IsEnabled = true;
        txtFrom.IsEnabled = true;
        txtTo.IsEnabled = true;
        chkA.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
      }
    } 
    #endregion

    #region PreviweTextNum
    /// <summary>
    /// Verifica que solo se escriban numeros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void txtFrom_PreviewTextNum(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }
    #endregion

    #region PreviewTextLetter
    /// <summary>
    /// Verifica que solo se escriban Letras
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void txtFrom_PreviewTextLetter(object sender, TextCompositionEventArgs e)
    {
      if (!char.IsLetter(Convert.ToChar(e.Text)))
      {
        e.Handled = true;
      }
    }
    #endregion

    #endregion

    #region Methods
    #region Accept
    /// <summary>
    /// Agrega o actualiza un registro dependiendo del modo de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      int nRes = 0;
      string strMsj = "";
      #region Validar El rango del folio      
      if(string.IsNullOrWhiteSpace(txtSerie.Text))
      {
        strMsj += "Specify the Serie. \n";
      }

      if (folioInvOut.fiFrom == 0)
      {
        txtFrom.Text = "0";
        strMsj += "Start number can not be 0.";
      }
      else
      {
        if (folioInvOut.fiTo < folioInvOut.fiFrom)
        {
          if (string.IsNullOrWhiteSpace(txtTo.Text))
          {
            txtTo.Text = txtFrom.Text;
          }
          strMsj += "Start number can not be greater than End Number.";
        }
      }
      #endregion

      if (strMsj == "")
      {
        nRes = BRFoliosInvitationsOuthouse.SaveFolioInvittionsOutside(folioInvOut, (enumMode == EnumMode.edit));

        #region respuesta
        switch (nRes)//Se valida la respuesta de la operacion
        {
          case 0:
            {
              UIHelper.ShowMessage("Folio Invitation Outhouse Type not saved.");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Folio Invitation Outhouse successfully saved.");
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

    #region LostFocus
    /// <summary>
    /// Cambiar el valor del textbox cuando pierde el foco
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void txt_LostFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if(string.IsNullOrWhiteSpace(txt.Text))
      {
        txt.Text = "0";
      }
    }
    #endregion 
    #endregion
  }
}
