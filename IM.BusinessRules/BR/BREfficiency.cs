using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BREfficiency
  {
    #region GetEfficiencyByWeeks
    /// <summary>
    /// Obtiene una lista de Efficiency 
    /// </summary>
    /// <param name="sr">Sales Room</param>
    /// <param name="dateFrom">Fecha de inicio </param>
    /// <param name="dateTo">Fecha final</param>
    /// <history>
    /// [ecanul] 30/07/2016 Created
    /// [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<EfficiencyData>> GetEfficiencyByWeeks(string sr, DateTime dateFrom, DateTime dateTo)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_IM_GetEfficiencyByWeeks(sr, dateFrom, dateTo).ToList();
        }
      });
    }
    #endregion

    #region SaveEfficiencies
    /// <summary>
    /// Guarda los datos del reporte de Eficiecias semanales
    /// </summary>
    /// <param name="list">Listado con las eficiencias de una semana</param>
    /// <param name="salesRoom">Sala de ventas</param>
    /// <param name="dtStart">Fecha inicial</param>
    /// <param name="dtEnd">Fecha Fin</param>
    /// <history>
    /// [ecanul] 18/08/2016 Created
    /// </history>
    /// <returns></returns>
    public async static Task<int> SaveEfficiencies(List<RptEfficiencyWeekly> list, string salesRoom, DateTime dtStart, DateTime dtEnd)
    {
      return await Task.Run(async () =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transact = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              //Lista para agregar las eficiencias nuevas
              var lstEffAdd = new List<Efficiency>();

              //Obtener las eficiencias existentes dentro del rango de fechas, sala de ventas y periodo seleccionado
              var lstEfficiency = dbContext.Efficiencies.Include(Ey => Ey.Personnels).Where(e => e.efsr == salesRoom && e.efDateFrom == dtStart && e.efDateTo == dtEnd && e.efpd == "W").ToList();
              //Obtiene todos los tipos de eficiencias
              var lstEfftypes = await BREfficiencyTypes.GetEfficiencyTypes();
              //Eficiencia vacia 
              Efficiency eff = null;
              //por cada tipo de Eficiencia 
              lstEfftypes.ForEach(eft =>
              {
                //Se busca en el listado todas las eficiencias que concuerden con el tipo de ficicicias actual
                eff = lstEfficiency.Where(ef => ef.efet == eft.etID).FirstOrDefault();
                //Si no se encontro la eficiencia se crea una nueva
                if (eff == null)
                {
                  eff = new Efficiency
                  {
                    efsr = salesRoom,
                    efDateFrom = dtStart,
                    efDateTo = dtEnd,
                    efet = eft.etID,
                    efpd = "W"
                  };
                  dbContext.Entry(eff).State = EntityState.Added;
                  var respuesta = dbContext.SaveChanges();
                }
                //Se guardan los vendedores del tipo de eficiencia
                //Se eliminan los vendedores que tengan la eficiencia actual
                //SP DE ELIMINAR USP_IM_DeleteEfficiencySalesmen
                dbContext.USP_IM_DeleteEfficiencySalesmen(Convert.ToInt32(eff.efID));
                //por cada elemento de la lista del reporte y que tengan un id
                list.Where(add => add.EfficiencyType == eft.etN).ToList().ForEach(a =>
                {
                  if (!string.IsNullOrEmpty(a.SalemanID))
                  {
                    //Salvar EfficiencySalesman USP_IM_ADDEfficiencySalesmen
                    dbContext.USP_IM_ADDEfficiencySalesmen((int)eff.efID, a.SalemanID);
                    //dbContext.Database.ExecuteSqlCommand($"Insert into efficiencysalesman vaues({eff.efID}, {a.SalemanID})");
                  }
                });
              });
              transact.Commit();
              return 1;
            }
            catch
            {
              transact.Rollback();
              throw;
            }
          }
        }
      });
    } 
    #endregion
  }
}