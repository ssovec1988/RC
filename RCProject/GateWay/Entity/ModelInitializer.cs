using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GateWay.Entity
{
    public class ModelInitializer
    {
        public IRCDICTIONARYEntities Dictionary { get; set; }
        public IRCFIZIKIEntities Fiziki { get; set; }
        public IRCRUSSIAEntities Russia { get; set; }
        public EnergoBillingEntities Billing { get; set; }
        public LogEntities Log { get; set; }

        public ModelInitializer()
        {
            Dictionary = new IRCDICTIONARYEntities();
            Fiziki = new IRCFIZIKIEntities();
            Russia = new IRCRUSSIAEntities();
            Billing = new EnergoBillingEntities();
            Log = new LogEntities();
        }
       
    }
}