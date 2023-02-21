using System;
using System.Collections.Generic;
using log4net;
using VDEM.Lib.Database;

namespace VDEM.Lib.BusinessLogic.Helper
{
    public class BusinessLogicBC
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(BusinessLogicBC));

        public int rc = 0;
        public String errorMessage = "";
        public String whereCondition = "";
        public DBConnection dbc = null;

        public BusinessLogicBC()
        {

        }

        public void GetCreateDateTime<T>(String id, ref T tbl, ref DateTime crdt, ref long crid)
        {
            if (id == null)
            {
                return;
            }

            whereCondition = " WHERE ID = " + id;
            List<T> tmpList = new List<T>();
            rc = dbc.Select(ref errorMessage, whereCondition, ref tmpList);
            if (tmpList.Count > 0)
            {
                var pdt = tmpList[0].GetType().GetProperty("CreatedDateTime");
                crdt = (DateTime)pdt.GetValue(tmpList[0]);

                var pID = tmpList[0].GetType().GetProperty("CreatedStaffID");
                crid = (long)pID.GetValue(tmpList[0]);
            }

            whereCondition = "";
        }

        public void SetDurationDateTime(String Duration, ref DateTime dtStart, ref DateTime dtEnd)
        {
            switch (Duration)
            {
                case "Current Day":
                    dtStart = DateTime.Now;
                    dtEnd = DateTime.Now;
                    break;
                case "Current Week":
                    dtEnd = DateTime.Now;
                    int dow = (int)dtEnd.DayOfWeek;
                    dtStart = dtEnd.Subtract(new TimeSpan(dow, 0, 0, 0));
                    break;
                case "Current Month":
                    dtEnd = DateTime.Now;
                    dtStart = new DateTime(dtEnd.Year, dtEnd.Month, 1);
                    break;
                case "Current Quarter":
                    dtEnd = DateTime.Now;
                    dtStart = dtEnd.Subtract(new TimeSpan(93, 0, 0, 0));
                    break;
                case "Current Year":
                    dtEnd = DateTime.Now;
                    dtStart = new DateTime(dtEnd.Year, 1, 1);
                    break;
                case "All":
                    dtEnd = DateTime.Now;
                    dtStart = new DateTime(1900, 1, 1);
                    break;
                default:
                    break;
            }
        }

        public String GetWhereContionOnDateTime(String dtColumnName, DateTime dtStart, DateTime dtEnd)
        {
            String strStartDate = "'" + dtStart.ToString("yyyy-MM-dd") + " 00:00:00'";
            String strEndDate = "'" + dtEnd.ToString("yyyy-MM-dd") + " 23:59:59'";
            String strWhereCondition = " WHERE " + dtColumnName + " >= " + strStartDate + " and " + dtColumnName + " <= " + strEndDate;
            return strWhereCondition;
        }

        public void GetStartEndSQLDate(String strOption, ref String strStart, ref String strEnd)
        {
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;

            switch (strOption)
            {
                case "DAY":
                    dtStart = DateTime.Now;
                    dtEnd = DateTime.Now;
                    break;
                case "WEEK":
                    dtEnd = DateTime.Now;
                    int dow = (int)dtEnd.DayOfWeek;
                    dtStart = dtEnd.Subtract(new TimeSpan(dow, 0, 0, 0));
                    break;
                case "MONTH":
                    dtEnd = DateTime.Now;
                    dtStart = new DateTime(dtEnd.Year, dtEnd.Month, 1);
                    break;
                case "QUARTER":
                    dtEnd = DateTime.Now;
                    dtStart = dtEnd.Subtract(new TimeSpan(93, 0, 0, 0));
                    break;
                case "YEAR":
                    dtEnd = DateTime.Now;
                    dtStart = new DateTime(dtEnd.Year, 1, 1);
                    break;
                case "ALL":
                    dtEnd = DateTime.Now;
                    dtStart = new DateTime(2000, 1, 1);
                    break;
                default:
                    break;
            }

            strStart = "'" + dtStart.ToString("yyyy-MM-dd") + " 00:00:00'";
            strEnd = "'" + dtEnd.ToString("yyyy-MM-dd") + " 23:59:59'";
        }

        public string Serialize(Object obj)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return jsonString;
        }

    }
}
