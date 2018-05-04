using System;
using System.Net;

namespace S9.Utility
{
    public interface IEnvironment
    {
        DateTime Now();
        DateTime UTCNow();
    }
}