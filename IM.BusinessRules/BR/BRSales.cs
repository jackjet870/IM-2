using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Classes;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;

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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
    /// [erosado] 05/072016 Se agregó Async
    /// </history>
    public async static Task<List<SaleByLiner>> GetSalesByLiner(DateTime dateFrom, DateTime dateTo, string salesRooms, string Liner)
    {
      List<SaleByLiner> result = new List<SaleByLiner>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          result = dbContext.USP_OR_GetSalesByLiner(dateFrom, dateTo, salesRooms, Liner).ToList();
        }
      });
      return result;
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
    /// [erosado] 04/07/2016  Se agregó Async.
    /// </history>
    public async static Task<List<SaleByCloser>> GetSalesByCloser(DateTime dateFrom, DateTime dateTo, string salesRooms, string closer)
    {
      List<SaleByCloser> result = new List<SaleByCloser>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          result = dbContext.USP_OR_GetSalesByCloser(dateFrom, dateTo, salesRooms, closer).ToList();
        }
      });
      return result;
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
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
        using (var dbCOntext = new IMEntities(ConnectionHelper.ConnectionString()))
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
    public static async Task<Sale> GetSalesbyId(int? saId = 0, string memebershipNum = "")
    {
      try
      {
        return await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var query = (from gu in dbContext.Sales where gu.saID == saId select gu).FirstOrDefault();
            //if (!string.IsNullOrEmpty(memebershipNum))
            //  return (query.Where(x => x.saMembershipNum == memebershipNum)).FirstOrDefault();

            //return (query.Where(x => x.saID == saId)).FirstOrDefault();
            return query;
          }
        });
      }
      catch (Exception)
      {

        throw;
      }
    }
    #endregion

    #region GetMembershipType

    /// <summary>
    ///Obtiene el tipo de una membresia
    /// </summary>   
    /// <param name="memebershipNum">numero de membresia</param>
    /// <returns>La fecha dse cierre DateTime</returns>
    /// <hitory>
    /// [jorcanche] 20/05/2016
    /// </hitory>
    public static async Task<string> GetMembershipType(string memebershipNum = "")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return (from gu in dbContext.Sales where gu.saMembershipNum == memebershipNum select gu.samtGlobal).FirstOrDefault();
        }
      });
    }
    #endregion

    #region GetSalesTypes

    /// <summary>
    /// Obtiene los Tipos de ventas activos por ID
    /// </summary>
    /// <param name="stId"></param>
    /// <returns>Una lista de Tipo de ventas</returns>
    /// <history>
    /// [jorcanche] 20/05/2016
    /// </history>
    public static async Task<List<SaleType>> GetSalesTypes(string stId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return (from gu in dbContext.SaleTypes
                  where gu.stA && gu.stID == stId
                  select gu).ToList();
        }
      });
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
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
    public static async Task<int> SaveChangedSale(Sale sale)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var personel = dbContext.Personnels.First();
          sale.Personnel_LinerCaptain1 = personel;
          dbContext.Entry(sale).State = System.Data.Entity.EntityState.Modified;
          return dbContext.SaveChanges();
        }
      });
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
    public static async Task<int> SaveSaleLog(int? sale, short hoursDif, string changedBy)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_SaveSaleLog(sale, hoursDif, changedBy);
        }
      });
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
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          return dbContext.USP_OR_DeleteSale(saleId);
      });
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
    public static async Task<ValidationData> ValidateSale(string changedBy, string password, int? sale, string membershipNumber, int? guest, string saleType, string salesRoom, string location, string pR1, string pR2, string pR3, string pRCaptain1, string pRCaptain2, string pRCaptain3, string liner1, string liner2, string linerCaptain, string closer1, string closer2, string closer3, string closerCaptain, string exit1, string exit2, string podium, string vLO)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var algo =
            dbContext.USP_OR_ValidateSale(changedBy, password, sale, membershipNumber, guest, saleType, salesRoom,
              location, pR1, pR2, pR3, pRCaptain1, pRCaptain2, pRCaptain3, liner1, liner2, linerCaptain, closer1,
              closer2, closer3, closerCaptain, exit1, exit2, podium, vLO).FirstOrDefault();
          return algo;

        }
      });
    }
    #endregion

    #region SaveSale

    /// <summary>
    /// Guarda un Sale con asincronia y con transaction
    /// </summary>
    /// <param name="saleOld">Sale Original antes de que se modificará</param>
    /// <param name="saleNew">Sale despues de que se modificará</param>
    /// <param name="obpayments">Observable Collection de tipo Payments</param>
    /// <param name="isEnabledsaRefMember">Para indicar como encuentra habilitado en txtsaRefMember.enabled </param>
    /// <param name="hoursDifSalesRoom">La direfencia de horas del Sales Room</param>
    /// <param name="user">Usuario que hizo el cambio</param>
    /// <param name="saleAmount">El monto de la venta</param>
    /// <param name="lstSalesSalesman">Listado de los SalesSalesmen que se modificaron</param>
    /// <param name="saleAmountOriginal">El monto de la venta original</param>
    /// <param name="ipMachine">Ip de la maquina en el que se hizo el cambio</param>
    /// <param name="lstSalesSalesmenChanges">Listadoque SalesSalesmen que se remplazaron</param>
    /// <param name="authorizedBy">Quien autorizo los cambios</param>
    /// <param name="isOnlySaveSalesSalesmen">Cuando este parametro esta en "True" indica que solo se guardara los SalesSalesmens</param>
    /// <history>
    /// [jorcanche]  created 02/ago/2016 
    /// </history>
    public static async Task<int> SaveSale(Sale saleOld, Sale saleNew, ObservableCollection<Payment> obpayments, bool isEnabledsaRefMember,
                                            short hoursDifSalesRoom, string user, decimal saleAmount,
                                            IEnumerable<SalesSalesman> lstSalesSalesman, decimal saleAmountOriginal, string ipMachine,
                                            IEnumerable<SalesmenChanges> lstSalesSalesmenChanges, string authorizedBy, bool isOnlySaveSalesSalesmen = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              //Si el parametro isOnlySaveSalesSalesmen esta en "True" No permitira hacer ningun movimiento mas que el de 
              //"Guardo de los movimientos de los SalesSalesmen"
              if (!isOnlySaveSalesSalesmen)
              {
                #region   Guardamos el Sale
                //*****************************************************************************************************
                //                                      Guardamos el Sale
                //*****************************************************************************************************
                //1.- Guardamos cambios en el Sale si hubo cambios
                if (!ObjectHelper.IsEquals(saleNew, saleOld))
                {
                  dbContext.Entry(saleNew).State = EntityState.Modified;
                }
                #endregion

                #region Guardamos los pagos
                //*****************************************************************************************************
                //                                      Guardamos los pagos
                //*****************************************************************************************************
                //2.- Eliminamos uno o mas registros que contengan el Id Sale en la tabla Payments
                //2.1.-  Obtenemos los Payments que estan actualmente en la base con este Sale 
                var lstPayments = dbContext.Payments.Where(p => p.pasa == saleNew.saID);
                //2.2 Si hubo Payments los eliminamos 
                if (lstPayments.Any())
                {
                  dbContext.Payments.RemoveRange(lstPayments);
                }
                //2.3 Guardamos los nuevos Payments.               
                foreach (var payment in obpayments)
                {
                  dbContext.Entry(payment).State = EntityState.Added;
                }
                #endregion

                #region  Marcamos las ventas como actualizadas 
                //*****************************************************************************************************
                //                                   Marcamos las ventas como actualizada  
                //*****************************************************************************************************
                //3.-Si hubo cambio con la anterior venta se procede
                if (isEnabledsaRefMember && saleNew.saReference != saleOld.saReference)
                {
                  //3.1.- si tenia venta anterior, la marcamos como actualizada
                  if (saleOld.saReference != null)
                  {
                    //3.2.- marcamos la venta anterior actual como actualizada
                    dbContext.USP_OR_UpdateSaleUpdated(saleOld.saReference, false);
                  }
                  //3.3.- actualizamos los vendedores del huesped en base a los vendedores de la venta 
                  dbContext.USP_OR_UpdateSaleUpdated(saleNew.saReference, true);
                }
                #endregion

                #region  Actualizamos los vendedores del huesped en base a los vendedores de la venta 
                //*****************************************************************************************************
                //          Actualizamos los vendedores del huesped en base a los vendedores de la venta 
                //*****************************************************************************************************
                //4.- Actualizamos
                dbContext.USP_OR_UpdateGuestSalesmen(saleNew.sagu, saleNew.saID);
                #endregion

                #region Si cambio de Guest ID realizamos lo cambios
                //*****************************************************************************************************
                //                              Si cambio de Guest ID realizamos lo cambios
                //*****************************************************************************************************
                //5.- Si hubo cambio en el sagu
                if (saleOld.sagu != saleNew.sagu)
                {
                  //5.1.- Marcamos como venta el guest Id Nuevo
                  dbContext.USP_OR_UpdateGuestSale(saleNew.sagu, true);

                  //5.2.- Desmarcamos como venta el Guest Id anterior si ya no le quedan ventas                
                  if (!(from s in dbContext.Sales where s.sagu == saleNew.sagu select s).Any())
                  {
                    dbContext.USP_OR_UpdateGuestSale(saleOld.sagu, false);
                  }
                }
                #endregion

             
              }

              #region Guarda los movimientos de los SalesSalesmen
              //*****************************************************************************************************
              //                             Guarda los movimientos de los SalesSalesmen
              //*****************************************************************************************************
              //7.1.-Extraemos el Listado de los SaleMan que se modificaron 
              //7.2.-Se elimina la propiedad virtual Persaonel para que no marque error de repeticion de llaves,
              // ya que personel tiene la llave de peID y la llave de la tabla SalesSaleman tiene igual la llave smpe y marcan conflicto
              var lstSalesSalemanAdd = new List<SalesSalesman>();
              lstSalesSalesman.Where(ss => !ss.smSale || ss.smSaleAmountOwn != saleAmount || ss.smSaleAmountWith != saleAmount).
                ToList().ForEach(x =>
                {
                  var ss = new SalesSalesman();
                  ObjectHelper.CopyProperties(ss, x);
                  lstSalesSalemanAdd.Add(ss);
                });
              //7.3.- Eliminamos todos los registros de la tabla SalesSalesmen que sean de este sale
              var lstSalesSalesmanDelete = dbContext.SalesSalesmen.Where(salesSalesman => salesSalesman.smsa == saleNew.saID);
              if (lstSalesSalesmanDelete.Any())
              {
                dbContext.SalesSalesmen.RemoveRange(lstSalesSalesmanDelete);
              }

              //7.4.- Se guardan los SalesSaleman que se modificaron       
              foreach (var salesSalesSaleman in lstSalesSalemanAdd)
              {
                dbContext.Entry(salesSalesSaleman).State = EntityState.Added;
              }
              #endregion

              //Si el parametro isOnlySaveSalesSalesmen esta en "True" No permitira hacer ningun movimiento mas que el de 
              //"Guardo de los movimientos de los SalesSalesmen"
              if (!isOnlySaveSalesSalesmen)
              {
                #region Guardamos el movimiento de venta del huesped
                //*****************************************************************************************************
                //                             Guardamos el movimiento de venta del huesped
                //*****************************************************************************************************

                //8.- Guardamos
                dbContext.USP_OR_SaveGuestMovement(saleNew.sagu, EnumToListHelper.GetEnumDescription(EnumGuestsMovementsType.Sale),
                                                  user, Environment.MachineName, ipMachine);
                #endregion

                #region Guardamos los cambios de vendedores y la persona que autorizo los cambios
                //*****************************************************************************************************
                //               Guardamos los cambios de vendedores y la persona que autorizo los cambios
                //*****************************************************************************************************
                //9.- Si No se autorizo no hacemos ningun cambio
                if (!string.IsNullOrEmpty(authorizedBy))
                {
                  //9.1.- Guardamos cambios
                  foreach (var salesmenChange in lstSalesSalesmenChanges)
                  {
                    dbContext.USP_OR_SaveSalesmenChanges
                       (saleNew.saID, authorizedBy, user, salesmenChange.roN, salesmenChange.schPosition,
                        salesmenChange.schOldSalesman, salesmenChange.schNewSalesman,null,"SL");
                  }
                }

                #endregion
              }

              //Si no hubo ningun problema guardamos cambios 
              var respuesta = dbContext.SaveChanges();

              if (!isOnlySaveSalesSalesmen)
              {
                #region Guardamos el historico de la venta
                //*****************************************************************************************************
                //                               Guardamos el historico de la venta
                //*****************************************************************************************************
                //6.1.- Guardamos SaleLog
                dbContext.USP_OR_SaveSaleLog(saleNew.sagu, hoursDifSalesRoom, user);

                #endregion
              }

              //Confirmamos la transaccion
              transaction.Commit();
              return respuesta;
            }
            catch (Exception)
            {
              //De lo contrario mandamos el mensaje de error en la interfaz y realizamos un Rollback 
              transaction.Rollback();
              throw;
            }
          }
        }
      });
    }
    #endregion

  }
}
