using IPInformationProvider.API.Interfaces;
using IPInformationProvider.API.Models;

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

        public async Task<IP> GetInformation(string iPAddress)
        {
            var resultCache = _information.GetFromCache(iPAddress);
            if (resultCache != null)
                return resultCache;

            var resultDb = await _information.GetFromDatabaseAsync(iPAddress);
            if (resultDb != null)
                return resultDb;

            var resultIP2C = await _information.GetFromIP2CAsync(iPAddress);

            await _information.InsertToDatabaseAsync(resultIP2C);
            _information.InsertOneOrManyToCache(resultIP2C);

            return resultIP2C;
        }

        public async Task<bool> UpdateIPInformation()
        {
            int batch = 0;
            bool hasMoreIPs = true;

            do
            {
                var ips = await _information.GetPartialFromDatabaseAsync(batch, batch += 100);

                if (ips != null && ips.Any())
                {
                    var results = new List<IP>(ips.Length);

                    foreach (var result in ips)
                        results.Add(await _information.GetFromIP2CAsync(result.IPAddress));

                    _information.InsertOneOrManyToCache(results.ToArray());
                }
                else
                {
                    hasMoreIPs = false;
                }
            }
            while (hasMoreIPs);

            return true;
        }

        public async Task<IEnumerable<IPResponse>> GetReport(string[]? twoLetterCountryCodes = null)
        {
            var result = await _report.GetReport(twoLetterCountryCodes);
            //print results ...
            return result;
        }
    }
}
