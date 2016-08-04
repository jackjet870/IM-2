using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace IM.Styles.Classes
{
  /// <summary>
  /// Logica del funcionamiento de la barra de menu para editar el Contenido del RichTextBox
  /// </summary>
  /// [erosado] 06/05/2016  Created
  public static class RichTextBoxToolBar
  {
    #region ToolBar RichTextBox
    /// <summary>
    /// Carga un archivo RTF al RichTextBox
    /// </summary>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// [emoguel] 04/08/2016 modified ahora sólo acepta archivos rtf
    /// </history>
    public static void OnLoadRTF(ref RichTextBox rtb)
    {
      System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
      dlg.Filter = "Rich Text Format (*.rtf)|*.rtf";
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
        TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
        range.Load(fileStream, DataFormats.Rtf);
        fileStream.Close();
      }
    }
    /// <summary>
    /// Exporta el contenido del RichTextBox a un archivo RTF
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    public static void OnExportRTF(ref RichTextBox rtb)
    {
      System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
      dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
        TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
        range.Save(fileStream, DataFormats.Rtf);
        fileStream.Close();
      }
    }
    /// <summary>
    /// Cambia a negritas el texto seleccionado en el RichTextBox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    public static void OnTextBold(ref RichTextBox rtb)
    {
      object temp = rtb.Selection.GetPropertyValue(TextElement.FontWeightProperty);
      if (temp != null)
      {
        if (!temp.Equals(FontWeights.Bold))
        {
          rtb.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }
        else
        {
          rtb.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
        }
      }
    }
    /// <summary>
    /// Cambia a italica el texto seleccionado en el RichTextBox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    public static void OnTextItalic(ref RichTextBox rtb)
    {
      object temp = rtb.Selection.GetPropertyValue(TextElement.FontStyleProperty);
      if (temp != null)
      {
        if (!temp.Equals(FontStyles.Italic))
        {
          rtb.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
        }
        else
        {
          rtb.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
        }
      }
    }
    /// <summary>
    /// Cambia a Subrayado el texto seleccionado en el RichTextBox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    public static void OnTextUnderLine(ref RichTextBox rtb)
    {
      object temp = rtb.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
      if (temp != null)
      {
        if (!temp.Equals(TextDecorations.Underline))
        {
          rtb.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
        }
        else
        {
          rtb.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        }
      }
    }
    /// <summary>
    /// Cambia a Tachado el texto seleccionado en el RichTextBox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    public static void OnTextStrikeOut(ref RichTextBox rtb)
    {
      object temp = rtb.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
      if (temp != null)
      {
        if (!temp.Equals(TextDecorations.Strikethrough))
        {
          rtb.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
        }
        else
        {
          rtb.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        }
      }
    }
    /// <summary>
    /// Alinea a la izquierda  el texto seleccionado en el RichTextBox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    public static void OnTextLeft(ref RichTextBox rtb)
    {
      if (!rtb.Selection.GetPropertyValue(FlowDocument.TextAlignmentProperty).Equals(TextAlignment.Left))
      {
        rtb.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Left);
      }
      else
      {
        rtb.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Left);
      }
    }
    /// <summary>
    /// Centra  el texto seleccionado en el RichTextBox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    public static void OnTextCenter(ref RichTextBox rtb)
    {
      if (!rtb.Selection.GetPropertyValue(FlowDocument.TextAlignmentProperty).Equals(TextAlignment.Center))
      {
        rtb.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Center);
      }
      else
      {
        rtb.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Left);
      }
    }
    /// <summary>
    /// Alinea a la derecha  el texto seleccionado en el RichTextBox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    public static void OnTextRight(ref RichTextBox rtb)
    {
      if (!rtb.Selection.GetPropertyValue(FlowDocument.TextAlignmentProperty).Equals(TextAlignment.Right))
      {
        rtb.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Right);
      }
      else
      {
        rtb.Selection.ApplyPropertyValue(FlowDocument.TextAlignmentProperty, TextAlignment.Left);
      }
    }
    /// <summary>
    /// Cambia el color de el texto seleccionado en el RichTextBox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    public static void OnColorPick(ref RichTextBox rtb)
    {
      System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
      if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
        TextRange range = new TextRange(rtb.Selection.Start, rtb.Selection.End);
        range.ApplyPropertyValue(TextElement.ForegroundProperty, scb);
      }

    }
    /// <summary>
    /// Cambia la letra del texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// [jorcanche]  Modified 25/07/2016 Se agrego el focus del RichTextBox 
    /// </history>
    public static void OnChangeFontFamily(ref RichTextBox rtb, ref ComboBox cbx)
    {
      rtb.Focus();
      if (cbx.SelectedItem != null)
        rtb.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, cbx.SelectedItem);
    }
    /// <summary>
    /// Cambia el tamaño del texto seleccionado en el richtextbox
    /// </summary>
    /// <history>
    /// [erosado] 06/04/2016
    /// [jorcanche]  Modified 25/07/2016 Se agrego el focus del RichTextBox 
    /// </history>
    public static void OnChangeFontSize(ref RichTextBox rtb, ref ComboBox cbx)
    {
      rtb.Focus();
      rtb.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, cbx.SelectedItem.ToString());
    }
    #endregion
  }
}
