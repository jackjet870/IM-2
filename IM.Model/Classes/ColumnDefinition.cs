namespace IM.Model.Classes
{
  public class ColumnDefinition
  {
    public string column { get; set; }
    public string type { get; set; }
    public byte precision { get; set; }
    public byte scale { get; set; }
    public int maxLength { get; set; }
    public string defaultValue { get; set; }
    public string nullable { get; set; }
    public string description { get; set;}
  }
}
