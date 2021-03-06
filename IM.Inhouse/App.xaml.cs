﻿using IM.Base.Classes;
using IM.Base.Forms;
using IM.Inhouse.Forms;
using IM.Model.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace IM.Inhouse
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
    ///   [wtorres]  09/Mar/2016 Created
    /// </history>
    public App() : base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }

    #endregion Constructores y destructores

    #region Metodos

    #region OnStartup

    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    /// <history>
    /// [jorcanche] 11/04/2016  created
    ///   [erosado] 01/06/2016  Modified. Se agrego async
    /// </history>
    protected override async void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash();

      frmLogin frmLogin = new frmLogin(frmSplash, EnumLoginType.Location, EnumProgram.Inhouse, true,
        changePassword: true, autoSign: true, permission: EnumPermission.Register,
        permissionLevel: EnumPermisionLevel.ReadOnly);

      await frmLogin.getAllPlaces();

      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        EventManager.RegisterClassHandler(typeof(DataGrid), UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(dataGrid_MouseLeftButtonUp));

        Context.User = frmLogin.UserData;
        frmInhouse frmMain = new frmInhouse();
        frmMain.Title = $"{Context.Module} - [{Context.Environment}]";
        frmMain.ShowDialog();
        frmSplash.Close();
      }
    }

    #endregion OnStartup

    #region App_UnhandledException

    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
    /// <history>
    ///   [wtorres]  09/Mar/2016 Created
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

    #endregion App_UnhandledException

    #region dataGrid_MouseLeftButtonUp

    /// <summary>
    /// Cambia el cmapo de busqueda del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [jorcanche] created 21/06/2016
    /// </history>
    private void dataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      DataGrid dgr = sender as DataGrid;
      if (dgr.CurrentColumn != null)
      {
        dgr.Resources["SearchField"] = dgr.CurrentColumn.SortMemberPath;
      }
    }

    #endregion dataGrid_MouseLeftButtonUp

    #endregion Metodos
  }
}