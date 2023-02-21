using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using VDEM.Lib.Database;

namespace VDEM.Lib.Interface.TkmAppDetails
{
    public class AddAppDetailsIP:InputDto
    {
        [DataMember]
        public TKM_APP_DETAILS AddAppDetails { get; set; }

        public AddAppDetailsIP()
        {
            AddAppDetails = new TKM_APP_DETAILS();
        }
    }
}
