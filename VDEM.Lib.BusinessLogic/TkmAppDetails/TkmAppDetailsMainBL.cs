using System;
using VDEM.Lib.Database;
using VDEM.Lib.Interface;
using VDEM.Lib.BusinessLogic.Helper;
using VDEM.Lib.BusinessLogic.TkmAppDetails;
using VDEM.Lib.Interface.TkmAppDetails;

namespace VDEM.Lib.BusinessLogic.TkmAppDetails
{
    /// <summary>
    /// Common class declared for all business logics related to Zone Master
    /// </summary>
    public class TkmAppDetailsMainBL
    {
        private int rc;
        private DBConnection dbc;

        public TkmAppDetailsMainBL()
        {
            
        }

        /// <summary>
        /// Calling GetZoneMaster method 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="op"></param>
        /// <returns>Object containing Zone & company master data</returns>
        public int GetAppDetails(ref GetAppDetailsIP ip, ref GetAppDetailsOP op)
        {
            InputDto _ip = ip;
            OutputDto _op = op;

            if (DBConnectionManagementBL.RequestConnectionObject(ref dbc, ref _ip, ref _op) < 0)
            {
                return -1;
            }
            try
            {
                GetAppDetailsBL bl = new GetAppDetailsBL(ref dbc);
                rc = bl.GetAppDetails(ref ip, ref op);
            }
            catch (Exception ex)
            {
                op.returnMessage = ex.Message;
                op.returnValue = rc;
            }

            DBConnectionManagementBL.FreeConnectionObject(ref dbc, ref _ip, ref _op);
            return rc;
        }

    }
}
