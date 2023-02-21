using System;
using System.Collections.Generic;
using System.Text;
using VDEM.Lib.BusinessLogic.Helper;
using VDEM.Lib.Database;
using VDEM.Lib.Interface;
using VDEM.Lib.Interface.TkmAppDetails;

namespace VDEM.Lib.BusinessLogic.TkmAppDetails
{
    public class AddAppDetailsMainBL
    {
        private int rc;
        private DBConnection dbc;

        public AddAppDetailsMainBL()
        {

        }
        public int AddAppDetails(ref AddAppDetailsIP ip, ref AddAppDetailsOP op)
        {
            InputDto _ip = ip;
            OutputDto _op = op;

            if (DBConnectionManagementBL.RequestConnectionObject(ref dbc, ref _ip, ref _op) < 0)
            {
                return -1;
            }
            try
            {
                AddAppDetailsBL bl = new AddAppDetailsBL(ref dbc);
                rc = bl.AddAppDetails(ref ip, ref op);
                
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
