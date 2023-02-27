using IPInformationProvider.API.Interfaces;

namespace IPInformationProvider.Services
{
    public class IPService : IIPService
    {
        private readonly IIPInformationEndpoint _information;
        private readonly IIPReportEndpoint _report;

        public IPService(IIPInformationEndpoint information, IIPReportEndpoint report)
        {
            _information = information;
            _report = report;
        }
        public async Task<IIPs> GetInformation(string iPAddress)
        {
            var resultCache = _information.GetFromCache(iPAddress);
            if (resultCache != null)
                return resultCache;

            var resultDb = await _information.GetFromDatabaseAsync(iPAddress);
            if (resultDb != null)
                return resultDb;

            var resultIP2C = await _information.GetFromIP2CAsync(iPAddress);

            await _information.InsertToDatabaseAsync(resultIP2C);
            _information.InsertOneOrManyToCache(new List<IIPs>(){ resultIP2C });

            return resultIP2C;
        }
        public async Task<bool> UpdateIPInformation()
        {
            IEnumerable<IIPs> iPsResults;
            int batch = 0;
            do
            {
                iPsResults = await _information.GetPartialFromDatabaseAsync(batch, batch += 100);
                if (iPsResults != Enumerable.Empty<IIPs>())
                {
                    var results = new List<IIPs>();
                    foreach (var result in iPsResults)
                    {
                        results.Add(await _information.GetFromIP2CAsync(result.IPAddress));
                    }
                    _information.InsertOneOrManyToCache(results,invalidateCache: true);
                }
            }
            while (iPsResults != Enumerable.Empty<IIPs>());
            return true;
        }
        public async Task<IEnumerable<IIPResponse>> GetReport(string[]? twoLetterCountryCodes = null)
        {
            var result = await _report.GetReport(twoLetterCountryCodes);
            //print results ...
            return result;
        }
    }
}
