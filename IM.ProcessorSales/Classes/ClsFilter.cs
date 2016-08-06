using IM.Model.Enums;
using System;
using System.Collections.Generic;
using IM.Model;

namespace IM.ProcessorSales.Classes
{
  public class ClsFilter
  {
    public DateTime DtmStart { get; set; }
    public DateTime DtmEnd { get; set; }

    public PersonnelShort Salesman { get; set; }
    public List<string> LstSalesRooms { get; set; } = new List<string>();
    public List<string> LstSegments { get; set; } = new List<string>();
    public List<MultiDateHelpper> LstMultiDate { get; set; } = new List<MultiDateHelpper>();
    public List<GoalsHelpper> LstGoals { get; set; } = new List<GoalsHelpper>();
    public List<EfficiencyData> lstEfficiency { get; set; } = new List<EfficiencyData>();

    public bool BlnAllSalesRooms { get; set; }

    public bool BlnAllSegments { get; set; }

    public List<EnumRole> LstEnumRole { get; set; } = new List<EnumRole>();

    public bool BlnGroupedByTeams { get; set; }
    public bool BlnIncludeAllSalesmen { get; set; }

    public EnumPredefinedDate CboDateSelected { get; set; }
    public EnumProgram EnumProgram { get; set; }

    public decimal Goal { get; set; }
    
  }
}