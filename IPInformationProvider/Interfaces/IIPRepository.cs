namespace IPInformationProvider.API.Interfaces
{
    public interface IIPRepository
    {
        Task<IEnumerable<IIPs>> GetAllAsync();
        Task<IEnumerable<IIPs>> GetPartialAsync(int start, int batch);
        Task<IIPs?> GetAsync(string id); 
        Task<bool> InsertAsync(IEnumerable<IIPs> iPAddress);
        Task<bool> UpdateAsync(IEnumerable<IIPs> iPAddress);
    }
}
