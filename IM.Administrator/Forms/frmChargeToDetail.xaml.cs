﻿using System;
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
    public EnumMode mode;
    public ChargeTo chargeTo=new ChargeTo();//Objeto a guardar|Actualizar
    public ChargeTo oldChargeTo = new ChargeTo();//Objeto con los datos iniciales
    public frmChargeToDetail()
    {
      InitializeComponent();
    }
    #region metodos
    #region LoadChargeType
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
    #endregion

    #region OpenMode
    /// <summary>
    /// Abre la ventana dependiendo del modo que elija el usuario
    /// Preview|Edit|Add
    /// </summary>
    /// <history>
    /// [emoguel] 02/03/2016 Created
    /// </history>
    protected void OpenMode()
    {
      DataContext = chargeTo;
      switch (mode)
      {
        case EnumMode.preview://show
          {
            btnAccept.Visibility = Visibility.Hidden;            
            break;
          }
        case EnumMode.add://add
          {
            txtID.IsEnabled = true;
            LockControls(true);

            break;
          }
        case EnumMode.edit://Edit
          {
            txtID.IsEnabled = false;
            LockControls(true);
            break;
          }
      }

    }

    #endregion
    #region LockControls
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
    #endregion

    #region eventos de los controles
    #region Accept
    /// <summary>
    /// Guarda o actuliza un registro en la BD dependiendo del modo en que se haya abierto la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if(ObjectHelper.IsEquals(chargeTo,oldChargeTo) && mode!=EnumMode.add)
      {
        Close();
      }
      else
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
            sMsj += "The price must be higher than 0 and must be smaller than 255. \n";
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

          nRes = BRChargeTos.SaveChargeTo(chargeTo, (mode == EnumMode.edit));

          UIHelper.ShowMessageResult("Charge To", nRes);
          if(nRes==1)
          {
            DialogResult = true;
            Close();
          }
        }
        else
        {
          UIHelper.ShowMessage(sMsj);
        }
      }

    }

    #endregion
    
    #region LoadFrom
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(chargeTo, oldChargeTo);
      LoadChargeType();
      OpenMode();

      tlpCalc.Text = "A - If the guest has tour then the charge is equal to the total of gifts less the maximum amount"
        + "authorized else the charge is equal to the total of gifts. The maximum amount authorized is based on the Lead Source. \n \n"
        + "B - The charge is equal to the total of gifts. \n \n "
        + "C - The charge is equal to the total of gifts less the maximum amount authorized. The maximum amount authorized is based on the Guest Status. \n \n"
        + "Z - No charge.";
    }

    #endregion
    #region PreviewKeyInput
    /// <summary>
    /// Valida que unicamente se inserten numeros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtPri_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }

    #endregion
    #region WindowKeyDown
    /// <summary>
    /// Cierra la ventana detalle con la tecla escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null,null);
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
      if(mode!=EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(chargeTo, oldChargeTo))
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
    #endregion
  }
}
