using System.IO;

namespace IM.Base.Helpers
{
  public class SettingsHelper
  {
    public static string GetReportsPath()
    {
      string computerName = ComputerHelper.GetMachineName();
      string directory = $@"C:\Temp\Intelligence Marketing\{ComputerHelper.GetMachineName()}";
      if (!Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }
      return directory;
    }
  }
}
