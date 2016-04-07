﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Administrator.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmMembershipTypeDetail.xaml
  /// </summary>
  public partial class frmMembershipTypeDetail : Window
  {
    #region variables
    public MembershipType membershipType = new MembershipType();//Objeto a guardar
    public MembershipType oldMembershipType = new MembershipType();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    public int nStatus = -1;//Estatus para el modo Search
    #endregion
    public frmMembershipTypeDetail()
    {
      InitializeComponent();
    }

    #region Methods Forms
    #region Window loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(membershipType, oldMembershipType);
      DataContext = membershipType;
      LoadMemberGroups();
      txtID.IsEnabled = (enumMode != EnumMode.edit);
      #region Mode Search
      if (enumMode == EnumMode.search)
      {
        chkA.Visibility = Visibility.Collapsed;
        cmbSta.Visibility = Visibility.Visible;
        lblFrom.Visibility = Visibility.Collapsed;
        lblLevel.Visibility = Visibility.Collapsed;
        lblTo.Visibility = Visibility.Collapsed;
        txtLevel.Visibility = Visibility.Collapsed;
        txtTo.Visibility = Visibility.Collapsed;
        txtFrom.Visibility = Visibility.Collapsed;
        lblStatus.Visibility = Visibility.Visible;
        LoadStatus();
        cmbSta.SelectedValue = nStatus;
      } 
      #endregion
      if(enumMode!=EnumMode.preview)
      {
        btnAccept.Visibility = Visibility.Visible;
      }
    }
    #endregion

    #region Window KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        btnCancel.Focus();
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Guarda un objeto en el catalogo membershipType
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      if (enumMode != EnumMode.search)
      {
        if (ObjectHelper.IsEquals(membershipType, oldMembershipType) && !Validation.GetHasError(txtLevel) && enumMode!=EnumMode.add)
        {
          Close();
        }
        else
        {
          string strMsj = ValidateHelper.ValidateForm(this, "Membership Type");
          #region ValidateLevel
          if(!string.IsNullOrWhiteSpace(txtLevel.Text.Trim()))
          {
            int nRes = Convert.ToInt32(txtLevel.Text.Trim());
            if(nRes>255 || nRes<1)
            {
              strMsj +=((strMsj=="")?"":" \n")+ "Level is out of range. Allowed values are 1 to 255.";
            }
          }
          #endregion
          if (strMsj == "")
          {
            int nRes = BRMemberShipTypes.SaveMemberShipType(membershipType, (enumMode == EnumMode.edit));
            UIHelper.ShowMessageResult("Membership Type", nRes);
            if (nRes == 1)
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
      else
      {
        nStatus = Convert.ToInt32(cmbSta.SelectedValue);
        DialogResult = true;
        Close();
      }

    } 
    #endregion

    #region Cancel
    /// <summary>
    /// cierra la ventana verificando que no haya cambios pendientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (enumMode != EnumMode.preview)
      {
        if (!ObjectHelper.IsEquals(membershipType, oldMembershipType) && enumMode!=EnumMode.search)
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
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

    #region LostFocus
    /// <summary>
    /// SI el campo está vacio por default le pone 0
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void txt_LostFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (string.IsNullOrWhiteSpace(txt.Text))
      {
        txt.Text = "0";
      }
    }
    #endregion

    #region Number Text Input
    /// <summary>
    /// verifica que unicamente acepte numero y 1 punto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void txt_NumberTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (e.Text == "." && !txt.Text.Trim().Contains("."))
      {
        e.Handled = false;
      }
      else
      {
        e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
      }
    }
    #endregion

    #region txt_LevelNumberImput
    /// <summary>
    /// Valida que solo se puedan escribir Números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void txt_LevelTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      if (txt.Text.Trim().Count() == 2 && ValidateHelper.OnlyNumbers(e.Text))
      {
        int nLevel = Convert.ToInt32(txt.Text.Trim()+e.Text);
        if(nLevel>255)
        {
          txt.Text = "255";
        }
      }
      else
      {
        e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
      }
    } 
    #endregion

    #region Got Focus
    /// <summary>
    /// Cambia de formato currency a standar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 04/04/2016
    /// </history>
    private void txt_GotFocus(object sender, RoutedEventArgs e)
    {
      TextBox txt = (TextBox)sender;
      txt.Text = ConvertHelper.DoubleCurrencyToStandar(txt.Text.Trim());
    }
    #endregion
    #endregion

    #region Methods
    #region LoadMemberGroups
    /// <summary>
    /// Llena el combobox de Groups
    /// </summary>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void LoadMemberGroups()
    {
      List<MembershipGroup> lstMembershipGroup = BRMembershipGroups.GetMembershipGroup();
      if (enumMode == EnumMode.search)
      {
        lstMembershipGroup.Insert(0, new MembershipGroup { mgID = "", mgN = "" });
      }
      cmbMtGroup.ItemsSource = lstMembershipGroup;
    }
    #endregion

    #region LoadStatus
    /// <summary>
    /// Llena la lista de estatus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    protected void LoadStatus()
    {
      List<object> lstStatus = new List<object>();
      lstStatus.Add(new { sName = "All", sValue = -1 });
      lstStatus.Add(new { sName = "Inactive", sValue = 0 });
      lstStatus.Add(new { sName = "Active", sValue = 1 });
      cmbSta.ItemsSource = lstStatus;
    }

    #endregion
    #endregion
  }
}
