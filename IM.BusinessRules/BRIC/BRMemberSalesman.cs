using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Collections.ObjectModel;
using IM.Model.Classes;

namespace IM.BusinessRules.BRIC
{
  /// <summary>
  /// Clase para el manejor de los Salesman de la base Hotel(Intelligence Contracts)
  /// </summary>
  /// <history>
  /// [jorcanche]  created 08/07/2016
  /// </history>
  public class BRMemberSalesman
  {
    #region GetMemberSalesmen

    /// <summary>
    /// Devuelve la lista MemberSalesmen
    /// </summary>    
    /// <returns>Lista de tipo MemberSalesmen</returns>
    /// <history>
    /// [jorcanche] created 08/07/2016
    /// </history>
    public static async Task<List<MemberSalesmen>> GetMemberSalesmen(string application, string job, string code, string roles)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new ICEntities(ConnectionHelper.ConnectionString(EnumDatabase.IntelligenceContracts)))
        {
          return dbContext.USP_CL_GetMemberSalesmen(application, job, code, roles).ToList();
        }
      });
    }

    #endregion

    #region SaveMemberSalesman

    /// <summary>
    /// Obtiene el resultado en tipo de  Salesman cuando se guarda est
    /// </summary>
    /// <param name="rEcnum"></param>
    /// <param name="memberSalesmen"></param>
    /// <param name="user"></param>
    /// <history>
    /// [jorcanche]  created 08/07/2016
    /// </history>        
    public static async Task<MemberSalesmen> SaveMemberSalesman(decimal? reccum ,MemberSalesmen memberSalesmen, string user)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new ICEntities(ConnectionHelper.ConnectionString(EnumDatabase.IntelligenceContracts)))
        {
          return dbContext.USP_CL_SaveMemberSalesman(
            reccum, memberSalesmen.CLMEMOPC_ID, memberSalesmen.APPLICATION, memberSalesmen.CLAOPC_ID, memberSalesmen.OPC1,
            memberSalesmen.OPC_PCT, memberSalesmen.OPC_PCT2, memberSalesmen.OPC_CPT3, memberSalesmen.OPC_PCT4, memberSalesmen.STATUS,
            memberSalesmen.ZONA, memberSalesmen.OPC, user)
            .FirstOrDefault();
        }
      });
    }
    #endregion

    #region DeleteMemberSalesman
    /// <summary>
    /// Elimina MemberSalesmen
    /// </summary>        
    /// <history>
    /// [jorcanche] created 08/07/2016
    /// </history>
    public static async Task<int> DeleteMemberSalesman(decimal? cLmemopcId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new ICEntities(ConnectionHelper.ConnectionString(EnumDatabase.IntelligenceContracts)))
        {
          return dbContext.USP_CL_DeleteMemberSalesman(cLmemopcId);
        }
      });
    }
    #endregion

    #region ExistsSalesman
    /// <summary>
    /// Indica si existe un Salesman
    /// </summary>
    /// <param name="zOna"></param>
    /// <param name="cOde"></param>
    /// <history>
    /// [jorcanche] created 08/07/2016
    /// </history>
    public static async Task<bool> ExistsSalesman(string zOna, string cOde)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new ICEntities(ConnectionHelper.ConnectionString(EnumDatabase.IntelligenceContracts)))
        {
          var aa = dbContext.USP_CL_ExistsSalesman(zOna, cOde).FirstOrDefault() == 1;
          return aa;
        }
      });
    }
    #endregion

    #region ExistsMembershipNum
    /// <summary>
    /// Devuelve un  boleano indicando si existe o no la membresia que se mando como parametro en la base de Intelligence Contracts
    /// </summary>
    /// <param name="membershipNum">Numero de membresia</param>
    /// <history>
    /// [jorcanche] created 01/ago/2016
    /// </history>
    public static async Task<bool> ExistsMembershipNum(string membershipNum)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new ICEntities(ConnectionHelper.ConnectionString(EnumDatabase.IntelligenceContracts)))
        {
          return dbContext.Members.Any(me => me.APPLICATION == membershipNum);
        }
      });
    }
    #endregion


    #region SaveMemberSalesmenClubes
    /// <summary>
    /// Guarda los Salesmen  en la base de datos de Intelligence Contracts
    /// </summary>
    /// <param name="saleNew">Sale que se modifico</param>
    /// <param name="user">Usuario que realizo los cambios</param>
    /// <history>
    /// [jorcanche]  created 04/ago/2016
    /// </history>
    public static async Task<List<string>> SaveMemberSalesmenClubes(Sale saleNew, string user)
    {
      return await Task.Run(() =>
      {
        using (var dbContextIC = new ICEntities(ConnectionHelper.ConnectionString(EnumDatabase.IntelligenceContracts)))
        {
          using (var dbContextIM = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            using (var transaction = dbContextIC.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
              try
              {
                string sPRs = string.Empty, sLiners = string.Empty, sClosers = string.Empty, sExits = string.Empty;

                var mensaje = new List<string>();

                //Obtenemos los vendedores de Intelligence Contracts 
                List<MemberSalesmen> lstMemberSalesmens = dbContextIC.USP_CL_GetMemberSalesmen(saleNew.saMembershipNum, "ALL", "ALL", "OPC,LINER,CLOSER,EXIT").ToList();

                //Agregamos  
                List<MemberSalesmanClubles> lstMemberSalesmenClubes = new List<MemberSalesmanClubles>();
                //PR's
                lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "OPC", Job = "OPC", Id = saleNew.saPR1 });
                lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "OPC", Job = "OPC", Id = saleNew.saPR2 });
                lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "OPC", Job = "OPC", Id = saleNew.saPR3 });
                //Liners
                lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "LINER", Job = "LIN", Id = saleNew.saLiner1 });
                lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "LINER", Job = "LIN", Id = saleNew.saLiner2 });
                //Closers
                lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "CLOSER", Job = "CLOS", Id = saleNew.saCloser1 });
                lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "CLOSER", Job = "CLOS", Id = saleNew.saCloser2 });
                lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "CLOSER", Job = "CLOS", Id = saleNew.saCloser3 });
                //Exit Closers
                lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "EXIT", Job = "JR", Id = saleNew.saExit1 });
                lstMemberSalesmenClubes.Add(new MemberSalesmanClubles { Role = "EXIT", Job = "JR", Id = saleNew.saExit1 });

                // Guarda un vendedor por rol en Intelligence Contracts
                foreach (var mbc in lstMemberSalesmenClubes)
                {
                  //si tiene una clave de vendedor de origos
                  if (!string.IsNullOrEmpty(mbc.Id))
                  {
                    //agregamos la clave de vendedor en IM
                    switch (mbc.Role)
                    {
                      case "OPC":
                        sPRs = sPRs + mbc.Id + " ";
                        break;
                      case "LINER":
                        sLiners = sLiners + mbc.Id + " ";
                        break;
                      case "CLOSER":
                        sClosers = sClosers + mbc.Id + " ";
                        break;
                      case "EXIT":
                        sExits = sExits + mbc.Id + " ";
                        break;
                    }

                    //obtenemos la clave del vendedor de Intelligence Contracts relaionando al vendedor de IM
                    var personnel = dbContextIM.Personnels.FirstOrDefault(p => p.peID == mbc.Id);
                    if (personnel == null)
                    {
                      mensaje.Add($"The personnel {mbc.Id} is not associated with a Intelligence Contract's salesman.");
                      break;
                    }

                    //Validamos si el vendedor de origos tiene relacionada una clave de Intelligence Contracts
                    var salemanId = personnel.peSalesmanID;
                    if (string.IsNullOrEmpty(salemanId))
                    {
                      mensaje.Add($"The personnel {mbc.Id} is not associated with a Intelligence Contract's salesman.");
                      break;
                    }

                    //Validamos si existe la clave de intelligence Contracts
                    var zone = saleNew.sasr;
                    if (!(dbContextIC.USP_CL_ExistsSalesman(zone, salemanId).FirstOrDefault() == 1))
                    {
                      mensaje.Add($"The salesman  {salemanId} from zone {zone} does not exists on Intelligence Contracts");
                      break;
                    }

                    //Localizamos el vendedor de Intelligence contracts
                    var member = lstMemberSalesmens.FirstOrDefault(sa => sa.OPC == salemanId && sa.CLAOPC_ID == mbc.Job);

                    //si no se localizo 
                    if (member == null)
                    {
                      //lo agregamos en Intelligence contracts
                      var memberSalesmen = new MemberSalesmen
                      {
                        CLMEMOPC_ID = 0,
                        APPLICATION = saleNew.saMembershipNum,
                        CLAOPC_ID = mbc.Job,
                        OPC1 = string.Empty,
                        OPC_PCT = 0,
                        OPC_PCT2 = 0,
                        OPC_CPT3 = 0,
                        OPC_PCT4 = 0,
                        STATUS = "A",
                        ZONA = zone,
                        OPC = salemanId
                      };
                      lstMemberSalesmens.Add(memberSalesmen);

                      //Guarda un vendedor de una afiliacion en Intelligence Contracts                   
                     dbContextIC.USP_CL_SaveMemberSalesman(0, memberSalesmen.CLMEMOPC_ID, memberSalesmen.APPLICATION,
                     memberSalesmen.CLAOPC_ID, memberSalesmen.OPC1, memberSalesmen.OPC_PCT, memberSalesmen.OPC_PCT2,
                     memberSalesmen.OPC_CPT3, memberSalesmen.OPC_PCT4, memberSalesmen.STATUS, memberSalesmen.ZONA,
                     memberSalesmen.OPC, user);
                      
                    }
                    else
                    {
                      // si tiene el rol solicitado
                      if (member.Role == mbc.Role)
                      {
                        member.CLAOPC_ID = mbc.Job;
                        member.STATUS = "A";
                        member.ZONA = saleNew.sasr;

                        //Actualizamos el vendedor en Intelligence Contracts                        
                        dbContextIC.USP_CL_SaveMemberSalesman(
                        member.RECNUM, member.CLMEMOPC_ID, member.APPLICATION, member.CLAOPC_ID, member.OPC1,
                        member.OPC_PCT, member.OPC_PCT2, member.OPC_CPT3, member.OPC_PCT4, member.STATUS,
                        member.ZONA, member.OPC, user);

                      }
                      else //Si no tiene el rol solicitad o
                      {
                        //Agregamos en Intelligence contracts
                        var memberSalesmen = new MemberSalesmen
                        {
                          CLMEMOPC_ID = 0,
                          APPLICATION = saleNew.saMembershipNum,
                          CLAOPC_ID = mbc.Job,
                          OPC1 = string.Empty,
                          OPC_PCT = 0,
                          OPC_PCT2 = 0,
                          OPC_CPT3 = 0,
                          OPC_PCT4 = 0,
                          STATUS = "A",
                          ZONA = zone,
                          OPC = salemanId
                        };
                        lstMemberSalesmens.Add(memberSalesmen);

                        //Agregamos el vendedor en intelligence Contracts                      
                        dbContextIC.USP_CL_SaveMemberSalesman(0, memberSalesmen.CLMEMOPC_ID, memberSalesmen.APPLICATION,
                        memberSalesmen.CLAOPC_ID, memberSalesmen.OPC1, memberSalesmen.OPC_PCT, memberSalesmen.OPC_PCT2,
                        memberSalesmen.OPC_CPT3, memberSalesmen.OPC_PCT4, memberSalesmen.STATUS, memberSalesmen.ZONA, 
                        memberSalesmen.OPC, user);
                      }
                    }
                  }
                }

                //Elimina los vendedores de una afiliacion en Intelligence Contracts si no estan en Intelligence marketing
                List<MemberSalesmanClubles> lstDeleteSalesmenClubes = new List<MemberSalesmanClubles>();
                lstDeleteSalesmenClubes.Add(new MemberSalesmanClubles { Role = "OPC", Salesmen = sPRs });
                lstDeleteSalesmenClubes.Add(new MemberSalesmanClubles { Role = "LINER", Salesmen = sLiners });
                lstDeleteSalesmenClubes.Add(new MemberSalesmanClubles { Role = "CLOSER", Salesmen = sClosers });
                lstDeleteSalesmenClubes.Add(new MemberSalesmanClubles { Role = "EXIT", Salesmen = sExits });


                foreach (var item in lstDeleteSalesmenClubes)
                {
                  //Obtenemos vendedores actuales de Intelligence Contracts                
                  var memberSalesmens = dbContextIC.USP_CL_GetMemberSalesmen(saleNew.saMembershipNum, "ALL", "ALL", item.Role).ToList();
                  if (memberSalesmens.Any())
                  {
                    //si el vendedor no esta en la lista de vendedores de Origos, lo eliminamos en Intellligence Contracts
                    memberSalesmens.ForEach(ms =>
                   {
                     if (!item.Salesmen.Contains(ms.OPC))
                     {
                       dbContextIC.USP_CL_DeleteMemberSalesman(ms.CLMEMOPC_ID);
                     }
                   });
                  }
                }

                dbContextIC.SaveChanges();
                transaction.Commit();
                return mensaje;
              }
              catch (System.Exception)
              {
                transaction.Rollback();
                throw;
              }
            }
          }
        }
      });
    } 
    #endregion
  }
}
