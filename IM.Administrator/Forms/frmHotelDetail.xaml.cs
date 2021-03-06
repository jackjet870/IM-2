﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using System.Linq;
using IM.Model.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmHotelDetail.xaml
  /// </summary>
  public partial class frmHotelDetail : Window
  {
    #region variables
    public Hotel hotel = new Hotel();//Objeto a guardar|Actualizar|Filtrar
    public Hotel oldHotel = new Hotel();//Objeto con los datos iniciales
    public int nStatus = -1;//Para cuando se abra en modo search
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    private bool _isClosing = false;
    #endregion
    public frmHotelDetail()
    {
      InitializeComponent();
    }

    #region Methods Form

    #region Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ObjectHelper.CopyProperties(hotel, oldHotel);
      DataContext = hotel;
      
      LoadAreas();
      LoadGroups();
      if (enumMode != EnumMode.ReadOnly)
      {
        btnAccept.Visibility = Visibility.Visible;
        cmbHotelAr.IsEnabled = true;
        cmbHotelGr.IsEnabled = true;
        chkA.IsEnabled = true;
        if (enumMode != EnumMode.Edit)
        {
          txthoID.IsEnabled = true;
        }
        #region Modo Busqueda
        if (enumMode == EnumMode.Search)
        {
          ComboBoxHelper.LoadComboDefault(cmbStatus);
          cmbStatus.SelectedValue = nStatus;
          chkA.Visibility = Visibility.Collapsed;
          lblStatus.Visibility = Visibility.Visible;
          cmbStatus.Visibility = Visibility.Visible;
        }
        #endregion
        UIHelper.SetUpControls(hotel, this);
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// Actualiza|agrega un registro en Hotels
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// [erosado] 19/05/2016  Modified. Se agregó Asincronía
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        if (enumMode != EnumMode.Search)
        {
          if (ObjectHelper.IsEquals(hotel, oldHotel) && enumMode != EnumMode.Add)//si no modifico nada
          {
            _isClosing = true;
            Close();
          }
          else//si hubo cambios
          {
            int nRes = 0;
            string strMsj = ValidateHelper.ValidateForm(this, "Hotel");

            if (strMsj == "")//Guardar
            {
              nRes = await BREntities.OperationEntity(hotel, enumMode);
              UIHelper.ShowMessageResult("Hotel", nRes);
              if (nRes > 0)
              {
                var r = await BRHotels.GetHotels(hotel, blnInclude: true);
                hotel = r.FirstOrDefault();
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
        else
        {
          _isClosing = true;
          nStatus = Convert.ToInt32(cmbStatus.SelectedValue);
          DialogResult = true;
          Close();
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_isClosing)
      {
        btnCancel.Focus();
        if (enumMode != EnumMode.ReadOnly && enumMode != EnumMode.Search)
        {
          if (!ObjectHelper.IsEquals(hotel, oldHotel))
          {
            MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
            if (result == MessageBoxResult.No)
            {
              e.Cancel = true;
            }
          }
        }
      }
    }
    #endregion

    #endregion

    #region Methods
    #region LoadGroups
    /// <summary>
    /// Llena el combo de Hotel Group
    /// </summary>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// </history>
    private async void LoadGroups()
    {
      try
      {
        List<HotelGroup> lstHoGroup =await BRHotelGroups.GetHotelGroups(nStatus: 1);
        if (enumMode == EnumMode.Search)
        {
          lstHoGroup.Insert(0, new HotelGroup { hgID = "", hgN = "ALL" });
        }
        cmbHotelGr.ItemsSource = lstHoGroup;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadAreas
    /// <summary>
    /// Llena el combo de ares
    /// </summary>
    /// <history>
    /// [emoguel] created 30/03/2016
    /// [emoguel] se volvió async
    /// </history>
    private async void LoadAreas()
    {
      try
      {
        List<Area> lstAreas = await BRAreas.GetAreas(nStatus: 1);
        if (enumMode == EnumMode.Search)
        {
          lstAreas.Insert(0, new Area { arID = "", arN = "ALL" });
        }

        cmbHotelAr.ItemsSource = lstAreas;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #endregion
  }
}
