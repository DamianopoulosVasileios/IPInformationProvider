using IPInformationProvider.API.Interfaces;
using IPInformationProvider.API.Static;
using Microsoft.Extensions.Caching.Memory;

namespace IPInformationProvider.API.Caching
{
    public class IPCaching : IIPCaching
    {
        private static readonly object ThisLock = new();
        private readonly IMemoryCache _memoryCache;

        public IPCaching(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IIPs? GetFromCache(string iPAddress)
        {
            _memoryCache.TryGetValue(iPAddress, out IIPs? query);
            return query;
        }
        public void InsertOneOrManyToCache(IEnumerable<IIPs> iPs,bool invalidateCache = false)
        {
            foreach (var ip in iPs)
            {
                lock (ThisLock)
                {
                    if (!_memoryCache.TryGetValue(ip.CountryName, out IIPs? obj))
                    {
                        try
                        {
                            _memoryCache.Set(ip.CountryName, ip);
                        }
                        catch
                        {
                            throw new Exception("Error while adding to cache");
                        }
                    }
                    else if(invalidateCache==true && IPDetailsChangesCheck.CheckIPForChanges(obj, ip))
                    {
                        //Invalidates IP at cache if IP is found and dirty
                        _memoryCache.Remove(ip);
                    }
                }
            }
        }

    }
}
