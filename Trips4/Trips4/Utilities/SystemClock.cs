using System;
using DRCOG.Interfaces;

namespace DRCOG.Web
{
    public class SystemClock : IClock
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

    }
}
