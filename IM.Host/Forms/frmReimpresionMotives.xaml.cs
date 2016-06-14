using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmReimpresionMotives.xaml
  /// </summary>
  public partial class frmReimpresionMotives : Window
  {
    public frmReimpresionMotives()
    {
      InitializeComponent();
    }

    #region btnCancel_Click
    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 09/Junio/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }

    #endregion

    #region Window_Loaded
    /// <summary>
    /// Caraga los motivos de reimpresion a la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 09/Junio/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      // Cargamos los motivos de reimpresion
      CollectionViewSource _dsReimpresionMotives = ((CollectionViewSource)(this.FindResource("dsReimpresionMotives")));
      _dsReimpresionMotives.Source = BRReimpresionMotives.GetReimpresionMotives(1);
    }
    #endregion

    #region btnOk_Click
    /// <summary>
    /// Valida que tenga un motivo seleccionado y cierra el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 09/Junio/2016 Created
    /// </history>
    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      ReimpresionMotive Selected = LstMotives.SelectedItem as ReimpresionMotive;

      if (Selected != null)
      {
        DialogResult = true;
      }
      else
      {
        UIHelper.ShowMessage("Please specify the motive", MessageBoxImage.Information, "Reimpresion Motives");
      }
    }
    #endregion

    #region LstMotives_MouseDoubleClick
    /// <summary>
    /// Cierra el formulario al darle double click a una opcion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void LstMotives_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      btnOk_Click(null, null);
    }
    #endregion

    #region LstMotives_KeyDown
    /// <summary>
    /// Cierra el formulario al darle enter a una opcion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 09/Junio/2016 Created
    /// </history>
    private void LstMotives_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        btnOk_Click(null, null);
      }
    } 
    #endregion

  }
}
