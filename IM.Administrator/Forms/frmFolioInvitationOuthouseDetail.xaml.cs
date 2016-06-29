using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;

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
    private bool _isClosing = false;
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
      txtfiSerie.PreviewTextInput += TextBoxHelper.LetterTextInput;
      if (enumMode != EnumMode.preview)
      {
        txtfiSerie.IsEnabled = true;
        txtfiFrom.IsEnabled = true;
        txtfiTo.IsEnabled = true;
        chkA.IsEnabled = true;
        UIHelper.SetUpControls(folioInvOut, this);
        btnAccept.Visibility = Visibility.Visible;
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
      btnCancel.Focus();
      if(enumMode!=EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(folioInvOut, oldFolioInvOut))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!_isClosing) { _isClosing = true; Close(); }
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
        _isClosing = true;
        Close();
      }
      else
      {
        int nRes = 0;
        string strMsj = "";
        #region Validar El rango del folio      
        if (string.IsNullOrWhiteSpace(txtfiSerie.Text))
        {
          strMsj += "Specify the serie. \n";
        }

        if (folioInvOut.fiFrom == 0)
        {
          txtfiFrom.Text = "0";
          strMsj += "Start number can not be 0.";
        }
        else
        {
          if (folioInvOut.fiTo < folioInvOut.fiFrom)
          {
            if (string.IsNullOrWhiteSpace(txtfiTo.Text))
            {
              txtfiTo.Text = txtfiFrom.Text;
            }
            strMsj += "Start number can not be greater than End Number.";
          }
        }
        #endregion

        if (strMsj == "")
        {
          nRes = BRFoliosInvitationsOuthouse.SaveFolioInvittionsOutside(folioInvOut, (enumMode == EnumMode.edit));
          UIHelper.ShowMessageResult("Folio Invitation Outhouse", nRes);
          if(nRes==1)
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
      }
    }
    #endregion

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/06/2016
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

    #endregion
  }
}
