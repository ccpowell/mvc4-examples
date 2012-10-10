using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DRCOG.Domain.Utility
{
    public static class RetryUtility
    {
        public static void RetryAction(Action action, int numRetries, int retryTimeout)
        {
            if (action == null)
                throw new ArgumentNullException("action"); // slightly safer...

            do
            {
                try { action(); return; }
                catch
                {
                    if (numRetries <= 0) throw;  // improved to avoid silent failure
                    else Thread.Sleep(retryTimeout);
                }
            } while (numRetries-- > 0);
        }
    }
}
