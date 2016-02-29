using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmInvitationBase.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 16/02/2016 Created
  /// </history>
  public partial class frmInvitationBase : Window
  {

    private IM.BusinessRules.Enums.InvitationType _invitationType;
    public frmInvitationBase()
    {
      InitializeComponent();
    }

    public frmInvitationBase(IM.BusinessRules.Enums.InvitationType invitationType)
    {
      this._invitationType = invitationType;
      InitializeComponent();
    }

    private void frmInvitationBase_Loaded(object sender, RoutedEventArgs e)
    {
      ConfiguracionControles();
    }

    private void btnChange_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnReschedule_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnRebook_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {

    }

    #region Métodos privados

    private void ConfiguracionControles()
    {
      switch (_invitationType)
      {
        case BusinessRules.Enums.InvitationType.InHouse:
          ConfiguracionControlesInHouse();
          break;
        case BusinessRules.Enums.InvitationType.OutHouse:
          break;
        case BusinessRules.Enums.InvitationType.Host:
          break;
        case BusinessRules.Enums.InvitationType.Animation:
          break;
        case BusinessRules.Enums.InvitationType.Regen:
          break;
      }
    }

    private void ConfiguracionControlesInHouse()
    {
      colOutInvitation.Width = new GridLength(0); //Con esta instrucción desaparece la columna
      rowPRContract.Height = new GridLength(0); //Con esta instrucción desaparece el renglon
      lblFlightNumber.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblSalesRoom2.Visibility = Visibility.Hidden;
      txtSalesRoom2.Visibility = Visibility.Hidden;
    }

    private void ConfiguracionControlesOutHouse()
    {
      colReservNum.Width = new GridLength(0);
      colBtnSearch.Width = new GridLength(0);
      colRebookRef.Width = new GridLength(0);
      btnReschedule.Visibility = Visibility.Hidden;
      btnRebook.Visibility = Visibility.Hidden;
      lblFlightNumber.Visibility = Visibility.Visible;
      txtFlightNumber.Visibility = Visibility.Visible;
      lblLocation2.SetValue(Grid.ColumnProperty, 9);
      txtLocation2.SetValue(Grid.ColumnProperty, 9);
      lblReschedule.Visibility = Visibility.Hidden;
      txtRescheduleDate.Visibility = Visibility.Hidden;
      cmbRescheduleTime.Visibility = Visibility.Hidden;
      lblRefresh.Visibility = Visibility.Hidden;
      chkRefresh.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblSalesRoom2.Visibility = Visibility.Hidden;
      txtSalesRoom2.Visibility = Visibility.Hidden;

    }
      #endregion
    }
}
