using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    public FolioInvitationOuthouse oldFolioInvOut = new FolioInvitationOuthouse();//Objeto con los datos iniciales
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
        btnCancel.Focus();
        btnCancel_Click(null, null);
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
      ObjectHelper.CopyProperties(folioInvOut, oldFolioInvOut);
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
        if (!ObjectHelper.IsEquals(folioInvOut, oldFolioInvOut))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Warning, "Closing window", MessageBoxButton.OKCancel);
          if (result == MessageBoxResult.OK)
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
    /// Agrega o actualiza un registro dependiendo del modo de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(folioInvOut,oldFolioInvOut)&& enumMode!=EnumMode.add)
      {
        Close();
      }
      else
      {
        int nRes = 0;
        string strMsj = "";
        #region Validar El rango del folio      
        if (string.IsNullOrWhiteSpace(txtSerie.Text))
        {
          strMsj += "Specify the serie. \n";
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
          UIHelper.ShowMessageResult("Folio Invitation Outhouse", nRes,blnIsRange:true);
          if(nRes==1)
          {
            DialogResult = true;
            Close();
          }          
        }
        else
        {
          UIHelper.ShowMessage(strMsj);
        }
      }
    }
    #endregion
    #endregion

    #region Methods


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
