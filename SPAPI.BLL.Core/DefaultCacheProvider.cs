using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace SPAPI.BLL.Core
{
    public class DefaultCacheProvider : ICacheProvider
    {
        private static ObjectCache cache = new MemoryCache("ApiTokenCache");
        private CacheItemPolicy policy = null;

        public DefaultCacheProvider(int interval)
        {
            policy = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 0, interval) };
        }

        public void Clear(string key)
        {
            if (cache.Contains(key))
                cache.Remove(key);
        }

        public object Get(string key)
        {
            if (cache.Contains(key))
                return cache.Get(key);
            return null;
        }

        public void Set(string key, object value)
        {
            cache.Set(key, value, policy);
        }

        public void ClearAllCache()
        {
            foreach (KeyValuePair<string, object> element in cache)
            {
                cache.Remove(element.Key);
            }
        }

    }

    public interface ICacheProvider
    {
        object Get(string key);
        void Set(string key, object value);
        void Clear(string key);
        void ClearAllCache();
    }
}
