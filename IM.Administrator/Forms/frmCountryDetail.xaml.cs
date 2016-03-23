using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Administrator.Enums;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCountryDetail.xaml
  /// </summary>
  public partial class frmCountryDetail : Window
  {
    public ModeOpen mode;//modo en el que se abrirá la ventana
    public Country country = new Country();//objeto con los datos del formulario

    public frmCountryDetail()
    {
      InitializeComponent();
    }

    #region Eventos del formulario
    #region Window Loaded
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      openMode();
      loadUnavailableMotives();
      LoadLanguages();
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana al presionar el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        DialogResult = false;
        Close();
      }
    }
    #endregion

    #region Boton aceptar
    /// <summary>
    /// Guarda|Actualiza un registro dependiendo del modo en que se abrió
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      int nRes = 0;
      string sMsj = "";
      #region validar campos
      if (string.IsNullOrWhiteSpace(txtID.Text))
      {
        sMsj += "Specify the Country ID. \n";
      }

      if (string.IsNullOrWhiteSpace(txtN.Text))
      {
        sMsj += "Specify the Contry description. \n";
      }

      if (cmbUnvMot.SelectedIndex < 0)
      {
        sMsj += "Specify the Country unavailable motive . \n";
      }

      if (cmbLang.SelectedIndex < 0)
      {
        sMsj += "Specify the Country language . \n";
      }

      #endregion

      if(sMsj=="")
      {
        nRes = BRCountries.SaveCountry(country, (mode==ModeOpen.edit));        

        #region respuesta
        switch (nRes)//Se valida la respuesta de la operacion
        {
          case 0:
            {
              UIHelper.ShowMessage("Country not saved");
              break;
            }
          case 1:
            {
              UIHelper.ShowMessage("Country successfully saved");
              DialogResult = true;
              this.Close();
              break;
            }
          case 2:
            {
              UIHelper.ShowMessage("Country ID already exist please select another one");
              break;
            }
        }
        #endregion
      }
      else
      {
        UIHelper.ShowMessage(sMsj.TrimEnd('\n'));
      }
    }
    #endregion
    
    #endregion

    #region Métodos
    #region Load Unavailable motive
    /// <summary>
    /// Carga el combobox de unavailable motives
    /// </summary>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// </history>
    private void loadUnavailableMotives()
    {
      List<UnavailableMotive> lstUnavailableMotives = BRUnavailableMotives.GetUnavailableMotives();
      cmbUnvMot.ItemsSource = lstUnavailableMotives;
    }
    #endregion

    #region Load Languages
    /// <summary>
    /// Llena el combobox de languages
    /// </summary>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// </history>
    private void LoadLanguages()
    {
      List<LanguageShort> lstLanguages = BRLanguages.GetLanguages(0);
      cmbLang.ItemsSource = lstLanguages;
    }
    #endregion

    #region Open Mode
    /// <summary>
    /// Varifica en que modo se abrió la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// </history>
    private void openMode()
    {
      DataContext = country;
      switch(mode)
      {
        case ModeOpen.add:
          {
            txtID.IsEnabled = true;
            cmbUnvMot.SelectedIndex = -1;
            lockControls(true);
            break;
          }
        case ModeOpen.edit:
          {
            txtID.IsEnabled = false;
            lockControls(true);
            break;
          }

        case ModeOpen.preview:
          {
            btnAccept.Visibility = Visibility.Hidden;
            break;
          }
      }
    }
    #endregion

    #region Lock Controls
    /// <summary>
    /// BLoquea| Desbloquea los controles
    /// </summary>
    /// <param name="blnValue"></param>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// </history>
    private void lockControls(bool blnValue)
    {
      txtN.IsEnabled = blnValue;
      txtNat.IsEnabled = blnValue;
      chkA.IsEnabled = blnValue;
      chkSil.IsEnabled = blnValue;
      cmbLang.IsEnabled = blnValue;
      cmbUnvMot.IsEnabled = blnValue;
    }
    #endregion

    #endregion
    
  }
}
