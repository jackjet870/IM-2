using System;
using System.Windows;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  ///   Formulario para el cierre de invitaciones
  /// Interaction logic for frmCloseInvitation.xaml
  /// </summary>
  /// <history>
  ///   [vku] 16/Jun/2016 Created
  ///   [vku] 30/Ago/2016 Modified. Elimine el evento dtpkCloseDate_SelectedDateChanged ahora se utiliza dtpkCloseInvit_ValueChanged
  /// </history>
  public partial class frmCloseInvitation : Window
  {
    #region Atributos
    DateTime lastClosedDate;
    DateTime closeInvit = DateTime.Today.AddDays(-1);
    #endregion

    #region Constructor
    public frmCloseInvitation()
    {
      InitializeComponent();
    }
    #endregion

    #region Eventos

    #region Windows_Loaded
    /// <summary>
    ///   Carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 16/Jun/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      GetCloseDate();
      dtpkCloseInvit.Value = closeInvit;  
    }
    #endregion

    #region btnClose_Click
    /// <summary>
    ///   Cierra el show de Invitaciones en la fecha seleccionada por el usuario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 16/Jun/2016 Created
    /// </history>
    private async void btnCloseShows_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateCloseInvitationDate())
      {
        try
        {
          btnCloseShows.Focus();
          if (closeInvit == lastClosedDate)
          {
            Close();
          }
          else
          {
            MessageBoxResult result = UIHelper.ShowMessage("Are you sure you want to close all Invitations until " + closeInvit.ToLongDateString() + " ? " +
              "You won’t be able to modifiy that Invitations anymore.", MessageBoxImage.Question, "Close Invitation");
            if (result == MessageBoxResult.Yes)
            {
              int nRes = 0;
              nRes = await BRConfiguration.SaveCloseDate(closeInvit);
              UIHelper.ShowMessageResult("Close Invitation", nRes);
              if (nRes > 0)
              {
                Close();
              }
            }
          }
        }
        catch (Exception ex)
        {
          UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Close Invitation");
        }
      }
    }
    #endregion

    #region dtpkCloseInvit_ValueChanged
    /// <summary>
    ///   Guarda la fecha seleccionada para el cierre de invitaciones
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 30/Ago/2016 Created
    /// </history>
    private void dtpkCloseInvit_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      closeInvit = dtpkCloseInvit.Value.Value;
    }
    #endregion

    #endregion

    #region Metodos

    #region GetCloseDate
    /// <summary>
    ///   Obtiene la ultima fecha
    /// </summary>
    /// <history>
    ///   [vku] 16/Jun/2016 Created
    ///   [erosado] 05/08/2016  Modified. Se agregó async.
    /// </history>
    protected async void GetCloseDate()
    {
      try
      {
        var result = await BRConfiguration.GetCloseDate();
        lastClosedDate = (DateTime)result;

        dtpkLastClose.Value = lastClosedDate;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Configuration");
      }
    }
    #endregion

    #region ValidateCloseInvitationDate
    /// <summary>
    ///   Valida que la fecha de cierre no sea mayor a la fecha de hoy
    /// </summary>
    /// <returns></returns>
    /// <history>
    ///   [vku] 11/Ago/2016 Created
    /// </history>
    protected bool ValidateCloseInvitationDate()
    {
      bool blnValid = true;
      if (dtpkCloseInvit.Value.Value > BRHelpers.GetServerDate())
      {
        blnValid = false;
        UIHelper.ShowMessage("Close date can't be greater than today");
      }
      return blnValid;
    }
    #endregion

    #endregion
  }
}
