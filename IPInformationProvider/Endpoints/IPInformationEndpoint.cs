using IPInformationProvider.API.Interfaces;
using IPInformationProvider.API.Models;
using IPInformationProvider.API.Static;

namespace IPInformationProvider.API.Endpoints
{
    public class IPInformationEndpoint : IIPInformationEndpoint
    {
        private readonly IIPCaching _cache;
        private readonly IIPRepository _repository;
        private readonly string _url;

        public IPInformationEndpoint(IIPCaching cache, IIPRepository repository, IConfiguration configuration)
        {
            _cache = cache;
            _repository = repository;
            _url = configuration.GetValue<string>("IP2CServiceUrl")!;
        }

        #region Cache
        public IP? GetFromCache(string iPAddress)
        {
            var result = _cache.GetFromCache(iPAddress);
            return result;
        }

        public void InsertOneOrManyToCache(params IP[] ips)
        {
            _cache.InsertOneOrManyToCache(ips);
        }
        #endregion

        #region Database
        public async Task<IP?> GetFromDatabaseAsync(string iPAddress)
        {
            var result = await _repository.GetAsync(iPAddress);
            return result;
        }

        public async Task<IP[]> GetPartialFromDatabaseAsync(int start, int batch)
        {
            return await _repository.GetPartialAsync(batch, batch += 100);
        }

        public async Task<bool> InsertToDatabaseAsync(IP iP)
        {
            return await _repository.InsertAsync(new List<IP>() { iP });
        }
        #endregion

        #region IP2C
        public async Task<IP> GetFromIP2CAsync(string iPAddress)
        {
            using (HttpClient client = new())
            {
                if (!ValidateIPAddress.ValidateIPv4(iPAddress))
                    throw new Exception("Invalid IP Address");

                return await GetContentFromIP2C(client, iPAddress);
            }
        }

        private async Task<IP> GetContentFromIP2C(HttpClient client, string iPAddress)
        {
            HttpResponseMessage response = await client.GetAsync(_url + iPAddress);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(result))
                {
                    var ipData = result.Trim().Split(';');
                    if (ipData.Length != 4) //1;GR;GRC;Greece
                    {
                        throw new Exception("Invalid Response from IP2C");
                    }

                    return new IP(
                                    iPAddress,
                                    ipData[1],
                                    ipData[2],
                                    ipData[3]);
                }
                throw new Exception("IP Address Not Found in IP2C");
            }
            else
            {
                throw new HttpRequestException($"Failed to get response from {_url}. StatusCode={response.StatusCode}");
            }
        }
        #endregion

    }
}
