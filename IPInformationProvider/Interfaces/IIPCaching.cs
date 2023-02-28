using IPInformationProvider.API.Models;

namespace IPInformationProvider.API.Interfaces
{
    public interface IIPCaching
    {
        public IP? GetFromCache(string id);
        public void InsertOneOrManyToCache(params IP[] ips);
    }
}
