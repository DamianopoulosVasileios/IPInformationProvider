using IPInformationProvider.API.Models;

namespace IPInformationProvider.API.Interfaces
{
    public interface IIPInformationEndpoint
    {
        IP? GetFromCache(string iPAddress);
        void InsertOneOrManyToCache(params IP[] ips);

        Task<IP?> GetFromDatabaseAsync(string iPAddress);
        Task<IP[]> GetPartialFromDatabaseAsync(int start, int batch);
        Task<bool> InsertToDatabaseAsync(IP iP);

        Task<IP> GetFromIP2CAsync(string iPAddress);
    }
}
