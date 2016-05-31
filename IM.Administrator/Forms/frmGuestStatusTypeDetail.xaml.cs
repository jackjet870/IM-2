using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using System;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmGuestStatusTypeDetail.xaml
  /// </summary>
  public partial class frmGuestStatusTypeDetail : Window
  {
    public GuestStatusType guestStaTypOld = new GuestStatusType();//Objeto con los antigüos valores
    public GuestStatusType guestStaTyp = new GuestStatusType();//Objeto a guardar en la BD
    public EnumMode enumMode;//Modo en el que se abrirá la ventana
    public frmGuestStatusTypeDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Accept
    /// <summary>
    /// Agrega| Actualiza un registro en el catalogo GuestStatus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/03/2016
    /// [emoguel] modified 30/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (ObjectHelper.IsEquals(guestStaTyp, guestStaTypOld) && !Validation.GetHasError(txtgsAgeEnd) && !Validation.GetHasError(txtgsAgeStart) && enumMode != EnumMode.add)//Verificamos si se realizó algun cambio
        {
          Close();
        }
        else
        {
          string strMsj = "";
          int nRes = 0;
          #region validar campos
          if (string.IsNullOrWhiteSpace(guestStaTyp.gsID))//ID
          {
            strMsj += "Specify the Guest status type ID. \n";
          }
          if (string.IsNullOrWhiteSpace(guestStaTyp.gsN))
          {
            strMsj += "Specify the Guest status type description. \n";
          }
          #region validar star Age y end age
          if (Validation.GetHasError(txtgsAgeStart))//Validamos que no pueda ser mayor a 255        
          {
            strMsj += "Start Age can not be greater than 255. \n";
          }
          if (Validation.GetHasError(txtgsAgeEnd))
          {
            strMsj += "End Age can not be greater than 255. \n";
          }
          if (!Validation.GetHasError(txtgsAgeStart) && !Validation.GetHasError(txtgsAgeEnd))
          {
            if (guestStaTyp.gsAgeStart > guestStaTyp.gsAgeEnd)//Validamos que start Edge no se mayor que end Age
            {
              strMsj += "Start age can not be greater than End age. \n";
            }
          }
          #endregion
          #endregion

          if (strMsj == "")
          {
            nRes = await BREntities.OperationEntity(guestStaTyp, enumMode);
            UIHelper.ShowMessageResult("Guest Status Type", nRes);
            if (nRes > 0)
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
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Guest Status Type");
      }
    }
    #endregion

    #region Window Loaded
    /// <summary>
    /// Carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(guestStaTyp, guestStaTypOld);
      DataContext = guestStaTyp;
      if(enumMode!=EnumMode.preview)
      {
        txtgsID.IsEnabled = (enumMode==EnumMode.add);
        txtgsN.IsEnabled = true;
        txtgsAgeStart.IsEnabled = true;
        txtgsAgeEnd.IsEnabled = true;
        txtgsMaxAuthGifts.IsEnabled = true;
        txtgsMaxQtyTours.IsEnabled = true;
        chkA.IsEnabled = true;
        chkAllTour.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
        UIHelper.SetUpControls(guestStaTyp, this);
      }
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Ejecuta el boton cancel con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 29/03/2016
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

    #region Cancel
    /// <summary>
    /// Cierra la ventana verificando que no tenga cambios pendientes
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
        if (!ObjectHelper.IsEquals(guestStaTyp, guestStaTypOld))//Verificar que no haya cambios pendientes
        {
          MessageBoxResult result = UIHelper.ShowMessage("If you close the window, the changes will be lost.", MessageBoxImage.Question, "Closing window");
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
    #endregion

  }
}
