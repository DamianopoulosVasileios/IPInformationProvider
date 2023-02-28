using IPInformationProvider.API.Models;

namespace IPInformationProvider.API.Interfaces
{
    public interface IIPReportEndpoint
    {
        Task<IEnumerable<IPResponse>> GetReport(string[]? twoLetterCountryCodes = null);
    }
}
