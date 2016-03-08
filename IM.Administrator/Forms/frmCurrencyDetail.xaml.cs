using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Administrator.Enums;
using IM.BusinessRules.BR;
using IM.Model;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCurrrencyDetail.xaml
  /// </summary>
  public partial class frmCurrencyDetail : Window
  {
    public Currency currency;//objeto para agrear|actualizar
    public ModeOpen mode;//modo en el que se abrirá la ventana add|preview|edit 
    public frmCurrencyDetail()
    {
      InitializeComponent();
    }

    #region event controls
    /// <summary>
    /// Cierra la ventana con el boton escape dependiendo del modo en el que se abrió
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        if (mode == ModeOpen.preview)
        {
          DialogResult = false;
          this.Close();
        }
        else
        {
          MessageBoxResult msgResult = MessageBox.Show("Are you sure close this window?", "Close confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
          if (msgResult == MessageBoxResult.Yes)
          {
            DialogResult = false;
            this.Close();
          }
        }
      }
    }

    /// <summary>
    /// llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      OpenMode();
    }

    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      string sMsj = "";
      int nRes = 0;
      #region validar campos
      if (string.IsNullOrWhiteSpace(txtID.Text))
      {
        sMsj += "Specify the Currency ID. \n";
      }
      if (string.IsNullOrWhiteSpace(txtN.Text))
      {
        sMsj += "Specify the Currency Description.";
      }
      #endregion

      if (sMsj == "")//Validar si hay cmapos vacios
      {
        switch (mode)
        {
          #region insert
          case ModeOpen.add://add
            {
              currency = new Currency { cuID = txtID.Text, cuN = txtN.Text, cuA = chkA.IsChecked.Value };
              nRes = BRCurrencies.saveCurrency(currency, false);

              break;
            }
          #endregion
          #region edit
          case ModeOpen.edit://Edit
            {
              currency = (Currency)DataContext;
              nRes = BRCurrencies.saveCurrency(currency, true);
              break;
            }
            #endregion
        }

        #region respuesta
        switch (nRes)
        {
          case 0:
            {
              MessageBox.Show("Currency not saved", "", MessageBoxButton.OK, MessageBoxImage.Warning);
              break;
            }
          case 1:
            {
              MessageBox.Show("Currency successfully saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              MessageBox.Show("Currency ID already exist please select another one", "", MessageBoxButton.OK, MessageBoxImage.Warning);
              break;
            }
        }
        #endregion
      }
      else
      {
        MessageBox.Show(sMsj);
      }

    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      this.Close();
    }
    #endregion

    #region metods
    /// <summary>
    /// Abre la ventana dependiendo del modo que elija el usuario
    /// Preview|Edit|Add
    /// </summary>
    /// <history>
    /// [emoguel] 08/03/2016 Created
    /// </history>
    protected void OpenMode()
    {
      switch (mode)
      {
        case ModeOpen.preview://show
          {
            btnAccept.Visibility = Visibility.Hidden;
            btnCancel.Content = "OK";
            this.DataContext = currency;
            break;
          }
        case ModeOpen.add://add
          {
            txtID.IsEnabled = true;
            LockControls(true);
            break;
          }
        case ModeOpen.edit://Edit
          {
            txtID.IsEnabled = false;
            LockControls(true);
            this.DataContext = currency;
            break;
          }
      }

    }

    /// <summary>
    /// Bloquea|Desbloquea los botones dependiendo del modo en que se habra
    /// </summary>
    /// <param name="bValue">true para desbloquear| false para bloquear</param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    protected void LockControls(bool blnValue)
    {
      txtN.IsEnabled = blnValue;
      chkA.IsEnabled = blnValue;
    }
    #endregion
  }
}
