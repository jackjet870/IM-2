using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmSearch.xaml
  /// </summary>
  public partial class frmSearch : Window
  {
    #region Variables
    public string strID="";//Id a filtrar
    public string strDesc="";//Descripcion a filtrar
    public int nStatus=-1;//Estatus a filtrar
    public EnumWindow enumWindow;//Formulario desde el que se utiliza
    #region Agency
    public string sSegment="";//Sement by agency para cuando se abra desde agency 
    #endregion
    #region FolioInvitationOutHouse
    public string strSerie = "";//Serie a filtrar
    #endregion
    #region Hotels
    public string strHotelGroup = "";
    public string strArea = "";
    #endregion
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
      ComboBoxHelper.LoadComboDefault(cmbStatus);
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

    #region Textbox OnlyNumber
    /// <summary>
    /// Valida que un texbox acepte solo números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created
    /// </history>
    private void PreviewTextInputNumber(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }
    #endregion

    #region TextBox OnlyLetter
    /// <summary>
    /// Valida que un texbox acepte sólo letras
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 23/03/2016
    /// </history>
    private void PreviewTextInputLetter(object sender, TextCompositionEventArgs e)
    {
      if (!(char.IsNumber(Convert.ToChar(e.Text))))
      {
        e.Handled = true;
      }
    } 
    #endregion

    #endregion
    #region metodos

    #region LoadForm
    /// <summary>
    /// Llena el formulario dependiendo de la ventana que lo utilice
    /// </summary>
    /// <history>
    /// [Emoguel] created 02/03/2016
    /// </history>
    protected void LoadForm()
    {
      txtID.Text = strID;
      cmbStatus.SelectedValue = nStatus;
      txtD.Text = strDesc;
      switch (enumWindow)
      { 

        #region ChargeTo
        case EnumWindow.ChargeTos:
          {
            txtD.PreviewTextInput += TextBoxHelper.IntTextInput;
            txtD.MaxLength = 3;
            lblDes.Content = "Price";
            lblSta.Content = "CxC";

            if (Convert.ToInt32(strDesc) > 0)
            {
              txtD.Text = strDesc;
            }
            else
            {
              txtD.Text = "";
            }
            break;
          }
        #endregion

        #region Agency
        case EnumWindow.Agencies:
          {
            cmbSegment.Visibility = Visibility.Visible;
            lblSegment.Visibility = Visibility.Visible;
            loadSegments();
            cmbSegment.SelectedValue = sSegment;            
            break;
          }
        #endregion

        #region Computer
        case EnumWindow.Computers:
          {
            cmbStatus.Visibility = Visibility.Collapsed;
            lblSta.Visibility = Visibility.Collapsed;            
            break;
          }
        #endregion

        #region DefaultInt
        case EnumWindow.DefaultInt:
          {
            txtID.PreviewTextInput += TextBoxHelper.IntTextInput;
            break;
          }
        #endregion

        #region FoliosCXC
        case EnumWindow.FoliosCxC:
          {
            txtD.PreviewTextInput += TextBoxHelper.IntTextInput;
            txtID.PreviewTextInput += TextBoxHelper.IntTextInput;
            lblID.Content = "From ";
            lblDes.Content = "To";
            if (strID == "0") { txtID.Text = ""; }
            if (strDesc == "0") { txtD.Text = ""; }
            break;
          }
        #endregion

        #region FolioInvOut
        case EnumWindow.FoliosInvitationOuthouse:
          {
            txtSerie.Visibility = Visibility.Visible;
            lblSerie.Visibility = Visibility.Visible;
            txtSerie.Text = strSerie;
            txtD.PreviewTextInput += TextBoxHelper.IntTextInput;
            txtID.PreviewTextInput += TextBoxHelper.IntTextInput;
            lblID.Content = "From ";
            lblDes.Content = "To";
            if (strID == "0") { txtID.Text = ""; }
            if (strDesc == "0") { txtD.Text = ""; }
            break;
          }
        #endregion

        #region DefaultByte
        case EnumWindow.DefaultByte:
          {
            txtID.MaxLength = 3;
            txtID.PreviewTextInput += TextBoxHelper.ByteTextInput;
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
      strID = txtID.Text.Trim();
      strDesc = txtD.Text.Trim();
      switch (enumWindow)
      {
        
        #region ChargeTo
        case EnumWindow.ChargeTos:
          {            
            if (!string.IsNullOrWhiteSpace(txtD.Text))//Si el campo price tiene algún valor
            {
              int nPrice = Convert.ToInt32(txtD.Text);
              if (nPrice > 0 && nPrice < 256)//Se valida que el número esté en el rango de tipo byte
              {
                strDesc = txtD.Text;
                DialogResult = true;
                this.Close();
              }
              else
              {
                txtD.Text = ((nPrice == 0) ? "" : nPrice.ToString());
                UIHelper.ShowMessage("The price must be higher than 0 \n and must be smaller than 255.");
              }
            }
            else
            {
              strDesc = "0";
              DialogResult = true;
              Close();
            }
            break;
          }
        #endregion
        #region Agency
        case EnumWindow.Agencies:
          {
            if (cmbSegment.SelectedValue != null)
            {
              sSegment = cmbSegment.SelectedValue.ToString();
            }
            strDesc = txtD.Text;
            DialogResult = true;
            Close();
            break;
          }
        #endregion
        #region DefaulInt
        case EnumWindow.DefaultByte:
        case EnumWindow.DefaultInt:
          {
            if (!string.IsNullOrWhiteSpace(txtID.Text))
            {
              if (Convert.ToInt32(txtID.Text.Trim()) > 0)
              {
                strID = txtID.Text.Trim();
                DialogResult = true;
                Close();
              }
              else
              {
                UIHelper.ShowMessage("The ID must be higher than 0.");
                txtID.Text = "0";
              }
            }
            else
            {
              strID = "0";
              DialogResult = true;
              Close();
            }
            break;
          }
        #endregion
        #region FoliosCXC
        case EnumWindow.FoliosCxC:
          { 
            int nFrom = 0;
            int nTo = 0;
            bool blnDialogResult = true;
            if (!string.IsNullOrWhiteSpace(txtID.Text) && string.IsNullOrWhiteSpace(txtD.Text))
            {
              UIHelper.ShowMessage("Specify the end number.");
              blnDialogResult = false;
            }
            else if (!string.IsNullOrWhiteSpace(txtD.Text) && string.IsNullOrWhiteSpace(txtID.Text))
            {
              UIHelper.ShowMessage("Specify the StartNumber.");
              blnDialogResult = false;
            }
            else if (!string.IsNullOrWhiteSpace(txtID.Text) && !string.IsNullOrWhiteSpace(txtD.Text))
            {
              nFrom = Convert.ToInt32(txtID.Text);
              nTo = Convert.ToInt32(txtD.Text);
              if (nFrom == 0 || nTo == 0)
              {
                UIHelper.ShowMessage("Start number and end number can not be 0.");
                blnDialogResult = false;
              }
              else if (nTo > nFrom)
              {
                UIHelper.ShowMessage("Start Number can not be greater than End Number.");
                blnDialogResult = false;
              }

            }
            
            if(blnDialogResult==true)
            {
              DialogResult = true;
              strID = nFrom.ToString();
              strDesc = nTo.ToString();
              Close();
            }
            break;
          }
        #endregion
        #region FolioInvOutHouse
        case EnumWindow.FoliosInvitationOuthouse:
          {
            int nFrom = 0;
            int nTo = 0;
            bool blnDialogResult = true;
            if (!string.IsNullOrWhiteSpace(txtID.Text) && string.IsNullOrWhiteSpace(txtD.Text))
            {
              UIHelper.ShowMessage("Specify the end number.");
              blnDialogResult = false;
            }
            else if (!string.IsNullOrWhiteSpace(txtD.Text) && string.IsNullOrWhiteSpace(txtID.Text))
            {
              UIHelper.ShowMessage("Specify the StartNumber.");
              blnDialogResult = false;
            }
            else if (!string.IsNullOrWhiteSpace(txtID.Text) && !string.IsNullOrWhiteSpace(txtD.Text))
            {
              nFrom = Convert.ToInt32(txtID.Text);
              nTo = Convert.ToInt32(txtD.Text);
              if (nFrom == 0 || nTo == 0)
              {
                UIHelper.ShowMessage("Start number and end number can not be 0.");
                blnDialogResult = false;
              }
              else if (nFrom < nTo)
              {
                UIHelper.ShowMessage("End Number can not be greater than Start Number.");
                blnDialogResult = false;
              }

            }

            if (blnDialogResult == true)
            {
              DialogResult = true;
              strID = nFrom.ToString();
              strDesc = nTo.ToString();
              strSerie = txtSerie.Text;
              Close();
            }
            break;
          } 
        #endregion
        #region Default
        default:
          {
            DialogResult = true;
            Close();
            break;
          }
        #endregion        
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
