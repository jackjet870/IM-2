using IM.Model;

namespace IM.ProcessorInhouse.Classes
{
  internal class GiftQuantity : GiftShort
  {
    public int quantity { get; set; }

    public bool include { get; set; }
  }
}