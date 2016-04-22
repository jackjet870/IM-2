using System;
using System.Windows.Input;

namespace IM.Base.Helpers
{
 
  public class ExecuteCommandHelper : ICommand
  {
    Action<object> _executeDelegate;
    /// <summary>
    /// Captura un comando del teclado y ejecuta una accion definida.
    /// Ejemplo Ctrl + f5
    /// </summary>
    /// <param name="executeDelegate">Action<object></param>
    /// <history>
    /// [erosado] 18/04/2016  Created.
    /// </history>
    public ExecuteCommandHelper(Action<object> executeDelegate)
    {
      _executeDelegate = executeDelegate;
    }
    public void Execute(object parameter)
    {
      _executeDelegate(parameter);
    }
    public bool CanExecute(object parameter)
    {
      return true;
    }

     public event EventHandler CanExecuteChanged
    {
      add {}
      remove { }
    }


  }
}
