using IPInformationProvider.API.Models;

namespace IPInformationProvider.API.Interfaces
{
    public interface IIPRepository
    {
        Task<IEnumerable<IP>> GetAllAsync();
        Task<IP[]> GetPartialAsync(int start, int batch);
        Task<IP?> GetAsync(string id);
        Task<bool> InsertAsync(IEnumerable<IP> iPAddress);
        Task<bool> UpdateAsync(IEnumerable<IP> iPAddress);
    }
}
