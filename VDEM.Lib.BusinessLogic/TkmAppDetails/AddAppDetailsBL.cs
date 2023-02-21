using Microsoft.AppCenter.Ingestion.Models;
using System;
using System.Collections.Generic;
using System.Text;
using VDEM.Lib.BusinessLogic.Helper;
using VDEM.Lib.Database;
using VDEM.Lib.Interface.TkmAppDetails;

namespace VDEM.Lib.BusinessLogic.TkmAppDetails
{
    public class AddAppDetailsBL : BusinessLogicBC
    {
        public AddAppDetailsBL(ref DBConnection dbco)
        {
            dbc = dbco;
        }

        public int AddAppDetails(ref AddAppDetailsIP ip, ref AddAppDetailsOP op)
        {

            if (ip.AddAppDetails.ID != 0)
            {
                op.returnMessage = "PK_ID ID is Not Equal To Zero, Primary Key ID = " + ip.AddAppDetails.ID.ToString();
                op.returnValue = -1;
                log.Error("Error: " + op.returnMessage);
                return -1;
            }



            List<TKM_APP_DETAILS> tmp_Name = new List<TKM_APP_DETAILS>();
            whereCondition = " Where APPLICATION_NAME = '" + ip.AddAppDetails.APPLICATION_NAME + "' ";
            rc = dbc.Select(ref errorMessage, whereCondition, ref tmp_Name);
            if (rc < 0)
            {
                op.returnMessage = errorMessage;
                op.returnValue = rc;
                return rc;
            }

            if (tmp_Name.Count > 0)
            {
                op.returnValue = -6;
                op.returnMessage = "Duplicate Line Name";
                return -1;
            }
            else
            {
                List<TKM_APP_DETAILS> tmpLine = new List<TKM_APP_DETAILS>();
                //ip.AddAppDetails.APPLIATION_CATEGORY = tmpLine[0].APPLIATION_CATEGORY;
                //ip.AddAppDetails.APPLICATION_NAME = tmpLine[0].APPLICATION_NAME;
                //ip.AddAppDetails.PRIMARY_SUPPORT_EMAIL = tmpLine[0].PRIMARY_SUPPORT_EMAIL;
                //ip.AddAppDetails.PRIMARY_SUPPORT_PHONE = tmpLine[0].PRIMARY_SUPPORT_PHONE;
                //ip.AddAppDetails.PRIMARY_SUPPORT_TEAMS = tmpLine[0].PRIMARY_SUPPORT_TEAMS;
                //ip.AddAppDetails.PRODUCTION_URL = tmpLine[0].PRODUCTION_URL;
                //ip.AddAppDetails.PURPOSE = tmpLine[0].PURPOSE;
                //ip.AddAppDetails.SECONDARY_SUPPORT_EMAIL = tmpLine[0].SECONDARY_SUPPORT_EMAIL;
                //ip.AddAppDetails.SECONDARY_SUPPORT_PHONE = tmpLine[0].SECONDARY_SUPPORT_PHONE;
                //ip.AddAppDetails.SECONDARY_SUPPORT_TEAMS = tmpLine[0].SECONDARY_SUPPORT_TEAMS;
                tmpLine.Add(ip.AddAppDetails);
                rc = dbc.Save(ref errorMessage, ref tmpLine);


                if (rc < 0)
                {
                    log.Error("Error: " + errorMessage);
                    op.returnMessage = errorMessage;
                    op.returnValue = rc;
                    return rc;
                }

                op.AddAppDetails = tmpLine[0];
            }
            return rc;

        }


    }
}
