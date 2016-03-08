using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Administrator.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmChargeToDetail.xaml
  /// </summary>
  public partial class frmChargeToDetail : Window
  {
    public ModeOpen mode;
    public ChargeTo chargeTo;
    public frmChargeToDetail()
    {
      InitializeComponent();
    }
    #region metodos
    /// <summary>
    /// Llena el combobox de Charge Type
    /// </summary>
    /// <history>
    /// [Emoguel] cretaed 02/03/2016
    /// </history>
    protected void LoadChargeType()
    {
      List<ChargeCalculationType> lstChargeType = BRChargeCalculationTypes.GetChargeCalculationTypes(new ChargeCalculationType(), 1);
      cmbCalTyp.ItemsSource = lstChargeType;
    }

    /// <summary>
    /// Abre la ventana dependiendo del modo que elija el usuario
    /// Preview|Edit|Add
    /// </summary>
    /// <history>
    /// [emoguel] 02/03/2016 Created
    /// </history>
    protected void OpenMode()
    {
      switch (mode)
      {
        case ModeOpen.preview://show
          {
            btnAccept.Visibility = Visibility.Hidden;
            btnCancel.Content = "OK";
            this.DataContext = chargeTo;
            break;
          }
        case ModeOpen.add://add
          {
            txtID.IsEnabled = true;
            LockControls(true);

            break;
          }
        case ModeOpen.edit://Edit
          {
            txtID.IsEnabled = false;
            LockControls(true);
            this.DataContext = chargeTo;
            break;
          }
      }

    }

    /// <summary>
    /// Bloquea controles dependiendo del modo en que se visualize la ventana
    /// </summary>
    /// <param name="blnValue"></param>
    protected void LockControls(bool blnValue)
    {
      txtPri.IsEnabled = blnValue;      
      cmbCalTyp.IsEnabled = blnValue;
      chkCxC.IsEnabled = blnValue;
    }
    #endregion

    #region eventos de los controles
    /// <summary>
    /// Guarda o actuliza un registro en la BD dependiendo del modo en que se haya abierto la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      string sMsj = "";
      int nRes = 0;
      #region validar campos
      if (string.IsNullOrWhiteSpace(txtID.Text))//ID
      {
        sMsj += "Specify the Area ID. \n";
      }
      #region validar price
      if (string.IsNullOrWhiteSpace(txtPri.Text))//Se valida que se haya llenado el campo
      {
        sMsj += "Specify the Charge To Price. \n";
      }
      else
      {
        int nPrice = Convert.ToInt32(txtPri.Text);
        if (!(nPrice > 0 && nPrice < 256))//Se valida que el número esté en el rango de tipo byte
        {
          txtPri.Text = ((nPrice == 0) ? "" : nPrice.ToString());
          sMsj += "The price must be higher than 0 and must be smaller than 255 \n";
        }
      }
      #endregion
      if (cmbCalTyp.SelectedIndex < 0)
      {
        sMsj += "Specify the Calculation Type.";
      }
      #endregion
      if (sMsj == "")
      {
        switch (mode)
        {
          #region add
          case ModeOpen.add://add
            {

              chargeTo = new ChargeTo { ctID = txtID.Text, ctPrice=Convert.ToByte(txtPri.Text),ctCalcType=cmbCalTyp.SelectedValue.ToString(),ctIsCxC=chkCxC.IsChecked.Value };
              nRes = BRChargeTos.SaveChargeTo(chargeTo,false);               
              break;
            }
          #endregion
          #region Edit
          case ModeOpen.edit://edit
            {
              chargeTo = (ChargeTo)this.DataContext;
              nRes = BRChargeTos.SaveChargeTo(chargeTo, true);                
              break;
            }
            #endregion
        }

        #region validacion respuesta
        switch (nRes)
        {
          case 0:
            {
              MessageBox.Show("Charge To not saved", "", MessageBoxButton.OK, MessageBoxImage.Warning);
              break;
            }
          case 1:
            {
              MessageBox.Show("Charge To successfully saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              MessageBox.Show("Charge To ID already exist please select another one", "", MessageBoxButton.OK, MessageBoxImage.Warning);
              break;
            }
        }
        #endregion
      }
      else
      {
        MessageBox.Show(sMsj);
      }
      
    }
  
    /// <summary>
    /// Cierra la ventana detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      this.Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadChargeType();
      OpenMode();

      tlpCalc.Text = "A - If the guest has tour then the charge is equal to the total of gifts less the maximum amount"
        + "authorized else the charge is equal to the total of gifts. The maximum amount authorized is based on the Lead Source. \n \n"
        + "B - The charge is equal to the total of gifts. \n \n "
        + "C - The charge is equal to the total of gifts less the maximum amount authorized. The maximum amount authorized is based on the Guest Status. \n \n"
        + "Z - No charge.";
    }

    /// <summary>
    /// Valida que unicamente se inserten numeros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtPri_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }

    /// <summary>
    /// Cierra la ventana detalle con la tecla escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key==Key.Escape)
      {
        if(mode== ModeOpen.preview)
        {
          this.Close();
        }
        else
        {
          MessageBoxResult msgResult = MessageBox.Show("Are you sure close this window?","Close confirmation",MessageBoxButton.YesNo,MessageBoxImage.Question);
          if(msgResult==MessageBoxResult.Yes)
          {
            DialogResult = false;
            this.Close();
          }
        }
      }
    }
    #endregion
  }
}
