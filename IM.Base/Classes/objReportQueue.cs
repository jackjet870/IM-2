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

    private string _fileName=null;

    public string FileName
    {
      get { return _fileName; }
      set { SetField(ref _fileName, value); }
    }

    private bool _exists;
    public bool Exists
    {
      get { return _exists; }
      set { SetField(ref _exists, value); }
    }
  }
}