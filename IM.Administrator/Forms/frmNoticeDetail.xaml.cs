using IM.Styles.Classes;
using IM.Styles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmNoticeDetail.xaml
  /// </summary>
  public partial class frmNoticeDetail : Window, IRichTextBoxToolBar
  {
    #region variables
    public EnumMode enumMode;
    public Notice oldNotice = new Notice();
    public Notice notice = new Notice();
    private bool _blnIsClosing = false;
    private bool _blnIsCellCancel = false;
    private List<LeadSource> _lstOldLeadSources = new List<LeadSource>();
    #endregion

    public frmNoticeDetail()
    {
      InitializeComponent();
      #region Manejadores de Eventos
      ucRichTextBoxToolBar1.eColorPick += new EventHandler(ColorPick);
      ucRichTextBoxToolBar1.eExportRTF += new EventHandler(ExportRTF);
      ucRichTextBoxToolBar1.eLoadRTF += new EventHandler(LoadRTF);
      ucRichTextBoxToolBar1.eTextBold += new EventHandler(TextBold);
      ucRichTextBoxToolBar1.eTextCenter += new EventHandler(TextCenter);
      ucRichTextBoxToolBar1.eTextItalic += new EventHandler(TextItalic);
      ucRichTextBoxToolBar1.eTextLeft += new EventHandler(TextLeft);
      ucRichTextBoxToolBar1.eTextRight += new EventHandler(TextRight);
      ucRichTextBoxToolBar1.eTextStrikeOut += new EventHandler(TextStrikeOut);
      ucRichTextBoxToolBar1.eTextUnderLine += new EventHandler(TextUnderLine);
      ucRichTextBoxToolBar2.eChangeFontFamily += new EventHandler(ChangeFontFamily);
      ucRichTextBoxToolBar2.eChangeFontSize += new EventHandler(ChangeFontSize);
      #endregion
    }

    #region Methods Form
    #region Accept
    /// <summary>
    /// Guarda los cambios realizados
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    private async void btnAccept_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        btnAccept.Focus();
        List<LeadSource> lstLeadSources = dgrLeadSources.ItemsSource as List<LeadSource>;
        notice.noText = UIRichTextBoxHelper.getRTFFromRichTextBox(ref richTextBox);
        if (ObjectHelper.IsEquals(notice, oldNotice) && ObjectHelper.IsListEquals(lstLeadSources, _lstOldLeadSources))
        {
          dgrLeadSources.CancelEdit();
          _blnIsClosing = true;
          Close();
        }
        else
        {
          if (DateHelper.ValidateValueDate(dtpkFrom, dtpkTo))
          {
            txtStatus.Text = "Saving Data...";
            skpStatus.Visibility = Visibility.Visible;
            btnAccept.Visibility = Visibility.Collapsed;
            string strMsj = "";
            strMsj = ValidateHelper.ValidateForm(this, "Notice", blnDatagrids: true);
            if (dgrLeadSources.Items.Count == 1)
            {
              strMsj += (strMsj != "") ? " \n " : "" + "specify at least one LeadSource.";
            }
            if (strMsj == "")
            {
              List<LeadSource> lstAddLeadSources = lstLeadSources.Where(ls => !_lstOldLeadSources.Any(lss => lss.lsID == ls.lsID)).ToList();
              List<LeadSource> lstDelLeadSources = _lstOldLeadSources.Where(ls => !lstLeadSources.Any(lss => lss.lsID == ls.lsID)).ToList();
              int nRes = await BRNotices.SaveNotice(notice, enumMode == EnumMode.edit, lstAddLeadSources, lstDelLeadSources);
              UIHelper.ShowMessageResult("Notice", nRes);
              if(nRes>0)
              {
                _blnIsClosing = true;
                DialogResult = true;
                Close();
              }
            }
            else
            {
              UIHelper.ShowMessage(strMsj);
            }
            skpStatus.Visibility = Visibility.Collapsed;
            btnAccept.Visibility = Visibility.Visible;
          }
        }
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Verifica los cambios de la ventana antes de cerrar
    /// </summary>
    /// <history>
    /// [emoguel] cretaed 26/07/2016
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      btnCancel.Focus();
      if(!_blnIsClosing)
      {
        dgrLeadSources.CancelEdit();
        notice.noText = UIRichTextBoxHelper.getRTFFromRichTextBox(ref richTextBox);
        List<LeadSource> lstLeadSources = dgrLeadSources.ItemsSource as List<LeadSource>;
        if(!ObjectHelper.IsEquals(notice,oldNotice) || !ObjectHelper.IsListEquals(lstLeadSources,_lstOldLeadSources))
        {
          MessageBoxResult result = UIHelper.ShowMessage("There are pending changes. Do you want to discard them?", MessageBoxImage.Question, "Closing window");
          if (result == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
        }
      }
    }
    #endregion

    #region btnAsignLsbyPg_Click
    /// <summary>
    /// Asigna los leadsource por Program
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    private void btnAsignLsbyPg_Click(object sender, RoutedEventArgs e)
    {
      if (cmbPrograms.SelectedValue != null)
      {
        string idProgram = cmbPrograms.SelectedValue.ToString();
        List<LeadSource> lstLeadSources = cmbLeadSources.ItemsSource as List<LeadSource>;
        List<LeadSource> lstLeadSourceByPg = lstLeadSources.Where(ls => ls.lspg == idProgram).ToList();
        dgrLeadSources.ItemsSource = lstLeadSourceByPg;
        cmbLeadSources.Header = "Lead Sources (" + lstLeadSourceByPg.Count + ")";
      }
    }
    #endregion

    #region btnAssignLsbyZn_Click
    /// <summary>
    /// Asigna Leadsource por Zona
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    private void btnAssignLsbyZn_Click(object sender, RoutedEventArgs e)
    {
      if (cmbZones.SelectedValue != null)
      {
        string idZone = cmbZones.SelectedValue.ToString();
        List<LeadSource> lstLeadSources = cmbLeadSources.ItemsSource as List<LeadSource>;
        List<LeadSource> lstLeadSourcesByZone = lstLeadSources.Where(ls => ls.lszn == idZone).ToList();
        dgrLeadSources.ItemsSource = lstLeadSourcesByZone;
        cmbLeadSources.Header = "Lead Sources (" + lstLeadSourcesByZone.Count + ")";
      }
    }
    #endregion

    #region CellEditing
    /// <summary>
    /// Verifica que el leadsource no se repita
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if(e.EditAction==DataGridEditAction.Commit)
      {
        _blnIsCellCancel = false;
        bool blnIsRepeat = GridHelper.HasRepeatItem((Control)e.EditingElement, dgrLeadSources);
        if(!blnIsRepeat)
        {
          cmbLeadSources.Header = "Lead Sources (" + (dgrLeadSources.Items.Count - 1) + ")";
        }
        e.Cancel = blnIsRepeat;
      }
      else
      {
        _blnIsCellCancel = true;
      }
    }
    #endregion

    #region IrichTextBox   
    #region LoadRTF
    /// <summary>
    /// Cargar rtf
    /// </summary>
    /// <history>
    /// [emoguel] created 27/26/2016
    /// </history>
    public void LoadRTF(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnLoadRTF(ref richTextBox);
    }
    #endregion

    #region ExportRTF
    /// <summary>
    /// Exportar a rtf
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    public void ExportRTF(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnExportRTF(ref richTextBox);
    }
    #endregion

    #region TextBold
    /// <summary>
    /// Cambia el tipo de fuente a Bold
    /// </summary>
    /// <history>
    /// [emoguel] created 27/06/2016
    /// </history>
    public void TextBold(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextBold(ref richTextBox);
    }
    #endregion

    #region TextItalic
    /// <summary>
    /// Cambia el tipo de fuente a italic
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    public void TextItalic(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }
    #endregion

    #region TextUnderLine
    /// <summary>
    /// Cambia el tipo de fuente a subrayado
    /// </summary>
    /// <histoty>
    /// [emoguel] created 26/07/2016
    /// </histoty>
    public void TextUnderLine(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }
    #endregion

    #region TextStrikeOut
    /// <summary>
    /// Cambia el modo de texto a StrikeOut
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    public void TextStrikeOut(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextStrikeOut(ref richTextBox);
    }
    #endregion

    #region TextCenter
    /// <summary>
    /// Centra el Texto
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    public void TextCenter(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextCenter(ref richTextBox);
    }
    #endregion

    #region TextRight
    /// <summary>
    /// Alinea a la derecha el texto
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    public void TextRight(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextRight(ref richTextBox);
    }
    #endregion

    #region TextLeft
    /// <summary>
    /// Alinea el texto a la izquierda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    public void TextLeft(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnTextLeft(ref richTextBox);
    }
    #endregion

    #region Color pick
    /// <summary>
    /// Cambia el color de la fuente
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    public void ColorPick(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnColorPick(ref richTextBox);
    }
    #endregion

    #region Change font size
    /// <summary>
    /// Cambia el tamano de fuente
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    public void ChangeFontSize(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnChangeFontSize(ref richTextBox, ref ucRichTextBoxToolBar2.cbxfontSize);
    }
    #endregion

    #region ChangeFontFamily
    /// <summary>
    /// Cambia la famila de la fuente
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    public void ChangeFontFamily(object sender, EventArgs e)
    {
      RichTextBoxToolBar.OnChangeFontFamily(ref richTextBox, ref ucRichTextBoxToolBar2.cbxfontFamilies);
    }
    #endregion

    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      UIHelper.SetUpControls(notice, this);
      ObjectHelper.CopyProperties(notice, oldNotice);
      LoadZones();
      LoadPrograms();
      LoadLeadSources();
      ComboBoxHelper.ConfigureDates(cmbRangeDate, EnumPeriod.None, selectedCmb: 0);      
      UIRichTextBoxHelper.LoadRTF(ref richTextBox, notice.noText);
      oldNotice.noText = UIRichTextBoxHelper.getRTFFromRichTextBox(ref richTextBox);
      DataContext = notice;
      dgrLeadSources.BeginningEdit += GridHelper.dgr_BeginningEdit;
    }
    #endregion

    #region dgrLeadSources_KeyDown
    /// <summary>
    /// Cambia el contador en caso de que se elimine un registro
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    private void dgrLeadSources_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        cmbLeadSources.Header = "Lead Sources (" + (dgrLeadSources.Items.Count - 2) + ")";
      }
    }
    #endregion

    #region cmbRangeDate_SelectionChanged
    /// <summary>
    /// bloquea o desbloquea los datetimepicker
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    private void cmbRangeDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count > 0)
      {
        EnumPredefinedDate enumPeriod = ((EnumPredefinedDate)cmbRangeDate.SelectedValue);
        Tuple<DateTime, DateTime> dateRange = DateHelper.GetDateRange(enumPeriod);

        dtpkFrom.Value = dateRange.Item1;
        dtpkTo.Value = dateRange.Item2;
        if (enumPeriod == EnumPredefinedDate.DatesSpecified)
        {
          dtpkFrom.IsEnabled = true;
          dtpkTo.IsEnabled = true;
        }
        else
        {
          dtpkFrom.IsEnabled = false;
          dtpkTo.IsEnabled = false;
        }
      }
    }
    #endregion

    #region dgrLeadSources_RowEditEnding
    /// <summary>
    /// Impide que se agreguen registros vacios
    /// </summary>
    /// <history>
    /// [emoguel] created 27/07/2016
    /// </history>
    private void dgrLeadSources_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_blnIsCellCancel)
        {
          dgrLeadSources.RowEditEnding -= dgrLeadSources_RowEditEnding;
          dgrLeadSources.CancelEdit();
          dgrLeadSources.RowEditEnding += dgrLeadSources_RowEditEnding;
          _blnIsCellCancel = false;
        }
      }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadZones
    /// <summary>
    /// Carga las zonas
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    private async void LoadZones()
    {
      try
      {
        cmbZones.ItemsSource = await BRZones.GetZones();        
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadPrograms
    /// <summary>
    /// Carga el combobox de programs
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    private async void LoadPrograms()
    {
      try
      {
        cmbPrograms.ItemsSource = (await BRPrograms.GetPrograms()).OrderBy(pg=>pg.pgN).ToList();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region LoadLeadSources
    /// <summary>
    /// Cargamos los LeadSources lgados al notice
    /// </summary>
    /// <history>
    /// [emoguel] created 26/07/2016
    /// </history>
    private async void LoadLeadSources()
    {
      try
      {
        cmbLeadSources.ItemsSource = await BRLeadSources.GetLeadSources(-1,-1,-1);
        List<LeadSource> lstLeadSources = await BRLeadSources.GetLeadsourcesByNotice(notice.noID);
        dgrLeadSources.ItemsSource = lstLeadSources;
        _lstOldLeadSources = lstLeadSources.Select(ls => {
          LeadSource lss = new LeadSource();
          ObjectHelper.CopyProperties(lss, ls);
          return lss;
        }).ToList();
        cmbLeadSources.Header = "Lead Sources (" + lstLeadSources.Count + ")";
        skpStatus.Visibility = Visibility.Collapsed;
        btnAccept.Visibility = Visibility.Visible;
        GridHelper.SelectRow(dgrLeadSources, 0);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #endregion
  }
}
