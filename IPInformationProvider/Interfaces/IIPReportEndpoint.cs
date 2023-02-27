namespace IPInformationProvider.API.Interfaces
{
    public interface IIPReportEndpoint
    {
        Task<IEnumerable<IIPResponse>> GetReport(string[]? twoLetterCountryCodes = null);
    }
}
