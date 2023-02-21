using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using VDEM.Lib.Database;

namespace VDEM.Lib.Interface.TkmAppDetails
{
    public class AddAppDetailsOP:OutputDto
    {
        [DataMember]
        public TKM_APP_DETAILS AddAppDetails { get; set; }

        public AddAppDetailsOP()
        {
            AddAppDetails = new TKM_APP_DETAILS();
        }
    }   
}
