﻿using IM.Base.Classes;
using IM.Base.Forms;
using IM.Model.Enums;
using IM.SalesPR.Forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace IM.SalesPR
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    #region Constructores y destructores
    /// <summary>
    /// Constructor de la aplicacion
    /// </summary>
    /// <history>
    ///   [erosado]  22/Mar/2016 Created
    /// </history>
    public App() : base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }
    #endregion

    #region Metodos

    #region OnStartup
    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    protected async  override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash();
      frmLogin frmLogin = new frmLogin(frmSplash, EnumLoginType.Location, changePassword: true, autoSign: true);
      await frmLogin.getAllPlaces();
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        EventManager.RegisterClassHandler(typeof(DataGrid), UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(dataGrid_MouseLeftButtonUp));
        Context.User = frmLogin.UserData;
        frmSalesPR frmMain = new frmSalesPR();
        frmMain.Title = $"{Context.Module} - [{Context.Environment}]";
        frmMain.ShowDialog();
        frmSplash.Close();
      }
    }
    #endregion

    #region App_UnhandledException
    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
    /// <history>
    ///  [erosado]  09/Mar/2016 Created
    /// </history>
    private void App_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      var frm = new frmError(e.Exception);
      frm.ShowDialog();
      if (frm.DialogResult.HasValue && !frm.DialogResult.Value)
      {
        Application.Current.Shutdown();
      }
    }
    #endregion

    #region dataGrid_MouseLeftButtonUp
    /// <summary>
    /// Cambia el cmapo de busqueda del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [erosado] created 21/06/2016
    /// </history>
    private void dataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      DataGrid dgr = sender as DataGrid;
      if (dgr.CurrentColumn != null)
      {
        dgr.Resources["SearchField"] = dgr.CurrentColumn.SortMemberPath;
      }
    }
    #endregion

    #endregion
  }
}
