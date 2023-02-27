namespace IPInformationProvider.API.Interfaces
{
    public interface IIPService
    {
        Task<IIPs> GetInformation(string iPAddress);
        Task<bool> UpdateIPInformation();
        Task<IEnumerable<IIPResponse>> GetReport(string[]? CountryCodes = null);
    }
}
