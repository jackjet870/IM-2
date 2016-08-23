using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using IM.Styles.UserControls;

namespace IM.Styles.Classes
{
  /// <summary>
  /// Logica del funcionamiento de la barra de menu para editar el Contenido del RichTextBox
  /// </summary>
  /// [erosado] 06/05/2016  Created
  public static class RichTextBoxToolBar
  {
    private static readonly SolidColorBrush Color = (SolidColorBrush)new BrushConverter().ConvertFrom("#147F79");   
    private static ToolBarRtfFontStyle _tbrFontStyle;
    private static ToolBarRtfStyle _tbrStyle;

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
      rtb.Focus();
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
      rtb.Focus();
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
      rtb.Focus();
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
      rtb.Focus();
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
      rtb.Focus();
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
      rtb.Focus();
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
      rtb.Focus();
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
      rtb.Focus();
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
      rtb.Focus();
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
    /// [jorcanche] modified 20/08/2016 ahora segun el color que escoja es el color que tendra el boton 
    /// </history>
    public static void OnColorPick(ref RichTextBox rtb, ref Border brd)
    {
      rtb.Focus();
      System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
      if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        var colorText =
          new SolidColorBrush(System.Windows.Media.Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R,colorDialog.Color.G, colorDialog.Color.B));
        rtb.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, colorText);
        brd.Background = colorText;
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
    /// <summary>
    /// Modifica los controles segun el las propiedades del texto seleccionado
    /// </summary>
    /// <history>
    /// [jorcanche] created 20/08/2016
    ///  </history>
    public static void OnSalectionChanded(ref RichTextBox rtb, ref ToolBarRtfFontStyle tbrFontStyle, ref ToolBarRtfStyle tbrStyle)
    {
      if (rtb == null || tbrFontStyle == null || tbrStyle == null || rtb.IsReadOnly) return;
      
      _tbrFontStyle = tbrFontStyle;
      _tbrStyle = tbrStyle;
      rtb.LostFocus += LostFocusRichTextBox;

      //Font Family
      var fontFamily = rtb.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
      if (fontFamily != null && fontFamily.ToString() != "{DependencyProperty.UnsetValue}") tbrFontStyle.cbxfontFamilies.SelectedItem = fontFamily.GetType().GetProperty("Source").GetValue(fontFamily);

      //Font Size
      var property = rtb.Selection.GetPropertyValue(TextElement.FontSizeProperty);
      double fontSize;
      if (property != null && double.TryParse(property.ToString(), out fontSize))
      {
        var redondear = (int)Math.Round(fontSize);
        tbrFontStyle.cbxfontSize.SelectedItem = redondear;
      }

      //TextBold
      var textBold = rtb.Selection.GetPropertyValue(TextElement.FontWeightProperty);
      tbrStyle.imgTextBold.Background = textBold.Equals(FontWeights.Bold) ? Color : Brushes.White;

      //TextItalic 
      var textItalic = rtb.Selection.GetPropertyValue(TextElement.FontStyleProperty);
      tbrStyle.imgTextItalic.Background = textItalic.Equals(FontStyles.Italic) ? Color : Brushes.White;

      //TextDecorations
      var textDecorations = rtb.Selection.GetPropertyValue(Inline.TextDecorationsProperty);

      //TextUnderLine      
      tbrStyle.imgTextUnderLine.Background = textDecorations.Equals(TextDecorations.Underline) ? Color : Brushes.White;

      //TextStrikeOut      
      tbrStyle.imgTextStrikeOut.Background = textDecorations.Equals(TextDecorations.Strikethrough) ? Color : Brushes.White;

      //TextAlignment
      var textAlignment = rtb.Selection.GetPropertyValue(FlowDocument.TextAlignmentProperty);

      //TextLeft      
      tbrStyle.imgTextLeft.Background = textAlignment.Equals(TextAlignment.Left) ? Color : Brushes.White;

      //TextCenter      
      tbrStyle.imgTextCenter.Background = textAlignment.Equals(TextAlignment.Center) ? Color : Brushes.White;

      //TextRight      
      tbrStyle.imgTextRight.Background = textAlignment.Equals(TextAlignment.Right) ? Color : Brushes.White;

      //TextColor
      var textColor = rtb.Selection.GetPropertyValue(TextElement.ForegroundProperty).ToString();
      if (textColor != "{DependencyProperty.UnsetValue}")
      {
        tbrStyle.imgColorPick.Background = textColor.Equals("#FF000000")
          ? Brushes.White
          : (SolidColorBrush)new BrushConverter().ConvertFrom(textColor);
      }
    }

    public static void LostFocus(ref RichTextBox rtb, ref ToolBarRtfFontStyle tbrFontStyle, ref ToolBarRtfStyle tbrStyle)
    {
      if (rtb == null || tbrFontStyle == null || tbrStyle == null || rtb.IsReadOnly) return;

    }

    #region LostFocus
    /// <summary>
    /// Asigna valor por defecto 0
    /// </summary>
    /// <history>
    /// </history>
    public static void LostFocusRichTextBox(object sender, RoutedEventArgs e)
    {      
      //TextBold      
      _tbrStyle.imgTextBold.Background = Brushes.White;

      //TextItalic       
      _tbrStyle.imgTextItalic.Background = Brushes.White;

      //TextUnderLine      
      _tbrStyle.imgTextUnderLine.Background = Brushes.White;

      //TextStrikeOut      
      _tbrStyle.imgTextStrikeOut.Background = Brushes.White;

      //TextLeft      
      _tbrStyle.imgTextLeft.Background = Brushes.White;

      //TextCenter      
      _tbrStyle.imgTextCenter.Background = Brushes.White;

      //TextRight      
      _tbrStyle.imgTextRight.Background = Brushes.White;

      //TextColor
      _tbrStyle.imgColorPick.Background = Brushes.White;
    }    


    #endregion

    #endregion
  }
}
