using System;
using System.Net;

namespace S9.Utility
{
    public class Environment: IEnvironment
    {
        // time
        private static DateTime _now = default(DateTime);
        private static DateTime _UTCnow = default(DateTime);

        // path
        //private string _currPath;


        public Environment()
        { }


        public Environment(DateTime now)
        {
            _now = now;
        }
        
        public DateTime Now()
        {
            if (_now == default(DateTime))
                return DateTime.Now;
            else
                return _now;
        }

        public DateTime UTCNow()
        {
            if (_UTCnow == default(DateTime))
                return DateTime.UtcNow;
            else
                return _now.ToUniversalTime();
        }
    }
}