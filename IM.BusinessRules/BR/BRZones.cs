using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
    class BRZones
    {
        #region GetZonesTransfer

        /// <summary>
        /// Obtiene las zonas de transferencia
        /// </summary>
        /// <returns></returns>
        /// <history>
        ///   [michan] 24/02/2016 Created
        /// </history>
        /// 
        public static List<ZoneTransfer> GetZonesTransfer()
        {
            using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
            {
                return dbContext.USP_OR_GetZonesTransfer().ToList();
            }
        }

        #endregion

        #region TransferStopZone

        /// <summary>
        /// Indica que la transferencia de reservaciones de una zona ha terminado
        /// </summary>
        /// <param name="zone">Id de la zona</param>
        /// <history>
        ///   [michan] 24/02/2016 Created
        /// </history>
        /// 
        public static void TransferStopZone(string zone)
        {
            using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
            {
                dbContext.USP_OR_TransferStopZone(zone);
            }
        }

        #endregion
    }
}
