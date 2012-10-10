using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using DRCOG.Common.DesignByContract;

namespace DRCOG.Domain.Caching
{
    public class CacheManager
    {
        private ICacheManager _userCacheManager;
        private ICacheManager _dataCacheManager;
        private ICacheManager _menuCacheManager;

        #region Thread-safe, lazy Singleton
        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static CacheManager Instance
        {
            get
            {
                return Nested.CacheManager;
            }
        }

        /// <summary>
        /// Initializes the Container on instantiation.
        /// </summary>
        private CacheManager()
        {
            InitManger();
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            static Nested() { }
            internal static readonly CacheManager CacheManager =
                new CacheManager();
        }

        #endregion

        /// <summary>
        /// Initializes the SiteContainer
        /// </summary>
        private void InitManger()
        {
            System.Diagnostics.Debug.WriteLine("Initializing Cache Manager");

            _userCacheManager = CacheFactory.GetCacheManager("User Cache Manager");
            Check.Ensure(UserCache != null, "UserCache cannot be null");

            //_dataCacheManager = CacheFactory.GetCacheManager("Data Cache Manager");
            //Check.Ensure(_dataCacheManager != null, "DataCache cannot be null");

            //_menuCacheManager = CacheFactory.GetCacheManager("Menu Cache Manager");
            //Check.Ensure(_dataCacheManager != null, "Menu cannot be null");

            System.Diagnostics.Debug.WriteLine("Cache Manager Initialized");
        }

        public ICacheManager MenuCache
        {
            get { return _menuCacheManager; }
        }

        public ICacheManager UserCache
        {
            get { return _userCacheManager; }
        }

        public ICacheManager DataCache
        {
            get { return _dataCacheManager; }
        }
    }
}
