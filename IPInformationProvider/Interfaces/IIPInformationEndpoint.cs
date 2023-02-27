namespace IPInformationProvider.API.Interfaces
{
    public interface IIPInformationEndpoint
    {
        IIPs? GetFromCache(string iPAddress);
        void InsertOneOrManyToCache(IEnumerable<IIPs> iP, bool invalidateCache = false);

        Task<IIPs?> GetFromDatabaseAsync(string iPAddress);
        Task<IEnumerable<IIPs>> GetPartialFromDatabaseAsync(int start, int batch);
        Task<bool> InsertToDatabaseAsync(IIPs iP);

        Task<IIPs> GetFromIP2CAsync(string iPAddress);
    }
}
