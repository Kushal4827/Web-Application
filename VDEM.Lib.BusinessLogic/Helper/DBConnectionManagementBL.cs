using System;
using log4net;
using VDEM.Lib.Database;
using VDEM.Lib.Interface;

namespace VDEM.Lib.BusinessLogic.Helper
{
    public class DBConnectionManagementBL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DBConnectionManagementBL));
        public static String lastErrorMessage = "";

        public static int RequestConnectionObject(ref DBConnection dbc, ref InputDto ipdc, ref OutputDto opdc)
        {
            DBConnectionManagementBL.lastErrorMessage = "";

            String DBConnectionString = ipdc.databaseCon;

            dbc = new DBConnection(DBConnectionString);
            dbc.OpenConnection(ref DBConnectionManagementBL.lastErrorMessage);
            return 0;
        }

        public static int FreeConnectionObject(ref DBConnection dbc, ref InputDto ipdc, ref OutputDto opdc)
        {
            dbc.CloseConnection(ref DBConnectionManagementBL.lastErrorMessage);
            dbc.busy = false;
            return 0;
        }
    }
}
