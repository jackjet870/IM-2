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

    #region Métodos de la forma

    /// <summary>
    /// Configura los controles de la forma
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void frmInvitationBase_Loaded(object sender, RoutedEventArgs e)
    {
      ConfiguracionControles();
      CargarControles();
    }

    /// <summary>
    /// Ejecuta el botón Change
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnChange_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Reschedule
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnReschedule_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Rebook
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnRebook_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Search
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Editar
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Imprimir
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void bntPrint_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Guardar
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Canelar
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Log
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region Métodos privados

    /// <summary>
    /// Oculta o habilita los controles específicos de cada módulo.
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void ConfiguracionControles()
    {
      switch (_invitationType)
      {
        case BusinessRules.Enums.InvitationType.InHouse:
          ConfiguracionControlesInHouse();
          break;
        case BusinessRules.Enums.InvitationType.OutHouse:
          ConfiguracionControlesOutHouse();
          break;
        case BusinessRules.Enums.InvitationType.Host:
          ConfiguracionControlesHost();
          break;
        case BusinessRules.Enums.InvitationType.Animation:
          ConfiguracionControlesAnimation();
          break;
        case BusinessRules.Enums.InvitationType.Regen:
          ConfiguracionControlesRegen();
          break;
      }
    }

    private void CargarControles()
    {
      CargarComboLanguage();
    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo In House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void ConfiguracionControlesInHouse()
    {
      colOutInvitation.Width = new GridLength(0); //Con esta instrucción desaparece la columna
      rowPRContract.Height = new GridLength(0); //Con esta instrucción desaparece el renglon
      rowPRContractTitles.Height = new GridLength(0); //Con esta instrucción desaparece el renglon
      lblFlightNumber.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblSalesRoom2.Visibility = Visibility.Hidden;
      txtSalesRoom2.Visibility = Visibility.Hidden;
    }

    /// <summary>
    /// /// Oculta o habilita los controles necesarios para el módulo Out House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void ConfiguracionControlesOutHouse()
    {
      colReservNum.Width = new GridLength(0);
      colBtnSearch.Width = new GridLength(0);
      colRebookRef.Width = new GridLength(0);
      btnReschedule.Visibility = Visibility.Hidden;
      btnRebook.Visibility = Visibility.Hidden;
      lblFlightNumber.Visibility = Visibility.Visible;
      txtFlightNumber.Visibility = Visibility.Visible;
      lblLocation2.SetValue(Grid.ColumnProperty, 1);
      txtLocation2.SetValue(Grid.ColumnProperty, 1);
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
      colElectronicPurseCreditCard.Width = new GridLength(0);
      rowRoomQuantity.Height = new GridLength(0);
    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo Host
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void ConfiguracionControlesHost()
    {
      rowPRContractTitles.Height = new GridLength(0);
      rowPRContract.Height = new GridLength(0);
      lblSalesRoom.Visibility = Visibility.Hidden;
      txtSalesRoom.Visibility = Visibility.Hidden;
      cmbSalesRoom.Visibility = Visibility.Hidden;
      lblLocation2.Visibility = Visibility.Hidden;
      txtLocation2.Visibility = Visibility.Hidden;
      lblFlightNumber.Visibility = Visibility.Hidden;
      txtFlightNumber.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Visible;
      txtLocation.Visibility = Visibility.Visible;
      cmbLocation.Visibility = Visibility.Visible;
      lblSalesRoom2.Visibility = Visibility.Visible;
      txtSalesRoom2.Visibility = Visibility.Visible;
      grbElectronicPurse.Visibility = Visibility.Hidden;


    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo Animation
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void ConfiguracionControlesAnimation()
    {
      colOutInvitation.Width = new GridLength(0);
      rowPRContractTitles.Height = new GridLength(0);
      rowPRContract.Height = new GridLength(0);
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblBeforeInOut.Visibility = Visibility.Hidden;
      chkBeforeInOut.Visibility = Visibility.Hidden;
      lblSalesRoom.Visibility = Visibility.Visible;
      txtSalesRoom.Visibility = Visibility.Visible;
      cmbSalesRoom.Visibility = Visibility.Visible;
      lblLocation2.Visibility = Visibility.Visible;
      txtLocation2.Visibility = Visibility.Visible;
      colElectronicPurseCreditCard.Width = new GridLength(0);
      
    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo Regen
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void ConfiguracionControlesRegen()
    {
      colOutInvitation.Width = new GridLength(0);
      rowPRContractTitles.Height = new GridLength(0);
      rowPRContract.Height = new GridLength(0);
      lblFlightNumber.Visibility = Visibility.Hidden;
      txtFlightNumber.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblSalesRoom2.Visibility = Visibility.Hidden;
      txtSalesRoom2.Visibility = Visibility.Hidden;
      lblBeforeInOut.Visibility = Visibility.Hidden;
      chkBeforeInOut.Visibility = Visibility.Hidden;
      colElectronicPurseCreditCard.Width = new GridLength(0);
    }

    private void CargarComboLanguage()
    {
      var languages= IM.BusinessRules.BR.BRInvitationBase.GetLanguage();
      cmbLanguage.DisplayMemberPath = "laN";
      cmbLanguage.SelectedValuePath = "laID";
      cmbLanguage.SelectedValue = "ES";
      cmbLanguage.ItemsSource = languages.ToList();
      
    }
    #endregion
  }
}
