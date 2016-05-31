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
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Base.Helpers;
using IM.BusinessRules.BR;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmProgramDetail.xaml
  /// </summary>
  public partial class frmProgramDetail : Window
  {
    #region Variables
    public Program program = new Program();//Objeto a guardar
    public Program oldProgram = new Program();//Objeto con los datos iniciales
    public EnumMode enumMode;//Modo en que se abrirá la ventana
    private List<LeadSource> _oldList = new List<LeadSource>();//Lista Inicial de leadSources
    private bool isCellCancel = false;
    private bool blnClosing = false;
    #endregion
    public frmProgramDetail()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      skpStatus.Visibility = Visibility.Visible;
      txtStatus.Text = "Loading...";
      ObjectHelper.CopyProperties(program, oldProgram);
      UIHelper.SetUpControls(program, this);
      LoadLeadSources();
      if(enumMode!=EnumMode.preview)
      {
        dgrLeadSources.IsReadOnly = false;
        txtpgID.IsEnabled = (enumMode == EnumMode.add);
        txtpgN.IsEnabled = true;
        btnAccept.Visibility = Visibility.Visible;
      }
      DataContext = program;
    }
    #endregion

    #region Closing
    /// <summary>
    /// Cierra la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2015
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!blnClosing)
      {
        blnClosing = true;
        btnCancel_Click(null, null);
        if (!blnClosing)
        {          
          e.Cancel = true;
        }
        else
        {
          blnClosing = false;
        }
      }
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Cierra la ventana con el boton escape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key==Key.Escape)
      {
        btnCancel_Click(null, null);
      }
    }
    #endregion

    #region dgrLeadSources_CellEditEnding
    /// <summary>
    /// Verifica que no se repita un registro ya seleccionado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void dgrLeadSources_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!Keyboard.IsKeyDown(Key.Escape))
      {
        isCellCancel = false;
        bool isRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement,dgrLeadSources);
        e.Cancel = isRepeat;
      }
      else
      {
        isCellCancel = true;
      }
    }
    #endregion

    #region dgrLeadSources_RowEditEnding
    /// <summary>
    /// Verifica que no se agreguen registros vacioss
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void dgrLeadSources_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (isCellCancel)
      {
        dgrLeadSources.RowEditEnding -= dgrLeadSources_RowEditEnding;
        dgrLeadSources.CancelEdit();
        dgrLeadSources.RowEditEnding += dgrLeadSources_RowEditEnding;        
      }
      else
      {
        cmbLeadSources.Header = "Lead Source (" + (dgrLeadSources.Items.Count-1) + ")";
      }
    }  
    #endregion

    #region Cancel
    /// <summary>
    /// Verifica cambios pendientes antes de cerrar la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      btnCancel.Focus();
      if(enumMode!=EnumMode.preview)
      {
        List<LeadSource> lstLeadSources = (List<LeadSource>)dgrLeadSources.ItemsSource;
        if (!ObjectHelper.IsEquals(program, oldProgram) || !ObjectHelper.IsListEquals(lstLeadSources, _oldList))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.Yes)
          {
            if (!blnClosing) { blnClosing = true; Close(); }
          }
          else
          {
            blnClosing = false;
          }
        }
        else
        {
          if (!blnClosing) { blnClosing = true; Close(); }
        }
      }
      else
      {
        if (!blnClosing){ blnClosing = true;Close(); }
      }
    } 
    #endregion

    #region btnAccept_Click
    /// <summary>
    /// Guaarda un program
    /// Asigan|Desasigna LeadSources
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      btnAccept.Focus();
      List<LeadSource> lstLeadSources = (List<LeadSource>)dgrLeadSources.ItemsSource;
      if (ObjectHelper.IsEquals(program, oldProgram) && ObjectHelper.IsListEquals(lstLeadSources, _oldList))
      {
        blnClosing = true;
        Close();
      }
      else
      {
        string strMsj = ValidateHelper.ValidateForm(this, "Program");
        if(strMsj=="")
        {
          skpStatus.Visibility = Visibility.Visible;
          txtStatus.Text = "Saving Data...";
          List<LeadSource> lstAdd = lstLeadSources.Where(ls => !_oldList.Any(lss => lss.lsID == ls.lsID)).ToList();

          int nRes = await BRPrograms.SaveProgram(program, lstAdd, (enumMode == EnumMode.edit));
          
          UIHelper.ShowMessageResult("Program", nRes);
          skpStatus.Visibility = Visibility.Collapsed;
          if (nRes > 0)
          {
            blnClosing = true;
            DialogResult = true;
            Close();
          }
        }
        else
        {
          UIHelper.ShowMessage(strMsj);
        }
      }
    } 
    #endregion

    #region Row KeyDown
    /// <summary>
    /// Elimina registros nuevos con el boton suprimir
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [created] 25/05/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        var item = dgrLeadSources.SelectedItem;
        if (item.GetType().Name == "LeadSource")
        {
          LeadSource leadSource = (LeadSource)item;
          if (leadSource.lspg == program.pgID)
          {
            e.Handled = true;
          }
        }
        else
        {
          cmbLeadSources.Header ="Lead Source ("+( dgrLeadSources.Items.Count - 1 )+ ")";
        }
      }
    }

    #endregion
    #endregion

    #region Methods
    #region LoadLeadSources
    /// <summary>
    /// Llena el grid de LeadSources
    /// </summary>
    /// <history>
    /// [emoguel] created 25/05/2016
    /// </history>
    private async void LoadLeadSources()
    {
      try
      {
        List<LeadSource> lstAllLeadSources = await BRLeadSources.GetLeadSources(-1, -1);
        cmbLeadSources.ItemsSource = lstAllLeadSources;
        List<LeadSource> lstLeadSources = lstAllLeadSources.Where(ls => ls.lspg == program.pgID).ToList();
        dgrLeadSources.ItemsSource = lstLeadSources;
        _oldList = lstLeadSources.ToList();
        cmbLeadSources.Header = "Lead Source (" + lstLeadSources.Count + ")";
        skpStatus.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Program");
      }
    } 
    #endregion
    #endregion
  }
}
