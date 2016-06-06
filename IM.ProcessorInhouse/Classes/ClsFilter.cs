using System;
using IM.Model.Enums;
using System.Collections.Generic;
using IM.Model;

namespace IM.ProcessorInhouse.Classes
{
  public class ClsFilter
  {
    public DateTime DtmInit { get; set; }
    public DateTime DtmStart { get; set; }
    public DateTime DtmEnd { get; set; }

    public List<string> LstPersonnel { get; set; } = new List<string>();
    public List<string> LstLeadSources { get; set; } = new List<string>();
    public List<string> LstMarkets { get; set; } = new List<string>();
    public List<string> LstAgencies { get; set; } = new List<string>();
    public List<string> LstGifts { get; set; } = new List<string>();
    public List<string> LstChargeTo { get; set; } = new List<string>();
    public Dictionary<string, int> LstGiftsQuantity { get; set; } = new Dictionary<string, int>();

    public bool BlnAllPersonnel { get; set; }
    public bool BlnAllLeadSources { get; set; }
    public bool BlnAllMarkets { get; set; }
    public bool BlnAllAgencies { get; set; }
    public bool BlnAllGifts { get; set; }
    public bool BlnAllChargeTo { get; set; }

    public EnumPredefinedDate CboDateSelected { get; set; }
    public EnumQuinellas EnumQuinellas { get; set; }
    public EnumBasedOnArrival EnumBasedOnArrival { get; set; }
    public EnumDetailGifts EnumDetailGifts { get; set; }
    public EnumSalesByMemberShipType EnumSalesByMemberShipType { get; set; }
    public EnumSaveCourtesyTours EnumSaveCourtesyTours { get; set; }
    public EnumExternalInvitation EnumExternalInvitation { get; set; }
    public EnumBasedOnPRLocation EnumBasedOnPRLocation { get; set; }
    public EnumOnlyWholesalers EnumOnlyWholesalers { get; set; }

    public string StrApplication { get; set; }
    public int IntCompany { get; set; }
    public Club Club { get; set; }

    public int IntStartN { get; set; }
    public int IntEndN { get; set; }

  }
}