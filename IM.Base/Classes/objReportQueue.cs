using IM.Model.Classes;
using System.IO;

namespace IM.Base.Classes
{
  internal class objReportQueue : EntityBase
  {
    public string Id { get; set; }

    private string _reportName;

    public string ReportName
    {
      get { return _reportName; }

      set { SetField(ref _reportName, value); }
    }

    private FileInfo _fileInfo;

    public FileInfo FileInfo
    {
      get { return _fileInfo; }
      set { SetField(ref _fileInfo, value); }
    }

    private bool _exists;
    public bool Exists
    {
      get { return _exists; }
      set { SetField(ref _exists, value); }
    }
  }
}