using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace IM.Base.Classes
{
  internal class objReportQueue : INotifyPropertyChanged
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

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value)) return;
      field = value;
      OnPropertyChanged(propertyName);
    }
  }
}