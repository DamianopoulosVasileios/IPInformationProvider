using IPInformationProvider.API.Interfaces;
using IPInformationProvider.API.Models;
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

        public IP? GetFromCache(string iPAddress)
        {
            _memoryCache.TryGetValue(iPAddress, out IP? query);
            return query;
        }

        public void InsertOneOrManyToCache(params IP[] iPs)
        {
            foreach (var ip in iPs)
            {
                lock (ThisLock)
                {
                    if (!_memoryCache.TryGetValue(ip.IPAddress, out IP? original))
                    {
                        try
                        {
                            _memoryCache.Set(ip.IPAddress, ip);
                        }
                        catch
                        {
                            throw new Exception("Error while adding to cache");
                        }
                    }
                    else if (IPDetailsChangesCheck.CheckIPForChanges(original, ip))
                    {
                        _memoryCache.Remove(ip);
                    }
                }
            }
        }
    }
}
