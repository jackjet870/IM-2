using System;

namespace IM.Styles.Interfaces
{
  /// <summary>
  /// Interfaz para utilizar UserControl- RichTextBoxMenu1 y RichTextBoxMenu2
  /// </summary>
  /// [erosado] 06/05/2016
  public interface IRichTextBoxToolBar
  {
    void LoadRTF(object sender, EventArgs e);
    void ExportRTF(object sender, EventArgs e);
    void TextBold(object sender, EventArgs e);
    void TextItalic(object sender, EventArgs e);
    void TextUnderLine(object sender, EventArgs e);
    void TextStrikeOut(object sender, EventArgs e);
    void TextCenter(object sender, EventArgs e);
    void TextRight(object sender, EventArgs e);
    void ColorPick(object sender, EventArgs e);
    void ChangeFontSize(object sender, EventArgs e);
    void ChangeFontFamily(object sender, EventArgs e);
  }
}
