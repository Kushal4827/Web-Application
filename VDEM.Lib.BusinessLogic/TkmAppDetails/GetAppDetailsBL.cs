using VDEM.Lib.Database;
using VDEM.Lib.BusinessLogic.Helper;
using VDEM.Lib.Interface.TkmAppDetails;
using System.Collections.Generic;
using System.Linq;

namespace VDEM.Lib.BusinessLogic.TkmAppDetails
{
    public class GetAppDetailsBL : BusinessLogicBC
    {
        public GetAppDetailsBL(ref DBConnection dbco)
        {
            dbc = dbco;
        }

        /// <summary>
        /// Business logic to fetch detail of specific zone.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="op"></param>
        /// <returns>Object containing zone data</returns>
        public int GetAppDetails(ref GetAppDetailsIP ip, ref GetAppDetailsOP op)
        {
            
            List<TKM_APP_DETAILS> tmpApp = new List<TKM_APP_DETAILS>();
            whereCondition = " Where ID > 0";
            
            rc = dbc.Select(ref errorMessage, whereCondition, ref tmpApp);
            if (rc < 0)
            {
                log.Error("Error: " + errorMessage);
                op.returnMessage = errorMessage;
                op.returnValue = rc;
                return rc;
            }
            op.systemCategory = tmpApp.Select(x => x.APPLIATION_CATEGORY).Distinct().ToList();
            op.localsystems = tmpApp.Where(x => x.APPLIATION_CATEGORY == "Local").Select(x => x.APPLICATION_NAME).ToList();
            op.regionalsystems = tmpApp.Where(x => x.APPLIATION_CATEGORY == "Regional").Select(x => x.APPLICATION_NAME).ToList();
            op.globalsystems = tmpApp.Where(x => x.APPLIATION_CATEGORY == "Global").Select(x => x.APPLICATION_NAME).ToList();
            op.engineeringsystems = tmpApp.Where(x => x.APPLIATION_CATEGORY == "Engineering system").Select(x => x.APPLICATION_NAME).ToList();
            op.appDetails = tmpApp;

            return rc;
        }
    }
}
