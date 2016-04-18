using System;
using System.Windows.Input;

namespace IM.Base.Helpers
{
  /// <summary>
  /// Se encarga de ejecutar un comando dentro de la interfaz o dentro de algun control en especifico.
  /// Ejemplo Ctrl + f5 
  /// </summary>
  /// <history>
  /// [erosado] 18/04/2016  Created.
  /// </history>
  public class ExecuteCommandHelper : ICommand
  {
    Action<object> _executeDelegate;
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
    public event EventHandler CanExecuteChanged;
  }
}
