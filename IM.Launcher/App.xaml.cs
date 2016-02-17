using System;
using System.Windows;

namespace IM.Launcher
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
      ///   [wtorres]  11/02/2015 Created
      /// </history>
      public App()
        : base()
      {
        this.Dispatcher.UnhandledException += App_UnhandledException;
      }

      #endregion

      #region Metodos privados

      /// <summary>
      /// Despliega los mensajes de error de la aplicacion
      /// </summary>
      /// <history>
      ///   [wtorres]  11/02/2015 Created
      /// </history>
      private void App_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
      {
        e.Handled = true;
        var errorMessage = new ErrorMessage(e.Exception.Message);
        errorMessage.ShowDialog();
        if(errorMessage.DialogResult.HasValue && !errorMessage.DialogResult.Value)
        {
          Application.Current.Shutdown();
        }
      }

      #endregion
    }



}
