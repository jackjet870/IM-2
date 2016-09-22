using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;

namespace IM.Base.Helpers
{
  public class PrinterHelper
  {
    // Structure and API declarions:
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DOCINFOA
    {
      [MarshalAs(UnmanagedType.LPStr)]
      public string pDocName;
      [MarshalAs(UnmanagedType.LPStr)]
      public string pOutputFile;
      [MarshalAs(UnmanagedType.LPStr)]
      public string pDataType;
    }
    [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

    [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool ClosePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

    [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool EndDocPrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool StartPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool EndPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

    #region SetDefaultPrinter
    /// <summary>
    /// Asigna una impresora como predefinida a partir del nombre
    /// </summary>
    /// <param name="Name">Nombre de la impresora</param>
    /// <history>
    /// [emoguel] 22/09/2016 created
    /// </history>
    [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool SetDefaultPrinter(string Name); 
    #endregion

    // SendBytesToPrinter()
    // When the function is given a printer name and an unmanaged array
    // of bytes, the function sends those bytes to the print queue.
    // Returns true on success, false on failure.
    public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
    {
      Int32 dwError = 0, dwWritten = 0;
      IntPtr hPrinter = new IntPtr(0);
      DOCINFOA di = new DOCINFOA();
      bool bSuccess = false; // Assume failure unless you specifically succeed.

      di.pDocName = "My C#.NET RAW Document";
      di.pDataType = "RAW";

      // Open the printer.
      if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
      {
        // Start a document.
        if (StartDocPrinter(hPrinter, 1, di))
        {
          // Start a page.
          if (StartPagePrinter(hPrinter))
          {
            // Write your bytes.
            bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
            EndPagePrinter(hPrinter);
          }
          EndDocPrinter(hPrinter);
        }
        ClosePrinter(hPrinter);
      }
      // If you did not succeed, GetLastError may give more information
      // about why not.
      if (bSuccess == false)
      {
        dwError = Marshal.GetLastWin32Error();
      }
      return bSuccess;
    }

    public static bool SendFileToPrinter(string szPrinterName, string szFileName)
    {
      // Open the file.
      FileStream fs = new FileStream(szFileName, FileMode.Open);
      // Create a BinaryReader on the file.
      BinaryReader br = new BinaryReader(fs);
      // Dim an array of bytes big enough to hold the file's contents.
      Byte[] bytes = new Byte[fs.Length];
      bool bSuccess = false;
      // Your unmanaged pointer.
      IntPtr pUnmanagedBytes = new IntPtr(0);
      int nLength;

      nLength = Convert.ToInt32(fs.Length);
      // Read the contents of the file into the array.
      bytes = br.ReadBytes(nLength);
      // Allocate some unmanaged memory for those bytes.
      pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
      // Copy the managed byte array into the unmanaged array.
      Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
      // Send the unmanaged bytes to the printer.
      bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
      // Free the unmanaged memory that you allocated earlier.
      Marshal.FreeCoTaskMem(pUnmanagedBytes);
      return bSuccess;
    }
    public static bool SendStringToPrinter(string szString)
    {
      IntPtr pBytes;
      Int32 dwCount;
      IntPtr hPrinter = new IntPtr(0);

      var strPrintName = ConfigRegistryHelper.GetConfiguredPrinter("PrintMealTicket");

      if (string.IsNullOrEmpty(strPrintName))
      {
        UIHelper.ShowMessage("The meal ticket printer is not configured.");
        return false;
      }
      var lreturn = OpenPrinter(strPrintName, out hPrinter, new IntPtr());
      if (!lreturn)
      {
        UIHelper.ShowMessage("The Printer name you typed wasn't recognized.");
        return false;
      }
      // How many characters are in the string?
      dwCount = szString.Length;
      // Assume that the printer is expecting ANSI text, and then convert
      // the string to ANSI text.
      pBytes = Marshal.StringToCoTaskMemAnsi(szString);
      // Send the converted ANSI string to the printer.
      SendBytesToPrinter(strPrintName, pBytes, dwCount);
      Marshal.FreeCoTaskMem(pBytes);
      ClosePrinter(hPrinter);
      return true;
    }

    #region getAllPrinters
    /// <summary>
    /// Obtiene la lista de impresoras
    /// </summary>
    /// <returns>devuelve el nombre de la lista de impresoras</returns>
    /// <history>
    /// [emoguel] 22/09/2016 created
    /// </history>
    public static List<string> getAllPrinters()
    {
      List<string> printers = new List<string>();
      foreach (var item in PrinterSettings.InstalledPrinters)
      {
        printers.Add(item.ToString());
      }
      return printers;
    }
    #endregion

    #region GetDefaultPrinter
    /// <summary>
    /// Obtiene la impresora predefinida de la PC
    /// </summary>
    /// <returns>Nombre de la impresora predefinida, en caso de quie no se tenga predefinina alguna, devuelve string.empty</returns>
    /// <history>
    /// [emoguel] m22/09/2016 created
    /// </history>
    public static string GetDefaultPrinter()
    {
      PrinterSettings settings = new PrinterSettings();
      foreach (string printer in PrinterSettings.InstalledPrinters)
      {
        settings.PrinterName = printer;
        if (settings.IsDefaultPrinter)
          return printer;
      }
      return string.Empty;
    } 
    #endregion

  }
}
