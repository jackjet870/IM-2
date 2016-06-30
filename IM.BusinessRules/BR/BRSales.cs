using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRSales
  {
    #region GetSalesByPR

    /// <summary>
    /// Obtiene Guests por PR
    /// </summary>
    /// <param name="dateFrom">FechaInicio</param>
    /// <param name="dateTo">FechaFin</param>
    /// <param name="leadSources">Lead Source</param>
    /// <param name="PR">PR</param>
    /// <param name="searchBySalePR">True SalePR - False ContactsPR</param>
    /// <returns>List<SaleByPR></returns>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    public static List<SaleByPR> GetSalesByPR(DateTime dateFrom, DateTime dateTo, string leadSources, string PR, bool searchBySalePR)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetSalesByPR(dateFrom, dateTo, leadSources, PR, searchBySalePR).ToList();
      }
    }

    #endregion

    #region GetSalesByLiner
    /// <summary>
    /// Obtiene Guests por PR
    /// </summary>
    /// <param name="dateFrom">FechaInicio</param>
    /// <param name="dateTo">FechaFin</param>
    /// <param name="salesRooms">Sales Room</param>
    /// <param name="Liner">liner</param>
    /// <returns>List<SaleByLiner></returns>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    public static List<SaleByLiner> GetSalesByLiner(DateTime dateFrom, DateTime dateTo, string salesRooms, string Liner)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetSalesByLiner(dateFrom, dateTo, salesRooms, Liner).ToList();

      }
    }

   

    #endregion

    #region GetSalesByCloser
    /// <summary>
    /// Obtiene Guests por PR
    /// </summary>
    /// <param name="dateFrom">FechaInicio</param>
    /// <param name="dateTo">FechaFin</param>
    /// <param name="salesRooms">Sales Room</param>
    /// <param name="Closer">closer</param>
    /// <returns>List<SaleByCloser></returns>
    /// <history>
    /// [erosado] 24/Mar/2016 Created
    /// </history>
    public static List<SaleByCloser> GetSalesByCloser(DateTime dateFrom, DateTime dateTo, string salesRooms, string closer)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetSalesByCloser(dateFrom, dateTo, salesRooms, closer).ToList();

      }
    }

    #endregion

    #region GetSalesShort
    /// <summary>
    ///  Trae una lista de Guest para una vista 
    /// </summary>
    /// <param name="guest">huesped</param>
    /// <param name="sale">venta</param>
    /// <param name="membership">membresia</param>
    /// <param name="name">nombre </param>
    /// <param name="leadSource"></param>
    /// <param name="salesRoom"></param>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <history>
    /// [jorcanche]  creted 28062016
    /// </history>
    public static async Task<List<SaleShort>> GetSalesShort(int guest = 0, int sale = 0, string membership = "ALL", string name = "ALL", string leadSource = "ALL", string salesRoom = "ALL", DateTime? dateFrom = null, DateTime? dateTo = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetSales_Timeout;
          return
            dbContext.USP_OR_GetSales(guest, sale, membership, name, leadSource, salesRoom, dateFrom, dateTo).ToList();
        }
      });
    }
    #endregion

    #region GetSalesCloseD
    /// <summary>
    /// Obtiene la fecha de cierre de las ventas
    /// </summary>
    /// <param name="srId">ID Sales Room</param>
    /// <returns>La fecha dse cierre DateTime</returns>
    /// <hitory>
    /// [jorcanche] 20/05/2016
    /// </hitory>
    public static async Task<DateTime> GetSalesCloseD(string srId)
    {
      return await Task.Run(() =>
      {
        using (var dbCOntext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return (from gu in dbCOntext.SalesRooms where gu.srID == srId select gu.srSalesCloseD).FirstOrDefault();
        }
      });
    }
    #endregion

    #region GetSalesbyID

    /// <summary>
    /// Obtiene la venta por ID
    /// </summary>   
    /// <param name="saId">Id sales room</param>
    /// <param name="memebershipNum">numero de membresia</param>
    /// <returns>La fecha dse cierre DateTime</returns>
    /// <hitory>
    /// [jorcanche] 20/05/2016
    /// </hitory>
    public static async Task<Sale> GetSalesbyId(int saId = 0, string memebershipNum = "")
    {
      var sale = new Sale();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from gu in dbContext.Sales select gu;
          if (!string.IsNullOrEmpty(memebershipNum))
            return sale = (query.Where(x => x.saMembershipNum == memebershipNum)).FirstOrDefault();

          return sale = (query.Where(x => x.saID == saId)).FirstOrDefault();
        }
      });
      return sale;
    }
    #endregion

    #region GetSalesTypes

    /// <summary>
    /// Obtiene los Tipos de ventas activos por ID
    /// </summary>
    /// <param name="stId"></param>
    /// <returns>Una lista de Tipo de ventas</returns>
    /// <hitory>
    /// [jorcanche] 20/05/2016
    /// </hitory>
    public static List<SaleType> GetSalesTypes(string stId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from gu in dbContext.SaleTypes
                where gu.stA == true && gu.stID == stId
                select gu).ToList();
      }
    }
    #endregion

    #region GetCoutSalesbyGuest
    /// <summary>
    /// Retorna el total de Sales por Guest
    /// </summary>
    /// <param name="sagu">Id del Guest</param>
    /// <history>
    /// [jorcanche] created 30062016
    /// </history>    
    public static async Task<int> GetCoutSalesbyGuest(int? sagu)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return (from s in dbContext.Sales
                  where s.sagu == sagu
                  select s).Count();
        }
      });
    }
    #endregion   

    #region UpdateSaleUpdated
    /// <summary>
    ///  Marca o desmarca una venta como actualizada
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="updated"></param>
    /// <history>
    /// [jorcanche]  creted 28062016
    /// </history>
    public static async Task<int> UpdateSaleUpdated(int? saleId, bool updated)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_UpdateSaleUpdated(saleId, updated);
        }
      });
    }
    #endregion

    #region UpdateGuestSale
    /// <summary>
    /// Marca como venta el Guest
    /// </summary>
    /// <param name="guestId">Id Guest </param>
    /// <param name="sale">true se marca como venta false nose marca como venta</param>
    /// <history>
    /// [jorcanche]  created 01072016
    /// </history>
    public static async Task<int> UpdateGuestSale(int? guestId, bool sale)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_UpdateGuestSale(guestId, sale);
        }
      });
    }
    #endregion

    #region SaveChangedSale
    /// <summary>
    /// Guarda los cambios de un Sale
    /// </summary>
    /// <param name="sale"></param>
    /// <history>
    /// [jorcanche]  creted 28062016
    /// </history>
    public static int SaveChangedSale(Sale sale)
    {  
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var personel = dbContext.Personnels.First();
        sale.Personnel_LinerCaptain1 = personel;
        dbContext.Entry(sale).State = System.Data.Entity.EntityState.Modified;
        return dbContext.SaveChanges();
      }
    }
    #endregion

    #region SaveSaleLog
    /// <summary>
    ///Guarda el Log del Sale 
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="hoursDif"></param>
    /// <param name="changedBy"></param>
    /// <history>
    /// [jorcanche]  created 01072016
    /// </history>
    public static void SaveSaleLog(int? sale, short hoursDif, string changedBy)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_SaveSaleLog(sale, hoursDif, changedBy);
      }
    }
    #endregion

    #region LogSale
    /// <summary>
    /// Retorna el log por sale 
    /// </summary>
    /// <param name="sale">Indentificador del sale</param>
    /// <history>
    /// [jorcanche] 16062016 created 
    /// </history>
    public static async Task<List<SaleLogData>> GetSaleLog(int sale)
    {
      var resul = new List<SaleLogData>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          resul = dbContext.USP_OR_GetSaleLog(sale).ToList();
        }
      });
      return resul;
    }

    #endregion

    #region DeleteSale
    /// <summary>
    /// Elimima un Sale por su ID
    /// </summary>
    /// <param name="saleId"> Id del Sale</param>
    /// <history>
    /// [jorcanche] created 01072016
    /// </history>
    public static async Task<int> DeleteSale(int saleId)
    {
      int res = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          res = dbContext.USP_OR_DeleteSale(saleId);
        }
      });
      return res;
    }
    #endregion

    #region ValidateSale
    /// <summary>
    /// Valida si todos los parametros no esten vacios y que existan en la Base de Datos
    /// </summary>
    /// <param name="changedBy">Modificado por</param>
    /// <param name="password">Contraseña</param>
    /// <param name="sale">Venta</param>
    /// <param name="membershipNumber">Membresia</param>
    /// <param name="guest">Huesped</param>
    /// <param name="saleType">Typo de venta</param>
    /// <param name="salesRoom">Cuarto de venta </param>
    /// <param name="location">Lugar</param>
    /// <param name="pR1"> PR 1</param>
    /// <param name="pR2">PR 2</param>
    /// <param name="pR3">PR 3</param>
    /// <param name="pRCaptain1">CAPITAN PR 1</param>
    /// <param name="pRCaptain2">CAPITAN PR 2</param>
    /// <param name="pRCaptain3">CAPITAN PR 3</param>
    /// <param name="liner1">LINER 1</param>
    /// <param name="liner2">LINER 2</param>
    /// <param name="linerCaptain">CAPITAN LINER 1</param>
    /// <param name="closer1">CLOSER 1</param>
    /// <param name="closer2">CLOSER 2</param>
    /// <param name="closer3">CLOSER 3</param>
    /// <param name="closerCaptain">CAPITAN CLOSER </param>
    /// <param name="exit1">EXIT 1</param>
    /// <param name="exit2">EXIT2</param>
    /// <param name="podium">PODIUM</param>
    /// <param name="vLO">VLO</param>
    /// <history>
    /// [jorcanche]  creted 28062016
    /// </history>
    public static List<ValidationData> ValidateSale(string changedBy, string password, Nullable<int> sale, string membershipNumber, Nullable<int> guest, string saleType, string salesRoom, string location, string pR1, string pR2, string pR3, string pRCaptain1, string pRCaptain2, string pRCaptain3, string liner1, string liner2, string linerCaptain, string closer1, string closer2, string closer3, string closerCaptain, string exit1, string exit2, string podium, string vLO)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_ValidateSale(changedBy, password, sale, membershipNumber, guest, saleType, salesRoom, location, pR1, pR2, pR3, pRCaptain1, pRCaptain2, pRCaptain3, liner1, liner2, linerCaptain, closer1, closer2, closer3, closerCaptain, exit1, exit2, podium, vLO).ToList();
      }
    }
    #endregion
  }
}
