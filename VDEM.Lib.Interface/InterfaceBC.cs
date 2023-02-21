using System;
using System.Runtime.Serialization;

namespace VDEM.Lib.Interface
{

    [DataContract]
    public class InterfaceBC 
    {

        //Module / Message Name
        String _mName;
        [DataMember]
        public String mName
        {
            get { return _mName; }
            set { _mName = value; }
        }

        //Module / Message Version
        String _version;
        [DataMember]
        public String version
        {
            get { return _version; }
            set { _version = value; }
        }

        //Module / Message Preiority
        String _priority;
        [DataMember]
        public String priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        // Possible Values 0 = Sucesses, > 0 = Warning, < = Error. Look in to status & Return Message for Details
        Int32 _returnValue;
        [DataMember]
        public Int32 returnValue
        {
            get { return _returnValue; }
            set { _returnValue = value; }
        }

        //Message status - SentFromSrource, ReceivedFromDestination, ResponcePending, ResponceComplete, ResponcedWithError (Timeout, Underlaying Expection, etc..), DataInputError
        String _status;
        [DataMember]
        public String status
        {
            get { return _status; }
            set { _status = value; }
        }

        String _returnMessage;
        [DataMember]
        public String returnMessage
        {
            get { return _returnMessage; }
            set { _returnMessage = value; }
        }


        DateTime _createdDateTime;
        [DataMember]
        public DateTime createdDateTime
        {
            get { return _createdDateTime; }
            set { _createdDateTime = value; }
        }

        String _createdMachineName;
        [DataMember]
        public String createdMachineName
        {
            get { return _createdMachineName; }
            set { _createdMachineName = value; }
        }

        String _createdMachineIPAddress;
        [DataMember]
        public String createdMachineIPAddress
        {
            get { return _createdMachineIPAddress; }
            set { _createdMachineIPAddress = value; }
        }

        String _createdProcessName;
        [DataMember]
        public String createdProcessName
        {
            get { return _createdProcessName; }
            set { _createdProcessName = value; }
        }

        String _createdProcessVersion;
        [DataMember]
        public String createdProcessVersion
        {
            get { return _createdProcessVersion; }
            set { _createdProcessVersion = value; }
        }

        String _createdUserName;
        [DataMember]
        public String createdUserName
        {
            get { return _createdUserName; }
            set { _createdUserName = value; }
        }


        String _createdUserRole;
        [DataMember]
        public String createdUserRole
        {
            get { return _createdUserRole; }
            set { _createdUserRole = value; }
        }

        String _createdUserID;
        [DataMember]
        public String createdUserID
        {
            get { return _createdUserID; }
            set { _createdUserID = value; }
        }

        String _createdUserCredentials;
        [DataMember]
        public String createdUserCredentials
        {
            get { return _createdUserCredentials; }
            set { _createdUserCredentials = value; }
        }


        String _receivedDateTime;
        [DataMember]
        public String receivedDateTime
        {
            get { return _receivedDateTime; }
            set { _receivedDateTime = value; }
        }

        String _receivedMachineName;
        [DataMember]
        public String receivedMachineName
        {
            get { return _receivedMachineName; }
            set { _receivedMachineName = value; }
        }

        String _receivedMachineIPAddress;
        [DataMember]
        public String receivedMachineIPAddress
        {
            get { return _receivedMachineIPAddress; }
            set { _receivedMachineIPAddress = value; }
        }

        String _receivedProcessName;
        [DataMember]
        public String receivedProcessName
        {
            get { return _receivedProcessName; }
            set { _receivedProcessName = value; }
        }

        String _receivedProcessVersion;
        [DataMember]
        public String receivedProcess
        {
            get { return _receivedProcessVersion; }
            set { _receivedProcessVersion = value; }
        }

        String _databaseCon;
        [DataMember]
        public String databaseCon
        {
            get { return _databaseCon; }
            set { _databaseCon = value; }
        }


    }
}
