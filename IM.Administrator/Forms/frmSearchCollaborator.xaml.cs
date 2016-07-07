using System;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.Model;
using IM.BusinessRules.BRAsistencia;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSearchCollaborator.xaml
  /// </summary>
  public partial class frmSearchCollaborator : Window
  {
    public string idCollaborator = "";
    public frmSearchCollaborator()
    {
      InitializeComponent();
      DataContext = new Collaborator();
    }

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/06/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window keyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 20/06/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            break;
          }
        case Key.Escape:
          {
            Close();
            break;
          }
      }
    }
    #endregion

    #region SearchCollaborator
    /// <summary>
    /// Busca los collaborator con los parametros asignados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 21/06/2016
    /// </history>
    private async void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnSearch.Focus();
        status.Visibility = Visibility.Visible;
        Collaborator collaborator = (Collaborator)DataContext;
        dgrCollaborator.ItemsSource = await BRCollaborator.GetCollaborators(collaborator);
        StatusBarReg.Content=dgrCollaborator.Items.Count+" Collaborators.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Personnel");
      }
    }
    #endregion

    #region Accept
    /// <summary>
    /// obtiene el numero de collaborator y cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] creayed 22/06/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      if(dgrCollaborator.SelectedItems.Count>0)
      {
        Collaborator collaborator = (Collaborator)dgrCollaborator.SelectedItem;
        idCollaborator = collaborator.EmpID;
        DialogResult = true;
        Close();
      }
      else
      {
        UIHelper.ShowMessage("Select a collaborator");
      }
    }
    #endregion

    #region DobleClic Grid
    /// <summary>
    /// Obtiene el número de collaborador y cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/07/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      if (dgrCollaborator.SelectedItems.Count > 0)
      {
        Collaborator collaborator = (Collaborator)dgrCollaborator.SelectedItem;
        idCollaborator = collaborator.EmpID;
        DialogResult = true;
        Close();
      }
      else
      {
        UIHelper.ShowMessage("Select a collaborator");
      }
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] modified 02/07/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      DataContext = new Collaborator();
    } 
    #endregion
  }
}
