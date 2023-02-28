using IPInformationProvider.API.Models;

namespace IPInformationProvider.API.Interfaces
{
    public interface IIPService
    {
        Task<IP> GetInformation(string iPAddress);
        Task<bool> UpdateIPInformation();
        Task<IEnumerable<IPResponse>> GetReport(string[]? CountryCodes = null);
    }
}
