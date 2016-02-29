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
using IM.Model;
using IM.Model.Entities;
using IM.BusinessRules.BR;
using IM.BusinessRules.Entities;
using IM.BusinessRules.Login;

namespace IM.InventoryMoves
{
  /// <summary>
  /// Interaction logic for frmLoginWH.xaml
  /// </summary>
  public partial class frmLoginWH : Window
  {
    private const string encryptCode = "1O3r5i7g9o8s";
    private List<GetWarehousesByUser> _lstWhsByUsr = new List<GetWarehousesByUser>();
    private UserData _unlObj = new UserData();
    public frmLoginWH()
    {
      InitializeComponent();
    }

    #region Métodos Formulario
    /// <summary>
    /// Método para validar al usuario, obtiene permisos y roles.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 24/Feb/2016 Created
    /// </history>
    private void btnAceptar_Click(object sender, RoutedEventArgs e)
    {
      //_unlObj = BRPersonnel.login(, txtUser.Text, cmbLocation.SelectedValue.ToString());
      //if (Base.Helpers.EncryptHelper.Encrypt(txtPassword.Password, encryptCode).Equals(_unlObj.User.pePwd))
      //{
      //  if (_unlObj.Permissions.Exists(c => c.pppm == "GIFTSRCPTS" && c.pppl > 1))
      //  {
      //    //using (var dbContext = new IMEntities())
      //    //{
      //    //  srObj = dbContext.SalesRooms.Where(c => c.srWH == cmbLocation.SelectedValue.ToString()).FirstOrDefault();
      //    //}
      //    frmInventoryMoves frmInvMovs = new frmInventoryMoves(_unlObj);
      //    frmInvMovs.Show();
      //    Close();
      //  }
      //  else
      //  {
      //    MessageBox.Show("User doesn't have access", "IM Inventory Movements", MessageBoxButton.OK, MessageBoxImage.Information);
      //    Close();
      //  }
      //}
      //else
      //{
      //  MessageBox.Show("Invalid password.", "IM Inventory Movements", MessageBoxButton.OK, MessageBoxImage.Information);
      //  iniciarCampos();
      //}
    }

    private void btnCancelar_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    /// <summary>
    /// Método para cargar el combobox de Warehouses cuando el textbox txtUser pierde el
    /// foco.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/Feb/2016 Created
    /// </history>
    private void txtUser_LostFocus(object sender, RoutedEventArgs e)
    {
      using (var dbContext = new Model.IMEntities())
      {
        _lstWhsByUsr = dbContext.USP_OR_GetWarehousesByUser(txtUser.Text, "ALL").ToList();
      }
      cmbLocation.ItemsSource = _lstWhsByUsr;
      cmbLocation.IsEnabled = true;
    }

    /// <summary>
    /// Carga inicial del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 26/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      iniciarCampos(); 
    }
    #endregion

    /// <summary>
    /// Inicializa los campos de Usuario y Password.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 26/Feb/2016 Created
    /// </history>
    private void iniciarCampos()
    {
      txtUser.Focus();
      txtUser.SelectAll();
      txtPassword.Password = string.Empty;
    }
  }
}