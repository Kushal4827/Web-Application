using System.Collections.Generic;
using System.Runtime.Serialization;
using VDEM.Lib.Database;

namespace VDEM.Lib.Interface.TkmAppDetails
{
    [DataContract]
    public class GetAppDetailsOP : OutputDto    
    {
        [DataMember]
        public List<TKM_APP_DETAILS> appDetails { get; set; }

        [DataMember]
        public List<string> systemCategory { get; set; }

        [DataMember]
        public List<string> localsystems { get; set; }
        [DataMember]
        public List<string> regionalsystems { get; set; }
        [DataMember]
        public List<string> globalsystems { get; set; }
        [DataMember]
        public List<string> engineeringsystems { get; set; }

        public GetAppDetailsOP()
        {
            appDetails = new List<TKM_APP_DETAILS>();
            systemCategory = new List<string>();
            localsystems = new List<string>();
            regionalsystems = new List<string>();
            globalsystems = new List<string>();
            engineeringsystems = new List<string>();
        }
    }
}
