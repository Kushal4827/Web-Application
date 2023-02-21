using System;
using System.Runtime.Serialization;

namespace VDEM.Lib.Interface
{

    [DataContract]
    public class InputDto
    {

        DateTime _createdDateTime;
        [DataMember]
        public DateTime createdDateTime
        {
            get { return _createdDateTime; }
            set { _createdDateTime = value; }
        }

        String _createdUserName;
        [DataMember]
        public String createdUserName
        {
            get { return _createdUserName; }
            set { _createdUserName = value; }
        }

        String _createdUserID;
        [DataMember]
        public String createdUserID
        {
            get { return _createdUserID; }
            set { _createdUserID = value; }
        }

        String _databaseCon;
        [DataMember]
        public String databaseCon
        {
            get { return _databaseCon; }
            set { _databaseCon = value; }
        }
        String _source;
        [DataMember]
        public String source
        {
            get { return _source; }
            set { _source = value; }
        }
        String _ipAddress;
        [DataMember]
        public String ipAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        String _userId;
        [DataMember]
        public String userId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        //Only for mobile
        String _deviceId;
        [DataMember]
        public String deviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }
        String _processCode;
        [DataMember]
        public String processCode
        {
            get { return _processCode; }
            set { _processCode = value; }
        }
    }
}
