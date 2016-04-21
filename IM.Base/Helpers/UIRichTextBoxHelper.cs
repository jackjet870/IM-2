using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace IM.Base.Helpers
{
  public class UIRichTextBoxHelper
  {
    #region GetRTFFromRichTextBox
    /// <summary>
    /// Obtiene toda la información del RichTextBox respetando el formato RTF
    /// </summary>
    /// <param name="rtb">ref RichTextBox</param>
    /// <returns>string RTF</returns>
    /// <history>
    /// [erosado] 08/04/2016  Created
    /// </history>
    public static string getRTFFromRichTextBox(ref RichTextBox rtb)
    {
      using(MemoryStream ms = new MemoryStream())
      {
        TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
        textRange.Save(ms, DataFormats.Rtf);
        return Encoding.Default.GetString(ms.ToArray());
      }
    }
    #endregion

    #region HasInfo
    /// <summary>
    /// Verifica si hay información escrita en el richtextBox
    /// </summary>
    /// <param name="rtb">RIchTextBox para verificar</param>
    /// <returns>True or False</returns>
    /// <history>
    /// [erosado] 08/04/2016  Created
    /// </history>
    public static bool HasInfo(ref RichTextBox rtb)
    {
      var start = rtb.Document.ContentStart;
      var end = rtb.Document.ContentEnd;
      int difference = start.GetOffsetToPosition(end);

      if (difference!=0 && difference != 2 && difference != 4)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
    #endregion

    #region LoadRTF
    /// <summary>
    /// Carga un RichTextBox con un Archivo RTF
    /// </summary>
    /// <param name="rtb">ref RichTextBox</param>
    /// <history>
    /// [erosado] 07/04/2016  Created
    /// </history>
    public static void LoadRTF(ref RichTextBox rtb)
    {
      System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
      dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
        TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
        range.Load(fileStream, DataFormats.Rtf);
        fileStream.Close();
      }

    }
    /// <summary>
    /// Carga un RichTextBox con un string con formato RTF.
    /// </summary>
    /// <param name="rtb">ref RichTextBox</param>
    /// <param name="rtf">string rtf</param>
    /// <history>
    /// [erosado] 16/04/2016  Created
    /// </history>
    public static void LoadRTF(ref RichTextBox rtb, string rtf)
    {
      if (!string.IsNullOrEmpty(rtf))
      {
        using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(rtf)))
        {
          TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
          range.Load(ms, DataFormats.Rtf);
          ms.Close();
        }
      }
    }

    #endregion

    #region ExportRTF
    /// <summary>
    /// Exporta el contenido del RichTextBox a un Archivo RTF
    /// </summary>
    /// <param name="rtb">ref RichTextBox</param>
    /// <history>
    /// [erosado] 06/04/2016
    /// </history>
    public static void ExportRTF(ref RichTextBox rtb)
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
    #endregion

    }
}
