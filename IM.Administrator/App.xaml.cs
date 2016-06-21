﻿using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Administrator.Forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace IM.Administrator
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    #region Propiedades

    public static UserData User;

    #endregion

    #region Constructores y destructores
    /// <summary>
    /// Constructor de la aplicacion
    /// </summary>
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
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash("Administrator");
      frmLogin frmLogin = new frmLogin(frmSplash, EnumLoginType.Normal, validateRole:true, role:EnumRole.Manager);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        User = frmLogin.UserData;
        if (User.HasRole(EnumRole.Manager))
        {
          EventManager.RegisterClassHandler(typeof(AccessText), AccessKeyManager.AccessKeyPressedEvent, new RoutedEventHandler(keyManager_keyPressed));
          EventManager.RegisterClassHandler(typeof(DataGrid), DataGridCell.MouseLeftButtonUpEvent,new MouseButtonEventHandler(dataGrid_MouseLeftButtonUp));
          frmAdmin frmMain = new frmAdmin();
          frmMain.ShowDialog();
          frmSplash.Close();
        }
        else
        {
          // No tiene permiso para ingresar al modulo
          UIHelper.ShowMessage("User doesn't have access");
        }
      }
    }
    #endregion

    #region App_UnhandledException
    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
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
    
    #region KeyManager
    /// <summary>
    /// Verifica que los accesText sólo se ejecuten en combinacion con ALtLeft
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 24/03/2016
    /// </history>
    void keyManager_keyPressed(object sender, RoutedEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.LeftAlt))
      {
        e.Handled = true;
      }
    }
    #endregion

    private void dataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      DataGrid dgr = sender as DataGrid;
      dgr.Resources["SearchField"] = dgr.CurrentColumn.SortMemberPath;
    }

    #endregion
  }
}
