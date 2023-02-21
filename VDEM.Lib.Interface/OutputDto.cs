using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace VDEM.Lib.Interface
{

    [DataContract]
    public class OutputDto
    {

        // Possible Values 0 = Sucesses, > 0 = Warning, < = Error. Look in to status & Return Message for Details
        Int32 _returnValue;
        [DataMember]
        public Int32 returnValue
        {
            get { return _returnValue; }
            set { _returnValue = value; }
        }

        String _returnMessage;
        [DataMember]
        public String returnMessage
        {
            get { return _returnMessage; }
            set { _returnMessage = value; }
        }

    }
}
