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
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSearch.xaml
  /// </summary>
  public partial class frmSearch : Window
  {
    #region Variables
    public string sID="";//Id a filtrar
    public string sDesc="";//Descripcion a filtrar
    public int nStatus;//Estatus a filtrar
    public string sForm = "Default";//Formulario desde el que se utiliza
    public string sSegment="";//Sement by agency para cuando se abra desde agency 
    #endregion

    public frmSearch()
    {
      InitializeComponent();
    }

    #region eventos de los controles
    
    #region Botton aceptar
    /// <summary>
    /// Cierra la ventana de busqueda guardando los filtros de la busqueda a realizar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 3/Mar/2016 Created
    /// </history>
    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      SetData();
    } 
    #endregion

    #region Loaded
    /// <summary>
    /// Muestra los datos del filtro actual del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 3/Mar/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadStatus();
      LoadForm();
    }

    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        this.Close();
      }
    }
    #endregion

    #region ChargeTo TextInput
    /// <summary>
    /// Valida que texbox de descripción acepte sólo números cuando se abre desde chargeTo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created
    /// </history>
    private void txtD_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }    
    #endregion
    #endregion
    #region metodos
    #region LoadStatus
    /// <summary>
    /// Llena la lista de estatus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 3/Mar/2016 Created
    /// </history>
    protected void LoadStatus()
    {
      List<object> lstStatus = new List<object>();
      lstStatus.Add(new { sName = "All", sValue = -1 });
      lstStatus.Add(new { sName = "Inactive", sValue = 0 });
      lstStatus.Add(new { sName = "Active", sValue = 1 });

      cmbStatus.ItemsSource = lstStatus;
    }

    #endregion
    #region LoadForm
    /// <summary>
    /// Llena el formulario dependiendo de la ventana que lo utilice
    /// </summary>
    /// <history>
    /// [Emoguel] created 02/03/2016
    /// </history>
    protected void LoadForm()
    {
      txtID.Text = sID;
      cmbStatus.SelectedValue = nStatus;
      txtD.Text = sDesc;
      switch (sForm)
      { 

        #region ChargeTo
        case "ChargeTo":
          {
            txtD.PreviewTextInput += new TextCompositionEventHandler(txtD_PreviewTextInput);
            txtD.MaxLength = 3;
            lblDes.Content = "Price";
            lblSta.Content = "CxC";

            if (Convert.ToInt32(sDesc) > 0)
            {
              txtD.Text = sDesc;
            }
            else
            {
              txtD.Text = "";
            }
            break;
          }
        #endregion

        #region Agency
        case "Agency":
          {
            cmbSegment.Visibility = Visibility.Visible;
            lblSegment.Visibility = Visibility.Visible;
            loadSegments();
            cmbSegment.SelectedValue = sSegment;            
            break;
          }
        #endregion

        #region Computer
        case "Computer":
          {
            cmbStatus.Visibility = Visibility.Collapsed;
            lblSta.Visibility = Visibility.Collapsed;            
            break;
          } 
          #endregion
      }

    }

    #endregion
    #region setData
    /// <summary>
    /// Llena los datos de busqueda dependiendo del formulario que  llame a la función
    /// </summary>
    /// <history>
    /// [Emoguel] created 02/03/2016
    /// </history>
    protected void SetData()
    {
      nStatus = Convert.ToInt32(cmbStatus.SelectedValue);
      sID = txtID.Text;
      sDesc = txtD.Text;
      switch (sForm)
      {
        
        #region ChargeTo
        case "ChargeTo":
          {            
            if (!string.IsNullOrWhiteSpace(txtD.Text))//Si el campo price tiene algún valor
            {
              int nPrice = Convert.ToInt32(txtD.Text);
              if (nPrice > 0 && nPrice < 256)//Se valida que el número esté en el rango de tipo byte
              {
                sDesc = txtD.Text;
                DialogResult = true;
                this.Close();
              }
              else
              {
                txtD.Text = ((nPrice == 0) ? "" : nPrice.ToString());
                MessageBox.Show("The price must be higher than 0 \n and must be smaller than 255");
              }
            }
            else
            {
              sDesc = "0";
              DialogResult = true;
              this.Close();
            }
            break;
          }
        #endregion
        #region Agency
        case "Agency":
          {
            if (cmbSegment.SelectedValue != null)
            {
              sSegment = cmbSegment.SelectedValue.ToString();
            }
            sDesc = txtD.Text;
            DialogResult = true;
            Close();
            break;
          }
        #endregion
        default:
          {            
            DialogResult = true;
            Close();
            break;
          }
      }
    }
    #endregion

    #region Agency

    #region LoadSegments
    /// <summary>
    /// Llena el combobox de segment cuando se abre desde agency
    /// </summary>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    protected void loadSegments()
    {
      List<SegmentByAgency> lstSegmentsByAgency = BRSegmentsByAgency.GetSegMentsByAgency(new SegmentByAgency());
      lstSegmentsByAgency.Insert(0,new SegmentByAgency { seID = "", seN = "" });
      cmbSegment.ItemsSource = lstSegmentsByAgency;
    } 
    #endregion
    #endregion
    #endregion
  }
}
