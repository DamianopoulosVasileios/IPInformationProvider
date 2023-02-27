namespace IPInformationProvider.API.Interfaces
{
    public interface IIPCaching
    {
        public IIPs? GetFromCache(string id);
        public void InsertOneOrManyToCache(IEnumerable<IIPs> iPs, bool invalidateCache = false);
    }
}
