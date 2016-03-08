using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace IM.Base.Helpers
{
  public class KeyboardHelper
  {
    #region estatus de las teclas INS|LockNum|Mayusc
    /// <summary>
    /// Valida el estatus de las teclas y cambia el font del statusBar
    /// </summary>
    /// <param name="statusBar"></param>
    /// <param name="key"></param>
    public static void CkeckKeysPress(System.Windows.Controls.Primitives.StatusBarItem statusBar, Key key)
    {
      var keyPess = Keyboard.GetKeyStates(key).ToString();

      if (keyPess.Contains("Toggled")) 
      {
        statusBar.FontWeight = FontWeights.Bold;
        statusBar.Foreground = Brushes.Black;
      }
      else
      {
        KeyDefault(statusBar);
      }
    }
    /// <summary>
    /// Estilo default del statusBar
    /// </summary>
    /// <param name="statusBar"></param>
    protected static void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }
    #endregion
  }
}
